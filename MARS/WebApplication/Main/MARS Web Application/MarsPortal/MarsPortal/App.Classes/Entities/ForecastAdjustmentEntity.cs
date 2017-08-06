using System;

namespace App.Entities
{
    public class ForecastAdjustmentEntity
    {
        public Country Country { get; set;}
        public CMSPool CMSPool { get; set; }
        public LocationGroup LocationGroup { get; set; }
        public CarSegment CarSegment { get; set; }
        public CarGroup CarGroup { get; set; }
        public CarClass CarClass { get; set; }
        public decimal Adjustment_TD { get; set; }
        public decimal Adjustment_BU1 { get; set; }
        public decimal Adjustment_BU2 { get; set; }
        public decimal Adjustment_RC { get; set; }
        public decimal OnRent { get; set; }
        public decimal Constrained { get; set; }
        public decimal UnConstrained { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public ForecastAdjustmentEntity()
        {
            Country = new Country();
            CMSPool = new CMSPool();
            LocationGroup = new LocationGroup();
            CarSegment = new CarSegment();
            CarGroup = new CarGroup();
            CarClass = new CarClass();
            Date = DateTime.MinValue;
        }
    }
}