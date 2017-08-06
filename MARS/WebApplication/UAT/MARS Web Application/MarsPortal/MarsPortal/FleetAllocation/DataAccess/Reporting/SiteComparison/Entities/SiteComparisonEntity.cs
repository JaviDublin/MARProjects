using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Reporting.SiteComparison.Entities
{
    public class SiteComparisonEntity
    {
        public static readonly string[] HeaderRows =
        {
            "Location", "Year", "Week", "Additions"
        };

        public string Location { get; set; }
        public short Year { get; set; }
        public byte Week { get; set; }
        public int Additions { get; set; }
    }
}