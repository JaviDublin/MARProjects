using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI.WebControls;
using App.BLL;
using Dundas.Charting.WebControl;
using System.Globalization;
using Mars.App.Classes.BLL.Common;

namespace App.AvailabilityTool.KPIDownload
{
    public partial class Default : PageBase
    {
        #region "Page Events"

        protected void Page_Init(object sender, System.EventArgs e)
        {
            //Set Chart Page
            this.AvailabilityChartKPIDownloadVehicleStatus.MARSPage = (int)ReportSettings.ReportSettingsPage.ATKPIDownload;
            this.AvailabilityChartKPIDownloadIdleUnitsOnPeak.MARSPage = (int)ReportSettings.ReportSettingsPage.ATKPIDownload;
            this.AvailabilityChartKPIDownloadOperationalUtilization.MARSPage = (int)ReportSettings.ReportSettingsPage.ATKPIDownload;

            //Set chart type
            this.AvailabilityChartKPIDownloadVehicleStatus.ChartType = (int)Charts.ChartType.VehicleStatus;
            this.AvailabilityChartKPIDownloadIdleUnitsOnPeak.ChartType = (int)Charts.ChartType.IdleUnitsOnPeak;
            this.AvailabilityChartKPIDownloadOperationalUtilization.ChartType = (int)Charts.ChartType.OperationalUtilization;

            //Load Chart features
            this.AvailabilityChartKPIDownloadVehicleStatus.LoadChartFeatures();
            this.AvailabilityChartKPIDownloadIdleUnitsOnPeak.LoadChartFeatures();
            this.AvailabilityChartKPIDownloadOperationalUtilization.LoadChartFeatures();

            //Set Extra Features
            Dundas.Charting.WebControl.Chart ChartVehicleStatus = (Dundas.Charting.WebControl.Chart)this.AvailabilityChartKPIDownloadVehicleStatus.FindControl("DundasChartAvailability");
            DrawStripLines(ChartVehicleStatus);
        }


        protected void Page_Load(object sender, System.EventArgs e)
        {

            //Set Page title
            this.Page.Title = "MARS - Availability Tool";

            //Set report settings tool
            this.UserControlReportSettings.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSettings.MarsPage = (int)ReportSettings.ReportSettingsPage.ATKPIDownload;


            if (!Page.IsPostBack)
            {


                //Set page informtion on usercontrol
                base.SetPageInformationTitle("ATKPIDownload", this.UserControlPageInformation, false);

                //Set Last update / next update labels
                base.SetDataImportUpdateLabel((int)ImportDetails.ImportType.Availability, this.UserControlPageInformation);

                //Check if user as prefences
                CheckReportPreferencesSettings();
                //Reset File Download Name
                SessionHandler.AvailabilityKPIDownloadFileName = null;

            }

            //Set validation group
            this.ModalConfirmKPIDownload.ValidationGroup = "GenerateReport";


        }


        protected void CheckReportPreferencesSettings()
        {
            List<ReportPreferences> preferences = base.GetPreferencesSettings();

            if ((preferences != null))
            {
                //There should be only one row
                foreach (ReportPreferences row in preferences)
                {
                    this.UserControlReportSettings.LoadReportSettingsControl(false);
                    this.UserControlReportSettings.SetUserDefaultSettingsAvailabilityTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id,
                            row.OPS_Region_Id, row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference,
                                row.Location, null, null, null, null, null, null);

                }
            }
            else
            {
                this.UserControlReportSettings.LoadReportSettingsControl(true);
            }

        }

        #endregion

        #region "Click Events"
        protected void ButtonDownloadReport_Click(object sender, System.EventArgs e)
        {
            //Generarte Report
            this.DownloadReportKPI();
        }


        protected void DownloadReportKPI()
        {
            //Set default values
            int logic = -1;
            string country = "-1";
            int cms_pool_id = -1;
            string cms_pool = null;
            int cms_location_group_id = -1;
            string cms_location_group = null;
            int ops_region_id = -1;
            string ops_region = null;
            int ops_area_id = -1;
            string ops_area = null;
            int car_segment_id = -1;
            string car_segment = null;
            int car_class_id = -1;
            string car_class = null;
            int car_group_id = -1;
            string car_group = null;
            DateTime availabilityDate = DateTime.Now;
            int dateRangeValue = -1;
            string dateRange = null;
            string location = "-1";


            //Pass values from usercontrol
            logic = this.UserControlReportSettings.Logic;
            country = this.UserControlReportSettings.Country;

            switch (logic)
            {

                case (int)ReportSettings.OptionLogic.CMS:
                    cms_pool_id = this.UserControlReportSettings.CMS_Pool_Id;
                    cms_pool = this.UserControlReportSettings.CMS_Pool;
                    cms_location_group_id = this.UserControlReportSettings.CMS_Location_Group_Id;
                    cms_location_group = this.UserControlReportSettings.CMS_Location_Group;

                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    ops_region_id = this.UserControlReportSettings.OPS_Region_Id;
                    ops_region = this.UserControlReportSettings.OPS_Region;
                    ops_area_id = this.UserControlReportSettings.OPS_Area_Id;
                    ops_area = this.UserControlReportSettings.OPS_Area;

                    break;
            }

            car_segment_id = this.UserControlReportSettings.Car_Segment_Id;
            car_segment = this.UserControlReportSettings.Car_Segment;
            car_class_id = this.UserControlReportSettings.Car_Class_Id;
            car_class = this.UserControlReportSettings.Car_Class;
            car_group_id = this.UserControlReportSettings.Car_Group_Id;
            car_group = this.UserControlReportSettings.Car_Group;
            location = this.UserControlReportSettings.Location;

            availabilityDate = this.UserControlReportSettings.DateAvailability;

            dateRangeValue = Convert.ToInt32(this.UserControlReportSettings.DateRangeValue);
            dateRange = this.UserControlReportSettings.DateRange;

            DateTime startDate = DateTime.Now;
            if (dateRangeValue == -1)
            {
                startDate = availabilityDate.AddDays(dateRangeValue + 1);
            }
            else
            {
                startDate = availabilityDate.AddDays(dateRangeValue);
            }


            //Set Last Selection Value
            base.SaveLastSelectionToSession(logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id);

            //Load Data
            System.Data.DataTable tblVehicleStatus = AvailabilityKPI.GetKPIDownloadData("VehicleStatus", country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id,
            startDate, availabilityDate);

            System.Data.DataTable tblIdleUnitsOnPeak = AvailabilityKPI.GetKPIDownloadData("IdleUnitsOnPeak", country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id,
            startDate, availabilityDate);

            System.Data.DataTable tblOperationalUtilization = AvailabilityKPI.GetKPIDownloadData("OperationalUtilization", country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id,
            startDate, availabilityDate);

            System.Data.DataTable tbl30DayOverview = AvailabilityKPI.GetKPIDownloadData("30DayOverview", country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id,
            startDate, availabilityDate);

            //Create Charts
            //Get Reference to the charts
            Chart ChartVehicleStatus = (Dundas.Charting.WebControl.Chart)this.AvailabilityChartKPIDownloadVehicleStatus.FindControl("DundasChartAvailability");
            Chart ChartIdleUnitsOnPeak = (Dundas.Charting.WebControl.Chart)this.AvailabilityChartKPIDownloadIdleUnitsOnPeak.FindControl("DundasChartAvailability");
            Chart ChartOperationalUtilization = (Dundas.Charting.WebControl.Chart)this.AvailabilityChartKPIDownloadOperationalUtilization.FindControl("DundasChartAvailability");

            //Populate chart VehicleStatus with data
            this.GenerateChartVehicleStatus(tblVehicleStatus, car_segment, car_class, car_group, country);
            //Populate chart IdleUnitsOnPeak with data
            this.GenerateChartIdleUnitsOnPeak(tblIdleUnitsOnPeak);
            //Populate chart OperationalUtilization with data
            this.GenerateChartOperationalUtilization(tblOperationalUtilization, car_segment, car_class, car_group, country);

            //Populate chart OperationalUtilization with data

            this.AddTitleToChart(tbl30DayOverview, ChartOperationalUtilization, car_segment, car_class, car_group, country);
            this.CustomLegendForChart3(tbl30DayOverview, ChartOperationalUtilization);


            //Log Usage Statistics
            base.LogUsageStatistics((int)ReportStatistics.ReportName.AvailabilityKPI, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location);

            //Download the reports
            this.CreateDowloadPackage(ChartVehicleStatus, ChartIdleUnitsOnPeak, ChartOperationalUtilization);

            //Set report Selection
            this.SetReportSelection(logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, car_segment_id, car_class_id, car_group_id, location,
            startDate, dateRangeValue);

            //this.DownloadFile(SessionHandler.AvailabilityKPIDownloadFileName);
            this.UpdatePanelKPIDownload.Update();

            
            //Response.Redirect("~/MarsV1/AvailabilityTool/KPIDownload/Downloadfile.aspx", true);

           
        }
        #endregion

        #region "Chart Events"


        protected void GenerateChartVehicleStatus(System.Data.DataTable dataValues, string car_segment, string car_class, string car_group, string country)
        {
            Dundas.Charting.WebControl.Chart ChartVehicleStatus = (Dundas.Charting.WebControl.Chart)this.AvailabilityChartKPIDownloadVehicleStatus.FindControl("DundasChartAvailability");
            ChartVehicleStatus.Series.Clear();

            AddTitleToChart(dataValues, ChartVehicleStatus, car_segment, car_class, car_group, country);
            ChartVehicleStatus.Visible = true;

            string xAxisValue = string.Empty;
            xAxisValue = "SORT";

            // Initializes a new instance of the DataView class
            System.Data.DataView firstView = new System.Data.DataView(dataValues);

            //Count how many series was retrieved by DB query
            int seriesStartIndex = 5;
            // Set to 5 because the first three columns contain date info

            for (int index1 = seriesStartIndex; index1 <= dataValues.Columns.Count - 2; index1++)
            {
                Series newSeries = ChartVehicleStatus.Series.Add(dataValues.Columns[index1].ColumnName);
                newSeries.Points.DataBindXY(firstView, xAxisValue, firstView, dataValues.Columns[index1].ColumnName);
                newSeries.Color = Charts.GetColourForColumn((dataValues.Columns[index1].ColumnName), (int)ReportSettings.ReportSettingsTool.Availability);
                newSeries.XValueIndexed = true;
            }

            if (dataValues.Columns.Count > 3)
            {
                int tmpInt = 0;
                string[] daysNames = { "", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                int[] daysCount = { 0, 0, 0, 0, 0, 0, 0, 0 };
                System.Drawing.Color[] daysColor = {System.Drawing.Color.FromArgb(255, 252, 252, 252),
			                System.Drawing.Color.FromArgb(255, 252, 252, 252),
			                System.Drawing.Color.FromArgb(255, 247, 247, 247),
			                 System.Drawing.Color.FromArgb(255, 239, 239, 239),
			System.Drawing.Color.FromArgb(255, 233, 233, 233),
			System.Drawing.Color.FromArgb(255, 225, 225, 225),
			System.Drawing.Color.FromArgb(255, 217, 217, 217),
			System.Drawing.Color.FromArgb(255, 208, 208, 208)
		};
                //Sum up the number of each day of the week in the retrieved table - will be used to divide Mondays from Tuesdays, etc
                foreach (System.Data.DataRow row in dataValues.Rows)
                {
                    tmpInt = Convert.ToInt32(row[3]);
                    if (tmpInt > 0 & tmpInt < 8)
                    {
                        daysCount[tmpInt] += 1;
                    }
                }
                //Add a stripLine to the chart higlighting the same days
                double[] stripLinesoffset = {
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0,
			0.0
		};
                string[] daysColorHTML = {
			System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, 252, 252, 252)),
			System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, 252, 252, 252)),
			System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, 247, 247, 247)),
			System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, 239, 239, 239)),
			System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, 233, 233, 233)),
			System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, 225, 225, 225)),
			System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, 217, 217, 217)),
			System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(255, 208, 208, 208))
		};

                double offsetSoFar = 0.5;
                //start from 0.5
                for (int indexDay = 1; indexDay <= 7; indexDay++)
                {
                    if (daysCount[indexDay] > 0)
                    {
                        //Add strip lines
                        //AddStripLine(daysNames(indexDay), 0.0, offsetSoFar, daysCount(indexDay), daysColor(indexDay))
                        stripLinesoffset[indexDay] = offsetSoFar;
                        offsetSoFar += daysCount[indexDay];
                    }
                }

                this.ViewState["stripLinesDaysNames"] = daysNames;
                ViewState["stripLinesOffset"] = stripLinesoffset;
                ViewState["stripLinesDaysCount"] = daysCount;
                ViewState["stripLinesDaysColor"] = daysColorHTML;

                DrawStripLines(ChartVehicleStatus);
            }

            //Set series properties
            int index = 0;
            foreach (Series chartSeries in ChartVehicleStatus.Series)
            {
                //Set series visibility
                chartSeries.Enabled = true;
                // Set range column chart type
                chartSeries.Type = SeriesChartType.StackedColumn;

                // Show point labels
                chartSeries.ShowLabelAsValue = true;

                //Set series tooltip
                chartSeries.ToolTip = "#VALX: #VAL, #SERIESNAME";

                // Set the side-by-side drawing style
                chartSeries["DrawSideBySide"] = "false";

                chartSeries.BorderWidth = 2;

                //Set x value type - mandatory to set for dates!
                //chartSeries.XValueType = ChartValueTypes.Int

                // Set series point labels format
                chartSeries.LabelFormat = "P2";
                chartSeries.ShowInLegend = true;

                //Attach chart series to the Default chart area
                chartSeries.ChartArea = "Default";
                index += 1;
            }

            //Disable Variable Label Intervals - force showing every X Value
            ChartVehicleStatus.ChartAreas["Default"].AxisX.Interval = 1;

            // Enable AntiAliasing for either Text and Graphics or just Graphics
            ChartVehicleStatus.AntiAliasing = AntiAliasing.All;

        }


        protected void GenerateChartIdleUnitsOnPeak(System.Data.DataTable dataValues)
        {
            Dundas.Charting.WebControl.Chart ChartIdleUnitsOnPeak = (Dundas.Charting.WebControl.Chart)this.AvailabilityChartKPIDownloadIdleUnitsOnPeak.FindControl("DundasChartAvailability");
            ChartIdleUnitsOnPeak.Series.Clear();

            string xAxisValue = string.Empty;
            xAxisValue = "REP_DATE";

            // Initializes a new instance of the DataView class
            System.Data.DataView firstView = new System.Data.DataView(dataValues);

            //Declare chart series
            Series seriesKpi = ChartIdleUnitsOnPeak.Series.Add("Idle Units on Peak");
            Series seriesMin = ChartIdleUnitsOnPeak.Series.Add("Min");
            Series seriesAvg = ChartIdleUnitsOnPeak.Series.Add("Avg");

            // Since the DataView implements IEnumerable, pass the reader directly into
            // the DataBind method with the name of the Columns selected in the query    
            seriesKpi.Points.DataBindXY(firstView, xAxisValue, firstView, "KPI");

            decimal min = (decimal)0.0;
            decimal avg = (decimal)0.0;

            if (dataValues.Rows.Count > 0 && !((dataValues.Rows[0][1]) is System.DBNull))
            {
                min = Math.Round(Convert.ToDecimal(dataValues.Compute("MIN(KPI)", "KPI>=0")), 2);
                avg = Math.Round(Convert.ToDecimal(dataValues.Compute("AVG(KPI)", "KPI>=0")), 2);
            }

            foreach (DataPoint point in seriesKpi.Points)
            {
                seriesMin.Points.AddXY(point.XValue, min);
                seriesAvg.Points.AddXY(point.XValue, avg);
            }

            //Set series colors
            seriesKpi.Color = System.Drawing.Color.Navy;
            seriesMin.Color = System.Drawing.Color.Red;
            seriesAvg.Color = System.Drawing.Color.Cyan;

            // Set Y axis labels format
            ChartIdleUnitsOnPeak.ChartAreas["IdleUnitsOnPeak"].AxisY.LabelStyle.Format = "N";

            //Set series properties
            int index = 0;
            foreach (Series chartSeries in ChartIdleUnitsOnPeak.Series)
            {
                //Set series visibility
                chartSeries.Enabled = true;
                // Set range column chart type
                chartSeries.Type = SeriesChartType.Line;

                //Set series tooltip
                chartSeries.ToolTip = "#VALX: #VAL, #SERIESNAME";

                // Set the side-by-side drawing style
                chartSeries["DrawSideBySide"] = "false";

                chartSeries.BorderWidth = 2;

                //Set x value type

                chartSeries.XValueType = ChartValueTypes.DateTime;
                chartSeries.LabelFormat = "d MM";

                //Attach chart series to the Default chart area
                chartSeries.ChartArea = "IdleUnitsOnPeak";

                index += 1;
            }

            //Disable Variable Label Intervals - force showing every X Value
            ChartIdleUnitsOnPeak.ChartAreas["IdleUnitsOnPeak"].AxisX.Interval = 1;

            // Enable AntiAliasing for either Text and Graphics or just Graphics
            ChartIdleUnitsOnPeak.AntiAliasing = AntiAliasing.All;
            ChartIdleUnitsOnPeak.ChartAreas["IdleUnitsOnPeak"].Visible = true;


        }


        protected void GenerateChartOperationalUtilization(System.Data.DataTable dataValues, string car_segment, string car_class, string car_group, string country)
        {
            Dundas.Charting.WebControl.Chart ChartOperationalUtilization = (Dundas.Charting.WebControl.Chart)this.AvailabilityChartKPIDownloadIdleUnitsOnPeak.FindControl("DundasChartAvailability");
            AddTitleToChart(dataValues, ChartOperationalUtilization, car_segment, car_class, car_group, country);

            string xAxisValue = string.Empty;
            xAxisValue = "REP_DATE";

            // Initializes a new instance of the DataView class
            System.Data.DataView firstView = new System.Data.DataView(dataValues);

            //Declare chart series
            Series seriesOpsUti = ChartOperationalUtilization.Series.Add("OPS UTI");
            Series seriesOpsFleet = ChartOperationalUtilization.Series.Add("OPS FLEET");
            Series seriesAvg1 = ChartOperationalUtilization.Series.Add("Avg OPS UTI");
            Series seriesAvg2 = ChartOperationalUtilization.Series.Add("Avg OPS FLEET");

            // Since the DataView implements IEnumerable, pass the reader directly into
            // the DataBind method with the name of the Columns selected in the query    
            seriesOpsUti.Points.DataBindXY(firstView, xAxisValue, firstView, "OPS_UTI");
            seriesOpsFleet.Points.DataBindXY(firstView, xAxisValue, firstView, "OPS_FLEET");

            decimal avg1 = (decimal)0.0;
            decimal avg2 = (decimal)0.0;

            if (dataValues.Rows.Count > 0)
            {
                avg1 = Math.Round(Convert.ToDecimal(dataValues.Compute("AVG(OPS_UTI)", "OPS_UTI>=0")), 2);
                avg2 = Math.Round(Convert.ToDecimal(dataValues.Compute("AVG(OPS_FLEET)", "OPS_FLEET>=0")), 2);
            }

            foreach (DataPoint point in seriesOpsUti.Points)
            {
                seriesAvg1.Points.AddXY(point.XValue, avg1);
                seriesAvg2.Points.AddXY(point.XValue, avg2);
            }

            //Set series colors
            seriesOpsUti.Color = System.Drawing.Color.DarkGreen;
            seriesOpsFleet.Color = System.Drawing.Color.Red;
            seriesAvg1.Color = System.Drawing.Color.Cyan;
            seriesAvg2.Color = System.Drawing.Color.Magenta;

            //Attach chart series to the Default chart area
            seriesOpsUti.ChartArea = "OperationalUtilization";
            seriesOpsFleet.ChartArea = "OperationalUtilization";
            seriesAvg1.ChartArea = "OperationalUtilization";
            seriesAvg2.ChartArea = "OperationalUtilization";

            // Set Y axis labels format
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.LabelStyle.Format = "N";

            //Set series properties
            int index = 0;
            foreach (Series chartSeries in ChartOperationalUtilization.Series)
            {
                //Set series visibility
                chartSeries.Enabled = true;
                // Set range column chart type
                chartSeries.Type = SeriesChartType.Line;

                //Set series tooltip
                chartSeries.ToolTip = "#VALX: #VAL, #SERIESNAME";

                // Set the side-by-side drawing style
                chartSeries["DrawSideBySide"] = "false";

                chartSeries.BorderWidth = 2;

                //Set x value type
                chartSeries.XValueType = ChartValueTypes.DateTime;
                chartSeries.LabelFormat = "d MM";

                index += 1;
            }

            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.ScaleBreakStyle.Enabled = true;
            // Set the scale break type
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.ScaleBreakStyle.BreakLineType = BreakLineType.Wave;
            // Set the spacing gap between the lines of the scale break (as a percentage of y-axis)
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.ScaleBreakStyle.Spacing = 5;
            // Set the line width of the scale break
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.ScaleBreakStyle.LineWidth = 2;
            // Set the color of the scale break
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.ScaleBreakStyle.LineColor = System.Drawing.Color.Black;
            // Show scale break if more than 25% of the chart is empty space
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 10;
            // If all data points are significantly far from zero, 
            // the Chart will calculate the scale minimum value
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.ScaleBreakStyle.StartFromZero = AutoBool.Auto;


            //Disable Variable Label Intervals - force showing every X Value
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.Interval = 0;
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;

            // Enable AntiAliasing for either Text and Graphics or just Graphics
            ChartOperationalUtilization.AntiAliasing = AntiAliasing.All;
            ChartOperationalUtilization.ChartAreas["OperationalUtilization"].Visible = true;


        }


        protected void AddStripLine(string slTitle, double slInterval, double slIntervalOffset, double slStripWidth, System.Drawing.Color slBackColor, Dundas.Charting.WebControl.Chart chart)
        {
            // Create a strip line
            StripLine stripLine = new StripLine();

            // Set strip line color
            stripLine.BackColor = slBackColor;

            // Set strip line interval
            stripLine.Interval = slInterval;

            // Set strip line offset
            stripLine.IntervalOffset = slIntervalOffset;

            // Set Strip Width
            stripLine.StripWidth = slStripWidth;

            // Set strip line width
            stripLine.StripWidthType = DateTimeIntervalType.NotSet;

            // Set strip line interval type
            stripLine.IntervalType = DateTimeIntervalType.NotSet;

            // Set strip line offset type
            stripLine.IntervalOffsetType = DateTimeIntervalType.NotSet;

            //Add stripline title
            stripLine.Title = slTitle;
            stripLine.TitleAlignment = System.Drawing.StringAlignment.Center;
            stripLine.TitleColor = System.Drawing.Color.LightSteelBlue;
            stripLine.TitleAngle = 0;

            // Add strip lines to the X axis.
            chart.ChartAreas["Default"].AxisX.StripLines.Add(stripLine);


        }


        protected void AddTitleToChart(System.Data.DataTable dataValues, Dundas.Charting.WebControl.Chart dundasChart, string car_segment, string car_class, string car_group, string country)
        {
            dundasChart.Titles.Clear();
            //Add Title "Hertz Europe Mars Availability Tool KPI Report"
            Dundas.Charting.WebControl.Title ctitle1 = new Dundas.Charting.WebControl.Title();
            ctitle1.Text = Resources.lang.HertzEurope + "\\n" + Resources.lang.MarsAvailabilityTool + " " + Resources.lang.KPI + " " + Resources.lang.Report;
            ctitle1.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            ctitle1.Alignment = System.Drawing.ContentAlignment.TopCenter;
            dundasChart.Titles.Add(ctitle1);

            Dundas.Charting.WebControl.Title ctitle2 = new Dundas.Charting.WebControl.Title();
            string carSegment = Resources.lang.CarsVans;
            string carClassOrGroup = Resources.lang.Total;

            if (!car_segment.Contains("All"))
            {
                carSegment = car_segment;
            }

            if (!car_class.Contains("All"))
            {
                carClassOrGroup = car_class;
            }
            if (!car_group.Contains("All"))
            {
                carClassOrGroup = car_group;
            }

            ctitle2.Text = country + " " + Resources.lang.RAC + "\\n" + carSegment + " " + "\\n" + carClassOrGroup;
            ctitle2.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
            ctitle2.Alignment = System.Drawing.ContentAlignment.TopLeft;
            ctitle2.Position.X = 3;
            ctitle2.Position.Y = 3;
            dundasChart.Titles.Add(ctitle2);

            Dundas.Charting.WebControl.Title ctitle3 = new Dundas.Charting.WebControl.Title();
            ctitle3.Text = DateTime.Today.ToLongDateString();
            ctitle3.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
            ctitle3.Alignment = System.Drawing.ContentAlignment.TopRight;
            ctitle3.Position.X = 97;
            ctitle3.Position.Y = 3;
            dundasChart.Titles.Add(ctitle3);


        }


        protected void DrawStripLines(Dundas.Charting.WebControl.Chart chart)
        {
            if (ViewState["stripLinesDaysNames"] != null)
            {
                string[] daysNames = (string[])ViewState["stripLinesDaysNames"];
                double[] stripLinesoffset = (double[])ViewState["stripLinesOffset"];
                int[] daysCount = (int[])ViewState["stripLinesDaysCount"];
                string[] daysColorHTML = (string[])ViewState["stripLinesDaysColor"];
                for (int index = 1; index <= 7; index++)
                {
                    this.AddStripLine(daysNames[index], 0.0, stripLinesoffset[index], daysCount[index], System.Drawing.ColorTranslator.FromHtml(daysColorHTML[index]), chart);
                }
            }

        }

        protected void CustomLegendForChart3(System.Data.DataTable dataValues, Dundas.Charting.WebControl.Chart chart)
        {
            //Add headers
            LegendItem legendHeader = new LegendItem();
            //legend item is a sigle row in legend table

            //Add the row header
            LegendCell legendCellRowHeader = new LegendCell();
            //text        
            legendCellRowHeader.CellType = LegendCellType.Text;
            legendCellRowHeader.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
            legendCellRowHeader.Text = "";
            legendCellRowHeader.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            legendCellRowHeader.TextColor = System.Drawing.Color.Black;
            legendHeader.Cells.Add(legendCellRowHeader);

            //Add the datatype cell (empty for the header)
            LegendCell legendCellDataType = new LegendCell();
            //text        
            legendCellDataType.CellType = LegendCellType.Text;
            legendCellDataType.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
            legendCellDataType.Text = "";
            legendCellDataType.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            legendCellDataType.TextColor = System.Drawing.Color.Black;
            legendHeader.Cells.Add(legendCellDataType);

            for (int rowIndex = 0; rowIndex <= dataValues.Rows.Count - 1; rowIndex++)
            {
                // Add custom legend item with image
                LegendCell legendCell1 = new LegendCell();
                //text        
                legendCell1.CellType = LegendCellType.Text;
                legendCell1.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
                if ((dataValues.Rows[rowIndex][4]) != DBNull.Value)
                {
                    string rep_str = string.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(dataValues.Rows[rowIndex][4]));
                    System.DateTime rep_date = DateTime.ParseExact(rep_str, "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                    legendCell1.Text = rep_date.DayOfWeek.ToString().Substring(0, 2).ToUpper() + "\\n" + rep_date.Day + "." + rep_date.Month;
                }
                else
                {
                    if (dataValues.Rows[rowIndex][0].ToString() == "9999")
                    {
                        if (dataValues.Rows[rowIndex][36].ToString() == "True")
                        {
                            legendCell1.Text = "MTD\\nAVG";
                        }
                        else if (dataValues.Rows[rowIndex][37].ToString() == "True")
                        {
                            legendCell1.Text = "30 DAY\\nAVG";
                        }
                    }
                    else
                    {
                        legendCell1.Text = string.Empty;
                    }
                }
                legendCell1.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
                legendCell1.TextColor = System.Drawing.Color.Black;
                legendHeader.Cells.Add(legendCell1);
            }
            legendHeader.Separator = LegendSeparatorType.Line;
            chart.Legends["Default"].CustomItems.Add(legendHeader);

            //-3 because two last columns are just for SQL info
            for (int columnIndex = 5; columnIndex <= dataValues.Columns.Count - 3; columnIndex++)
            {
                LegendItem legendItem = new LegendItem();
                //legend item is a sigle row in legend table
                bool isPercentage = false;

                //Add the row header
                LegendCell legendRowHeader = new LegendCell();
                //text        
                legendRowHeader.CellType = LegendCellType.Text;
                legendRowHeader.Font = new System.Drawing.Font("Arial", 6, System.Drawing.FontStyle.Bold);
                legendRowHeader.Text = dataValues.Columns[columnIndex].ColumnName;
                legendRowHeader.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
                legendRowHeader.TextColor = System.Drawing.Color.Black;
                legendItem.Cells.Add(legendRowHeader);

                //Add the datatype cell (empty for the header)
                LegendCell legendDataType = new LegendCell();
                //text        
                legendDataType.CellType = LegendCellType.Text;
                legendDataType.Font = new System.Drawing.Font("Arial", 6, System.Drawing.FontStyle.Bold);
                if (dataValues.Columns[columnIndex].ColumnName.Contains("PERC"))
                {
                    legendDataType.Text = "%";
                    isPercentage = true;
                }
                else
                {
                    legendDataType.Text = "#";
                }
                legendDataType.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
                legendDataType.TextColor = System.Drawing.Color.Black;
                legendItem.Cells.Add(legendDataType);

                for (int rowIndex = 0; rowIndex <= dataValues.Rows.Count - 1; rowIndex++)
                {
                    // Add custom legend item with image
                    LegendCell legendCell1 = new LegendCell();
                    //text        
                    legendCell1.CellType = LegendCellType.Text;
                    legendCell1.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Regular);
                    if ((dataValues.Rows[rowIndex][columnIndex]) != DBNull.Value)
                    {
                        legendCell1.Text = dataValues.Rows[rowIndex][columnIndex].ToString();
                    }
                    else
                    {
                        legendCell1.Text = string.Empty;
                    }
                    if (isPercentage == true)
                    {
                        legendCell1.Text += "%";
                    }
                    legendCell1.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
                    legendCell1.TextColor = System.Drawing.Color.Black;
                    legendItem.Cells.Add(legendCell1);
                    legendItem.Separator = LegendSeparatorType.Line;
                }
                chart.Legends["Default"].CustomItems.Add(legendItem);
            }

        }

        #endregion

        #region "Download Package"

        protected void CreateDowloadPackage(Dundas.Charting.WebControl.Chart chartVehicleStatus, Dundas.Charting.WebControl.Chart chartIdleUnitsOnPeak, Dundas.Charting.WebControl.Chart chartOperationUtilization)
        {
            System.IO.MemoryStream StreamForChartVehicleStatus = new System.IO.MemoryStream();
            System.IO.MemoryStream StreamForChartIdleUnitsOnPeak = new System.IO.MemoryStream();
            System.IO.MemoryStream StreamForChartOperationUtilization = new System.IO.MemoryStream();

            chartVehicleStatus.Save(StreamForChartVehicleStatus, ChartImageFormat.Png);
            chartIdleUnitsOnPeak.Save(StreamForChartIdleUnitsOnPeak, ChartImageFormat.Png);
            chartOperationUtilization.Save(StreamForChartOperationUtilization, ChartImageFormat.Png);

            string filepath = Server.MapPath("~/TempFiles");
            filepath = filepath.Replace("\\", "/");
            //generate new file name
            string pdfFileName = "KPIDownload-" + string.Format("{0:ddMMyyhhmmss}", DateTime.Now) + ".pdf";

            //Create pdf document and save it to the above path
            try
            {
                this.RunReportingServicesReport(ConvertImageToBase64(StreamForChartVehicleStatus), ConvertImageToBase64(StreamForChartIdleUnitsOnPeak), ConvertImageToBase64(StreamForChartOperationUtilization), filepath + "/" + pdfFileName);


                //Set Session Value of filename to download
                SessionHandler.AvailabilityKPIDownloadFileName = filepath + "/" + pdfFileName;

                //Delete files older than one day
                ClearOldReports(filepath);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                StreamForChartVehicleStatus.Close();
                StreamForChartIdleUnitsOnPeak.Close();
                StreamForChartOperationUtilization.Close();

            }

        }

        protected void DownloadFile(string FilePath)
        {

            // retrieve the physical path of the file to download, and create
            // a FileInfo object to read its properties
            //Dim FilePath As String = Server.MapPath(virtualPath)
            System.IO.FileInfo TargetFile = new System.IO.FileInfo(FilePath);

            // clear the current output content from the buffer
            Response.Clear();
            // add the header that specifies the default filename for the Download/
            // SaveAs dialog
            Response.AddHeader("Content-Disposition", "attachment; filename=" + TargetFile.Name);
            // add the header that specifies the file size, so that the browser
            // can show the download progress
            Response.AddHeader("Content-Length", TargetFile.Length.ToString());
            // specify that the response is a stream that cannot be read by the
            // client and must be downloaded
            Response.ContentType = "application/octet-stream";
            // send the file stream to the client
            Response.WriteFile(TargetFile.FullName);
        }

        private string ReadFileToBase64String(string imageName)
        {
            System.IO.FileStream inFile = null;
            byte[] binaryData = null;
            // Convert the binary input into Base64 UUEncoded output.
            string base64String = null;

            inFile = new System.IO.FileStream(imageName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            binaryData = new byte[inFile.Length + 1];
            long bytesRead = inFile.Read(binaryData, 0, (int)inFile.Length);
            inFile.Close();

            base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);

            return base64String;
        }

        private string ConvertImageToBase64(System.IO.MemoryStream imageStream)
        {

            long lBytes = imageStream.Length;
            byte[] fileData = new byte[lBytes];

            if ((lBytes > 0))
            {
                // Read the file into a byte array
                imageStream.Seek(0, System.IO.SeekOrigin.Begin);
                imageStream.Read(fileData, 0, (int)lBytes);
                imageStream.Close();
            }

            return Convert.ToBase64String(fileData);

        }


        protected void RunReportingServicesReport(string base64ImageString1, string base64ImageString2, string base64ImageString3, string filePath)
        {
            ////Create a new proxy to the web service

            //rs2005.ReportingService2005 rs = new rs2005.ReportingService2005();
            //rsExecService.ReportExecutionService rsExec = new rsExecService.ReportExecutionService();

            ////Authenticate to the Web service using Windows credentials
            //rs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            //rsExec.Credentials = System.Net.CredentialCache.DefaultCredentials;

            ////Assign the URL of the Web service
            //rs.Url = "http://hescap01/ReportServer$sql2005" + "/ReportService2005.asmx";
            //rsExec.Url = "http://hescap01/ReportServer$sql2005" + "/ReportExecution2005.asmx";

            ////Prepare Render arguments
            //string historyID = null;
            //string deviceInfo = null;
            //string format = "PDF";
            //byte[] results = null;
            //string encoding = String.Empty;
            //string mimeType = String.Empty;
            //string extension = String.Empty;
            //rsExecService.Warning[] warnings = null;
            //string[] streamIDs = null;

            ////Define variables needed for GetParameters() method
            ////Get the report name
            //string reportName = "/MarsAvailabilityReports/MarsAvailabilityImagesToPDF";
            //bool forRendering = false;
            //rs2005.DataSourceCredentials[] dsCredentials = null;
            //rs2005.ReportParameter[] parameters = null;
            //rs2005.ParameterValue[] parameter = null;
            //rsExecService.ParameterValue[] parametersValues = null;

            ////Get if any parameters needed.
            //parameters = rs.GetReportParameters(reportName, historyID, forRendering, parameter, dsCredentials);

            ////Load the selected report.
            //rsExecService.ExecutionInfo ei = rsExec.LoadReport(reportName, historyID);

            ////Prepare report parameter.
            ////Set the parameters for the report needed.
            //parametersValues = new rsExecService.ParameterValue[parameters.Length];

            ////Place to include the parameter.
            //if ((parameters.Length > 0))
            //{
                //parametersValues[0] = new rsExecService.ParameterValue();
                //parametersValues[0].Label = "ImgMimeType";
                //parametersValues[0].Name = "ImgMimeType";
                //parametersValues[0].Value = "image/png";

                //parametersValues[1] = new rsExecService.ParameterValue();
                //parametersValues[1].Label = "Img1Base64";
                //parametersValues[1].Name = "Img1Base64";

                //parametersValues[1].Value = base64ImageString1;
                //parametersValues[2] = new rsExecService.ParameterValue();
                //parametersValues[2].Label = "Img2Base64";
                //parametersValues[2].Name = "Img2Base64";
                //parametersValues[2].Value = base64ImageString2;

                //parametersValues[3] = new rsExecService.ParameterValue();
                //parametersValues[3].Label = "Img3Base64";
                //parametersValues[3].Name = "Img3Base64";
                //parametersValues[3].Value = base64ImageString3;
            //}

            //rsExec.SetExecutionParameters(parametersValues, "en-us");
            
          

            //results = rsExec.Render(format, deviceInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);

            ////Create a file stream and write the report to it

            //using (System.IO.FileStream stream = System.IO.File.OpenWrite(filePath))
            //{
                //stream.Write(results, 0, results.Length);
            //}


        }


        protected void ClearOldReports(string path)
        {
            // Iterate through a collection of report files and delete all older than 2 days

            foreach (string reportFile in System.IO.Directory.GetFiles(path, "*.pdf"))
            {
                System.IO.FileInfo objFileInfo = new System.IO.FileInfo(reportFile);
                if (objFileInfo.LastWriteTimeUtc < DateTime.Today.AddDays(-1))
                {
                    System.IO.File.Delete(reportFile);
                }
            }

        }

        #endregion

        #region "Report Selection"

        protected void SetReportSelection(int selectedLogic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id, int car_segment_id, int car_class_id, int car_group_id, string location,
        DateTime start_date, int dateRange)
        {
            //Set Report Selection User Control
            //Logic
            this.UserControlReportSelections.Logic = selectedLogic;

            //Country
            if (country == "-1")
            {
                this.UserControlReportSelections.Country = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Country = country;
            }

            //Select logic
            switch (selectedLogic)
            {

                case (int)ReportSettings.OptionLogic.CMS:
                    //CMS Pool
                    if (cms_pool_id == -1)
                    {
                        this.UserControlReportSelections.CMS_Pool = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.CMS_Pool = this.UserControlReportSettings.CMS_Pool;
                    }
                    //CMS Location Group
                    if (cms_location_group_id == -1)
                    {
                        this.UserControlReportSelections.CMS_Location_Group = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.CMS_Location_Group = this.UserControlReportSettings.CMS_Location_Group;
                    }

                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    //OPS Region
                    if (ops_region_id == -1)
                    {
                        this.UserControlReportSelections.OPS_Region = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.OPS_Region = this.UserControlReportSettings.OPS_Region;
                    }
                    //OPS Area
                    if (ops_area_id == -1)
                    {
                        this.UserControlReportSelections.OPS_Area = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.OPS_Area = this.UserControlReportSettings.OPS_Area;
                    }
                    break;
            }

            //Location
            if (location == "-1")
            {
                this.UserControlReportSelections.Location = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Location = location;
            }

            //Car Segment
            if (car_segment_id == -1)
            {
                this.UserControlReportSelections.Car_Segment = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Car_Segment = this.UserControlReportSettings.Car_Segment;
            }

            //Car Class
            if (car_class_id == -1)
            {
                this.UserControlReportSelections.Car_Class = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Car_Class = this.UserControlReportSettings.Car_Class;
            }

            //Car Group
            if (car_group_id == -1)
            {
                this.UserControlReportSelections.Car_Group = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Car_Group = this.UserControlReportSettings.Car_Group;
            }
            this.UserControlReportSelections.StartDate = string.Format("{0:dd/MM/yyyy}", start_date);
            this.UserControlReportSelections.DateRange = ReportSettings.GetDateRange(dateRange);

            //Set Report Selection for tool and page
            this.UserControlReportSelections.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSelections.MarsPage = (int)ReportSettings.ReportSettingsPage.ATKPIDownload;
            //Display report selection
            this.UserControlReportSelections.ShowReportSelections();
        }

        #endregion
    }
}