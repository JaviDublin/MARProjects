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
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4.Availability
{
    public partial class HistoricalTrendChart : UserControl
    {
        private const string HistoricalTrendChartGraphInfo = "HistoricalTrendChartGraphInfo";
        private const string HistoricalTrendHiddenSeries = "HistoricalTrendHiddenSeries";

        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[HistoricalTrendChartGraphInfo];
            }
            set { Session[HistoricalTrendChartGraphInfo] = value; }
        }

        public bool HourlySeries
        {
            get { return bool.Parse(hfHourlySeries.Value); }
            set { hfHourlySeries.Value = value.ToString(); }

        }

        public List<string> ShowSeries
        {
            set { Session[HistoricalTrendHiddenSeries] = value; }
            get { return (List<string>) Session[HistoricalTrendHiddenSeries]; }
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
                BuildLineChart();
            }

        }

        public void LoadChart(List<FleetStatusRow> fsd, PercentageDivisorType percentType)
        {
            var seriesList = new List<GraphSeries>();
            hfPercentageValues.Value = percentType.ToString();

            var topics = Enum.GetValues(typeof(AvailabilityTopic));

            ShowSeries.Add(TopicTranslation.GetAvailabilityTopicDescription(AvailabilityTopic.Utilization));
            ShowSeries.Add(TopicTranslation.GetAvailabilityTopicDescription(AvailabilityTopic.OnRent));

            foreach (AvailabilityTopic t in topics)
            {
                
                if (percentType == PercentageDivisorType.Values)
                {
                    if (t == AvailabilityTopic.Utilization || t == AvailabilityTopic.UtilizationInclOverdue)
                    {
                        
                        continue;
                        
                    }
                }
                else
                {
                    if (t == AvailabilityTopic.OnRent)
                    {
                        
                        continue;
                    }
                }
                var seriesName = TopicTranslation.GetAvailabilityTopicDescription(t);
                var displaySeries = false;
                if (ShowSeries.Count == 0)
                {
                    if (t == AvailabilityTopic.Utilization || t == AvailabilityTopic.OnRent)
                    {
                        displaySeries = true;
                        
                    }
                }
                else
                {
                    displaySeries = ShowSeries.Contains(seriesName);
                }

                var gs = new GraphSeries(seriesName)
                {
                    GraphColour = TopicTranslation.GetAvailabilityColour(t),
                    Displayed = displaySeries
                };
                foreach (var f in fsd)
                {
                    gs.Xvalue.Add(f.Day);

                    var yVal = percentType == PercentageDivisorType.Values ? f.GetValue(t) : f.GetValuePercent(t);

                    gs.Yvalue.Add(yVal);

                }
                seriesList.Add(gs);
            }

            GraphInformation.SeriesData = seriesList;
        }

        private void BuildLineChart()
        {
            var ca = new ChartArea { Name = "Historical Trend" };

            ca.AxisX.MajorGrid.Enabled = false;
            //ca.AxisX.Interval = 1;
            ca.AxisX.IntervalType = HourlySeries ? DateTimeIntervalType.Hours : DateTimeIntervalType.Days;
           
            //ca.AxisX.LabelStyle.Enabled = true;
            //ca.AxisX.IsLabelAutoFit = true;
            //ca.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep30;

            ca.AxisY.MajorGrid.Enabled = true;
            ca.AxisY.MajorGrid.LineColor = Color.LightGray;

            ca.AxisX.MajorGrid.Enabled = true;
            ca.AxisX.MajorGrid.LineColor = Color.LightGray;
            ca.AxisX.MajorGrid.LineWidth = 1;
            

            var percentageValueSelected = (PercentageDivisorType) Enum.Parse(typeof(PercentageDivisorType), hfPercentageValues.Value);


            chrtHistoricalTrend.ChartAreas.Add(ca);
            chrtHistoricalTrend.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chrtHistoricalTrend.ChartAreas[0].AxisX.LabelStyle.Format = HourlySeries ? "HH:mm:00" : "dd/MM/yyyy";
            chrtHistoricalTrend.ChartAreas[0].AxisY.LabelStyle.Format =
                percentageValueSelected == PercentageDivisorType.Values ? "#,0" : "P";

            
            foreach (var cd in GraphInformation.SeriesData)
            {

                var cs = new Series(cd.SeriesName)
                         {
                             ChartType = SeriesChartType.Line,
                             Color = cd.GraphColour,
                             BorderWidth = 2,
                             XValueType = ChartValueType.DateTime,
                             IsValueShownAsLabel = GraphInformation.ShowLabelSeriesNames.Contains(cd.SeriesName),
                             ToolTip = "#SERIESNAME - #VALX{dd/MM/yyyy" + (HourlySeries ? " HH:mm:00" : string.Empty) + 
                                    "} - #VALY{" + (percentageValueSelected == PercentageDivisorType.Values ? "#,0" : "P") + "}",
                             LabelFormat = percentageValueSelected == PercentageDivisorType.Values ? "#,0" : "P"
                         };

                for (var i = 0; i < cd.Xvalue.Count; i ++)
                {
                    cs.Points.AddXY(cd.Xvalue[i], cd.Yvalue[i]);    
                }
                
                

                chrtHistoricalTrend.Series.Add(cs);


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

                chrtHistoricalTrend.Legends["RightLegend"].CustomItems.Add(legendItem);
                //


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
            chrtHistoricalTrend.Legends["SlideLegend"].CustomItems.Add(slideLegendItem);
        }

        protected void chrtHistoricalTrend_Click(object sender, ImageMapEventArgs e)
        {
            if (e.PostBackValue.StartsWith("HideLegend"))
            {
                ShowLegend = false;
                chrtHistoricalTrend.Legends["RightLegend"].Enabled = false;
                return;
            }
            if (e.PostBackValue.StartsWith("ShowLegend"))
            {
                ShowLegend = true;
                chrtHistoricalTrend.Legends["RightLegend"].Enabled = true;
                return;
            }
            if (e.PostBackValue.StartsWith("LegendClick"))
            {
                var seriesName = e.PostBackValue.Split('/')[1];
                var gs = GraphInformation.SeriesData.Find(s => s.SeriesName == seriesName);
                gs.Displayed = !gs.Displayed;

                ShowSeries.AddOrRemoveString(gs.SeriesName);
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

        }
    }
}