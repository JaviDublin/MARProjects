using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class RevenueDatabaseRow
    {
        public RevenueDatabaseRow()
        {

        }

        public DateTime ReportingDate { get; set; }
        public string DwCountry { get; set; }
        public string LocationCode { get; set; }
        public string CarGroupCode { get; set; }
        public string CommercialCarSegmentCode { get; set; }
        public int RentalCount { get; set; }
        public double DaysDriven { get; set; }
        public int RtDays { get; set; }
        public int TranDays { get; set; }
        public double GrossRev { get; set; }
        public double PerformanceRevenue { get; set; }

    }
}