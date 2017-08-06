using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Administration.Entities;
using Mars.App.Classes.Phase4Dal.Administration.UserEntities;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal.Administration.Users
{
    public class UsersAndRolesDataAccess : IDisposable
    {
        public const string RacfIdAlreadyExists = "The selected RACF ID already exists";
        public const string UnrecognizedRacfId = "The selected RACF ID could not be found in the Membership Database";
        public const string EmployeeIdAlreadyExists = "The selected Employee is already on the system";

        protected readonly MarsDBDataContext DataContext;

        public UsersAndRolesDataAccess()
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


        public int GetCompanyIdForType(CompanyTypeEnum companyType)
        {
            int returned = 0;
            if (companyType == CompanyTypeEnum.Corporate)
            {
                var companyTypeId =
                    DataContext.CompanyTypes.FirstOrDefault(d => d.CompanyTypeName == companyType.ToString());
                if (companyTypeId == null)
                {
                    throw new InvalidOperationException("Corporate CompanyType not found");
                }
                returned = companyTypeId.CompanyTypeId;
            }
            if (companyType == CompanyTypeEnum.Licensee)
            {
                var companyTypeId =
                    DataContext.CompanyTypes.FirstOrDefault(d => d.CompanyTypeName == companyType.ToString());
                if (companyTypeId == null)
                {
                    throw new InvalidOperationException("Liscensee CompanyType not found");
                }
                returned = companyTypeId.CompanyTypeId;
            }
            return returned;
        }

        public List<ListItem> GetCompanyTypes()
        {
            var userTypes = from ut in DataContext.CompanyTypes
                            select new ListItem
                                       {
                                           Value = ut.CompanyTypeId.ToString(CultureInfo.InvariantCulture),
                                           Text = ut.CompanyTypeName
                                       };
            var returned = userTypes.ToList();
            return returned;
        }

        public string UpdateMarsUserEntry(UserEntity ue)
        {
            var marsUserEntry = DataContext.MarsUsers.Single(d => d.MarsUserId == ue.MarsUserId);
            marsUserEntry.CompanyId = ue.CompanyId;
            marsUserEntry.CompanyTypeId = ue.CompanyTypeId;

            var reutrned = SubmitDbChanges();
            return reutrned;
        }

        public string AddMarsUserEntry(UserEntity ue)
        {
            var existingMarsUser = DataContext.MarsUsers.SingleOrDefault(d => d.EmployeeId == ue.EmployeeId);
            if (existingMarsUser != null)
            {
                return EmployeeIdAlreadyExists;
            }

            var marsUser = new MarsUser
                               {
                                   EmployeeId = ue.EmployeeId,
                                   CompanyId = ue.CompanyId,
                                   CompanyTypeId = ue.CompanyTypeId
                               };

            DataContext.MarsUsers.InsertOnSubmit(marsUser);

            var returned = SubmitDbChanges();
            if (returned != string.Empty) return returned;
            var userId = marsUser.MarsUserId;
            var roleId = DataContext.MarsUserRoles.First(d => d.BaseAccess && d.CompanyTypeId == ue.CompanyTypeId).MarsUserRoleId;
            GrantUserRole(userId, roleId);
            return returned;
        }

        public List<CountrtyAdministrator> GetCountryAdmin(CompanyTypeEnum typeRequested)
        {
            
            var admins = DataContext.CountryAdmins.Select(d=> d);
            if (typeRequested != CompanyTypeEnum.Both)
            {
                var companyTypeId = GetCompanyIdForType(typeRequested);
                admins = admins.Where(d => d.CompanyTypeId == companyTypeId);
            }

            var adminEntities = from ca in admins
                
                select new CountrtyAdministrator
                       {
                           CountryAdministratorId = ca.CountryAdminId,
                           Country = ca.COUNTRy.country_description,
                           Name = ca.Name,
                           Email = ca.EmailAddress
                       };
            var returned = adminEntities.ToList();
            return returned;
        }
        
        public void ChangeUserCompanyType(int marsUserId, int companyTypeIdWanted)
        {
        

            var marsUserEntity = DataContext.MarsUsers.Single(d => d.MarsUserId == marsUserId);
            marsUserEntity.CompanyType = DataContext.CompanyTypes.Single(d=> d.CompanyTypeId == companyTypeIdWanted);
            marsUserEntity.Company = null;

            DataContext.MarsUserUserRoles.DeleteAllOnSubmit(DataContext.MarsUserUserRoles.Where(d => d.MarsUserId == marsUserId));
                
            var roleId = DataContext.MarsUserRoles.First(d => d.BaseAccess && d.CompanyTypeId == companyTypeIdWanted).MarsUserRoleId;
            GrantUserRole(marsUserId, roleId);

        }

        public List<ListItem> GetRoleListItems(int companyTypeId = 0)
        {
            var roles = DataContext.MarsUserRoles.Select(d=> d);
            if (companyTypeId != 0)
            {
                roles = roles.Where(d => d.CompanyTypeId == companyTypeId);
            }
            var returned = roles.Select(d => new ListItem(d.CompanyType.CompanyTypeName + " " +  d.Name, d.MarsUserRoleId.ToString(CultureInfo.InvariantCulture))).ToList();
            return returned;
        }

        public List<RoleEntity> GetRolesForUser(int userId)
        {
            var companyTypeId = DataContext.MarsUsers.Single(d => d.MarsUserId == userId).CompanyTypeId;

            var userData = from re in DataContext.MarsUserUserRoles
                           where re.MarsUserId == userId
                           select new RoleEntity
                           {
                               UserRoleId = re.MarsUserRoleId,
                               Granted = true
                           };

            var roleData = from re in DataContext.MarsUserRoles.Where(d=> d.CompanyTypeId == companyTypeId)
                           join ud in userData on re.MarsUserRoleId equals ud.UserRoleId into joinedData
                           from udg in joinedData.DefaultIfEmpty()
                           select new RoleEntity
                                      {
                                           RoleName = re.Name,
                                           RoleDescription = re.Description,
                                           UserRoleId = re.MarsUserRoleId,
                                           Enabled = true,
                                           Granted = udg != null,
                                           AdminRole = re.AdminAccess
                                      };
            
            var returned = roleData.ToList();

            var loggedOnEmployee = ApplicationAuthentication.GetEmployeeId();

            var userHasSysAdmin = DataContext.MarsUserUserRoles.FirstOrDefault(
                                                        d => d.MarsUser.EmployeeId == loggedOnEmployee 
                                                                && d.MarsUserRole.AdminAccess
                                                );

            if (userHasSysAdmin == null)
            {
                returned.Single(d => d.AdminRole).Enabled = false;
            }
            
            return returned;
        }

        public List<UserRoleEntity> GetAllNonAdminTypes()
        {
            var userData = from ud in DataContext.MarsUserRoles
                           where !ud.BaseAccess
                           select new UserRoleEntity
                                      {
                                          MarsUserRoleId = ud.MarsUserRoleId,
                                          ButtonText = ud.Name
                                      };

            var returned = userData.ToList();
            return returned;
        }

        public CompanyTypeEnum GetUserType(string employeeId)
        {
            var userData = DataContext.MarsUsers.SingleOrDefault(d => d.EmployeeId == employeeId);

            if (userData != null && userData.CompanyType.CompanyTypeName == CompanyTypeEnum.Corporate.ToString())
            {
                return CompanyTypeEnum.Corporate;
            }
            
            return CompanyTypeEnum.Licensee;
            
        }

        public UserEntity GetUserEntityByEmployeeId(string employeeId)
        {
            var userData = DataContext.MarsUsers.SingleOrDefault(d => d.EmployeeId == employeeId);
            if (userData == null) return null;
            var userEntity = new UserEntity
            {
                MarsUserId = userData.MarsUserId,
                CompanyId = userData.CompanyId,
                CompanyType = userData.CompanyType.CompanyTypeName,
                CompanyTypeId = userData.CompanyTypeId,
                CompanyCountryId = userData.Company == null ? (int?)null : userData.Company.COUNTRy.CountryId,
                CompanyName = userData.Company == null ? string.Empty : userData.Company.CompanyName
            };
            return userEntity;
        }

        public UserEntity GetUserEntity(int userId)
        {
            var userData = DataContext.MarsUsers.Single(d => d.MarsUserId == userId);
            var adUserData = GetPrincipalForEmployee(userData.EmployeeId);

            var userName = adUserData == null ? string.Empty : adUserData.Name;
            var racfId = adUserData == null ? string.Empty : adUserData.SamAccountName;            

            var userEntity = new UserEntity
            {
                MarsUserId = userId,
                CompanyId = userData.CompanyId,
                RacfId = racfId,
                UserName = userName,
                CompanyType = userData.CompanyType.CompanyTypeName,
                CompanyTypeId = userData.CompanyTypeId,
                CompanyCountryId = userData.Company == null ? (int?)null : userData.Company.COUNTRy.CountryId,
                CompanyName = userData.Company == null ? string.Empty : userData.Company.CompanyName
            };
            return userEntity;
        }



        private Principal GetPrincipalForEmployee(string employeeId, PrincipalContext principal = null)
        {
            //var sw = new Stopwatch();
           // sw.Start();
            var principalContext = principal ??  new PrincipalContext(ContextType.Domain,
                Properties.Settings.Default.ActiveDirectoryDomain,
                Properties.Settings.Default.ADContainer);

            var userQuery = new UserPrincipal(principalContext) { EmployeeId = employeeId };
            var principalSearcher = new PrincipalSearcher { QueryFilter = userQuery };
            var searchResults = principalSearcher.FindOne();
            //sw.Stop();
            //var ss = sw.Elapsed;
            return searchResults;
        }

        private Principal GetPrincipalForLogon(string racfId, PrincipalContext principal = null)
        {
            //var sw = new Stopwatch();
            // sw.Start();
            var principalContext = principal ?? new PrincipalContext(ContextType.Domain,
                Properties.Settings.Default.ActiveDirectoryDomain,
                Properties.Settings.Default.ADContainer);

            var userQuery = new UserPrincipal(principalContext) { SamAccountName = racfId };
            var principalSearcher = new PrincipalSearcher { QueryFilter = userQuery };
            var searchResults = principalSearcher.FindOne();
            //sw.Stop();
            //var ss = sw.Elapsed;
            return searchResults;
        }

        public List<UserEntity> GetUserEntities(int roleId = 0)
        {            
            var marsUsers = DataContext.MarsUsers.Select(d => d);

            if (roleId != 0)
            {
                marsUsers = from muur in DataContext.MarsUserUserRoles
                            join mu in marsUsers on muur.MarsUserId equals mu.MarsUserId
                            where muur.MarsUserRoleId == roleId
                            select mu;
            }

            var localMarsData = (from mu in marsUsers
                                 select new UserEntity
                                        {
                                            MarsUserId = mu.MarsUserId,
                                            CompanyId = mu.CompanyId,
                                            EmployeeId = mu.EmployeeId,
                                            JoinedRoleIds = string.Join(",", mu.MarsUserUserRoles.Select(d=> d.MarsUserRoleId)),
                                            CompanyType = mu.CompanyType.CompanyTypeName,
                                            CompanyTypeId = mu.CompanyTypeId,
                                            CompanyCountryId = mu.Company == null ? (int?)null : mu.Company.COUNTRy.CountryId,
                                            CompanyCountryName = mu.Company == null ? string.Empty : mu.Company.COUNTRy.country_description,
                                            CompanyName = mu.Company == null ? string.Empty : mu.Company.CompanyName
                                        }).ToList();

            var principal = new PrincipalContext(ContextType.Domain,
                Properties.Settings.Default.ActiveDirectoryDomain,
                Properties.Settings.Default.ADContainer);
            foreach (var ue in localMarsData)
            {

                var userPrinciple = GetPrincipalForEmployee(ue.EmployeeId, principal) ??
                                    GetPrincipalForLogon(ue.EmployeeId, principal);     //Special case for ITDEMO accounts
                ue.RacfId = userPrinciple == null ? string.Empty : userPrinciple.SamAccountName;
                ue.UserName = userPrinciple == null ? string.Empty : userPrinciple.Name;
            }
            return localMarsData;
            
        }

        public string GrantUserRole(int userId, int roleId)
        {
            var newMarsUserUserRole = new MarsUserUserRole
                                          {
                                              MarsUserId = userId,
                                              MarsUserRoleId = roleId
                                          };
            DataContext.MarsUserUserRoles.InsertOnSubmit(newMarsUserUserRole);
            DataContext.SubmitChanges();
            return string.Empty;
        }

        public string RevokeUserRole(int userId, int roleId)
        {
            var existingMarsUserUserRole =
                DataContext.MarsUserUserRoles.FirstOrDefault(d => d.MarsUserId == userId && d.MarsUserRoleId == roleId);
            if(existingMarsUserUserRole != null)
            {
                DataContext.MarsUserUserRoles.DeleteOnSubmit(existingMarsUserUserRole);
                DataContext.SubmitChanges();
            }
            return string.Empty;
        }
    }
}