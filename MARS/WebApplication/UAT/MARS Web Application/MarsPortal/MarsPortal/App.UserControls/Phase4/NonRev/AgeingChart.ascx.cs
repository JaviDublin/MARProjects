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
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class AgeingChart : UserControl
    {
        private const string AgeingChartGraphInfo = "AgeingChartGraphInfo";

        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[AgeingChartGraphInfo];
            }
            set { Session[AgeingChartGraphInfo] = value; }
        }


        public const string Checkedboximagelocation = "~/App.Images/Checked.jpg";
        public const string Uncheckedboximagelocation = "~/App.Images/Unchecked.jpg";



        protected void Page_Load(object sender, EventArgs e)
        {
            if (GraphInformation == null)
                GraphInformation = new GraphData();
            

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (GraphInformation.SeriesData != null)
            {
                BuildTotalCylinderChart();    
            }
            
            chrtAge.Legends["RightLegend"].Enabled = cbLegend.Checked;
        }

        public void LoadChart(List<AgeingRow> comparisonData)
        {
            comparisonData.RemoveAll(d => d.Key == NonRevBaseDataAccess.TotalKeyName);
            var dataList = GenerateAgingGraphingData(comparisonData);
            GraphInformation.SeriesData = dataList;
        }


        private void BuildTotalCylinderChart()
        {
            
            var ca = new ChartArea { Name = "Total Fleet Area" };
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisY.MajorGrid.Enabled = false;
            ca.AxisY.Name = "Non Revenue Vehicles";

            

            chrtAge.ChartAreas.Add(ca);

            foreach (var cd in GraphInformation.SeriesData)
            {
                var cs = new Series(cd.SeriesName) { ChartType = SeriesChartType.Line, Color = cd.GraphColour };
                //cs["DrawingStyle"] = "Cylinder";

                cs.IsValueShownAsLabel = GraphInformation.ShowLabelSeriesNames.Contains(cd.SeriesName);
                cs.LabelFormat = "#,#";

                

                for (int i = 0; i < cd.Xvalue.Count; i++)
                {
                    cs.Points.AddXY(cd.Xvalue[i], cd.Yvalue[i]);
                }
                chrtAge.Series.Add(cs);
                

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
                    ForeColor = legendCell2Colour
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
                    PostBackValue = "LegendShowLabels/" + cd.SeriesName
                };
                legendItem.Cells.Add(legendCell);

                chrtAge.Legends["RightLegend"].CustomItems.Add(legendItem);
                //


                if (cd.Displayed == false)
                {
                    cs.IsVisibleInLegend = false;
                    cs.Enabled = false;
                    
                }

            }

            var leftLegendItem = new LegendItem { Name = "Legend Item Left" };

            var leftLegendCell = new LegendCell
            {
                Name = "RightCell",
                Text = "Non Rev Vehicles",
                Font = new Font("Tahoma", 9)
            };
            leftLegendItem.Cells.Add(leftLegendCell);

            chrtAge.Legends["LeftLegend"].CustomItems.Add(leftLegendItem);
        }

        protected void chrtAge_Click(object sender, ImageMapEventArgs e)
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

        }

        private static List<double> ExtractGroupValues(AgeingRow ar)
        {
            var returned = new List<double> {ar.Group1, ar.Group2, ar.Group3, ar.Group4, ar.Group5, ar.Group6, ar.Group7, ar.Group8, ar.Group8, ar.Group9};
            return returned;
        }

        private static List<GraphSeries> GenerateAgingGraphingData(List<AgeingRow> data)
        {
            var returned = new List<GraphSeries>();
            var groupKeys = new List<object>();
            var group1 = new List<double>();
            var group2 = new List<double>();
            var group3 = new List<double>();
            var group4 = new List<double>();
            var group5 = new List<double>();
            var group6 = new List<double>();
            var group7 = new List<double>();
            var group8 = new List<double>();
            var group9 = new List<double>();

            groupKeys.Add("0-2");
            groupKeys.Add("3");
            groupKeys.Add("4-6");
            groupKeys.Add("7");
            groupKeys.Add("8-10");
            groupKeys.Add("11-15");
            groupKeys.Add("16-30");
            groupKeys.Add("31-60");
            groupKeys.Add("60+");

            foreach (var d in data)
            {
                
                group1.Add(d.Group1);
                group2.Add(d.Group2);
                group3.Add(d.Group3);
                group4.Add(d.Group4);
                group5.Add(d.Group5);
                group6.Add(d.Group6);
                group7.Add(d.Group7);
                group8.Add(d.Group8);
                group9.Add(d.Group9);
            }

            var colourList = new List<Color>();
            var startingColour = Color.Blue;
            colourList.Add(startingColour);
            
            
            for (var i = 0; i < data.Count; i++)
            {
                
                colourList.Add(Color.FromArgb((i * 255 / data.Count), 0, 255 - (i * 255 / data.Count)));
                returned.Add(new GraphSeries(data[i].Key)
                {
                    GraphColour = Classes.BLL.Common.Charts.GetColourForColumn(data[i].Key),
                    Xvalue = groupKeys,
                    Yvalue = ExtractGroupValues(data[i])
                });
            }

            return returned;
        }
    }
}