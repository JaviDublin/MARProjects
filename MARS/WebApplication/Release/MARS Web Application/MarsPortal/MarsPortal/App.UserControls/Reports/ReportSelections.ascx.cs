
namespace App.UserControls.Reports
{
    public partial class ReportSelections : System.Web.UI.UserControl
    {
        #region "Properties and Fields"
        
        private string _country;
        private int _logic;
        private string _cms_pool;
        private string _cms_location_group;
        private string _ops_region;
        private string _ops_area;
        private string _location;
        private string _car_segment;
        private string _car_class;
        private string _car_group;
        private string _checkInOut;
        private string _startDate;
        private string _endDate;
        private string _filter;
        private string _racfId;
        private string _dateRange;

        private string _dayOfWeek;
        
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public int Logic
        {
            get { return _logic; }
            set { _logic = value; }
        }

        public string CMS_Pool
        {
            get { return _cms_pool; }
            set { _cms_pool = value; }
        }

        public string CMS_Location_Group
        {
            get { return _cms_location_group; }
            set { _cms_location_group = value; }
        }

        public string OPS_Region
        {
            get { return _ops_region; }
            set { _ops_region = value; }
        }

        public string OPS_Area
        {
            get { return _ops_area; }
            set { _ops_area = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string Car_Segment
        {
            get { return _car_segment; }
            set { _car_segment = value; }
        }

        public string Car_Class
        {
            get { return _car_class; }
            set { _car_class = value; }
        }

        public string Car_Group
        {
            get { return _car_group; }
            set { _car_group = value; }
        }

        public string CheckInOut
        {
            get { return _checkInOut; }
            set { _checkInOut = value; }
        }

        public string StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public string EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        public string RacfID
        {
            get { return _racfId; }
            set { _racfId = value; }
        }

        public string DateRange
        {
            get { return _dateRange; }
            set { _dateRange = value; }
        }

        public string DayOfWeek
        {
            get { return _dayOfWeek; }
            set { _dayOfWeek = value; }
        }
        
        #endregion

        #region "Usercontrol Settings"

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


        public void ShowReportSelections()
        {
            //Set logic labels
            SetLogicVisibility(_marsTool, _logic);

            //Check which Mars tool we are using and which page is calling the report settings
            switch (_marsTool)
            {

                case (int)App.BLL.ReportSettings.ReportSettingsTool.Availability:
                case (int)App.BLL.ReportSettings.ReportSettingsTool.NonRevenue:
                    //Availability Tool and selected page
                    //Set usercontrol for availability tool and selected page
                    this.SetReportSelectionAvailability(_marsPage);

                    break;

                case (int)App.BLL.ReportSettings.ReportSettingsTool.Statistics:
                    //Statistics Tool
                    //Set usercontrol for statistics
                    this.SetReportSelectionStatistics();

                    break;
            }
        }

        protected void SetReportSelectionStatistics()
        {
            //Show Correct Panel
            this.PanelStatisticsReportSelection.Visible = true;

            //Set Common Items
            //Set label text with report selection values
            this.LabelStatisticsCountrySelection.Text = _country;
            if (_logic == (int)App.BLL.ReportSettings.OptionLogic.CMS)
            {
                this.LabelStatisticsCMSPoolSelection.Text = _cms_pool;
                this.LabelStatisticsCMSLocationGroupSelection.Text = _cms_location_group;

            }
            else if (_logic == (int)App.BLL.ReportSettings.OptionLogic.OPS)
            {
                this.LabelStatisticsOPSRegionSelection.Text = _ops_region;
                this.LabelStatisticsOPSAreaSelection.Text = _ops_area;
            }
            this.LabelStatisticsBranchSelection.Text = _location;
            this.LabelStatisticsStartDateSelection.Text = _startDate;
            this.LabelStatisticsEndDateSelection.Text = _endDate;
            this.LabelStatisticsRacfidSelection.Text = _racfId;



        }

        protected void SetReportSelectionAvailability(int marsPage)
        {
            //Show Correct Panel
            this.PanelAvailabilityReportSelection.Visible = true;

            //Set Common Items
            //Set label text with report selection values
            this.LabelAvailabilityCountrySelection.Text = _country;
            if (_logic == (int)App.BLL.ReportSettings.OptionLogic.CMS)
            {
                this.LabelAvailabilityCMSPoolSelection.Text = _cms_pool;
                this.LabelAvailabilityCMSLocationGroupSelection.Text = _cms_location_group;

            }
            else if (_logic == (int)App.BLL.ReportSettings.OptionLogic.OPS)
            {
                this.LabelAvailabilityOPSRegionSelection.Text = _ops_region;
                this.LabelAvailabilityOPSAreaSelection.Text = _ops_area;
            }
            if (!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison))
            {
                this.LabelAvailabilityBranchSelection.Text = _location;
                this.LabelAvailabilityBranch.Visible = true;
                this.LabelAvailabilityBranchSelection.Visible = true;
            }

            this.LabelAvailabilityCarSegmentSelection.Text = _car_segment;
            this.LabelAvailabilityCarClassSelection.Text = _car_class;

            if (!(_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison))
            {
                this.LabelAvailabilityCarGroup.Visible = true;
                this.LabelAvailabilityCarGroupSelection.Visible = true;
                this.LabelAvailabilityCarGroupSelection.Text = _car_group;
            }

            switch (_marsPage)
            {
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATKPI):
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATKPIDownload):
                    this.LabelAvailabilityDayOfWeek.Visible = false;
                    this.LabelAvailabilityDayOfWeekSelection.Visible = false;

                    break;
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.ATCarSearch):
                case ((int)App.BLL.ReportSettings.ReportSettingsPage.NRCarSearch):
                    this.LabelAvailabilityStartDate.Visible = false;
                    this.LabelAvailabilityStartDateSelection.Visible = false;
                    this.LabelAvailabilityDateRange.Visible = false;
                    this.LabelAvailabilityDateRangeSelection.Visible = false;
                    this.LabelAvailabilityDayOfWeek.Visible = false;
                    this.LabelAvailabilityDayOfWeekSelection.Visible = false;

                    break;
            }

            if (_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATKPI || _marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATKPIDownload)
            {
                this.LabelAvailabilityStartDateSelection.Text = _startDate;
                this.LabelAvailabilityDateRangeSelection.Text = _dateRange;
            }

            if ((_marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetComparison || _marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATSiteComparison || _marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATFleetStatus || _marsPage == (int)App.BLL.ReportSettings.ReportSettingsPage.ATHistoricalTrend))
            {
                this.LabelAvailabilityStartDateSelection.Text = _startDate;
                this.LabelAvailabilityDayOfWeekSelection.Text = _dayOfWeek;
                this.LabelAvailabilityDateRangeSelection.Text = _dateRange;
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
                        this.LabelAvailabilityOPSArea.Visible = false;
                        this.LabelAvailabilityOPSRegion.Visible = false;
                        this.LabelAvailabilityOPSAreaSelection.Visible = false;
                        this.LabelAvailabilityOPSRegionSelection.Visible = false;
                        //Show CMS Options
                        this.LabelAvailabilityCMSPool.Visible = true;
                        this.LabelAvailabilityCMSLocationGroup.Visible = true;
                        this.LabelAvailabilityCMSPoolSelection.Visible = true;
                        this.LabelAvailabilityCMSLocationGroupSelection.Visible = true;


                    }
                    else if (logic == (int)App.BLL.ReportSettings.OptionLogic.OPS)
                    {
                        //Hide CMS options
                        this.LabelAvailabilityCMSPool.Visible = false;
                        this.LabelAvailabilityCMSLocationGroup.Visible = false;
                        this.LabelAvailabilityCMSPoolSelection.Visible = false;
                        this.LabelAvailabilityCMSLocationGroupSelection.Visible = false;
                        //Show OPS Options
                        this.LabelAvailabilityOPSArea.Visible = true;
                        this.LabelAvailabilityOPSRegion.Visible = true;
                        this.LabelAvailabilityOPSAreaSelection.Visible = true;
                        this.LabelAvailabilityOPSRegionSelection.Visible = true;

                    }

                    break;


                case (int)App.BLL.ReportSettings.ReportSettingsTool.Statistics:

                    if (logic == (int)App.BLL.ReportSettings.OptionLogic.CMS)
                    {
                        //Hide OPS options
                        this.LabelStatisticsOPSArea.Visible = false;
                        this.LabelStatisticsOPSRegion.Visible = false;
                        this.LabelStatisticsOPSAreaSelection.Visible = false;
                        this.LabelStatisticsOPSRegionSelection.Visible = false;
                        //Show CMS Options
                        this.LabelStatisticsCMSPool.Visible = true;
                        this.LabelStatisticsCMSLocationGroup.Visible = true;
                        this.LabelStatisticsCMSPoolSelection.Visible = true;
                        this.LabelStatisticsCMSLocationGroupSelection.Visible = true;


                    }
                    else if (logic == (int)App.BLL.ReportSettings.OptionLogic.OPS)
                    {
                        //Hide CMS options
                        this.LabelStatisticsCMSPool.Visible = false;
                        this.LabelStatisticsCMSLocationGroup.Visible = false;
                        this.LabelStatisticsCMSPoolSelection.Visible = false;
                        this.LabelStatisticsCMSLocationGroupSelection.Visible = false;
                        //Show OPS Options
                        this.LabelStatisticsOPSArea.Visible = true;
                        this.LabelStatisticsOPSRegion.Visible = true;
                        this.LabelStatisticsOPSAreaSelection.Visible = true;
                        this.LabelStatisticsOPSRegionSelection.Visible = true;
                    }

                    break;
            }
        }

        #endregion
    }
}