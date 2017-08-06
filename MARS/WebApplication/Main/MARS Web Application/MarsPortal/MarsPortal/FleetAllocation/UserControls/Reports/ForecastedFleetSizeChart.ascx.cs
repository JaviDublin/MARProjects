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
using Mars.FleetAllocation.DataAccess.Reporting.ForecastedFleetSize.Entities;

namespace Mars.FleetAllocation.UserControls.Reports
{
    public partial class ForecastedFleetSizeChart : UserControl
    {
        private const string ForecastedFleetSizeChartGraphInfo = "ForecastedFleetSizeChartGraphInfo";
        private const string ForecastedFleetSizeChartHiddenSeries = "ForecastedFleetSizeChartHiddenSeries";

        public bool ValuesChart
        {
            get { return bool.Parse(hfValues.Value); }
            set { hfValues.Value = value.ToString(); }
        }

        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[ForecastedFleetSizeChartGraphInfo];
            }
            set { Session[ForecastedFleetSizeChartGraphInfo] = value; }
        }

        public List<string> ShowSeries
        {
            set { Session[ForecastedFleetSizeChartHiddenSeries] = value; }
            get { return (List<string>)Session[ForecastedFleetSizeChartHiddenSeries]; }
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
                chrtForecastedFleetSize.Visible = true;
                BuildLineChart();
            }
            else
            {
                chrtForecastedFleetSize.Visible = false;
            }

        }

        public void chrtForecastedFleetSize_Click(object sender, ImageMapEventArgs e)
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
                var legend = chrtForecastedFleetSize.Legends["Legend1"];

                legend.Enabled = !GraphInformation.ShowingSeriesInformation;
                GraphInformation.ShowingSeriesInformation = !GraphInformation.ShowingSeriesInformation;

                return;
            }

            if (e.PostBackValue.StartsWith("HideLegend"))
            {
                ShowLegend = false;
                chrtForecastedFleetSize.Legends["RightLegend"].Enabled = false;
                return;
            }
            if (e.PostBackValue.StartsWith("ShowLegend"))
            {
                ShowLegend = true;
                chrtForecastedFleetSize.Legends["RightLegend"].Enabled = true;
                return;
            }
        }

        public void LoadChart(List<ForecastedFleetSizeEntity> setA, List<ForecastedFleetSizeEntity> setB)
        {
            if (ValuesChart)
            {
                LoadValuesChart(setA, setB);
            }
            else
            {
                LoadDifferenceChart(setA, setB);
            }
        }

        private void LoadDifferenceChart(IEnumerable<ForecastedFleetSizeEntity> setA, IEnumerable<ForecastedFleetSizeEntity> setB)
        {
            var seriesList = new List<GraphSeries>();

            var series = new List<string> { "Forecast Demand Gap", "Demand Gap Scenario A", "Demand Gap Scenario B" };

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

            foreach (var f in setA)
            {
                expectedSeries.Xvalue.Add(f.Week);
                expectedSeries.Yvalue.Add(((double)f.ExpectedFleet - f.UnConstrained));
                expectedWithAddsA.Xvalue.Add(f.Week);
                expectedWithAddsA.Yvalue.Add((double)f.ExpectedWithAdditionPlan - f.UnConstrained);
            }


            foreach (var f in setB)
            {
                expectedWithAddsB.Xvalue.Add(f.Week);
                expectedWithAddsB.Yvalue.Add((double)f.ExpectedWithAdditionPlan - f.UnConstrained);
            }


            seriesList.Add(expectedSeries);
            seriesList.Add(expectedWithAddsA);
            seriesList.Add(expectedWithAddsB);

            GraphInformation.SeriesData = seriesList;

        }
        private void LoadValuesChart(IEnumerable<ForecastedFleetSizeEntity> setA, IEnumerable<ForecastedFleetSizeEntity> setB)
        {
            var seriesList = new List<GraphSeries>();
         
            var series = new List<string> {"Expected", "Unconstrained", "Nessesary"
                    , "Max A", "Min A", "Expected With Adds A", "Max B", "Min B", "Expected With Adds B"};

            ShowSeries.AddRange(series.ToArray());

            var expectedSeries = new GraphSeries(series[0])
            {
                GraphColour = Color.DarkGreen,
                Displayed = true
            };

            var unconstrainedSeries = new GraphSeries(series[1])
            {
                GraphColour = Color.CornflowerBlue,
                Displayed = true
            };

            var nessesarySeries = new GraphSeries(series[2])
            {
                GraphColour = Color.Brown,
                Displayed = true
            };

            var maxSeriesA = new GraphSeries(series[3])
            {
                GraphColour = Color.Magenta,
                Displayed = false
            };

            GraphInformation.HiddenSeriesNames.Add(series[3]);

            var minSeriesA = new GraphSeries(series[4])
            {
                GraphColour = Color.Magenta,
                Displayed = false
            };
            GraphInformation.HiddenSeriesNames.Add(series[4]);

            var expectedWithAddsA = new GraphSeries(series[5])
            {
                GraphColour = Color.DarkOrchid,
                Displayed = true
            };


            var maxSeriesB = new GraphSeries(series[6])
            {
                GraphColour = Color.Orange,
                Displayed = false
            };
            GraphInformation.HiddenSeriesNames.Add(series[6]);


            var minSeriesB = new GraphSeries(series[7])
            {
                GraphColour = Color.Orange,
                Displayed = false
            };
            GraphInformation.HiddenSeriesNames.Add(series[7]);

            var expectedWithAddsB = new GraphSeries(series[8])
            {
                GraphColour = Color.OrangeRed,
                Displayed = true
            };

            foreach (var f in setA)
            {
                expectedSeries.Xvalue.Add(f.Week);
                unconstrainedSeries.Xvalue.Add(f.Week);
                nessesarySeries.Xvalue.Add(f.Week);
                maxSeriesA.Xvalue.Add(f.Week);
                minSeriesA.Xvalue.Add(f.Week);

                expectedSeries.Yvalue.Add((double) f.ExpectedFleet);
                unconstrainedSeries.Yvalue.Add(f.UnConstrained);
                nessesarySeries.Yvalue.Add(f.Nessesary);
                maxSeriesA.Yvalue.Add(f.MaxFleet);
                minSeriesA.Yvalue.Add(f.MinFleet);
                expectedWithAddsA.Xvalue.Add(f.Week);
                expectedWithAddsA.Yvalue.Add((double) f.ExpectedWithAdditionPlan);
            }


            foreach (var f in setB)
            {
                maxSeriesB.Xvalue.Add(f.Week);
                minSeriesB.Xvalue.Add(f.Week);
                maxSeriesB.Yvalue.Add(f.MaxFleet);
                minSeriesB.Yvalue.Add(f.MinFleet);
                expectedWithAddsB.Xvalue.Add(f.Week);
                expectedWithAddsB.Yvalue.Add((double) f.ExpectedWithAdditionPlan);
            }


            seriesList.Add(expectedSeries);
            seriesList.Add(unconstrainedSeries);
            seriesList.Add(nessesarySeries);
            seriesList.Add(maxSeriesA);
            seriesList.Add(minSeriesA);
            seriesList.Add(maxSeriesB);
            seriesList.Add(minSeriesB);
            seriesList.Add(expectedWithAddsA);
            seriesList.Add(expectedWithAddsB);

            GraphInformation.SeriesData = seriesList;
        }


        private void BuildLineChart()
        {
            var ca = new ChartArea { Name = "Forecasted Fleet Size" };

            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisY.MajorGrid.Enabled = true;
            ca.AxisY.MajorGrid.LineColor = Color.LightGray;
            ca.AxisX.MajorGrid.Enabled = true;
            ca.AxisX.MajorGrid.LineColor = Color.LightGray;
            ca.AxisX.MajorGrid.LineWidth = 1;

            chrtForecastedFleetSize.ChartAreas.Add(ca);

            foreach (var cd in GraphInformation.SeriesData)
            {
                var cs = new Series(cd.SeriesName)
                {
                    ChartType = ValuesChart ? SeriesChartType.Line : SeriesChartType.Column,
                    Color = cd.GraphColour,
                    BorderWidth = 2,
                    XValueType = ChartValueType.Int32,
                    IsValueShownAsLabel = GraphInformation.ShowLabelSeriesNames.Contains(cd.SeriesName),
                    ToolTip = "#SERIESNAME - #VALY{#,0}",
                    LabelFormat = "#,0"
                };
                if (!ValuesChart)
                {
                    cs["DrawingStyle"] = "Cylinder";
                }

                for (var i = 0; i < cd.Xvalue.Count; i++)
                {
                    cs.Points.AddXY(cd.Xvalue[i], cd.Yvalue[i]);
                }

                chrtForecastedFleetSize.Series.Add(cs);

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

                var legendCell3Colour = !valuesShown || seriesHidden ? MarsColours.ChartLegendValuesHidden : MarsColours.ChartLegendValuesShown;


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

                chrtForecastedFleetSize.Legends["RightLegend"].CustomItems.Add(legendItem);

                if (cd.Displayed == false)
                {
                    cs.IsVisibleInLegend = false;
                    cs.Enabled = false;

                }
            }

            var slideLegendItem = new LegendItem { Name = "SlideButton" };
            var slideLegendCell = new LegendCell
            {
                Name = "Cell4",
                Alignment = ContentAlignment.TopRight,
                PostBackValue = ShowLegend ? "HideLegend" : "ShowLegend",
                CellType = LegendCellType.Image,
                Image = ShowLegend ? "~/App.Images/hideRightIcon.gif" : "~/App.Images/hideLeftIcon.gif",

            };
            slideLegendItem.Cells.Add(slideLegendCell);
            chrtForecastedFleetSize.Legends["SlideLegend"].CustomItems.Add(slideLegendItem);
        }


    }
}