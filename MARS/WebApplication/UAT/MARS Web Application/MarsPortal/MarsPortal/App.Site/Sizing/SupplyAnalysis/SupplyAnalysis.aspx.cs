using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using App.BLL.Cache;
using App.BLL.EventArgs;
using App.BLL.ExtensionMethods;
using App.BLL.ReportEnums.FleetSize;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.ParameterAccess.DataHolders;
using App.DAL.MarsDataAccess.Sizing;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using App.Graphing.Parameters;
using Mars.App.Classes.BLL.ExtensionMethods;
using Resources;

namespace App.Reports.Sizing
{
    public partial class SupplyAnalysis : Page
    {
        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session["SupplyAnalysisGraphInformation"];
            }
            set { Session["SupplyAnalysisGraphInformation"] = value; }
        }

        private List<LocationGroupHolder> _locationGroupData;

        protected void Page_Init(object sender, EventArgs e)
        {
            _locationGroupData = ParameterCache.GetAllLocationGroups();

            ccSuplyAnalysis.GraphTypesAllowed = new List<SeriesChartType>
                                        {
                                            SeriesChartType.Column,
                                            SeriesChartType.Bar
                                        };

            if (GraphInformation == null)
            {
                GraphInformation = new GraphData
                {
                    IsXValueDate = true,
                    AllowNegativeValuesOnYAxis = true,
                    ShowGraphLinkingButton = true,
                    ShowingSeriesInformation = false,
                    ShowWeeklyMinimum = true,
                    SeriesDrawingStyle = "Cylinder",
                    LightXAxisLineColour = Color.LightGray,
                    NegativeValuesColour = Color.Red,
                    DataPointsYAxisTooltip = "#VALY{0,0}",
                    DataPointsXAxisTooltip = "#VALX (day #INDEX)",
                    XAxisDateFormat = "dd/MM (ddd)",
                    YAxisNumberFormat = "#,##0",
                    LabelFormat = "#,##0",
                    ChartDescription = LocalizedReportPages.SupplyAnalysisDescription,
                    ChartName = LocalizedReportPages.SupplyAnalysisReportName,
                    GraphLinkingText = LocalizedReportPages.SupplyAnalysisToFutureTrendText,
                    GraphLinkingPage = "~/App.Site/Sizing/FutureTrend/FutureTrend.aspx",
                    ReportParameters = this.GetDefaultParameters()
                };
            }

            GraphInformation.ShowLabelSeriesNames.Add("Weekly Minimum Supply");

            GeneralParams.ReportTypeControl.ShowFleetPlanParameter = true;
            GeneralParams.ReportTypeControl.ShowTimeZoneParameter = true;
            GeneralParams.ReportTypeControl.ShowForecastTypeParameter = true;

            ccSuplyAnalysis.GraphInformation = GraphInformation;

            var dynamicParams = Session["FutureTrendSupplyAnalysisDynamicParams"] as List<ReportParameter>;
            if (dynamicParams != null)
            {
                GraphInformation.ReportParameters = dynamicParams;
                GraphInformation.RefreshData = true;
                Session["FutureTrendSupplyAnalysisDynamicParams"] = null;
            }

            GeneralParams.SelectedParameters = GraphInformation.SelectedParameters;

            GeneralParams.ParamsHolder = GraphInformation.ReportParameters;

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

            var datePickerControl = GeneralParams.DynamicReportParametersControl.DateRangePickerControl;
            var reportTypeControl = GeneralParams.ReportTypeControl;

            

            var staticParams = Session["FutureTrendSupplyAnalysisStaticParams"] as FutureTrendSupplyAnalysisTransfer;
            if(staticParams != null)
            {
                reportTypeControl.SelectedScenario.SelectedIndex = staticParams.FleetPlanSelectedIndex;
                reportTypeControl.SelectedForecastType.SelectedIndex = staticParams.ForecastTypeSelectedIndex;
                reportTypeControl.SelectedTimeZone.SelectedIndex = staticParams.TimeZoneSelectedIndex;
                datePickerControl.DateRangeControl.SelectedIndex = staticParams.DateRangeSelectedIndex;
                datePickerControl.SetToAndFromDateTextBoxes(staticParams.FromDate, staticParams.ToDate);
                Session["FutureTrendSupplyAnalysisStaticParams"] = null;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataType selectedTimeZoneType;
            Enum.TryParse(GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedValue, out selectedTimeZoneType);

            var frozenZoneSelected = selectedTimeZoneType == DataType.FrozenZone;

            GeneralParams.DynamicReportParametersControl.DateRangePickerControl.ShowThisWeekAndNextWeekInDropdown = frozenZoneSelected;
            GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(frozenZoneSelected);

            if (GraphInformation.RefreshData)
            {
                
                var scenarioId = int.Parse(GeneralParams.ReportTypeControl.SelectedScenario.SelectedValue);

                FutureTrendDataType futureTrendDataType;
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);

                var seriesData = SupplyAnalysisDataAccess.GetSupplyAnalysisData(GeneralParams.SelectedParameters, futureTrendDataType,
                                                               scenarioId, selectedTimeZoneType);


                GraphInformation.WeeklySeriesData = seriesData.Where(d => d.SeriesName == "Weekly Minimum Supply").ToList();
                seriesData.RemoveAll(d => d.SeriesName == "Weekly Minimum Supply");

                GraphInformation.DataDate = ParameterDataAccess.GetLastDateFromCmsForecast();

                GraphInformation.TitleAdditional = string.Format("{0}  -  {1}  -  {2}",
                        GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedItem.Text,
                        GeneralParams.ReportTypeControl.SelectedForecastType.SelectedItem.Text,
                        GeneralParams.ReportTypeControl.SelectedScenario.SelectedItem.Text);

                GraphInformation.SeriesData = seriesData;

                GraphInformation.CalculateYEntriesCount();


                GraphInformation.RefreshData = false;
            }
            var intervalType = ccSuplyAnalysis.GetIntervalType();
            GraphInformation.UseWeeklyData = intervalType == DateTimeIntervalType.Weeks;
            GraphInformation.IsXValueDate = !GraphInformation.UseWeeklyData;

        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is RefreshGraphEventArgs)
            {
                GraphInformation.RefreshData = true;
                handled = true;
            }

            if (args is ExportToExcelEventArgs)
            {
                GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(false);
                var siteGroup = GeneralParams.ExcelExportControl.SelectedGroupBySite;
                var fleetGroup = GeneralParams.ExcelExportControl.SelectedGroupByFleet;

                DataType selectedTimeZoneType;
                FutureTrendDataType futureTrendDataType;
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedValue, out selectedTimeZoneType);
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);
                var scenarioId = int.Parse(GeneralParams.ReportTypeControl.SelectedScenario.SelectedValue);
                GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(selectedTimeZoneType == DataType.FrozenZone);

                var intervalType = ccSuplyAnalysis.GetIntervalType();


                Session["ExportData"] = SupplyAnalysisDataAccess.GetSupplyAnalysisExcelData(GeneralParams.SelectedParameters,
                                                                      futureTrendDataType, scenarioId, int.Parse(siteGroup),
                                                                      int.Parse(fleetGroup), selectedTimeZoneType
                                                                      , intervalType == DateTimeIntervalType.Weeks);
                Session["ExportFileName"] = "SupplyAnalysisExport";

                handled = true;
            }

            if (args is GraphLinkingEventArgs)
            {
                var datePickerControl = GeneralParams.DynamicReportParametersControl.DateRangePickerControl;
                var reportTypeControl = GeneralParams.ReportTypeControl;
                Session["FutureTrendSupplyAnalysisDynamicParams"] = GraphInformation.ReportParameters;
                var transferParameterData = new FutureTrendSupplyAnalysisTransfer()
                {
                    DateRangeSelectedIndex = datePickerControl.DateRangeControl.SelectedIndex,
                    FromDate = datePickerControl.FromDate,
                    ToDate = datePickerControl.ToDate,
                    FleetPlanSelectedIndex = reportTypeControl.SelectedScenario.SelectedIndex,
                    ForecastTypeSelectedIndex = reportTypeControl.SelectedForecastType.SelectedIndex,
                    TimeZoneSelectedIndex = reportTypeControl.SelectedTimeZone.SelectedIndex
                };
                Session["FutureTrendSupplyAnalysisStaticParams"] = transferParameterData;
                Session["ControlRibbonTitle"] = LocalizedReportPages.FutureTrendReportName;
                Response.Redirect(((GraphLinkingEventArgs)args).LinkingPage, true);
            }

            return handled;
        }
    }
}