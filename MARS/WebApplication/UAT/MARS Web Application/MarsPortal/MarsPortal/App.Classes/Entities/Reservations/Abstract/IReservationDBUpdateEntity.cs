using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Classes.Entities.Reservations.Abstract {
    public interface IReservationDBUpdateEntity {
        DateTime? LastUpdate { get; set; }
        string TeraDataMessage { get; set; }
        string FleetMessage { get; set; }
    }
}
