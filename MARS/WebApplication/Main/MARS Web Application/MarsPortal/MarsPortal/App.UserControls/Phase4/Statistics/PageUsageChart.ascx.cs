using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using App.BLL.ExtensionMethods;
using App.Entities.Graphing;
using App.Styles;
using Mars.App.Classes.Phase4Dal.UsageStatistics.Entities;
using Mars.FleetAllocation.BusinessLogic;

namespace Mars.App.UserControls.Phase4.Statistics
{
    public partial class PageUsageChart : UserControl
    {
        private const string PageUsageChartChartGraphInfo = "PageUsageChartChartGraphInfo";
        private const string PageUsageChartChartHiddenSeries = "PageUsageChartChartHiddenSeries";


        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[PageUsageChartChartGraphInfo];
            }
            set { Session[PageUsageChartChartGraphInfo] = value; }
        }

        public List<string> ShowSeries
        {
            set { Session[PageUsageChartChartHiddenSeries] = value; }
            get { return (List<string>)Session[PageUsageChartChartHiddenSeries]; }
        }

        private bool ShowLegend
        {
            get { return bool.Parse(hfShowLegend.Value); }
            set { hfShowLegend.Value = value.ToString(); }
        }

        public const string Checkedboximagelocation = "~/App.Images/TickBoxTicked.png";
        public const string Uncheckedboximagelocation = "~/App.Images/TickBox.png";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (ShowSeries == null)
            {
                ShowSeries = new List<string>();
            }
            if (GraphInformation == null)
            {
                GraphInformation = new GraphData();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (GraphInformation.SeriesData != null)
            {
                chrtPageUseageComparison.Visible = true;
                BuildColumnChart();
            }
            else
            {
                chrtPageUseageComparison.Visible = false;
            }

        }

        public void LoadUseageData(IEnumerable<UsageStatisticsDataRow> statisticsData)
        {
            var colourNumber = 0;
            var seriesToGraph = new List<GraphSeries>();
            foreach (var s in statisticsData)
            {
                var pageName = s.PageName;
                ShowSeries.Add(pageName);
                var series = new GraphSeries(pageName)
                             {
                                 GraphColour =
                                     ColorTranslator.FromHtml("#" + GraphingColours.ColourValues[colourNumber]),
                                 Displayed = true
                             };
                series.Xvalue.Add(pageName);
                series.Yvalue.Add(s.PageRequests);

                seriesToGraph.Add(series);



                colourNumber++;
            }

            GraphInformation.SeriesData = seriesToGraph;
        }

        private void BuildColumnChart()
        {
            var ca = new ChartArea { Name = "Forecasted Fleet Size" };

            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisY.MajorGrid.Enabled = true;
            ca.AxisY.MajorGrid.LineColor = Color.LightGray;
            ca.AxisY.LabelStyle.Format = "#,0";
            ca.AxisX.MajorGrid.Enabled = true;
            ca.AxisX.MajorGrid.LineColor = Color.LightGray;
            ca.AxisX.MajorGrid.LineWidth = 1;

            chrtPageUseageComparison.ChartAreas.Add(ca);

            foreach (var cd in GraphInformation.SeriesData)
            {
                var cs = new Series(cd.SeriesName)
                {
                    ChartType = SeriesChartType.Column,
                    Color = cd.GraphColour,
                    BorderWidth = 2,
                    XValueType = ChartValueType.Int32,

                    IsValueShownAsLabel = GraphInformation.ShowLabelSeriesNames.Contains(cd.SeriesName),
                    ToolTip = "#SERIESNAME - #VALY{#,0}",
                    LabelFormat = "#,0"
                };

                cs["DrawingStyle"] = "Cylinder";


                for (var i = 0; i < cd.Xvalue.Count; i++)
                {
                    cs.Points.AddXY(cd.Xvalue[i], cd.Yvalue[i]);
                }

                chrtPageUseageComparison.Series.Add(cs);

                var imageLocation = cd.Displayed ? Checkedboximagelocation : Uncheckedboximagelocation;

                var legendItem = new LegendItem { Name = "Legend Item" };
                var legendCell = new LegendCell
                {
                    CellType = LegendCellType.Image,
                    Image = imageLocation,
                    Name = "Cell1",
                    Margins =
                    {
                        Left = 15,
                        Right = 15
                    },
                    PostBackValue = "LegendClick/" + cd.SeriesName
                };

                var seriesHidden = GraphInformation.HiddenSeriesNames.Contains(cd.SeriesName);

                var legendCell2Colour = seriesHidden ? MarsColours.ChartLegendValuesHidden : cd.GraphColour;

                legendItem.Cells.Add(legendCell);
                legendCell = new LegendCell
                {
                    Name = "Cell2",
                    Text = cd.SeriesName,
                    ForeColor = legendCell2Colour,
                    Alignment = ContentAlignment.MiddleLeft
                };
                legendItem.Cells.Add(legendCell);

                var valuesShown = GraphInformation.ShowLabelSeriesNames.Contains(cd.SeriesName);

                var legendCell3Colour = !valuesShown || seriesHidden
                    ? MarsColours.ChartLegendValuesHidden
                    : MarsColours.ChartLegendValuesShown;


                legendCell = new LegendCell
                {
                    Name = "Cell3",
                    Text = "Values",
                    ForeColor = legendCell3Colour,
                    Font = new Font("Tahoma", 9),
                    Alignment = ContentAlignment.MiddleRight,
                    PostBackValue = "LegendShowLabels/" + cd.SeriesName

                };
                legendItem.Cells.Add(legendCell);

                chrtPageUseageComparison.Legends["RightLegend"].CustomItems.Add(legendItem);

                if (cd.Displayed == false)
                {
                    cs.IsVisibleInLegend = false;
                    cs.Enabled = false;

                }
            }
        }


        protected void chrtPageUseageComparison_Click(object sender, ImageMapEventArgs e)
        {
            if (e.PostBackValue.StartsWith("LegendClick"))
            {
                var seriesName = e.PostBackValue.Split('/')[1];
                var gs = GraphInformation.SeriesData.Find(s => s.SeriesName == seriesName);
                gs.Displayed = !gs.Displayed;

                GraphInformation.HiddenSeriesNames.AddOrRemoveString(gs.SeriesName);
                return;
            }
            if (e.PostBackValue.StartsWith("LegendShowLabels"))
            {
                var seriesName = e.PostBackValue.Split('/')[1];
                var gs = GraphInformation.SeriesData.Find(s => s.SeriesName == seriesName);

                gs.ShowLabel = !gs.ShowLabel;

                GraphInformation.ShowLabelSeriesNames.AddOrRemoveString(gs.SeriesName);
                return;
            }
            if (e.PostBackValue == "HideSeries")
            {
                var legend = chrtPageUseageComparison.Legends["Legend1"];

                legend.Enabled = !GraphInformation.ShowingSeriesInformation;
                GraphInformation.ShowingSeriesInformation = !GraphInformation.ShowingSeriesInformation;

                return;
            }

            if (e.PostBackValue.StartsWith("HideLegend"))
            {
                ShowLegend = false;
                chrtPageUseageComparison.Legends["RightLegend"].Enabled = false;
                return;
            }
            if (e.PostBackValue.StartsWith("ShowLegend"))
            {
                ShowLegend = true;
                chrtPageUseageComparison.Legends["RightLegend"].Enabled = true;
                return;
            }
        }
    }
}