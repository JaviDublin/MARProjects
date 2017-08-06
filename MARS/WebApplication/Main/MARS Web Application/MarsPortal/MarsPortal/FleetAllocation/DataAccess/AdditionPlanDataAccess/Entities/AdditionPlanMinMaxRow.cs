using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.FleetAllocation.UserControls;

namespace Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess.Entities
{
    public class AdditionPlanMinMaxRow
    {
        public static readonly string[] HeaderRows =
        {
            "Year", "Week", "Car Group", "Location", 
            "Rank","Operational Fleet", "Additions and Deletions", "Min Fleet", "Max Fleet", "Contribution"
        };

        public int Year { get; set; }
        public int Week { get; set; }
        public string CarGroup { get; set; }
        public string Location { get; set; }
        public int Rank { get; set; }
        public int TotalFleet { get; set; }
        public int AdditionsAndDeletions { get; set; }
        public int MinFleet { get; set; }
        public int MaxFleet { get; set; }
        public decimal Contribution { get; set; }

    }
}