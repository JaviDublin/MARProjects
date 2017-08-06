using System;
using System.Collections.Generic;
using App.BLL;

using Dundas.Charting.WebControl;
namespace App.AvailabilityTool.SiteComparison {
    public partial class Default : PageBase {
        // Update at line 427 by Gavin for MarsV3 2-5-12 for ReportSettings to display "Custom" instead of date range
        #region "Page Events"

        protected void Page_Init(object sender, System.EventArgs e) {
            //Set Chart Page
            this.AvailibilityChartSiteComparison.MARSPage = (int)ReportSettings.ReportSettingsPage.ATSiteComparison;

            this.AvailibilityChartSiteComparison.LoadChartFeatures();
        }


        protected void Page_Load(object sender, System.EventArgs e) {
            //Set Page title
            this.Page.Title = "MARS - Availability Tool";

            //Set report settings tool
            this.UserControlReportSettings.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSettings.MarsPage = (int)ReportSettings.ReportSettingsPage.ATSiteComparison;

            if (!Page.IsPostBack) {


                //Set page informtion on usercontrol
                base.SetPageInformationTitle("ATSiteComparison", this.UserControlPageInformation, false);

                //Set Last update / next update labels
                base.SetDataImportUpdateLabel((int)ImportDetails.ImportType.Availability, this.UserControlPageInformation);

                //Check if user as prefences
                CheckReportPreferencesSettings();

            }

            //Set validation group
            this.ModalConfirmSiteComparison.ValidationGroup = "GenerateReport";


        }


        protected void CheckReportPreferencesSettings() {
            List<ReportPreferences> preferences = base.GetPreferencesSettings();

            if ((preferences != null)) {
                //There should be only one row
                foreach (ReportPreferences row in preferences) {
                    this.UserControlReportSettings.LoadReportSettingsControl(false);
                    this.UserControlReportSettings.SetUserDefaultSettingsAvailabilityTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id, row.OPS_Region_Id,
                            row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference,
                                row.Location, null, null, null, null, null, null);

                }
            }
            else {
                this.UserControlReportSettings.LoadReportSettingsControl(true);
            }

            //Update Update Panel
            this.UpdatePanelSiteComparison.Update();

        }
        #endregion

        #region "Click Events"

        protected void ButtonGenerateReport_Click(object sender, System.EventArgs e) {
            //Generarte Report
            GenerateReport();
        }


        protected void GenerateReport() {
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
            int dayOfWeekValue = -1;
            string dayOfWeek = null;
            int dateRangeValue = -1;
            string dateRange = null;
            string fleetName = "-1";

            //Pass values from usercontrol
            logic = this.UserControlReportSettings.Logic;
            country = this.UserControlReportSettings.Country;

            switch (logic) {

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

            fleetName = this.UserControlReportSettings.Fleet_Name;

            availabilityDate = this.UserControlReportSettings.DateAvailability;
            dayOfWeekValue = this.UserControlReportSettings.DayOfWeekValue;
            dayOfWeek = this.UserControlReportSettings.DayOfWeek;
            DateTime startDate = DateTime.Now;
            if (UserControlReportSettings.DateRangeValue.Contains("Custom")) {
                startDate = UserControlReportSettings.customStartDate;
                availabilityDate = UserControlReportSettings.customEndDate;
            }
            else {
                dateRangeValue = Convert.ToInt32(this.UserControlReportSettings.DateRangeValue);
                dateRange = this.UserControlReportSettings.DateRange;
                if (dateRangeValue == -1) {
                    startDate = availabilityDate.AddDays(dateRangeValue + 1);
                }
                else {
                    startDate = availabilityDate.AddDays(dateRangeValue);
                }
            }

            string availabilityLogic = this.UserControlReportSettings.AvailabilityLogic;
            string selectBy = null;
            if (availabilityLogic == "NUMERIC") {
                selectBy = "VALUE";
            }
            else {
                selectBy = "PERCENTAGE";
            }
            string operStatTopic = this.UserControlReportSettings.AvailabilityTopicOperStat;


            //Set Last Selection Value
            base.SaveLastSelectionToSession(logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, null, car_segment_id, car_class_id, car_group_id);

            System.Data.DataTable tblGroups = AvailabilitySiteComparison.GetSiteComparisonData(operStatTopic, logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, fleetName, car_segment_id, car_class_id,
            car_group_id, startDate, availabilityDate, dayOfWeekValue, selectBy);
            this.GenerateChart(tblGroups, availabilityLogic);

            //Set report Selection
            this.SetReportSelection(logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, car_segment_id, car_class_id, car_group_id, startDate,
            dayOfWeekValue, dateRangeValue);

            //Log Usage Statistics
            base.LogUsageStatistics((int)ReportStatistics.ReportName.AvailabilitySiteComparison, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, null);


            this.UpdatePanelSiteComparison.Update();


        }
        #endregion

        #region "Chart Events"


        protected void GenerateChart(System.Data.DataTable dataValues, string logic) {
            int rowCount = dataValues.Rows.Count;
            if (rowCount >= 1) {
                this.AvailibilityChartSiteComparison.Visible = true;
                this.EmptyDataTemplateSiteComparison.Visible = false;
            }
            else {
                this.EmptyDataTemplateSiteComparison.Visible = true;
                this.AvailibilityChartSiteComparison.Visible = false;
            }
            Dundas.Charting.WebControl.Chart ChartSiteComparison = (Dundas.Charting.WebControl.Chart)this.AvailibilityChartSiteComparison.FindControl("DundasChartAvailability");
            ChartSiteComparison.Series.Clear();
            ChartSiteComparison.Titles.Clear();
            Dundas.Charting.WebControl.Title ctitle = new Dundas.Charting.WebControl.Title();
            ctitle.Text = Resources.lang.SiteComparison;
            ctitle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            ctitle.Alignment = System.Drawing.ContentAlignment.TopLeft;
            ChartSiteComparison.Titles.Add(ctitle);


            //Add a item to reset zoom
            Dundas.Charting.WebControl.Title resetZoomTitle = new Dundas.Charting.WebControl.Title();
            resetZoomTitle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            resetZoomTitle.Alignment = System.Drawing.ContentAlignment.BottomRight;
            resetZoomTitle.Docking = Docking.Bottom;
            resetZoomTitle.ToolTip = Resources.lang.ChartZoomFeature;
            resetZoomTitle.DockOffset = 1;
            resetZoomTitle.MapAreaAttributes = "onclick=\"" + ChartSiteComparison.CallbackManager.GetCallbackEventReference("ResetZoom", "ResetZoom") + "\"";
            resetZoomTitle.Text = Resources.lang.ChartZoomText;
            resetZoomTitle.BackImage = "~/App.images/reset_zoom.png";
            resetZoomTitle.BackImageMode = ChartImageWrapMode.Unscaled;
            resetZoomTitle.BackImageAlign = ChartImageAlign.BottomRight;
            ChartSiteComparison.Titles.Add(resetZoomTitle);

            Series series1 = ChartSiteComparison.Series.Add("Default");

            //Set the chart type
            series1.Type = SeriesChartType.Column;

            // Initializes a new instance of the DataView class
            System.Data.DataView firstView = new System.Data.DataView(dataValues);

            // Since the DataView implements IEnumerable, pass the reader directly into
            // the DataBind method with the name of the Columns selected in the query    
            ChartSiteComparison.Series["Default"].Points.DataBindXY(firstView, "location", firstView, "car_count");

            //Change the way labels are displayed when there's more columns
            series1.ShowLabelAsValue = true;
            series1.SmartLabels.Enabled = true;
            series1.SmartLabels.MovingDirection = LabelAlignment.Top;
            //Set label tooltip to Y value
            series1.LabelToolTip = "#VALX: #VAL";
            //Set series bars tooltip to X value
            series1.ToolTip = "#VALX: #VAL";
            // Sort series in ascending order by the default Y value. 
            ChartSiteComparison.DataManipulator.Sort(PointsSortOrder.Descending, "Default");

            series1.BackGradientType = GradientType.TopBottom;
            series1.Color = System.Drawing.Color.FromArgb(230, 0, 125, 255);
            series1.BackGradientEndColor = System.Drawing.Color.Aqua;

            //If the current number of samples is bigger than 10 make the size of font smaller
            if (dataValues.Rows.Count > 10) {
                //Set Data Point Label
                series1["LabelStyle"] = "Top";
                // Set labels font
                series1.Font = new System.Drawing.Font("Arial", 6, System.Drawing.FontStyle.Bold);
                // Set labels color
                series1.FontColor = System.Drawing.Color.Black;

            }
            else {
                //Set Data Point Label
                series1["LabelStyle"] = "Bottom";
                // Set labels font
                series1.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                // Set labels color
                series1.FontColor = System.Drawing.Color.Black;
                // Set ColumnLabel Style
                series1["ColumnLabelStyle"] = "Horizontal";
            }

            if (logic == "PERCENTAGE") {
                // Set Y axis labels format
                ChartSiteComparison.ChartAreas["Default"].AxisY.LabelStyle.Format = "P0";
                // Set series point labels format
                ChartSiteComparison.Series["Default"].LabelFormat = "P2";
            }
            else {
                //Restore decimal formatting - since we're using ViewState for charts, formatting won't be reset automatically
                ChartSiteComparison.ChartAreas["Default"].AxisY.LabelStyle.Format = "D";
                ChartSiteComparison.Series["Default"].LabelFormat = "D";
            }

            //Set X Axis style
            ChartSiteComparison.ChartAreas["Default"].AxisX.LabelsAutoFit = false;
            ChartSiteComparison.ChartAreas["Default"].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
            ChartSiteComparison.ChartAreas["Default"].AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            ChartSiteComparison.ChartAreas["Default"].AxisX.Interlaced = false;
            ChartSiteComparison.ChartAreas["Default"].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            ChartSiteComparison.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -45;

            ChartSiteComparison.ChartAreas["Default"].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
            ChartSiteComparison.ChartAreas["Default"].AxisY.LineColor = System.Drawing.Color.Gray;
            ChartSiteComparison.ChartAreas["Default"].AxisY.Interlaced = true;
            ChartSiteComparison.ChartAreas["Default"].AxisY.InterlacedColor = System.Drawing.Color.FromArgb(20, 20, 20, 20);
            ChartSiteComparison.ChartAreas["Default"].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);

            //Disable Variable Label Intervals - force showing every X Value
            ChartSiteComparison.ChartAreas["Default"].AxisX.Interval = 1;

            // Enable AntiAliasing for either Text and Graphics or just Graphics
            ChartSiteComparison.AntiAliasing = AntiAliasing.All;

        }

        #endregion

        #region "Report Selection"

        protected void SetReportSelection(int selectedLogic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id, int car_segment_id, int car_class_id, int car_group_id, DateTime start_date,
        int dayOfWeek, int dateRange) {
            //Set Report Selection User Control
            //Logic
            this.UserControlReportSelections.Logic = selectedLogic;

            //Country
            if (country == "-1") {
                this.UserControlReportSelections.Country = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Country = country;
            }

            //Select logic
            switch (selectedLogic) {

                case (int)ReportSettings.OptionLogic.CMS:
                    //CMS Pool
                    if (cms_pool_id == -1) {
                        this.UserControlReportSelections.CMS_Pool = Resources.lang.ReportSettingsALLSelection;
                    }
                    else {
                        this.UserControlReportSelections.CMS_Pool = this.UserControlReportSettings.CMS_Pool;
                    }
                    //CMS Location Group
                    if (cms_location_group_id == -1) {
                        this.UserControlReportSelections.CMS_Location_Group = Resources.lang.ReportSettingsALLSelection;
                    }
                    else {
                        this.UserControlReportSelections.CMS_Location_Group = this.UserControlReportSettings.CMS_Location_Group;
                    }

                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    //OPS Region
                    if (ops_region_id == -1) {
                        this.UserControlReportSelections.OPS_Region = Resources.lang.ReportSettingsALLSelection;
                    }
                    else {
                        this.UserControlReportSelections.OPS_Region = this.UserControlReportSettings.OPS_Region;
                    }
                    //OPS Area
                    if (ops_area_id == -1) {
                        this.UserControlReportSelections.OPS_Area = Resources.lang.ReportSettingsALLSelection;
                    }
                    else {
                        this.UserControlReportSelections.OPS_Area = this.UserControlReportSettings.OPS_Area;
                    }
                    break;
            }

            //Car Segment
            if (car_segment_id == -1) {
                this.UserControlReportSelections.Car_Segment = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Car_Segment = this.UserControlReportSettings.Car_Segment;
            }

            //Car Class
            if (car_class_id == -1) {
                this.UserControlReportSelections.Car_Class = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Car_Class = this.UserControlReportSettings.Car_Class;
            }

            //Car Group
            if (car_group_id == -1) {
                this.UserControlReportSelections.Car_Group = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Car_Group = this.UserControlReportSettings.Car_Group;
            }


            this.UserControlReportSelections.StartDate = string.Format("{0:dd/MM/yyyy}", start_date);
            this.UserControlReportSelections.DayOfWeek = ReportSettings.GetDayOfWeek(dayOfWeek);
            this.UserControlReportSelections.DateRange = UserControlReportSettings.DateRangeValue.Contains("Custom") ? "Custom" : ReportSettings.GetDateRange(dateRange);


            //Set Report Selection for tool and page
            this.UserControlReportSelections.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSelections.MarsPage = (int)ReportSettings.ReportSettingsPage.ATSiteComparison;
            //Display report selection
            this.UserControlReportSelections.ShowReportSelections();
        }

        #endregion
    }
}