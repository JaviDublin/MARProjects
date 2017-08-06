using System;
using App.Classes.Entities.VehiclesAbroad.Abstract;

namespace App.Entities.VehiclesAbroad {
    public class ReservationMatchEntity:IReservationMatchEntity {
        public string ResLocation { get; set; }
        public string ResGroup { get; set; }
        public DateTime? ResCheckoutDate { get; set; }
        public string ResCheckoutLoc { get; set; }
        public string ResCheckinLoc { get; set; }
        public string ResId { get; set; }
        public string ResNoDaysUntilCheckout { get; set; }
        public string ResNoDaysReserved { get; set; }
        public string ResDriverName { get; set; }
        public string Matches { get; set; }
    }
}