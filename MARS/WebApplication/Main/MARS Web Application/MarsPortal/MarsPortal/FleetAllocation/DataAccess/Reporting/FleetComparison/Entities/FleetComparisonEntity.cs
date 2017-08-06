using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Reporting.FleetComparison.Entities
{
    public class FleetComparisonEntity
    {
        public static readonly string[] HeaderRows =
        {
            "Fleet", "Year", "Week", "Additions"
        };

        public string FleetGroupName { get; set; }
        public short Year { get; set; }
        public byte Week { get; set; }
        public int Additions { get; set; }
    }
}