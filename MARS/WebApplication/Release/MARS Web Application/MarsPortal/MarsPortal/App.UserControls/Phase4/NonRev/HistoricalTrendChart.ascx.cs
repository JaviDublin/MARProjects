using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class HistoricalTrendChart : UserControl
    {
        private const string HistoricalChartGraphInfo = "HistoricalChartGraphInfo";

        public const string Checkedboximagelocation = "~/App.Images/Checked.jpg";
        public const string Uncheckedboximagelocation = "~/App.Images/Unchecked.jpg";


        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session[HistoricalChartGraphInfo];
            }
            set { Session[HistoricalChartGraphInfo] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (GraphInformation == null)
                GraphInformation = new GraphData();   
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            BuildTotalCylinderChart();
            chrtHistoricalTrend.Legends["RightLegend"].Enabled = cbLegend.Checked;
        }

        private void BuildTotalCylinderChart()
        {
            var ca = new ChartArea { Name = "Total Fleet Area" };
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisY.MajorGrid.Enabled = false;
            chrtHistoricalTrend.ChartAreas.Add(ca);
            if (GraphInformation.SeriesData == null) return;

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

                chrtHistoricalTrend.Legends["RightLegend"].CustomItems.Add(legendItem);

                if (cd.Displayed == false)
                {
                    cs.IsVisibleInLegend = false;
                    cs.Enabled = false;
                }
            }
        }

        protected void chrtHistoricalTrend_Click(object sender, ImageMapEventArgs e)
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


        public void LoadChart(List<HistoricalTrendRow> historicalTrendData)
        {
            historicalTrendData.RemoveAll(d => d.ColumnCode == "Total Non Rev");
            var dataList = GenerateHistoricalTrendGraphingData(historicalTrendData);
            
            GraphInformation.SeriesData = dataList;
        }

        private void ExtractColumnHeaders(List<HistoricalTrendRow> comparisonData)
        {
            comparisonData = comparisonData.OrderByDescending(d => d.Date).ToList();
            var dt = new DataTable();

            dt.Columns.Add("Date");
            //Add Headers
            foreach (var code in comparisonData.Select(d => d.ColumnCode).Distinct().ToList())
            {
                dt.Columns.Add(code);
            }


            foreach (var day in comparisonData.Select(d => d.Date).Distinct().ToList())
            {
                var dataForDay = comparisonData.Where(d => d.Date == day).ToList();
                var dataRow = dataForDay.Select(d => string.Format("{0:##,###}", d.CodeCount)).ToList();
                if (dataRow.Count == 0) continue;
                dataRow.Insert(0, day.ToShortDateString());
                dt.Rows.Add(dataRow.ToArray());
            }

        }

        private static List<GraphSeries> GenerateHistoricalTrendGraphingData(IEnumerable<HistoricalTrendRow> data)
        {
            data = data.OrderBy(d => d.Date).ToList();
            var keys = data.Select(d => d.ColumnCode).Distinct().ToList();

            var returned = new List<GraphSeries>();
            var groupLists = new List<List<double>>() ;
            var days = data.Select(d => d.Date).Distinct().ToList();

            int rowCount = 0;

            //var colourList = new List<Color>();

            foreach (var day in days)
            {
                var dataForDay = data.Where(d => d.Date == day).ToList();
                var dataRow = new List<double>();
                foreach (var code in keys)
                {
                    if (dataForDay.Count(d => d.ColumnCode == code) > 0)
                    {
                        var codeCount = dataForDay.Single(d => d.ColumnCode == code).CodeCount;
                        

                        dataRow.Add(codeCount);
                    }
                    else
                    {
                        dataRow.Add(0);
                    }
                }
                groupLists.Add(dataRow);

                rowCount++;
            }

            for (int i = 0; i < keys.Count; i++)
            {                
                var xx = days.Select(d => d.ToShortDateString()).Cast<object>().ToList();

                returned.Add(new GraphSeries(keys[i])
                {
                    GraphColour = Classes.BLL.Common.Charts.GetColourForColumn(keys[i], 3),
                    Xvalue = xx,
                    Yvalue = groupLists.Select(d=> d[i]).ToList()
                });
            }

            return returned;
        }

    }
}