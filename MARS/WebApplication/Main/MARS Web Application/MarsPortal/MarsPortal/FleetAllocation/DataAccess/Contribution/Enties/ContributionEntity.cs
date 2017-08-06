using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Contribution.Enties
{
    public class ContributionEntity
    {
        public int CarGroupId { get; set; }
        public int LocationId { get; set; }
        public double Revenue { get; set; }
        public double HoldingCost { get; set; }
    }
}