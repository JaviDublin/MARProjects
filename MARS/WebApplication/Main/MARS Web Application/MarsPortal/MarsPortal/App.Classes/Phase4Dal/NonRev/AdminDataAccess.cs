using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using App.BLL.VehiclesAbroad.Models.Filters;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev.Entities.FileUploadEntities;

namespace Mars.App.Classes.Phase4Dal.NonRev
{
    public class AdminDataAccess : NonRevBaseDataAccess
    {
        public AdminDataAccess(Dictionary<DictionaryParameter, string> parameters)
            : base(parameters)
        {
            
        }

        public string UploadRows(List<UploadRow> dataToUpload, string userId)
        {
            var reasons = (from nrv in DataContext.NonRev_Remarks_Lists
                                  select new { nrv.RemarkId, nrv.RemarkText })
                                  .ToDictionary(item => item.RemarkText, item => item.RemarkId);
            
            var nonRevVehicles = (from nrv in DataContext.Vehicles
                            where nrv.IsNonRev && nrv.IsFleet && nrv.Vin != string.Empty
                            group nrv by nrv.Vin + nrv.OwningCountry
                            into groupedData
                                      select new { VehicleId = groupedData.Min(d => d.VehicleId), Identifier = groupedData.Key }
                ).ToDictionary(item => item.Identifier, item => item.VehicleId);


            var vechicleAndActivePeriods = (from pe in DataContext.VehicleNonRevPeriods
                where pe.Active
                select new {pe.VehicleId, LatestPeriod = pe.VehicleNonRevPeriodEntries.Max(d=> d.VehicleNonRevPeriodEntryId)}).ToDictionary(t=> t.VehicleId, t=> t.LatestPeriod);

            var remarksToBeInserted = new List<VehicleNonRevPeriodEntryRemark>();
            foreach (var d in dataToUpload)
            {
                try
                {
                    d.VehicleId = nonRevVehicles[d.Vin + d.OwningCountry];
                    d.ReasonId = reasons[d.ReasonName];
                    

                    //var periodEntryId = (from pe in DataContext.VehicleNonRevPeriodEntries
                    //             where pe.VehicleNonRevPeriod.Active
                    //             && pe.VehicleNonRevPeriod.Vehicle.VehicleId == d.VehicleId
                    //             select pe.VehicleNonRevPeriodEntryId).FirstOrDefault();

                    var periodEntryId = vechicleAndActivePeriods[d.VehicleId];

                    if(periodEntryId == 0) continue;

                    var remark = new VehicleNonRevPeriodEntryRemark
                                 {
                                     VehicleNonRevPeriodEntryId = periodEntryId,
                                     RemarkId = d.ReasonId,
                                     ExpectedResolutionDate = d.ParsedEstimatedResolved,
                                     Remark = d.RemarkText,
                                     TimeStamp = DateTime.Now,
                                     UserId = userId
                                 };
                    remarksToBeInserted.Add(remark);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            DataContext.VehicleNonRevPeriodEntryRemarks.InsertAllOnSubmit(remarksToBeInserted);

            DataContext.SubmitChanges();

            return "Upload Successful";
        }

        public UploadSummary FillUploadSummary(UploadSummary us)
        {
            var parsedSummary = new UploadSummary();


            var invalidDates = new List<string>();
            var invalidReasons = new List<string>();
            var invalidVins = new List<string>();
            var notUniqueVinAndOwningCountry = new List<string>();

            var nonUniqueVins =
                DataContext.Vehicles.Where(d=> d.IsFleet).GroupBy(d => d.Vin + d.OwningCountry).Where(d => d.Count() > 1).Select(d=> d.Key).ToList();

            

            var validReasonsFromDatabase =
                DataContext.NonRev_Remarks_Lists.Select(d => d.RemarkText).Distinct().ToList();

            

            var validVinsForNonRevFromDatabase = (from nrv in DataContext.Vehicles
                                  where nrv.IsNonRev && nrv.IsFleet
                                  select nrv.Vin).ToList();

            foreach (var r in us.DataToBeUploaded)
            {
                var validEntry = true;
                DateTime dt;
                if (!DateTime.TryParse(r.EstimatedResolved, out dt))
                {
                    validEntry = false;
                    us.InvalidDates++;
                    invalidDates.Add(r.EstimatedResolved);
                }

                if (!validReasonsFromDatabase.Contains(r.ReasonName))
                {
                    validEntry = false;
                    us.InvalidReasons++;
                    invalidReasons.Add(r.ReasonName);
                }

                if (!validVinsForNonRevFromDatabase.Contains(r.Vin))
                {
                    validEntry = false;
                    us.InvalidVins++;
                    invalidVins.Add(r.Vin);
                }

                if (nonUniqueVins.Contains(r.Vin + r.OwningCountry))
                {
                    validEntry = false;
                    us.NonUniqueVinAndOwningCountry++;
                    notUniqueVinAndOwningCountry.Add(string.Format("{0} - {1}",r.Vin, r.OwningCountry));
                }
                
                if (validEntry)
                {
                    r.ParsedEstimatedResolved = dt;
                    parsedSummary.DataToBeUploaded.Add(r);
                }
            }

            us.ErrorList.AppendLine(string.Format("{0} row(s) found. {1} valid Non Rev Entries to be added."
                    , us.DataToBeUploaded.Count, parsedSummary.DataToBeUploaded.Count));
            us.ErrorList.AppendLine();
            if (us.InvalidDates > 0)
            {
                us.ErrorList.AppendLine(string.Format("Invalid Dates: {0}", string.Join(",", invalidDates)));
                us.ErrorList.AppendLine();
            }

            if (us.InvalidReasons > 0)
            {
                us.ErrorList.AppendLine(string.Format("Invalid Reasons: {0}", string.Join(",", invalidReasons)));
                us.ErrorList.AppendLine();
            }
            if (us.InvalidVins > 0)
            {
                us.ErrorList.AppendLine(string.Format("Invalid Vins: {0}", string.Join(",", invalidVins)));
                us.ErrorList.AppendLine();
            }

            if (us.NonUniqueVinAndOwningCountry > 0)
            {
                us.ErrorList.AppendLine(string.Format("Non Unique Vin + Owning Country: {0}", string.Join(",", notUniqueVinAndOwningCountry)));
                us.ErrorList.AppendLine();
            }

            return parsedSummary;
        }

        public void SetReasonInactive(int reasonId)
        {
            var reason = DataContext.NonRev_Remarks_Lists.Single(d => d.RemarkId == reasonId);
            reason.isActive = false;
            DataContext.SubmitChanges();

        }

        public List<ListItem> GetReasonListItems()
        {
            var reasons = from r in DataContext.NonRev_Remarks_Lists
                where r.isActive.HasValue && r.isActive.Value
                    && r.RemarkId != 1
                select new ListItem(r.RemarkText, r.RemarkId.ToString(CultureInfo.InvariantCulture));

            var returned = reasons.ToList();
            return returned;
        }

        public List<NonRev_Remarks_List> GetReasonEntries()
        {
            var reasons = from r in DataContext.NonRev_Remarks_Lists
                          where r.isActive.HasValue 
                                //&& r.isActive.Value
                              && r.RemarkId != 1
                          select r;

            var returned = reasons.ToList();
            return returned;
        }

        public string AddNewReason(string reasonName, bool active)
        {
            var reasons = from r in DataContext.NonRev_Remarks_Lists
                          where r.isActive.HasValue && r.isActive.Value
                          && r.RemarkText == reasonName
                          select r;
            if (reasons.Any())
            {
                return "Reason Name already exists";
            }

            var newReason = new NonRev_Remarks_List
                            {
                                RemarkText = reasonName,
                                isActive = active,
                            };
            DataContext.NonRev_Remarks_Lists.InsertOnSubmit(newReason);
            DataContext.SubmitChanges();
            return string.Empty;
        }

        public NonRev_Remarks_List GetReason(int reasonId)
        {
            var reason = DataContext.NonRev_Remarks_Lists.Single(d => d.RemarkId == reasonId);
            return reason;
        }

        public void UpdateReason(int reasonId, string newReasonName, bool active)
        {
            var reason = DataContext.NonRev_Remarks_Lists.Single(d => d.RemarkId == reasonId);
            reason.RemarkText = newReasonName;
            reason.isActive = active;
            DataContext.SubmitChanges();
        }
    }
}