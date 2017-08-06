using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities
{
    public class VehicleMatchGridRow
    {
        public int VehicleId { get; set; }
        public string OwningCountry { get; set; }
        public string LastLocation { get; set; }
        public string CarGroup { get; set; }
        public string LicensePlate { get; set; }
        public int UnitNumber { get; set; }
        public string ModelDescription { get; set; }
        public int NonRevDays { get; set; }
        public string OperationalStatusCode { get; set; }
        public int ReservationsMatched { get; set; }
    }
}