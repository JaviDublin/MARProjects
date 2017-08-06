//=================================================================
//  File:		
//
//  Namespace:	
//
//	Classes:	
//
//  Purpose:	
//
//===================================================================
// 
//===================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rad.Web;

namespace App.BLL
{
    public class PageBase : Page
    {

        #region Page Methods

        /// <summary>
        /// Overide the Initialize culture event
        /// </summary>
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            //Check to see if we have a cookie value for selected culture
            if (Cookie.GetCookieString(Cookie.LanguagePreference) != null)
            {
                //Get the value for the culture
                object o = Cookie.GetCookieString(Cookie.LanguagePreference);

                //Convert the value into correct data type
                CultureInfo culture = new CultureInfo(o.ToString());

                //Apply the language setting
                ApplyLanguage(culture, false);

            }


        }

        /// <summary>
        /// Apply language culture and option to refresh page
        /// </summary>
        /// <param name="culture">Culture info en-GB</param>
        /// <param name="refresh">Refresh page or not</param>
        protected void ApplyLanguage(CultureInfo culture, bool refresh)
        {
            //Set the new culture
            Rad.Globalization.LanguageManager.ApplicationCurrentCulture = culture;

            //Add selected culture to cookie value
            if (refresh)
            {
                //Refresh the current page
                Response.Redirect(Request.Url.AbsoluteUri, false);
            }

        }

        #endregion

        #region Page Information

        private string _languageFile = "lang";

        protected void SetPageInformationTitle(string titleRK, UserControl ucPageInformation, bool hideUpdateLabel)
        {
            Label labelTitle = (Label)ucPageInformation.FindControl("LabelPageTitle");
            labelTitle.Text = HttpContext.GetGlobalResourceObject(_languageFile, titleRK).ToString();


            if ((hideUpdateLabel))
            {
                Label labelLastUpdate = (Label)ucPageInformation.FindControl("LabelLastUpdateInfo");
                labelLastUpdate.Visible = false;

            }

        }

        protected void SetDataImportUpdateLabel(int importTypeId, UserControl ucPageInformation)
        {
            //Set the last update label
            Label lastUpdateLabel = (Label)ucPageInformation.FindControl("LabelLastUpdate");

            //Set text for next update label
            Label nextUpdateLabelInfo = (Label)ucPageInformation.FindControl("LabelNextUpdateInfo");
            nextUpdateLabelInfo.Visible = true;
            Label nextUpdateLabel = (Label)ucPageInformation.FindControl("LabelNextUpdate");
            nextUpdateLabel.Visible = true;

            //Set Variables
            DateTime lastUpdate = DateTime.Now;
            DateTime nextUpdate = DateTime.Now;

            //Check which tool is being used
            switch (importTypeId)
            {

                case (int)ImportDetails.ImportType.Availability:

                    if (!(SessionHandler.LastUpdateTimeAvailability == null))
                    {
                        lastUpdateLabel.Text = SessionHandler.LastUpdateTimeAvailability.ToString() + " GMT";
                        nextUpdateLabel.Text = SessionHandler.NextUpdateTimeAvailability.ToString() + " GMT";

                    }
                    else
                    {
                        lastUpdate = ImportDetails.GetLastDataImportTime(importTypeId);
                        lastUpdateLabel.Text = lastUpdate.ToString() + " GMT";
                        //Update the label that to show the next time data import is due
                        nextUpdate = ImportDetails.GetNextDataImportTime(importTypeId);
                        nextUpdateLabel.Text = nextUpdate.ToString() + " GMT";

                        //Set session value for last update so we can get the value throughout application
                        SessionHandler.LastUpdateTimePooling = lastUpdate;
                        SessionHandler.NextUpdateTimePooling = nextUpdate;

                    }
                    break;
            }


        }

        #endregion

        #region "Report Settings"


        protected int CheckForQueryString(HttpRequest request)
        {


            if (((request.QueryString != null)))
            {
                //There is a query string
                return 0;
            }
            else
            {
                //No query string
                return -1;
            }

        }

        protected int CheckUserLastSetting()
        {

            //Check session values
            if ((!(SessionHandler.ULSLogic == null)))
            {
                //Last settings exist
                return 0;
            }
            else
            {
                //no last settings exist
                return -1;
            }

        }

        protected void SaveLastSelectionToSession(int logic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id,
                                                    int ops_area_id, string location, int? car_segment_id, int? car_class_id, int? car_group_id)
        {

            //Clear last selection session values
            SessionHandler.ClearLastSelectionSessionValues();

            //Set session values
            if (!(logic == -1))
            {
                SessionHandler.ULSLogic = logic;
            }

            if (!(country == null))
            {
                SessionHandler.ULSCountry = country;
            }
            if (!(cms_pool_id == -1))
            {
                SessionHandler.ULSCMSPoolId = cms_pool_id;
            }
            if (!(cms_location_group_id == -1))
            {
                SessionHandler.ULSCMSLocationGroupCode = cms_location_group_id;
            }
            if (!(ops_region_id == -1))
            {
                SessionHandler.ULSOPSRegionId = ops_region_id;
            }
            if (!(ops_area_id == -1))
            {
                SessionHandler.ULSOPSAreaId = ops_area_id;
            }
            if (!(car_segment_id == -1))
            {
                SessionHandler.ULSCarSegmentId = car_segment_id;
            }
            if (!(car_class_id == -1))
            {
                SessionHandler.ULSCarClassId = car_class_id;
            }
            if (!(car_group_id == -1))
            {
                SessionHandler.ULSCarGroupId = car_group_id;
            }
            if (!(location == null))
            {
                SessionHandler.ULSLocation = location;
            }


        }

        protected List<ReportPreferences> GetPreferencesSettings(bool checkLastSelection = true)
        {


            int userPreference = -1;
            int logic = -1;
            string country = "-1";
            int cms_pool_id = -1;
            int cms_location_group_id = -1;
            string location = "-1";
            int ops_region_id = -1;
            int ops_area_id = -1;
            int car_segment_id = -1;
            int car_class_id = -1;
            int car_group_id = -1;

            bool hasValues = false;


            if (hasValues == false)
            {
                if (checkLastSelection)
                {
                    if ((CheckUserLastSetting() == 0))
                    {
                        //Get the session values and set dropdownlists
                        logic = Convert.ToInt32(SessionHandler.ULSLogic);
                        country = SessionHandler.ULSCountry;

                        switch (logic)
                        {

                            case (int)ReportSettings.OptionLogic.CMS:
                                cms_pool_id = Convert.ToInt32(SessionHandler.ULSCMSPoolId);
                                cms_location_group_id = SessionHandler.ULSCMSLocationGroupCode ?? 0;

                                break;
                            case (int)ReportSettings.OptionLogic.OPS:
                                ops_region_id = Convert.ToInt32(SessionHandler.ULSOPSRegionId);
                                ops_area_id = Convert.ToInt32(SessionHandler.ULSOPSAreaId);

                                break;
                        }
                        location = SessionHandler.ULSLocation;
                        car_segment_id = Convert.ToInt32(SessionHandler.ULSCarSegmentId);
                        car_class_id = Convert.ToInt32(SessionHandler.ULSCarClassId);
                        car_group_id = Convert.ToInt32(SessionHandler.ULSCarGroupId);
                        userPreference = (int)UserPreferences.Type.ULS;
                        hasValues = true;
                    }
                }

            }

            if (hasValues == true)
            {
                return new List<ReportPreferences>
                                                      {
                                                          new ReportPreferences(logic, country, cms_pool_id,
                                                                                cms_location_group_id, ops_region_id,
                                                                                ops_area_id,
                                                                                car_segment_id, car_class_id,
                                                                                car_group_id, userPreference, location,
                                                                                null, null,
                                                                                null, null, null, null, null)
                                                      };
            }
            return null;

        }

        protected List<ReportPreferences> CheckForQueryString()
        {


            int optionLogic = -1;
            string country = "-1";
            int cms_pool_id = -1;
            int cms_location_group_id = -1;
            int ops_region_id = -1;
            int ops_area_id = -1;
            string location = "-1";
            int car_segment_id = -1;
            int car_class_id = -1;
            int car_group_id = -1;
            int? actuals = null;
            string dateValue = null;
            string fleetname = null;
            string operstat = null;
            int? fleetStatus = null;
            int? dayOfWeek = null;
            int? dateRange = null;


            if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OptionLogic)])))
            {
                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Country)])))
                {
                    country = Convert.ToString(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Country)]);
                }

                optionLogic = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OptionLogic)]);

                switch (optionLogic)
                {

                    case (int)ReportSettings.OptionLogic.CMS:
                        //Get query strings
                        if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Pool_Id)])))
                        {
                            cms_pool_id = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Pool_Id)]);
                        }
                        if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Location_Group_Code)])))
                        {
                            cms_location_group_id = int.Parse(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.CMS_Location_Group_Code)]);
                        }

                        break;
                    case (int)ReportSettings.OptionLogic.OPS:
                        //Get query strings
                        if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Region_Id)])))
                        {
                            ops_region_id = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Region_Id)]);
                        }
                        if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Area_Id)])))
                        {
                            ops_area_id = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.OPS_Area_Id)]);
                        }

                        break;
                }

                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Location)])))
                {
                    location = Convert.ToString(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Location)]);
                }
                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Segment_Id)])))
                {
                    car_segment_id = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Segment_Id)]);
                }
                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Class_Id)])))
                {
                    car_class_id = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Class_Id)]);
                }
                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Group_Id)])))
                {
                    car_group_id = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Car_Group_Id)]);
                }

                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Actuals)])))
                {
                    actuals = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Actuals)]);
                }

                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateValue)])))
                {
                    dateValue = Convert.ToString(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateValue)]);
                }

                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetName)])))
                {
                    fleetname = Convert.ToString(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetName)]);
                }

                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Operstat)])))
                {
                    operstat = Convert.ToString(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.Operstat)]);
                }

                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetStatus)])))
                {
                    fleetStatus = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.FleetStatus)]);
                }

                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DayOfWeek)])))
                {
                    dayOfWeek = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DayOfWeek)]);
                }

                if ((!string.IsNullOrEmpty(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateRange)])))
                {
                    dateRange = Convert.ToInt32(Request.QueryString[QueryStringHandler.QueryStr((int)QueryStringHandler.QueryString.DateRange)]);
                }

                List<ReportPreferences> results = new List<ReportPreferences>();
                results.Add(new ReportPreferences(optionLogic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, car_segment_id,
                                                    car_class_id, car_group_id, (int)UserPreferences.Type.QSTR, location, actuals, dateValue, fleetname, operstat,
                                                        fleetStatus, dayOfWeek, dateRange));
                return results;

            }
            else
            {
                return null;
            }


        }
        
        #endregion

        #region "Usage Statistics"


        protected void LogUsageStatistics(int reportId, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id, string location)
        {

            ReportStatistics.InsertStatistics(reportId, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, Users.GetUserRACFID(), DateTime.Now);

        }

        #endregion

    }
}