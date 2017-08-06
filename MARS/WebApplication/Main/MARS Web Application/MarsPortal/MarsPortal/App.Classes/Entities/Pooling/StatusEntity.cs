using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Pooling.Abstract;

namespace App.Classes.Entities.Pooling {
    public class StatusEntity : IStatusEntity {
        public DateTime RepDate { get; set; }
        public int Available { get; set; }
        public int OpenTrips { get; set; }
        public int Reservations { get; set; }
        public int Oneway { get; set; }
        public int Gold { get; set; }
        public int Prepaid { get; set; }
        public int CheckIn { get; set; }
        public int OnewayCI { get; set; }
        public int CheckInOffset { get; set; }
        public int Balance { get; set; }
    }
}