using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Entities.VehiclesAbroad {

    public class ReservationDetailsEntity {

        public string country { get; set; }
        public string destination { get; set; }
        public string rentLocation { get; set; }
        public string rentDestination { get; set; }
        public string driverName { get; set; }
        public string carClass { get; set; }
        public string reservationNo { get; set; }
        public string reservationDays { get; set; }
        public string mileage { get; set; }
        public string remarks { get; set; }
        public string arrivalDate { get; set; }
        public string arrivalTime { get; set; }
        public string returnDate { get; set; }
        public string returnTime { get; set; }
    }
}