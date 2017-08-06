using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Castle.Windsor.Installer;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.Classes.Phase4Dal.NonRev
{
    public class ApprovalDataAccess : NonRevBaseDataAccess
    {
        public ApprovalDataAccess(Dictionary<DictionaryParameter, string> parameters)
            : base(parameters)
        {
            
        }

        public List<VehicleNonRevApproval> GetApprovalEntries(string owningCountry, string locationCountry, DateTime monthSelected)
        {
            var vehicleApprovals = from apr in DataContext.VehicleNonRevApprovals
                                       select apr;
            if (!string.IsNullOrEmpty(owningCountry))
            {
                vehicleApprovals = vehicleApprovals.Where(d => d.OwningCountry == owningCountry);
            }

            if (!string.IsNullOrEmpty(locationCountry))
            {
                vehicleApprovals = vehicleApprovals.Where(d => d.LocationCountry == locationCountry);
            }


            var approvalList = from apr in vehicleApprovals
                where apr.ApprovedOn.Month == monthSelected.Month
                      && apr.ApprovedOn.Year == monthSelected.Year
                //orderby apr.ApprovedOn descending
                select apr;
                            //select new ListItem(apr.UserId + " " + apr.ApprovedOn + " Location: " + apr.LocationCountry
                            //    + " Owner: " + apr.OwningCountry
                            //    + " " + apr.VehiclesApproved + " Vehicles"
                            //    , apr.VehicleNonRevApprovalId.ToString(CultureInfo.InvariantCulture));

            var returned = approvalList.ToList();
            return returned;
        }

        public List<OverviewGridRow> BuildApprovalList(int vehicleNonRevApprovalId)
        {
            //var periodEntriesHolders = from pe in DataContext.VehicleNonRevApprovalEntries
            //    where pe.VehicleNonRevApprovalId == vehicleNonRevApprovalId
            //    select new {pe.VehicleNonRevPeriodEntryId, pe.DaysNonRevAtApproval};

            //var dateApproved = DataContext.VehicleNonRevApprovals
            //    .Single(d => d.VehicleNonRevApprovalId == vehicleNonRevApprovalId).ApprovedOn;


            var periodEntries = from ape in DataContext.VehicleNonRevApprovalEntries
                                where ape.VehicleNonRevApprovalId == vehicleNonRevApprovalId
                                select new OverviewGridRow
                                {
                                    Serial = ape.Vin,
                                    NonRevDays = ape.DaysNonRevAtApproval,
                                    //LastLocationCode = pe.LastLocationCode,
                                    ModelDescription = ape.ModelDescription,
                                    MovementTypeCode = ape.MovementType,
                                    OperationalStatusCode = ape.OperationalStatus,
                                    VehicleFleetTypeName = ape.FleetType,
                                    CarGroup = ape.CarGroup,
                                    LicensePlate = ape.LicencePlate,
                                    UnitNumber = ape.UnitNumber.ToString(),
                                    LastChangeDateTime = ape.ApprovedDateTime,
                                };
            var returned = periodEntries.ToList();
            return returned;
        }

        public int InsertApproval(string userId, DateTime approvalDateTime, int approvedVehicles, int minimumDaysNonRev)
        {
            var owningCountry = Parameters.ContainsKey(DictionaryParameter.OwningCountry) ? Parameters[DictionaryParameter.OwningCountry] : string.Empty;
            var locationCountry = Parameters.ContainsKey(DictionaryParameter.LocationCountry) ? Parameters[DictionaryParameter.LocationCountry] : string.Empty;
            var operationalStatusList = string.Empty;
            if (Parameters.ContainsKey(DictionaryParameter.OperationalStatuses))
            {
                if (Parameters[DictionaryParameter.OperationalStatuses] != string.Empty)
                {
                    operationalStatusList = Parameters[DictionaryParameter.OperationalStatuses];
                }
            }

            var newApproval = new VehicleNonRevApproval
                              {
                                  ApprovedOn = approvalDateTime,
                                  UserId = userId,
                                  VehiclesApproved = approvedVehicles,
                                  MinimumDaysNonRev = minimumDaysNonRev,
                                  LocationCountry = locationCountry,
                                  OwningCountry = owningCountry,
                                  OperationalStatusIds = operationalStatusList
                              };

            DataContext.VehicleNonRevApprovals.InsertOnSubmit(newApproval);

            DataContext.SubmitChanges();
            return newApproval.VehicleNonRevApprovalId;
        }

        public void InsertApprovalEntries(int approvalId, List<Tuple<int?, int>> entryIds)
        {
            var userId = Rad.Security.ApplicationAuthentication.GetGlobalId();
            
            var strJoinedIds = string.Join(",",entryIds.Where(d => d.Item1.HasValue).Select(d=> d.Item1));


            DataContext.NonRevApproveVehicles(strJoinedIds, approvalId, userId);

            //foreach (var e in entryIds.Where(d=> d.Item1.HasValue))
            //{


            //    DataContext.VehicleNonRevApprovalEntries.InsertOnSubmit(new VehicleNonRevApprovalEntry
            //                                                            {
            //                                                                VehicleNonRevApprovalId = approvalId,
            //                                                                VehicleNonRevPeriodEntryId = e.Item1.Value,
            //                                                                DaysNonRevAtApproval = e.Item2
            //                                                            });
            //}
            //DataContext.SubmitChanges();
        }
    }
}