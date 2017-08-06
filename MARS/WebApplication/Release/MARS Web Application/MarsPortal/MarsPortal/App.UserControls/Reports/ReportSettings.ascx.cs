using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Reports {
    ///TODO delete me
    public partial class ReportSettings : System.Web.UI.UserControl 
        
    {
        // Added "Custom" selection to DropDownListAvailabilityDateRange at approx 947 
        // and DropDownListAvailabilityDateRangeKPI at 959 also points to the same event below **
        // Added "Custom" to Historical Trend, Fleet Comparison and Site Comparison
        // ** Added DropDownListAvailabilityDateRange_SelectedIndexChanged event at approx line 2092(!)
        // Added customEndDate and customStartDate to properties (NB: the small-case 1st char is a mistake)
        // Added method in Worker methods getDate(DropDownList ddl) around line 2066
        // Changes done to logic for "Custom" selection at line 1632

        #region "Properties and Fields"

        private string _country;
        private int _logic;
        private int _cms_pool_id;
        private string _cms_pool;
        private int _cms_location_group_id;
        private string _cms_location_group;
        private int _ops_region_id;
        private string _ops_region;
        private int _ops_area_id;
        private string _ops_area;
        private string _location;
        private int _car_segment_id;
        private string _car_segment;
        private int _car_class_id;
        private string _car_class;
        private int _car_group_id;
        private string _car_group;
        private DateTime _statisticsStartDate;
        private DateTime _statisticsEndDate;
        private string _racfID;
        private string _ownArea;
        private string _modelCode;
        private string _status;
        private int _noRev;
        private string _fleet_name;
        private string _carSearch_SelectBy;
        private string _dayOfWeek;
        private int _dayOfWeekValue;
        private string _dateRange;
        private string _dateRangeValue;
        private DateTime _dateAvailability;
        private string _availabilityTopic;
        private string _availabilityTopicValue;
        private string _availabilityLogic;
        private string _availabilityTopicOperstat;
        private string _availabilityKPIValue;

        private string _availabilityKPIText;

        public bool OnRentTypeVisible
        {
            set
            {
                lblOnRentType.Visible = value;
                ddlOnRentType.Visible = value;
            }
        }

        public string OnRentTypeSelected
        {
            get { return ddlOnRentType.SelectedValue; }
            set { ddlOnRentType.SelectedValue = value; }
        }

        public string Country 
        {
            get { return _country; }
        }

        public int Logic 
        {
            get { return _logic; }
        }

        public int CMS_Pool_Id 
        {
            get { return _cms_pool_id; }
        }

        public string CMS_Pool 
        {
            get { return _cms_pool; }
        }

        public int CMS_Location_Group_Id 
        {
            get { return _cms_location_group_id; }
        }

        public string CMS_Location_Group 
        {
            get { return _cms_location_group; }
        }

        public int OPS_Region_Id 
        {
            get { return _ops_region_id; }
        }

        public string OPS_Region 
        {
            get { return _ops_region; }
        }

        public int OPS_Area_Id 
        {
            get { return _ops_area_id; }
        }

        public string OPS_Area 
        {
            get { return _ops_area; }
        }

        public string Location 
        {
            get { return _location; }
        }

        public int Car_Segment_Id 
        {
            get { return _car_segment_id; }
        }

        public string Car_Segment 
        {
            get { return _car_segment; }
        }

        public int Car_Class_Id 
        {
            get { return _car_class_id; }
        }

        public string Car_Class 
        {
            get { return _car_class; }
        }

        public int Car_Group_Id 
        {
            get { return _car_group_id; }
        }

        public string Car_Group 
        {
            get { return _car_group; }
        }

        public DateTime StatisticsStartDate 
        {
            get { return _statisticsStartDate; }
        }

        public DateTime StatisticsEndDate 
        {
            get { return _statisticsEndDate; }
        }

        public string RacfId 
        {
            get { return _racfID; }
        }

        public string OwnArea 
        {
            get { return _ownArea; }
        }

        public string ModelCode 
        {
            get { return _modelCode; }
        }

        public string Status 
        {
            get { return _status; }
        }

        public int NoRev 
        {
            get { return _noRev; }
        }

        public string Fleet_Name 
        {
            get { return _fleet_name; }
        }

        public string CarSearch_SelectBy 
        {
            get { return _carSearch_SelectBy; }
        }

        public string DayOfWeek 
        {
            get { return _dayOfWeek; }
        }

        public int DayOfWeekValue 
        {
            get { return _dayOfWeekValue; }
        }

        public string DateRange 
        {
            get { return _dateRange; }
        }

        public string DateRangeValue 
        {
            get { return _dateRangeValue; }
        }

        public DateTime DateAvailability 
        {
            get { return _dateAvailability; }
        }

        public string AvailabilityTopic 
        {
            get { return _availabilityTopic; }
        }

        public string AvailabilityTopicValue 
        {
            get { return _availabilityTopicValue; }
        }

        public string AvailabilityLogic 
        {
            get { return _availabilityLogic; }
        }

        public string AvailabilityTopicOperStat 
        {
            get { return _availabilityTopicOperstat; }
        }

        public string AvailabilityKPIValue 
        {
            get { return _availabilityKPIValue; }
        }

        public string AvailabilityKPIText 
        {
            get { return _availabilityKPIText; }
        }

        // -- Added --
        public DateTime customStartDate { get; private set; }
        
        public DateTime customEndDate { get; private set; }
        
        public Button ButtonAvailabilityGenerateReport; // added to access
        
        #endregion

        #region "Usercontrol Settings"

        private bool _loadDropDownLists;
        
        private int _marsTool;

        private int _marsPage;
        
        public int MarsTool 
        {
            get { return _marsTool; }
            set { _marsTool = value; }
        }

        public int MarsPage 
        {
            get { return _marsPage; }
            set { _marsPage = value; }
        }

        public void LoadReportSettingsControl(bool loadDropDownLists) 
        {
            //Set the report settings panel called by page
            _loadDropDownLists = loadDropDownLists;
            SetReportSettingsControl();
        }

        /// <summary>
        /// Check which tool and page is calling the usercontrol
        /// </summary>
        /// <remarks></remarks>

        protected void SetReportSettingsControl() 
        {
            //Check which Mars tool we are using and which page is calling the report settings
            switch (_marsTool) 
            {
                case (int)App.BLL.ReportSettings.ReportSettingsTool.Availability:
                    //Availability Tool and selected page
                    //Set usercontrol for availability tool and selected page
                    this.SetReportSettingsAvailability(MarsPage);

                    break;

                case (int)App.BLL.ReportSettings.ReportSettingsTool.Statistics:
                    //Statistics Tool
                    //Set usercontrol for statistics
                    this.SetReportSettingsStatistics(MarsPage);

                    break;
                
                case (int)App.BLL.ReportSettings.ReportSettingsTool.NonRevenue:
                    //Availability Tool and selected page
                    //Set usercontrol for availability tool and selected page
                    this.SetReportSettingsAvailability(MarsPage);

                    break;
            }

        }


        /// <summary>
        /// Set up report settings panel for Availability tool 
        /// Set visibility of certain controls based on which page has called it
        /// </summary>
        /// <param name="marsPage"></param>
        /// <remarks></remarks>

        protected void SetReportSettingsAvailability(int marsPage) 
        {
            //Set visibility to report title title
            this.PanelTitleReportSettings.Visible = true;
            //Set visiblity of availability panel options
            this.PanelAvailabilityReportSettings.Visible = true;


            //Set Initial visibility for logic
            if (!Page.IsPostBack) 
            {
                this.RadioButtonListAvailabilityLogic.SelectedValue = ((int)App.BLL.ReportSettings.OptionLogic.CMS).ToString();
                this.RadioButtonAvailabilityLSTWWD.Checked = true;
                this.SetLogicVisibility((int)App.BLL.ReportSettings.ReportSettingsTool.Availability, (int)App.BLL.ReportSettings.OptionLogic.CMS);
                this.LoadData(_marsTool, marsPage);
                if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATHistoricalTrend)) 
                {
                    this.DropDownListAvailabilityDateRange.SelectedIndex = 1;
                }
                if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATKPI)) 
                {
                    this.DropDownListAvailabilityDateRangeKPI.SelectedIndex = 1;
                    this.DropDownListAvailabilityKPI.SelectedIndex = 0;
                    this.RadioButtonListAvailabilityValue.SelectedValue = "PERCENTAGE";
                    this.RadioButtonListAvailabilityValue.Enabled = false;
                }
                if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATKPIDownload)) 
                {
                    this.DropDownListAvailabilityDateRangeKPI.SelectedIndex = 2;
                    this.DropDownListAvailabilityDateRangeKPI.Enabled = false;

                }
            }
            //Check which page has called the usercontrol and display items
            //depending on selected page
            switch (marsPage) {
                case (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetStatus:
                    //Hide Controls No needed
                    this.LabelAvailabilityOwnarea.Visible = false;
                    this.LabelAvailabilityModelcode.Visible = false;
                    this.LabelAvailabilityNoRev.Visible = false;
                    this.DropDownListAvailabilityNoRev.Visible = false;
                    this.LabelAvailabilityStatus.Visible = false;
                    this.LabelAvailabilityLSTWWD.Visible = false;
                    this.RadioButtonAvailabilityLSTWWD.Visible = false;
                    this.LabelAvailabilityDUEWWD.Visible = false;
                    this.RadioButtonAvailabilityDUEWWD.Visible = false;
                    this.ButtonAvailabilityDownloadReport.Visible = false;
                    this.ButtonAvailabilityKPIDownload.Visible = false;
                    this.LabelAvailabilityTopic.Visible = false;
                    this.DropDownListAvailabilityTopic.Visible = false;
                    this.DropDownListAvailabilityKPI.Visible = false;
                    this.RadioButtonListAvailabilityValue.Visible = false;

                    break;
                case (int)App.BLL.ReportSettings.ReportSettingsPage.ATHistoricalTrend:
                    //Hide Controls No needed
                    this.LabelAvailabilityOwnarea.Visible = false;
                    this.LabelAvailabilityModelcode.Visible = false;
                    this.LabelAvailabilityNoRev.Visible = false;
                    this.DropDownListAvailabilityNoRev.Visible = false;
                    this.LabelAvailabilityStatus.Visible = false;
                    this.LabelAvailabilityLSTWWD.Visible = false;
                    this.RadioButtonAvailabilityLSTWWD.Visible = false;
                    this.LabelAvailabilityDUEWWD.Visible = false;
                    this.RadioButtonAvailabilityDUEWWD.Visible = false;
                    this.ButtonAvailabilityDownloadReport.Visible = false;
                    this.ButtonAvailabilityKPIDownload.Visible = false;
                    this.LabelAvailabilityTopic.Visible = false;
                    this.DropDownListAvailabilityTopic.Visible = false;
                    this.DropDownListAvailabilityKPI.Visible = false;

                    break;

                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison):
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison):
                    //Hide Controls No needed
                    this.LabelAvailabilityOwnarea.Visible = false;
                    this.LabelAvailabilityModelcode.Visible = false;
                    this.LabelAvailabilityNoRev.Visible = false;
                    this.DropDownListAvailabilityNoRev.Visible = false;
                    this.LabelAvailabilityStatus.Visible = false;
                    this.LabelAvailabilityLSTWWD.Visible = false;
                    this.RadioButtonAvailabilityLSTWWD.Visible = false;
                    this.LabelAvailabilityDUEWWD.Visible = false;
                    this.RadioButtonAvailabilityDUEWWD.Visible = false;
                    this.ButtonAvailabilityDownloadReport.Visible = false;
                    this.ButtonAvailabilityKPIDownload.Visible = false;
                    this.DropDownListAvailabilityKPI.Visible = false;

                    break;
                case (int)App.BLL.ReportSettings.ReportSettingsPage.ATKPI:
                    //Hide Controls No needed
                    this.LabelAvailabilityDayOfWeek.Visible = false;
                    this.DropDownListAvailabilityDayOfWeek.Visible = false;
                    this.LabelAvailabilityOwnarea.Visible = false;
                    this.LabelAvailabilityModelcode.Visible = false;
                    this.LabelAvailabilityNoRev.Visible = false;
                    this.DropDownListAvailabilityNoRev.Visible = false;
                    this.LabelAvailabilityStatus.Visible = false;
                    this.LabelAvailabilityLSTWWD.Visible = false;
                    this.RadioButtonAvailabilityLSTWWD.Visible = false;
                    this.LabelAvailabilityDUEWWD.Visible = false;
                    this.RadioButtonAvailabilityDUEWWD.Visible = false;
                    this.LabelAvailabilityTopic.Visible = false;
                    this.DropDownListAvailabilityTopic.Visible = false;
                    this.ButtonAvailabilityDownloadReport.Visible = false;
                    this.DropDownListAvailabilityDateRange.Visible = false;
                    this.LabelAvailabilityDateRange.Visible = false;
                    this.DropDownListAvailabilityDateRangeKPI.Visible = true;
                    this.LabelAvailabilityDateRangeKPI.Visible = true;

                    break;

                case (int)App.BLL.ReportSettings.ReportSettingsPage.ATKPIDownload:
                    //Hide Controls No needed
                    this.LabelAvailabilityDayOfWeek.Visible = false;
                    this.DropDownListAvailabilityDayOfWeek.Visible = false;
                    this.LabelAvailabilityOwnarea.Visible = false;
                    this.LabelAvailabilityModelcode.Visible = false;
                    this.LabelAvailabilityNoRev.Visible = false;
                    this.DropDownListAvailabilityNoRev.Visible = false;
                    this.LabelAvailabilityStatus.Visible = false;
                    this.LabelAvailabilityLSTWWD.Visible = false;
                    this.RadioButtonAvailabilityLSTWWD.Visible = false;
                    this.LabelAvailabilityDUEWWD.Visible = false;
                    this.RadioButtonAvailabilityDUEWWD.Visible = false;
                    this.ButtonAvailabilityGenerateReport.Visible = false;
                    this.ButtonAvailabilityKPIDownload.Visible = false;
                    this.LabelAvailabilityTopic.Visible = false;
                    this.DropDownListAvailabilityTopic.Visible = false;
                    this.DropDownListAvailabilityKPI.Visible = false;
                    this.DropDownListAvailabilityDateRange.Visible = false;
                    this.LabelAvailabilityDateRange.Visible = false;
                    this.DropDownListAvailabilityDateRangeKPI.Visible = true;
                    this.LabelAvailabilityDateRangeKPI.Visible = true;
                    this.RadioButtonListAvailabilityValue.Visible = false;

                    break;

                case (int)App.BLL.ReportSettings.ReportSettingsPage.ATCarSearch:
                case (int)App.BLL.ReportSettings.ReportSettingsPage.NRCarSearch:
                    //Hide Controls No needed
                    this.LabelAvailabilityDate.Visible = false;
                    this.DatePickerTextBoxAvailabilityDate.Visible = false;
                    this.LabelAvailabilityDayOfWeek.Visible = false;
                    this.DropDownListAvailabilityDayOfWeek.Visible = false;
                    this.LabelAvailabilityDateRange.Visible = false;
                    this.DropDownListAvailabilityDateRange.Visible = false;
                    this.LabelAvailabilityDayOfWeek.Visible = false;
                    this.DropDownListAvailabilityDayOfWeek.Visible = false;
                    this.ButtonAvailabilityDownloadReport.Visible = false;
                    this.RadioButtonListAvailabilityValue.Visible = false;
                    this.ButtonAvailabilityKPIDownload.Visible = false;
                    this.LabelAvailabilityTopic.Visible = false;
                    this.DropDownListAvailabilityTopic.Visible = false;
                    this.DropDownListAvailabilityKPI.Visible = false;
                    this.PopupCheckBoxListAvailabilityOwnArea.Visible = true;
                    this.PopupCheckBoxListAvailabilityModelcode.Visible = false;
                    this.PopupCheckBoxListStatus.Visible = true;

                    break;
            }

            

            switch (marsPage) {
                case (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison:
                    //Hide location dropdownlist and disable autopostback on other controls
                    this.DropDownListAvailabilityLocations.Visible = false;
                    this.DropDownListAvailabilityOPSArea.AutoPostBack = false;
                    this.DropDownListAvailabilityCMSLocationGroup.AutoPostBack = false;
                    this.LabelAvailabilityBranch.Visible = false;

                    break;
                case (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison:
                    //Hide car group dropdownlist and disable autopostback on other controls
                    this.DropDownListAvailabilityCarGroup.Visible = false;
                    this.DropDownListAvailabilityCarClass.AutoPostBack = false;
                    this.LabelAvailabilityCarGroup.Visible = false;
                    break;
            }

            string clientId = null;
            string uniqueId = null;
            if (_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATKPIDownload) 
            {
                clientId = this.DropDownListAvailabilityCountry.ClientID;
                uniqueId = this.ButtonAvailabilityDownloadReport.UniqueID;
                this.ButtonAvailabilityDownloadReport.Attributes.Add("onclick", "return ShowConfirm('" + clientId + "','" + uniqueId + "')");
            }
            else 
            {
                //Set Javascript function for Generate Report Click
                clientId = this.DropDownListAvailabilityCountry.ClientID;
                uniqueId = this.ButtonAvailabilityGenerateReport.UniqueID;
                this.ButtonAvailabilityGenerateReport.Attributes.Add("onclick", "return ShowConfirm('" + clientId + "','" + uniqueId + "')");
            }



        }

        /// <summary>
        /// Set up report settings panel for usage report on all tools
        /// </summary>
        /// <remarks></remarks>

        protected void SetReportSettingsStatistics(int marsPage) 
        {
            //Set visibility to report title title
            this.PanelTitleReportSettings.Visible = true;
            this.PanelStatisticsReportSettings.Visible = true;

            //Set Initial visibility for logic
            if (!Page.IsPostBack) 
            {
                this.RadioButtonListStatisticsLogic.SelectedValue = ((int)App.BLL.ReportSettings.OptionLogic.CMS).ToString();
                this.SetLogicVisibility((int)App.BLL.ReportSettings.ReportSettingsTool.Statistics, (int)App.BLL.ReportSettings.OptionLogic.CMS);
                this.LoadData(_marsTool, marsPage);
            }

            //Set Javascript function for Generate Report Click
            string clientId = this.DropDownListStatisticsCountry.ClientID;
            string uniqueId = this.ButtonStatisticsGenerateReport.UniqueID;
            this.ButtonStatisticsGenerateReport.Attributes.Add("onclick", "return ShowConfirm('" + clientId + "','" + uniqueId + "')");


        }

        /// <summary>
        /// Load drop down list depending on requested tool and page
        /// </summary>
        /// <param name="tool"></param>
        /// <param name="marsPage"></param>
        /// <remarks></remarks>

        protected void LoadData(int tool, int marsPage) 
        {
            //Load Drop Down lists
            switch (tool) 
            {
                case (int)App.BLL.ReportSettings.ReportSettingsTool.Availability:
                case (int)App.BLL.ReportSettings.ReportSettingsTool.NonRevenue:
                    if ((_loadDropDownLists == true)) 
                    {
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.AllNew, true);
                    }

                    this.RebindAvailabilityFleet();
                    //load specific items
                    switch (marsPage) 
                    {
                        case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetStatus):
                        case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATHistoricalTrend):
                        case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison):
                        case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison):
                            this.RebindAvailabilityDayOfWeek();
                            this.RebindAvailabilityDateRange();

                            break;
                        case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATKPI):
                            this.RebindAvailabilityDateRangeKPI();
                            this.RebindAvailabilityKPI();

                            break;
                        case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATCarSearch):
                        case ((int)App.BLL.ReportSettings.ReportSettingsPage.NRCarSearch):
                            this.RebindAvailabilityOwnArea();
                            this.RebindAvailabilityStatus();
                            this.RebindAvailabilityNoRev();

                            break;
                        case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATKPIDownload):
                            this.RebindAvailabilityDateRangeKPI();

                            break;
                    }

                    break;

                case (int)App.BLL.ReportSettings.ReportSettingsTool.Statistics:
                    if ((_loadDropDownLists == true)) {
                        this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.AllNew, true);
                    }

                    break;
            }

            //Load specific items for each page
            switch (marsPage) 
            {
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison):
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison):
                    this.RebindAvailabilityTopics();

                    break;
            }
        }

        #endregion

        #region "DropDownList Events"

        #region "Availability"

        // DropDownListAvailabilityCountry SelectedIndexChanged event handling method
        protected void DropDownListAvailabilityCountry_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            string selectedCountry = Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue);
            if (selectedCountry == "-1") 
            {
                //Databind all options
                this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.AllNew, true);

                // Implementation of CAL presentation logic
                // The assumption here being that this is the "*** All ***" selection
                DropDownListCAL.SelectedIndex = 0;
            }
            else {

                //Databind options for specific country
                int selectedLogic = Convert.ToInt32(this.RadioButtonListAvailabilityLogic.SelectedValue);

                switch (selectedLogic) 
                {
                    case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSPool, true);

                        break;
                    case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSRegion, true);

                        break;
                }

                this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarSegment, true);

                if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATCarSearch))
                {
                    RebindAvailabilityModelCode();
                }

                if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.NRCarSearch))
                {
                    RebindAvailabilityModelCode();
                }

            }
        }


        protected void DropDownListAvailabilityCMSPool_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSLocationGroup, true);
        }

        protected void DropDownListAvailabilityOPSRegion_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSArea, true);
        }


        protected void DropDownListAvailabilityCMSLocationGroup_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.Location, true);
        }

        protected void DropDownListAvailabilityOPSArea_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.Location, true);
        }

        protected void DropDownListAvailabilityCarSegment_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarClass, true);
        }

        protected void DropDownListAvailabilityCarClass_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarGroup, true);
        }


        protected void DropDownListAvailabilityTopic_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            if (this.DropDownListAvailabilityTopic.SelectedValue.Contains("UTILISATION")) 
            {
                this.RadioButtonListAvailabilityValue.Enabled = false;
                this.RadioButtonListAvailabilityValue.SelectedValue = "PERCENTAGE";
            }
            else 
            {
                this.RadioButtonListAvailabilityValue.Enabled = true;
            }
            this.UpdatePanelAvailabilityReportSettingsFooter.Update();

        }


        protected void DropDownListAvailabilityKPI_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            if (this.DropDownListAvailabilityKPI.SelectedIndex == 0) 
            {
                this.RadioButtonListAvailabilityValue.SelectedValue = "PERCENTAGE";
                this.RadioButtonListAvailabilityValue.Enabled = false;
            }
            else 
            {
                this.RadioButtonListAvailabilityValue.Enabled = true;
            }
            this.UpdatePanelAvailabilityReportSettingsFooter.Update();

        }


        protected void RebindDropDownListsAvailability(int dropDownListOption, bool allSelected) 
        {
            //Check which logic is selected
            int selectedLogic = Convert.ToInt32(this.RadioButtonListAvailabilityLogic.SelectedValue);

            // Put Cal into the ReportLookups class for the different queries
            ReportLookups.CAL = DropDownListCAL.SelectedValue;

            switch (dropDownListOption) 
            {
                case (int)App.BLL.ReportSettings.ReportOptions.CMSPool:
                    //Clear Items
                    this.DropDownListAvailabilityCMSPool.Items.Clear();
                    //Databind CMS Pools
                    this.DropDownListAvailabilityCMSPool.DataSource = ReportLookups.GetCMSPools(Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue));
                    this.DropDownListAvailabilityCMSPool.DataBind();
                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCMSPool, allSelected);
                    //Clear children below and add all items
                    this.DropDownListAvailabilityCMSLocationGroup.Items.Clear();
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCMSLocationGroup, allSelected);
                    //Check if mars page is site comparsion
                    //if don't use location
                    if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison)))
                    {
                        this.DropDownListAvailabilityLocations.Items.Clear();
                        this.AddInitialValueToDropDown(this.DropDownListAvailabilityLocations, allSelected);
                    }

                    break;

                case (int)App.BLL.ReportSettings.ReportOptions.CMSLocationGroup:
                    //Clear Items
                    this.DropDownListAvailabilityCMSLocationGroup.Items.Clear();
                    //Databind CMS Location Groups
                    if (!(Convert.ToInt32(this.DropDownListAvailabilityCMSPool.SelectedValue) == -1)) {
                        this.DropDownListAvailabilityCMSLocationGroup.DataSource = ReportLookups.GetCMSLocationGroups((Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue)), (Convert.ToInt32(this.DropDownListAvailabilityCMSPool.SelectedValue)));
                        this.DropDownListAvailabilityCMSLocationGroup.DataBind();
                    }

                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCMSLocationGroup, allSelected);
                    //Clear children below and add all items
                    //Check if mars page is site comparsion
                    //if don't use location
                    if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison)))
                    {
                        this.DropDownListAvailabilityLocations.Items.Clear();
                        this.AddInitialValueToDropDown(this.DropDownListAvailabilityLocations, allSelected);
                    }

                    break;

                case (int)App.BLL.ReportSettings.ReportOptions.OPSRegion:
                    //Clear Items
                    this.DropDownListAvailabilityOPSRegion.Items.Clear();
                    //Databind OPS Regions
                    this.DropDownListAvailabilityOPSRegion.DataSource = ReportLookups.GetOPSRegions(Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue));

                    this.DropDownListAvailabilityOPSRegion.DataBind();
                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityOPSRegion, allSelected);
                    //Clear children below and add all items
                    this.DropDownListAvailabilityOPSArea.Items.Clear();
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityOPSArea, allSelected);
                    //Check if mars page is site comparsion
                    //if don't use location
                    if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison)))
                    {
                        this.DropDownListAvailabilityLocations.Items.Clear();
                        this.AddInitialValueToDropDown(this.DropDownListAvailabilityLocations, allSelected);
                    }

                    break;

                case (int)App.BLL.ReportSettings.ReportOptions.OPSArea:
                    //Clear Items
                    this.DropDownListAvailabilityOPSArea.Items.Clear();
                    //Databind OPS Areas 
                    if (!(Convert.ToInt32(this.DropDownListAvailabilityOPSRegion.SelectedValue) == -1)) {
                        this.DropDownListAvailabilityOPSArea.DataSource = ReportLookups.GetOPSAreas(Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue), Convert.ToInt32(this.DropDownListAvailabilityOPSRegion.SelectedValue));

                        this.DropDownListAvailabilityOPSArea.DataBind();
                    }

                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityOPSArea, allSelected);
                    //Clear children below and add all items
                    //Check if mars page is site comparsion
                    //if don't use location
                    if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison)))
                    {
                        this.DropDownListAvailabilityLocations.Items.Clear();
                        this.AddInitialValueToDropDown(this.DropDownListAvailabilityLocations, allSelected);
                    }

                    break;

                case (int)App.BLL.ReportSettings.ReportOptions.Location:
                    //Clear Items
                    this.DropDownListAvailabilityLocations.Items.Clear();
                    //Check Logic
                    switch (selectedLogic) {

                        case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                            //Set location datasource
                            if (!(Convert.ToString(this.DropDownListAvailabilityCMSLocationGroup.SelectedValue) == "-1")) {
                                this.DropDownListAvailabilityLocations.DataSource = ReportLookups.GetLocations((Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue)), -1, -1, (Convert.ToInt32(this.DropDownListAvailabilityCMSPool.SelectedValue)), int.Parse(this.DropDownListAvailabilityCMSLocationGroup.SelectedValue), selectedLogic);
                                //Databind location                                
                                this.DropDownListAvailabilityLocations.DataBind();
                            }
                            break;

                        case (int)App.BLL.ReportSettings.OptionLogic.OPS:

                            //Set location datasource
                            if (!(Convert.ToInt32(this.DropDownListAvailabilityOPSArea.SelectedValue) == -1)) {
                                this.DropDownListAvailabilityLocations.DataSource = ReportLookups.GetLocations((Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue)), (Convert.ToInt32(this.DropDownListAvailabilityOPSRegion.SelectedValue)), (Convert.ToInt32(this.DropDownListAvailabilityOPSArea.SelectedValue)), -1, -1, selectedLogic);
                                //Databind location
                                this.DropDownListAvailabilityLocations.DataBind();
                            }
                            break;
                    }


                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityLocations, allSelected);

                    break;
                case (int)App.BLL.ReportSettings.ReportOptions.CarSegment:
                    this.DropDownListAvailabilityCarSegment.Items.Clear();
                    //Databind Car Segments
                    this.DropDownListAvailabilityCarSegment.DataSource = ReportLookups.GetCarSegments(Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue));

                    this.DropDownListAvailabilityCarSegment.DataBind();
                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCarSegment, allSelected);
                    //Clear children below and add all items
                    this.DropDownListAvailabilityCarClass.Items.Clear();
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCarClass, allSelected);
                    //Check if mars page is fleet comparsion
                    //if don't use group
                    if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison)))
                    {
                        this.DropDownListAvailabilityCarGroup.Items.Clear();
                        this.AddInitialValueToDropDown(this.DropDownListAvailabilityCarGroup, allSelected);
                    }

                    break;

                case (int)App.BLL.ReportSettings.ReportOptions.CarClass:
                    this.DropDownListAvailabilityCarClass.Items.Clear();
                    //Databind Car Classes
                    if (!(Convert.ToInt32(this.DropDownListAvailabilityCarSegment.SelectedValue) == -1)) {
                        this.DropDownListAvailabilityCarClass.DataSource = ReportLookups.GetCarClasses((Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue)), (Convert.ToInt32(this.DropDownListAvailabilityCarSegment.SelectedValue)));

                        this.DropDownListAvailabilityCarClass.DataBind();
                    }

                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCarClass, allSelected);
                    //Check if mars page is fleet comparsion
                    //if don't use group
                    if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison)))
                    {
                        this.DropDownListAvailabilityCarGroup.Items.Clear();
                        this.AddInitialValueToDropDown(this.DropDownListAvailabilityCarGroup, allSelected);
                    }

                    break;

                case (int)App.BLL.ReportSettings.ReportOptions.CarGroup:
                    //Clear current items 
                    this.DropDownListAvailabilityCarGroup.Items.Clear();
                    //Databind Car Groups
                    if (!(Convert.ToInt32(this.DropDownListAvailabilityCarClass.SelectedValue) == -1)) {
                        this.DropDownListAvailabilityCarGroup.DataSource = ReportLookups.GetCarGroups((Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue)), (Convert.ToInt32(this.DropDownListAvailabilityCarSegment.SelectedValue)), (Convert.ToInt32(this.DropDownListAvailabilityCarClass.SelectedValue)));

                        this.DropDownListAvailabilityCarGroup.DataBind();
                    }

                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCarGroup, allSelected);

                    break;
                case (int)App.BLL.ReportSettings.ReportOptions.AllNew:

                    // ensure the dropdownlist cal is at index 0 for ***all***
                    DropDownListCAL.SelectedIndex = 0;

                    //Clear all items
                    //Databind countries and add all items to each other list
                    this.DropDownListAvailabilityCMSPool.Items.Clear();
                    this.DropDownListAvailabilityCMSLocationGroup.Items.Clear();
                    this.DropDownListAvailabilityOPSRegion.Items.Clear();
                    this.DropDownListAvailabilityOPSArea.Items.Clear();
                    //Check if mars page is site comparsion
                    //if don't use location
                    if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison)))
                    {
                        this.DropDownListAvailabilityLocations.Items.Clear();
                    }

                    //Data bind countries list
                    this.DropDownListAvailabilityCountry.Items.Clear();
                    this.DropDownListAvailabilityCountry.DataSource = ReportLookups.GetCountries();
                    this.DropDownListAvailabilityCountry.DataBind();
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCountry, allSelected);

                    //For Car Segment / Car Class / Car Group Just show all when all countries are selected
                    this.DropDownListAvailabilityCarSegment.Items.Clear();
                    this.DropDownListAvailabilityCarClass.Items.Clear();

                    //Check if mars page is fleet comparsion
                    //if don't use car group
                    if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison)))
                    {
                        this.DropDownListAvailabilityCarGroup.Items.Clear();
                        this.AddInitialValueToDropDown(this.DropDownListAvailabilityCarGroup, allSelected);
                    }

                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCarSegment, allSelected);
                    this.AddInitialValueToDropDown(this.DropDownListAvailabilityCarClass, allSelected);

                    //Check which option logic we are using
                    switch (selectedLogic) {

                        case (int)App.BLL.ReportSettings.OptionLogic.CMS:

                            //Add initial item to cmspool and cmslocationgroup
                            this.AddInitialValueToDropDown(this.DropDownListAvailabilityCMSPool, allSelected);
                            this.AddInitialValueToDropDown(this.DropDownListAvailabilityCMSLocationGroup, allSelected);

                            break;

                        case (int)App.BLL.ReportSettings.OptionLogic.OPS:

                            //Add initial item to OPS Regions and OPS Areas
                            this.AddInitialValueToDropDown(this.DropDownListAvailabilityOPSRegion, allSelected);
                            this.AddInitialValueToDropDown(this.DropDownListAvailabilityOPSArea, allSelected);

                            break;
                    }

                    //Check if mars page is site comparsion
                    //if don't use location
                    if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison)))
                    {
                        this.AddInitialValueToDropDown(this.DropDownListAvailabilityLocations, allSelected);
                    }

                    if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATCarSearch))
                    {
                        RebindAvailabilityModelCode();
                    }

                    if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.NRCarSearch))
                    {
                        RebindAvailabilityModelCode();
                    }

                    break;
            }

            this.UpdatePanelReportSettings.Update();

        }


        protected void RebindAvailabilityFleet() 
        {
            this.DropDownListAvailabilityFleet.Items.Clear();
            this.DropDownListAvailabilityFleet.DataSource = ReportLookups.GetFleet();
            this.DropDownListAvailabilityFleet.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListAvailabilityFleet, true);
        }


        protected void RebindAvailabilityDayOfWeek() 
        {
            this.DropDownListAvailabilityDayOfWeek.Items.Clear();
            this.CreateNewListItem(this.DropDownListAvailabilityDayOfWeek, Resources.lang.Monday, "2");
            this.CreateNewListItem(this.DropDownListAvailabilityDayOfWeek, Resources.lang.Tuesday, "3");
            this.CreateNewListItem(this.DropDownListAvailabilityDayOfWeek, Resources.lang.Wednesday, "4");
            this.CreateNewListItem(this.DropDownListAvailabilityDayOfWeek, Resources.lang.Thursday, "5");
            this.CreateNewListItem(this.DropDownListAvailabilityDayOfWeek, Resources.lang.Friday, "6");
            this.CreateNewListItem(this.DropDownListAvailabilityDayOfWeek, Resources.lang.Saturday, "7");
            this.CreateNewListItem(this.DropDownListAvailabilityDayOfWeek, Resources.lang.Sunday, "1");
            this.AddInitialValueToDropDown(this.DropDownListAvailabilityDayOfWeek, true);
        }


        protected void RebindAvailabilityDateRange() 
        {
            this.DropDownListAvailabilityDateRange.Items.Clear();
            this.CreateNewListItem(this.DropDownListAvailabilityDateRange, Resources.lang.Oneday, "-1");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRange, Resources.lang.Previous7days, "-7");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRange, Resources.lang.Previous30days, "-30");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRange, Resources.lang.Previous90days, "-90");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRange, Resources.lang.Previous180days, "-180");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRange, Resources.lang.Previous365days, "-365");
            CreateNewListItem(DropDownListAvailabilityDateRange, Resources.lang.PreviousCustom, "Custom");
        }


        protected void RebindAvailabilityDateRangeKPI() 
        {
            this.DropDownListAvailabilityDateRangeKPI.Items.Clear();
            this.CreateNewListItem(this.DropDownListAvailabilityDateRangeKPI, Resources.lang.Oneday, "-1");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRangeKPI, Resources.lang.Previous7days, "-7");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRangeKPI, Resources.lang.Previous30days, "-30");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRangeKPI, Resources.lang.Previous90days, "-90");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRangeKPI, Resources.lang.Previous180days, "-180");
            this.CreateNewListItem(this.DropDownListAvailabilityDateRangeKPI, Resources.lang.Previous365days, "-365");
            CreateNewListItem(DropDownListAvailabilityDateRangeKPI, Resources.lang.PreviousCustom, "Custom");
        }


        protected void RebindAvailabilityKPI() 
        {
            this.DropDownListAvailabilityKPI.Items.Clear();
            this.CreateNewListItem(this.DropDownListAvailabilityKPI, Resources.lang.OperationalUtilization, "OperationalUtilization");
            this.CreateNewListItem(this.DropDownListAvailabilityKPI, Resources.lang.Overdues, "Overdues");
            this.CreateNewListItem(this.DropDownListAvailabilityKPI, Resources.lang.IdleFleet, "IdleFleet");
            this.CreateNewListItem(this.DropDownListAvailabilityKPI, Resources.lang.RentableFleetOnPeak, "RentableFleetOnPeak");
            this.CreateNewListItem(this.DropDownListAvailabilityKPI, Resources.lang.FleetStatus, "FleetStatus");
            this.CreateNewListItem(this.DropDownListAvailabilityKPI, Resources.lang.AVAILABLEFLEET, "AvailableFleet");
            this.CreateNewListItem(this.DropDownListAvailabilityKPI, Resources.lang.UnavailableOperationalFleet, "UnavailableOperationalFleet");
            this.CreateNewListItem(this.DropDownListAvailabilityKPI, Resources.lang.UnavailableNonOperationalFleet, "UnavailableNonOperationalFleet");

        }


        protected void RebindAvailabilityOwnArea() 
        {
            System.Web.UI.WebControls.CheckBoxList checkBoxListOwnArea = (System.Web.UI.WebControls.CheckBoxList)this.PopupCheckBoxListAvailabilityOwnArea.FindControl("CheckBoxListPopUp");
            checkBoxListOwnArea.DataTextField = "ownarea";
            checkBoxListOwnArea.DataValueField = "ownarea";
            checkBoxListOwnArea.Items.Clear();
            checkBoxListOwnArea.DataSource = ReportLookups.GetOwnareas();
            checkBoxListOwnArea.DataBind();
            foreach (ListItem item in checkBoxListOwnArea.Items) {
                item.Selected = false;
            }
            //Load the check box list settings
            this.PopupCheckBoxListAvailabilityOwnArea.LoadCheckBoxList();
        }


        protected void RebindAvailabilityStatus() 
        {
            System.Web.UI.WebControls.CheckBoxList checkBoxListStatus = (System.Web.UI.WebControls.CheckBoxList)this.PopupCheckBoxListStatus.FindControl("CheckBoxListPopUp");

            checkBoxListStatus.DataTextField = "OperStat_Name";
            checkBoxListStatus.DataValueField = "OperStat_Name";
            checkBoxListStatus.Items.Clear();
            checkBoxListStatus.DataSource = ReportLookups.GetOperstats();
            checkBoxListStatus.DataBind();
            foreach (ListItem item in checkBoxListStatus.Items) {
                item.Selected = true;
            }

            checkBoxListStatus.Items.Remove("UTILISATION");
            checkBoxListStatus.Items.Remove("UTILISATION (incl. Overdue)");

            //Load the check box list settings
            this.PopupCheckBoxListStatus.LoadCheckBoxList();
        }


        protected void RebindAvailabilityModelCode() 
        {

            System.Web.UI.WebControls.CheckBoxList checkBoxListModelCode = (System.Web.UI.WebControls.CheckBoxList)this.PopupCheckBoxListAvailabilityModelcode.FindControl("CheckBoxListPopUp");
            checkBoxListModelCode.Items.Clear();


            if (this.DropDownListAvailabilityCountry.SelectedValue == "-1") 
            {
            }
            else 
            {
                checkBoxListModelCode.DataTextField = "Model";
                checkBoxListModelCode.DataValueField = "Model";
                checkBoxListModelCode.DataSource = ReportLookups.GetModelcodes(Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue));
                checkBoxListModelCode.DataBind();
                foreach (ListItem item in checkBoxListModelCode.Items) 
                {
                    item.Selected = false;
                }

            }

            //Load the check box list settings
            this.PopupCheckBoxListAvailabilityModelcode.LoadCheckBoxList();

        }


        protected void RebindAvailabilityNoRev() 
        {
            this.DropDownListAvailabilityNoRev.Items.Clear();
            this.CreateNewListItem(this.DropDownListAvailabilityNoRev, "3 " + Resources.lang.Days, "3");
            this.CreateNewListItem(this.DropDownListAvailabilityNoRev, "5 " + Resources.lang.Days, "5");
            this.CreateNewListItem(this.DropDownListAvailabilityNoRev, "7 " + Resources.lang.Days, "7");
            this.CreateNewListItem(this.DropDownListAvailabilityNoRev, "10 " + Resources.lang.Days + " +", "10");
            this.CreateNewListItem(this.DropDownListAvailabilityNoRev, "60 " + Resources.lang.Days + " +", "60");
            this.AddInitialValueToDropDown(this.DropDownListAvailabilityNoRev, true);


        }

        protected void RebindAvailabilityTopics() 
        {
            this.DropDownListAvailabilityTopic.Items.Clear();
            this.DropDownListAvailabilityTopic.DataSource = ReportLookups.GetOperstats();
            this.DropDownListAvailabilityTopic.DataBind();
            if (!Page.IsPostBack) {
                this.DropDownListAvailabilityTopic.SelectedValue = "RT";
            }

        }

        #endregion

        #region "Statistics"


        protected void DropDownListStatisticsCountry_SelectedIndexChanged(object sender, System.EventArgs e) {
            string selectedCountry = Convert.ToString(this.DropDownListStatisticsCountry.SelectedValue);
            if (selectedCountry == "-1") {
                //Databind all options
                this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.AllNew, true);
            }
            else {
                //Databind options for specific country
                int selectedLogic = Convert.ToInt32(this.RadioButtonListStatisticsLogic.SelectedValue);

                switch (selectedLogic) {

                    case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                        this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.CMSPool, true);

                        break;
                    case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                        this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.OPSRegion, true);

                        break;
                }

            }


        }


        protected void DropDownListStatisticsCMSPool_SelectedIndexChanged(object sender, System.EventArgs e) {
            this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.CMSLocationGroup, true);

        }


        protected void DropDownListStatisticsOPSregion_SelectedIndexChanged(object sender, System.EventArgs e) {
            this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.OPSArea, true);


        }


        protected void DropDownListStatisticsCMSLocationGroup_SelectedIndexChanged(object sender, System.EventArgs e) {
            this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.Location, true);

        }


        protected void DropDownListStatisticsOPSArea_SelectedIndexChanged(object sender, System.EventArgs e) {
            this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.Location, true);

        }


        protected void RebindDropDownListsStatistics(int dropDownListOption, bool selectAll) 
        {
            //Check which logic is selected
            int selectedLogic = Convert.ToInt32(this.RadioButtonListStatisticsLogic.SelectedValue);

            switch (dropDownListOption) {

                case (int)App.BLL.ReportSettings.ReportOptions.CMSPool:
                    //Clear CMS Pool items
                    this.DropDownListStatisticsCMSPool.Items.Clear();
                    //Databind CMS Pools
                    this.DropDownListStatisticsCMSPool.DataSource = ReportLookups.GetCMSPools(Convert.ToString(this.DropDownListStatisticsCountry.SelectedValue));
                    this.DropDownListStatisticsCMSPool.DataBind();
                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsCMSPool, selectAll);
                    //Clear child items below and add all items to each
                    this.DropDownListStatisticsCMSLocationGroup.Items.Clear();
                    this.DropDownListStatisticsLocations.Items.Clear();
                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsCMSLocationGroup, selectAll);
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsLocations, selectAll);

                    break;
                case (int)App.BLL.ReportSettings.ReportOptions.CMSLocationGroup:
                    //Clear Items
                    this.DropDownListStatisticsCMSLocationGroup.Items.Clear();
                    //Databind CMS Location Groups
                    if (!(Convert.ToInt32(this.DropDownListStatisticsCMSPool.SelectedValue) == -1)) {
                        this.DropDownListStatisticsCMSLocationGroup.DataSource = ReportLookups.GetCMSLocationGroups((Convert.ToString(this.DropDownListStatisticsCountry.SelectedValue)), (Convert.ToInt32(this.DropDownListStatisticsCMSPool.SelectedValue)));
                        this.DropDownListStatisticsCMSLocationGroup.DataBind();
                    }

                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsCMSLocationGroup, selectAll);
                    //Clear child items below and add all items to each
                    this.DropDownListStatisticsLocations.Items.Clear();
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsLocations, selectAll);

                    break;
                case (int)App.BLL.ReportSettings.ReportOptions.OPSRegion:
                    //Clear Items
                    this.DropDownListStatisticsOPSRegion.Items.Clear();
                    //Databind OPS Regions
                    this.DropDownListStatisticsOPSRegion.DataSource = ReportLookups.GetOPSRegions(Convert.ToString(this.DropDownListStatisticsCountry.SelectedValue));

                    this.DropDownListStatisticsOPSRegion.DataBind();
                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsOPSRegion, selectAll);
                    //Clear child items below and add all items to each
                    this.DropDownListStatisticsOPSArea.Items.Clear();
                    this.DropDownListStatisticsLocations.Items.Clear();
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsOPSArea, selectAll);
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsLocations, selectAll);

                    break;
                case (int)App.BLL.ReportSettings.ReportOptions.OPSArea:
                    //Clear Items
                    this.DropDownListStatisticsOPSArea.Items.Clear();
                    //Databind OPS Areas 
                    if (!(Convert.ToInt32(this.DropDownListStatisticsOPSRegion.SelectedValue) == -1)) {
                        this.DropDownListStatisticsOPSArea.DataSource = ReportLookups.GetOPSAreas(Convert.ToString(this.DropDownListStatisticsCountry.SelectedValue), Convert.ToInt32(this.DropDownListStatisticsOPSRegion.SelectedValue));

                        this.DropDownListStatisticsOPSArea.DataBind();
                    }

                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsOPSArea, selectAll);
                    //Clear child items below and add all items to each
                    this.DropDownListStatisticsLocations.Items.Clear();
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsLocations, selectAll);

                    break;
                case (int)App.BLL.ReportSettings.ReportOptions.Location:
                    //Clear Items
                    this.DropDownListStatisticsLocations.Items.Clear();
                    //Check Logic
                    switch (selectedLogic) {

                        case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                            //Set location datasource
                            if (!(Convert.ToString(this.DropDownListStatisticsCMSLocationGroup.SelectedValue) == "-1")) {
                                this.DropDownListStatisticsLocations.DataSource = ReportLookups.GetLocations((Convert.ToString(this.DropDownListStatisticsCountry.SelectedValue)), -1, -1, (Convert.ToInt32(this.DropDownListStatisticsCMSPool.SelectedValue)), int.Parse(this.DropDownListStatisticsCMSLocationGroup.SelectedValue), selectedLogic);
                                //Databind location
                                this.DropDownListStatisticsLocations.DataBind();
                            }

                            break;

                        case (int)App.BLL.ReportSettings.OptionLogic.OPS:

                            //Set location datasource
                            if (!(Convert.ToInt32(this.DropDownListStatisticsOPSArea.SelectedValue) == -1)) {
                                this.DropDownListStatisticsLocations.DataSource = ReportLookups.GetLocations((Convert.ToString(this.DropDownListStatisticsCountry.SelectedValue)), (Convert.ToInt32(this.DropDownListStatisticsOPSRegion.SelectedValue)), (Convert.ToInt32(this.DropDownListStatisticsOPSArea.SelectedValue)), -1, -1, selectedLogic);
                                //Databind location
                                this.DropDownListStatisticsLocations.DataBind();
                            }

                            break;

                    }


                    //Add initial value to drop down list
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsLocations, selectAll);

                    break;

                case (int)App.BLL.ReportSettings.ReportOptions.AllNew:
                    //Clear all items
                    //Databind countries and add all items to each other list

                    this.DropDownListStatisticsCMSPool.Items.Clear();
                    this.DropDownListStatisticsCMSLocationGroup.Items.Clear();
                    this.DropDownListStatisticsOPSRegion.Items.Clear();
                    this.DropDownListStatisticsOPSArea.Items.Clear();
                    this.DropDownListStatisticsLocations.Items.Clear();

                    //Data bind countries list
                    this.DropDownListStatisticsCountry.Items.Clear();
                    this.DropDownListStatisticsCountry.DataSource = ReportLookups.GetCountries();
                    this.DropDownListStatisticsCountry.DataBind();
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsCountry, selectAll);

                    //Check which option logic we are using
                    switch (selectedLogic) {

                        case (int)App.BLL.ReportSettings.OptionLogic.CMS:

                            //Add initial items to cmspool and cms_location_group
                            this.AddInitialValueToDropDown(this.DropDownListStatisticsCMSPool, selectAll);
                            this.AddInitialValueToDropDown(this.DropDownListStatisticsCMSLocationGroup, selectAll);

                            break;
                        case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                            //Add initial items to opsregions and opsareas
                            this.AddInitialValueToDropDown(this.DropDownListStatisticsOPSRegion, selectAll);
                            this.AddInitialValueToDropDown(this.DropDownListStatisticsOPSArea, selectAll);
                            break;
                    }

                    //Add initial item to locations
                    this.AddInitialValueToDropDown(this.DropDownListStatisticsLocations, selectAll);
                    break;
            }
            this.UpdatePanelReportSettings.Update();

        }

        #endregion

        #region "General"

        /// <summary>
        /// Add "ALL" item to drop down lists
        /// </summary>
        /// <param name="dropDownListOption"></param>
        /// <remarks></remarks>

        private void AddInitialValueToDropDown(System.Web.UI.WebControls.DropDownList dropDownListOption, bool selected) 
        {
            ListItem item = new ListItem();
            item.Text = Resources.lang.ReportSettingsALL;
            item.Selected = selected;
            item.Value = "-1";
            dropDownListOption.Items.Insert(0, item);
        }


        private void AddInitialValueToDropDownPleaseSelect(System.Web.UI.WebControls.DropDownList dropDownListOption, bool selected) 
        {
            ListItem item = new ListItem();
            item.Text = "- Please Select -";
            item.Selected = selected;
            item.Value = "-1";
            dropDownListOption.Items.Insert(0, item);
        }

        /// <summary>
        /// Add "ALL" item to drop down lists
        /// </summary>
        /// <param name="dropDownListOption"></param>
        /// <remarks></remarks>

        private void AddInitialValueToCheckBoxListBox(System.Web.UI.WebControls.CheckBoxList checkBoxListOption, bool selected) 
        {
            ListItem item = new ListItem();
            item.Text = Resources.lang.ReportSettingsALL;
            item.Selected = selected;
            item.Value = "-1";
            checkBoxListOption.Items.Insert(0, item);


        }


        protected void CreateNewListItem(System.Web.UI.WebControls.DropDownList dropDownListToAddItem, string text, string value) 
        {
            ListItem item = new ListItem();
            item.Value = value;
            item.Text = text;
            dropDownListToAddItem.Items.Add(item);

        }

        #endregion

        #endregion

        #region "RadioButtonList Events"


        protected void RadioButtonListAvailabilityLogic_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
            //Get the selected logic
            int selectedLogic = Convert.ToInt32(this.RadioButtonListAvailabilityLogic.SelectedValue);
            //Set visibility of items depending on logic selected
            SetLogicVisibility((int)App.BLL.ReportSettings.ReportSettingsTool.Availability, selectedLogic);
            
            if (this.DropDownListAvailabilityCountry.SelectedValue == "-1") 
            {
                //Databind all options
                this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.AllNew, true);
            }
            else 
            {
                //Logic Changed
                switch (selectedLogic) 
                {
                    case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                        //Rebind CMS Pools
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSPool, true);
                        break;
                    case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                        //Rebind OPS Regions
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSRegion, true);
                        break;
                }
            }

        }





        protected void RadioButtonListStatisticsLogic_SelectedIndexChanged(object sender, System.EventArgs e) {
            //Get the selected logic
            int selectedLogic = Convert.ToInt32(this.RadioButtonListStatisticsLogic.SelectedValue);
            //Set visibility of items depending on logic selected
            SetLogicVisibility((int)App.BLL.ReportSettings.ReportSettingsTool.Statistics, selectedLogic);
            if (this.DropDownListStatisticsCountry.SelectedValue == "-1") {
                //Databind all options
                this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.AllNew, true);
            }
            else {
                //Logic Changed
                switch (selectedLogic) {
                    case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                        //Rebind CMS Pools
                        this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.CMSPool, true);
                        break;
                    case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                        //Rebind OPS Regions
                        this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.OPSRegion, true);
                        break;
                }
            }


        }



        protected void SetLogicVisibility(int tool, int logic) 
        {
            //Set visibility of items depending on tool selected and logic
            switch (tool) 
            {

                case (int)App.BLL.ReportSettings.ReportSettingsTool.Availability:
                case (int)App.BLL.ReportSettings.ReportSettingsTool.NonRevenue:

                    if (logic == (int)App.BLL.ReportSettings.OptionLogic.CMS) 
                    {
                        //Hide OPS options
                        this.DropDownListAvailabilityOPSArea.Visible = false;
                        this.DropDownListAvailabilityOPSRegion.Visible = false;
                        this.LabelAvailabilityOPSArea.Visible = false;
                        this.LabelAvailabilityOPSRegion.Visible = false;
                        //Show CMS Options
                        this.LabelAvailabilityCMSPool.Visible = true;
                        this.LabelAvailabilityCMSLocationGroup.Visible = true;
                        this.DropDownListAvailabilityCMSPool.Visible = true;
                        this.DropDownListAvailabilityCMSLocationGroup.Visible = true;


                    }
                    else if (logic == (int)App.BLL.ReportSettings.OptionLogic.OPS) 
                    {
                        //Hide CMS options
                        this.LabelAvailabilityCMSPool.Visible = false;
                        this.LabelAvailabilityCMSLocationGroup.Visible = false;
                        this.DropDownListAvailabilityCMSPool.Visible = false;
                        this.DropDownListAvailabilityCMSLocationGroup.Visible = false;
                        //Show OPS Options
                        this.DropDownListAvailabilityOPSArea.Visible = true;
                        this.DropDownListAvailabilityOPSRegion.Visible = true;
                        this.LabelAvailabilityOPSArea.Visible = true;
                        this.LabelAvailabilityOPSRegion.Visible = true;

                    }

                    break;

                case (int)App.BLL.ReportSettings.ReportSettingsTool.Statistics:


                    if (logic == (int)App.BLL.ReportSettings.OptionLogic.CMS) 
                    {
                        //Hide OPS options
                        this.DropDownListStatisticsOPSArea.Visible = false;
                        this.DropDownListStatisticsOPSRegion.Visible = false;
                        this.LabelStatisticsOPSArea.Visible = false;
                        this.LabelStatisticsOPSRegion.Visible = false;
                        //Show CMS Options
                        this.LabelStatisticsCMSPool.Visible = true;
                        this.LabelStatisticsCMSLocationGroup.Visible = true;
                        this.DropDownListStatisticsCMSPool.Visible = true;
                        this.DropDownListStatisticsCMSLocationGroup.Visible = true;


                    }
                    else if (logic == (int)App.BLL.ReportSettings.OptionLogic.OPS) 
                    {
                        //Hide CMS options
                        this.LabelStatisticsCMSPool.Visible = false;
                        this.LabelStatisticsCMSLocationGroup.Visible = false;
                        this.DropDownListStatisticsCMSPool.Visible = false;
                        this.DropDownListStatisticsCMSLocationGroup.Visible = false;
                        //Show OPS Options
                        this.DropDownListStatisticsOPSArea.Visible = true;
                        this.DropDownListStatisticsOPSRegion.Visible = true;
                        this.LabelStatisticsOPSArea.Visible = true;
                        this.LabelStatisticsOPSRegion.Visible = true;

                    }

                    break;
            }

        }

        #endregion

        #region "Usercontrol Click Events"

        #region "Custom Event Handlers"

        public event EventHandler GenerateReport;
        public event EventHandler DownloadReport;

        #endregion

        #region "Availability"

        protected void ButtonAvailabilityGenerateReport_Click(object sender, System.EventArgs e) 
        {
            AvailabilityGenerateReport();
        }

        public void ClickAvailabilityReportButton() 
        {
            ButtonAvailabilityGenerateReport_Click(this, null);
        }

        public void AvailabilityGenerateReport() 
        {
            //Set property values so other pages can access the selection of the report

            //Get the selected logic
            int selectedLogic = Convert.ToInt32(this.RadioButtonListAvailabilityLogic.SelectedValue);

            _country = Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue);
            _logic = selectedLogic;

            switch (selectedLogic) 
            {
                case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                    //Set Pool Information
                    _cms_pool_id = Convert.ToInt32(this.DropDownListAvailabilityCMSPool.SelectedValue);
                    _cms_pool = Convert.ToString(this.DropDownListAvailabilityCMSPool.SelectedItem.Text);

                    //Set Location Group Information
                    _cms_location_group_id = int.Parse(this.DropDownListAvailabilityCMSLocationGroup.SelectedValue);
                    _cms_location_group = Convert.ToString(this.DropDownListAvailabilityCMSLocationGroup.SelectedItem.Text);

                    break;
                //Set default values for OPS logic


                case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                    //Set Region Information
                    _ops_region_id = Convert.ToInt32(this.DropDownListAvailabilityOPSRegion.SelectedValue);
                    _ops_region = Convert.ToString(this.DropDownListAvailabilityOPSRegion.SelectedItem.Text);

                    //Set Area Information
                    _ops_area_id = Convert.ToInt32(this.DropDownListAvailabilityOPSArea.SelectedValue);
                    _ops_area = Convert.ToString(this.DropDownListAvailabilityOPSArea.SelectedItem.Text);

                    break;

            }

            _location = Convert.ToString(this.DropDownListAvailabilityLocations.SelectedValue);
            _car_segment_id = Convert.ToInt32(this.DropDownListAvailabilityCarSegment.SelectedValue);
            _car_segment = Convert.ToString(this.DropDownListAvailabilityCarSegment.SelectedItem.Text);
            _car_class_id = Convert.ToInt32(this.DropDownListAvailabilityCarClass.SelectedValue);
            _car_class = Convert.ToString(this.DropDownListAvailabilityCarClass.SelectedItem.Text);

            //Car Group
            if ((!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison)))
            {
                _car_group_id = Convert.ToInt32(this.DropDownListAvailabilityCarGroup.SelectedValue);
                _car_group = Convert.ToString(this.DropDownListAvailabilityCarGroup.SelectedItem.Text);
            }


            if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATCarSearch) || (_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.NRCarSearch)) 
            {
                //Declare variables for loops
                int countSelected = 0;

                //Get Values for Own Area
                //Get a reference to the listbox
                System.Web.UI.WebControls.CheckBoxList checkBoxListBoxOwnArea = (System.Web.UI.WebControls.CheckBoxList)this.PopupCheckBoxListAvailabilityOwnArea.FindControl("CheckBoxListPopUp");

                //Find out how many items are checked
                foreach (ListItem item in checkBoxListBoxOwnArea.Items) 
                {
                    if (item.Selected == true) {
                        countSelected += 1;
                    }
                }

                if (countSelected == 0) 
                {
                    _ownArea = null;
                }
                else 
                {
                    foreach (ListItem item in checkBoxListBoxOwnArea.Items) 
                    {
                        if (item.Selected == true) 
                        {
                            _ownArea = _ownArea + item.Value.ToString() + ",";
                        }
                    }
                }
                if (!(_ownArea == null)) 
                {
                    //Trim any comma's at the end
                    _ownArea = _ownArea.Trim().Remove(_ownArea.Length - 1);
                }

                //Get values for Model Code
                System.Web.UI.WebControls.CheckBoxList checkBoxListBoxModelCode = (System.Web.UI.WebControls.CheckBoxList)this.PopupCheckBoxListAvailabilityModelcode.FindControl("CheckBoxListPopUp");
                //reset variables
                countSelected = 0;

                //Find out how many items are checked
                foreach (ListItem item in checkBoxListBoxModelCode.Items) 
                {
                    if (item.Selected == true) 
                    {
                        countSelected += 1;
                    }
                }
                if (countSelected == 0) 
                {
                    _modelCode = null;
                }
                else 
                {
                    foreach (ListItem item in checkBoxListBoxModelCode.Items) 
                    {
                        if (item.Selected == true) 
                        {
                            _modelCode = _modelCode + item.Value.ToString() + ",";
                        }
                    }
                }

                //Trim any comma's at the end
                if (!(_modelCode == null)) 
                {
                    _modelCode = _modelCode.Remove(_modelCode.Length - 1);
                }

                //Get values for Status
                System.Web.UI.WebControls.CheckBoxList checkBoxListBoxStatus = (System.Web.UI.WebControls.CheckBoxList)this.PopupCheckBoxListStatus.FindControl("CheckBoxListPopUp");
                //reset variables
                countSelected = 0;

                //Find out how many items are checked
                foreach (ListItem item in checkBoxListBoxStatus.Items) 
                {
                    if (item.Selected == true) 
                    {
                        countSelected += 1;
                    }
                }
                if (countSelected == 0) 
                {
                    _status = null;
                }
                else 
                {
                    foreach (ListItem item in checkBoxListBoxStatus.Items) 
                    {
                        if (item.Selected == true) 
                        {
                            _status = _status + item.Value.ToString() + ",";
                        }
                    }
                }

                //Trim any comma's at the end
                if (!(_status == null)) 
                {
                    _status = _status.Trim().Remove(_status.Length - 1);
                }

                //Set No Rev
                _noRev = Convert.ToInt32(this.DropDownListAvailabilityNoRev.SelectedValue);
                //Set select By
                if (this.RadioButtonAvailabilityDUEWWD.Checked == true) 
                {
                    _carSearch_SelectBy = "DUEWWD";
                }
                else if (this.RadioButtonAvailabilityLSTWWD.Checked == true) 
                {
                    _carSearch_SelectBy = "LSTWWD";
                }

            }

            //Set Fleet Name
            _fleet_name = Convert.ToString(this.DropDownListAvailabilityFleet.SelectedValue);

            //Set Date and DateRange
            switch (_marsPage) 
            {
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATHistoricalTrend):
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison):
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison):
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetStatus):
                    setDates(DropDownListAvailabilityDateRange);
                    _dayOfWeek = Convert.ToString(this.DropDownListAvailabilityDayOfWeek.SelectedItem.Text);
                    _dayOfWeekValue = Convert.ToInt32(this.DropDownListAvailabilityDayOfWeek.SelectedValue);
                    break;

                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATKPI):
                    setDates(DropDownListAvailabilityDateRangeKPI); break;
            }

            //Set Logic and Topic
            switch (_marsPage) 
            {
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison):
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison):
                    _availabilityTopic = Convert.ToString(this.DropDownListAvailabilityTopic.SelectedItem.Text);
                    _availabilityTopicValue = Convert.ToString(this.DropDownListAvailabilityTopic.SelectedValue);
                    _availabilityLogic = Convert.ToString(this.RadioButtonListAvailabilityValue.SelectedValue);
                    break;

                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATKPI):
                    _availabilityKPIText = Convert.ToString(this.DropDownListAvailabilityKPI.SelectedItem.Text);
                    _availabilityKPIValue = Convert.ToString(this.DropDownListAvailabilityKPI.SelectedValue);
                    _availabilityLogic = Convert.ToString(this.RadioButtonListAvailabilityValue.SelectedValue);
                    break;
            }

            //Set Logic
            if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATHistoricalTrend))
            {
                _availabilityLogic = Convert.ToString(this.RadioButtonListAvailabilityValue.SelectedValue);
            }

            //Set OperStat Topic
            switch (_marsPage) 
            {
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison):
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison):
                    _availabilityTopicOperstat = Convert.ToString(this.DropDownListAvailabilityTopic.SelectedValue);
                    break;
            }

            //Raise custom event from parent page
            if (GenerateReport != null) 
            {
                GenerateReport(this, EventArgs.Empty);
            }
        }

        protected void ButtonAvailabilityDownloadReport_Click(object sender, System.EventArgs e) 
        {
            //Get the selected logic
            int selectedLogic = Convert.ToInt32(this.RadioButtonListAvailabilityLogic.SelectedValue);

            _country = Convert.ToString(this.DropDownListAvailabilityCountry.SelectedValue);
            _logic = selectedLogic;

            switch (selectedLogic) 
            {
                case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                    //Set Pool Information
                    _cms_pool_id = Convert.ToInt32(this.DropDownListAvailabilityCMSPool.SelectedValue);
                    _cms_pool = Convert.ToString(this.DropDownListAvailabilityCMSPool.SelectedItem.Text);

                    //Set Location Group Information
                    _cms_location_group_id = int.Parse(this.DropDownListAvailabilityCMSLocationGroup.SelectedValue);
                    _cms_location_group = Convert.ToString(this.DropDownListAvailabilityCMSLocationGroup.SelectedItem.Text);

                    break;
                //Set default values for OPS logic
                case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                    //Set Region Information
                    _ops_region_id = Convert.ToInt32(this.DropDownListAvailabilityOPSRegion.SelectedValue);
                    _ops_region = Convert.ToString(this.DropDownListAvailabilityOPSRegion.SelectedItem.Text);

                    //Set Area Information
                    _ops_area_id = Convert.ToInt32(this.DropDownListAvailabilityOPSArea.SelectedValue);
                    _ops_area = Convert.ToString(this.DropDownListAvailabilityOPSArea.SelectedItem.Text);

                    break;

            }

            _location = Convert.ToString(this.DropDownListAvailabilityLocations.SelectedValue);
            _car_segment_id = Convert.ToInt32(this.DropDownListAvailabilityCarSegment.SelectedValue);
            _car_segment = Convert.ToString(this.DropDownListAvailabilityCarSegment.SelectedItem.Text);
            _car_class_id = Convert.ToInt32(this.DropDownListAvailabilityCarClass.SelectedValue);
            _car_class = Convert.ToString(this.DropDownListAvailabilityCarClass.SelectedItem.Text);
            _car_group_id = Convert.ToInt32(this.DropDownListAvailabilityCarGroup.SelectedValue);
            _car_group = Convert.ToString(this.DropDownListAvailabilityCarGroup.SelectedItem.Text);

            TextBox dateTextBox = (TextBox)this.DatePickerTextBoxAvailabilityDate.FindControl("TextBoxDatePicker");
            _dateAvailability = Convert.ToDateTime(dateTextBox.Text);
            _dateRange = Convert.ToString(this.DropDownListAvailabilityDateRangeKPI.SelectedItem.Text);
            _dateRangeValue = this.DropDownListAvailabilityDateRangeKPI.SelectedValue;

            //Raise custom event from parent page
            if (DownloadReport != null) 
            {
                DownloadReport(this, EventArgs.Empty);
            }
        }

        protected void ButtonAvailabilityKPIDownload_Click(object sender, System.EventArgs e) 
        {
            Response.Redirect("~/App.Site/Availability/KPI/Download", false);
        }

        protected void ButtonAvailabilityReturnToFleetStatusReport_Click(object sender, System.EventArgs e) 
        {
            string redirectUrl = SessionHandler.AvailabilityCarSearchReturnQueryString;
            Response.Redirect(redirectUrl, false);
        }
        
        #endregion

        #region "Statistics"

        protected void ButtonStatisticsGenerateReport_Click(object sender, System.EventArgs e) 
        {
            //Set property values so other pages can access the selection of the report

            //Get the selected logic
            int selectedLogic = Convert.ToInt32(this.RadioButtonListStatisticsLogic.SelectedValue);

            _country = Convert.ToString(this.DropDownListStatisticsCountry.SelectedValue);
            _logic = selectedLogic;

            switch (selectedLogic) 
            {
                case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                    //Set Pool Information
                    _cms_pool_id = Convert.ToInt32(this.DropDownListStatisticsCMSPool.SelectedValue);
                    _cms_pool = Convert.ToString(this.DropDownListStatisticsCMSPool.SelectedItem.Text);

                    //Set Location Group Information
                    _cms_location_group_id = int.Parse(this.DropDownListStatisticsCMSLocationGroup.SelectedValue);
                    _cms_location_group = Convert.ToString(this.DropDownListStatisticsCMSLocationGroup.SelectedItem.Text);

                    break;
                //Set default values for OPS logic


                case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                    //Set Region Information
                    _ops_region_id = Convert.ToInt32(this.DropDownListStatisticsOPSRegion.SelectedValue);
                    _ops_region = Convert.ToString(this.DropDownListStatisticsOPSRegion.SelectedItem.Text);

                    //Set Area Information
                    _ops_area_id = Convert.ToInt32(this.DropDownListStatisticsOPSArea.SelectedValue);
                    _ops_area = Convert.ToString(this.DropDownListStatisticsOPSArea.SelectedItem.Text);

                    break;

            }

            _location = Convert.ToString(this.DropDownListStatisticsLocations.SelectedValue);
            _racfID = this.TextBoxStatisticsRACFID.Text.Trim();

            TextBox startDateTextBox = (TextBox)this.DatePickerTextBoxStatisticsStartDate.FindControl("TextBoxDatePicker");
            _statisticsStartDate = Convert.ToDateTime(startDateTextBox.Text);
            TextBox endDateTextBox = (TextBox)this.DatePickerTextBoxStatisticsEndDate.FindControl("TextBoxDatePicker");
            _statisticsEndDate = Convert.ToDateTime(endDateTextBox.Text);

            //Raise custom event from parent page
            if (GenerateReport != null) 
            {
                GenerateReport(this, EventArgs.Empty);
            }

        }

        #endregion

        #endregion

        #region "Report Settings UserPreferences"

        protected void LoadSavedSearches(int logic, string country, int cms_pool_id, int cms_location_group_id,
            int ops_region_id, int ops_area_id, int car_segment_id, int car_class_id, int car_group_id, string location) 
        {
            switch (_marsTool) 
            {
                case (int)App.BLL.ReportSettings.ReportSettingsTool.Availability:
                case (int)App.BLL.ReportSettings.ReportSettingsTool.NonRevenue:
                    
                    this.SetLogicVisibility((int)App.BLL.ReportSettings.ReportSettingsTool.Availability, logic); //Set logic visibility                
                    
                    this.RadioButtonListAvailabilityLogic.SelectedValue = logic.ToString(); //Set Option Logic
                    
                    this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.AllNew, false);
                    
                    if (country != "-1") 
                    {
                        this.DropDownListAvailabilityCountry.SelectedValue = country; //Set values
                        switch (logic) {
                            case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                                this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSPool, false);
                                if ((cms_pool_id != -1 && cms_pool_id != 0)) {
                                    this.DropDownListAvailabilityCMSPool.SelectedValue = cms_pool_id.ToString();
                                    this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSLocationGroup, false);
                                }
                                if ((cms_location_group_id != -1)) {
                                    this.DropDownListAvailabilityCMSLocationGroup.SelectedValue = cms_location_group_id.ToString();
                                    this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.Location, false);
                                    this.DropDownListAvailabilityLocations.SelectedValue = location;
                                }
                                break;

                            case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                                this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSRegion, false);
                                if ((ops_region_id != -1 && ops_region_id != 0)) {
                                    this.DropDownListAvailabilityOPSRegion.SelectedValue = ops_region_id.ToString();
                                    this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSArea, false);
                                }
                                if ((ops_area_id != -1 && ops_area_id != 0)) {
                                    this.DropDownListAvailabilityOPSArea.SelectedValue = ops_area_id.ToString();
                                    this.DropDownListAvailabilityLocations.SelectedValue = location;
                                }
                                break;
                        }
                        if (!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison)) 
                        {
                            if ((location != "-1")) {
                                this.DropDownListAvailabilityLocations.SelectedValue = location;
                            }
                        }
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarSegment, false);
                        if ((car_segment_id != -1 && car_segment_id != 0)) 
                        {
                            this.DropDownListAvailabilityCarSegment.SelectedValue = car_segment_id.ToString();
                            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarClass, false);
                        }
                        if ((car_class_id != -1 && car_class_id != 0)) 
                        {
                            this.DropDownListAvailabilityCarClass.SelectedValue = car_class_id.ToString();
                            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarGroup, false);
                        }
                        if (!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison)) 
                        {
                            if ((car_group_id != -1 && car_group_id != 0)) 
                            {
                                this.DropDownListAvailabilityCarGroup.SelectedValue = car_group_id.ToString();
                            }
                        }
                        if (_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATCarSearch) 
                        {
                            this.RebindAvailabilityModelCode();
                        }
                    }
                    break;
            }

        }
        
        public void SetUserDefaultSettingsAvailabilityTool(int logic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id,
                                        int ops_area_id, int car_segment_id, int car_class_id, int car_group_id, int userPreference,
                                         string location, string fleetName, string operstat, int? fleetStatus, DateTime? startDate, int? dayOfWeek, int? dateRange) 
        {
            this.SetLogicVisibility((int)App.BLL.ReportSettings.ReportSettingsTool.Availability, logic); //Set logic visibility
            
            this.RadioButtonListAvailabilityLogic.SelectedValue = logic.ToString(); //Set Option Logic
            
            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.AllNew, false);
            
            if (country != "-1") 
            { 
                //User last setting               
                this.DropDownListAvailabilityCountry.SelectedValue = country; //Set values
                switch (logic) {
                    case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSPool, false);
                        if ((cms_pool_id != -1)) {
                            this.DropDownListAvailabilityCMSPool.SelectedValue = cms_pool_id.ToString();
                            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSLocationGroup, false);
                        }
                        if (cms_location_group_id != -1) {
                            this.DropDownListAvailabilityCMSLocationGroup.SelectedValue = cms_location_group_id.ToString();
                            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.Location, false);
                            this.DropDownListAvailabilityLocations.SelectedValue = location;
                        }
                        break;
                    case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSRegion, false);
                        if (ops_region_id != -1) {
                            this.DropDownListAvailabilityOPSRegion.SelectedValue = ops_region_id.ToString();
                            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSArea, false);
                        }
                        if (ops_area_id != -1) {
                            this.DropDownListAvailabilityOPSArea.SelectedValue = ops_area_id.ToString();
                            this.DropDownListAvailabilityLocations.SelectedValue = location;
                        }
                        break;
                }

                if (location != "-1") 
                {
                    this.DropDownListAvailabilityLocations.SelectedValue = location;
                }
                
                this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarSegment, false);
                
                if (car_segment_id != -1) 
                {
                    this.DropDownListAvailabilityCarSegment.SelectedValue = car_segment_id.ToString();
                    this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarClass, false);
                }
                
                if (car_class_id != -1) 
                {
                    this.DropDownListAvailabilityCarClass.SelectedValue = car_class_id.ToString();
                    this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarGroup, false);
                }
                
                if (car_group_id != -1) 
                {
                    this.DropDownListAvailabilityCarGroup.SelectedValue = car_group_id.ToString();
                }
                
                if (_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATCarSearch) 
                {
                    this.RebindAvailabilityModelCode();
                }
                
                if (fleetStatus == 1) 
                {
                    if (!(fleetName == null)) 
                    {
                        this.DropDownListAvailabilityFleet.SelectedValue = fleetName;
                    }
                    
                    if (!(operstat == null)) 
                    {
                        //Get values for Status
                        System.Web.UI.WebControls.CheckBoxList checkBoxListBoxStatus = (System.Web.UI.WebControls.CheckBoxList)this.PopupCheckBoxListStatus.FindControl("CheckBoxListPopUp");

                        foreach (ListItem item in checkBoxListBoxStatus.Items) 
                        {
                            if (!(item.Value == operstat)) 
                            {
                                item.Selected = false;
                            }
                            else 
                            {
                                item.Selected = true;
                            }
                        }
                    }

                    switch (logic) 
                    {
                        case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                            SessionHandler.AvailabilityCarSearchReturnQueryString = "~/App.Site/Availability/FleetStatus/?"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OptionLogic) + "=" + logic.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Country) + "=" + country + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Pool_Id) + "=" + cms_pool_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Location_Group_Code) + "=" + cms_location_group_id + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Location) + "=" + (location ?? "-1") + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Segment_Id) + "=" + car_segment_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Class_Id) + "=" + car_class_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateValue) + "=" + string.Format("{0:ddMMyyyyhhmmss}", startDate) + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DayOfWeek) + "=" + dayOfWeek.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateRange) + "=" + dateRange.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetStatus) + "=0";

                            break;
                        case (int)App.BLL.ReportSettings.OptionLogic.OPS:

                            SessionHandler.AvailabilityCarSearchReturnQueryString = "~/App.Site/Availability/FleetStatus/?"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OptionLogic) + "=" + logic.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Country) + "=" + country + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Region_Id) + "=" + ops_region_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Area_Id) + "=" + ops_area_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Location) + "=" + (location ?? "-1") + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Segment_Id) + "=" + car_segment_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Class_Id) + "=" + car_class_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateValue) + "=" + string.Format("{0:ddMMyyyyhhmmss}", startDate) + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DayOfWeek) + "=" + dayOfWeek.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateRange) + "=" + dateRange.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetStatus) + "=0";

                            break;
                    }



                    this.ButtonAvailabilityReturnToFleetStatusReport.Visible = true;

                }
                else if (fleetStatus == 0) 
                {
                    //return from car search to fleet status

                    TextBox dateTextBox = (TextBox)this.DatePickerTextBoxAvailabilityDate.FindControl("TextBoxDatePicker");
                    dateTextBox.Text = string.Format("{0:dd/MM/yyyy}", startDate);

                    this.DropDownListAvailabilityDayOfWeek.SelectedValue = dayOfWeek.ToString();

                    this.DropDownListAvailabilityDateRange.SelectedValue = dateRange.ToString();

                }
            }
        }


        public void SetUserDefaultSettingsNonRevTool(int logic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id,
                                        int ops_area_id, int car_segment_id, int car_class_id, int car_group_id, int userPreference,
                                         string location, string fleetName, string operstat, int? fleetStatus, DateTime? startDate, int? dayOfWeek, int? dateRange)
        {
            this.SetLogicVisibility((int)App.BLL.ReportSettings.ReportSettingsTool.NonRevenue, logic); //Set logic visibility

            this.RadioButtonListAvailabilityLogic.SelectedValue = logic.ToString(); //Set Option Logic

            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.AllNew, false);

            if (country != "-1")
            {
                //User last setting               
                this.DropDownListAvailabilityCountry.SelectedValue = country; //Set values
                switch (logic)
                {
                    case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSPool, false);
                        if ((cms_pool_id != -1))
                        {
                            this.DropDownListAvailabilityCMSPool.SelectedValue = cms_pool_id.ToString();
                            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSLocationGroup, false);
                        }
                        if (cms_location_group_id != -1)
                        {
                            this.DropDownListAvailabilityCMSLocationGroup.SelectedValue = cms_location_group_id.ToString();
                            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.Location, false);
                            this.DropDownListAvailabilityLocations.SelectedValue = location;
                        }
                        break;
                    case (int)App.BLL.ReportSettings.OptionLogic.OPS:
                        this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSRegion, false);
                        if (ops_region_id != -1)
                        {
                            this.DropDownListAvailabilityOPSRegion.SelectedValue = ops_region_id.ToString();
                            this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSArea, false);
                        }
                        if (ops_area_id != -1)
                        {
                            this.DropDownListAvailabilityOPSArea.SelectedValue = ops_area_id.ToString();
                            this.DropDownListAvailabilityLocations.SelectedValue = location;
                        }
                        break;
                }

                if (location != "-1")
                {
                    this.DropDownListAvailabilityLocations.SelectedValue = location;
                }

                this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarSegment, false);

                if (car_segment_id != -1)
                {
                    this.DropDownListAvailabilityCarSegment.SelectedValue = car_segment_id.ToString();
                    this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarClass, false);
                }

                if (car_class_id != -1)
                {
                    this.DropDownListAvailabilityCarClass.SelectedValue = car_class_id.ToString();
                    this.RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CarGroup, false);
                }

                if (car_group_id != -1)
                {
                    this.DropDownListAvailabilityCarGroup.SelectedValue = car_group_id.ToString();
                }

                if (_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATCarSearch)
                {
                    this.RebindAvailabilityModelCode();
                }

                if (fleetStatus == 1)
                {
                    if (!(fleetName == null))
                    {
                        this.DropDownListAvailabilityFleet.SelectedValue = fleetName;
                    }

                    if (!(operstat == null))
                    {
                        //Get values for Status
                        System.Web.UI.WebControls.CheckBoxList checkBoxListBoxStatus = (System.Web.UI.WebControls.CheckBoxList)this.PopupCheckBoxListStatus.FindControl("CheckBoxListPopUp");

                        foreach (ListItem item in checkBoxListBoxStatus.Items)
                        {
                            if (!(item.Value == operstat))
                            {
                                item.Selected = false;
                            }
                            else
                            {
                                item.Selected = true;
                            }
                        }
                    }

                    switch (logic)
                    {
                        case (int)App.BLL.ReportSettings.OptionLogic.CMS:
                            SessionHandler.AvailabilityCarSearchReturnQueryString = "~/App.Site/NonRevenue/CarSearch/?"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OptionLogic) + "=" + logic.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Country) + "=" + country + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Pool_Id) + "=" + cms_pool_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Location_Group_Code) + "=" + cms_location_group_id + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Location) + "=" + (location ?? "-1") + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Segment_Id) + "=" + car_segment_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Class_Id) + "=" + car_class_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateValue) + "=" + string.Format("{0:ddMMyyyyhhmmss}", startDate) + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DayOfWeek) + "=" + dayOfWeek.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateRange) + "=" + dateRange.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetStatus) + "=0";

                            break;
                        case (int)App.BLL.ReportSettings.OptionLogic.OPS:

                            SessionHandler.AvailabilityCarSearchReturnQueryString = "~/App.Site/NonRevenue/CarSearch/?"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OptionLogic) + "=" + logic.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Country) + "=" + country + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Region_Id) + "=" + ops_region_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Area_Id) + "=" + ops_area_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Location) + "=" + (location ?? "-1") + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Segment_Id) + "=" + car_segment_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Class_Id) + "=" + car_class_id.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateValue) + "=" + string.Format("{0:ddMMyyyyhhmmss}", startDate) + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DayOfWeek) + "=" + dayOfWeek.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateRange) + "=" + dateRange.ToString() + "&"
                                + QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetStatus) + "=0";

                            break;
                    }



                    this.ButtonAvailabilityReturnToFleetStatusReport.Visible = true;

                }
                else if (fleetStatus == 0)
                {
                    //return from car search to fleet status

                    TextBox dateTextBox = (TextBox)this.DatePickerTextBoxAvailabilityDate.FindControl("TextBoxDatePicker");
                    dateTextBox.Text = string.Format("{0:dd/MM/yyyy}", startDate);

                    this.DropDownListAvailabilityDayOfWeek.SelectedValue = dayOfWeek.ToString();

                    this.DropDownListAvailabilityDateRange.SelectedValue = dateRange.ToString();

                }
            }
        }

        public void SetUserDefaultSettingsStatistics(int logic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id,
                                                        int userPreference, string location) 
        {

            //Set logic visibility
            this.SetLogicVisibility((int)App.BLL.ReportSettings.ReportSettingsTool.Statistics, logic);

            //Set Option Logic
            this.RadioButtonListStatisticsLogic.SelectedValue = logic.ToString();
            this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.AllNew, false);

            //User last setting
            if (country != "-1") { //Set values
                this.DropDownListStatisticsCountry.SelectedValue = country;
                switch (logic) {
                    case (int)App.BLL.ReportSettings.OptionLogic.CMS:

                        this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.CMSPool, false);

                        if (cms_pool_id != -1) {
                            this.DropDownListStatisticsCMSPool.SelectedValue = cms_pool_id.ToString();
                            this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.CMSLocationGroup, false);
                        }

                        if (cms_location_group_id != -1) {
                            this.DropDownListStatisticsCMSLocationGroup.SelectedValue = cms_location_group_id.ToString();
                            this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.Location, false);
                            this.DropDownListStatisticsLocations.SelectedValue = location;
                        } break;
                    case (int)App.BLL.ReportSettings.OptionLogic.OPS:

                        this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.OPSRegion, false);
                        if (ops_region_id != -1) {
                            this.DropDownListStatisticsOPSRegion.SelectedValue = ops_region_id.ToString();
                            this.RebindDropDownListsStatistics((int)App.BLL.ReportSettings.ReportOptions.OPSArea, false);
                        }
                        if (ops_area_id != -1) {
                            this.DropDownListStatisticsOPSArea.SelectedValue = ops_area_id.ToString();
                            this.DropDownListStatisticsLocations.SelectedValue = location;
                        } break;
                }
                if (location != "-1") {
                    this.DropDownListStatisticsLocations.SelectedValue = location;
                }
            }
        }
        #endregion

        #region Custom date event handles
        protected void DropDownListAvailabilityDateRange_SelectedIndexChanged(object sender, EventArgs e) { // Added to customise the date range - KPI is a different control
            if (((System.Web.UI.WebControls.DropDownList)sender).Text.Contains("Custom"))
                updateStartEnd(true);
            else
                updateStartEnd(false);
        }
        /*protected void DropDownListMonth_SelectedIndexChanged(object sender, EventArgs e) { // The event from the Custom month drop downlist
            if (Convert.ToInt32(DropDownListYear.Text) == DateTime.Now.Year && DropDownListMonth.SelectedIndex >= DateTime.Now.Month)  // Outside date so return
                return;
            else 
                setStartDate(DropDownListYear.Text, DropDownListMonth.SelectedIndex);
        }
        protected void DropDownListYear_SelectedIndexChanged(object sender, EventArgs e) {
            if (Convert.ToInt32(DropDownListYear.Text) <= DateTime.Now.Year){
                setStartDate(DropDownListYear.Text, 0);
            }
        }*/
        
        #endregion

        #region Worker methods

        private void updateStartEnd(bool b) 
        { 
            // makes the start end date appear/disappear
            lbEndDate.Visible = b;
            dptbEndDate.Visible = b;
            lbStartDate.Visible = b;
            dptbStartDate.Visible = b;
            DatePickerTextBoxAvailabilityDate.Visible = !b;
            UpdatePanelReportSettings.Update();
        }
        
        private void setDates(System.Web.UI.WebControls.DropDownList dropDownList) 
        { 
            // Method to handle the date thats used
            
            if (dropDownList.Text.Contains("Custom")) 
            {
                customEndDate = dptbEndDate.getDate().AddHours(23).AddMinutes(59).AddSeconds(59);
                customStartDate = dptbStartDate.getDate();
            }
            else 
            {
                TextBox dateTextBox = (TextBox)this.DatePickerTextBoxAvailabilityDate.FindControl("TextBoxDatePicker");
                _dateAvailability = Convert.ToDateTime(dateTextBox.Text);
            }
            _dateRange = dropDownList.SelectedItem.Text;
            _dateRangeValue = dropDownList.SelectedValue;
        }
        
        private void setStartDate(string year, int index) 
        { 
            // the year as string (format 2010) and month as index (essentially month number - 1)
            setStartDate(Convert.ToInt32(year), index + 1);
        }
        
        private void setStartDate(int year, int month) 
        {
            DateTime dt = new DateTime(year, month, 1);
            dptbStartDate.StartDate = dt.ToString();
            customStartDate = dptbStartDate.getDate();
            UpdatePanelReportSettings.Update();
        }
        
        #endregion

        protected void DropDownListCAL_SelectedIndexChanged(object sender, EventArgs e) 
        {
            // Adds extra functionality for the dropDownBox to filter the Branch (location) to 
            // { Corporate, Agency, Licensee }
            

            if (RadioButtonListAvailabilityLogic.SelectedValue == "1") 
            {
                RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.CMSPool, false);
            }
            else 
            {
                RebindDropDownListsAvailability((int)App.BLL.ReportSettings.ReportOptions.OPSRegion, false);
            }
        }
    }
}