using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.NonRev.Entities.FileUploadEntities
{
    public class UploadRow
    {
        public int VehicleId{ get; set; } 
        public string Vin { get; set; }
        public string OwningCountry { get; set; } 
        public string EstimatedResolved { get; set; }
        public DateTime ParsedEstimatedResolved { get; set; }
        public string ReasonName { get; set; }
        public string RemarkText { get; set; }
        public int ReasonId { get; set; }

        public int PeriodId { get; set; }
    }
}