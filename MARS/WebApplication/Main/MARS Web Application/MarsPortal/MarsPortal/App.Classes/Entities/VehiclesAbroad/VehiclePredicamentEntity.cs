using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.VehiclesAbroad.Abstract;

namespace App.Classes.Entities.VehiclesAbroad {
    public class VehiclePredicamentEntity : IVehiclePredicamentEntity {
        public string ownwwd { get; set; }
        public string duewwd { get; set; }
        public string lstwwd { get; set; }
    }
}