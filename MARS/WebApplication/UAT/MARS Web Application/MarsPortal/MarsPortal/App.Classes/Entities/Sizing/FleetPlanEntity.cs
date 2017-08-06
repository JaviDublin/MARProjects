using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Entities.Sizing.Abstract;

namespace Mars.Entities.Sizing {
    public class FleetPlanEntity : IFleetPlanEntity {
        public string Message { get; set; }
        public string Status { get; set; }
    }
}