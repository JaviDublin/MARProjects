using System;
using System.Collections.Generic;
using App.BLL;
using Dundas.Charting.WebControl;
using Mars.App.Classes.BLL.Common;

namespace App.AvailabilityTool.FleetStatus
{
    public partial class Default : PageBase
    {
        // Updated : 17th Feb 12
        // Generates the Fleet status for the availability tool
        // Update at around line 130 to accept custom end and start date
        // Update at line 180 for ReportSettings to display "Custom" instead of date range
        // GOLD, PREDELIVERY & CARSALES removed from Fleet status chart. Real hank with if ... continue; at line 446

        #region "Page Events"

        protected void Page_Init(object sender, System.EventArgs e)
        {
            //Set Chart Page
            this.AvailibilityChartFleetStatus.MARSPage = (int)ReportSettings.ReportSettingsPage.ATFleetStatus; // Change property has it has no constructor enum is 6
            this.AvailibilityChartFleetStatus.LoadChartFeatures();
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.Page.Title = "MARS - Availability Tool"; //Set Page title
            this.UserControlReportSettings.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability; //Set report settings tool
            this.UserControlReportSettings.MarsPage = (int)ReportSettings.ReportSettingsPage.ATFleetStatus;

            if (!Page.IsPostBack)
            { // New Page
                base.SetPageInformationTitle("ATFleetStatus", this.UserControlPageInformation, false); //Set page informtion on usercontrol
                base.SetDataImportUpdateLabel((int)ImportDetails.ImportType.Availability, this.UserControlPageInformation); //Set Last update / next update labels
                List<ReportPreferences> queryStringResults = base.CheckForQueryString(); //Check for query string - returns 0 true and returns -1 for no query string
                if ((queryStringResults != null))
                {
                    foreach (ReportPreferences row in queryStringResults)
                    {
                        this.UserControlReportSettings.LoadReportSettingsControl(false);
                        this.UserControlReportSettings.SetUserDefaultSettingsAvailabilityTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id, row.OPS_Region_Id, row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference,
                        row.Location, null, null, Convert.ToInt32(row.FleetStatus), Conversions.ConvertStringToDate(row.DateValue), Convert.ToInt32(row.DayOfWeek), Convert.ToInt32(row.DateRange));
                    }
                    this.UserControlReportSettings.AvailabilityGenerateReport(); //Load Data
                }
                else
                {
                    CheckReportPreferencesSettings(); //Check if user as prefences
                    SessionHandler.ClearFleetStatusSessions();
                }
            }
            this.ModalConfirmFleetStatus.ValidationGroup = "GenerateReport"; //Set validation group

        }

        protected void CheckReportPreferencesSettings()
        {
            List<ReportPreferences> preferences = base.GetPreferencesSettings();
            if ((preferences != null))
            {
                foreach (ReportPreferences row in preferences)
                { //There should be only one row
                    this.UserControlReportSettings.LoadReportSettingsControl(false);
                    this.UserControlReportSettings.SetUserDefaultSettingsAvailabilityTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id, row.OPS_Region_Id,
                        row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference, row.Location, null, null, null, null, null, null);
                }
            }
            else
            {
                this.UserControlReportSettings.LoadReportSettingsControl(true);
            }
            //Update Update Panel
            this.UpdatePanelFleetStatus.Update();
        }
        #endregion

        #region "Click Events"
        protected void ButtonGenerateReport_Click(object sender, System.EventArgs e)
        {
            if (Page.IsPostBack)
            { //Reset Datatable Session Value
                SessionHandler.AvailabilityFleetStatusDataTable = null;
            }
            GenerateReport();
        }

        protected void GenerateReport()
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
            int dayOfWeekValue = -1;
            string dayOfWeek = null;
            int dateRangeValue = -1;
            string dateRange = null;
            string location = "-1";
            string fleetName = "-1";

            //Pass values from usercontrol
            logic = this.UserControlReportSettings.Logic;
            country = this.UserControlReportSettings.Country;

            switch (UserControlReportSettings.Logic)
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
            fleetName = this.UserControlReportSettings.Fleet_Name;
            availabilityDate = this.UserControlReportSettings.DateAvailability;
            dayOfWeekValue = this.UserControlReportSettings.DayOfWeekValue;
            dayOfWeek = this.UserControlReportSettings.DayOfWeek;
            DateTime startDate = DateTime.Now;
            if (UserControlReportSettings.DateRangeValue.Contains("Custom"))
            {
                startDate = UserControlReportSettings.customStartDate;
                availabilityDate = UserControlReportSettings.customEndDate;
            }
            else
            {
                dateRangeValue = Convert.ToInt32(this.UserControlReportSettings.DateRangeValue);
                dateRange = this.UserControlReportSettings.DateRange;
                if (dateRangeValue == -1)
                {
                    startDate = availabilityDate.AddDays(dateRangeValue + 1);
                }
                else
                {
                    startDate = availabilityDate.AddDays(dateRangeValue);
                }
            }
            //Set Last Selection Value
            base.SaveLastSelectionToSession(logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id);
            //Load Data
            System.Data.DataTable dataTable = new System.Data.DataTable();

            if ((SessionHandler.AvailabilityFleetStatusDataTable != null))
            {
                dataTable = SessionHandler.AvailabilityFleetStatusDataTable;
            }
            else
            {
                var userLoggedOn = Session["UserLoggedOn"] == null ? string.Empty : Session["UserLoggedOn"].ToString();
                dataTable = (AvailabilityFleetStatus.GetFleetStatusData(country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, fleetName, car_segment_id, car_class_id, car_group_id,
                            startDate, availabilityDate, dayOfWeekValue, userLoggedOn));


                //Add datatable to session variable for user to navigate back from carsearch
                SessionHandler.AvailabilityFleetStatusDataTable = dataTable;
                //Create query string
                this.CreateQueryString(UserControlReportSettings.Logic, UserControlReportSettings.Country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, fleetName, car_segment_id, car_group_id,
                                        car_class_id, startDate, dayOfWeekValue, dateRangeValue);
                //Log Usage Statistics
                base.LogUsageStatistics((int)ReportStatistics.ReportName.AvailabilityFleetStatus, UserControlReportSettings.Country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location);
            }

            //Generate Chart
            this.GenerateChart(dataTable);
            //Set report Selection
            this.SetReportSelection(UserControlReportSettings.Logic, UserControlReportSettings.Country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, car_segment_id, car_class_id, car_group_id, location,
            startDate, dayOfWeekValue, dateRangeValue);
            this.UpdatePanelFleetStatus.Update();
        }

        protected void CreateQueryString(int logic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id, string location, string fleetname, int car_segment_id, int car_group_id,
        int car_class_id, DateTime startDate, int dayOfWeek, int dateRange)
        {
            switch (logic)
            {
                case (int)ReportSettings.OptionLogic.CMS:
                    SessionHandler.AvailabilityFleetStatusRedirectQueryString = this.ResolveUrl("~/App.Site/Availability/CarSearch/") + "?"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OptionLogic) + "=" + logic.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Country) + "=" + country + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Pool_Id) + "=" + cms_pool_id.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Location_Group_Code) + "=" + cms_location_group_id + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Location) + "=" + (location ?? "-1") + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Segment_Id) + "=" + car_segment_id.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Class_Id) + "=" + car_class_id.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Group_Id) + "=" + car_group_id.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetName) + "=" + fleetname + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateValue) + "=" + string.Format("{0:ddMMyyyyhhmmss}", startDate) + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DayOfWeek) + "=" + dayOfWeek.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateRange) + "=" + dateRange.ToString() + "&";
                    break;

                case (int)ReportSettings.OptionLogic.OPS:
                    SessionHandler.AvailabilityFleetStatusRedirectQueryString = this.ResolveUrl("~/App.Site/Availability/CarSearch/") + "?"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OptionLogic) + "=" + logic.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Country) + "=" + country + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Region_Id) + "=" + ops_region_id.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Area_Id) + "=" + ops_area_id.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Location) + "=" + (location ?? "-1") + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Segment_Id) + "=" + car_segment_id.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Class_Id) + "=" + car_class_id.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Group_Id) + "=" + car_group_id.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetName) + "=" + fleetname + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateValue) + "=" + startDate.ToString("ddMMyyyyhhmmss") + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DayOfWeek) + "=" + dayOfWeek.ToString() + "&"
                        + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateRange) + "=" + dateRange.ToString() + "&";
                    break;
            }
        }
        #endregion

        #region "Chart Events"

        protected void GenerateChart(System.Data.DataTable dataValues)
        {
            // For MarsV3 tasked with removing CARSALES, GOLD and PREDELIVERY
            // Not as easy as it looks! The datatable datavalues arrives full of all 27 columns of data
            // The method accesses certain values by index - CARSALES index 1, GOLD index 23  and PREDELIVERY index 24

            // if they're in the table then, remove the columns CARSALES, GOLD and PREDELIVERY
            if (dataValues.Columns.Contains("CARSALES"))
            {
                dataValues.Columns.Remove("CARSALES");
            }
            if (dataValues.Columns.Contains("GOLD"))
            {
                dataValues.Columns.Remove("GOLD");
            }
            if (dataValues.Columns.Contains("PREDELIVERY"))
            {
                dataValues.Columns.Remove("PREDELIVERY");
            }

            int rowCount = dataValues.Rows.Count;
            if (rowCount >= 1)
            {
                this.AvailibilityChartFleetStatus.Visible = true;
                this.EmptyDataTemplateFleetStatus.Visible = false;
            }
            else
            {
                this.EmptyDataTemplateFleetStatus.Visible = true;
                this.AvailibilityChartFleetStatus.Visible = false;
            }
            Dundas.Charting.WebControl.Chart ChartFleetStatus = (Dundas.Charting.WebControl.Chart)this.AvailibilityChartFleetStatus.FindControl("DundasChartAvailability");

            //Points names
            string[] arrNames = {
		                        "TOTAL FLEET", // 0
		                        //"CARSALES",
		                        "CU", // 1
		                        "HA", // 2
		                        "HL", // 3
		                        "LL", // 4
		                        "NC", // 5
		                        "PL", // 6
		                        "TC", // 7
		                        "SV", // 8
		                        "WS", // 9
		                        "OPERATIONAL FLEET", // 10
		                        "BD", // 11
		                        "MM", // 12
		                        "TW", // 13
		                        "TB", // 14
		                        "FS", // 15
		                        "RL", // 16
		                        "RP", // 17
		                        "TN", // 18
		                        "AVAILABLE FLEET", // 19
		                        "RT", // 20
		                        "SU", // 21
		                        //"GOLD",
		                        //"PREDELIVERY",
		                        "OVERDUE", // 22
		                        "ON RENT" //23
	                        };

            //Points range - it is a car count in each group which will decide on column's height
            bool[] arrXActive = {
		                        true,
		                       // true, // carsales
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true, // available fleet
		                        true, // rt
		                        true, // su
		                        //true, // gold
		                        //true, // predelivery
		                        true, // overdue
		                        true // on rent //
	                        };
            int[] arrYValues = new int[24]; // changed from 27 to 24

            for (int i = 0; i <= dataValues.Rows[0].ItemArray.Length - 1; i++)
            {
                if (object.ReferenceEquals(dataValues.Rows[0].ItemArray[i], System.DBNull.Value))
                {
                    arrYValues[i] = 0;
                }
                else
                {
                    arrYValues[i] = Convert.ToInt32(dataValues.Rows[0].ItemArray[i]);
                }
            }

            //Points Y2 value - calculate the top end of the column
            //Points are drawn based on previous point's end - from top to bottom
            int[] arrY2Values = {
		                        arrYValues[0],
		                        // was carsales
		                        arrYValues[0],
		                        arrYValues[0] - arrYValues[1],
		                        arrYValues[0] - arrYValues[1] - arrYValues[2],
		                        arrYValues[0] - arrYValues[1] - arrYValues[2] - arrYValues[3],
		                        arrYValues[0] - arrYValues[1] - arrYValues[2] - arrYValues[3] - arrYValues[4],
		                        arrYValues[0] - arrYValues[1] - arrYValues[2] - arrYValues[3] - arrYValues[4] - arrYValues[5],
		                        arrYValues[0] - arrYValues[1] - arrYValues[2] - arrYValues[3] - arrYValues[4] - arrYValues[5] - arrYValues[6],
		                        arrYValues[0] - arrYValues[1] - arrYValues[2] - arrYValues[3] - arrYValues[4] - arrYValues[5] - arrYValues[6] - arrYValues[7],
		                        arrYValues[0] - arrYValues[1] - arrYValues[2] - arrYValues[3] - arrYValues[4] - arrYValues[5] - arrYValues[6] - arrYValues[7] - arrYValues[8],
		                        arrYValues[10],
		                        arrYValues[10],
		                        arrYValues[10] - arrYValues[11],
		                        arrYValues[10] - arrYValues[11] - arrYValues[12],
		                        arrYValues[10] - arrYValues[11] - arrYValues[12] - arrYValues[13],
		                        arrYValues[10] - arrYValues[11] - arrYValues[12] - arrYValues[13] - arrYValues[14],
		                        arrYValues[10] - arrYValues[11] - arrYValues[12] - arrYValues[13] - arrYValues[14] - arrYValues[15],
		                        arrYValues[10] - arrYValues[11] - arrYValues[12] - arrYValues[13] - arrYValues[14] - arrYValues[15] - arrYValues[16],
		                        arrYValues[10] - arrYValues[11] - arrYValues[12] - arrYValues[13] - arrYValues[14] - arrYValues[15] - arrYValues[16] - arrYValues[17],
		                        arrYValues[19],
		                        arrYValues[19],
		                        arrYValues[19] - arrYValues[20], // su
		                        //arrYValues[20] - arrYValues[21] - arrYValues[22], // gold
		                        //arrYValues[20] - arrYValues[21] - arrYValues[22] - arrYValues[23], // predelivery
		                        arrYValues[19] - arrYValues[20] - arrYValues[21], // overdue
		                        arrYValues[23] // on rent
	                        };

            //Store data in Viewstate so the points can be freely removed / recreated / recalculated later
            ViewState["arrNames"] = arrNames;
            ViewState["arrYValues"] = arrYValues;
            ViewState["arrY2Values"] = arrY2Values;
            ViewState["arrXActive"] = arrXActive;
            Session["arrXActive"] = arrXActive;
            //is is stored in session as it changes between page reloads!

            ChartFleetStatus.Series.Clear();
            //reset chart title
            ChartFleetStatus.Titles.Clear();
            Dundas.Charting.WebControl.Title ctitle = new Dundas.Charting.WebControl.Title();
            ctitle.Text = Resources.lang.FleetStatus;
            ctitle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            ctitle.Alignment = System.Drawing.ContentAlignment.TopLeft;
            ChartFleetStatus.Titles.Add(ctitle);

            //Clear adn add new legends
            ChartFleetStatus.Legends.Clear();
            ChartFleetStatus.Legends.Add("Default");
            //Calculate and display KPI legend
            decimal shop = default(decimal);
            decimal overdue = default(decimal);
            decimal utilInclOverdue = default(decimal);
            decimal utilization = default(decimal);

            //Protect from division by zero when dividing by OPERATIONAL_FLEET
            if (arrYValues[10] == 0)
            {
                shop = 0;
                utilInclOverdue = 0;
                utilization = 0;
            }
            else
            {
                shop = ((arrYValues[11] + arrYValues[12] + arrYValues[13]) * 100) / arrYValues[10];
                //(BD + MM + TW)*100 / OPERATIONAL_FLEET
                utilInclOverdue = ((arrYValues[22] + arrYValues[23]) * 100) / arrYValues[10];
                //(OVERDUE + ON_RENT)*100 / OPERATIONAL_FLEET
                utilization = (arrYValues[23] * 100) / arrYValues[10];
                //ON_RENT * 100 / OPERATIONAL_FLEET
            }
            //OVERDUE * 100 / AVAILABLE_FLEET
            if (arrYValues[19] == 0)
            {
                overdue = 0;
            }
            else
            {
                overdue = (arrYValues[22] * 100) / arrYValues[19];
            }

            AddKPILegend(arrYValues[0], arrYValues[10], arrYValues[19], arrYValues[23], shop, overdue, utilInclOverdue, utilization);

            //Declare chart series
            Series series1 = ChartFleetStatus.Series.Add("FLEET STATUS");

            // Initializes a new instance of the DataView class
            //Dim firstView As New System.Data.DataView(dataValues)

            //Set series colors
            series1.Color = System.Drawing.Color.Black;

            //Set series visibility
            series1.Enabled = true;

            //Set series properties
            int index = 0;
            foreach (Series chartSeries in ChartFleetStatus.Series)
            {
                // Set range column chart type
                chartSeries.Type = SeriesChartType.RangeColumn;

                //Set series tooltip - we don't use it, tooltip is defined on points level
                //chartSeries.ToolTip = #VALY2
                // Set the side-by-side drawing style (false = stack one bar on another or not)
                chartSeries["DrawSideBySide"] = "false";

                chartSeries.BorderWidth = 2;

                //Set x value type - otherwise XAxis labels might be reset after a postback
                chartSeries.XValueType = ChartValueTypes.String;

                chartSeries.ShowInLegend = false;

                index += 1;
            }

            int indexX = 0;

            //Set the initial Active / Inactive status for columns
            for (int index1 = 9; index1 >= 1; index1 += -1)
            {
                arrXActive[index1] = false;
                //ToggleVisibilityOfLegendItem(index1)
            }
            //Disable GOLD and PREDELIVERY
            //arrXActive[23] = false;
            //arrXActive[24] = false;
            //Disable columns with value 0
            for (int index1 = 0; index1 <= arrYValues.Length - 1; index1++)
            {
                if (arrYValues[index1] == 0)
                {
                    arrXActive[index1] = false;
                }
            }

            for (indexX = 0; indexX <= arrXActive.Length - 1; indexX++)
            {
                // Add custom legend item with image
                LegendItem legendItem = new LegendItem();
                //legend item is a sigle row in legend table
                LegendCell legendCell1 = new LegendCell();
                //checkbox
                LegendCell legendCell2 = new LegendCell();
                //point name
                LegendCell legendCell3 = new LegendCell();
                //percentage

                legendItem.BackImageTranspColor = System.Drawing.Color.Red;
                legendCell1.CellType = LegendCellType.Image;
                if (arrXActive[indexX] == true)
                {
                    legendCell1.Image = "~/App.Images/chk_checked.png";
                }
                else
                {
                    legendCell1.Image = "~/App.Images/chk_unchecked.png";
                }

                legendCell1.ImageTranspColor = System.Drawing.Color.Red;
                legendCell2.CellType = LegendCellType.Text;
                legendCell2.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
                legendCell2.Text = arrNames[indexX];
                legendCell2.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
                legendCell2.TextColor = Charts.GetColourForColumn(arrNames[indexX], (int)ReportSettings.ReportSettingsTool.Availability);
                legendCell3.CellType = LegendCellType.Text;
                legendCell3.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
                switch (arrNames[indexX])
                {
                    case "TOTAL FLEET":
                        //Clear checkbox image
                        legendCell1.Image = "";
                        legendCell3.Text = arrYValues[indexX].ToString();
                        legendCell2.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                        legendCell3.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                        break;
                    //case "CARSALES":
                    case "CU":
                    case "HA":
                    case "HL":
                    case "LL":
                    case "NC":
                    case "PL":
                    case "TC":
                    case "SV":
                    case "WS":

                        legendCell3.Text = (arrYValues[0] == 0) ? "0" : (Math.Round((decimal)(arrYValues[indexX] * 100) / arrYValues[0], 1)).ToString() + "%" + " (" + arrYValues[indexX].ToString() + ")";
                        //percent of TOTAL FLEET  
                        break;
                    case "OPERATIONAL FLEET":
                        //Clear checkbox image
                        legendCell1.Image = "";
                        legendCell3.Text = (arrYValues[0] == 0) ? "0" : (Math.Round((decimal)(arrYValues[indexX] * 100) / arrYValues[0], 1)).ToString() + "%" + " (" + arrYValues[indexX].ToString() + ")";
                        //percent of TOTAL FLEET 
                        legendCell2.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                        legendCell3.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                        break;
                    case "BD":
                    case "MM":
                    case "TW":
                    case "TB":
                    case "FS":
                    case "RL":
                    case "RP":
                    case "TN":
                        legendCell3.Text = (arrYValues[10] == 0) ? "0" : (Math.Round((decimal)(arrYValues[indexX] * 100) / arrYValues[10], 1)).ToString() + "%" + " (" + arrYValues[indexX].ToString() + ")";
                        //percent of OPERATIONAL FLEET    
                        break;
                    case "AVAILABLE FLEET":
                        //Clear checkbox image
                        legendCell1.Image = "";
                        legendCell3.Text = (arrYValues[10] == 0) ? "0" : (Math.Round((decimal)(arrYValues[indexX] * 100) / arrYValues[10], 1)).ToString() + "%" + " (" + arrYValues[indexX].ToString() + ")";
                        //percent of OPERATIONAL FLEET 
                        legendCell2.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                        legendCell3.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                        break;
                    case "RT":
                    case "SU":
                    //case "GOLD":
                    //case "PREDELIVERY":
                    case "OVERDUE":
                        legendCell3.Text = (arrYValues[19] == 0) ? "0" : (Math.Round((decimal)(arrYValues[indexX] * 100) / arrYValues[19], 1)).ToString() + "%" + " (" + arrYValues[indexX].ToString() + ")";
                        //percent of AVAILABLE FLEET   
                        break;
                    case "ON RENT":
                        //Clear checkbox image
                        legendCell1.Image = "";
                        legendCell3.Text = (arrYValues[19] == 0) ? "0" : (Math.Round((decimal)(arrYValues[indexX] * 100) / arrYValues[19], 1)).ToString() + "%" + " (" + arrYValues[indexX].ToString() + ")";
                        //percent of AVAILABLE FLEET 
                        legendCell2.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                        legendCell3.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                        break;
                    default:
                        break;
                    //There should be no other cases
                }
                legendCell3.Alignment = System.Drawing.ContentAlignment.MiddleRight;
                legendCell3.TextColor = Charts.GetColourForColumn(arrNames[indexX], (int)ReportSettings.ReportSettingsTool.Availability);
                legendItem.Cells.Add(legendCell1);
                legendItem.Cells.Add(legendCell2);
                legendItem.Cells.Add(legendCell3);
                legendItem.MapAreaAttributes = "onclick=\"" + ChartFleetStatus.CallbackManager.GetCallbackEventReference("LegendClick", indexX.ToString()) + "\"";

                ChartFleetStatus.Legends["Default"].CustomItems.Add(legendItem);
            }

            //Call CombineColumns procedure for any point - it will redraw all columns according to arrIsActive table
            CombineColumns(1, false);

            //Set X Axis style
            ChartFleetStatus.ChartAreas["Default"].AxisX.LabelsAutoFit = false;
            ChartFleetStatus.ChartAreas["Default"].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
            ChartFleetStatus.ChartAreas["Default"].AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            ChartFleetStatus.ChartAreas["Default"].AxisX.Interlaced = false;
            ChartFleetStatus.ChartAreas["Default"].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            ChartFleetStatus.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -45;

            ChartFleetStatus.ChartAreas["Default"].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
            ChartFleetStatus.ChartAreas["Default"].AxisY.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            ChartFleetStatus.ChartAreas["Default"].AxisY.Interlaced = true;
            ChartFleetStatus.ChartAreas["Default"].AxisY.InterlacedColor = System.Drawing.Color.FromArgb(20, 20, 20, 20);
            ChartFleetStatus.ChartAreas["Default"].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);


            //Disable Variable Label Intervals - force showing every X Value
            ChartFleetStatus.ChartAreas["Default"].AxisX.Interval = 1;

            // Enable AntiAliasing for either Text and Graphics or just Graphics
            ChartFleetStatus.AntiAliasing = AntiAliasing.All;




        }

        protected void CombineColumns(int indexX, bool pointIsChecked)
        {
            Dundas.Charting.WebControl.Chart ChartFleetStatus = (Dundas.Charting.WebControl.Chart)this.AvailibilityChartFleetStatus.FindControl("DundasChartAvailability");
            string[] arrNames = (string[])ViewState["arrNames"];
            int[] arrYValues = (int[])ViewState["arrYValues"];
            int[] arrY2Values = (int[])ViewState["arrY2Values"];
            bool[] arrXActive;
            if(Session["arrXActive"] == null)
            {
                arrXActive = new [] {
		                        true,
		                       // true, // carsales
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true,
		                        true, // available fleet
		                        true, // rt
		                        true, // su
		                        //true, // gold
		                        //true, // predelivery
		                        true, // overdue
		                        true // on rent //
	                        };
            }
            else
            {
                arrXActive = (bool[])Session["arrXActive"];
            }
            
            //is is stored in session as it changes between page reloads!


            arrXActive[indexX] = pointIsChecked;

            //Remove all points
            ChartFleetStatus.Series[0].Points.Clear();

            //And recreate them according to the changes
            for (indexX = 0; indexX <= arrXActive.Length - 1; indexX++)
            {
                if (arrXActive[indexX] == true)
                {
                    int pointIndex = ChartFleetStatus.Series[0].Points.AddXY(arrNames[indexX], arrY2Values[indexX] - arrYValues[indexX], arrY2Values[indexX]);
                    ChartFleetStatus.Series[0].Points[pointIndex].ToolTip = arrNames[indexX] + ": " + arrYValues[indexX];
                    ChartFleetStatus.Series[0].Points[pointIndex].Name = arrNames[indexX];
                    ChartFleetStatus.Series[0].Points[pointIndex].Empty = !(arrXActive[indexX]);

                    //Set points' colours
                    ChartFleetStatus.Series[0].Points[pointIndex].Color = Charts.GetColourForColumn(arrNames[indexX], (int)ReportSettings.ReportSettingsTool.Availability);
                    ChartFleetStatus.Series[0].Points[pointIndex].Href = SessionHandler.AvailabilityFleetStatusRedirectQueryString +
                            QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Operstat) + "=" + arrNames[indexX] + "&" +
                            QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetStatus) + "=1";
                }
            }
            //Update information about columns active / inactive status in session variable
            Session["arrXActive"] = arrXActive;
        }

        protected void AddKPILegend(int totalFleet, int operationalFleet, int availableFleet, int onRent, decimal shop, decimal overdue, decimal UtilInclOverdue, decimal utilization)
        {
            Dundas.Charting.WebControl.Chart ChartFleetStatus = (Dundas.Charting.WebControl.Chart)this.AvailibilityChartFleetStatus.FindControl("DundasChartAvailability");
            ChartFleetStatus.Legends.Add("KPI");
            //Chart1.Legends("KPI").Title = Resources.lang.DisplayStatus
            // Set legend style
            ChartFleetStatus.Legends["KPI"].LegendStyle = LegendStyle.Table;
            // Set legend docking
            ChartFleetStatus.Legends["KPI"].Docking = LegendDocking.Top;
            // Set legend alignment
            ChartFleetStatus.Legends["KPI"].Alignment = System.Drawing.StringAlignment.Center;
            ChartFleetStatus.Legends["KPI"].BackColor = System.Drawing.Color.FromArgb(20, 20, 20, 20);
            ChartFleetStatus.Legends["KPI"].BorderColor = System.Drawing.Color.FromArgb(40, 40, 40, 40);
            ChartFleetStatus.Legends["KPI"].BorderWidth = 1;
            ChartFleetStatus.Legends["KPI"].TableStyle = LegendTableStyle.Wide;

            // Add custom legend item 
            LegendItem legendItem1 = new LegendItem();
            //legend item is a sigle row in legend table
            LegendItem legendItem2 = new LegendItem();
            //legend item is a sigle row in legend table
            LegendItem legendItem3 = new LegendItem();
            //legend item is a sigle row in legend table
            LegendItem legendItem4 = new LegendItem();
            //legend item is a sigle row in legend table

            //KPI
            legendItem1.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold), Resources.lang.KPI + ":", System.Drawing.ContentAlignment.MiddleLeft));
            //Total fleet
            legendItem1.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Resources.lang.TOTALFLEET, System.Drawing.ContentAlignment.MiddleLeft));
            //Total fleet count
            legendItem1.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), totalFleet.ToString(), System.Drawing.ContentAlignment.MiddleRight));
            //Empty cell
            legendItem1.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), "             ", System.Drawing.ContentAlignment.MiddleCenter));
            //Shop
            legendItem1.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Resources.lang.Shop, System.Drawing.ContentAlignment.MiddleLeft));
            //Shop count
            legendItem1.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Math.Round(shop, 1) + "%", System.Drawing.ContentAlignment.MiddleRight));
            //Empty cell
            legendItem2.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), "                   ", System.Drawing.ContentAlignment.MiddleLeft));
            //Operational fleet
            legendItem2.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Resources.lang.OPERATIONALFLEET, System.Drawing.ContentAlignment.MiddleLeft));
            //Operational fleet count
            legendItem2.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), operationalFleet.ToString(), System.Drawing.ContentAlignment.MiddleRight));
            //Empty cell
            legendItem2.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), "             ", System.Drawing.ContentAlignment.MiddleCenter));
            //Overdue
            legendItem2.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Resources.lang.OVERDUE, System.Drawing.ContentAlignment.MiddleLeft));
            //Overdue count
            legendItem2.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Math.Round(overdue, 1) + "%", System.Drawing.ContentAlignment.MiddleRight));
            //Empty cell
            legendItem3.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), "                 ", System.Drawing.ContentAlignment.MiddleLeft));
            //Available fleet
            legendItem3.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Resources.lang.AVAILABLEFLEET, System.Drawing.ContentAlignment.MiddleLeft));
            //Available fleet count
            legendItem3.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), availableFleet.ToString(), System.Drawing.ContentAlignment.MiddleRight));
            //Empty cell
            legendItem3.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), "             ", System.Drawing.ContentAlignment.MiddleCenter));
            //Util.Incl.Overdue
            legendItem3.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Resources.lang.UtilInclOverdue, System.Drawing.ContentAlignment.MiddleLeft));
            //Util.incl.overdue count
            legendItem3.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Math.Round(UtilInclOverdue, 1) + "%", System.Drawing.ContentAlignment.MiddleRight));
            //Empty cell
            legendItem4.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), "                   ", System.Drawing.ContentAlignment.MiddleLeft));
            //On rent
            legendItem4.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Resources.lang.ONRENT, System.Drawing.ContentAlignment.MiddleLeft));
            //On rent count
            legendItem4.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), onRent.ToString(), System.Drawing.ContentAlignment.MiddleRight));
            //Empty cell
            legendItem4.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), "             ", System.Drawing.ContentAlignment.MiddleCenter));
            //Utilization
            legendItem4.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), Resources.lang.Utilization, System.Drawing.ContentAlignment.MiddleLeft));
            //Utilization count
            legendItem4.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold), Math.Round(utilization, 1) + "%", System.Drawing.ContentAlignment.MiddleRight));

            ChartFleetStatus.Legends["KPI"].CustomItems.Add(legendItem1);
            ChartFleetStatus.Legends["KPI"].CustomItems.Add(legendItem2);
            ChartFleetStatus.Legends["KPI"].CustomItems.Add(legendItem3);
            ChartFleetStatus.Legends["KPI"].CustomItems.Add(legendItem4);
        }

        protected void AddReportParametersLegend(string country, string region, string pool, string branch, string fleet, string carClass, string carGroup, string rDate, string dayOfWeek, string dateRange)
        {
            Dundas.Charting.WebControl.Chart ChartFleetStatus = (Dundas.Charting.WebControl.Chart)this.AvailibilityChartFleetStatus.FindControl("DundasChartAvailability");
            ChartFleetStatus.Legends.Add("Parameters");
            //Chart1.Legends("KPI").Title = Resources.lang.DisplayStatus
            // Set legend style
            ChartFleetStatus.Legends["Parameters"].LegendStyle = LegendStyle.Table;
            // Set legend docking
            ChartFleetStatus.Legends["Parameters"].Docking = LegendDocking.Left;
            // Set legend alignment
            ChartFleetStatus.Legends["Parameters"].Alignment = System.Drawing.StringAlignment.Center;

            ChartFleetStatus.Legends["Parameters"].TableStyle = LegendTableStyle.Wide;

            // Add custom legend item 
            LegendItem legendItem1 = new LegendItem();
            //legend item is a sigle row in legend table
            LegendItem legendItem2 = new LegendItem();
            //legend item is a sigle row in legend table
            LegendItem legendItem3 = new LegendItem();
            //legend item is a sigle row in legend table


            //KPI
            legendItem1.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold), Resources.lang.Country + ":", System.Drawing.ContentAlignment.MiddleLeft));
            //Total fleet
            legendItem1.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), country, System.Drawing.ContentAlignment.MiddleRight));
            //Empty cell
            legendItem1.Cells.Add(CreateNewLegendCell(LegendCellType.Text, new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular), "             ", System.Drawing.ContentAlignment.MiddleCenter));

            ChartFleetStatus.Legends["Parameters"].CustomItems.Add(legendItem1);
            ChartFleetStatus.Legends["Parameters"].CustomItems.Add(legendItem2);
            ChartFleetStatus.Legends["Parameters"].CustomItems.Add(legendItem3);
        }

        private LegendCell CreateNewLegendCell(LegendCellType cellType, System.Drawing.Font cellFont, string text, System.Drawing.ContentAlignment cellAlignment)
        {

            LegendCell legendCell = new LegendCell();

            legendCell.CellType = cellType;
            legendCell.Font = cellFont;
            legendCell.Text = text;
            legendCell.Alignment = cellAlignment;

            return legendCell;
        }

        protected void ChartCallBack(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            // Handle the legend item click
            if (e.CommandName == "LegendClick")
            {
                // Get the index of the legend item that was clicked
                int index = int.Parse(e.CommandArgument.ToString());
                this.ToggleVisibilityOfLegendItem(index);
            }
        }

        private void ToggleVisibilityOfLegendItem(int index)
        {
            Dundas.Charting.WebControl.Chart ChartFleetStatus = (Dundas.Charting.WebControl.Chart)this.AvailibilityChartFleetStatus.FindControl("DundasChartAvailability");
            // Legend item result
            LegendItem legendItem = ChartFleetStatus.Legends["Default"].CustomItems[index];

            // Get the series item that was selected
            //Dim selectedPoint As DataPoint = Chart1.Series(0).Points(index)
            bool pointIsChecked = false;

            if (legendItem.Cells[0].Image == "~/App.Images/chk_checked.png")
            {
                //selectedPoint.Empty = True
                legendItem.Cells[0].Image = "~/App.Images/chk_unchecked.png";
                legendItem.Cells[0].ImageTranspColor = System.Drawing.Color.Red;
                pointIsChecked = false;
            }
            else
            {
                //selectedPoint.Empty = False
                legendItem.Cells[0].Image = "~/App.Images/chk_checked.png";
                legendItem.Cells[0].ImageTranspColor = System.Drawing.Color.Red;
                pointIsChecked = true;
            }
            this.CombineColumns(index, pointIsChecked);
        }
        #endregion

        #region "Report Selection"

        protected void SetReportSelection(int selectedLogic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id,
                                            int car_segment_id, int car_class_id, int car_group_id, string location, DateTime start_date, int dayOfWeek, int dateRange)
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
            this.UserControlReportSelections.DayOfWeek = ReportSettings.GetDayOfWeek(dayOfWeek);
            this.UserControlReportSelections.DateRange = UserControlReportSettings.DateRangeValue.Contains("Custom") ? "Custom" : ReportSettings.GetDateRange(dateRange);

            //Set Report Selection for tool and page
            this.UserControlReportSelections.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSelections.MarsPage = (int)ReportSettings.ReportSettingsPage.ATFleetStatus;
            //Display report selection
            this.UserControlReportSelections.ShowReportSelections();
        }

        #endregion

    }
}