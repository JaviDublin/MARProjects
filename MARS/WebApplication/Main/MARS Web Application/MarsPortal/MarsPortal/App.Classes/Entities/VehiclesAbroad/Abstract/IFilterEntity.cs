using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Entities.VehiclesAbroad {
    public interface IFilterEntity {
        string OwnCountry { get; set; }
        string DueCountry { get; set; }
        string Pool { get; set; }
        string Location { get; set; }
        string CarSegment { get; set; }
        string CarGroup { get; set; }
        string CarClass { get; set; }
        int VehiclePredicament { get; set; }

        // start date and end date required for the reservation 
        DateTime ReservationStartDate { get; set; }
        DateTime ReservationEndDate { get; set; }

        // the reservation return filter entities, for DueCountry
        string DuePool { get; set; }
        string DueLocationGroup { get; set; }

        string Operstat { get; set; }
        string MoveType { get; set; }

        int nonRev { get; set; }
    }
}
