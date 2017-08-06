using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Dal.NonRev.Entities
{
    public class OverviewVehicleDetails
    {
        public int VehicleId { get; set; }

        public string Group { get; set; }
        public string GroupCharged { get; set; }
        public string TasModelCode { get; set; }
        public string ModelDescription { get; set; }
        public int UnitNumber { get; set; }
        public string Vin { get; set; }
        public string LastLocation { get; set; }
        public DateTime? LastChangeDateTime { get; set; }
        public string ExpectedLocation { get; set; }
        public DateTime? ExpectedDateTime { get; set; }
        public TimeSpan NonRevTimeSpan { get; set; }
        public int NonRevDays { get; set; }
        public int InCountryDays { get; set; }
        public string OwningCountry { get; set; }
        public string OperationalStatus { get; set; }
        public string MovementType { get; set; }
        public double LastMilage { get; set; }
        public string LastDocumentNumber { get; set; }
        public string LastDriverName { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? InstallationMsoDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public DateTime? BlockDate { get; set; }
        public string DepreciationStatus { get; set; }
        public string HoldFlag1 { get; set; }
        public string OwningArea { get; set; }
        public string PreviousLocationCode { get; set; }
        public int? DaysInBd { get; set; }
        public int? DaysInMm { get; set; }
        public string LiscencePlate { get; set; }
        public int? BlockMilage { get; set; }
        public string VehicleComment { get; set; }

        public bool IsNonRev { get; set; }

        public VehicleNonRevPeriod ActivePeriod
        {
            get
            {
                return Periods.FirstOrDefault(d => d.Active);
            }
        }

        public List<NonRevPeriodEntry> ActivePeriodEntries
        {
            get { return PeriodEntries.Where(d => d.PeriodId == ActivePeriod.VechicleNonRevPeriodId).ToList(); }
        }

        public List<NonRevPeriodEntryRemark> ActivePeriodEntryRemarks
        {
            get
            { return PeriodEntryRemarks.Where(d => d.PeriodId == ActivePeriod.VechicleNonRevPeriodId).ToList(); }
        }

        public string ExpectedDateString
        {
            get { return ExpectedDateTime.HasValue && ExpectedDateTime.Value > new DateTime(1950, 1, 1) ? ExpectedDateTime.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty; }
        }

        public string LastChangeDateTimeString
        {
            get { return LastChangeDateTime.HasValue && LastChangeDateTime.Value > new DateTime(1950, 1, 1) ? LastChangeDateTime.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty; }
        }

        public List<VehicleNonRevPeriod> Periods {get; set;}
        public List<NonRevPeriodEntry> PeriodEntries { get; set; }
        public List<NonRevPeriodEntryRemark> PeriodEntryRemarks { get; set; }
    }
}