using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class DemandGapVisualization
    {
        public static readonly string[] HeaderRows =
        {
            "Car Group ID", "Location Id", "Car Group", "Location", "Week Number", "Operational Fleet", "Min Fleet Required", "Max Fleet",
            "Additions", "Deletions",  "Expected Fleet", "Gap"
        };

        public int CarGroupId { get; set; }
        public int LocationId { get; set; }
        public string CarGroupName { get; set; }
        public string LocationName { get; set; }
        public int WeekNumber { get; set; }

        public int OperationalFleet { get; set; }
        public double MinFleetRequired { get; set; }
        public double MaxFleet { get; set; }

        public int Additions { get; set; }
        public int Deletions { get; set; }

        
        public int ExpectedFleet { get; set; }

        public int GapRounded { get { return ExpectedFleet - ((int)MinFleetRequired); } }

        

    }
}