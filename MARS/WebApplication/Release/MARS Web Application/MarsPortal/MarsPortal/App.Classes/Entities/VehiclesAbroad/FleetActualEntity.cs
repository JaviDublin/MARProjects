using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.VehiclesAbroad {
    public class FleetActualEntity {
        public string OwningCountry { get; set; }
        public string CurrentLocation { get; set; }
        public int Count { get; set; }
    }
}