using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using App.BLL.EventArgs;
using App.BLL.ExtensionMethods;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.Sizing;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using Mars.App.Classes.BLL.ExtensionMethods;
using Resources;
using Mars.Sizing;


namespace App.Reports.Sizing
{
    public partial class SiteComparison : System.Web.UI.Page
    {
        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session["SiteComparisonGraphInformation"];
            }
            set { Session["SiteComparisonGraphInformation"] = value; }
        }

        private Dictionary<string, List<GraphSeries>> SiteComparisonGraphData
        {
            get
            {
                return (Dictionary<string, List<GraphSeries>>)Session["SiteComparisonData"];
            }
            set { Session["SiteComparisonData"] = value; }
        }

        private string CurrentKey
        {
            get
            {
                return (string)Session["SiteComparisonCurrentKey"];
            }
            set { Session["SiteComparisonCurrentKey"] = value; }
        }

        private const string CountryKey = "Country";
        private const string PoolKey = "Pool";
        private const string LocationGroupKey = "LocationGroup";


        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += PageLoadComplete;
            ccSiteComparison.GraphTypesAllowed = new List<SeriesChartType>
                                        {
                                            SeriesChartType.Column,
                                            SeriesChartType.Bar,
                                            SeriesChartType.Pie,
                                            SeriesChartType.Doughnut
                                        };

            //var ll = Enum.GetValues(typeof(SeriesChartType)).Cast<SeriesChartType>().ToList();
            //ccSiteComparison.GraphTypesAllowed = ll;

            if (SiteComparisonGraphData == null)
            {
                CurrentKey = CountryKey;
                SiteComparisonGraphData = new Dictionary<string, List<GraphSeries>>
                                              {
                                                  {CountryKey, null},
                                                  {PoolKey, null},
                                                  {LocationGroupKey, null}
                                              };
            }

            if (GraphInformation == null)
            {
                GraphInformation = new GraphData
                {
                    IsXValueDate = false,
                    AllowNegativeValuesOnYAxis = true,
                    SeriesDrawingStyle = "Cylinder",
                    HideSeriesInfo = true,
                    DataPointsXAxisTooltip = "#VALX",
                    DataPointsYAxisTooltip = "#VALY{0,0}",
                    ChartDescription = LocalizedReportPages.SiteComparisonDescription,
                    XAxisDateFormat = "dd/MM (ddd)",
                    ChartName = LocalizedReportPages.SiteComparisonReportName,
                    BranchForDrilldown = 1,
                    ReportParameters = this.GetDefaultParameters()
                };
            }
            GraphInformation.ReportParameters.First(p => p.Name == ParameterNames.LocationGroup).ParameterDropDownList.Enabled = false;

            ccSiteComparison.GraphInformation = GraphInformation;

            GeneralParams.ReportTypeControl.ShowTopicParameter = true;
            GeneralParams.ReportTypeControl.ShowFleetPlanParameter = true;
            GeneralParams.DynamicReportParametersControl.ShowQuickLocationGroupBox = false;
            GeneralParams.SelectedParameters = GraphInformation.SelectedParameters;
            GeneralParams.ParamsHolder = GraphInformation.ReportParameters;
            GraphInformation.AllowDrillDown = true;

            GeneralParams.ExportType = 3;
        }

        protected void PageLoadComplete(object sender, EventArgs e)
        {
            if (GeneralParams.ReportTypeControl.HasReportTypeChanged || GeneralParams.ParamsHolder.HasAnythingInBranchChanged(2) || this.HaveDatesChanged(GeneralParams))
            {
                SiteComparisonGraphData[CountryKey] = null;
                SiteComparisonGraphData[PoolKey] = null;
                SiteComparisonGraphData[LocationGroupKey] = null;
                return;
            }

            var countryParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.Country);
            var poolParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.Pool);

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
                        SiteComparisonGraphData[PoolKey] = null;
                    }
                    CurrentKey = PoolKey;
                }
                return;
            }

            if (poolParam.HasValueChanged)
            {
                if (poolParam.SelectedValue.Length == 0)
                {
                    CurrentKey = PoolKey;
                }
                else
                {
                    if (poolParam.SelectedValue != poolParam.LastNonEmptyValue)
                    {
                        SiteComparisonGraphData[LocationGroupKey] = null;
                    }
                    CurrentKey = LocationGroupKey;
                }
                return;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //If this page is being being loaded but we have parameters from a previous visit
            if (!IsPostBack && GraphInformation.SelectedParameters[ParameterNames.Country].Length != 0)
            {
                SetQuickNavigationMenu();
            }

            //Don't allow drilldown past Pool
            if (GraphInformation.ReportParameters.First(p => p.Name == ParameterNames.Pool).SelectedValue != "") GraphInformation.AllowDrillDown = false;


            if (GraphInformation.RefreshData || GraphInformation.CheckIfCachedDataCanBeUsed)
            {
                SetQuickNavigationMenu();

                var topicId = int.Parse(GeneralParams.ReportTypeControl.SelectedTopic.Value);
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

                if (SiteComparisonGraphData[CurrentKey] == null || GraphInformation.RefreshData)
                {
                    //SiteComparisonGraphData[CurrentKey] = new SiteComparisonLogic(new SiteComparisonRepository()).GetData(GeneralParams.SelectedParameters, topicId, scenarioId);
                    SiteComparisonGraphData[CurrentKey] =
                        SiteComparisonDataAccess.GetSiteComparisonData(GeneralParams.SelectedParameters, topicId,
                                                                        scenarioId);
                    GraphInformation.DataDate = ParameterDataAccess.GetLastDateFromCmsForecast();
                }
                else
                {
                    GraphInformation.UsingCachedGraphingData = true;
                }

                GraphInformation.SeriesData = SiteComparisonGraphData[CurrentKey];

                if (string.IsNullOrEmpty(GraphInformation.TitleDate))
                {
                    GraphInformation.ShowLabelSeriesNames.Add(GraphInformation.SeriesData.First().SeriesName);
                }

                GraphInformation.TitleDate = string.Format("{0} - {1}", GeneralParams.SelectedParameters[ParameterNames.FromDate], GeneralParams.SelectedParameters[ParameterNames.ToDate]);
                GraphInformation.TitleAdditional = string.Format("{0}", GeneralParams.ReportTypeControl.SelectedTopic.Text);

                GraphInformation.CalculateYEntriesCount();

                this.SetPreviousDates(GeneralParams);

                GraphInformation.RefreshData = false;
            }
        }

        protected void SetQuickNavigationMenu()
        {
            if (GraphInformation.SelectedParameters.Count != 0)
            {
                menuSiteComparisonQuickNav.Items.Clear();
            }

            var countryParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.Country);
            var poolParam = GeneralParams.ParamsHolder.First(p => p.Name == ParameterNames.Pool);

            if (countryParam.SelectedValue.Length != 0)
            {
                menuSiteComparisonQuickNav.Items.Add(new MenuItem("Back to Country", ParameterNames.Country));

            }

            if (poolParam.SelectedValue.Length != 0)
            {
                menuSiteComparisonQuickNav.Items.Add(new MenuItem("Back to Pool", ParameterNames.Pool));
            }

        }

        public void SiteComparisonMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            var reportParameters = GraphInformation.ReportParameters;
            var param = reportParameters.First(p => p.Name == e.Item.Value);
            param.ParameterDropDownList.SelectedIndex = 0;
            GraphInformation.CheckIfCachedDataCanBeUsed = true;
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

                var siteGroup = int.Parse(GeneralParams.ExcelExportControl.SelectedGroupBySite);
                var topicId = int.Parse(GeneralParams.ReportTypeControl.SelectedTopic.Value);
                var scenarioId = int.Parse(GeneralParams.ReportTypeControl.SelectedScenario.SelectedValue);


                Session["ExportData"] = SiteComparisonDataAccess.GetSiteComparisonExcelData(GeneralParams.SelectedParameters, siteGroup, topicId, scenarioId);

                Session["ExportFileName"] = "SiteComparisonExport";

                handled = true;
            }

            return handled;
        }
    }
}