using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.NonRev.Entities
{
    public class VehicleApprovalHistory
    {
        public int VehicleId { private get; set; }
        public string Vin { get; set; }
        public int DaysNonRev { get; set; }
        public string VehicleFleetType { get; set; }
        public string LastLocation { get; set; }
        public string LastOperationalStatus { get; set; }
    }
}