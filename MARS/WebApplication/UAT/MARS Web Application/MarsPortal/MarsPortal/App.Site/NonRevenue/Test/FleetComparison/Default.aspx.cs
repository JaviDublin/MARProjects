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
using Resources;
using App.DAL.MarsDataAccess.NonRev;

namespace App.Site.NonRevenue.Comparison.Fleet.Test
{
    public partial class Default : System.Web.UI.Page
    {
        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session["FleetComparisonNRGraphInformation"];
            }
            set { Session["FleetComparisonNRGraphInformation"] = value; }
        }

        private Dictionary<string, List<GraphSeries>> FleetComparisonNRGraphData
        {
            get
            {
                return (Dictionary<string, List<GraphSeries>>)Session["FleetComparisonNRData"];
            }
            set { Session["FleetComparisonNRData"] = value; }
        }

        private string CurrentKey
        {
            get
            {
                return (string)Session["FleetComparisonNRCurrentKey"];
            }
            set { Session["FleetComparisonNRCurrentKey"] = value; }
        }

        private const string CountryKey = "Country";
        private const string CarSegmentKey = "CarSegment";
        private const string CarClassGroupKey = "CarClassGroup";
        private const string CarClassKey = "CarClass";

        private const string CarGroup = "CarGroup";

        private List<LocationGroupHolder> _locationGroupData;

        protected void Page_Init(object sender, EventArgs e)
        {
            _locationGroupData = LocationGroupCache.GetAllLocationGroups();

            Page.LoadComplete += PageLoadComplete;
            ccFleetComparison.GraphTypesAllowed = new List<SeriesChartType>
                                        {
                                            SeriesChartType.Column,
                                            SeriesChartType.Bar,
                                            SeriesChartType.Pie,
                                            SeriesChartType.Doughnut
                                        };

            if (FleetComparisonNRGraphData == null)
            {
                CurrentKey = CountryKey;
                FleetComparisonNRGraphData = new Dictionary<string, List<GraphSeries>>
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
                    ChartDescription = LocalizedReportPages.FleetComparisonNRDescription,
                    ChartName = LocalizedReportPages.FleetComparisonNRReportName,
                    BranchForDrilldown = 2,
                    ReportParameters = this.GetDefaultParameters()
                };
            }

            GraphInformation.ReportParameters.First(p => p.Name == ParameterNames.CarClass).ParameterDropDownList.Enabled = false;
            ccFleetComparison.GraphInformation = GraphInformation;

            GeneralParams.ReportTypeControl.ShowFleetPlanParameter = false;
            GeneralParams.ReportTypeControl.ShowTopicParameter = true;
            GeneralParams.DynamicReportParametersControl.ShowQuickLocationGroupBox = false;
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
                FleetComparisonNRGraphData[CountryKey] = null;
                FleetComparisonNRGraphData[CarSegmentKey] = null;
                FleetComparisonNRGraphData[CarClassGroupKey] = null;
                FleetComparisonNRGraphData[CarClassKey] = null;
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
                        FleetComparisonNRGraphData[CarSegmentKey] = null;
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
                        FleetComparisonNRGraphData[CarClassGroupKey] = null;
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
                        FleetComparisonNRGraphData[CarClassKey] = null;
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

                if (FleetComparisonNRGraphData[CurrentKey] == null || GraphInformation.RefreshData)
                {
                    FleetComparisonNRGraphData[CurrentKey] = NonRevDataAccess.GetFleetComarisonNRData(GeneralParams.SelectedParameters, topicId, scenarioId);
                    GraphInformation.DataDate = ParameterDataAccess.GetLastDateFromViewNonRevLogStats();
                }
                else
                {
                    GraphInformation.UsingCachedGraphingData = true;
                }

                GraphInformation.SeriesData = FleetComparisonNRGraphData[CurrentKey];

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

                Session["ExportData"] = NonRevDataAccess.GetFleetComparisonNRExcelData(
                                                        GeneralParams.SelectedParameters, fleetGroup, topicId, scenarioId);

                Session["ExportFileName"] = "FleetComparisonNRExport";

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