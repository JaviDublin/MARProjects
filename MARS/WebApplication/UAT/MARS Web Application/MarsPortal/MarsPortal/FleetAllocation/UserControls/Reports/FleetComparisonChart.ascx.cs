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
using Mars.FleetAllocation.BusinessLogic;
using Mars.FleetAllocation.DataAccess.Reporting.FleetComparison.Entities;
using Mars.FleetAllocation.DataAccess.Reporting.ForecastedFleetSize.Entities;


namespace Mars.FleetAllocation.UserControls.Reports
{
    public partial class FleetComparisonChart : UserControl
    {
        private const string FaoFleetComparisonGraphInfo = "FaoFleetComparisonGraphInfo";
        private const string FaoFleetComparisonHiddenSeries = "FaoFleetComparisonHiddenSeries";

        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[FaoFleetComparisonGraphInfo];
            }
            set { Session[FaoFleetComparisonGraphInfo] = value; }
        }

        public List<string> ShowSeries
        {
            set { Session[FaoFleetComparisonHiddenSeries] = value; }
            get { return (List<string>)Session[FaoFleetComparisonHiddenSeries]; }
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
                chrtFleetComparison.Visible = true;
                BuildLineChart();
            }
            else
            {
                chrtFleetComparison.Visible = false;
            }
        }

        public void chrtForecastedFleetSize_Click(object sender, EventArgs e)
        {

        }

        public void LoadChart(List<FleetComparisonEntity> comparisonData)
        {
            if (!comparisonData.Any()) return;

            var series = comparisonData.Select(d => d.FleetGroupName).Distinct().ToList();

            ShowSeries.AddRange(series.ToArray());

            var colourNumber = 0;
            var seriesToGraph = new List<GraphSeries>();
            foreach (var s in series)
            {
                seriesToGraph.Add(new GraphSeries(s)
                                  {
                                      GraphColour = ColorTranslator.FromHtml("#" + GraphingColours.ColourValues[colourNumber]),
                                      Displayed = true
                                  });
                colourNumber++;
            }


            var minYear = comparisonData.Min(d => d.Year);
            var minWeekYear = comparisonData.Where(d => d.Year == minYear).Min(d => d.Week);

            var maxYear = comparisonData.Max(d => d.Year);
            var maxWeekYear = comparisonData.Where(d => d.Year == maxYear).Max(d => d.Week);

            int currentWeek = minWeekYear;
            int currentYear = minYear;
            do
            {
                foreach (var stg in seriesToGraph)
                {
                    stg.Xvalue.Add(currentWeek);
                    var dataEntry = comparisonData.FirstOrDefault(d => d.FleetGroupName == stg.SeriesName
                                                                       && d.Year == currentYear
                                                                       && d.Week == currentWeek);
                    int yVal = 0;
                    if (dataEntry != null)
                    {
                        yVal = dataEntry.Additions;
                    }
                    stg.Yvalue.Add(yVal);
                }

                currentWeek++;
                if (currentWeek == 13)
                {
                    currentWeek = 1;
                    currentYear++;
                }
            } while (currentWeek != maxWeekYear && currentYear != maxYear);

            GraphInformation.SeriesData = seriesToGraph;
        }

        private void BuildLineChart()
        {
            var ca = new ChartArea { Name = "Fleet Comparison" };

            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;


            //ca.AxisX.LabelStyle.Enabled = true;
            //ca.AxisX.IsLabelAutoFit = true;
            //ca.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep30;

            ca.AxisY.MajorGrid.Enabled = true;
            ca.AxisY.MajorGrid.LineColor = Color.LightGray;

            ca.AxisX.MajorGrid.Enabled = true;
            ca.AxisX.MajorGrid.LineColor = Color.LightGray;
            ca.AxisX.MajorGrid.LineWidth = 1;

            chrtFleetComparison.ChartAreas.Add(ca);
            
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

                chrtFleetComparison.Series.Add(cs);

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


                legendCell = new LegendCell
                {
                    Name = "Cell3",
                    Text = "Values",
                    ForeColor = legendCell2Colour,
                    Font = new Font("Tahoma", 9),
                    Alignment = ContentAlignment.MiddleRight,
                    PostBackValue = "LegendShowLabels/" + cd.SeriesName

                };
                legendItem.Cells.Add(legendCell);

                chrtFleetComparison.Legends["RightLegend"].CustomItems.Add(legendItem);

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
            chrtFleetComparison.Legends["SlideLegend"].CustomItems.Add(slideLegendItem);
        }

        protected void chrtFleetComparison_Click(object sender, ImageMapEventArgs e)
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
                var legend = chrtFleetComparison.Legends["Legend1"];

                legend.Enabled = !GraphInformation.ShowingSeriesInformation;
                GraphInformation.ShowingSeriesInformation = !GraphInformation.ShowingSeriesInformation;

                return;
            }

            if (e.PostBackValue.StartsWith("HideLegend"))
            {
                ShowLegend = false;
                chrtFleetComparison.Legends["RightLegend"].Enabled = false;
                return;
            }
            if (e.PostBackValue.StartsWith("ShowLegend"))
            {
                ShowLegend = true;
                chrtFleetComparison.Legends["RightLegend"].Enabled = true;
                return;
            }
        }
    }
}