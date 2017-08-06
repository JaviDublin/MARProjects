using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities
{
    public class LocationIdHolder
    {
        public string LocationName { get; set; }
        public string LocationId { get; set; }
        public string OwningCountry { get; set; }
        public int VehicleCount { get; set; }
    }
}