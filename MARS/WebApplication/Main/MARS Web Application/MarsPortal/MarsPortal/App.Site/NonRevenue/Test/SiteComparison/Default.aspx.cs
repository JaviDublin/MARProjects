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

namespace App.Site.NonRevenue.Comparison.Site.Test
{
    public partial class Default : System.Web.UI.Page
    {
        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session["SiteComparisonNRGraphInformation"];
            }
            set { Session["SiteComparisonNRGraphInformation"] = value; }
        }

        private Dictionary<string, List<GraphSeries>> SiteComparisonNRGraphData
        {
            get
            {
                return (Dictionary<string, List<GraphSeries>>)Session["SiteComparisonNRData"];
            }
            set { Session["SiteComparisonNRData"] = value; }
        }

        private string CurrentKey
        {
            get
            {
                return (string)Session["SiteComparisonNRCurrentKey"];
            }
            set { Session["SiteComparisonNRCurrentKey"] = value; }
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

            if (SiteComparisonNRGraphData == null)
            {
                CurrentKey = CountryKey;
                SiteComparisonNRGraphData = new Dictionary<string, List<GraphSeries>>
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
                    ChartDescription = LocalizedReportPages.SiteComparisonNRDescription,
                    XAxisDateFormat = "dd/MM (ddd)",
                    ChartName = LocalizedReportPages.SiteComparisonNRReportName,
                    BranchForDrilldown = 1,
                    ReportParameters = this.GetDefaultParameters()
                };
            }
            GraphInformation.ReportParameters.First(p => p.Name == ParameterNames.LocationGroup).ParameterDropDownList.Enabled = false;

            ccSiteComparison.GraphInformation = GraphInformation;

            GeneralParams.ReportTypeControl.ShowTopicParameter = true;
            GeneralParams.ReportTypeControl.ShowFleetPlanParameter = false;
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
                SiteComparisonNRGraphData[CountryKey] = null;
                SiteComparisonNRGraphData[PoolKey] = null;
                SiteComparisonNRGraphData[LocationGroupKey] = null;
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
                        SiteComparisonNRGraphData[PoolKey] = null;
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
                        SiteComparisonNRGraphData[LocationGroupKey] = null;
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

                if (SiteComparisonNRGraphData[CurrentKey] == null || GraphInformation.RefreshData)
                {
                    SiteComparisonNRGraphData[CurrentKey] = NonRevDataAccess.GetSiteComparisonNRData(GeneralParams.SelectedParameters, topicId, scenarioId);
                    GraphInformation.DataDate = ParameterDataAccess.GetLastDateFromCmsForecast();
                }
                else
                {
                    GraphInformation.UsingCachedGraphingData = true;
                }

                GraphInformation.SeriesData = SiteComparisonNRGraphData[CurrentKey];

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

                Session["ExportData"] = SiteComparisonDataAccess.GetSiteComparisonExcelData(
                                                        GeneralParams.SelectedParameters, siteGroup, topicId, scenarioId);

                Session["ExportFileName"] = "SiteComparisonNRExport";

                handled = true;
            }

            return handled;
        }
    }
}