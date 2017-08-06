using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Pooling.Abstract;

namespace App.Classes.Entities.Pooling {
    public class ResevationDetailsFilterEntity : IReservationDetailsFilterEntity {
        public string CheckInOut { get; set; }
        public string Filter { get; set; }
        public string Cdp { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ResId { get; set; }
        public string Gold1 { get; set; }
        public string CustomerName { get; set; }
        public string FlightNbr { get; set; }
    }
}