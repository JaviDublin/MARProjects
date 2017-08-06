using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using App.BLL.Cache;
using App.BLL.EventArgs;
using App.BLL.ExtensionMethods;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.ParameterAccess.DataHolders;
using App.DAL.MarsDataAccess.Sizing;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using App.UserControls.Parameters;
using Mars.App.Classes.BLL.ExtensionMethods;
using Resources;


namespace App.Reports.Sizing
{
    public partial class FleetComparison : System.Web.UI.Page
    {
        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session["FleetComparisonGraphInformation"];
            }
            set { Session["FleetComparisonGraphInformation"] = value; }
        }

        private Dictionary<string, List<GraphSeries>> FleetComparisonGraphData
        {
            get
            {
                return (Dictionary<string, List<GraphSeries>>)Session["FleetComparisonData"];
            }
            set { Session["FleetComparisonData"] = value; }
        }

        private string CurrentKey
        {
            get
            {
                return (string)Session["FleetComparisonCurrentKey"];
            }
            set { Session["FleetComparisonCurrentKey"] = value; }
        }

        private const string CountryKey = "Country";
        private const string CarSegmentKey = "CarSegment";
        private const string CarClassGroupKey = "CarClassGroup";
        private const string CarClassKey = "CarClass";

        private List<LocationGroupHolder> _locationGroupData;

        protected void Page_Init(object sender, EventArgs e)
        {
            _locationGroupData = ParameterCache.GetAllLocationGroups();

            Page.LoadComplete += PageLoadComplete;
            ccSiteComparison.GraphTypesAllowed = new List<SeriesChartType>
                                        {
                                            SeriesChartType.Column,
                                            SeriesChartType.Bar,
                                            SeriesChartType.Pie,
                                            SeriesChartType.Doughnut
                                        };

            if (FleetComparisonGraphData == null)
            {
                CurrentKey = CountryKey;
                FleetComparisonGraphData = new Dictionary<string, List<GraphSeries>>
                                              {
                                                  {CountryKey, null},
                                                  {CarSegmentKey, null},
                                                  {CarClassGroupKey, null},
                                                  {CarClassKey, null}
                                              };
            }

            if (GraphInformation == null)
            {
                GraphInformation = new GraphData
                {
                    IsXValueDate = false,
                    AllowNegativeValuesOnYAxis = true,
                    HideSeriesInfo = true,
                    SeriesDrawingStyle = "Cylinder",
                    DataPointsXAxisTooltip = "#VALX",
                    DataPointsYAxisTooltip = "#VALY{0,0}",
                    XAxisDateFormat = "dd/MM (ddd)",
                    ChartDescription = LocalizedReportPages.FleetComparisonDescription,
                    ChartName = LocalizedReportPages.FleetComparisonReportName,
                    BranchForDrilldown = 2,
                    ReportParameters = this.GetDefaultParameters()
                };
            }

            GraphInformation.ReportParameters.First(p => p.Name == ParameterNames.CarClass).ParameterDropDownList.Enabled = false;
            ccSiteComparison.GraphInformation = GraphInformation;

            GeneralParams.ReportTypeControl.ShowFleetPlanParameter = true;
            GeneralParams.ReportTypeControl.ShowTopicParameter = true;
            GeneralParams.DynamicReportParametersControl.ShowQuickLocationGroupBox = true;
            GeneralParams.SelectedParameters = GraphInformation.SelectedParameters;
            GeneralParams.ParamsHolder = GraphInformation.ReportParameters;
            GraphInformation.AllowDrillDown = true;
            GeneralParams.ExportType = 2;
        }

        protected void PageLoadComplete(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GeneralParams.DynamicReportParametersControl.QuickSelectedValue))
            {
                this.CheckQuickLocationGroupSelected(_locationGroupData, GeneralParams.DynamicReportParametersControl.QuickSelectedValue, GeneralParams.SelectedParameters, GraphInformation.ReportParameters);
                upnlGeneralParameters.Update();
            }

            if (GeneralParams.ReportTypeControl.HasReportTypeChanged || GeneralParams.ParamsHolder.HasAnythingInBranchChanged(1) || this.HaveDatesChanged(GeneralParams))
            {
                FleetComparisonGraphData[CountryKey] = null;
                FleetComparisonGraphData[CarSegmentKey] = null;
                FleetComparisonGraphData[CarClassGroupKey] = null;
                FleetComparisonGraphData[CarClassKey] = null;
                return;
            }

            var countryParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.Country);
            var carSegmentParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.CarSegment);
            var carClassGroupParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.CarClassGroup);

            if (countryParam.HasValueChanged)
            {
                if (countryParam.SelectedValue.Length == 0)
                {
                    CurrentKey = CountryKey;
                }
                else
                {
                    if (countryParam.SelectedValue != countryParam.LastNonEmptyValue)
                    {
                        FleetComparisonGraphData[CarSegmentKey] = null;
                    }
                    CurrentKey = CarSegmentKey;
                }
                return;
            }

            if (carSegmentParam.HasValueChanged)
            {
                if (carSegmentParam.SelectedValue.Length == 0)
                {
                    CurrentKey = CarSegmentKey;
                }
                else
                {
                    if (carSegmentParam.SelectedValue != carSegmentParam.LastNonEmptyValue)
                    {
                        FleetComparisonGraphData[CarClassGroupKey] = null;
                    }
                    CurrentKey = CarClassGroupKey;
                }
                return;
            }

            if (carClassGroupParam.HasValueChanged)
            {
                if (carClassGroupParam.SelectedValue.Length == 0)
                {
                    CurrentKey = CarClassGroupKey;
                }
                else
                {
                    if (carClassGroupParam.SelectedValue != carClassGroupParam.LastNonEmptyValue)
                    {
                        FleetComparisonGraphData[CarClassKey] = null;
                    }
                    CurrentKey = CarClassKey;
                }
                return;
            }
        }

        protected void SetQuickNavigationMenu()
        {
            if (GraphInformation.SelectedParameters.Count != 0)
            {
                menuFleetComparisonQuickNav.Items.Clear();
            }

            var countryParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.Country);
            var carSegmentParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.CarSegment);
            var carClassGroupParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.CarClassGroup);

            if (countryParam.SelectedValue.Length != 0)
            {
                menuFleetComparisonQuickNav.Items.Add(new MenuItem("Back to Country", ParameterNames.Country));
            }
            if (carSegmentParam.SelectedValue.Length != 0)
            {
                menuFleetComparisonQuickNav.Items.Add(new MenuItem("Back to Car Segment", ParameterNames.CarSegment));
            }
            if (carClassGroupParam.SelectedValue.Length != 0)
            {
                menuFleetComparisonQuickNav.Items.Add(new MenuItem("Back to Car Class Group", ParameterNames.CarClassGroup));
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //If this page is being being loaded but we have parameters from a previous visit
            if (!IsPostBack && GraphInformation.SelectedParameters[ParameterNames.Country].Length != 0)
            {
                SetQuickNavigationMenu();
            }

            //Don't allow drilldown past Car Class Group
            if (GraphInformation.ReportParameters.First(p => p.Name == ParameterNames.CarClassGroup).SelectedValue.Length != 0) GraphInformation.AllowDrillDown = false;

            if (GraphInformation.RefreshData || GraphInformation.CheckIfCachedDataCanBeUsed)
            {
                SetQuickNavigationMenu();

                var rtSelectionReport = (ReportTypeParameters)GeneralParams.FindControl("rtSelectionReport");
                var topicId = int.Parse(rtSelectionReport.SelectedTopic.Value);
                var scenarioId = int.Parse(GeneralParams.ReportTypeControl.SelectedScenario.SelectedValue);

                if (topicId > 2)
                {
                    GraphInformation.LabelFormat = "0%";
                    GraphInformation.DataPointsYAxisTooltip = "#VALY{0%}";
                    GraphInformation.YAxisNumberFormat = "0%";
                }
                else
                {
                    GraphInformation.LabelFormat = "#,##0";
                    GraphInformation.DataPointsYAxisTooltip = "#VALY{0,0}";
                    GraphInformation.YAxisNumberFormat = "#,##0";
                }

                if (FleetComparisonGraphData[CurrentKey] == null || GraphInformation.RefreshData)
                {
                    //FleetComparisonGraphData[CurrentKey]=new FleetComparisonLogic(new FleetComparisonRepository()).GetData(GeneralParams.SelectedParameters,topicId,scenarioId);
                    FleetComparisonGraphData[CurrentKey] =
                        FleetComparisonDataAccess.GetFleetComarisonData(GeneralParams.SelectedParameters, topicId,
                                                                        scenarioId);


                    GraphInformation.DataDate = ParameterDataAccess.GetLastDateFromCmsForecast();
                }
                else
                {
                    GraphInformation.UsingCachedGraphingData = true;
                }

                GraphInformation.SeriesData = FleetComparisonGraphData[CurrentKey];

                if (string.IsNullOrEmpty(GraphInformation.TitleDate))
                {
                    GraphInformation.ShowLabelSeriesNames.Add(GraphInformation.SeriesData.First().SeriesName);
                }

                this.SetPreviousDates(GeneralParams);

                GraphInformation.CalculateYEntriesCount();
                GraphInformation.TitleDate = string.Format("{0} - {1}", GeneralParams.SelectedParameters[ParameterNames.FromDate], GeneralParams.SelectedParameters[ParameterNames.ToDate]);
                GraphInformation.TitleAdditional = string.Format("{0}", rtSelectionReport.SelectedTopic.Text);

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
                GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(false);


                var fleetGroup = int.Parse(GeneralParams.ExcelExportControl.SelectedGroupByFleet);
                var topicId = int.Parse(GeneralParams.ReportTypeControl.SelectedTopic.Value);
                var scenarioId = int.Parse(GeneralParams.ReportTypeControl.SelectedScenario.SelectedValue);

                Session["ExportData"] = FleetComparisonDataAccess.GetFleetComparisonExcelData(GeneralParams.SelectedParameters, fleetGroup, topicId, scenarioId);
                
                //Session["ExportData"] = new FleetComparisonExcelLogic().GetExcelData(
                //                                        GeneralParams.SelectedParameters, fleetGroup, topicId, scenarioId);

                Session["ExportFileName"] = "FleetComparisonExport";

                handled = true;
            }

            return handled;
        }

        protected void SiteComparisonMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            var reportParameters = GraphInformation.ReportParameters;
            var param = reportParameters.First(p => p.Name == e.Item.Value);
            param.ParameterDropDownList.SelectedIndex = 0;
            GraphInformation.CheckIfCachedDataCanBeUsed = true;
        }
    }
}