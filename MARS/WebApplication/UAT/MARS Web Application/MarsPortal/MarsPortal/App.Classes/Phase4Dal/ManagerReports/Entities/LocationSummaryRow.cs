using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.ManagerReports.Entities
{
    public class LocationSummaryRow
    {
        public int RowId { get; set; }
        public string CarClassName { get; set; }
        public string CarSegmentName { get; set; }
        public double AvailabilityOp { get; set; }
        public double AvailabilityShop { get; set; }
        public double AvailabilityAvailable { get; set; }
        public double AvailabilityOnRent { get; set; }
        public double AvailabilityIdle { get; set; }
        public double AvailabilityOverdue { get; set; }
        public double AvailabilityUtilization { get; set; }


        public double NonRevGreaterThanThree { get; set; }
        public double NonRevGreaterThanSeven { get; set; }


        public double ReservationCheckInToday { get; set; }
        public double ReservationCheckInRemaining { get; set; }
        public double ReservationCheckOutToday { get; set; }
        public double ReservationCheckOutRemaining { get; set; }

    }
}