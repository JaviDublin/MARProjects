using System.Globalization;

namespace Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities
{
    public class MonthlyLimitRow
    {
        public static readonly string[] HeaderRows =
        {
            "Year", "Month", "Car Segment", "Car Group", "Additons", "Assigned"
        };

        public int GetMonth()
        {
            return Month;
        }

        public int GetCarGroupId()
        {
            return CarGroupId;
        }

        public int Year { get; set; }
        public int Month { private get; set; }
        public string MonthName { get { return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month); } }
        public int CarGroupId { private get; set; }
        public string CarSegmentName { get; set; }
        public string CarGroupName { get; set; }
        
        public int AdditionsLimit { get; set; }
        public int Assigned { get; set; }
    }
}