using System;
using System.Collections.Generic;
using App.BLL;

using Dundas.Charting.WebControl;
using Mars.App.Classes.BLL.Common;

namespace App.AvailabilityTool.KPI {
    public partial class Default : PageBase {
        // Altered by Gavin 27-4-12 for MarsV3
        // Altered at line 212 uses ChartFactory.BuildLegend to create equation for percentage
        // Altered at line 270
        // Update at line 480 for ReportSettings to display "Custom" instead of date range

        #region "Page Events"

        protected void Page_Init(object sender, System.EventArgs e) {
            //Set Chart Page
            this.AvailibilityChartKPI.MARSPage = (int)ReportSettings.ReportSettingsPage.ATKPI;
            this.AvailibilityChartKPI.LoadChartFeatures();
        }


        protected void Page_Load(object sender, System.EventArgs e) {

            //Set Page title
            this.Page.Title = "MARS - Availability Tool";

            //Set report settings tool
            this.UserControlReportSettings.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSettings.MarsPage = (int)ReportSettings.ReportSettingsPage.ATKPI;

            if (!Page.IsPostBack) {


                //Set page informtion on usercontrol
                base.SetPageInformationTitle("ATKPI", this.UserControlPageInformation, false);

                //Set Last update / next update labels
                base.SetDataImportUpdateLabel((int)ImportDetails.ImportType.Availability, this.UserControlPageInformation);

                //Check if user as prefences
                CheckReportPreferencesSettings();

            }

            //Set validation group
            this.ModalConfirmKPI.ValidationGroup = "GenerateReport";


        }


        protected void CheckReportPreferencesSettings() {
            List<ReportPreferences> preferences = base.GetPreferencesSettings();

            if ((preferences != null)) {
                //There should be only one row
                foreach (ReportPreferences row in preferences) {
                    this.UserControlReportSettings.LoadReportSettingsControl(false);
                    this.UserControlReportSettings.SetUserDefaultSettingsAvailabilityTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id,
                        row.OPS_Region_Id, row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference,
                            row.Location, null, null, null, null, null, null);

                }
            }
            else {
                this.UserControlReportSettings.LoadReportSettingsControl(true);
            }

            //Update Update Panel
            this.UpdatePanelKPI.Update();

        }
        #endregion

        #region "Click Events"
        protected void ButtonGenerateReport_Click(object sender, System.EventArgs e) {
            //Generarte Report
            this.GenerateReport();
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
            int dateRangeValue = -1;
            string dateRange = null;
            string location = "-1";
            string fleetName = "-1";
            string kpiValue = null;
            string kpiText = null;

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

            location = this.UserControlReportSettings.Location;
            fleetName = this.UserControlReportSettings.Fleet_Name;
            kpiValue = this.UserControlReportSettings.AvailabilityKPIValue;
            kpiText = this.UserControlReportSettings.AvailabilityKPIText;

            availabilityDate = this.UserControlReportSettings.DateAvailability;

            //string grouping_criteria = ReportSettings.GetGroupingCriteria(dateRangeValue);

            DateTime startDate = DateTime.Now;
            if (UserControlReportSettings.DateRangeValue.Contains("Custom")) {
                startDate = UserControlReportSettings.customStartDate;
                availabilityDate = UserControlReportSettings.customEndDate;

                // work out the data range value by finfing the difference between the end and first date
                TimeSpan timeSpan = startDate - availabilityDate;
                dateRangeValue = timeSpan.Days;
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

            string grouping_criteria = ReportSettings.getRangeGroupingCriteria(dateRangeValue);

            string availabilityLogic = this.UserControlReportSettings.AvailabilityLogic;
            string selectBy = null;
            if (availabilityLogic == "NUMERIC") {
                selectBy = "NUMERIC";
            }
            else {
                selectBy = "PERCENTAGE";
            }
            //Set Last Selection Value
            base.SaveLastSelectionToSession(logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id);

            //Load Data

            System.Data.DataTable tblGroups = AvailabilityKPI.GetKPIData(kpiValue, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, fleetName, car_segment_id, car_class_id,
            car_group_id, startDate, availabilityDate, grouping_criteria, selectBy);

            this.GenerateChart(tblGroups, dateRangeValue.ToString(), selectBy, kpiText, kpiValue);

            //Set report Selection
            this.SetReportSelection(logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, car_segment_id, car_class_id, car_group_id, location,
            startDate, dateRangeValue);


            //Log Usage Statistics
            base.LogUsageStatistics((int)ReportStatistics.ReportName.AvailabilityKPI, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location);

            this.UpdatePanelKPI.Update();

        }
        #endregion

        #region "Chart Events"

        protected void GenerateChart(System.Data.DataTable dataValues, string grouping_criteria, string logic, string kpi_text, string kpi_value) {
            // logic is value or percentage
            // kpi_text is the value of the dropdownlist (right of generate report button)
            // kpi_value us the value from the dropdownlist (as above)

            int rowCount = dataValues.Rows.Count;
            if (rowCount >= 1) {
                this.AvailibilityChartKPI.Visible = true;
                this.EmptDataTemplateKPI.Visible = false;
            }
            else {
                this.EmptDataTemplateKPI.Visible = true;
                this.AvailibilityChartKPI.Visible = false;
            }

            Dundas.Charting.WebControl.Chart ChartKPI = (Dundas.Charting.WebControl.Chart)this.AvailibilityChartKPI.FindControl("DundasChartAvailability");
            ChartKPI.Series.Clear();

            ChartKPI.Titles.Clear();
            Dundas.Charting.WebControl.Title ctitle = new Dundas.Charting.WebControl.Title();
            ctitle.Text = Resources.lang.KPI + ": " + kpi_text;
            ctitle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            ctitle.Alignment = System.Drawing.ContentAlignment.TopLeft;
            ChartKPI.Titles.Add(ctitle);

            // ---- Alteration by Gavin ----
            BLL.ChartFactory.BuildLegend bl = new BLL.ChartFactory.BuildLegend("Formulae");
            int legendIndex = bl.indexOf(ChartKPI);
            if (legendIndex > -1) // delete at index
                ChartKPI.Legends.RemoveAt(legendIndex);

            //Add a item to reset zoom
            Dundas.Charting.WebControl.Title resetZoomTitle = new Dundas.Charting.WebControl.Title();
            resetZoomTitle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            resetZoomTitle.Alignment = System.Drawing.ContentAlignment.BottomRight;
            resetZoomTitle.Docking = Docking.Bottom;
            resetZoomTitle.ToolTip = Resources.lang.ChartZoomFeature;
            resetZoomTitle.DockOffset = 1;
            resetZoomTitle.MapAreaAttributes = "onclick=\"" + ChartKPI.CallbackManager.GetCallbackEventReference("ResetZoom", "ResetZoom") + "\"";
            resetZoomTitle.Text = Resources.lang.ChartZoomText;
            resetZoomTitle.BackImage = "~/App.Images/reset_zoom.png";
            resetZoomTitle.BackImageMode = ChartImageWrapMode.Unscaled;
            resetZoomTitle.BackImageAlign = ChartImageAlign.BottomRight;
            ChartKPI.Titles.Add(resetZoomTitle);

            // has been changed to a range to coincide with the changes in the ReportSettings - getGroupingCriteria, changed to getRangeGroupingCriteria
            // convert the grouping criteria to a range
            int groupingCriteriaRange = Convert.ToInt32(grouping_criteria);

            string xAxisValue = "REP_MONTH"; // default

            if (groupingCriteriaRange >= -180) {
                xAxisValue = "REP_WEEK_OF_YEAR";
            }
            if (groupingCriteriaRange >= -30) {
                xAxisValue = "REP_DATE";
            }

            // Initializes a new instance of the DataView class
            System.Data.DataView firstView = new System.Data.DataView(dataValues);

            switch (kpi_value) {
                case "OperationalUtilization":
                case "Overdues":
                case "IdleFleet":
                case "RentableFleetOnPeak":
                    //Declare chart series
                    Series series1 = ChartKPI.Series.Add("KPI");
                    Series seriesMin = ChartKPI.Series.Add("Min");
                    Series seriesMax = ChartKPI.Series.Add("Max");
                    Series seriesAvg = ChartKPI.Series.Add("Avg");

                    // ---- Altered by Gavin ----   
                    ChartKPI.Legends.Add(bl.getLegend(kpi_value, logic));

                    // Since the DataView implements IEnumerable, pass the reader directly into
                    // the DataBind method with the name of the Columns selected in the query    
                    series1.Points.DataBindXY(firstView, xAxisValue, firstView, "KPI");

                    decimal min = (decimal)0.0;
                    decimal max = (decimal)0.0;
                    decimal avg = (decimal)0.0;

                    if (dataValues.Rows.Count > 0) {
                        min = Math.Round(Convert.ToDecimal(dataValues.Compute("MIN(KPI)", "KPI>=0")), 2);
                        max = Math.Round(Convert.ToDecimal(dataValues.Compute("MAX(KPI)", "KPI>=0")), 2);
                        avg = Math.Round(Convert.ToDecimal(dataValues.Compute("AVG(KPI)", "KPI>=0")), 2);
                    }

                    foreach (DataPoint point in series1.Points) {
                        seriesMin.Points.AddXY(point.XValue, min);
                        seriesMax.Points.AddXY(point.XValue, max);
                        seriesAvg.Points.AddXY(point.XValue, avg);
                    }


                    //Set series colors
                    series1.Color = System.Drawing.Color.Navy;
                    seriesMin.Color = System.Drawing.Color.Red;
                    seriesMax.Color = System.Drawing.Color.Green;
                    seriesAvg.Color = System.Drawing.Color.Cyan;
                    break;
                case "FleetStatus":
                case "AvailableFleet":
                case "UnavailableOperationalFleet":
                case "UnavailableNonOperationalFleet":
                    //Count how many series was retrieved by DB query
                    int seriesStartIndex = 4;
                    // Set to 4 because the first three columns contain date info

                    for (int index1 = seriesStartIndex; index1 <= dataValues.Columns.Count - 1; index1++) {
                        Series newSeries = ChartKPI.Series.Add(dataValues.Columns[index1].ColumnName);
                        newSeries.Points.DataBindXY(firstView, xAxisValue, firstView, dataValues.Columns[index1].ColumnName);
                        newSeries.Color = Charts.GetColourForColumn((dataValues.Columns[index1].ColumnName.Replace("_", " ")), (int)ReportSettings.ReportSettingsTool.Availability);
                    }

                    break;
                default:
                    break;
            }

            if (logic == "NUMERIC") {
                //Restore decimal formatting - because we're using ViewState for charts, formatting won't be reset automatically
                ChartKPI.ChartAreas["Default"].AxisY.LabelStyle.Format = "D";
            }
            else {
                // Set Y axis labels format
                ChartKPI.ChartAreas["Default"].AxisY.LabelStyle.Format = "P0";
            }

            //Set series properties
            int index = 0;
            foreach (Series chartSeries in ChartKPI.Series) {
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
                if (xAxisValue == "REP_DATE") {
                    chartSeries.XValueType = ChartValueTypes.DateTime;
                }
                else {
                    chartSeries.XValueType = ChartValueTypes.Int;
                }

                // Set series point labels format

                chartSeries.LabelFormat = logic == "NUMERIC" ? string.Empty : "P2";
                

                chartSeries.ShowInLegend = true;

                index += 1;
            }



            //Set X Axis style
            ChartKPI.ChartAreas["Default"].AxisX.LabelsAutoFit = true;
            ChartKPI.ChartAreas["Default"].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
            ChartKPI.ChartAreas["Default"].AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            ChartKPI.ChartAreas["Default"].AxisX.Interlaced = false;
            ChartKPI.ChartAreas["Default"].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            ChartKPI.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -45;

            ChartKPI.ChartAreas["Default"].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
            ChartKPI.ChartAreas["Default"].AxisY.LineColor = System.Drawing.Color.Gray;
            ChartKPI.ChartAreas["Default"].AxisY.Interlaced = true;
            ChartKPI.ChartAreas["Default"].AxisY.InterlacedColor = System.Drawing.Color.FromArgb(20, 20, 20, 20);
            ChartKPI.ChartAreas["Default"].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);

            //Disable Variable Label Intervals - force showing every X Value
            ChartKPI.ChartAreas["Default"].AxisX.Interval = 1;

            // Enable AntiAliasing for either Text and Graphics or just Graphics
            ChartKPI.AntiAliasing = AntiAliasing.All;


        }

        #endregion

        #region "Report Selection"

        protected void SetReportSelection(int selectedLogic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id, int car_segment_id, int car_class_id, int car_group_id, string location,
        DateTime start_date, int dateRange) {
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

            //Location
            if (location == "-1") {
                this.UserControlReportSelections.Location = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Location = location;
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
            this.UserControlReportSelections.DateRange = UserControlReportSettings.DateRangeValue.Contains("Custom") ? "Custom" : ReportSettings.GetDateRange(dateRange);


            //Set Report Selection for tool and page
            this.UserControlReportSelections.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSelections.MarsPage = (int)ReportSettings.ReportSettingsPage.ATKPI;
            //Display report selection
            this.UserControlReportSelections.ShowReportSelections();
        }

        #endregion
    }
}