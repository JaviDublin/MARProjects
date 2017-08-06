using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal.LicenceeRestriction
{
    public static class ReservationRestriction
    {
        public static IQueryable<Reservation> RestrictVehicleQueryable(MarsDBDataContext dataContext
                    , IQueryable<Reservation> reservations)
        {
            var employeeId = ApplicationAuthentication.GetEmployeeId();
            var companyEntity = dataContext.MarsUsers.Single(d => d.EmployeeId == employeeId);
            if (companyEntity.CompanyTypeId == VehicleRestriction.GetIdForCompanyType(CompanyTypeEnum.Corporate, dataContext)) 
                return reservations;
            var locationsWithCompany = dataContext.LOCATIONs.Where(d => d.CompanyId == companyEntity.CompanyId).Select(d => d.dim_Location_id);

            reservations = from v in reservations
                      where locationsWithCompany.Contains(v.PickupLocationId)
                           || locationsWithCompany.Contains(v.ReturnLocationId)
                      select v;
            return reservations;
        }
    }
}