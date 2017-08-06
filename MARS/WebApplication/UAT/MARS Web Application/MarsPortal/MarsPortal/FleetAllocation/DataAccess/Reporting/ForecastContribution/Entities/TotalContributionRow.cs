using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Reporting.ForecastContribution.Entities
{
    public class TotalContributionRow
    {
        public short Year { get; set; }
        public byte Month { get; set; }

        public double ExpectedContribution { get; set; }
        public double ExpectedContributionA { get; set; }
        public double ExpectedContributionB { get; set; }
    }
}