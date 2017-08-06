using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using App.BLL;
using App.DAL;

namespace App.Site.NonRevenue.CarSearch
{
    public partial class Default : PageBase
    {
        #region "Page Events"

        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.FormFilter.MarsTool = (int)ReportSettings.ReportSettingsTool.NonRevenue;
            
            this.FormFilter.MarsPage = (int)ReportSettings.ReportSettingsPage.NRCarSearch;

            this.Page.Title = "MARS - Non Revenue Tool";

            if (!Page.IsPostBack)
            {
                List<ReportPreferences> queryStringResults = base.CheckForQueryString();

                if ((queryStringResults != null))
                {
                    foreach (ReportPreferences row in queryStringResults)
                    {
                        this.FormFilter.LoadFormSettingsControl(false);

                        this.FormFilter.SetUserDefaultSettingsTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id,
                            row.OPS_Region_Id, row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference,
                              row.Location, row.FleetName, row.Operstat, Convert.ToInt32(row.FleetStatus), Conversions.ConvertStringToDate(row.DateValue), Convert.ToInt32(row.DayOfWeek), Convert.ToInt32(row.DateRange));
                    }

                    //Load data
                    GenerateReport();
                }
                else
                {
                    CheckReportPreferencesSettings();
                }
 
             
                this.SetNavigtionMenu();
            }
        }

        protected void GenerateReport()
        {
            int selectedLogic = this.FormFilter.Logic;

            //Set values from report selection
            string country = this.FormFilter.Country;

            //Set default values for CMS / OPS logic
            int cms_pool_id = -1;
            string cms_pool = null;
            int cms_location_group_id = -1;
            string cms_location_group = null;

            int ops_region_id = -1;
            string ops_region = null;
            int ops_area_id = -1;
            string ops_area = null;

            //Check option logic
            switch (selectedLogic)
            {
                case (int)ReportSettings.OptionLogic.CMS:
                    //Set Values for CMS
                    cms_pool_id = this.FormFilter.CMS_Pool_Id;
                    cms_pool = this.FormFilter.CMS_Pool;
                    cms_location_group_id = this.FormFilter.CMS_Location_Group_Id;
                    cms_location_group = this.FormFilter.CMS_Location_Group;

                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    //Set Values for OPS
                    ops_region_id = this.FormFilter.OPS_Region_Id;
                    ops_region = this.FormFilter.OPS_Region;
                    ops_area_id = this.FormFilter.OPS_Area_Id;
                    ops_area = this.FormFilter.OPS_Area;

                    break;
            }

            //Set common values
            string location = this.FormFilter.Location;
            int car_segment_id = this.FormFilter.Car_Segment_Id == 0 ? -1 : FormFilter.Car_Segment_Id;
            string car_segment = this.FormFilter.Car_Segment;
            int car_class_id = this.FormFilter.Car_Class_Id == 0 ? -1 : FormFilter.Car_Class_Id;
            string car_class = this.FormFilter.Car_Class;
            int car_group_id = this.FormFilter.Car_Group_Id == 0 ? -1 : FormFilter.Car_Group_Id;
            string car_group = this.FormFilter.Car_Group;
            string ownArea = this.FormFilter.OwnArea;
            string modelCode = this.FormFilter.ModelCode;
            string status = this.FormFilter.Status;
            string selectBy = this.FormFilter.CarSearch_SelectBy;
            string fleet_name = this.FormFilter.Fleet_Name ?? "-1";

            //Set Selection Session Values
            this.SetSelectionSessionValues(country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id, fleet_name,
            ownArea, modelCode, status, selectBy);

            ////Set Last Selection Value
            base.SaveLastSelectionToSession(selectedLogic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id);


            ////Log Usage Statistics
            base.LogUsageStatistics((int)ReportStatistics.ReportName.NonRevCarSearch, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location);

            this.UpdatePanelCarSearchNonRev.Update();

        }

        protected void CheckReportPreferencesSettings()
        {
            List<ReportPreferences> preferences = base.GetPreferencesSettings();

            if ((preferences != null))
            {
                foreach (ReportPreferences row in preferences)
                {
                    this.FormFilter.LoadFormSettingsControl(false);

                    this.FormFilter.SetUserDefaultSettingsTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id,
                            row.OPS_Region_Id, row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference,
                              row.Location, row.FleetName, row.Operstat, Convert.ToInt32(row.FleetStatus), Conversions.ConvertStringToDate(row.DateValue), Convert.ToInt32(row.DayOfWeek), Convert.ToInt32(row.DateRange));
                }
            }
            else
            {
                this.FormFilter.LoadFormSettingsControl(true);
            }

            this.UpdatePanelCarSearchNonRev.Update();
        }
        
        #endregion

        #region "Button Events"

        protected void ButtonGenerateReport_Click(object sender, System.EventArgs e)
        {
            SessionHandler.ClearNonRevCarSearchGridviewSessions();
            GenerateReport();
        }

        #endregion

        #region "Navigation Events"

        protected void NavigationMenuClick(object sender, Navigation e)
        {
            int index = e.Index;
            this.MultiViewNonRev.ActiveViewIndex = index;
            this.UpdatePanelCarSearchNonRev.Update();
        }

        protected void SetNavigtionMenu()
        {
            var results = this.NavigationPanelNonRev.LoadControlData((int)MenuType.MenuMultiViewA);
            int index = NavigationMenu.GetMinimumIndex(results);
            this.MultiViewNonRev.ActiveViewIndex = index;
            this.NavigationPanelNonRev.SetMenuStyle(this.MultiViewNonRev.ActiveViewIndex);
        }

        #endregion

        protected void SetSelectionSessionValues(string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id,
                                                    string location, int car_segment_id, int car_class_id, int car_group_id, string fleet_name,
                                                        string ownArea, string modelCode, string status, string selectBy)
        {
            SessionHandler.NonRevCarSearchCountry = country;
            SessionHandler.NonRevCarSearchCMSPoolId = cms_pool_id;
            SessionHandler.NonRevCarSearchCMSLocationGroupCode = cms_location_group_id;
            SessionHandler.NonRevCarSearchOPSRegionId = ops_region_id;
            SessionHandler.NonRevCarSearchOPSAreaId = ops_area_id;
            SessionHandler.NonRevCarSearchLocation = location;
            SessionHandler.NonRevCarSearchCarSegmentId = car_segment_id;
            SessionHandler.NonRevCarSearchCarClassId = car_class_id;
            SessionHandler.NonRevCarSearchCarGroupId = car_group_id;
            SessionHandler.NonRevCarSearchFleetName = fleet_name;
            SessionHandler.NonRevCarSearchSelectBy = selectBy;
            SessionHandler.NonRevCarSearchStatus = status;
            SessionHandler.NonRevCarSearchModelCode = modelCode;
            SessionHandler.NonRevCarSearchOwnArea = ownArea;
        }

    }
}