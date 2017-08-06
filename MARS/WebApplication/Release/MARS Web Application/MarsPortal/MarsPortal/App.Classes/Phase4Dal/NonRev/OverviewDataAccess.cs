using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using App.BLL.Utilities;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.LicenceeRestriction;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;
using Mars.App.Classes.Phase4Dal.NonRev.Parameters;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal.NonRev
{
    public class OverviewDataAccess : NonRevBaseDataAccess
    {
        public OverviewDataAccess(Dictionary<DictionaryParameter, string> parameters = null) : base(parameters)
        {
            
        }


        public List<OverviewGridRow> GetVehicles()
        {
            bool includeRev = true;
            if (Parameters.ContainsKey(DictionaryParameter.NonRevOnly) &&
                !string.IsNullOrEmpty(Parameters[DictionaryParameter.NonRevOnly]))
            {
                includeRev = Parameters[DictionaryParameter.NonRevOnly] == false.ToString();
            }
            var vehicleData = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, includeRev, true);
            if(vehicleData == null) return new List<OverviewGridRow>();

            var today = DateTime.Now.Date;

            vehicleData = BaseVehicleDataAccess.RestrictByAdditionalParameters(Parameters, DataContext, vehicleData);

            
            var overViewDataGrid = from v in vehicleData
                                   orderby v.DaysSinceLastRevenueMovement descending, v.LastLocationCode, v.DepreciationStatus, v.CarGroup
                                   select new OverviewGridRow
                                   {
                                       VehicleId = v.VehicleId,
                                       LastLocationCode = v.LastLocationCode,
                                       CarGroup = v.CarGroup,
                                       LicensePlate = v.LicensePlate,
                                       Serial = v.Vin,
                                       HoldFlag1 = v.HoldFlag1,
                                       InCountryDays = v.DaysInCountry.HasValue ? v.DaysInCountry.Value : 0,
                                       DepreciationStatus = v.DepreciationStatus,
                                       LastChangeDateTime = v.LastChangeDateTime,
                                       LastDriverName = v.LastDriverName,
                                       DocumentNumber = v.LastDocumentNumber,
                                       CarGroupCharged = v.CarGroupCharged,
                                       LastMilage = v.LastMilage ?? 0,
                                       ModelDescription = v.ModelDescription,
                                       ModelCode = v.TasModelCode,
                                       BdDays = v.DaysInBd ?? 0,
                                       MmDays = v.DaysInMm ?? 0,
                                       InstallationDateTime = v.InstallationDate,
                                       InstallationMsoDateTime = v.InstallationMsoDate,
                                       BlockDateTime = v.BlockDate,
                                       MovementTypeCode = v.Movement_Type.MovementTypeCode,
                                       NextRent = null,
                                       OwningArea = v.OwningArea,
                                       OwningCountry = v.OwningCountry,
                                       NonRevDays = v.DaysSinceLastRevenueMovement ?? 0,
                                       NextRentColor = v.IsNonRev ? "~/App.Images/exclamation.png" : string.Empty,
                                       LastUpdate = v.VehicleNonRevPeriods.First(d => d.Active).VehicleNonRevPeriodEntries.Max(d => d.LastChangeDateTime),
                                       LastPeriodEntryId = v.IsNonRev ? v.VehicleNonRevPeriods.First(d => d.Active).VehicleNonRevPeriodEntries.Max(d => d.VehicleNonRevPeriodEntryId) 
                                                                    : (int?)null,
                                       OperationalStatusCode = v.Operational_Status.OperationalStatusCode,
                                       Remark = string.Empty,
                                       RemarkCode = string.Empty,
                                       UnitNumber = v.UnitNumber.ToString(),
                                       ExpectedDateTime = v.ExpectedDateTime,
                                       ExpectedLocationCode = v.ExpectedLocationCode ?? string.Empty,
                                       Comment = v.Comment,
                                       CommentShort = v.Comment.Substring(0, v.Comment.Length < 20 ? v.Comment.Length : 20)
                                       
                                   };

            var remData = from rem in DataContext.VehicleNonRevPeriodEntryRemarks
                          where rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Active
                          group rem by rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.VehicleId
                              into g
                              join rem2 in DataContext.VehicleNonRevPeriodEntryRemarks
                                      on g.Max(d => d.VehicleNonRevPeriodEntryRemarkId) equals rem2.VehicleNonRevPeriodEntryRemarkId
                              select new
                              {
                                  VehicleId = g.Key,
                                  RemarkText = rem2.Remark.Substring(0, 20),
                                  RemarkTextFull = rem2.Remark,
                                  EstimatedResolution = rem2.ExpectedResolutionDate,
                                  Reason = rem2.NonRev_Remarks_List.RemarkText
                              };



            var localVehicleData = overViewDataGrid.ToList();
            var remarksData = remData.ToList();

            
            foreach (var v in localVehicleData)
            {
                int vehicleId = v.VehicleId;
                
                var remarkEntity = remarksData.FirstOrDefault(d => d.VehicleId == vehicleId);

                if (remarkEntity == null)
                {
                    v.LastRemark = string.Empty;
                    v.LastReason = string.Empty;
                    continue;
                }
                v.LastRemark = remarkEntity.RemarkText ?? string.Empty;
                v.LastRemarkFull = remarkEntity.RemarkTextFull ?? string.Empty;
                v.LastReason = remarkEntity.Reason ?? string.Empty;
                v.LastRemarkFull = remarkEntity.RemarkTextFull ?? string.Empty;
                v.EstimatedResultion = remarkEntity.EstimatedResolution;

                if (remarkEntity.EstimatedResolution.Date == today)
                {
                    v.NextRentColor = "~/App.Images/warning.png";
                }
                if (remarkEntity.EstimatedResolution.Date > today)
                {
                    v.NextRentColor = "~/App.Images/accept.png";
                }
                
            }

            return localVehicleData;
        }

        internal void UpdateVehicleComment(int vehicleId, string comment)
        {
            var vehicle = DataContext.Vehicles.Single(d => d.VehicleId == vehicleId);
            vehicle.Comment = comment;
            DataContext.SubmitChanges();
        }


        internal OverviewVehicleDetails GetVehicleHistoryDetails(int vehicleId)
        {
            var vehicleDetails = from v in DataContext.Vehicles.Where(d => d.VehicleId == vehicleId)
                                 select new OverviewVehicleDetails
                                 {
                                     VehicleId = v.VehicleId,
                                     TasModelCode = v.TasModelCode,
                                     ModelDescription = v.ModelDescription,
                                     UnitNumber = v.UnitNumber ?? 0,
                                     Vin = v.Vin,
                                     LastLocation = v.LastLocationCode,
                                     LastChangeDateTime = v.LastChangeDateTime,
                                     ExpectedLocation = v.ExpectedLocationCode,
                                     ExpectedDateTime = v.ExpectedDateTime,
                                     NonRevTimeSpan = TimeSpan.Zero,
                                     OperationalStatus = v.Operational_Status.OperationalStatusCode + " : " + v.Operational_Status.OperationalStatusName,
                                     MovementType = v.Movement_Type.MovementTypeCode + " : " + v.Movement_Type.MovementTypeName,
                                     LastMilage = v.LastMilage ?? 0,
                                     LastDocumentNumber = v.LastDocumentNumber,
                                     LastDriverName = v.LastDriverName,
                                     InstallationDate = v.InstallationDate,
                                     InstallationMsoDate = v.InstallationMsoDate,
                                     GroupCharged = v.CarGroupCharged,
                                     OwningCountry = v.OwningCountry,
                                     InCountryDays = v.DaysInCountry ?? 0,
                                     NonRevDays = v.DaysSinceLastRevenueMovement ?? 0,
                                     BlockMilage = v.BlockMileage,
                                     SaleDate = v.SaleDate,
                                     BlockDate = v.BlockDate,
                                     DepreciationStatus = v.DepreciationStatus,
                                     HoldFlag1 = v.HoldFlag1,
                                     Group = v.CarGroup,
                                     OwningArea = v.OwningArea,
                                     PreviousLocationCode = v.PreviousLocationCode,
                                     DaysInBd = v.DaysInBd,
                                     DaysInMm = v.DaysInMm,
                                     LiscencePlate = v.LicensePlate,
                                     IsNonRev = v.IsNonRev,
                                     VehicleComment = v.Comment
                                 };
            var returned = vehicleDetails.First();

            var periods = from nrp in DataContext.VehicleNonRevPeriods
                          where nrp.VehicleId == vehicleId
                          select nrp;

            returned.Periods = periods.ToList();

            var periodEntries = from pe in DataContext.VehicleNonRevPeriodEntries
                                where returned.Periods.Select(d => d.VechicleNonRevPeriodId).Contains(pe.VehicleNonRevPeriodId)
                                orderby pe.LastChangeDateTime descending 
                                select new NonRevPeriodEntry
                                {
                                    PeriodEntryId = pe.VehicleNonRevPeriodEntryId,
                                    PeriodId = pe.VehicleNonRevPeriodId,
                                    VehicleId = vehicleId,
                                    OperationalStatusFull = pe.Operational_Status.OperationalStatusCode + " " + pe.Operational_Status.OperationalStatusName,
                                    MovementTypeFull = pe.Movement_Type.MovementTypeCode + " " + pe.Movement_Type.MovementTypeName,
                                    OperationalStatusCode = pe.Operational_Status.OperationalStatusCode,
                                    MovementTypeCode = pe.Movement_Type.MovementTypeCode,
                                    LastLocationCode = pe.LastLocationCode,
                                    LastChangeDateTime = pe.LastChangeDateTime,
                                    RemarksEntered = pe.VehicleNonRevPeriodEntryRemarks.Count()
                                };

            returned.PeriodEntries = periodEntries.ToList();

            var periodEntryRemarks = from rem in DataContext.VehicleNonRevPeriodEntryRemarks
                                     where returned.Periods.Select(d => d.VechicleNonRevPeriodId).Contains(rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriodId)
                                     orderby rem.TimeStamp descending 
                                     select new NonRevPeriodEntryRemark
                                     {
                                         PeriodEntryRemarkId = rem.VehicleNonRevPeriodEntryRemarkId,
                                         PeriodEntryId = rem.VehicleNonRevPeriodEntryId,
                                         PeriodId = rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriodId,
                                         VehicleId = vehicleId,
                                         RemarkId = rem.RemarkId,
                                         ReasonText = rem.NonRev_Remarks_List.RemarkText,
                                         Remark = rem.Remark,
                                         Timestamp = rem.TimeStamp,
                                         ExpectedResolutionDate = rem.ExpectedResolutionDate,
                                         UserId = rem.UserId
                                     };

            returned.PeriodEntryRemarks = periodEntryRemarks.ToList();

            return returned;
        }

        internal List<MultiVehicleReasonEntry> GetMultiRemarkVehicleEntries(List<int> vehicleIds)
        {
            var vehicleData = from v in DataContext.Vehicles.Where(d => vehicleIds.Contains(d.VehicleId))
                              select new MultiVehicleReasonEntry
                              {
                                  Country = v.OwningCountry,
                                  Group = v.ModelGroup,
                                  Model = v.ModelDescription,
                                  Vin = v.Vin,
                                  VehicleId = v.VehicleId
                              };

            var returned = vehicleData.ToList();
            return returned;
        }

         

        internal void AddRemarkToManyEntries(List<int> vehicleIds, string userId, string remarkText, int remarkId,
            DateTime expectedResolution)
        {
            var activePeriods = from v in DataContext.Vehicles
                                where vehicleIds.Contains(v.VehicleId)
                                select v.VehicleNonRevPeriods.First(d => d.Active);

            var activePeriodLatestEntries = from ap in activePeriods
                                            join entries in DataContext.VehicleNonRevPeriodEntries
                                                on ap.VechicleNonRevPeriodId equals entries.VehicleNonRevPeriodId
                                            group entries by entries.VehicleNonRevPeriodEntryId
                                                into g
                                                select g.Max(d => d.VehicleNonRevPeriodEntryId);

            foreach (var periodEntryId in activePeriodLatestEntries)
            {
                AddRemarkToPeriodEntry(periodEntryId, userId, remarkText, remarkId, expectedResolution, false);
            }
            DataContext.SubmitChanges();
        }

        internal void AddRemarkToPeriodEntry(int periodEntryId, string userId, string remarkText
            , int remarkId, DateTime expectedResolution, bool autoSubmit = true)
        {
            var remark = new VehicleNonRevPeriodEntryRemark
            {
                ExpectedResolutionDate = expectedResolution,
                VehicleNonRevPeriodEntryId = periodEntryId,
                Remark = remarkText,
                UserId = userId,
                RemarkId = remarkId,
                TimeStamp = DateTime.Now,
            };
            DataContext.VehicleNonRevPeriodEntryRemarks.InsertOnSubmit(remark);
            if (autoSubmit)
            {
                DataContext.SubmitChanges();
            }
        }

    }
}