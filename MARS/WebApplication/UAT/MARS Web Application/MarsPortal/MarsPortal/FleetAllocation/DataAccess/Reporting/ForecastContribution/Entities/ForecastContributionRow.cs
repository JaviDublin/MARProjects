using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Reporting.ForecastContribution.Entities
{
    public class ForecastContributionRow
    {
        public static readonly string[] HeaderRows =
        {
            "Year", "Month", "Car Group", "Location", "CpU", "Expected" ,"Cumulative Additions A", "Cumulative Additions B"
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, "#,0", "#,0.00", "#,0.00", "#,0.00"
        };

        public short Year { get; set; }
        public byte Month { get; set; }
        public string CarGroup { get; set; }
        public string Location { get; set; }
        public double CpU { get; set; }
        public double Expected { get; set; }
        public int CumulativeAdditionsA { get; set; }
        public int CumulativeAdditionsB { get; set; }
    }
}