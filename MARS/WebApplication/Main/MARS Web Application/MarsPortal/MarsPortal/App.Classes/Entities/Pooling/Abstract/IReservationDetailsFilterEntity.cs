using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Classes.Entities.Pooling.Abstract {
    public interface IReservationDetailsFilterEntity {
        string CheckInOut { get; set; }
        string Filter { get; set; }
        string Cdp { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        string ResId { get; set; }
        string Gold1 { get; set; }
        string CustomerName { get; set; }
        string FlightNbr { get; set; }
    }
}
