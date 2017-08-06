using System;
namespace App.Entities.VehiclesAbroad.Abstract {
    interface IReservationBalanceEntity {
        string destination { get; set; }
        string owning { get; set; }
        int result { get; set; }
    }
}
