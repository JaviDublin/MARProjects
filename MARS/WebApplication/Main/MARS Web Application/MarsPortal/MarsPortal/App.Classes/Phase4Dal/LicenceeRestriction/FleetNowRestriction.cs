using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal.LicenceeRestriction
{
    public static class FleetNowRestriction
    {
        public static IQueryable<FleetNow> RestrictVehicleQueryable(MarsDBDataContext dataContext,
            IQueryable<FleetNow> fleetNow)
        {
            var employeeId = ApplicationAuthentication.GetEmployeeId();
            var companyEntity = dataContext.MarsUsers.Single(d => d.EmployeeId == employeeId);
            if (companyEntity.CompanyTypeId == VehicleRestriction.GetIdForCompanyType(CompanyTypeEnum.Corporate, dataContext)) return fleetNow;
            var locationsWithCompany = dataContext.LOCATIONs.Where(d => d.CompanyId == companyEntity.CompanyId);

            fleetNow = from v in fleetNow
                           join l in locationsWithCompany on v.LocationId equals l.dim_Location_id
                           select v;
            return fleetNow;
        }
    }
}