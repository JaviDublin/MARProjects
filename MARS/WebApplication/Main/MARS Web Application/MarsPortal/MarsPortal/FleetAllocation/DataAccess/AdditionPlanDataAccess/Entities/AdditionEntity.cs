using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess.Entities
{
    public class AdditionEntity
    {
        public static readonly string[] HeaderRows =
        {
            "Year", "Week", "Car Group", "Location", "Additions", "CpU"
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, "#,0", "#,0.00"
        };

        public int GetCarGroupId()
        {
            return CarGroupId;
        }

        public int GetLocationId()
        {
            return LocationId;
        }

        public int Year { get; set; }

        public int IsoWeek { get; set; }
        
        public string CarGroup { get; set; }
        public string Location { get; set; }

        public int Amount { get; set; }

        public int CarGroupId { private get; set; }
        public int LocationId { private get; set; }

        public double Contribution { get; set; }
    }
}