using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.Utilities;
using Mars.App.Classes.DAL.MarsDataAccess;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Administration.Entities;
using Mars.App.Classes.Phase4Dal.Administration.UserEntities;
using Mars.App.Classes.Phase4Dal.Administration.Users;
using Rad.Events;
using Rad.Security;
using ResultCode = Rad.Common.ResultCode;

namespace App.Login {
    public partial class Default : Page
    {
        private const string LicenceeUserNeedsCompany = "As a Licencee user you must have an assigned Company, talk to Uwe ";
        private const string UserNotSetup = "You are not in the User Table, talk to Uwe Uwe";
        private const string NotInActiveDirectoy = "Your Windows ID is not in Active Directory. Please create an e-form (Logon ID Request – Europe, W1-70) requesting access to MARS";
        private const string InBothActiveDirectoyGroups = "Your Windows ID appears in both Corporate and Licensee Active Directory Groups. Please raise a ticket with the Helpdesk (assign to H-PC Support-Dublin) and request to be removed from the incorrect Active Directory Group. ";

        private const string LoginCountryAdminList = "LoginCountryAdminList";


        private List<CountrtyAdministrator> LicenseeCountryAdmins
        {
            get { return (List<CountrtyAdministrator>)Session[LoginCountryAdminList]; }
            set { Session[LoginCountryAdminList] = value; }
        }

        protected void Page_Load(object sender, EventArgs e) 
        {
            if (!IsPostBack)
            {
                using (var dataAccess = new UsersAndRolesDataAccess())
                {
                    LicenseeCountryAdmins = dataAccess.GetCountryAdmin(CompanyTypeEnum.Licensee);
                }
                var ddlItems = LicenseeCountryAdmins.Select(d => new ListItem(d.Country));
                ddlLicenseeCountries.Items.AddRange(ddlItems.ToArray());
                ddlLicenseeCountries.Items.Insert(0, new ListItem("Select", "" ));
        
            }


          
            PanelLogOn.DefaultLinkButton = LogOnControl.LogOnButton;
            lblMessage.Text = string.Empty;
        }

        

        protected void OnLoggedIn_Command(object sender, LogOnEventArgs e) 
        {
            if (e.ResultCode == ResultCode.Success)
            {
                
                UserActivity.LogUserLogon(e.UserName, e.GlobalId, DateTime.Now);
                var loggedOnString = string.Format("{0} {1} {2}", e.UserName, e.GlobalId, DateTime.Now.ToShortDateString());
                var redirectUrl = FormsAuthentication.GetRedirectUrl(e.UserName, e.IsPersistent);

                Session["UserLoggedOn"] = loggedOnString;
                Session["LoggedOnEmployeeId"] = e.GlobalId;

                var useAd = bool.Parse(Mars.Properties.Settings.Default.UseActiveDirectoryRoleCheck);

                if (useAd)
                {


                    var principalContext = new PrincipalContext(ContextType.Domain,
                        Mars.Properties.Settings.Default.ActiveDirectoryDomain,
                        Mars.Properties.Settings.Default.ADContainer);

                    var inLicenceeGroup = ActiveDirectory.IsUserInADGroup(e.GlobalId, principalContext,
                        Mars.Properties.Settings.Default.AdLicenseeGroup);



                    var inCorporateGroup = ActiveDirectory.IsUserInADGroup(e.GlobalId, principalContext,
                        Mars.Properties.Settings.Default.AdCorporateGroup);


                    if (inCorporateGroup == ResultCode.Success && inLicenceeGroup == ResultCode.Success)
                    {
                        lblMessage.Text = InBothActiveDirectoyGroups;
                        return;
                    }

                    var employeeId = e.EmployeeId;

                    if (employeeId == string.Empty)
                    {
                        //Special case for ITDEMO accounts that don't have employeeIds
                        employeeId = e.GlobalId;
                    }

                    if (inCorporateGroup == ResultCode.Success)
                    {
                        SetCookieExpirationForMidnight();
                        ProcessUser(employeeId, redirectUrl, CompanyTypeEnum.Corporate);

                        return;
                    }

                    if (inLicenceeGroup == ResultCode.Success)
                    {
                        SetCookieExpirationForMidnight();
                        ProcessUser(employeeId, redirectUrl, CompanyTypeEnum.Licensee);

                        return;
                    }

                    lblMessage.Text = NotInActiveDirectoy;

                }
                else
                {
                    SetCookieExpirationForMidnight();
                    Response.Redirect(redirectUrl);    
                }
                
                
            }
        }

        private void SetCookieExpirationForMidnight()
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var httpCookie = Response.Cookies[cookieName];
            if (httpCookie != null)
                httpCookie.Expires = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
        }

        private void ProcessUser(string employeeId, string redirectUrl, CompanyTypeEnum companyType)
        {
            using (var dataAccess = new UsersAndRolesDataAccess())
            {
                var companyTypeId = dataAccess.GetCompanyIdForType(companyType);
                var userEnitty = dataAccess.GetUserEntityByEmployeeId(employeeId);

                if (userEnitty == null)             //User doesn't exist
                {
                    var userEntity = new UserEntity
                    {
                        EmployeeId = employeeId,
                        CompanyTypeId = companyTypeId,
                        CompanyId = null
                    };
                    
                    var insertResult = dataAccess.AddMarsUserEntry(userEntity);
                    if (companyType == CompanyTypeEnum.Corporate)
                    {
                        if (insertResult == string.Empty)
                        {
                            Response.Redirect(redirectUrl);
                        }
                        else
                        {
                            lblMessage.Text = insertResult;
                        }
                    }
                    else            //Newly created Lisencee, needs Company
                    {
                        lblMessage.Text = NotInActiveDirectoy;
                    }
                }
                else                                //User does exist
                {
                    if (userEnitty.CompanyTypeId != companyTypeId)
                    {
                        dataAccess.ChangeUserCompanyType(userEnitty.MarsUserId, companyTypeId);
                    }

                    if (companyType == CompanyTypeEnum.Licensee && userEnitty.CompanyId == null)
                    {    
                        
                        pnlCountryAdmin.Visible = true;
                        lblMessage.Text = LicenceeUserNeedsCompany;
                        return;   
                    }

                    Response.Redirect(redirectUrl);
                }
            }
        }

        protected void ddlLicenseeCountries_SelectionChanged(object sender, EventArgs e)
        {
            var countrySelected = ddlLicenseeCountries.SelectedValue;
            if (countrySelected == string.Empty) return;
            var filteredAdmins = LicenseeCountryAdmins.Where(d => d.Country == countrySelected).ToList();

            lvLicenseeAdmins.DataSource = filteredAdmins;
            lvLicenseeAdmins.DataBind();
            
            upnlNoCompany.Update();
        }

    }
}