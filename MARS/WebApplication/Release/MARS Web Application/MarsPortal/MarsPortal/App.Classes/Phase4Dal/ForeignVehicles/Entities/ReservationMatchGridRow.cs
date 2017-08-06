using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities
{
    public class ReservationMatchGridRow
    {
        public int ReservationId { get; set; }
        public string ExternalId { get; set; }
        public DateTime PickupDate { get; set; }
        public string PickupLocation { get; set; }
        public string CarGroup { get; set; }
        public string CustomerName { get; set; }
        public int ReservationDuration { get; set; }
        public int DaysToPickup { get; set; }
        public int VehiclesMatched { get; set; }
    }
}