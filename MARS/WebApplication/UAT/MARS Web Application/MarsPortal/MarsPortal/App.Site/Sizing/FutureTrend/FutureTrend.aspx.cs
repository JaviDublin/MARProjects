using System;
using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;
using App.BLL;
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
    public partial class FutureTrend : PageBase
    {
        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session["FutureTrendGraphInformation"];
            }
            set { Session["FutureTrendGraphInformation"] = value; }
        }

        private List<LocationGroupHolder> _locationGroupData;

        protected void Page_Init(object sender, EventArgs e)
        {
            _locationGroupData = ParameterCache.GetAllLocationGroups();

            ccFutureTrend.GraphTypesAllowed = new List<SeriesChartType>
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
                    ShowGraphLinkingButton = true,
                    XAxisDateFormat = "dd/MM (ddd)",
                    YAxisNumberFormat = "#,##0",
                    LabelFormat = "#,##0",
                    DataPointsXAxisTooltip = "#VALX (day #INDEX)",
                    DataPointsYAxisTooltip = "#VALY{0,0}",
                    GraphLinkingPage = "~/App.Site/Sizing/SupplyAnalysis/SupplyAnalysis.aspx",
                    YAxisZoom = "75",
                    ChartDescription = LocalizedReportPages.FutureTrendDescription,
                    GraphLinkingText = LocalizedReportPages.FutureTrendToSupplyAnalysisText,
                    ChartName = LocalizedReportPages.FutureTrendReportName,
                    ReportParameters = this.GetDefaultParameters()
                };
            }

            GeneralParams.ReportTypeControl.ShowFleetPlanParameter = true;
            GeneralParams.ReportTypeControl.ShowTimeZoneParameter = true;
            GeneralParams.ReportTypeControl.ShowForecastTypeParameter = true;

            var dynamicParams = Session["FutureTrendSupplyAnalysisDynamicParams"] as List<ReportParameter>;
            if (dynamicParams != null)
            {
                GraphInformation.ReportParameters = dynamicParams;
                GraphInformation.RefreshData = true;
                Session["FutureTrendSupplyAnalysisDynamicParams"] = null;
            }

            GeneralParams.DynamicReportParametersControl.ShowQuickLocationGroupBox = true;
            GeneralParams.ParamsHolder = GraphInformation.ReportParameters;
            GeneralParams.SelectedParameters = GraphInformation.SelectedParameters;

            ccFutureTrend.GraphInformation = GraphInformation;
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
            if (staticParams != null)
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
            GraphInformation.HasReportTypeChanged = GeneralParams.ReportTypeControl.HasReportTypeChanged;            

            DataType selectedTimeZoneType;
            FutureTrendDataType futureTrendDataType;
            
            
            Enum.TryParse(GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedValue, out selectedTimeZoneType);
            Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);
            var scenarioId =  int.Parse(GeneralParams.ReportTypeControl.SelectedScenario.SelectedValue);

            var frozenZoneSelected = selectedTimeZoneType == DataType.FrozenZone;
            
            GeneralParams.DynamicReportParametersControl.DateRangePickerControl.ShowThisWeekAndNextWeekInDropdown = frozenZoneSelected;
            GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(frozenZoneSelected);

            if (GraphInformation.RefreshData)
            {
                GraphInformation.DataDate = ParameterDataAccess.GetLastDateFromCmsForecast();

                GraphInformation.TitleAdditional = string.Format("{0}  -  {1}  -  {2}", 
                        GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedItem.Text,
                        GeneralParams.ReportTypeControl.SelectedForecastType.SelectedItem.Text, 
                        GeneralParams.ReportTypeControl.SelectedScenario.SelectedItem.Text);

                GraphInformation.SeriesData = FutureTrendDataAccess.GetFutureTrendGraphingData(GeneralParams.SelectedParameters,
                                                                                  futureTrendDataType, scenarioId,
                                                                                  selectedTimeZoneType);
                GraphInformation.CalculateYEntriesCount();

                GraphInformation.RefreshData = false;
            }
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
                var siteGroup = GeneralParams.ExcelExportControl.SelectedGroupBySite;
                var fleetGroup = GeneralParams.ExcelExportControl.SelectedGroupByFleet;

                DataType selectedTimeZoneType;
                FutureTrendDataType futureTrendDataType;
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedValue, out selectedTimeZoneType);
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);
                var scenarioId = int.Parse(GeneralParams.ReportTypeControl.SelectedScenario.SelectedValue);
                GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(selectedTimeZoneType == DataType.FrozenZone);

                Session["ExportData"] = FutureTrendDataAccess.GetFutureTrendExcelData(GeneralParams.SelectedParameters,
                                                                      futureTrendDataType, scenarioId, int.Parse(siteGroup),
                                                                      int.Parse(fleetGroup), selectedTimeZoneType);
                Session["ExportFileName"] = "FutureTrendExport";

                handled = true;
            }

            if(args is GraphLinkingEventArgs)
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
                Session["ControlRibbonTitle"] = LocalizedReportPages.SupplyAnalysisReportName;
                Session["FutureTrendSupplyAnalysisStaticParams"] = transferParameterData;
                Response.Redirect( ((GraphLinkingEventArgs) args).LinkingPage, true);
            }

            return handled;
        }

    }
}