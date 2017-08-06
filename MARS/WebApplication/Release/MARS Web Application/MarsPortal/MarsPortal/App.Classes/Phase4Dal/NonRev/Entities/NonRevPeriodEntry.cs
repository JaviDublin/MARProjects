using System;

namespace Mars.App.Classes.Phase4Dal.NonRev.Entities
{
    public class NonRevPeriodEntry
    {
        public int PeriodEntryId { get; set; }
        public int PeriodId { get; set; }
        public int VehicleId { get; set; }
        public string OperationalStatusCode { get; set; }
        public string MovementTypeCode { get; set; }
        public string OperationalStatusFull { get; set; }
        public string MovementTypeFull { get; set; }
        public string LastLocationCode { get; set; }
        public DateTime? LastChangeDateTime { get; set; }
        public int RemarksEntered { get; set; }
    }
}