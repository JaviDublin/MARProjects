using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.FleetAllocation.UserControls;

namespace Mars.App.Classes.Phase4Dal.Administration.Sublease.Entities
{
    public class SubleaseDataRow
    {
        public static readonly string[] HeaderRows =
        {
            "VIN", "Unit", "Model", "Owning Country", "Renting Country", "Start Date"
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "dd MMM yyyy"
        };

        public string Vin { get; set; }
        public int UnitNumber { get; set; }
        public string Model { get; set; }
        public string OwningCountry { get; set; }
        public string RentingCountry { get; set; }
        public DateTime StartDate { get; set; }
    }
}