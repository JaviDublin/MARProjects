using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal.LicenceeRestriction
{
    public static class VehicleHistoryRestriction
    {
        public static IQueryable<VehicleHistory> RestrictVehicleHistoryQueryable(MarsDBDataContext dataContext,
            IQueryable<VehicleHistory> vehicleHistory)
        {
            var employeeId = ApplicationAuthentication.GetEmployeeId();
            var companyEntity = dataContext.MarsUsers.Single(d => d.EmployeeId == employeeId);
            if (companyEntity.CompanyTypeId == VehicleRestriction.GetIdForCompanyType(CompanyTypeEnum.Corporate, dataContext)) return vehicleHistory;

            var locationsWithCompany = dataContext.LOCATIONs.Where(d => d.CompanyId == companyEntity.CompanyId).Select(d => d.dim_Location_id);

            vehicleHistory = from v in vehicleHistory
                             where locationsWithCompany.Contains(v.ExpectedLocationId.Value)
                                  || locationsWithCompany.Contains(v.LastLocationId.Value)
                             select v;
            return vehicleHistory;
        }
    }
}