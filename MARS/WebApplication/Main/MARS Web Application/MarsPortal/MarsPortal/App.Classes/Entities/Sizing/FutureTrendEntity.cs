using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.Entities.Sizing {
    public class FutureTrendEntity {
        public DateTime REP_DATE;
        public Int32 constrained;
        public Int32 unconstrained;
        public Int32 reservations_booked;
        public Int32 Current_Onrent;
        public Int32 NecessaryConstrained;
        public Int32 NecessaryUnconstrained;
        public Int32 NecessaryBooked;
        public Int32 ExpectedFleet;
    }
}