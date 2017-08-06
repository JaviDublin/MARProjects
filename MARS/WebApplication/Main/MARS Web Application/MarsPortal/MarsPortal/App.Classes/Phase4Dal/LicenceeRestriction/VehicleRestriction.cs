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
    public static class VehicleRestriction
    {
        //public const int CorporateId = 1;

        public static int GetIdForCompanyType(CompanyTypeEnum typeRequested, MarsDBDataContext dataContext = null)
        {
            if (dataContext == null)
            {
                using (var dc = new MarsDBDataContext())
                {
                    var companyTypeId = dc.CompanyTypes.FirstOrDefault(d => d.CompanyTypeName == typeRequested.ToString());
                    if (companyTypeId == null)
                    {
                        throw new InvalidOperationException("Invalid CompanyType, change Enum CompanyTypeId to match Database table");
                    }
                    return companyTypeId.CompanyTypeId;
                }
            }
            else
            {
                var companyTypeId = dataContext.CompanyTypes.FirstOrDefault(d => d.CompanyTypeName == typeRequested.ToString());
                if (companyTypeId == null)
                {
                    throw new InvalidOperationException("Invalid CompanyType, change Enum CompanyTypeId to match Database table");
                }
                return companyTypeId.CompanyTypeId;
            }
            
            

        }
        public static IQueryable<Vehicle> RestrictVehicleQueryable(MarsDBDataContext dataContext, IQueryable<Vehicle> vehicles )
        {
            var employeeId = ApplicationAuthentication.GetEmployeeId();
            var companyEntity = dataContext.MarsUsers.Single(d => d.EmployeeId == employeeId);
            if (companyEntity.CompanyTypeId == GetIdForCompanyType(CompanyTypeEnum.Corporate, dataContext)) return vehicles;

            var locationsWithCompany = dataContext.LOCATIONs.Where(d => d.CompanyId == companyEntity.CompanyId).Select(d => d.dim_Location_id);
            var locationsCodesWithCompany = dataContext.LOCATIONs.Where(d => d.CompanyId == companyEntity.CompanyId).Select(d => d.location1);

            vehicles = from v in vehicles
                            join f in dataContext.FleetOwners on v.OwningArea equals f.OwningAreaCode
                            where f.CompanyId == companyEntity.CompanyId
                                    || locationsWithCompany.Contains(v.LastLocationId.Value)
                                    || locationsCodesWithCompany.Contains(v.ExpectedLocationCode)
                            select v;


            return vehicles;
        }

    }
}