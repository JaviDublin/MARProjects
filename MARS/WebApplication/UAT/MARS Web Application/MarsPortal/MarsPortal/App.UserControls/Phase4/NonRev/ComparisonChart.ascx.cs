using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using App.BLL.ExtensionMethods;
using App.Styles;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;
using App.Entities.Graphing;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class ComparisonChart : UserControl
    {
        private const string SiteCompGraphInfo = "SiteCompGraphInfo";

        private string SessionStringName
        {
            get { return hfSessionString.Value; }
            set { hfSessionString.Value = value; }
        }

        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[SessionStringName];
            }
            set { Session[SessionStringName] = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {

            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (hfSessionString.Value == string.Empty) return;
            BuildTotalCylinderChart();
            chrtComparison.Legends["RightLegend"].Enabled = cbLegend.Checked;
        }




        public void LoadChart(List<ComparisonRow> comparisonData, ReportType report)
        {
            SessionStringName = string.Format("{0}SessionString", report );

            if (GraphInformation == null)
                GraphInformation = new GraphData();
            
            var seriesList = new List<GraphSeries>();
            var tf = new GraphSeries("TotalFleet") {GraphColour = Color.Blue};
            var tnr = new GraphSeries("TotalNonRev") { GraphColour = Color.Red };
            var re = new GraphSeries("Comments") { GraphColour = Color.Green };


            foreach (var cd in comparisonData)
            {
                if (cd.Key == "Total") continue;
                tf.Xvalue.Add(cd.Key);
                tnr.Xvalue.Add(cd.Key);
                re.Xvalue.Add(cd.Key);


                tf.Yvalue.Add(cd.FleetCount);
                tnr.Yvalue.Add(cd.NonRevCount);
                re.Yvalue.Add(cd.ReasonsEntered);
            }
            seriesList.Add(tf);
            seriesList.Add(tnr);
            seriesList.Add(re);

            GraphInformation.SeriesData = seriesList;
        }

        private void BuildTotalCylinderChart()
        {
            var ca = new ChartArea {Name = "Total Fleet Area"};
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisY.MajorGrid.Enabled = false;
            chrtComparison.ChartAreas.Add(ca);

            if (GraphInformation.SeriesData == null) return;

            foreach (var cd in GraphInformation.SeriesData)
            {
                var cs = new Series(cd.SeriesName) {ChartType = SeriesChartType.Column, Color = cd.GraphColour};
                cs["DrawingStyle"] = "Cylinder";
                
                cs.IsValueShownAsLabel = GraphInformation.ShowLabelSeriesNames.Contains(cd.SeriesName);
                cs.LabelFormat = "#,#";
                for (int i = 0; i < cd.Xvalue.Count; i++)
                {
                    cs.Points.AddXY(cd.Xvalue[i], cd.Yvalue[i]);
                }
                chrtComparison.Series.Add(cs);

                var imageLocation = cd.Displayed ? AgeingChart.Checkedboximagelocation : AgeingChart.Uncheckedboximagelocation;

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

                chrtComparison.Legends["RightLegend"].CustomItems.Add(legendItem);

                if (cd.Displayed == false)
                {
                    cs.IsVisibleInLegend = false;
                    cs.Enabled = false;

                }

            }
        }

        protected void chrtComparison_Click(object sender, ImageMapEventArgs e)
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

    }
}