using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Reservations.Entities
{
    public class ReservationData
    {
        public int ReservationId { get; set; }
        public string ExternalId { get; set; }
        public string CarGroupReserved { get; set; }
        public string CarGroupUpgraded { get; set; }
        public string Country { get; set; }
        public string PickupLocation { get; set; }
        public DateTime PickupDate { get; set; }
        public string ReturnLocation { get; set; }
        public DateTime ReturnDate { get; set; }
        public string CustomerName { get; set; }
        public string FlightNumber { get; set; }
        

        public int DaysReserved 
        {
            get { return (int)Math.Round((ReturnDate - PickupDate).TotalDays,0) ; }
        }

        public string Remark { get; set; }
        public string Comment { get; set; }
        
        public bool? NeverLost { get; set; }
        public DateTime BookedDate { get; set; }
        public string Tariff { get; set; }
        public string N1Type { get; set; }
        public string GoldService { get; set; }
    }
}