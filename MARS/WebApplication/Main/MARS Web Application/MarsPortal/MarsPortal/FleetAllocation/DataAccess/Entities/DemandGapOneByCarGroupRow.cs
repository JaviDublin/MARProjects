using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class DemandGapOneByCarGroup
    {
        public static readonly string[] HeaderRows =
        {
           "Car Group", "Week Number", "Demand Gap 1"
        };

        public int CarGroupId { private get; set; }
        public string CarGroupName { get; set; }
        public int WeekNumber { get; set; }

        public double Gap { private get; set; }

        public int GapRounded {
            get { return (int) Math.Round(Gap, 0); }
        }
    }
}