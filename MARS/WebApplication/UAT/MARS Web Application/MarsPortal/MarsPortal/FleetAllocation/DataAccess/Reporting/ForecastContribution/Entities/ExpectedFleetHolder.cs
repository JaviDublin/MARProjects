using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Reporting.ForecastContribution.Entities
{
    public class ExpectedFleetHolder
    {
        public short Year { get; set; }
        public byte Month { get; set; }
        public int CarGroupId { get; set; }
        public int LocationId { get; set; }

        public decimal Expected { get; set; }
    }
}