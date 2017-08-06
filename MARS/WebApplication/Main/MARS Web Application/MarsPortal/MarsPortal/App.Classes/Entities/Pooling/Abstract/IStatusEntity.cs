using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Classes.Entities.Pooling.Abstract {
    public interface IStatusEntity {
        DateTime RepDate { get; set; }
        int Available { get; set; }
        int OpenTrips { get; set; }
        int Reservations { get; set; }
        int Oneway { get; set; }
        int Gold { get; set; }
        int Prepaid { get; set; }
        int CheckIn { get; set; }
        int OnewayCI { get; set; }
        int CheckInOffset { get; set; }
        int Balance { get; set; }
    }
}
