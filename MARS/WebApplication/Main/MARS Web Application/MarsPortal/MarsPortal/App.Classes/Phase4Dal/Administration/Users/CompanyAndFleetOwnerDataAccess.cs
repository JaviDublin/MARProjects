using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;
using Mars.App.Classes.Phase4Dal.Administration.UserEntities;

namespace Mars.App.Classes.Phase4Dal.Administration.Users
{
    public class CompanyAndFleetOwnerDataAccess : IDisposable
    {
        public readonly MarsDBDataContext DataContext;

        private const string CompanyNameAlreadyExists = "Company Name already exists";

        public CompanyAndFleetOwnerDataAccess()
        {
            DataContext = new MarsDBDataContext();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        private string SubmitDbChanges()
        {
            try
            {
                DataContext.SubmitChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return string.Empty;
        }

        public CompanyEntity GetCompanyEntity(int companyId)
        {
            var companyData = from c in DataContext.Companies
                              select new CompanyEntity
                                         {
                                             CompanyName = c.CompanyName,
                                             CompanyId = c.CompanyId,
                                             CompanyTypeId = c.CompanyTypeId,
                                             CompanyTypeName = c.CompanyType.CompanyTypeName,
                                             CountryId = c.CountryId,
                                             CountryName = c.COUNTRy.country_description
                                         };
            var returned = companyData.Single(d => d.CompanyId == companyId);
            return returned;
        }

        public bool IsCompanyCorporate(int companyId)
        {
            var companyEntity = DataContext.Companies.Single(d => d.CompanyId == companyId);
            var returned = companyEntity.CompanyType.CompanyTypeName == "Corporate";
            return returned;

        }

        

        public List<FleetOwnerEntity> GetFleetEntitiesByCompany(int companyId)
        {
            var fleetOwners = from fo in DataContext.FleetOwners
                              where fo.CompanyId == companyId
                                orderby fo.OwningAreaCode
                                select new FleetOwnerEntity
                                {
                                    FleetOwnerId = fo.FleetOwnerId,
                                    FleetOwnerName = fo.OwnerName,
                                    FleetOwnerCode = fo.OwningAreaCode
                                };
            var returned = fleetOwners.ToList();
            return returned;
        }

        public List<UserEntity> GetUserEntitiesByCompany(int companyId)
        {
            var usersInCompany = from mu in DataContext.MarsUsers
                                 where mu.CompanyId == companyId
                                 select new UserEntity
                                 {
                                     MarsUserId = mu.MarsUserId,
                                     CompanyId = mu.CompanyId,
                                     EmployeeId = mu.EmployeeId,
                                     CompanyType = mu.CompanyType.CompanyTypeName,
                                     CompanyTypeId = mu.CompanyTypeId,
                                     CompanyCountryId = mu.Company == null ? (int?)null : mu.Company.COUNTRy.CountryId,
                                     CompanyCountryName = mu.Company == null ? string.Empty : mu.Company.COUNTRy.country_description,
                                     
                                 };
            var userData = usersInCompany.ToList();

            var principal = new PrincipalContext(ContextType.Domain,
                Properties.Settings.Default.ActiveDirectoryDomain,
                Properties.Settings.Default.ADContainer);
            foreach (var ue in userData)
            {
                var userPrinciple = GetPrincipalForEmployee(ue.EmployeeId, principal) ??
                                    GetPrincipalForLogon(ue.EmployeeId, principal);     //Special case for ITDEMO accounts
                ue.RacfId = userPrinciple.SamAccountName;
                ue.UserName = userPrinciple.Name;
            }

            return userData;
        }

        private Principal GetPrincipalForEmployee(string employeeId, PrincipalContext principal = null)
        {
            var principalContext = principal ?? new PrincipalContext(ContextType.Domain,
                Properties.Settings.Default.ActiveDirectoryDomain,
                Properties.Settings.Default.ADContainer);

            var userQuery = new UserPrincipal(principalContext) { EmployeeId = employeeId };
            var principalSearcher = new PrincipalSearcher { QueryFilter = userQuery };
            var searchResults = principalSearcher.FindOne();
            return searchResults;
        }

        private Principal GetPrincipalForLogon(string racfId, PrincipalContext principal = null)
        {
            var principalContext = principal ?? new PrincipalContext(ContextType.Domain,
                Properties.Settings.Default.ActiveDirectoryDomain,
                Properties.Settings.Default.ADContainer);

            var userQuery = new UserPrincipal(principalContext) { SamAccountName = racfId };
            var principalSearcher = new PrincipalSearcher { QueryFilter = userQuery };
            var searchResults = principalSearcher.FindOne();
            return searchResults;
        }

        public List<LocationEntity> GetLocationEntitiesForCompany(int companyId)
        {
            var locations = from l in DataContext.LOCATIONs
                            where l.CompanyId.HasValue && l.CompanyId.Value == companyId
                orderby l.COUNTRy1.country_description, l.location1
                select new LocationEntity
                       {
                           LocationCode = l.location1,
                           LocationFullName = l.location_name,
                           ServedBy = l.served_by_locn
                       };
            var returned = locations.ToList();
            return returned;
        }

        public string UpdateCompany(CompanyEntity ce)
        {
            if (DoesAnotherCompanyWithThisNameExist(ce))
            {
                return CompanyNameAlreadyExists;
            }

            var companyDbEntry = DataContext.Companies.Single(d => d.CompanyId == ce.CompanyId);
            companyDbEntry.CompanyName = ce.CompanyName;
            companyDbEntry.CompanyTypeId = ce.CompanyTypeId;
            //companyDbEntry.CountryId = ce.CountryId;
            DataContext.SubmitChanges();
            return string.Empty;
        }

        public bool DoesCompanyNameAlreadyExist(string companyName)
        {
            var companyExists = DataContext.Companies.Any(d => d.CompanyName == companyName);
            return companyExists;
        }

        public bool DoesAnotherCompanyWithThisNameExist(CompanyEntity ce)
        {
            var companyExists = DataContext.Companies.Any(d => d.CompanyName == ce.CompanyName
                                                            && d.CompanyId != ce.CompanyId);
            return companyExists;
        }

        public string CreateCompany(CompanyEntity ce)
        {
            if (DoesCompanyNameAlreadyExist(ce.CompanyName))
            {
                return CompanyNameAlreadyExists;
            }
            var companyDbEntry = new Company
                                     {
                                         CompanyName = ce.CompanyName,
                                         CompanyTypeId = ce.CompanyTypeId,
                                         CountryId =  ce.CountryId
                                     };
            DataContext.Companies.InsertOnSubmit(companyDbEntry);
            DataContext.SubmitChanges();
            return string.Empty;
        }

        public List<ListItem> GetCompanyTypeListItems()
        {
            
            var companyTypes = from ct in DataContext.CompanyTypes
                               select new ListItem
                                          {
                                              Text = ct.CompanyTypeName,
                                              Value = ct.CompanyTypeId.ToString(CultureInfo.InvariantCulture)
                                          };

            var returned = companyTypes.ToList();
            return returned;
        }

        public List<FleetOwnerEntity> GetFleetOwnersForCompany(int companyId, int countryId = 0)
        {
            var countryIdOfSelectedCompany = DataContext.Companies.Single(d => d.CompanyId == companyId).CountryId;

            var assignedFleetOwners = from fo in DataContext.FleetOwners
                                      where fo.CountryId == countryIdOfSelectedCompany
                                      select fo;

            if (countryId != 0)
            {
                assignedFleetOwners = assignedFleetOwners.Where(d => d.CountryId == countryId);
            }

            var fleetOwnerDataEntityOwnerData = from fo in assignedFleetOwners
                                                orderby fo.OwningAreaCode
                                                select new FleetOwnerEntity
                                                {
                                                    FleetOwnerId = fo.FleetOwnerId,
                                                    FleetOwnerName = fo.OwningAreaCode + ": " + fo.OwnerName,
                                                    AssignedToCompanyName = fo.CompanyId.HasValue ? fo.Company.CompanyName : string.Empty,
                                                    Granted = fo.CompanyId.HasValue && fo.CompanyId.Value == companyId
                                                };

            var returned = fleetOwnerDataEntityOwnerData.ToList();
            return returned;
        }

        public bool CheckCompanyNotConnectedToUsers(int companyId)
        {
            var returned = DataContext.MarsUsers.Any(d => d.CompanyId == companyId);
            return returned;
        }

        public bool CheckCompanyNotConnectedToFleet(int companyId)
        {
            var returned = DataContext.FleetOwners.Any(d => d.CompanyId == companyId);
            return returned;
        }

        public string DeleteCompany(int companyId)
        {
            var companyToDelete = DataContext.Companies.First(d => d.CompanyId == companyId);
            DataContext.Companies.DeleteOnSubmit(companyToDelete);
            var returned = SubmitDbChanges();
            return returned;
        }

        public string CheckCompanyNotConnectedToLocations(int companyId)
        {
            var locations = DataContext.LOCATIONs.Where(d => d.CompanyId == companyId);

            string returned;
            if (locations.Any())
            {
                var sb = new StringBuilder();
                sb.Append("Company connected to Locations: ");
                var locationWwds = locations.Select(d => d.location1);
                foreach (var l in locationWwds)
                {
                    sb.Append(l + ",");
                }
                returned = sb.ToString();
            }
            else
            {
                returned = string.Empty;
            }
            return returned;
        }

        public List<ListItem> GetCompanyListItemByCountry(int countryId, int companyTypeId )
        {
            var companies = DataContext.Companies.Select(d => d);
            if (countryId != 0)
            {
                companies = companies.Where(d => d.CountryId == countryId);
            }
            if (companyTypeId != 0)
            {
                companies = companies.Where(d => d.CompanyTypeId == companyTypeId);
            }
            var companyData = from c in companies
                              orderby c.CompanyName
                              select new ListItem
                                         {
                                             Text = c.CompanyName,
                                             Value = c.CompanyId.ToString(CultureInfo.InvariantCulture)
                                         };
            var returned = companyData.ToList();
            return returned;
        }

        public List<CompanyEntity> GetCompanyEntities()
        {
            var companyData = from c in DataContext.Companies
                           orderby c.CompanyName
                              select new CompanyEntity
                           {
                               CompanyId = c.CompanyId,
                               CompanyTypeId = c.CompanyTypeId,
                               CompanyTypeName = c.CompanyType.CompanyTypeName,
                               CompanyName = c.CompanyName,
                               CountryId = c.CountryId,
                               CountryName = c.COUNTRy.country_description
                           };
            var returned = companyData.ToList();
            return returned;
        }

        /// <summary>
        /// Pass company Id 0 to revoke ownership of this fleet
        /// </summary>
        /// <param name="fleetOwnerId"></param>
        /// <param name="companyId">0 to revoke Ownership</param>
        /// <returns></returns>
        public string GrantOrRevokeFleetOwner(int fleetOwnerId, int companyId)
        {
            var fleetOwner = DataContext.FleetOwners.Single(d => d.FleetOwnerId == fleetOwnerId);
            fleetOwner.CompanyId = companyId == 0 ?  (int?) null : companyId;
            DataContext.SubmitChanges();
            return string.Empty;
        }


    }
}