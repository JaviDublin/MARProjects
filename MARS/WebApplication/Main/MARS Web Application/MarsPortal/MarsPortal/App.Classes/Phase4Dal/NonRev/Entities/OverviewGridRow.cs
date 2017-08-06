using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.NonRev.Entities
{
    public class OverviewGridRow
    {
        public int VehicleId { get; set; }
        public string LastLocationCode { get; set; }
        public string CarGroup{ get; set; }
        public string LicensePlate { get; set; }
        public string UnitNumber { get; set; }
        public string ModelCode { get; set; }
        public string ModelDescription { get; set; }
        public DateTime? InstallationDateTime { get; set; }
        public DateTime? BlockDateTime { get; set; }
        public DateTime? InstallationMsoDateTime { get; set; }
        public DateTime? LastChangeDateTime { get; set; }
        public int LastMilage { get; set; }
        public string OwningArea { get; set; }
        public string OwningCountry { get; set; }

        public int BdDays { get; set; }
        public int MmDays { get; set; }
        
        public string OperationalStatusCode { get; set; }
        public string MovementTypeCode { get; set; }
        public string DepreciationStatus { get; set; }
        public string HoldFlag1 { get; set; }
        public DateTime? NextRent{ get; set; }
        public string RemarkCode{ get; set; }
        public string Remark{ get; set; }
        public string Serial{ get; set; }
        public int NonRevDays { get; set; }
        public int InCountryDays { get; set; }
        public string NextRentColor { get; set; }
        
        public string ExpectedLocationCode { get; set; }
        public DateTime? ExpectedDateTime { get; set; }

        public DateTime? LastUpdate { get; set; }

        public string LastRemark { get; set; }
        public string Comment { get; set; }
        public string CommentShort { get; set; }
        
        public string LastRemarkFull { get; set; }
        public string LastReason { get; set; }
        

        public string LastDriverName { get; set; }
        public string DocumentNumber { get; set; }
        public string CarGroupCharged { get; set; }
        

        public DateTime EstimatedResultion { get; set; }

        public string VehicleFleetTypeName { get; set; }

        public int? LastPeriodEntryId { get; set; }

        public string ExpectedDateString
        {
            get { return ExpectedDateTime.HasValue && ExpectedDateTime.Value > new DateTime(1950, 1, 1) ? ExpectedDateTime.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty; }
        }

        public string NonRevTimeSpanString
        {
            get
            {
                if (!LastUpdate.HasValue) return string.Empty;
                var nonRevTimeSpan = DateTime.Now - LastUpdate.Value;
                return string.Format("{0} {2} {1} {3}", nonRevTimeSpan.Days, nonRevTimeSpan.Hours,
                    nonRevTimeSpan.Days == 1 ? "Day" : "Days", nonRevTimeSpan.Hours == 1 ? "Hour" : "Hours");
            }
        }
    }
}