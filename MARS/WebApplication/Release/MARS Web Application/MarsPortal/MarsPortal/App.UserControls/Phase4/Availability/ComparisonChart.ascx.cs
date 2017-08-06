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
    public partial class ComparisonChart : UserControl
    {
        private const string SiteComparisonChartGraphInfo = "SiteComparisonChartGraphInfo";
        private const string FleetComparisonChartGraphInfo = "FleetComparisonChartGraphInfo";

        private const string SiteComparisonHiddenSeries = "SiteComparisonHiddenSeries";
        private const string FleetComparisonHiddenSeries = "FleetComparisonHiddenSeries";

        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[SiteComparison ? SiteComparisonChartGraphInfo : FleetComparisonChartGraphInfo];
            }
            set { Session[SiteComparison ? SiteComparisonChartGraphInfo : FleetComparisonChartGraphInfo] = value; }
        }

        private bool ShowLegend
        {
            get { return bool.Parse(hfShowLegend.Value); }
            set { hfShowLegend.Value = value.ToString(); }
        }

        private List<string> ShowSeries
        {
            get
            {
                return (List<string>)Session[SiteComparison ? SiteComparisonHiddenSeries : FleetComparisonHiddenSeries];
            }
            set { Session[SiteComparison ? SiteComparisonHiddenSeries : FleetComparisonHiddenSeries] = value; }
        }

        public bool SiteComparison
        {
            get { return bool.Parse(hfSiteComparison.Value); }
            set { hfSiteComparison.Value = value.ToString(); }
        }

        public const string Checkedboximagelocation = "~/App.Images/TickBoxTicked.png";
        public const string Uncheckedboximagelocation = "~/App.Images/TickBox.png";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (ShowSeries == null)
            {
                var defaultSeriesName =
                    TopicTranslation.GetAvailabilityTopicDescription(AvailabilityTopic.Utilization);
                ShowSeries = new List<string> { defaultSeriesName };
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
                BuildColumnChart();
            }

         
        }

        public void LoadChart(List<FleetStatusRow> fsd, PercentageDivisorType percentType)
        {
            var seriesList = new List<GraphSeries>();
            hfPercentageValues.Value = percentType.ToString();

            var topics = Enum.GetValues(typeof(AvailabilityTopic));

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
                    GraphColour = Classes.BLL.Common.Charts.GetColourForColumn(
                            TopicTranslation.GetAvailabilityTopicShortDescription(t)    
                            ,3),
                    Displayed = displaySeries
                };
                foreach (var f in fsd)
                {
                    gs.Xvalue.Add(f.Key);

                    var yVal = percentType == PercentageDivisorType.Values ? f.GetValue(t) : f.GetValuePercent(t);

                    gs.Yvalue.Add(yVal);

                }
                seriesList.Add(gs); 
            }

            GraphInformation.SeriesData = seriesList;
        }

        private void BuildColumnChart()
        {
            var ca = new ChartArea { Name = "Comparison Chart" };
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisX.IntervalType = DateTimeIntervalType.Days;
            
            ca.AxisY.MajorGrid.Enabled = true;
            ca.AxisY.MajorGrid.LineColor = Color.LightGray;

            var percentTypeSelected = (PercentageDivisorType)Enum.Parse(typeof(PercentageDivisorType), hfPercentageValues.Value); 
            //bool.Parse(hfPercentageCalculation.Value);


            chrtComparison.ChartAreas.Add(ca);
            chrtComparison.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chrtComparison.ChartAreas[0].AxisY.LabelStyle.Format = 
                    percentTypeSelected == PercentageDivisorType.Values ? "#,0" : "P";


            foreach (var cd in GraphInformation.SeriesData)
            {

                var cs = new Series(cd.SeriesName)
                {
                    ChartType = SeriesChartType.Column,
                    Color = cd.GraphColour,
                    XValueType = ChartValueType.Date,
                    
                    IsValueShownAsLabel = GraphInformation.ShowLabelSeriesNames.Contains(cd.SeriesName),
                    ToolTip = "#SERIESNAME - #VALY{" + (percentTypeSelected == PercentageDivisorType.Values ? "#,0" : "P") + "}",
                    LabelFormat = percentTypeSelected == PercentageDivisorType.Values ? "#,0" : "P"
                };
                cs["DrawingStyle"] = "Emboss";          //LightToDark, Cylinder, Emboss, Wedge
                cs["LabelStyle"] = "Top";
                for (var i = 0; i < cd.Xvalue.Count; i++)
                {
                    cs.Points.AddXY(cd.Xvalue[i], cd.Yvalue[i]);
                }

                

                chrtComparison.Series.Add(cs);


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

                var legendCell2Colour = cd.GraphColour;

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

                chrtComparison.Legends["RightLegend"].CustomItems.Add(legendItem);
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
            chrtComparison.Legends["SlideLegend"].CustomItems.Add(slideLegendItem);
        }

        protected void chrtFleetComparison_Click(object sender, ImageMapEventArgs e)
        {
            if (e.PostBackValue.StartsWith("HideLegend"))
            {
                ShowLegend = false;
                chrtComparison.Legends["RightLegend"].Enabled = false;
                return;
            }
            if (e.PostBackValue.StartsWith("ShowLegend"))
            {
                ShowLegend = true;
                chrtComparison.Legends["RightLegend"].Enabled = true;
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