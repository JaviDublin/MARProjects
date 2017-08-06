using System;

namespace Mars.App.Classes.Phase4Dal.NonRev.Entities
{
    public class NonRevPeriodEntryRemark
    {
        public int PeriodEntryRemarkId { get; set; }
        public int PeriodEntryId { get; set; }
        public int PeriodId { get; set; }
        public int VehicleId { get; set; }

        public int RemarkId { get; set; }
        public string ReasonText { get; set; }
        public string Remark { get; set; }
        public DateTime ExpectedResolutionDate { get; set; }
        public DateTime? Timestamp { get; set; }
        public string UserId { get; set; }
    }
}