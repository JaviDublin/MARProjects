using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using App.BLL.EventArgs;
using App.BLL.ExtensionMethods;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using App.Styles;
using Mars.App.Classes.BLL.EventArgs;
using Resources;

namespace App.UserControls.Charting
{
    //Base class for all MarsV2 Charts. All Charts create an instance of this class and use the GraphData class to customize it.

    public partial class ChartControls : UserControl
    {
        internal GraphData GraphInformation;
        internal List<SeriesChartType> GraphTypesAllowed;

        private const string Checkedboximagelocation = "~/App.Images/Checked.jpg";
        private const string Uncheckedboximagelocation = "~/App.Images/Unchecked.jpg";


        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.Cookies["ChartMonitorSize"] != null)
            {
                ddlMonitorSize.SelectedIndex = Request.Cookies["ChartMonitorSize"].Value == LocalizedChartControl.ChartMonitorSizeNormal ? 0 : 1;
            }
            if (Request.Cookies["ChartHighlightWeekends"] == null)
            {
                Response.Cookies["ChartHighlightWeekends"].Value = "true";
            }

            cbHighlightWeekends.Checked = Request.Cookies["ChartHighlightWeekends"].Value == "true";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnShowGridData.Visible = GraphInformation.ShowGridData;
            if (ddlGraphTypes.Items.Count == 0)
            {
                foreach (var t in GraphTypesAllowed)
                {
                    ddlGraphTypes.Items.Add(new ListItem(t.ToString(), t.ToString()));
                }
            }
            if (ddlIntervalOptions.Items.Count == 0)
            {
                ddlIntervalOptions.Items.Add(new ListItem("Default"));
                ddlIntervalOptions.Items.Add(new ListItem(DateTimeIntervalType.Days.ToString()));
                ddlIntervalOptions.Items.Add(new ListItem(DateTimeIntervalType.Weeks.ToString()));
            }

            Page.PreRenderComplete += Page_PreRenderComplete;
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            Chart1.Visible = !pnlGridData.Visible;
            sldrYAxisZoom.Text = GraphInformation.YAxisZoom;


            gvChartData.DataSource = GraphInformation.GridData;
            gvChartData.DataBind();

            var chartArea = Chart1.ChartAreas["ChartArea1"];
            var chartTitle = Chart1.Titles["ParameterTitle"];
            var chartTitleAdditional = Chart1.Titles["ParameterTitleAdditional"];


            Chart1.Annotations.Add(new TextAnnotation { X = 0, Y = 0,
                                                               Font = MarsFonts.ChartingChartNameTitle,
                                                               ToolTip = GraphInformation.ChartDescription,
                                                               Text = GraphInformation.ChartName });
            Chart1.Annotations.Add(new RectangleAnnotation
                                       {
                                           X = 89,
                                           Y = 2,
                                           BackColor = MarsColours.MarsYellow,
                                           Font = MarsFonts.ChartingShowLegend,
                                           PostBackValue = "HideSeries",
                                           Visible = !GraphInformation.HideSeriesInfo,
                                           Text = GraphInformation.ShowingSeriesInformation ? LocalizedChartControl.HideLegendText : LocalizedChartControl.ShowLegendText
                                       });
            Chart1.Legends["Legend2"].Enabled = GraphInformation.ShowingSeriesInformation;
            CheckMonitorSize();

            if (GraphInformation.SeriesData == null || GraphInformation.YEntriesCount == 0)
            {
                DisplayNoDataTitle(chartTitle);
                cbHighlightWeekends.Enabled = false;
                lblDataUpdated.Text = string.Empty;
                return;
            }

            lblDataUpdated.Text = string.Format("{0}: {1} {2}",
                LocalizedChartControl.ChartDataFromText,
                GraphInformation.DataDate.ToLongDateString(),
                GraphInformation.UsingCachedGraphingData ? LocalizedChartControl.ChartDataCachedString : "");

            if (GraphInformation.ShowGraphLinkingButton)
            {
                Chart1.Annotations.Add(new RectangleAnnotation()
                {
                    X = 0,
                    Y = 9,
                    Font = MarsFonts.ChartingLinkingMessage,
                    BackColor = MarsColours.MarsYellow,
                    Text = GraphInformation.GraphLinkingText,
                    PostBackValue = "LinkToGraph"
                });
            }

            if (!string.IsNullOrEmpty(GraphInformation.YAxisNumberFormat))
            {
                chartArea.AxisY.LabelStyle.Format = GraphInformation.YAxisNumberFormat;
            }

            if (GraphInformation.HideSeriesInfo) Chart1.Legends["Legend2"].Enabled = false;

            var titleStringBuilder = new StringBuilder();
            GraphInformation.ReportParameters.Where(p => p.ParameterDropDownList.SelectedValue != LocalizedParameterControl.AllParameterSelection).ToList().ForEach(p => titleStringBuilder.Append(p.GetParameterTitle()));

            var titleText = titleStringBuilder.ToString() == ""
                                  ? LocalizedChartControl.NoParametersTitleString
                                  : titleStringBuilder.ToString();

            if (!string.IsNullOrEmpty(GraphInformation.TitleDate)) titleText += GraphInformation.TitleDate;
            chartTitle.Text = titleText;

            if (!string.IsNullOrEmpty(GraphInformation.TitleAdditional))
            {
                chartTitleAdditional.Text = GraphInformation.TitleAdditional;    
            }
            else
            {
                chartTitleAdditional.Visible = false;
            }
            

            chartArea.AxisX.Title = GraphInformation.XAxisTitle;
            chartArea.AxisY.Title = GraphInformation.YAxisTitle;

            GenerateLegends();

            SetScaleBreakStyle(chartArea.AxisY.ScaleBreakStyle, cbAllowYAxisBreak.Checked);
            SetScaleBreakStyle(chartArea.AxisX.ScaleBreakStyle, cbAllowXAxisBreak.Checked);

            SetIntervals(chartArea);

            SetYAxisGrid(chartArea.AxisY);

            var minYValue = PopulateSeries(chartArea);

            SetWidthAndHeightSliders();

            SetYaxisSlider(minYValue);

            GraphInformation.UsingCachedGraphingData = false;

        }

        protected void gridViewSorting(object sender, GridViewSortEventArgs e)
        {
            var dv = GraphInformation.GridData.DefaultView;
            dv.Sort = e.SortExpression + (hfSortDirection.Value == "1" ? " asc" : " desc");
            var sortedDt = dv.ToTable();
            GraphInformation.GridData = sortedDt;
            hfSortDirection.Value = hfSortDirection.Value == "1" ? "0" : "1";
        }


        protected void btnShowGridData_Click(object sender, EventArgs e)
        {
            var showGrid = btnShowGridData.Text == LocalizedChartControl.ShowGridTitle;
            btnShowGridData.Text = showGrid ? LocalizedChartControl.HideGridTitle : LocalizedChartControl.ShowGridTitle;
            ShowGridData(showGrid);
        }

        private void ShowGridData(bool showGrid)
        {
            pnlGridData.Visible = showGrid;
            lblOptions.Enabled = !showGrid;
        }

        protected void ddlMonitorSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlMonitorSize.SelectedValue)
            {
                case "0":
                    Response.Cookies["ChartMonitorSize"].Value = LocalizedChartControl.ChartMonitorSizeNormal;
                    break;
                case "1":
                    Response.Cookies["ChartMonitorSize"].Value = LocalizedChartControl.ChartMonitorSizeWide;
                    break;
            }
        }

        protected void cbHighlightWeekends_CheckedChanged(object sender, EventArgs e)
        {
            Response.Cookies["ChartHighlightWeekends"].Value = cbHighlightWeekends.Checked ? "true" : "false";
        }

        protected void btnRefreshChart_click(object sender, EventArgs e)
        {
            RaiseBubbleEvent(this, new RefreshGraphEventArgs());
        }

        protected void Chart1_Click(object sender, ImageMapEventArgs e)
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
                var legend = Chart1.Legends["Legend2"];

                legend.Enabled = !GraphInformation.ShowingSeriesInformation;
                GraphInformation.ShowingSeriesInformation = !GraphInformation.ShowingSeriesInformation;

                return;
            }
            if (e.PostBackValue == "LinkToGraph")
            {
                RaiseBubbleEvent(this, new GraphLinkingEventArgs() { LinkingPage = GraphInformation.GraphLinkingPage });
                return;
            }
            //Drilldown click
            UpdateParameters(e.PostBackValue);
        }

        protected void sldrYAxisZoom_TextChanged(object sender, EventArgs e)
        {
            GraphInformation.YAxisZoom = sldrYAxisZoom.Text;
        }

        private double PopulateSeries(ChartArea chartArea)
        {
            var minYValue = double.MaxValue;

            if (GraphInformation.IsXValueDate)
            {
                chartArea.AxisX.Minimum = DateTime.Parse(GraphInformation.SelectedParameters[ParameterNames.FromDate]).ToOADate();
                chartArea.AxisX.Maximum = DateTime.Parse(GraphInformation.SelectedParameters[ParameterNames.ToDate]).ToOADate();
                var chartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlGraphTypes.SelectedValue);
                if (chartType == SeriesChartType.Column || chartType == SeriesChartType.Bar)
                {
                    chartArea.AxisX.Minimum -= 0.9;
                    chartArea.AxisX.Maximum += 0.9;
                }
            }

            if (GraphInformation.UseWeeklyData)
            {
                chartArea.AxisX.Maximum = double.Parse(GraphInformation.SeriesData[0].Xvalue.Max().ToString()) + 0.9;
            }

            foreach (var s in GraphInformation.SeriesData)
            {
                var series = Chart1.Series[s.SeriesName];

                series["DrawingStyle"] = GraphInformation.SeriesDrawingStyle;
                series.BorderWidth = GraphInformation.ChartLineWidth;
                

                if (s.Displayed == false)
                {
                    series.IsVisibleInLegend = false;
                    series.Enabled = false;
                    continue;
                }

                series.Color = s.GraphColour;
                


                if (!series.Enabled) continue;

                series.IsValueShownAsLabel = GraphInformation.ShowLabelSeriesNames.Contains(s.SeriesName);

                if (!string.IsNullOrEmpty(GraphInformation.LabelFormat))
                {
                    series.LabelFormat = GraphInformation.LabelFormat;
                }

                for (var i = 0; i < s.Yvalue.Count; i++)
                {
                    minYValue = s.Yvalue[i] < minYValue ? s.Yvalue[i] : minYValue;

                    series.Points.AddXY(s.Xvalue[i], s.Yvalue[i]);
                    if (GraphInformation.NegativeValuesColour != Color.Empty && s.Yvalue[i] < 0)
                    {
                        series.Points[series.Points.Count - 1].Color = GraphInformation.NegativeValuesColour;
                    }
                }
                //
                //var myCal = CultureInfo.CurrentCulture.Calendar;
                //GraphInformation.DataPointsXAxisTooltip = "#VALX (day #INDEX)";
                Chart1.Series[s.SeriesName].ToolTip = string.Format("{0}: {2} {1} ", s.SeriesName, GraphInformation.DataPointsXAxisTooltip, GraphInformation.DataPointsYAxisTooltip);


                if (GraphInformation.AllowDrillDown)
                {
                    Chart1.Series[s.SeriesName].PostBackValue = "#AXISLABEL";
                }
                var parsedEnum = Enum.Parse(typeof(SeriesChartType), ddlGraphTypes.SelectedValue);

                series.ChartType = (SeriesChartType)parsedEnum;    


                if (series.ChartType == SeriesChartType.Doughnut || series.ChartType == SeriesChartType.Pie)
                {
                    series.IsValueShownAsLabel = false;
                }
            }
            return minYValue;
        }
        
        private void CheckMonitorSize()
        {
            switch (ddlMonitorSize.SelectedValue)
            {
                case "0":
                    pnlChartHolder.Width = 950;
                    break;
                case "1":
                    pnlChartHolder.Width = 1300;
                    
                    Chart1.Width = Unit.Parse((Chart1.Width.Value + 350).ToString());
                    break;
            }
        }

        private void GenerateLegends()
        {
            foreach (var s in GraphInformation.SeriesData)
            {
                Chart1.Series.Add(new Series(s.SeriesName));

                var imageLocation = s.Displayed ? Checkedboximagelocation : Uncheckedboximagelocation;

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
                    PostBackValue = "LegendClick/" + s.SeriesName
                };

                var seriesHidden = GraphInformation.HiddenSeriesNames.Contains(s.SeriesName);

                var legendCell2Colour = seriesHidden ? MarsColours.ChartLegendValuesHidden : s.GraphColour;

                legendItem.Cells.Add(legendCell);
                legendCell = new LegendCell { Name = "Cell2", 
                                                Text = s.SeriesName,
                                                ForeColor = legendCell2Colour
                                            };
                legendItem.Cells.Add(legendCell);

                var valuesShown = GraphInformation.ShowLabelSeriesNames.Contains(s.SeriesName);

                var legendCell3Colour = !valuesShown || seriesHidden ? MarsColours.ChartLegendValuesHidden : MarsColours.ChartLegendValuesShown;

                legendCell = new LegendCell
                {
                    Name = "Cell3",
                    Text = "Values",
                    ForeColor = legendCell3Colour,
                    Font = new Font("Tahoma", 9),
                    PostBackValue = "LegendShowLabels/" + s.SeriesName
                };
                legendItem.Cells.Add(legendCell);

                Chart1.Legends["Legend2"].CustomItems.Add(legendItem);
            }
        }

        internal DateTimeIntervalType GetIntervalType()
        {
            DateTimeIntervalType intervalType;
            if (ddlIntervalOptions.SelectedValue == "Default")
            {
                if (!GraphInformation.SelectedParameters.ContainsKey(ParameterNames.FromDate)) return DateTimeIntervalType.Days;
                var dateFrom = DateTime.Parse(GraphInformation.SelectedParameters[ParameterNames.FromDate]);
                var dateTo = DateTime.Parse(GraphInformation.SelectedParameters[ParameterNames.ToDate]);
                var dateDiff = dateTo - dateFrom;

                intervalType = dateDiff.Days > int.Parse(LocalizedChartControl.MaxDaysForDailyInterval) ? DateTimeIntervalType.Weeks : DateTimeIntervalType.Days;
            }
            else
            {
                intervalType = (DateTimeIntervalType)Enum.Parse(typeof(DateTimeIntervalType), ddlIntervalOptions.SelectedValue);
            }
            return intervalType;
        }

        private void SetIntervals(ChartArea chartArea)
        {
            var chartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlGraphTypes.SelectedValue);
            ddlIntervalOptions.Enabled = true;
            if (GraphInformation.IsXValueDate)
            {
                cbHighlightWeekends.Enabled = true;

                var intervalType = GetIntervalType();

                if(!GraphInformation.UseWeeklyData && cbHighlightWeekends.Checked)
                {
                    var addonForColumnGraphs = chartType == SeriesChartType.Column || chartType == SeriesChartType.Bar ? 0.5 : 0;
                    var stripLine = new StripLine
                    {
                        BackColor = MarsColours.ChartHighlightWeekendColour,
                        IntervalOffset =  - (1 + addonForColumnGraphs),
                        IntervalOffsetType = DateTimeIntervalType.Days,
                        Interval = 1,
                        IntervalType = DateTimeIntervalType.Weeks,
                        StripWidth = 2,
                        StripWidthType = DateTimeIntervalType.Days
                    };
                    Chart1.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripLine);    
                }

                SetXAxisForDate(chartArea.AxisX, intervalType);

            }
            else
            {
                if (GraphInformation.UseWeeklyData)
                {
                    Chart1.Titles["ParameterTitleAdditional"].Text += " - Weekly Minimum";
                    GraphInformation.DataPointsXAxisTooltip = "Week #VALX";

                    chartArea.AxisX.LabelStyle.Format = "Week #";

                    chartArea.AxisX.MajorTickMark.Interval = 1;
                    chartArea.AxisX.MajorTickMark.Size = 1;
                    chartArea.AxisX.MajorGrid.LineColor = GraphInformation.LightXAxisLineColour;

                    return;
                }

                cbHighlightWeekends.Enabled = false;

                if (GraphInformation.SeriesDrawingStyle == "Cylinder")
                {
                    chartArea.AxisX.MajorGrid.Enabled = false;
                }

                chartArea.AxisX.LabelStyle.Interval = 1;
                chartArea.AxisX.LabelStyle.Angle = chartType == SeriesChartType.Bar ? 0 : int.Parse(LocalizedChartControl.XAxisLabelAngle);
                ddlIntervalOptions.Enabled = false;
            }
        }

        private void SetWidthAndHeightSliders()
        {
            Chart1.Width = Unit.Parse((Chart1.Width.Value + int.Parse(sldrChartWidth.Text)).ToString());
            Chart1.Height = Unit.Parse((Chart1.Height.Value - int.Parse(sldrChartHeight.Text) + xeChartHeight.Maximum).ToString());

            string widthDescription;
            string heightDescription;
            //Set width dynamically
            switch (sldrChartWidth.Text)
            {
                case "1000":
                    widthDescription = LocalizedChartControl.ChartWidthLarge;
                    break;
                case "2000":
                    widthDescription = LocalizedChartControl.ChartWidthLarger;
                    break;
                case "3000":
                    widthDescription = LocalizedChartControl.ChartWidthLargest;
                    break;
                default:
                    widthDescription = LocalizedChartControl.ChartWidthNormal;
                    break;
            }

            switch (sldrChartHeight.Text)
            {
                case "500":
                    heightDescription = LocalizedChartControl.ChartWidthLarge;
                    break;
                case "250":
                    heightDescription = LocalizedChartControl.ChartWidthLarger;
                    break;
                case "0":
                    heightDescription = LocalizedChartControl.ChartWidthLargest;
                    break;
                default:
                    heightDescription = LocalizedChartControl.ChartWidthNormal;
                    break;
            }
            lblChartSize.Text = string.Format("Width: {0}  Height: {1}", widthDescription, heightDescription);
        }

        private void SetYaxisSlider(double minYValue)
        {
            if (GraphInformation.AllowNegativeValuesOnYAxis)
            {
                Chart1.ChartAreas[0].AxisY.Crossing = 0;
                Chart1.ChartAreas[0].AxisX.IsMarksNextToAxis = false;
            }

            var chartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlGraphTypes.SelectedValue);
            if (chartType == SeriesChartType.Column || chartType == SeriesChartType.Bar) return;

            if (minYValue < 0)
            {
                sldrYAxisZoom.Text = "100";
                GraphInformation.YAxisZoom = "100";
            }

            tbYAxisSliderValue.Text = sldrYAxisZoom.Text + @"%";
            var val = int.Parse(GraphInformation.YAxisZoom);
            Chart1.ChartAreas[0].AxisY.Minimum = minYValue < 1
                                         ? val * minYValue / 100
                                         : Math.Round(val * minYValue / 100);
        }

        private static void SetYAxisGrid(Axis axis)
        {
            axis.MajorGrid.Enabled = false;
            axis.MinorGrid.Enabled = false;
        }

        private void SetXAxisForDate(Axis axis, DateTimeIntervalType t)
        {
            axis.LabelStyle.Angle = int.Parse(LocalizedChartControl.XAxisLabelAngle);
            axis.LabelStyle.Format = GraphInformation.XAxisDateFormat;


           axis.LabelStyle.Interval = 1;
            axis.LabelStyle.IntervalType = t;

            axis.MajorTickMark.Interval = 1;
            axis.MajorTickMark.IntervalType = t;
            axis.MajorTickMark.Size = 1;

            axis.MajorGrid.Interval = 1;
            axis.MajorGrid.IntervalType = t;
            axis.MajorGrid.LineWidth = 1;
            

            if (t == DateTimeIntervalType.Weeks)
            {
                axis.MajorGrid.LineColor = GraphInformation.DarkXAxisLineColour;
                axis.MinorGrid.Enabled = true;
                axis.MinorGrid.LineWidth = 1;
                axis.MinorGrid.LineColor = GraphInformation.LightXAxisLineColour;
                axis.MinorGrid.Interval = 1;
                axis.MinorGrid.IntervalType = DateTimeIntervalType.Days;

                //Offsets
                axis.MajorGrid.IntervalOffset = 1;
                axis.MajorGrid.IntervalOffsetType = DateTimeIntervalType.Days;

                axis.MajorTickMark.IntervalOffset = 1;
                axis.MajorTickMark.IntervalOffsetType = DateTimeIntervalType.Days;

                axis.LabelStyle.IntervalOffset = 1;
                axis.LabelStyle.IntervalOffsetType = DateTimeIntervalType.Days;    

            }
            else
            {

                axis.MajorGrid.LineColor = GraphInformation.LightXAxisLineColour;
            }
        }

        private static void SetScaleBreakStyle(AxisScaleBreakStyle style, bool enable)
        {
            style.Enabled = enable;
            style.BreakLineStyle = BreakLineStyle.Ragged;
            style.Spacing = 2;
            style.LineWidth = 1;
            style.LineColor = MarsColours.ChartLineBreakColour;
        }

        private void UpdateParameters(string parameterValue)
        {
            var reportParameters = GraphInformation.ReportParameters;
            var root = reportParameters.GetRootParameter();

            GraphInformation.CheckIfCachedDataCanBeUsed = true;

            if (root.SelectedValue.Length == 0)
            {
                root.ParameterDropDownList.SelectedIndex = root.ParameterDropDownList.Items.IndexOf(root.ParameterDropDownList.Items.FindByText(parameterValue));
                return;
            }
            var param = reportParameters.GetFirstParameterInBranch(GraphInformation.BranchForDrilldown);

            while (param.SelectedValue.Length != 0)
            {
                param = reportParameters.GetNextHighestParameter(param);
                if (param == null) return;
            }

            param.ParameterDropDownList.SelectedIndex = param.ParameterDropDownList.Items.IndexOf(param.ParameterDropDownList.Items.FindByText(parameterValue));
        }

        private static void DisplayNoDataTitle(Title chartTitle)
        {
            chartTitle.Text = LocalizedChartControl.NoChartDataMessage;
            chartTitle.Position = new ElementPosition(0, 50, 100, 200);
            chartTitle.Font = MarsFonts.ChartingNoDataMessage;
            chartTitle.ForeColor = MarsColours.ChartNoDataMessageColour;
        }
    }
}