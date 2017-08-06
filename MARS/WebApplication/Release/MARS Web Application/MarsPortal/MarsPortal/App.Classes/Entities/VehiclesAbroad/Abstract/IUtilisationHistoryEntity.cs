using System;
namespace App.Classes.Entities.VehiclesAbroad.Abstract {
    public interface IUtilisationHistoryEntity {
        string ownCountry { get; set; }
        DateTime? REPDATE { get; set; }
        double? UTILIZATION_IN_COUNTRY { get; set; }
        double? UTILIZATION_OUT_OF_COUNTRY { get; set; }
    }
}
