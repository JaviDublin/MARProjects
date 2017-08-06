using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;



namespace Mars.App.Classes.DAL.MarsDataAccess
{
    public static class UserActivity
    {
        public static void LogUserLogon(string userName, string userId, DateTime loggedOnAt)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var userLogonEntry = new UserLogonHistory {TimeStamp = loggedOnAt, UserName = userName, UserId = userId};
                dataContext.UserLogonHistories.InsertOnSubmit(userLogonEntry);
                dataContext.SubmitChanges();
            }

        }

        public static void LogPageAccess(string userId, string url, string userName)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var spa = new StatisticsPageAccess()
                              {
                                  AccessedBy = userId,
                                  AccessedOn = DateTime.Now,
                                  Url = url,
                                  UserName = userName
                              };
                dataContext.StatisticsPageAccesses.InsertOnSubmit(spa);
                dataContext.SubmitChanges();
            }
        }

        public static bool Phase4UserAllowedToAccessUrl(string employeeId, string urlRequested)
        {

            if (urlRequested == "~/Default.aspx" || employeeId == string.Empty)
                return true;            //Always allowed access to the base level

            

            var returned = false;
            using (var dc = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var rolesThatCanAccessPage = from mur in dc.MarsUserRoles
                    join murma in dc.MarsUserRoleMenuAccesses on mur.MarsUserRoleId equals murma.MarsUserRoleId
                    join muur in dc.MarsUserUserRoles on mur.MarsUserRoleId equals muur.MarsUserRoleId
                            where muur.MarsUser.EmployeeId == employeeId
                    select murma.RibbonMenu.Url;

                var pageList = rolesThatCanAccessPage.ToList();
                urlRequested = "~" +urlRequested.Replace("/default.aspx", string.Empty);
                
                if (pageList.Contains(urlRequested, StringComparer.OrdinalIgnoreCase))
                {
                    returned = true;
                }
                
            }

            return returned;
        }

        public static bool UserAllowedToAccessUrl(string userId, string urlRequested)
        {
            if (urlRequested.EndsWith("UsersAndRoles.aspx"))
            {
                return true;
            }
            //if (urlRequested.EndsWith("NonRevenue/Reports/Reports.aspx") 
            //    || urlRequested.EndsWith("NonRevenue/Overview/Overview.aspx")
            //    || urlRequested.Contains("Availability")
            //    || urlRequested.EndsWith("NonRevenue/Comparison/Site/Site.aspx"))
            //{
            //    return true;
            //
            
            urlRequested = string.Format("{0}{1}", "~", urlRequested.ToLower());
            if (urlRequested == "~/default.aspx")
                return true;            //Always allowed access to the base level

            using (var dc = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var roleIds = dc.MARS_UsersInRoles.Where(d => d.userId == userId).Select(d=> d.roleId).ToList();

                var lowestRoleId = roleIds.Count == 0 ? 7 : roleIds.Min();


                var pagesAllowedToAccess = (from rrm in dc.MARS_RoleRibbonMenus
                                           join rm in dc.RibbonMenus on rrm.MenuId equals rm.MenuId
                                           where rrm.roleId == lowestRoleId && rrm.isActive == true
                                           select rm.Url.ToLower()).ToList();


                if (pagesAllowedToAccess.Contains(urlRequested) || pagesAllowedToAccess.Contains(urlRequested.Replace("/default.aspx", string.Empty)))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}