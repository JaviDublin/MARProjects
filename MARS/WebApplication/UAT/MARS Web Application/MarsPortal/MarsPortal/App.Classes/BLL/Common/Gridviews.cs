
namespace App.BLL
{
    public class Gridviews
    {

        public enum GridviewToPage : int
        {
            PoolingReservations = 1,
            PoolingStatisticsSelection = 2,
            PoolingStatisticsDates = 3,
            AvailabilityCarSearch = 4,
            AvailabilityStatisticsSelection = 5,
            AvailabilityStatisticsDates = 6,
            MaintenanceUsers = 7,
            MappingCountry = 8,
            MappingAreaCode = 9,
            MappingCMSPool = 10,
            MappingCMSLocation = 11,
            MappingOPSRegion = 12,
            MappingOPSArea = 13,
            MappingLocation = 14,
            MappingCarSegment = 15,
            MappingCarClass = 16,
            MappingCarGroup = 17,
            MappingModelCode = 18,
            UserSearches = 19,
            MappingVehiclesLease = 20,
            NonRevenueCarSearch = 21,
            NonRevenueReportStart = 22,
            NonRevenueApproval = 23,
            NonRevenueComparison = 24,
            NonRevenueHistoricalTrend = 25,
            NonRevenueApprovalUsers = 26,
            NonRevenueApprovalUsersList = 27
        }

        public enum GridviewNoDataMessage
        {
            General = 1,
            StatisticsSelection = 2,
            StatisticsDate = 3
        }

        public static string GetEmptyDataText(int? gridviewNoData)
        {

            string emptyDataString = null;

            switch (gridviewNoData)
            {
                case (int)GridviewNoDataMessage.StatisticsDate:
                case (int)GridviewNoDataMessage.StatisticsSelection:
                    emptyDataString = Resources.lang.GridviewStatisticsNoData;
                    break;

            }

            return emptyDataString;

        }

    }
}