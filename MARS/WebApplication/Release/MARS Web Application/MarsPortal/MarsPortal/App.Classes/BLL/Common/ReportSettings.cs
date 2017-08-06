
namespace App.BLL
{
    public class ReportSettings {

        public enum ReportSettingsPage : int 
        {

            ATFleetStatus = 6,
            ATHistoricalTrend = 7,
            ATSiteComparison = 8,
            ATFleetComparison = 9,
            ATKPI = 10,
            ATKPIDownload = 11,
            ATCarSearch = 12,
            ATStatistics = 13,
            NRCarSearch = 14,
            NRReportStart = 15
        }

        public enum ReportSettingsTool : int 
        {
            MarsHome = 1,
            Statistics = 2,
            Availability = 3,
            NonRevenue = 4,
            NonRevStartReport = 5,
            NonRevApproval = 6
        }

        public enum ReportOptions : int {
            Country = 1,
            CMSPool = 2,
            OPSRegion = 3,
            OPSArea = 4,
            CMSLocationGroup = 5,
            Location = 6,
            Fleet = 7,
            CarSegment = 8,
            CarClass = 9,
            CarGroup = 10,
            AllNew = 11
        }

        public enum OptionLogic : int {
            CMS = 1,
            OPS = 2
        }

        public enum LocationOptions : int
        { 
            LSTWWD = 1,
            DUEWWD = 2
        }

        public enum NonRevComparison : int
        { 
            Fleet = 1,
            Site = 2
        }

       
        public static string GroupByStartOptions(string option)
        {
            switch (option)
            { 
                case "1":
                    return "KCI";
                   
                case "2":
                    return "STAT";
                   
                default:
                    return "KCI";
                  
            }
        }

        public static string GetDayOfWeek(int dayOfWeek) {

            string dayOfWeekStr = string.Empty;
            switch (dayOfWeek) {

                case 1:
                    dayOfWeekStr = Resources.lang.Sunday;
                    break;
                case 2:
                    dayOfWeekStr = Resources.lang.Monday;
                    break;
                case 3:
                    dayOfWeekStr = Resources.lang.Tuesday;
                    break;
                case 4:
                    dayOfWeekStr = Resources.lang.Wednesday;
                    break;
                case 5:
                    dayOfWeekStr = Resources.lang.Thursday;
                    break;
                case 6:
                    dayOfWeekStr = Resources.lang.Friday;
                    break;
                case 7:
                    dayOfWeekStr = Resources.lang.Saturday;
                    break;
                default:
                    dayOfWeekStr = Resources.lang.ReportSettingsALLSelection;
                    break;
            }

            return dayOfWeekStr;


        }

        public static string GetDateRange(int dateRange) {

            string dateRangeStr = string.Empty;
            switch (dateRange) {
                case -1:
                    dateRangeStr = Resources.lang.Oneday;
                    break;
                case -7:
                    dateRangeStr = Resources.lang.Previous7days;
                    break;
                case -30:
                    dateRangeStr = Resources.lang.Previous30days;
                    break;
                case -90:
                    dateRangeStr = Resources.lang.Previous90days;
                    break;
                case -180:
                    dateRangeStr = Resources.lang.Previous180days;
                    break;
                case -365:
                    dateRangeStr = Resources.lang.Previous365days;
                    break;
            }
            return dateRangeStr;


        }

        public static string GetGroupingCriteria(int dateRange) {

            string grouping_criteria = string.Empty;
            switch (dateRange) {
                case -7:
                    grouping_criteria = "DAY";
                    break;
                case -30:
                    grouping_criteria = "DAY";
                    break;
                case -90:
                    grouping_criteria = "WEEK";
                    break;
                case -180:
                    grouping_criteria = "WEEK";
                    break;
                case -365:
                    grouping_criteria = "MONTH";
                    break;
                default:
                    grouping_criteria = "DAY";
                    break;
            }
            return grouping_criteria;

        }

        public static string getRangeGroupingCriteria(int dateRange) {
            // Similar to the above method but returns a string dependant on the range
            if (dateRange >= -30) {
                return "DAY";
            }
            if (dateRange >= -180) {
                return "WEEK";
            }
            if (dateRange >= -365) {
                return "MONTH";
            }
            return "DAY"; // default
        }
    }
}