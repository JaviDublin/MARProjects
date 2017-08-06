using System;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class NessesaryFleet
    {
        public int LocationId { get; set; }
        public int CarGroupId { get; set; }
        public int CommericalCarSegmentId { get; set; }

        public string LocationName { get; set; }
        public string CarGroupName { get; set; }
        public string CommercialCarSegmentNameName { get; set; }
        

        public double AverageOnRent { get; set; }
        
        public double MinCommercialSegmentRequired { get; set; }

        public double Revenue { get; set; }
        public double TotalRevenueByCommercialCarSegment { get; set; }

        public double PercentOfTotalRevenue { get { return Revenue / TotalRevenueByCommercialCarSegment; } }
        public double MinNessesaryFleetRequired { get; set; }



    }
}