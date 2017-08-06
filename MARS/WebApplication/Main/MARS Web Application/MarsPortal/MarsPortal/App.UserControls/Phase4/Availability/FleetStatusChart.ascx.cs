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
using Mars.Pooling.HTMLFactories;
using Resources;

namespace Mars.App.UserControls.Phase4.Availability
{
    public partial class FleetStatusChart : UserControl
    {
        private const string FleetStatusChartGraphInfo = "FleetStatusChartGraphInfo";
        public const string RedirectToCarSearch = "RedirectToCarSearch";

        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[FleetStatusChartGraphInfo];
            }
            set { Session[FleetStatusChartGraphInfo] = value; }
        }

        public const string Checkedboximagelocation = "~/App.Images/TickBoxTicked.png";
        public const string Uncheckedboximagelocation = "~/App.Images/TickBox.png";

        private bool ShowLegend
        {
            get { return bool.Parse(hfShowLegend.Value); }
            set { hfShowLegend.Value = value.ToString(); }
        }

        public bool TodaysData
        {
            get { return bool.Parse(hfTodaysData.Value); }
            set { hfTodaysData.Value = value.ToString(); }
        }

        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GraphInformation == null)
            {
                GraphInformation = new GraphData();
                
            }
                
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (GraphInformation.SeriesData != null)
            {
                BuildWaterfallChart();
            }

            
        }

        public void LoadChart(FleetStatusRow fsd)
        {
            var seriesList = new List<GraphSeries>();
            double startingPoint = 0;
            var series = AddWaterfallSeries(AvailabilityTopic.TotalFleet
                                 , fsd.TotalFleet,   fsd.TotalFleet);
            seriesList.Add(series);

            series = AddWaterfallSeries(AvailabilityTopic.Cu
                                , fsd.TotalFleet - startingPoint, fsd.Cu, false);
            startingPoint += fsd.Cu;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Ha
                , fsd.TotalFleet - startingPoint, fsd.Ha, false);
            startingPoint += fsd.Ha;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Hl
                , fsd.TotalFleet - startingPoint, fsd.Hl, false);
            startingPoint += fsd.Hl;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Nc
                , fsd.TotalFleet - startingPoint, fsd.Nc, false);
            startingPoint += fsd.Nc;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Pl
                , fsd.TotalFleet - startingPoint, fsd.Pl, false);
            startingPoint += fsd.Pl;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Tc
                , fsd.TotalFleet - startingPoint, fsd.Tc, false);
            startingPoint += fsd.Tc;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Sv
                , fsd.TotalFleet - startingPoint, fsd.Sv, false);
            startingPoint += fsd.Sv;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Ws
                , fsd.TotalFleet - startingPoint, fsd.Ws, false);
            startingPoint += fsd.Ws;
            seriesList.Add(series);

            series = AddWaterfallSeries(AvailabilityTopic.OperationalFleet
                , fsd.OperationalFleet, fsd.OperationalFleet);
            startingPoint = 0;
            seriesList.Add(series);

            series = AddWaterfallSeries(AvailabilityTopic.Bd
                , fsd.OperationalFleet - startingPoint, fsd.Bd);
            startingPoint += fsd.Bd;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Mm
                , fsd.OperationalFleet - startingPoint, fsd.Mm);
            startingPoint += fsd.Mm;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Tw
                , fsd.OperationalFleet - startingPoint, fsd.Tw, false);
            startingPoint += fsd.Tw;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Tb
                , fsd.OperationalFleet - startingPoint, fsd.Tb);
            startingPoint += fsd.Tb;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Fs
                , fsd.OperationalFleet - startingPoint, fsd.Fs, false);
            startingPoint += fsd.Fs;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Rl
                , fsd.OperationalFleet - startingPoint, fsd.Rl, false);
            startingPoint += fsd.Rl;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Rp
                , fsd.OperationalFleet - startingPoint, fsd.Rp, false);
            startingPoint += fsd.Rp;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Tn
                , fsd.OperationalFleet - startingPoint, fsd.Tn, false);
            startingPoint += fsd.Tn;
            seriesList.Add(series);

            series = AddWaterfallSeries(AvailabilityTopic.AvailableFleet
                , fsd.AvailableFleet, fsd.AvailableFleet);
            startingPoint = 0;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Idle
                , fsd.AvailableFleet - startingPoint, fsd.Idle);
            startingPoint += fsd.Idle;
            seriesList.Add(series);
            series = AddWaterfallSeries(AvailabilityTopic.Su
                , fsd.AvailableFleet - startingPoint, fsd.Su);
            startingPoint += fsd.Su;
            seriesList.Add(series);

            series = AddWaterfallSeries(AvailabilityTopic.Overdue
                , fsd.AvailableFleet - startingPoint, fsd.Overdue);
            startingPoint += fsd.Overdue;
            seriesList.Add(series);

            series = AddWaterfallSeries(AvailabilityTopic.OnRent
                , fsd.OnRent, fsd.OnRent);
            seriesList.Add(series);
            

            GraphInformation.SeriesData = seriesList;
        }



        private GraphSeries AddWaterfallSeries(AvailabilityTopic series, double total, double value, bool show = true)
        {

            var seriesName = TopicTranslation.GetAvailabilityTopicDescription(series);
            var gs = new GraphSeries(seriesName)
                     {
                         GraphColour = TopicTranslation.GetAvailabilityColour(series),
                         Displayed = show
                     };
            gs.Xvalue.Add(seriesName);
            
            gs.Yvalue.Add(total);              
            gs.Yvalue.Add(total - value);    
            return gs;
        }


        
        private void BuildWaterfallChart()
        {
            
            var ca = new ChartArea { Name = "Total Fleet Area" };
            var pos = new ElementPosition { Width = ShowLegend ? 70 : 90, Height = 90, X = 0, Y = 5 };
            
            
            
            ca.Position = pos;
            
            ca.AxisX.MajorGrid.Enabled = false;

            //ca.AxisX.CustomLabels.Add(new CustomLabel());
            
            
            ca.AxisX.Interval = 2;
            //ca.AxisX.LabelStyle.Angle = -45;
            
            ca.AxisY.MajorGrid.Enabled = true;
            var ls = new LabelStyle {Format = "#,#"};
            ca.AxisY.LabelStyle = ls ;
            ca.AxisY.MajorGrid.LineColor = Color.LightGray;
            //ca.InnerPlotPosition.Width = 55;

            


            chrtFleetStatus.ChartAreas.Add(ca);
            var totalFleet = 0.0;
            foreach (var cd in GraphInformation.SeriesData)
            {
                
                var cs = new Series(cd.SeriesName) { ChartType = SeriesChartType.RangeColumn
                        , Color = cd.GraphColour
                        , XValueType = ChartValueType.String
                        , YValuesPerPoint = 2 
                        };
                //cs["DrawSideBySide"] = "false";
                cs["DrawingStyle"] = "Emboss";

                if (TodaysData)
                {
                    cs.PostBackValue = "BarClick/" + cd.SeriesName;
                }
                
                cs.IsValueShownAsLabel = GraphInformation.ShowLabelSeriesNames.Contains(cd.SeriesName);
                cs.LabelFormat = "#,#";
                //cs.Label = cd.SeriesName;
                //cs.LabelAngle = -45;
                
                cs.ToolTip = "#SERIESNAME #CUSTOMPROPERTY(SIZE)";
                
                cs.Points.AddXY(cd.SeriesName, cd.Yvalue.Cast<object>().ToArray());
                //cs.Label = string.Format("{0:#,#}", cd.Yvalue[0] - cd.Yvalue[1]);
                //cs.LabelAngle = -45;
                cs.AxisLabel = cd.SeriesName;
                //cs.Points.AddXY(cd.SeriesName, cd.Yvalue[0]);
                var size = cd.Yvalue[0] - cd.Yvalue[1];

                
                var availTopic  = TopicTranslation.GetAvailabilityTopicFromDescription(cd.SeriesName);
                if (availTopic == AvailabilityTopic.TotalFleet)
                {
                    totalFleet = cd.Yvalue[0];
                }

                cs.Points[0]["SIZE"] = string.Format("{0:#,#}", size);
                cs["PixelPointWidth"] = "400";
                
                chrtFleetStatus.Series.Add(cs);


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

                var legendCell2Colour = cd.GraphColour;//seriesHidden ? MarsColours.ChartLegendValuesHidden : cd.GraphColour;

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
                    Text = string.Format("{0:#,0}",size),
                    ForeColor = legendCell2Colour,
                    Font = new Font("Tahoma", 9),
                    Alignment = ContentAlignment.MiddleRight,
                    //PostBackValue = "LegendShowLabels/" + cd.SeriesName
                };
                
                legendItem.Cells.Add(legendCell);

                legendCell = new LegendCell
                {
                    Name = "Cell4",
                    Text = string.Format("{0:p}", (size / totalFleet)),
                    ForeColor = legendCell2Colour,
                    Font = new Font("Tahoma", 9),
                    Alignment = ContentAlignment.MiddleRight,
                };

                legendItem.Cells.Add(legendCell);

                chrtFleetStatus.Legends["RightLegend"].CustomItems.Add(legendItem);
                
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
            chrtFleetStatus.Legends["SlideLegend"].CustomItems.Add(slideLegendItem);


        }


        protected void chrtFleetStatus_Click(object sender, ImageMapEventArgs e)
        {
            if (e.PostBackValue.StartsWith("HideLegend"))
            {
                ShowLegend = false;
                chrtFleetStatus.Legends["RightLegend"].Enabled = false;
                return;
            }
            if (e.PostBackValue.StartsWith("ShowLegend"))
            {
                ShowLegend = true;
                chrtFleetStatus.Legends["RightLegend"].Enabled = true;
                return;
            }

            if (e.PostBackValue.StartsWith("BarClick"))
            {
                var seriesName = e.PostBackValue.Split('/')[1];
                var args = new CommandEventArgs(RedirectToCarSearch, seriesName);
                RaiseBubbleEvent(this, args);
                return;
            }
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

        }
    }
}