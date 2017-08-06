﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class SpreadCountryRow
    {
        public static readonly string[] HeaderRows =
        {
            "Country", "Car Segment", "Car Class", "Car Group", "Total Fleet", "Revenue", "Cost", "Spread"
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, "#,#.00", "#,#.00", "#,#.00", "#,#.00"
        };

        public string Country { get; set; }
        public string CarSegment { get; set; }
        public string CarClass { get; set; }
        public string CarGroup { get; set; }
        public double TotalFleet { get; set; }
        public double Revenue { get; set; }
        public double Cost { get; set; }
        public double Spread
        {
            get { return Revenue - Cost; }
        }

    }
}