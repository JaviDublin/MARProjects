using System;

namespace App.Entities
{
    public class FleetPlanEntryArchive
    {
        public int PKID { get; set; }
        public string FleetPlan { get; set; }
        public bool IsAddition { get; set; }        
        public string UploadedBy { get; set; }
        public DateTime UploadedDate { get; set; }
        public string UploadedFileName { get; set; }
        public string UploadedArchiveFileName { get; set; }
        public string Country { get; set; }
    }
}