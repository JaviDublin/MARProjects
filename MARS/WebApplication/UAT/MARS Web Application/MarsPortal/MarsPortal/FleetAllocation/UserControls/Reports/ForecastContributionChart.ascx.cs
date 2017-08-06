using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using App.BLL.ExtensionMethods;
using App.Entities.Graphing;
using App.Styles;
using Mars.FleetAllocation.DataAccess.Reporting.ForecastContribution.Entities;

namespace Mars.FleetAllocation.UserControls.Reports
{
    public partial class ForecastContributionChart : UserControl
    {
        private const string FaoForecastContributionGraphInfo = "FaoForecastContributionGraphInfo";
        private const string FaoForecastContributionHiddenSeries = "FaoForecastContributionHiddenSeries";

        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[FaoForecastContributionGraphInfo];
            }
            set { Session[FaoForecastContributionGraphInfo] = value; }
        }

        public List<string> ShowSeries
        {
            set { Session[FaoForecastContributionHiddenSeries] = value; }
            get { return (List<string>)Session[FaoForecastContributionHiddenSeries]; }
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
                chrtForecastContribution.Visible = true;
                BuildColumnChart();
            }
            else
            {
                chrtForecastContribution.Visible = false;
            }
        }

        public void LoadChart(List<TotalContributionRow> comparisonData)
        {
            if (!comparisonData.Any()) return;

            var series = new List<string> { "Expected Revenue", "Scenario A Revenue", "Scenario B Revenue" };

            ShowSeries.AddRange(series.ToArray());

            var expectedSeries = new GraphSeries(series[0])
            {
                GraphColour = Color.DarkGreen,
                Displayed = true
            };

            var expectedWithAddsA = new GraphSeries(series[1])
            {
                GraphColour = Color.DarkOrchid,
                Displayed = true
            };

            var expectedWithAddsB = new GraphSeries(series[2])
            {
                GraphColour = Color.OrangeRed,
                Displayed = true
            };

            foreach (var f in comparisonData)
            {
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(f.Month);
                expectedSeries.Xvalue.Add(monthName);
                expectedSeries.Yvalue.Add(( f.ExpectedContribution));
                expectedWithAddsA.Xvalue.Add(monthName);
                expectedWithAddsA.Yvalue.Add(f.ExpectedContributionA);
                expectedWithAddsB.Xvalue.Add(monthName);
                expectedWithAddsB.Yvalue.Add(f.ExpectedContributionB);
            }

            var seriesList = new List<GraphSeries> {expectedSeries, expectedWithAddsA, expectedWithAddsB};

            GraphInformation.SeriesData = seriesList;
        }

        private void BuildColumnChart()
        {
            var ca = new ChartArea {Name = "Forecasted Fleet Size"};

            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisY.MajorGrid.Enabled = true;
            ca.AxisY.MajorGrid.LineColor = Color.LightGray;
            ca.AxisY.LabelStyle.Format = "#,0 €";
            ca.AxisX.MajorGrid.Enabled = true;
            ca.AxisX.MajorGrid.LineColor = Color.LightGray;
            ca.AxisX.MajorGrid.LineWidth = 1;

            chrtForecastContribution.ChartAreas.Add(ca);

            foreach (var cd in GraphInformation.SeriesData)
            {
                var cs = new Series(cd.SeriesName)
                         {
                             ChartType = SeriesChartType.Column,
                             Color = cd.GraphColour,
                             BorderWidth = 2,
                             XValueType = ChartValueType.Int32,
                             
                             IsValueShownAsLabel = GraphInformation.ShowLabelSeriesNames.Contains(cd.SeriesName),
                             ToolTip = "#SERIESNAME - #VALY{#,0} €",
                             LabelFormat = "#,0"
                         };
                
                cs["DrawingStyle"] = "Cylinder";


                for (var i = 0; i < cd.Xvalue.Count; i++)
                {
                    cs.Points.AddXY(cd.Xvalue[i], cd.Yvalue[i]);
                }

                chrtForecastContribution.Series.Add(cs);

                var imageLocation = cd.Displayed ? Checkedboximagelocation : Uncheckedboximagelocation;

                var legendItem = new LegendItem {Name = "Legend Item"};
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

                chrtForecastContribution.Legends["RightLegend"].CustomItems.Add(legendItem);

                if (cd.Displayed == false)
                {
                    cs.IsVisibleInLegend = false;
                    cs.Enabled = false;

                }
            }
        }

        protected void chrtForecastContribution_Click(object sender, ImageMapEventArgs e)
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
                var legend = chrtForecastContribution.Legends["Legend1"];

                legend.Enabled = !GraphInformation.ShowingSeriesInformation;
                GraphInformation.ShowingSeriesInformation = !GraphInformation.ShowingSeriesInformation;

                return;
            }

            if (e.PostBackValue.StartsWith("HideLegend"))
            {
                ShowLegend = false;
                chrtForecastContribution.Legends["RightLegend"].Enabled = false;
                return;
            }
            if (e.PostBackValue.StartsWith("ShowLegend"))
            {
                ShowLegend = true;
                chrtForecastContribution.Legends["RightLegend"].Enabled = true;
                return;
            }
        }
    }
}