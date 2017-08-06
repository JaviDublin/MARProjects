using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Internal;

namespace Mars.FleetAllocation.DataAccess.Entities.Ranking
{
    public class RankingOrderEntitiy
    {
        public static readonly string[] HeaderRows =
        {
           "Car Group", "Location", "SpreadPerUnit", "Rank", "Contribution"
           
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, "#,0.00", "#,0", "#,0"
        };

        public int GetCarGroupId()
        {
            return CarGroupId;
        }

        public int GetLocationId()
        {
            return LocationId;
        }


        public int CarGroupId { private get; set; }
        public string CarGroup { get; set; }
        public int LocationId { private get; set; }
        public string Location { get; set; }
        public double SpreadPerUnit { get; set; }
        public int Rank { get; set; }

        public double Contribution { get; set; }

    }
}