using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.ManagerReports.Entities
{
    public class LocationSummaryForeignRow
    {
        public string CarSegmentName { get; set; }
        public double VehicleCount { get; set; }
        public double ReservationCount { get; set; }
    }
}