using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using App.BLL.Cache;
using App.BLL.EventArgs;
using App.BLL.ExtensionMethods;
using App.BLL.ReportEnums.FleetSize;
using App.DAL.MarsDataAccess.Forecasting;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.ParameterAccess.DataHolders;
using App.Entities.Graphing;
using Mars.App.Classes.BLL.ExtensionMethods;
using Resources;

namespace App.Reports.Forecasting
{
    public partial class Benchmark : System.Web.UI.Page
    {
        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session["BenchmarkGraphInformation"];
            }
            set { Session["BenchmarkGraphInformation"] = value; }
        }

        private List<GraphSeries> SavedConstrainedSeriesData
        {
            get
            {
                return (List<GraphSeries>) Session["BenchmarkSavedConstrainedData"];
            }
            set { Session["BenchmarkSavedConstrainedData"] = value; }
        }
        private List<GraphSeries> SavedUnconstrainedSeriesData
        {
            get
            {
                return (List<GraphSeries>)Session["BenchmarkSavedUnconstrainedData"];
            }
            set { Session["BenchmarkSavedUnconstrainedData"] = value; }
        }

        private List<LocationGroupHolder> _locationGroupData;

        protected void Page_Init(object sender, EventArgs e)
        {
            _locationGroupData = ParameterCache.GetAllLocationGroups();

            ccBenchmark.GraphTypesAllowed = new List<SeriesChartType>
                                        {
                                            SeriesChartType.Line,
                                            SeriesChartType.StepLine,
                                            SeriesChartType.Spline,
                                        };

            if (GraphInformation == null)
            {
                GraphInformation = new GraphData
                {
                    IsXValueDate = true,
                    XAxisDateFormat = "dd/MM (ddd)",
                    YAxisNumberFormat = "#,##0",
                    DataPointsXAxisTooltip = "#VALX (day #INDEX)",
                    DataPointsYAxisTooltip = "#VALY{0,0}",
                    LabelFormat = "#,##0",
                    ChartDescription = LocalizedReportPages.BenchmarkReportDescription,
                    ChartName = LocalizedReportPages.BenchmarkReportName,
                    YAxisZoom = "75",
                    ChartLineWidth = 1,
                    ShowGraphLinkingButton = true,
                    GraphLinkingText = LocalizedReportPages.BenchmarkShowUnconstrained,
                    GraphLinkingPage = "",
                    ReportParameters = this.GetDefaultParameters()
                };
            }

            GeneralParams.ReportTypeControl.ShowForecastTypeParameter = true;

            GeneralParams.ParamsHolder = GraphInformation.ReportParameters;
            GeneralParams.SelectedParameters = GraphInformation.SelectedParameters;

            GeneralParams.ReportTypeControl.HideAlreadyBooked = true;

            ccBenchmark.GraphInformation = GraphInformation;
            GeneralParams.ExportType = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += PageLoadComplete;



        }

        protected void PageLoadComplete(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GeneralParams.DynamicReportParametersControl.QuickSelectedValue))
            {
                this.CheckQuickLocationGroupSelected(_locationGroupData, GeneralParams.DynamicReportParametersControl.QuickSelectedValue, GeneralParams.SelectedParameters, GraphInformation.ReportParameters);
                upnlGeneralParameters.Update();
            }

            GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(true);

            if (GraphInformation.ReportParameters.HasAnyParameterChanged())
            {
                GraphInformation.HaveDynamicParametersChanged = true;
            }

            GraphInformation.HaveDatesChanged = this.HaveDatesChanged(GeneralParams);

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            GraphInformation.HasReportTypeChanged = GeneralParams.ReportTypeControl.HasReportTypeChanged;


            if (GraphInformation.RefreshData || GraphInformation.UsingCachedGraphingData)
            {
                
                FutureTrendDataType futureTrendDataType;
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);

                var constrained = futureTrendDataType == FutureTrendDataType.Constrained ? true : false;

                var refreshDataFromDatabase = false;
                
                if (constrained && SavedConstrainedSeriesData == null)
                    refreshDataFromDatabase = true;

                if (!constrained && SavedUnconstrainedSeriesData == null)
                    refreshDataFromDatabase = true;

                if (GraphInformation.HaveDatesChanged || GraphInformation.HaveDynamicParametersChanged)
                {
                    refreshDataFromDatabase = true;
                    SavedConstrainedSeriesData = null;
                    SavedUnconstrainedSeriesData = null;
                }

                if (refreshDataFromDatabase || GraphInformation.RefreshData)
                {
                    var seriesData = BenchMarkDataAccess.GetBenchMarkGraphingData(GeneralParams.SelectedParameters, constrained);

                    if (constrained)
                    {
                        SavedConstrainedSeriesData = seriesData;
                    }
                    else
                    {
                        SavedUnconstrainedSeriesData = seriesData;
                    }

                    GraphInformation.SeriesData = seriesData;
                    GraphInformation.DataDate = ParameterDataAccess.GetLastDateFromCmsForecast();
                    GraphInformation.UsingCachedGraphingData = false;
                }
                else
                {
                    GraphInformation.UsingCachedGraphingData = true;
                    GraphInformation.SeriesData = constrained
                                  ? SavedConstrainedSeriesData
                                  : SavedUnconstrainedSeriesData;
                }

                GraphInformation.TitleAdditional = string.Format("{0}", 
                    GeneralParams.ReportTypeControl.SelectedForecastType.SelectedItem.Text);

                GraphInformation.CalculateYEntriesCount();
                GraphInformation.RefreshData = false;

                this.SetPreviousDates(GeneralParams);

                GraphInformation.HaveDynamicParametersChanged = false;
            }
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is RefreshGraphEventArgs)
            {
                UpdateChartLinkingButton();
                GraphInformation.RefreshData = true;
                handled = true;
            }

            if (args is ExportToExcelEventArgs)
            {
                GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(true);
                
                var siteGroup = int.Parse(GeneralParams.ExcelExportControl.SelectedGroupBySite);
                var fleetGroup = int.Parse(GeneralParams.ExcelExportControl.SelectedGroupByFleet);
                FutureTrendDataType futureTrendDataType;
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);

                var csvData = new StringBuilder();

                var constrained = futureTrendDataType == FutureTrendDataType.Constrained;
                csvData.Append(BenchMarkDataAccess.GetBenchmarkCsvData(GeneralParams.SelectedParameters, constrained, siteGroup, fleetGroup));

                Session["ExportData"] = csvData.ToString();
                Session["ExportFileName"] = "BenchmarkExport";


                handled = true;
            }
            if (args is GraphLinkingEventArgs)
            {
                BenchmarkUpdateGraphLinkingText();
                upnlGeneralParameters.Update();
                GraphInformation.UsingCachedGraphingData = true;
            }

            return handled;
        }

        private void UpdateChartLinkingButton()
        {
            FutureTrendDataType futureTrendDataType;
            Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);
            
            if(futureTrendDataType == FutureTrendDataType.Constrained)
                GraphInformation.GraphLinkingText = LocalizedReportPages.BenchmarkShowUnconstrained;
            if (futureTrendDataType == FutureTrendDataType.Unconstrained)
                GraphInformation.GraphLinkingText = LocalizedReportPages.BenchmarkShowConstrained;

        }

        private void BenchmarkUpdateGraphLinkingText()
        {
            FutureTrendDataType futureTrendDataType;
            Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);


            if (futureTrendDataType == FutureTrendDataType.Constrained)
            {
                GeneralParams.ReportTypeControl.SelectedForecastType.SelectedIndex = 1;
                GraphInformation.GraphLinkingText = LocalizedReportPages.BenchmarkShowConstrained;
            }
            else
            {
                GeneralParams.ReportTypeControl.SelectedForecastType.SelectedIndex = 0;
                GraphInformation.GraphLinkingText = LocalizedReportPages.BenchmarkShowUnconstrained;
            }
        }
    }
}