using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class MaxFleetFactorRow
    {
        private readonly string _dayOfWeekString;
        public MaxFleetFactorRow(int dow)
        {
            //Database Dow is 0-6, C# 1-7
            _dayOfWeekString = Enum.GetName(typeof (DayOfWeek), dow - 1);
        }

        public static readonly string[] HeaderRows =
        {
            "Country", "Pool", "Location Group", "Location", "Car Segment","Car Class","Car Group",
            "CommercialSegment", "Last Changed On", "Last Changed By", "Non Revenue", "Utilization"
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, string.Empty, string.Empty, "P", "P"
        };

        public string Country { get; set; }
        public string Pool { get; set; }
        public string LocationGroup { get; set; }
        public string Location { get; set; }
        public string CarSegment { get; set; }
        public string CarClass { get; set; }
        public string CarGroup { get; set; }

        public string DayOfWeekName
        {
            get { return _dayOfWeekString; }
        }

        public DateTime? LastChangedOn { get; set; }
        public string LastChangedBy { get; set; }
        public double? NonRevenue { get; set; }
        public double? Utilization { get; set; }
    }
}