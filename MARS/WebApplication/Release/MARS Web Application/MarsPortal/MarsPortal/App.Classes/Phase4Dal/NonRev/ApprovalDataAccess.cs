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
            var periodEntriesHolders = from pe in DataContext.VehicleNonRevApprovalEntries
                where pe.VehicleNonRevApprovalId == vehicleNonRevApprovalId
                select new {pe.VehicleNonRevPeriodEntryId, pe.DaysNonRevAtApproval};

            var dateApproved = DataContext.VehicleNonRevApprovals
                .Single(d => d.VehicleNonRevApprovalId == vehicleNonRevApprovalId).ApprovedOn;

            
            var periodEntries = from pe in DataContext.VehicleNonRevPeriodEntries
                                join pId in periodEntriesHolders on pe.VehicleNonRevPeriodEntryId equals pId.VehicleNonRevPeriodEntryId
                                select new OverviewGridRow
                                {
                                    Serial = pe.VehicleNonRevPeriod.Vehicle.Vin,
                                    VehicleId = pe.VehicleNonRevPeriod.VehicleId.HasValue ? pe.VehicleNonRevPeriod.VehicleId.Value : 0,
                                    NonRevDays = pId.DaysNonRevAtApproval,
                                    //LastLocationCode = pe.LastLocationCode,
                                    ModelDescription = pe.VehicleNonRevPeriod.Vehicle.ModelDescription,
                                    MovementTypeCode = pe.Movement_Type.MovementTypeCode,
                                    OperationalStatusCode = pe.Operational_Status.OperationalStatusCode,
                                    VehicleFleetTypeName = pe.VehicleFleetType.FleetTypeName,
                                    CarGroup = pe.VehicleNonRevPeriod.Vehicle.CarGroup,
                                    LicensePlate = pe.VehicleNonRevPeriod.Vehicle.LicensePlate,
                                    UnitNumber = pe.VehicleNonRevPeriod.Vehicle.UnitNumber.HasValue 
                                        ? pe.VehicleNonRevPeriod.Vehicle.UnitNumber.ToString() : string.Empty,
                                    LastChangeDateTime = pe.TimeStamp,
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
            
            foreach (var e in entryIds)
            {
                if (!e.Item1.HasValue) continue;
                
                DataContext.VehicleNonRevApprovalEntries.InsertOnSubmit(new VehicleNonRevApprovalEntry
                                                                        {
                                                                            VehicleNonRevApprovalId = approvalId,
                                                                            VehicleNonRevPeriodEntryId = e.Item1.Value,
                                                                            DaysNonRevAtApproval = e.Item2
                                                                        });
            }
            DataContext.SubmitChanges();
        }
    }
}