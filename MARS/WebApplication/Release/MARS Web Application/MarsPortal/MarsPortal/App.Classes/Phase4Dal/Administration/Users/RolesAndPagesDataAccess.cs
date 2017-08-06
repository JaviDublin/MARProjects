using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Administration.UserEntities;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal.Administration.Users
{
    public class RolesAndPagesDataAccess : IDisposable
    {
        protected readonly MarsDBDataContext DataContext;

        public RolesAndPagesDataAccess()
        {
            DataContext = new MarsDBDataContext();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        public List<RoleEntity> GetAllRoles(int employeeTypeId)
        {
            var roleData = from re in DataContext.MarsUserRoles
                           where re.CompanyTypeId == employeeTypeId
                           select new RoleEntity
                           {
                               RoleName = re.Name,
                               RoleDescription = re.Description,
                               UserRoleId = re.MarsUserRoleId
                           };

            var returned = roleData.ToList();

            return returned;
        }

        public string AssignUrlToRole(int roleId, int urlId)
        {
            var newMarsUserUserRole = new MarsUserRoleMenuAccess
            {
                MarsUserRoleId = roleId,
                UrlId = urlId
            };
            DataContext.MarsUserRoleMenuAccesses.InsertOnSubmit(newMarsUserUserRole);
            DataContext.SubmitChanges();
            return string.Empty;
        }

        public string UnassignUrlFromRole(int roleId, int urlId)
        {
            var existingPageAccess =
                DataContext.MarsUserRoleMenuAccesses.FirstOrDefault(d => d.MarsUserRoleId == roleId
                                                    && d.UrlId == urlId);
            if (existingPageAccess != null)
            {
                DataContext.MarsUserRoleMenuAccesses.DeleteOnSubmit(existingPageAccess);
                DataContext.SubmitChanges();
            }
            return string.Empty;
        }

        public List<PageEntity> GetPagesForRole(int roleId, CompanyTypeEnum companyType)
        {
            var accessData = from ma in DataContext.MarsUserRoleMenuAccesses
                             where ma.MarsUserRoleId == roleId
                             select ma;

            var baseAccess = from ma in DataContext.MarsUserRoleMenuAccesses
                             where ma.MarsUserRole.BaseAccess
                                && ma.MarsUserRole.CompanyType.CompanyTypeName == companyType.ToString()
                             select ma;


            var loggedOnEmployee = ApplicationAuthentication.GetEmployeeId();

            var userHasSysAdmin = DataContext.MarsUserUserRoles.FirstOrDefault(
                d => d.MarsUser.EmployeeId == loggedOnEmployee
                    && d.MarsUserRole.AdminAccess);

            

            var roleData = from rm in DataContext.RibbonMenus
                           join ad in accessData on rm.UrlId equals ad.UrlId into joinedData
                           from jd in joinedData.DefaultIfEmpty()
                           join ba in baseAccess on rm.UrlId equals ba.UrlId into joinedBaseData
                           from jbd in joinedBaseData.DefaultIfEmpty()
                           where rm.ParentId != null
                            && rm.Enabled.HasValue && rm.Enabled.Value
                           select new PageEntity
                           {
                               UrlId = rm.UrlId,
                               PageName = rm.Title,
                               Url = rm.Url,
                               Assigned = jd != null,
                               IsBranch = rm.ParentId == 1,
                               BaseHasAccess = jbd != null,
                               ParentId = rm.ParentId.HasValue ? rm.ParentId.Value : 0,
                               Enabled = userHasSysAdmin != null,
                               MenuId = rm.MenuId.HasValue ? rm.MenuId.Value : 0
                           };


            var localRoleData = roleData.ToList();
            var returned = GetTreeStructurePages(localRoleData);

            return returned;
        }

        private List<PageEntity> GetTreeStructurePages(List<PageEntity> unorderedPages)
        {

            
            var returned = new List<PageEntity>();
            foreach(var p in unorderedPages.Where(d=> d.ParentId == 1))
            {
                var localP = p;
                returned.Add(p);
                var subPages = unorderedPages.Where(d => d.ParentId == localP.MenuId).ToList();
                foreach(var subP in subPages)
                {
                    var localPage = subP;
                    returned.Add(localPage);
                    var subSubPages = unorderedPages.Where(d => d.ParentId == localPage.MenuId).ToList();
                    returned.AddRange(subSubPages);
                }
            }

            return returned;
        }

        

    }
}