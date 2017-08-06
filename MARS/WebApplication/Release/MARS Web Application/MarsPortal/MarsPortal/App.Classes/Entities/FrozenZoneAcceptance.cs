using System;

namespace App.Entities
{
    public class FrozenZoneAcceptance
    {
        public int PKID { get; set; }
        public string FleetPlan { get; set; }
        public string AcceptedBy { get; set; }
        public string Year { get; set; }
        public DateTime AcceptedDate { get; set; }
        public int AcceptedWeekNumber { get; set; }
        public string Country { get; set; }
    }
}