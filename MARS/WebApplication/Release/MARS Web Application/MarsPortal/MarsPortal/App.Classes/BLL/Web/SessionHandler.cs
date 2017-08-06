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

using System.Globalization;
using System.Web;
using System;


namespace App.BLL
{

    /// <summary>
    /// This class references all Session Variables used in the application
    /// </summary>
    public static class SessionHandler
    {

        #region Application - Culture Settings

        #region Declarations

        private static readonly string _ApplicationCulture = @"SV:Application-Culture";

        #endregion

        #region Properties

        /// <summary>
        /// Application Culture
        /// </summary>
        public static CultureInfo ApplicationCulture
        {
            get
            {
                object o = HttpContext.Current.Session[_ApplicationCulture];
                return ((o != null) ? (CultureInfo)o : null);
            }
            set
            {
                HttpContext.Current.Session[_ApplicationCulture] = value;
            }
        }

        #endregion

        #endregion

        #region "User Preferences Report Settings"

        //SV = Session Variable
        //UPRS = User Preferences Report Settings
        private static string _UPRSLogic = "SV-UPRS-Logic";
        private static string _UPRSCountry = "SV-UPRS-Country";
        private static string _UPRSCMSPoolId = "SV-UPRS-CMSPoolId";
        private static string _UPRSCMSLocationGroupCode = "SV-UPRS-CMSLocationGroupCode";

        private static string _UPRSLocation = "SV-UPRS-Location";

        public static int? UPRSLogic
        {
            get
            {
                if ((HttpContext.Current.Session[_UPRSLogic] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_UPRSLogic]));
                }
            }
            set { HttpContext.Current.Session[_UPRSLogic] = value; }
        }

        public static string UPRSCountry
        {
            get
            {
                if ((HttpContext.Current.Session[_UPRSCountry] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_UPRSCountry]));
                }
            }
            set { HttpContext.Current.Session[_UPRSCountry] = value; }
        }

        public static int? UPRSCMSPoolId
        {
            get
            {
                if ((HttpContext.Current.Session[_UPRSCMSPoolId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_UPRSCMSPoolId]));
                }
            }
            set { HttpContext.Current.Session[_UPRSCMSPoolId] = value; }
        }

        public static string UPRSCMSLocationGroupCode
        {
            get
            {
                if ((HttpContext.Current.Session[_UPRSCMSLocationGroupCode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_UPRSCMSLocationGroupCode]));
                }
            }
            set { HttpContext.Current.Session[_UPRSCMSLocationGroupCode] = value; }
        }

        public static string UPRSLocation
        {
            get
            {
                if ((HttpContext.Current.Session[_UPRSLocation] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_UPRSLocation]));
                }
            }
            set { HttpContext.Current.Session[_UPRSLocation] = value; }
        }

        #endregion

        #region "User Last Selection"

        private static string _ULSLogic = "SV-ULS-Logic";
        private static string _ULSCountry = "SV-ULS-Country";
        private static string _ULSCMSPoolId = "SV-ULS-CMSPoolId";
        private static string _ULSCMSLocationGroupCode = "SV-ULS-CMSLocationGroupCode";
        private static string _ULSOPSRegionId = "SV-ULS-OPSRegionId";
        private static string _ULSOPSAreaId = "SV-ULS-OPSAreaId";
        private static string _ULSLocation = "SV-ULS-Location";
        private static string _ULSCarSegmentId = "SV-ULS-CarSegmentId";
        private static string _ULSCarClassId = "SV-ULS-CarClassId";

        private static string _ULSCarGroupId = "SV-ULS-CarGroupId";

        public static int? ULSLogic
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSLogic] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_ULSLogic]));
                }
            }
            set { HttpContext.Current.Session[_ULSLogic] = value; }
        }

        public static string ULSCountry
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSCountry] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_ULSCountry]));
                }
            }
            set { HttpContext.Current.Session[_ULSCountry] = value; }
        }

        public static int? ULSCMSPoolId
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSCMSPoolId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_ULSCMSPoolId]));
                }
            }
            set { HttpContext.Current.Session[_ULSCMSPoolId] = value; }
        }

        public static int? ULSCMSLocationGroupCode
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSCMSLocationGroupCode] == null))
                {
                    return null;
                }

                return (int.Parse(HttpContext.Current.Session[_ULSCMSLocationGroupCode].ToString()));

            }
            set { HttpContext.Current.Session[_ULSCMSLocationGroupCode] = value; }
        }

        public static int? ULSOPSRegionId
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSOPSRegionId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_ULSOPSRegionId]));
                }
            }
            set { HttpContext.Current.Session[_ULSOPSRegionId] = value; }
        }

        public static int? ULSOPSAreaId
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSOPSAreaId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_ULSOPSAreaId]));
                }
            }
            set { HttpContext.Current.Session[_ULSOPSAreaId] = value; }
        }
        public static string ULSLocation
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSLocation] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_ULSLocation]));
                }
            }
            set { HttpContext.Current.Session[_ULSLocation] = value; }
        }
        public static int? ULSCarSegmentId
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSCarSegmentId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_ULSCarSegmentId]));
                }
            }
            set { HttpContext.Current.Session[_ULSCarSegmentId] = value; }
        }

        public static int? ULSCarClassId
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSCarClassId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_ULSCarClassId]));
                }
            }
            set { HttpContext.Current.Session[_ULSCarClassId] = value; }
        }

        public static int? ULSCarGroupId
        {
            get
            {
                if ((HttpContext.Current.Session[_ULSCarGroupId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_ULSCarGroupId]));
                }
            }
            set { HttpContext.Current.Session[_ULSCarGroupId] = value; }
        }

        public static void ClearLastSelectionSessionValues()
        {
            ULSLogic = null;
            ULSCountry = null;
            ULSCMSPoolId = null;
            ULSCMSLocationGroupCode = null;
            ULSOPSRegionId = null;
            ULSOPSAreaId = null;
            ULSCarSegmentId = null;
            ULSCarClassId = null;
            ULSCarGroupId = null;
            ULSLocation = null;
        }
        #endregion

        #region "Report Selection"

        private static string _LastUpdateTimePooling = "SV-LastUpdateTime-Pooling";

        private static string _NextUpdateTimePooling = "SV-NextUpdateTime-Pooling";
        private static string _LastUpdateTimeAvailability = "SV-LastUpdateTime-Availability";

        private static string _NextUpdateTimeAvailability = "SV-NextUpdateTime-Availability";
        public static DateTime? LastUpdateTimePooling
        {
            get
            {
                if ((HttpContext.Current.Session[_LastUpdateTimePooling] == null))
                {
                    return null;
                }
                else
                {
                    return ((DateTime)HttpContext.Current.Session[_LastUpdateTimePooling]);
                }
            }
            set { HttpContext.Current.Session[_LastUpdateTimePooling] = value; }
        }

        public static DateTime? NextUpdateTimeAvailability
        {
            get
            {
                if ((HttpContext.Current.Session[_NextUpdateTimeAvailability] == null))
                {
                    return null;
                }
                else
                {
                    return ((DateTime)HttpContext.Current.Session[_NextUpdateTimeAvailability]);
                }
            }
            set { HttpContext.Current.Session[_NextUpdateTimeAvailability] = value; }
        }

        public static DateTime? LastUpdateTimeAvailability
        {
            get
            {
                if ((HttpContext.Current.Session[_LastUpdateTimeAvailability] == null))
                {
                    return null;
                }
                else
                {
                    return ((DateTime)HttpContext.Current.Session[_LastUpdateTimeAvailability]);
                }
            }
            set { HttpContext.Current.Session[_LastUpdateTimeAvailability] = value; }
        }

        public static DateTime? NextUpdateTimePooling
        {
            get
            {
                if ((HttpContext.Current.Session[_NextUpdateTimePooling] == null))
                {
                    return null;
                }
                else
                {
                    return ((DateTime)HttpContext.Current.Session[_NextUpdateTimePooling]);
                }
            }
            set { HttpContext.Current.Session[_NextUpdateTimePooling] = value; }
        }

        #endregion

        #region "Availability Tool"

        #region "FleetStatus"
        
        private static string _AvailabilityFleetStatusDataTable = "SV-AvailabilityFleetStatus-DataTable";

        private static string _AvailabilityFleetStatusRedirectQueryString = "SV-AvailabilityFleetStatus-RedirectQueryString";

        private static string _OnRentTypeSelected = "OnRentTypeSelected";

        public static string OnRentTypeSelected
        {
            get { return HttpContext.Current.Session[_OnRentTypeSelected] == null ? "1" : HttpContext.Current.Session[_OnRentTypeSelected].ToString() ; }
            set { HttpContext.Current.Session[_OnRentTypeSelected] = value; }
        }

        public static System.Data.DataTable AvailabilityFleetStatusDataTable
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityFleetStatusDataTable] == null))
                {
                    return null;
                }
                else
                {
                    return ((System.Data.DataTable)HttpContext.Current.Session[_AvailabilityFleetStatusDataTable]);
                }
            }
            set { HttpContext.Current.Session[_AvailabilityFleetStatusDataTable] = value; }
        }

        public static string AvailabilityFleetStatusRedirectQueryString
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityFleetStatusRedirectQueryString] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityFleetStatusRedirectQueryString]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityFleetStatusRedirectQueryString] = value; }
        }

        public static void ClearFleetStatusSessions()
        {
            AvailabilityFleetStatusDataTable = null;
            AvailabilityFleetStatusRedirectQueryString = null;
        }
        
        #endregion

        #region "Car Search"
        
        private static string _AvailabilityCarSearchSortExpression = "SV-AvailabilityCarSearch-SortExpression";
        private static string _AvailabilityCarSearchSortDirection = "SV-AvailabilityCarSearch-SortDirection";
        private static string _AvailabilityCarSearchSortOrder = "SV-AvailabilityCarSearch-SortOrder";
        private static string _AvailabilityCarSearchPageSize = "SV-AvailabilityCarSearch-PageSize";
        private static string _AvailabilityCarSearchCurrentPageNumber = "SV-AvailabilityCarSearch-CurrentPageNumber";
        private static string _AvailabilityCarSearchCountry = "SV-AvailabilityCarSearch-Country";
        private static string _AvailabilityCarSearchCMSPoolId = "SV-AvailabilityCarSearch-CMSPoolId";
        private static string _AvailabilityCarSearchCMSLocationGroupCode = "SV-AvailabilityCarSearch-CMSLocationGroupCode";
        private static string _AvailabilityCarSearchOPSRegionId = "SV-AvailabilityCarSearch-OPSRegionId";
        private static string _AvailabilityCarSearchOPSAreaId = "SV-AvailabilityCarSearch-OPSAreaId";
        private static string _AvailabilityCarSearchLocation = "SV-AvailabilityCarSearch-Location";
        private static string _AvailabilityCarSearchCarSegmentId = "SV-AvailabilityCarSearch-CarSegmentId";
        private static string _AvailabilityCarSearchCarClassId = "SV-AvailabilityCarSearch-CarClassId";
        private static string _AvailabilityCarSearchCarGroupId = "SV-AvailabilityCarSearch-CarGroupId";
        private static string _AvailabilityCarSearchFleetName = "SV-AvailabilityCarSearch-FleetName";
        private static string _AvailabilityCarSearchNoRev = "SV-AvailabilityCarSearch-NoRev";
        private static string _AvailabilityCarSearchModelCode = "SV-AvailabilityCarSearch-ModelCode";
        private static string _AvailabilityCarSearchStatus = "SV-AvailabilityCarSearch-Status";
        private static string _AvailabilityCarSearchOwnArea = "SV-AvailabilityCarSearch-OwnArea";
        private static string _AvailabilityCarSearchSelectBy = "SV-AvailabilityCarSearch-SelectBy";

        private static string _AvailabilityCarSearchReturnQueryString = "SV-AvailabilityCarSearch-ReturnQueryString";

        public static string AvailabilityCarSearchSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchSortExpression] = value; }
        }

        public static int? AvailabilityCarSearchSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchSortDirection] = value; }
        }

        public static string AvailabilityCarSearchSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchSortOrder] = value; }
        }

        public static int? AvailabilityCarSearchPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchPageSize]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchPageSize] = value; }
        }

        public static int? AvailabilityCarSearchCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchCurrentPageNumber] = value; }
        }

        public static string AvailabilityCarSearchCountry
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchCountry] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchCountry]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchCountry] = value; }
        }

        public static int? AvailabilityCarSearchCMSPoolId
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchCMSPoolId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchCMSPoolId]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchCMSPoolId] = value; }
        }

        public static int? AvailabilityCarSearchCMSLocationGroupCode
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchCMSLocationGroupCode] == null))
                {
                    return null;
                }
                return (int.Parse(HttpContext.Current.Session[_AvailabilityCarSearchCMSLocationGroupCode].ToString()));

            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchCMSLocationGroupCode] = value; }
        }

        public static int? AvailabilityCarSearchOPSRegionId
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchOPSRegionId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchOPSRegionId]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchOPSRegionId] = value; }
        }

        public static int? AvailabilityCarSearchOPSAreaId
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchOPSAreaId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchOPSAreaId]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchOPSAreaId] = value; }
        }

        public static string AvailabilityCarSearchLocation
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchLocation] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchLocation]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchLocation] = value; }
        }

        public static int? AvailabilityCarSearchCarSegmentId
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchCarSegmentId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchCarSegmentId]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchCarSegmentId] = value; }
        }

        public static int? AvailabilityCarSearchCarClassId
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchCarClassId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchCarClassId]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchCarClassId] = value; }
        }

        public static int? AvailabilityCarSearchCarGroupId
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchCarGroupId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchCarGroupId]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchCarGroupId] = value; }
        }

        public static string AvailabilityCarSearchFleetName
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchFleetName] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchFleetName]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchFleetName] = value; }
        }

        public static int? AvailabilityCarSearchNoRev
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchNoRev] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityCarSearchNoRev]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchNoRev] = value; }
        }

        public static string AvailabilityCarSearchModelCode
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchModelCode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchModelCode]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchModelCode] = value; }
        }

        public static string AvailabilityCarSearchOwnArea
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchOwnArea] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchOwnArea]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchOwnArea] = value; }
        }

        public static string AvailabilityCarSearchStatus
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchStatus] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchStatus]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchStatus] = value; }
        }

        public static string AvailabilityCarSearchSelectBy
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchSelectBy] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchSelectBy]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchSelectBy] = value; }
        }

        public static string AvailabilityCarSearchReturnQueryString
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityCarSearchReturnQueryString] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityCarSearchReturnQueryString]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityCarSearchReturnQueryString] = value; }
        }

        public static void ClearAvailabilityCarSearchSessions()
        {

            AvailabilityCarSearchSortExpression = null;
            AvailabilityCarSearchSortDirection = null;
            AvailabilityCarSearchSortOrder = null;
            AvailabilityCarSearchPageSize = null;
            AvailabilityCarSearchCurrentPageNumber = null;
            AvailabilityCarSearchCountry = null;
            AvailabilityCarSearchCMSPoolId = null;
            AvailabilityCarSearchCMSLocationGroupCode = null;
            AvailabilityCarSearchOPSRegionId = null;
            AvailabilityCarSearchOPSAreaId = null;
            AvailabilityCarSearchLocation = null;
            AvailabilityCarSearchCarSegmentId = null;
            AvailabilityCarSearchCarClassId = null;
            AvailabilityCarSearchCarGroupId = null;
            AvailabilityCarSearchFleetName = null;
            AvailabilityCarSearchNoRev = null;
            AvailabilityCarSearchSelectBy = null;
            AvailabilityCarSearchStatus = null;
            AvailabilityCarSearchModelCode = null;
            AvailabilityCarSearchOwnArea = null;
            AvailabilityCarSearchReturnQueryString = null;


        }

        public static void ClearAvailabilityCarSearchGridviewSessions()
        {
            AvailabilityCarSearchSortExpression = null;
            AvailabilityCarSearchSortDirection = null;
        }

        #endregion

        #region "Statistics"

        private static string _AvailabilityStatisticsSelectionSortExpression = "SV-AvailabilityStatisticsSelection-SortExpression";
        private static string _AvailabilityStatisticsSelectionSortDirection = "SV-AvailabilityStatisticsSelection-SortDirection";
        private static string _AvailabilityStatisticsSelectionSortOrder = "SV-AvailabilityStatisticsSelection-SortOrder";
        private static string _AvailabilityStatisticsSelectionPageSize = "SV-AvailabilityStatisticsSelection-PageSize";

        private static string _AvailabilityStatisticsSelectionCurrentPageNumber = "SV-AvailabilityStatisticsSelection-CurrentPageNumber";
        private static string _AvailabilityStatisticsDateSortExpression = "SV-AvailabilityStatisticsDate-SortExpression";
        private static string _AvailabilityStatisticsDateSortDirection = "SV-AvailabilityStatisticsDate-SortDirection";
        private static string _AvailabilityStatisticsDateSortOrder = "SV-AvailabilityStatisticsDate-SortOrder";
        private static string _AvailabilityStatisticsDatePageSize = "SV-AvailabilityStatisticsDate-PageSize";

        private static string _AvailabilityStatisticsDateCurrentPageNumber = "SV-AvailabilityStatisticsDate-CurrentPageNumber";
        private static string _AvailabilityStatisticsLogic = "SV-AvailabilityStatistics-Logic";
        private static string _AvailabilityStatisticsCountry = "SV-AvailabilityStatistics-Country";
        private static string _AvailabilityStatisticsCMSPoolId = "SV-AvailabilityStatistics-CMSPoolId";
        private static string _AvailabilityStatisticsCMSLocationGroupCode = "SV-AvailabilityStatistics-CMSLocationGroupCode";
        private static string _AvailabilityStatisticsOPSRegionId = "SV-AvailabilityStatistics-OPSRegionId";
        private static string _AvailabilityStatisticsOPSAreaId = "SV-AvailabilityStatistics-OPSAreaId";
        private static string _AvailabilityStatisticsLocation = "SV-AvailabilityStatistics-Location";
        private static string _AvailabilityStatisticsStartDate = "SV-AvailabilityStatistics-StartDate";
        private static string _AvailabilityStatisticsEndDate = "SV-AvailabilityStatistics-EndDate";

        private static string _AvailabilityStatisticsRacfID = "SV-AvailabilityStatistics-RACFID";
        public static string AvailabilityStatisticsSelectionSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsSelectionSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityStatisticsSelectionSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsSelectionSortExpression] = value; }
        }

        public static int? AvailabilityStatisticsSelectionSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsSelectionSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsSelectionSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsSelectionSortDirection] = value; }
        }

        public static string AvailabilityStatisticsSelectionSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsSelectionSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityStatisticsSelectionSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsSelectionSortOrder] = value; }
        }

        public static int? AvailabilityStatisticsSelectionPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsSelectionPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsSelectionPageSize]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsSelectionPageSize] = value; }
        }

        public static int? AvailabilityStatisticsSelectionCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsSelectionCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsSelectionCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsSelectionCurrentPageNumber] = value; }
        }

        public static string AvailabilityStatisticsDateSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsDateSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityStatisticsDateSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsDateSortExpression] = value; }
        }

        public static int? AvailabilityStatisticsDateSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsDateSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsDateSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsDateSortDirection] = value; }
        }

        public static string AvailabilityStatisticsDateSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsDateSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityStatisticsDateSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsDateSortOrder] = value; }
        }

        public static int? AvailabilityStatisticsDatePageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsDatePageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsDatePageSize]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsDatePageSize] = value; }
        }

        public static int? AvailabilityStatisticsDateCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsDateCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsDateCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsDateCurrentPageNumber] = value; }
        }

        public static int? AvailabilityStatisticsLogic
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsLogic] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsLogic]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsLogic] = value; }
        }

        public static string AvailabilityStatisticsCountry
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsCountry] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityStatisticsCountry]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsCountry] = value; }
        }

        public static int? AvailabilityStatisticsCMSPoolId
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsCMSPoolId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsCMSPoolId]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsCMSPoolId] = value; }
        }

        public static int? AvailabilityStatisticsCMSLocationGroupCode
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsCMSLocationGroupCode] == null))
                {
                    return null;
                }
                else
                {
                    return int.Parse(HttpContext.Current.Session[_AvailabilityStatisticsCMSLocationGroupCode].ToString());
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsCMSLocationGroupCode] = value; }
        }

        public static int? AvailabilityStatisticsOPSRegionId
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsOPSRegionId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsOPSRegionId]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsOPSRegionId] = value; }
        }

        public static int? AvailabilityStatisticsOPSAreaId
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsOPSAreaId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_AvailabilityStatisticsOPSAreaId]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsOPSAreaId] = value; }
        }

        public static string AvailabilityStatisticsLocation
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsLocation] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityStatisticsLocation]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsLocation] = value; }
        }

        public static DateTime? AvailabilityStatisticsStartDate
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsStartDate] == null))
                {
                    return null;
                }
                else
                {
                    return ((DateTime)HttpContext.Current.Session[_AvailabilityStatisticsStartDate]);
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsStartDate] = value; }
        }

        public static DateTime? AvailabilityStatisticsEndDate
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsEndDate] == null))
                {
                    return null;
                }
                else
                {
                    return ((DateTime)HttpContext.Current.Session[_AvailabilityStatisticsEndDate]);
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsEndDate] = value; }
        }

        public static string AvailabilityStatisticsRacfID
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityStatisticsRacfID] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityStatisticsRacfID]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityStatisticsRacfID] = value; }
        }

        public static void ClearAvailabilityStatisticsSessions()
        {

            AvailabilityStatisticsSelectionSortExpression = null;
            AvailabilityStatisticsSelectionSortDirection = null;
            AvailabilityStatisticsSelectionSortOrder = null;
            AvailabilityStatisticsSelectionPageSize = null;
            AvailabilityStatisticsSelectionCurrentPageNumber = null;
            AvailabilityStatisticsDateSortExpression = null;
            AvailabilityStatisticsDateSortDirection = null;
            AvailabilityStatisticsDateSortOrder = null;
            AvailabilityStatisticsDatePageSize = null;
            AvailabilityStatisticsDateCurrentPageNumber = null;
            AvailabilityStatisticsLogic = null;
            AvailabilityStatisticsCountry = null;
            AvailabilityStatisticsCMSPoolId = null;
            AvailabilityStatisticsCMSLocationGroupCode = null;
            AvailabilityStatisticsOPSRegionId = null;
            AvailabilityStatisticsOPSAreaId = null;
            AvailabilityStatisticsLocation = null;
            AvailabilityStatisticsStartDate = null;
            AvailabilityStatisticsEndDate = null;
            AvailabilityStatisticsRacfID = null;



        }

        public static void ClearAvailabilityStatisticsSelectionGridviewSessions()
        {
            AvailabilityStatisticsSelectionSortExpression = null;
            AvailabilityStatisticsSelectionSortDirection = null;
        }

        public static void ClearAvailabilityStatisticsDateGridviewSessions()
        {
            AvailabilityStatisticsDateSortExpression = null;
            AvailabilityStatisticsDateSortDirection = null;
        }


        #endregion

        #region "KPI Download"


        private static string _AvailabilityKPIDownloadFileName = "SV-AvailabilityKPIDownload-FileName";
        public static string AvailabilityKPIDownloadFileName
        {
            get
            {
                if ((HttpContext.Current.Session[_AvailabilityKPIDownloadFileName] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_AvailabilityKPIDownloadFileName]));
                }
            }
            set { HttpContext.Current.Session[_AvailabilityKPIDownloadFileName] = value; }
        }
        #endregion

        #endregion

        #region "Administration"

        #region "Users"
        private static string _MaintanenceUsersSortExpression = "SV-MaintanenceUsers-SortExpression";
        private static string _MaintanenceUsersSortDirection = "SV-MaintanenceUsers-SortDirection";
        private static string _MaintanenceUsersSortOrder = "SV-MaintanenceUsers-SortOrder";
        private static string _MaintanenceUsersPageSize = "SV-MaintanenceUsers-PageSize";
        private static string _MaintanenceUsersCurrentPageNumber = "SV-MaintanenceUsers-CurrentPageNumber";
        private static string _MaintanenceUsersDefaultMode = "SV-MaintanenceUsers-DefaultMode";

        private static string _MaintanenceUsersValidationGroup = "SV-MaintanenceUsers-ValidationGroup";

        public static string MaintanenceUsersSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MaintanenceUsersSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MaintanenceUsersSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MaintanenceUsersSortExpression] = value; }
        }

        public static int? MaintanenceUsersSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MaintanenceUsersSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MaintanenceUsersSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MaintanenceUsersSortDirection] = value; }
        }

        public static string MaintanenceUsersSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MaintanenceUsersSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MaintanenceUsersSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MaintanenceUsersSortOrder] = value; }
        }

        public static int? MaintanenceUsersPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MaintanenceUsersPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MaintanenceUsersPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MaintanenceUsersPageSize] = value; }
        }

        public static int? MaintanenceUsersCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MaintanenceUsersCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MaintanenceUsersCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MaintanenceUsersCurrentPageNumber] = value; }
        }

        public static int? MaintanenceUsersDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MaintanenceUsersDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MaintanenceUsersDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MaintanenceUsersDefaultMode] = value; }
        }

        public static string MaintanenceUsersValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MaintanenceUsersValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MaintanenceUsersValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MaintanenceUsersValidationGroup] = value; }
        }

        public static void ClearMaintanenceUsersSessions()
        {

            MaintanenceUsersSortExpression = null;
            MaintanenceUsersSortDirection = null;
            MaintanenceUsersSortOrder = null;
            MaintanenceUsersPageSize = null;
            MaintanenceUsersCurrentPageNumber = null;
            MaintanenceUsersDefaultMode = null;
            MaintanenceUsersValidationGroup = null;


        }

        public static void ClearMaintanenceUsersGridviewSessions()
        {
            MaintanenceUsersSortExpression = null;
            MaintanenceUsersSortDirection = null;
        }
        #endregion

        #region "Mappings"

        #region "General"
        private static string _MappingSelectedTable = "SV-Mapping-SelectedTable";
        private static string _MappingSelectedCountry = "SV-Mapping-SelectedCountry";
        private static string _MappingSelectedCMSPoolId = "SV-Mapping-SelectedCMSPoolId";
        private static string _MappingSelectedCMSLocationGroupCode = "SV-Mapping-SelectedCMSLocationGroupCode";
        private static string _MappingSelectedOPSRegionId = "SV-Mapping-SelectedOPSRegionId";
        private static string _MappingSelectedOPSAreaId = "SV-Mapping-SelectedOPSAreaId";
        private static string _MappingSelectedCarSegmentId = "SV-Mapping-SelectedCarSegmentId";
        private static string _MappingSelectedCarClassId = "SV-Mapping-SelectedCarClassId";

        private static string _MappingSelectedCountryOwner = "SV-Mapping-SelectedCountryOwner";
        private static string _MappingSelectedCountryRent = "SV-Mapping-SelectedCountryRent";
        private static string _MappingSelectedStartDate = "SV-Mapping-SelectedStartDate";
        private static string _MappingSelectedModelDescription = "SV-Mapping-SelectedModelDescription";

        public static int? MappingSelectedTable
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedTable] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingSelectedTable]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedTable] = value; }
        }

        public static string MappingSelectedCountry
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedCountry] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingSelectedCountry]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedCountry] = value; }
        }

        public static int? MappingSelectedCMSPoolId
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedCMSPoolId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingSelectedCMSPoolId]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedCMSPoolId] = value; }
        }

        public static int? MappingSelectedCMSLocationGroupCode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedCMSLocationGroupCode] == null))
                {
                    return null;
                }
                else
                {
                    return int.Parse(HttpContext.Current.Session[_MappingSelectedCMSLocationGroupCode].ToString());
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedCMSLocationGroupCode] = value; }
        }

        public static int? MappingSelectedOPSRegionId
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedOPSRegionId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingSelectedOPSRegionId]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedOPSRegionId] = value; }
        }

        public static int? MappingSelectedOPSAreaId
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedOPSAreaId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingSelectedOPSAreaId]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedOPSAreaId] = value; }
        }

        public static int? MappingSelectedCarSegmentId
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedCarSegmentId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingSelectedCarSegmentId]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedCarSegmentId] = value; }
        }

        public static int? MappingSelectedCarClassId
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedCarClassId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingSelectedCarClassId]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedCarClassId] = value; }
        }

        public static void ClearMappingSelectionSessions()
        {

            MappingSelectedTable = -1;
            MappingSelectedCountry = "-1";
            MappingSelectedCMSPoolId = null;
            MappingSelectedCMSLocationGroupCode = null;
            MappingSelectedOPSRegionId = null;
            MappingSelectedOPSAreaId = null;
            MappingSelectedCarSegmentId = null;
            MappingSelectedCarClassId = null;


        }

        public static string MappingSelectedCountryOwner
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedCountryOwner] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingSelectedCountryOwner]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedCountryOwner] = value; }
        }

        public static string MappingSelectedCountryRent
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedCountryRent] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingSelectedCountryRent]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedCountryRent] = value; }
        }

        public static string MappingSelectedStartDate
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedStartDate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingSelectedStartDate]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedStartDate] = value; }
        }

        public static string MappingSelectedModelDescription
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingSelectedModelDescription] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingSelectedModelDescription]));
                }
            }
            set { HttpContext.Current.Session[_MappingSelectedModelDescription] = value; }
        }

        #endregion

        #region "Countries"

        private static string _MappingCountrySortExpression = "SV-MappingCountry-SortExpression";
        private static string _MappingCountrySortDirection = "SV-MappingCountry-SortDirection";
        private static string _MappingCountrySortOrder = "SV-MappingCountry-SortOrder";
        private static string _MappingCountryPageSize = "SV-MappingCountry-PageSize";
        private static string _MappingCountryCurrentPageNumber = "SV-MappingCountry-CurrentPageNumber";
        private static string _MappingCountryDefaultMode = "SV-MappingCountry-DefaultMode";

        private static string _MappingCountryValidationGroup = "SV-MappingCountry-ValidationGroup";

        public static string MappingCountrySortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCountrySortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCountrySortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingCountrySortExpression] = value; }
        }

        public static int? MappingCountrySortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCountrySortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCountrySortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingCountrySortDirection] = value; }
        }

        public static string MappingCountrySortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCountrySortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCountrySortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingCountrySortOrder] = value; }
        }

        public static int? MappingCountryPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCountryPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCountryPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingCountryPageSize] = value; }
        }

        public static int? MappingCountryCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCountryCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCountryCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingCountryCurrentPageNumber] = value; }
        }

        public static int? MappingCountryDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCountryDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCountryDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingCountryDefaultMode] = value; }
        }

        public static string MappingCountryValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCountryValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCountryValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingCountryValidationGroup] = value; }
        }

        public static void ClearMappingCountrySessions()
        {

            MappingCountrySortExpression = null;
            MappingCountrySortDirection = null;
            MappingCountrySortOrder = null;
            MappingCountryPageSize = null;
            MappingCountryCurrentPageNumber = null;
            MappingCountryDefaultMode = null;
            MappingCountryValidationGroup = null;

        }

        public static void ClearMappingCountryGridviewSessions()
        {
            MappingCountrySortExpression = null;
            MappingCountrySortDirection = null;
        }

        #endregion

        #region "Area Codes"
        private static string _MappingAreaCodeSortExpression = "SV-MappingAreaCode-SortExpression";
        private static string _MappingAreaCodeSortDirection = "SV-MappingAreaCode-SortDirection";
        private static string _MappingAreaCodeSortOrder = "SV-MappingAreaCode-SortOrder";
        private static string _MappingAreaCodePageSize = "SV-MappingAreaCode-PageSize";
        private static string _MappingAreaCodeCurrentPageNumber = "SV-MappingAreaCode-CurrentPageNumber";
        private static string _MappingAreaCodeDefaultMode = "SV-MappingAreaCode-DefaultMode";

        private static string _MappingAreaCodeValidationGroup = "SV-MappingAreaCode-ValidationGroup";

        public static string MappingAreaCodeSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingAreaCodeSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingAreaCodeSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingAreaCodeSortExpression] = value; }
        }

        public static int? MappingAreaCodeSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingAreaCodeSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingAreaCodeSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingAreaCodeSortDirection] = value; }
        }

        public static string MappingAreaCodeSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingAreaCodeSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingAreaCodeSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingAreaCodeSortOrder] = value; }
        }

        public static int? MappingAreaCodePageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingAreaCodePageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingAreaCodePageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingAreaCodePageSize] = value; }
        }

        public static int? MappingAreaCodeCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingAreaCodeCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingAreaCodeCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingAreaCodeCurrentPageNumber] = value; }
        }

        public static int? MappingAreaCodeDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingAreaCodeDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingAreaCodeDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingAreaCodeDefaultMode] = value; }
        }

        public static string MappingAreaCodeValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingAreaCodeValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingAreaCodeValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingAreaCodeValidationGroup] = value; }
        }

        public static void ClearMappingAreaCodeSessions()
        {

            MappingAreaCodeSortExpression = null;
            MappingAreaCodeSortDirection = null;
            MappingAreaCodeSortOrder = null;
            MappingAreaCodePageSize = null;
            MappingAreaCodeCurrentPageNumber = null;
            MappingAreaCodeDefaultMode = null;
            MappingAreaCodeValidationGroup = null;


        }

        public static void ClearMappingAreaCodeGridviewSessions()
        {
            MappingAreaCodeSortExpression = null;
            MappingAreaCodeSortDirection = null;
        }
        #endregion

        #region "CMS Pools"
        private static string _MappingCMSPoolSortExpression = "SV-MappingCMSPool-SortExpression";
        private static string _MappingCMSPoolSortDirection = "SV-MappingCMSPool-SortDirection";
        private static string _MappingCMSPoolSortOrder = "SV-MappingCMSPool-SortOrder";
        private static string _MappingCMSPoolPageSize = "SV-MappingCMSPool-PageSize";
        private static string _MappingCMSPoolCurrentPageNumber = "SV-MappingCMSPool-CurrentPageNumber";
        private static string _MappingCMSPoolDefaultMode = "SV-MappingCMSPool-DefaultMode";

        private static string _MappingCMSPoolValidationGroup = "SV-MappingCMSPool-ValidationGroup";

        public static string MappingCMSPoolSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSPoolSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCMSPoolSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSPoolSortExpression] = value; }
        }

        public static int? MappingCMSPoolSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSPoolSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCMSPoolSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSPoolSortDirection] = value; }
        }

        public static string MappingCMSPoolSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSPoolSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCMSPoolSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSPoolSortOrder] = value; }
        }

        public static int? MappingCMSPoolPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSPoolPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCMSPoolPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSPoolPageSize] = value; }
        }

        public static int? MappingCMSPoolCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSPoolCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCMSPoolCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSPoolCurrentPageNumber] = value; }
        }

        public static int? MappingCMSPoolDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSPoolDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCMSPoolDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSPoolDefaultMode] = value; }
        }

        public static string MappingCMSPoolValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSPoolValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCMSPoolValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSPoolValidationGroup] = value; }
        }

        public static void ClearMappingCMSPoolSessions()
        {

            MappingCMSPoolSortExpression = null;
            MappingCMSPoolSortDirection = null;
            MappingCMSPoolSortOrder = null;
            MappingCMSPoolPageSize = null;
            MappingCMSPoolCurrentPageNumber = null;
            MappingCMSPoolDefaultMode = null;
            MappingCMSPoolValidationGroup = null;

        }

        public static void ClearMappingCMSPoolGridviewSessions()
        {
            MappingCMSPoolSortExpression = null;
            MappingCMSPoolSortDirection = null;
        }
        #endregion

        #region "CMS Locations"
        private static string _MappingCMSLocationSortExpression = "SV-MappingCMSLocation-SortExpression";
        private static string _MappingCMSLocationSortDirection = "SV-MappingCMSLocation-SortDirection";
        private static string _MappingCMSLocationSortOrder = "SV-MappingCMSLocation-SortOrder";
        private static string _MappingCMSLocationPageSize = "SV-MappingCMSLocation-PageSize";
        private static string _MappingCMSLocationCurrentPageNumber = "SV-MappingCMSLocation-CurrentPageNumber";
        private static string _MappingCMSLocationDefaultMode = "SV-MappingCMSLocation-DefaultMode";

        private static string _MappingCMSLocationValidationGroup = "SV-MappingCMSLocation-ValidationGroup";

        public static string MappingCMSLocationSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSLocationSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCMSLocationSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSLocationSortExpression] = value; }
        }

        public static int? MappingCMSLocationSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSLocationSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCMSLocationSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSLocationSortDirection] = value; }
        }

        public static string MappingCMSLocationSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSLocationSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCMSLocationSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSLocationSortOrder] = value; }
        }

        public static int? MappingCMSLocationPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSLocationPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCMSLocationPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSLocationPageSize] = value; }
        }

        public static int? MappingCMSLocationCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSLocationCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCMSLocationCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSLocationCurrentPageNumber] = value; }
        }

        public static int? MappingCMSLocationDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSLocationDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCMSLocationDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSLocationDefaultMode] = value; }
        }

        public static string MappingCMSLocationValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCMSLocationValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCMSLocationValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingCMSLocationValidationGroup] = value; }
        }

        public static void ClearMappingCMSLocationSessions()
        {

            MappingCMSLocationSortExpression = null;
            MappingCMSLocationSortDirection = null;
            MappingCMSLocationSortOrder = null;
            MappingCMSLocationPageSize = null;
            MappingCMSLocationCurrentPageNumber = null;
            MappingCMSLocationDefaultMode = null;
            MappingCMSLocationValidationGroup = null;

        }

        public static void ClearMappingCMSLocationGridviewSessions()
        {
            MappingCMSLocationSortExpression = null;
            MappingCMSLocationSortDirection = null;
        }
        #endregion

        #region "OPS Regions"
        private static string _MappingOPSRegionSortExpression = "SV-MappingOPSRegion-SortExpression";
        private static string _MappingOPSRegionSortDirection = "SV-MappingOPSRegion-SortDirection";
        private static string _MappingOPSRegionSortOrder = "SV-MappingOPSRegion-SortOrder";
        private static string _MappingOPSRegionPageSize = "SV-MappingOPSRegion-PageSize";
        private static string _MappingOPSRegionCurrentPageNumber = "SV-MappingOPSRegion-CurrentPageNumber";
        private static string _MappingOPSRegionDefaultMode = "SV-MappingOPSRegion-DefaultMode";

        private static string _MappingOPSRegionValidationGroup = "SV-MappingOPSRegion-ValidationGroup";

        public static string MappingOPSRegionSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSRegionSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingOPSRegionSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSRegionSortExpression] = value; }
        }

        public static int? MappingOPSRegionSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSRegionSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingOPSRegionSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSRegionSortDirection] = value; }
        }

        public static string MappingOPSRegionSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSRegionSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingOPSRegionSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSRegionSortOrder] = value; }
        }

        public static int? MappingOPSRegionPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSRegionPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingOPSRegionPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSRegionPageSize] = value; }
        }

        public static int? MappingOPSRegionCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSRegionCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingOPSRegionCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSRegionCurrentPageNumber] = value; }
        }

        public static int? MappingOPSRegionDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSRegionDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingOPSRegionDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSRegionDefaultMode] = value; }
        }

        public static string MappingOPSRegionValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSRegionValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingOPSRegionValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSRegionValidationGroup] = value; }
        }

        public static void ClearMappingOPSRegionSessions()
        {

            MappingOPSRegionSortExpression = null;
            MappingOPSRegionSortDirection = null;
            MappingOPSRegionSortOrder = null;
            MappingOPSRegionPageSize = null;
            MappingOPSRegionCurrentPageNumber = null;
            MappingOPSRegionDefaultMode = null;
            MappingOPSRegionValidationGroup = null;

        }

        public static void ClearMappingOPSRegionGridviewSessions()
        {
            MappingOPSRegionSortExpression = null;
            MappingOPSRegionSortDirection = null;
        }
        #endregion

        #region "OPS Areas"
        private static string _MappingOPSAreaSortExpression = "SV-MappingOPSArea-SortExpression";
        private static string _MappingOPSAreaSortDirection = "SV-MappingOPSArea-SortDirection";
        private static string _MappingOPSAreaSortOrder = "SV-MappingOPSArea-SortOrder";
        private static string _MappingOPSAreaPageSize = "SV-MappingOPSArea-PageSize";
        private static string _MappingOPSAreaCurrentPageNumber = "SV-MappingOPSArea-CurrentPageNumber";
        private static string _MappingOPSAreaDefaultMode = "SV-MappingOPSArea-DefaultMode";

        private static string _MappingOPSAreaValidationGroup = "SV-MappingOPSArea-ValidationGroup";

        public static string MappingOPSAreaSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSAreaSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingOPSAreaSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSAreaSortExpression] = value; }
        }

        public static int? MappingOPSAreaSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSAreaSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingOPSAreaSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSAreaSortDirection] = value; }
        }

        public static string MappingOPSAreaSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSAreaSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingOPSAreaSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSAreaSortOrder] = value; }
        }

        public static int? MappingOPSAreaPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSAreaPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingOPSAreaPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSAreaPageSize] = value; }
        }

        public static int? MappingOPSAreaCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSAreaCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingOPSAreaCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSAreaCurrentPageNumber] = value; }
        }

        public static int? MappingOPSAreaDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSAreaDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingOPSAreaDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSAreaDefaultMode] = value; }
        }

        public static string MappingOPSAreaValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingOPSAreaValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingOPSAreaValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingOPSAreaValidationGroup] = value; }
        }

        public static void ClearMappingOPSAreaSessions()
        {

            MappingOPSAreaSortExpression = null;
            MappingOPSAreaSortDirection = null;
            MappingOPSAreaSortOrder = null;
            MappingOPSAreaPageSize = null;
            MappingOPSAreaCurrentPageNumber = null;
            MappingOPSAreaDefaultMode = null;
            MappingOPSAreaValidationGroup = null;


        }

        public static void ClearMappingOPSAreaGridviewSessions()
        {
            MappingOPSAreaSortExpression = null;
            MappingOPSAreaSortDirection = null;
        }
        #endregion

        #region "Locations"
        private static string _MappingLocationSortExpression = "SV-MappingLocation-SortExpression";
        private static string _MappingLocationSortDirection = "SV-MappingLocation-SortDirection";
        private static string _MappingLocationSortOrder = "SV-MappingLocation-SortOrder";
        private static string _MappingLocationPageSize = "SV-MappingLocation-PageSize";
        private static string _MappingLocationCurrentPageNumber = "SV-MappingLocation-CurrentPageNumber";
        private static string _MappingLocationDefaultMode = "SV-MappingLocation-DefaultMode";

        private static string _MappingLocationValidationGroup = "SV-MappingLocation-ValidationGroup";

        public static string MappingLocationSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingLocationSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingLocationSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingLocationSortExpression] = value; }
        }

        public static int? MappingLocationSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingLocationSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingLocationSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingLocationSortDirection] = value; }
        }

        public static string MappingLocationSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingLocationSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingLocationSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingLocationSortOrder] = value; }
        }

        public static int? MappingLocationPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingLocationPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingLocationPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingLocationPageSize] = value; }
        }

        public static int? MappingLocationCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingLocationCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingLocationCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingLocationCurrentPageNumber] = value; }
        }

        public static int? MappingLocationDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingLocationDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingLocationDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingLocationDefaultMode] = value; }
        }

        public static string MappingLocationValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingLocationValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingLocationValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingLocationValidationGroup] = value; }
        }

        public static void ClearMappingLocationSessions()
        {

            MappingLocationSortExpression = null;
            MappingLocationSortDirection = null;
            MappingLocationSortOrder = null;
            MappingLocationPageSize = null;
            MappingLocationCurrentPageNumber = null;
            MappingLocationDefaultMode = null;
            MappingLocationValidationGroup = null;


        }

        public static void ClearMappingLocationGridviewSessions()
        {
            MappingLocationSortExpression = null;
            MappingLocationSortDirection = null;
        }
        #endregion

        #region "Car Segments"
        private static string _MappingCarSegmentSortExpression = "SV-MappingCarSegment-SortExpression";
        private static string _MappingCarSegmentSortDirection = "SV-MappingCarSegment-SortDirection";
        private static string _MappingCarSegmentSortOrder = "SV-MappingCarSegment-SortOrder";
        private static string _MappingCarSegmentPageSize = "SV-MappingCarSegment-PageSize";
        private static string _MappingCarSegmentCurrentPageNumber = "SV-MappingCarSegment-CurrentPageNumber";
        private static string _MappingCarSegmentDefaultMode = "SV-MappingCarSegment-DefaultMode";

        private static string _MappingCarSegmentValidationGroup = "SV-MappingCarSegment-ValidationGroup";

        public static string MappingCarSegmentSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarSegmentSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCarSegmentSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarSegmentSortExpression] = value; }
        }

        public static int? MappingCarSegmentSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarSegmentSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarSegmentSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarSegmentSortDirection] = value; }
        }

        public static string MappingCarSegmentSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarSegmentSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCarSegmentSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarSegmentSortOrder] = value; }
        }

        public static int? MappingCarSegmentPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarSegmentPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarSegmentPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarSegmentPageSize] = value; }
        }

        public static int? MappingCarSegmentCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarSegmentCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarSegmentCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarSegmentCurrentPageNumber] = value; }
        }

        public static int? MappingCarSegmentDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarSegmentDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarSegmentDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarSegmentDefaultMode] = value; }
        }

        public static string MappingCarSegmentValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarSegmentValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCarSegmentValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarSegmentValidationGroup] = value; }
        }

        public static void ClearMappingCarSegmentSessions()
        {

            MappingCarSegmentSortExpression = null;
            MappingCarSegmentSortDirection = null;
            MappingCarSegmentSortOrder = null;
            MappingCarSegmentPageSize = null;
            MappingCarSegmentCurrentPageNumber = null;
            MappingCarSegmentDefaultMode = null;
            MappingCarSegmentValidationGroup = null;


        }

        public static void ClearMappingCarSegmentGridviewSessions()
        {
            MappingCarSegmentSortExpression = null;
            MappingCarSegmentSortDirection = null;
        }
        #endregion

        #region "Car Classes"
        private static string _MappingCarClassSortExpression = "SV-MappingCarClass-SortExpression";
        private static string _MappingCarClassSortDirection = "SV-MappingCarClass-SortDirection";
        private static string _MappingCarClassSortOrder = "SV-MappingCarClass-SortOrder";
        private static string _MappingCarClassPageSize = "SV-MappingCarClass-PageSize";
        private static string _MappingCarClassCurrentPageNumber = "SV-MappingCarClass-CurrentPageNumber";
        private static string _MappingCarClassDefaultMode = "SV-MappingCarClass-DefaultMode";

        private static string _MappingCarClassValidationGroup = "SV-MappingCarClass-ValidationGroup";

        public static string MappingCarClassSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarClassSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCarClassSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarClassSortExpression] = value; }
        }

        public static int? MappingCarClassSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarClassSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarClassSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarClassSortDirection] = value; }
        }

        public static string MappingCarClassSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarClassSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCarClassSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarClassSortOrder] = value; }
        }

        public static int? MappingCarClassPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarClassPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarClassPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarClassPageSize] = value; }
        }

        public static int? MappingCarClassCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarClassCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarClassCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarClassCurrentPageNumber] = value; }
        }

        public static int? MappingCarClassDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarClassDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarClassDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarClassDefaultMode] = value; }
        }

        public static string MappingCarClassValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarClassValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCarClassValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarClassValidationGroup] = value; }
        }

        public static void ClearMappingCarClassSessions()
        {

            MappingCarClassSortExpression = null;
            MappingCarClassSortDirection = null;
            MappingCarClassSortOrder = null;
            MappingCarClassPageSize = null;
            MappingCarClassCurrentPageNumber = null;
            MappingCarClassDefaultMode = null;
            MappingCarClassValidationGroup = null;


        }

        public static void ClearMappingCarClassGridviewSessions()
        {
            MappingCarClassSortExpression = null;
            MappingCarClassSortDirection = null;
        }
        #endregion

        #region "Car Groups"
        private static string _MappingCarGroupSortExpression = "SV-MappingCarGroup-SortExpression";
        private static string _MappingCarGroupSortDirection = "SV-MappingCarGroup-SortDirection";
        private static string _MappingCarGroupSortOrder = "SV-MappingCarGroup-SortOrder";
        private static string _MappingCarGroupPageSize = "SV-MappingCarGroup-PageSize";
        private static string _MappingCarGroupCurrentPageNumber = "SV-MappingCarGroup-CurrentPageNumber";
        private static string _MappingCarGroupDefaultMode = "SV-MappingCarGroup-DefaultMode";

        private static string _MappingCarGroupValidationGroup = "SV-MappingCarGroup-ValidationGroup";

        public static string MappingCarGroupSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarGroupSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCarGroupSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarGroupSortExpression] = value; }
        }

        public static int? MappingCarGroupSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarGroupSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarGroupSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarGroupSortDirection] = value; }
        }

        public static string MappingCarGroupSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarGroupSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCarGroupSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarGroupSortOrder] = value; }
        }

        public static int? MappingCarGroupPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarGroupPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarGroupPageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarGroupPageSize] = value; }
        }

        public static int? MappingCarGroupCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarGroupCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarGroupCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarGroupCurrentPageNumber] = value; }
        }

        public static int? MappingCarGroupDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarGroupDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingCarGroupDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarGroupDefaultMode] = value; }
        }

        public static string MappingCarGroupValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingCarGroupValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingCarGroupValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingCarGroupValidationGroup] = value; }
        }

        public static void ClearMappingCarGroupSessions()
        {

            MappingCarGroupSortExpression = null;
            MappingCarGroupSortDirection = null;
            MappingCarGroupSortOrder = null;
            MappingCarGroupPageSize = null;
            MappingCarGroupCurrentPageNumber = null;
            MappingCarGroupDefaultMode = null;
            MappingCarGroupValidationGroup = null;


        }

        public static void ClearMappingCarGroupGridviewSessions()
        {
            MappingCarGroupSortExpression = null;
            MappingCarGroupSortDirection = null;
        }
        #endregion

        #region "Model Codes"
        private static string _MappingModelCodeSortExpression = "SV-MappingModelCode-SortExpression";
        private static string _MappingModelCodeSortDirection = "SV-MappingModelCode-SortDirection";
        private static string _MappingModelCodeSortOrder = "SV-MappingModelCode-SortOrder";
        private static string _MappingModelCodePageSize = "SV-MappingModelCode-PageSize";
        private static string _MappingModelCodeCurrentPageNumber = "SV-MappingModelCode-CurrentPageNumber";
        private static string _MappingModelCodeDefaultMode = "SV-MappingModelCode-DefaultMode";

        private static string _MappingModelCodeValidationGroup = "SV-MappingModelCode-ValidationGroup";

        public static string MappingModelCodeSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingModelCodeSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingModelCodeSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingModelCodeSortExpression] = value; }
        }

        public static int? MappingModelCodeSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingModelCodeSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingModelCodeSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingModelCodeSortDirection] = value; }
        }

        public static string MappingModelCodeSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingModelCodeSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingModelCodeSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingModelCodeSortOrder] = value; }
        }

        public static int? MappingModelCodePageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingModelCodePageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingModelCodePageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingModelCodePageSize] = value; }
        }

        public static int? MappingModelCodeCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingModelCodeCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingModelCodeCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingModelCodeCurrentPageNumber] = value; }
        }

        public static int? MappingModelCodeDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingModelCodeDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingModelCodeDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingModelCodeDefaultMode] = value; }
        }

        public static string MappingModelCodeValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingModelCodeValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingModelCodeValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingModelCodeValidationGroup] = value; }
        }

        public static void ClearMappingModelCodeSessions()
        {

            MappingModelCodeSortExpression = null;
            MappingModelCodeSortDirection = null;
            MappingModelCodeSortOrder = null;
            MappingModelCodePageSize = null;
            MappingModelCodeCurrentPageNumber = null;
            MappingModelCodeDefaultMode = null;
            MappingModelCodeValidationGroup = null;

        }

        public static void ClearMappingModelCodeGridviewSessions()
        {
            MappingModelCodeSortExpression = null;
            MappingModelCodeSortDirection = null;
        }
        #endregion

        #region "Vehicles Lease"

        private static string _MappingVehiclesLeaseSortExpression = "SV-MappingVehiclesLease-SortExpression";
        private static string _MappingVehiclesLeaseSortDirection = "SV-MappingVehiclesLease-SortDirection";
        private static string _MappingVehiclesLeaseSortOrder = "SV-MappingVehiclesLease-SortOrder";
        private static string _MappingVehiclesLeasePageSize = "SV-MappingVehiclesLease-PageSize";
        private static string _MappingVehiclesLeaseCurrentPageNumber = "SV-MappingVehiclesLease-CurrentPageNumber";
        private static string _MappingVehiclesLeaseDefaultMode = "SV-MappingVehiclesLease-DefaultMode";
        private static string _MappingVehiclesLeaseValidationGroup = "SV-MappingVehiclesLease-ValidationGroup";

        public static string MappingVehiclesLeaseSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingVehiclesLeaseSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingVehiclesLeaseSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_MappingVehiclesLeaseSortExpression] = value; }
        }

        public static int? MappingVehiclesLeaseSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingVehiclesLeaseSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingVehiclesLeaseSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_MappingVehiclesLeaseSortDirection] = value; }
        }

        public static string MappingVehiclesLeaseSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingVehiclesLeaseSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingVehiclesLeaseSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_MappingVehiclesLeaseSortOrder] = value; }
        }

        public static int? MappingVehiclesLeasePageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingVehiclesLeasePageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingVehiclesLeasePageSize]));
                }
            }
            set { HttpContext.Current.Session[_MappingVehiclesLeasePageSize] = value; }
        }

        public static int? MappingVehiclesLeaseCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingVehiclesLeaseCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingVehiclesLeaseCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_MappingVehiclesLeaseCurrentPageNumber] = value; }
        }

        public static int? MappingVehiclesLeaseDefaultMode
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingVehiclesLeaseDefaultMode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_MappingVehiclesLeaseDefaultMode]));
                }
            }
            set { HttpContext.Current.Session[_MappingVehiclesLeaseDefaultMode] = value; }
        }

        public static string MappingVehiclesLeaseValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_MappingVehiclesLeaseValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_MappingVehiclesLeaseValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_MappingVehiclesLeaseValidationGroup] = value; }
        }

        public static void ClearMappingVehiclesLeaseSessions()
        {

            MappingVehiclesLeaseSortExpression = null;
            MappingVehiclesLeaseSortDirection = null;
            MappingVehiclesLeaseSortOrder = null;
            MappingVehiclesLeasePageSize = null;
            MappingVehiclesLeaseCurrentPageNumber = null;
            MappingVehiclesLeaseDefaultMode = null;
            MappingVehiclesLeaseValidationGroup = null;

        }

        public static void ClearMappingVehiclesLeaseGridviewSessions()
        {
            MappingVehiclesLeaseSortExpression = null;
            MappingVehiclesLeaseSortDirection = null;
        }

        #endregion

        #endregion

        #endregion

        #region "Non Revenue"

        #region "Non Rev General Form Values"

        private static string _NonRev_FormValue_Country = "SV-NonRev-Formvalue-Country";

        public static string NonRevFormValueCountry
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_Country] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_Country]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_Country] = value; }
        }

        private static string _NonRev_FormValue_SelectedLogic = "SV-NonRev-Formvalue-SelectedLogic";

        public static int? NonRevFormValueSelectedLogic
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_SelectedLogic] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRev_FormValue_SelectedLogic]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_SelectedLogic] = value; }
        }

        private static string _NonRev_FormValue_PoolId = "SV-NonRev-Formvalue-PoolId";

        public static int? NonRevFormValuePoolId
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_PoolId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRev_FormValue_PoolId]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_PoolId] = value; }
        }

        private static string _NonRev_FormValue_LocationGroupId = "SV-NonRev-Formvalue-LocationGroupId";

        public static int? NonRevFormValueLocationGroupId
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_LocationGroupId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRev_FormValue_LocationGroupId]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_LocationGroupId] = value; }
        }

        private static string _NonRev_FormValue_RegionId = "SV-NonRev-Formvalue-RegionId";

        public static int? NonRevFormValueRegionId
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_RegionId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRev_FormValue_RegionId]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_RegionId] = value; }
        }

        private static string _NonRev_FormValue_AreaId = "SV-NonRev-Formvalue-AreaId";

        public static int? NonRevFormValueAreaId
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_AreaId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRev_FormValue_AreaId]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_AreaId] = value; }
        }

        private static string _NonRev_FormValue_Location = "SV-NonRev-FormValue-Location";

        public static string NonRevFormValueLocation
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_Location] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_Location]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_Location] = value; }
        }

        private static string _NonRev_FormValue_CarSegmentId = "SV-NonRev-FormValue-CarSegmentId";

        public static int? NonRevFormValueCarSegmentId
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_CarSegmentId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRev_FormValue_CarSegmentId]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_CarSegmentId] = value; }
        }

        private static string _NonRev_FormValue_CarClassId = "SV-NonRev-FormValue-CarClassId";

        public static int? NonRevFormValueCarClassId
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_CarClassId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRev_FormValue_CarClassId]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_CarClassId] = value; }
        }

        private static string _NonRev_FormValue_CarGroupId = "SV-NonRev-FormValue-CarGroupId";

        public static int? NonRevFormValueCarGroupId
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_CarGroupId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRev_FormValue_CarGroupId]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_CarGroupId] = value; }
        }

        private static string _NonRev_FormValue_FleetName = "SV-NonRev-FormValue-FleetName";

        public static string NonRevFormValueFleetName
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_FleetName] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_FleetName]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_FleetName] = value; }
        }

        private static string _NonRev_FormValue_OperStat = "SV-NonRev-FormValue-OperStat";

        public static string NonRevFormValueOperStat
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_OperStat] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_OperStat]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_OperStat] = value; }
        }

        private static string _NonRev_FormValue_DayGroupCode = "SV-NonRev-FormValue-DayGroupCode";

        public static string NonRevFormValueDayGroupCode
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_DayGroupCode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_DayGroupCode]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_DayGroupCode] = value; }
        }

        private static string _NonRev_FormValue_CustomEndDate = "SV-NonRev-FormValue-CustomEndDate";

        public static string NonRevFormValueCustomEndDate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_CustomEndDate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_CustomEndDate]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_CustomEndDate] = value; }
        }

        private static string _NonRev_FormValue_CustomStartDate = "SV-NonRev-FormValue-CustomStartDate";

        public static string NonRevFormValueCustomStartDate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_CustomStartDate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_CustomStartDate]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_CustomStartDate] = value; }
        }

        private static string _NonRev_FormValue_DateComparison = "SV-NonRev-FormValue-DateComparison";

        public static string NonRevFormValueDateComparison
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_DateComparison] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_DateComparison]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_DateComparison] = value; }
        }

        private static string _NonRev_FormValue_DateRange = "SV-NonRev-FormValue-DateRange";

        public static string NonRevFormValueDateRange
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_DateRange] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_DateRange]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_DateRange] = value; }
        }

        private static string _NonRev_FormValue_DateRangeValue = "SV-NonRev-FormValue-DateRangeValue";

        public static string NonRevFormValueDateRangeValue
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRev_FormValue_DateRangeValue] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRev_FormValue_DateRangeValue]));
                }
            }
            set { HttpContext.Current.Session[_NonRev_FormValue_DateRangeValue] = value; }
        }

        #endregion

        #region "Grid View Sessions"

        #region "Overview"
        
        private static string _NonRevCarSearchSortExpression = "SV-NonRevCarSearch-SortExpression";
        private static string _NonRevCarSearchSortDirection = "SV-NonRevCarSearch-SortDirection";
        private static string _NonRevCarSearchSortOrder = "SV-NonRevCarSearch-SortOrder";
        private static string _NonRevCarSearchPageSize = "SV-NonRevCarSearch-PageSize";
        private static string _NonRevCarSearchCurrentPageNumber = "SV-NonRevCarSearch-CurrentPageNumber";
        private static string _NonRevCarSearchRowCount = "SV-NonRevCarSearch-RowCount";

        public static string NonRevCarSearchSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchSortExpression] = value; }
        }

        public static int? NonRevCarSearchSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevCarSearchSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchSortDirection] = value; }
        }

        public static string NonRevCarSearchSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchSortOrder] = value; }
        }

        public static int? NonRevCarSearchPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevCarSearchPageSize]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchPageSize] = value; }
        }

        public static int? NonRevCarSearchCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevCarSearchCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchCurrentPageNumber] = value; }
        }

        public static int? NonRevCarSearchRowCount
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchRowCount] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevCarSearchRowCount]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchRowCount] = value; }
        }

        #endregion

        #region "Historical Trend"
        
        private static string _NonRevHTrendSortExpression = "SV-NonRevHTrend-SortExpression";
        private static string _NonRevHTrendSortDirection = "SV-NonRevHTrend-SortDirection";
        private static string _NonRevHTrendSortOrder = "SV-NonRevHTrend-SortOrder";
        private static string _NonRevHTrendPageSize = "SV-NonRevHTrend-PageSize";
        private static string _NonRevHTrendCurrentPageNumber = "SV-NonRevHTrend-CurrentPageNumber";
        private static string _NonRevHTrendRowCount = "SV-NonRevHTrend-RowCount";

        public static string NonRevHTrendSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevHTrendSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevHTrendSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_NonRevHTrendSortExpression] = value; }
        }

        public static int? NonRevHTrendSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevHTrendSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevHTrendSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_NonRevHTrendSortDirection] = value; }
        }

        public static string NonRevHTrendSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevHTrendSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevHTrendSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_NonRevHTrendSortOrder] = value; }
        }

        public static int? NonRevHTrendPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevHTrendPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevHTrendPageSize]));
                }
            }
            set { HttpContext.Current.Session[_NonRevHTrendPageSize] = value; }
        }

        public static int? NonRevHTrendCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevHTrendCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevHTrendCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_NonRevHTrendCurrentPageNumber] = value; }
        }

        public static int? NonRevHTrendRowCount
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevHTrendRowCount] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevHTrendRowCount]));
                }
            }
            set { HttpContext.Current.Session[_NonRevHTrendRowCount] = value; }
        }

        #endregion

        #region "Fleet - Site Comparison"

        private static string _NonRevComparisonSortExpression = "SV-NonRevComparison-SortExpression";
        private static string _NonRevComparisonSortDirection = "SV-NonRevComparison-SortDirection";
        private static string _NonRevComparisonSortOrder = "SV-NonRevComparison-SortOrder";
        private static string _NonRevComparisonPageSize = "SV-NonRevComparison-PageSize";
        private static string _NonRevComparisonCurrentPageNumber = "SV-NonRevComparison-CurrentPageNumber";
        private static string _NonRevComparisonRowCount = "SV-NonRevComparison-RowCount";

        public static string NonRevComparisonSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevComparisonSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonSortExpression] = value; }
        }

        public static int? NonRevComparisonSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevComparisonSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonSortDirection] = value; }
        }

        public static string NonRevComparisonSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevComparisonSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonSortOrder] = value; }
        }

        public static int? NonRevComparisonPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevComparisonPageSize]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonPageSize] = value; }
        }

        public static int? NonRevComparisonCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevComparisonCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonCurrentPageNumber] = value; }
        }

        public static int? NonRevComparisonRowCount
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonRowCount] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevComparisonRowCount]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonRowCount] = value; }
        }

        #endregion

        #region "Reports"

        private static string _NonRevReportStartSortExpression = "SV-NonRevReportStart-SortExpression";
        private static string _NonRevReportStartSortDirection = "SV-NonRevReportStart-SortDirection";
        private static string _NonRevReportStartSortOrder = "SV-NonRevReportStart-SortOrder";
        private static string _NonRevReportStartPageSize = "SV-NonRevReportStart-PageSize";
        private static string _NonRevReportStartCurrentPageNumber = "SV-NonRevReportStart-CurrentPageNumber";
        private static string _NonRevReportStartRowCount = "SV-NonRevReportStart-RowCount";

        public static string NonRevReportStartSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevReportStartSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevReportStartSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_NonRevReportStartSortExpression] = value; }
        }

        public static int? NonRevReportStartSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevReportStartSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevReportStartSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_NonRevReportStartSortDirection] = value; }
        }

        public static string NonRevReportStartSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevReportStartSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevReportStartSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_NonRevReportStartSortOrder] = value; }
        }

        public static int? NonRevReportStartPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevReportStartPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevReportStartPageSize]));
                }
            }
            set { HttpContext.Current.Session[_NonRevReportStartPageSize] = value; }
        }

        public static int? NonRevReportStartCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevReportStartCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevReportStartCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_NonRevReportStartCurrentPageNumber] = value; }
        }

        public static int? NonRevReportStartRowCount
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevReportStartRowCount] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevReportStartRowCount]));
                }
            }
            set { HttpContext.Current.Session[_NonRevReportStartRowCount] = value; }
        }

        #endregion

        #region "Approval"

        private static string _NonRevApprovalSortExpression = "SV-NonRevApproval-SortExpression";
        private static string _NonRevApprovalSortDirection = "SV-NonRevApproval-SortDirection";
        private static string _NonRevApprovalSortOrder = "SV-NonRevApproval-SortOrder";
        private static string _NonRevApprovalPageSize = "SV-NonRevApproval-PageSize";
        private static string _NonRevApprovalCurrentPageNumber = "SV-NonRevApproval-CurrentPageNumber";
        private static string _NonRevApprovalRowCount = "SV-NonRevApproval-RowCount";

        public static string NonRevApprovalSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalSortExpression] = value; }
        }

        public static int? NonRevApprovalSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalSortDirection] = value; }
        }

        public static string NonRevApprovalSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalSortOrder] = value; }
        }

        public static int? NonRevApprovalPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalPageSize]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalPageSize] = value; }
        }

        public static int? NonRevApprovalCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalCurrentPageNumber] = value; }
        }

        public static int? NonRevApprovalRowCount
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalRowCount] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalRowCount]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalRowCount] = value; }
        }

        private static string _NonRevApprovalUsersSortExpression = "SV-NonRevApprovalUsers-SortExpression";
        private static string _NonRevApprovalUsersSortDirection = "SV-NonRevApprovalUsers-SortDirection";
        private static string _NonRevApprovalUsersSortOrder = "SV-NonRevApprovalUsers-SortOrder";
        private static string _NonRevApprovalUsersPageSize = "SV-NonRevApprovalUsers-PageSize";
        private static string _NonRevApprovalUsersCurrentPageNumber = "SV-NonRevApprovalUsers-CurrentPageNumber";
        private static string _NonRevApprovalUsersRowCount = "SV-NonRevApprovalUsers-RowCount";

        public static string NonRevApprovalUsersSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalUsersSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersSortExpression] = value; }
        }

        public static int? NonRevApprovalUsersSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalUsersSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersSortDirection] = value; }
        }

        public static string NonRevApprovalUsersSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalUsersSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersSortOrder] = value; }
        }

        public static int? NonRevApprovalUsersPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalUsersPageSize]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersPageSize] = value; }
        }

        public static int? NonRevApprovalUsersCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalUsersCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersCurrentPageNumber] = value; }
        }

        public static int? NonRevApprovalUsersRowCount
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersRowCount] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalUsersRowCount]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersRowCount] = value; }
        }

        private static string _NonRevApprovalUsersListSortExpression = "SV-NonRevApprovalUsersList-SortExpression";
        private static string _NonRevApprovalUsersListSortDirection = "SV-NonRevApprovalUsersList-SortDirection";
        private static string _NonRevApprovalUsersListSortOrder = "SV-NonRevApprovalUsersList-SortOrder";
        private static string _NonRevApprovalUsersListPageSize = "SV-NonRevApprovalUsersList-PageSize";
        private static string _NonRevApprovalUsersListCurrentPageNumber = "SV-NonRevApprovalUsersList-CurrentPageNumber";
        private static string _NonRevApprovalUsersListRowCount = "SV-NonRevApprovalUsersList-RowCount";

        public static string NonRevApprovalUsersListSortExpression
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersListSortExpression] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalUsersListSortExpression]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersListSortExpression] = value; }
        }

        public static int? NonRevApprovalUsersListSortDirection
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersListSortDirection] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalUsersListSortDirection]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersListSortDirection] = value; }
        }

        public static string NonRevApprovalUsersListSortOrder
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersListSortOrder] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalUsersListSortOrder]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersListSortOrder] = value; }
        }

        public static int? NonRevApprovalUsersListPageSize
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersListPageSize] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalUsersListPageSize]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersListPageSize] = value; }
        }

        public static int? NonRevApprovalUsersListCurrentPageNumber
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersListCurrentPageNumber] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalUsersListCurrentPageNumber]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersListCurrentPageNumber] = value; }
        }

        public static int? NonRevApprovalUsersListRowCount
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalUsersListRowCount] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalUsersListRowCount]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalUsersListRowCount] = value; }
        }

        #endregion

        #endregion

        #region "Overview"

        private static string _NonRevCarSearchModelCode = "SV-NonRevCarSearch-ModelCode";
        private static string _NonRevCarSearchStatus = "SV-NonRevCarSearch-Status";
        private static string _NonRevCarSearchOwnArea = "SV-NonRevCarSearch-OwnArea";
        private static string _NonRevCarSearchSelectBy = "SV-NonRevCarSearch-SelectBy";
        private static string _NonRevCarSearchSerial = "SV-NonRevCarSearch-Serial";
        private static string _NonRevCarSearchPlate = "SV-NonRevCarSearch-Plate";
        private static string _NonRevCarSearchUnit = "SV-NonRevCarSearch-Unit";
        private static string _NonRevCarSearchDriverName = "SV-NonRevCarSearch-DriverName";
        private static string _NonRevCarSearchColour = "SV-NonRevCarSearch-Colour";
        private static string _NonRevCarSearchModelDesc = "SV-NonRevCarSearch-ModelDesc";
        private static string _NonRevCarSearchMileage = "SV-NonRevCarSearch-Mileage";
        private static string _NonRevCarSearchMovType = "SV-NonRevCarSearch-MovType";
        private static string _NonRevCarSearchDaysNonRev = "SV-NonRevCarSearch-DaysNonRev";
        private static string _NonRevCarSearchNoRev = "SV-NonRevCarSearch-NoRev";
        private static string _NonRevCarSearchHasRemark = "SV-NonRevCarSearch-HasRemark";

        public static int? NonRevCarSearchNoRev
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchNoRev] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevCarSearchNoRev]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchNoRev] = value; }
        }

        public static string NonRevCarSearchModelCode
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchModelCode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchModelCode]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchModelCode] = value; }
        }

        public static string NonRevCarSearchOwnArea
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchOwnArea] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchOwnArea]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchOwnArea] = value; }
        }

        public static string NonRevCarSearchStatus
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchStatus] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchStatus]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchStatus] = value; }
        }

        public static string NonRevCarSearchSelectBy
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchSelectBy] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchSelectBy]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchSelectBy] = value; }
        }

        public static string NonRevCarSearchSerial
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchSerial] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchSerial]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchSerial] = value; }
        }

        public static string NonRevCarSearchPlate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchPlate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchPlate]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchPlate] = value; }
        }

        public static string NonRevCarSearchUnit
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchUnit] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchUnit]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchUnit] = value; }
        }

        public static string NonRevCarSearchDriverName
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchDriverName] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchDriverName]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchDriverName] = value; }
        }

        public static string NonRevCarSearchColour
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchColour] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchColour]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchColour] = value; }
        }

        public static string NonRevCarSearchModelDesc
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchModelDesc] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchModelDesc]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchModelDesc] = value; }
        }

        public static string NonRevCarSearchMileage
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchMileage] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchMileage]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchMileage] = value; }
        }

        public static string NonRevCarSearchMovType
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchMovType] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchMovType]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchMovType] = value; }
        }

        public static string NonRevCarSearchDaysNonRev
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchDaysNonRev] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchDaysNonRev]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchDaysNonRev] = value; }
        }

        public static string NonRevCarSearchHasRemark
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSearchHasRemark] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSearchHasRemark]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSearchHasRemark] = value; }
        }

        #region "Sessions Car Selected"

        public static string _NonRevCarSelectedPlate = @"SV-NonRevCarSelected-Plate";

        public static string NonRevCarSelectedPlate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedPlate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedPlate]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedPlate] = value; }
        }


        // Form Details First Row
        //---------------------------------------------------------------------------------------
        private static string _NonRevCarSelectedCarGroup = @"SV-NonRevCarSelected-CarGroup";
        private static string _NonRevCarSelectedModelCode = @"SV-NonRevCarSelected-ModelCode";
        private static string _NonRevCarSelectedModelDescription = @"SV-NonRevCarSelected-ModelDescription";
        private static string _NonRevCarSelectedUnit = @"SV-NonRevCarSelected-Unit";
        private static string _NonRevCarSelectedSerial = @"SV-NonRevCarSelected-Serial";

        public static string NonRevCarSelectedCarGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedCarGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedCarGroup]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedCarGroup] = value; }
        }

        public static string NonRevCarSelectedModelCode
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedModelCode] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedModelCode]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedModelCode] = value; }
        }

        public static string NonRevCarSelectedModelDescription
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedModelDescription] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedModelDescription]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedModelDescription] = value; }
        }

        public static string NonRevCarSelectedUnit
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedUnit] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedUnit]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedUnit] = value; }
        }

        public static string NonRevCarSelectedSerial
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedSerial] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedSerial]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedSerial] = value; }
        }

        // Form Details Second Row
        //---------------------------------------------------------------------------------------
        private static string _NonRevCarSelectedLstwwd = @"SV-NonRevCarSelected-Lstwwd";
        private static string _NonRevCarSelectedLstDate = @"SV-NonRevCarSelected-LstDate";
        private static string _NonRevCarSelectedDuewwd = @"SV-NonRevCarSelected-Duewwd";
        private static string _NonRevCarSelectedDueDate = @"SV-NonRevCarSelected-DueDate";
        private static string _NonRevCarSelectedNRDays = @"SV-NonRevCarSelected-NRDays";

        public static string NonRevCarSelectedLstwwd
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedLstwwd] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedLstwwd]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedLstwwd] = value; }
        }

        public static string NonRevCarSelectedLstDate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedLstDate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedLstDate]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedLstDate] = value; }
        }

        public static string NonRevCarSelectedDuewwd
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedDuewwd] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedDuewwd]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedDuewwd] = value; }
        }

        public static string NonRevCarSelectedDueDate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedDueDate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedDueDate]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedDueDate] = value; }
        }

        public static string NonRevCarSelectedNRDays
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedNRDays] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedNRDays]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedNRDays] = value; }
        }

        // Form Details Third Row
        //---------------------------------------------------------------------------------------
        private static string _NonRevCarSelectedOperStat = @"SV-NonRevCarSelected-OperStat";
        private static string _NonRevCarSelectedMovType = @"SV-NonRevCarSelected-MovType";
        private static string _NonRevCarSelectedLstMlg = @"SV-NonRevCarSelected-LstMlg";
        private static string _NonRevCarSelectedLstNo = @"SV-NonRevCarSelected-LstNo";
        private static string _NonRevCarSelectedDriverName = @"SV-NonRevCarSelected-DriverName";

        public static string NonRevCarSelectedOperStat
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedOperStat] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedOperStat]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedOperStat] = value; }
        }

        public static string NonRevCarSelectedMovType
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedMovType] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedMovType]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedMovType] = value; }
        }

        public static string NonRevCarSelectedLstMlg
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedLstMlg] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedLstMlg]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedLstMlg] = value; }
        }

        public static string NonRevCarSelectedLstNo
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedLstNo] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedLstNo]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedLstNo] = value; }
        }

        public static string NonRevCarSelectedDriverName
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedDriverName] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedDriverName]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedDriverName] = value; }
        }

        // Form Details Fourth Row
        //---------------------------------------------------------------------------------------

        private static string _NonRevCarSelectedIDate = @"SV-NonRevCarSelected-IDate";
        private static string _NonRevCarSelectedMSODate = @"SV-NonRevCarSelected-MSODate";
        private static string _NonRevCarSelectedSDate = @"SV-NonRevCarSelected-SDate";
        private static string _NonRevCarSelectedDepStat = @"SV-NonRevCarSelected-DepStat";
        private static string _NonRevCarSelectedCarHold = @"SV-NonRevCarSelected-CarHold";

        public static string NonRevCarSelectedIDate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedIDate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedIDate]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedIDate] = value; }
        }

        public static string NonRevCarSelectedMSODate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedMSODate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedMSODate]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedMSODate] = value; }
        }

        public static string NonRevCarSelectedSDate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedSDate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedSDate]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedSDate] = value; }
        }

        public static string NonRevCarSelectedDepStat
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedDepStat] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedDepStat]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedDepStat] = value; }
        }

        public static string NonRevCarSelectedCarHold
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedCarHold] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedCarHold]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedCarHold] = value; }
        }

        // Form Details Fifth Row
        //---------------------------------------------------------------------------------------
        private static string _NonRevCarSelectedOwnArea = @"SV-NonRevCarSelected-OwnArea";
        private static string _NonRevCarSelectedPrewwd = @"SV-NonRevCarSelected-Prewwd";
        private static string _NonRevCarSelectedBDDays = @"SV-NonRevCarSelected-BDDays";
        private static string _NonRevCarSelectedMMDays = @"SV-NonRevCarSelected-MMDays";

        public static string NonRevCarSelectedOwnArea
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedOwnArea] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedOwnArea]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedOwnArea] = value; }
        }

        public static string NonRevCarSelectedPrewwd
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedPrewwd] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedPrewwd]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedPrewwd] = value; }
        }

        public static string NonRevCarSelectedBDDays
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedBDDays] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedBDDays]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedBDDays] = value; }
        }

        public static string NonRevCarSelectedMMDays
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedMMDays] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedMMDays]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedMMDays] = value; }
        }


        // Form Details Last Rows
        //---------------------------------------------------------------------------------------

        private static string _NonRevCarSelectedRemark = @"SV-NonRevCarSelected-Remark";
        private static string _NonRevCarSelectedRemarkText = @"SV-NonRevCarSelected-RemarkText";
        private static string _NonRevCarSelectedRemarkId = @"SV-NonRevCarSelected-RemarkId";
        private static string _NonRevCarSelectedERDate = @"SV-NonRevCarSelected-ERDate";


        public static string NonRevCarSelectedRemark
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedRemark] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedRemark]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedRemark] = value; }
        }

        public static string NonRevCarSelectedRemarkText
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedRemarkText] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedRemarkText]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedRemarkText] = value; }
        }

        public static string NonRevCarSelectedRemarkId
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedRemarkId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedRemarkId]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedRemarkId] = value; }
        }

        public static string NonRevCarSelectedERDate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevCarSelectedERDate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevCarSelectedERDate]));
                }
            }
            set { HttpContext.Current.Session[_NonRevCarSelectedERDate] = value; }
        }

        #endregion

        #endregion

        #region "Historical Trend"

        private static string _NonRevFleetHTrendDayGroup = "SV-NonRevFleetHTrend-DayGroup";
        private static string _NonRevFleetHTrendDayOfWeek = "SV-NonRevFleetHTrend-DayOfWeek";
        private static string _NonRevFleetHTrendDayOfWeekValue = "SV-NonRevFleetHTrend-DayOfWeekValue";
        private static string _NonRevFleetHTrendIsPercent = "SV-NonRevFleetHTrend-IsPercent";
        private static string _NonRevFleetHTrendReportType = "SV-NonRevFleetHTrend-ReportType";
        private static string _NonRevFleetHTrendLogic = "SV-NonRevFleetHTrend-Logic";

        public static string NonRevFleetHTrendDayGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevFleetHTrendDayGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevFleetHTrendDayGroup]));
                }
            }
            set { HttpContext.Current.Session[_NonRevFleetHTrendDayGroup] = value; }
        }

        public static string NonRevFleetHTrendDayOfWeek
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevFleetHTrendDayOfWeek] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevFleetHTrendDayOfWeek]));
                }
            }
            set { HttpContext.Current.Session[_NonRevFleetHTrendDayOfWeek] = value; }
        }

        public static string NonRevFleetHTrendDayOfWeekValue
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevFleetHTrendDayOfWeekValue] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevFleetHTrendDayOfWeekValue]));
                }
            }
            set { HttpContext.Current.Session[_NonRevFleetHTrendDayOfWeekValue] = value; }
        }

        public static string NonRevFleetHTrendIsPercent
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevFleetHTrendIsPercent] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevFleetHTrendIsPercent]));
                }
            }
            set { HttpContext.Current.Session[_NonRevFleetHTrendIsPercent] = value; }
        }

        public static string NonRevFleetHTrendReportType
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevFleetHTrendReportType] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevFleetHTrendReportType]));
                }
            }
            set { HttpContext.Current.Session[_NonRevFleetHTrendReportType] = value; }
        }

        public static string NonRevFleetHTrendLogic
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevFleetHTrendLogic] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevFleetHTrendLogic]));
                }
            }
            set { HttpContext.Current.Session[_NonRevFleetHTrendLogic] = value; }
        }

        private static string _NonRevFleetHTrendTotalFleetType = "SV-NonRevFleetHTrend-TotalFleetType";

        public static string NonRevFleetHTrendTotalFleetType
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevFleetHTrendTotalFleetType] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevFleetHTrendTotalFleetType]));
                }
            }
            set { HttpContext.Current.Session[_NonRevFleetHTrendTotalFleetType] = value; }
        }

        private static string _NonRevHTrendOption = "SV-NonRevHTrend-Option";

        public static int? NonRevHTrendOption
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevHTrendOption] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevHTrendOption]));
                }
            }
            set { HttpContext.Current.Session[_NonRevHTrendOption] = value; }
        }

        #endregion

        #region "Fleet - Site Comparison"

        private static string _NonRevComparisonOption = "SV-NonRevComparison-Option";

        public static int? NonRevComparisonOption
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonOption] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevComparisonOption]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonOption] = value; }
        }

        private static string _NonRevComparisonDayGroup = "SV-NonRevComparison-DayGroup";
        private static string _NonRevComparisonDayOfWeek = "SV-NonRevComparison-DayOfWeek";
        private static string _NonRevComparisonDayOfWeekValue = "SV-NonRevComparison-DayOfWeekValue";
        private static string _NonRevComparisonIsPercent = "SV-NonRevComparison-IsPercent";
        private static string _NonRevComparisonReportType = "SV-NonRevComparison-ReportType";
        private static string _NonRevComparisonLogic = "SV-NonRevComparison-Logic";
        private static string _NonRevComparisonTotalFleetType = "SV-NonRevFleetComparison-TotalFleetType";

        public static string NonRevComparisonDayGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonDayGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevComparisonDayGroup]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonDayGroup] = value; }
        }

        public static string NonRevComparisonDayOfWeek
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonDayOfWeek] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevComparisonDayOfWeek]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonDayOfWeek] = value; }
        }

        public static string NonRevComparisonDayOfWeekValue
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonDayOfWeekValue] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevComparisonDayOfWeekValue]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonDayOfWeekValue] = value; }
        }

        public static string NonRevComparisonIsPercent
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonIsPercent] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevComparisonIsPercent]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonIsPercent] = value; }
        }

        public static string NonRevComparisonReportType
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonReportType] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevComparisonReportType]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonReportType] = value; }
        }

        public static string NonRevComparisonLogic
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonLogic] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevComparisonLogic]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonLogic] = value; }
        }

        public static string NonRevComparisonTotalFleetType
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevComparisonTotalFleetType] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevComparisonTotalFleetType]));
                }
            }
            set { HttpContext.Current.Session[_NonRevComparisonTotalFleetType] = value; }
        }

        #endregion

        #region "Reports"

        private static string _NonRevReportStartGroupBy = "SV-NonRevReportStart-GroupBy";

        public static string NonRevReportStartGroupBy
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevReportStartGroupBy] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevReportStartGroupBy]));
                }
            }
            set { HttpContext.Current.Session[_NonRevReportStartGroupBy] = value; }
        }
        
        #endregion

        #region "Approval"

        private static string _NonRevApprovalFormOption = "SV-NonRevApproval-FormOption";

        public static int? NonRevApprovalFormOption
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalFormOption] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToInt32(HttpContext.Current.Session[_NonRevApprovalFormOption]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalFormOption] = value; }
        }

        private static string _NonRevApprovalDayGroup = "SV-NonRevApproval-DayGroup";
        private static string _NonRevApprovalRacfId = "SV-NonRevApproval-RacfId";
        private static string _NonRevApprovalDate = "SV-NonRevApproval-Date";
        private static string _NonRevApprovalCountryGrid = "SV-NonRevApproval-CountryGrid";

        public static string NonRevApprovalDayGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalDayGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalDayGroup]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalDayGroup] = value; }
        }

        public static string NonRevApprovalRacfId
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalRacfId] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalRacfId]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalRacfId] = value; }
        }

        public static string NonRevApprovalDate
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalDate] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalDate]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalDate] = value; }
        }

        public static string NonRevApprovalCountryGrid
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevApprovalCountryGrid] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevApprovalCountryGrid]));
                }
            }
            set { HttpContext.Current.Session[_NonRevApprovalCountryGrid] = value; }
        }


        #endregion

        private static string _NonRevRemarkValidationGroup = "SV-NonRevRemark-ValidationGroup";

        public static string NonRevRemarkValidationGroup
        {
            get
            {
                if ((HttpContext.Current.Session[_NonRevRemarkValidationGroup] == null))
                {
                    return null;
                }
                else
                {
                    return (Convert.ToString(HttpContext.Current.Session[_NonRevRemarkValidationGroup]));
                }
            }
            set { HttpContext.Current.Session[_NonRevRemarkValidationGroup] = value; }
        }

        #endregion

    }
}