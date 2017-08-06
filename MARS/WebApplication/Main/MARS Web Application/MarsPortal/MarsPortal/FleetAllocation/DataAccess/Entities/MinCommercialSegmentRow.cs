using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class MinCommercialSegmentRow
    {
        public static readonly string[] HeaderRows =
        {
            "Country", "Pool", "Location Group", "Location", "Car Segment",
            "CommercialSegment", "Last Changed On", "Last Changed By", "Percent"
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, string.Empty, string.Empty, "P"
        };

        public string Country { get; set; }
        public string Pool { get; set; }
        public string LocationGroup { get; set; }
        public string Location { get; set; }
        public string CarSegment { get; set; }
        public string CommercialSegment { get; set; }
        public DateTime? LastChangedOn { get; set; }
        public string LastChangedBy { get; set; }
        public double? Percentage { get; set; }
    }
}