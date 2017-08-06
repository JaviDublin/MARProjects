using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Reservations.Abstract;

namespace App.Classes.Entities.Reservations {
    public class ReservationDBUpdateEntity : IReservationDBUpdateEntity {
        public DateTime? LastUpdate { get; set; }
        public string FleetMessage { get; set; }
        public string TeraDataMessage { get; set; }
        public int Id { get; set; }
    }
}