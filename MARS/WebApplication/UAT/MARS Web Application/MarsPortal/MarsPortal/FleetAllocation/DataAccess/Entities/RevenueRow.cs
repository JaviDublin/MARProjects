using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class RevenueRow
    {
        public static readonly string[] HeaderRows =
        {
            "Date", "Location", "Owning Country", "Car Segment", "Car Class", "Car Group",  "Commercial Car Segment" , "Finance Days", "Gross Revenue"
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "#,#.00"
        };

        public DateTime Date { get; set; }
        public string LocationCode { get; set; }
        public string OwningCountry { get; set; }

        public string CarSegment { get; set; }
        public string CarClass { get; set; }
        public string CarGroup { get; set; }

        public string CommercialCarSegment { get;set; }

        public int FinanceDays { get; set; }
        public double Revenue { get; set; }
    }
}