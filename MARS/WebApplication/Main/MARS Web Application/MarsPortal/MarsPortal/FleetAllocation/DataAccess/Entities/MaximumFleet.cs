using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class MaximumFleet
    {
        public int LocationGroupId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int CarGroupId { get; set; }
        public int CarSegmentId { get; set; }
        public int Year { get; set; }

        public byte WeekNumber { get; set; }
        public DateTime PeakDay { get; set; }
        public decimal MaxForLocationGroup { get; set; }
        public double MaxForLocation { get; set; }

        public string LocationGroupName { get; set; }
        public string CarGroupName { get; set; }
    }
}