using System;
namespace App.Classes.Entities.VehiclesAbroad.Abstract {
    public interface IReservationMatchEntity {
        string Matches { get; set; }
        string ResCheckinLoc { get; set; }
        DateTime? ResCheckoutDate { get; set; }
        string ResCheckoutLoc { get; set; }
        string ResDriverName { get; set; }
        string ResGroup { get; set; }
        string ResId { get; set; }
        string ResLocation { get; set; }
        string ResNoDaysReserved { get; set; }
        string ResNoDaysUntilCheckout { get; set; }
    }
}
