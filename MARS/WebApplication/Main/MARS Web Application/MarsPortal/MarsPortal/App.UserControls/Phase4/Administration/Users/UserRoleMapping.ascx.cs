using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.Utilities;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Dal.Administration.UserEntities;
using Mars.App.Classes.Phase4Dal.Administration.Users;
using Mars.App.Site.Administration.Users;
using System.Reflection;
using Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups;
using Rad.Security;

namespace Mars.App.UserControls.Phase4.Administration.Users
{
    public partial class UserRoleMapping : UserControl
    {
        public const string RevokeCommand = "RevokeCommand";
        public const string GrantCommand = "GrantCommmand";
        public const string NoEmployeeId = "No valid EmployeeId was selected";

        private int CurrentGvPage
        {
            get { return int.Parse(string.IsNullOrEmpty(hfCurrentGvPage.Value) ? "0" : hfCurrentGvPage.Value); }
            set { hfCurrentGvPage.Value = value.ToString(CultureInfo.InvariantCulture); }
        }

        private int PageSize
        {
            get { return int.Parse(ddlPageSize.SelectedValue); }
            set { ddlPageSize.SelectedValue = value.ToString(CultureInfo.InvariantCulture); }
        }

        private int UserTypeIdSelected
        {
            get { return ddlUserType.SelectedIndex == -1 ? 0 : int.Parse(ddlUserType.SelectedValue); }
        }

        private int UserIdSelected {
            get { return int.Parse(hfUserId.Value); }
            set { hfUserId.Value = value.ToString(); }
        }

        public List<UserEntity> GridData
        {
            get { return (List<UserEntity>)Session[MarsUserGridData]; }
            set { Session[MarsUserGridData] = value; }
        }

        private const string MarsUserGridData = "MarsUserGridData";
        private const string MarsUserGridSortDirection = "MarsUserGridSortDirection";
        private const string MarsUserGridSortColumn = "MarsUserGridSortColumn";

        private SortDirection OverviewSortDirection
        {
            get { return (SortDirection)Session[MarsUserGridSortDirection]; }
            set { Session[MarsUserGridSortDirection] = value; }
        }

        private PropertyInfo OverviewSortColumn
        {
            get { return Session[MarsUserGridSortColumn] as PropertyInfo; }
            set { Session[MarsUserGridSortColumn] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            if(!IsPostBack)
            {
                OverviewSortColumn = null;
                OverviewSortDirection = SortDirection.Descending;

                using (var dataAccess = new UsersAndRolesDataAccess())
                {
                    PopulateUserGrid(dataAccess);
                    FillUserTypes(dataAccess);
                    FillCountryDropdown(ddlCompanyCountry);
                    FillCountryDropdown(ddlParameterCompanyCountry);
                    FillRolesDropdown(dataAccess);
                    
                    var parameterCompanyCountryId = ddlParameterCompanyCountry.SelectedValue == string.Empty ? 0 : int.Parse(ddlParameterCompanyCountry.SelectedValue);
                    var parameterUserTypeId = ddlParameterUserType.SelectedValue == string.Empty ? 0 : int.Parse(ddlParameterUserType.SelectedValue);
                    PopulateCompanyDropdown(parameterCompanyCountryId, parameterUserTypeId, ddlParameterCompany, true);

                }
            }
        }

        private void FillRolesDropdown(UsersAndRolesDataAccess dataAccess)
        {
            var roleListItems = dataAccess.GetRoleListItems();
            roleListItems.Insert(0, ParameterDataAccess.EmptyItem);
            ddlRole.Items.AddRange(roleListItems.ToArray());

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var dataToBind = RestrictUserEntitiesByParameters();
            pnlPager.Visible = dataToBind != null && dataToBind.Count != 0;
            gvUserGrid.DataSource = dataToBind;
            gvUserGrid.DataBind();
        }



        private List<UserEntity> RestrictUserEntitiesByParameters()
        {
            if (GridData == null) return null;
            var textEntered = tbUserSearch.Text.ToLower();
            var restrictedData = GridData.Select(d => d);
            if(textEntered != string.Empty)
            {
                restrictedData = GridData.Where(d => d.RacfId.ToLower().Contains(textEntered) || d.UserName.ToLower().Contains(textEntered));    
            }
            
            var userTypeId = ddlParameterUserType.SelectedValue == string.Empty ? 0 : int.Parse(ddlParameterUserType.SelectedValue);
            if(userTypeId != 0)
            {
                restrictedData = restrictedData.Where(d => d.CompanyTypeId == userTypeId);
            }

            var companyCountryId = ddlParameterCompanyCountry.SelectedValue == string.Empty
                ? 0
                : int.Parse(ddlParameterCompanyCountry.SelectedValue);
            if (companyCountryId != 0)
            {
                restrictedData = restrictedData.Where(d => d.CompanyCountryId == companyCountryId);
            }

            var roleId = ddlRole.SelectedValue == string.Empty ? 0 : int.Parse(ddlRole.SelectedValue);
            if (roleId != 0)
            {
                restrictedData = restrictedData.Where(d => d.JoinedRoleIds.Split(',').Contains(roleId.ToString()));
                //restrictedData = restrictedData.Where(d => d.DomainId == roleId);
            }

            var companyId = ddlParameterCompany.SelectedValue == string.Empty ? 0 : int.Parse(ddlParameterCompany.SelectedValue);
            if (companyId == -1)
            {
                restrictedData = restrictedData.Where(d => !d.CompanyId.HasValue);   
            } else if (companyId != 0)
            {
                restrictedData = restrictedData.Where(d => d.CompanyId.HasValue && d.CompanyId == companyId);
            }

            var returned = restrictedData.ToList();
            gvUserGrid.PageIndex = CurrentGvPage - 1;
            hfRowCount.Value = returned.Count.ToString(CultureInfo.InvariantCulture);

            lblRowCount.Text = string.Format("Total Users: {0:##,##0}", returned.Count);
            lblPageAt.Text = string.Format("Page {0} of {1}", CurrentGvPage, (returned.Count + PageSize - 1) / PageSize);
            return returned;
        }

        protected void UserGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            OverviewSortColumn = typeof(UserEntity).GetProperty(e.SortExpression);

            OverviewSortDirection = OverviewSortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            this.SortGrid(OverviewSortDirection, OverviewSortColumn, GridData);
        }

        private void PopulateCompanyDropdown(int countryId, int companyTypeId, DropDownList ddl, bool includeAll = false)
        {
            LocationPopup.AddCompanyDropDownItems(countryId, companyTypeId, ddl, includeAll);
        }

        private void ShowPopup()
        {
            mpeUserUpdate.Show();
        }

        private void FillCountryDropdown(DropDownList ddl)
        {
            List<ListItem> companyCountries;

            using (var dataAccess = new MappingSingleSelect())
            {
                companyCountries = AdminParameterDataAccess.GetAllCountryListItems(dataAccess.DataContext, false.ToString(), true);
            }
            ddl.Items.AddRange(companyCountries.ToArray());
        }

        protected void ddlCompanyCountry_SelectionChanged(object sender, EventArgs e)
        {
            PopulatePopupCompany();
            ShowPopup();
        }

        private void PopulatePopupCompany()
        {
            var companyCountryId = ddlCompanyCountry.SelectedValue == string.Empty ? 0 : int.Parse(ddlCompanyCountry.SelectedValue);
            var parameterUserTypeId = ddlUserType.SelectedValue == string.Empty ? 0 : int.Parse(ddlUserType.SelectedValue);
            PopulateCompanyDropdown(companyCountryId, parameterUserTypeId, ddlCompany);
        }

        protected void ddlUserType_SelectionChanged(object sender, EventArgs e)
        {
            PopulatePopupCompany();
            ShowPopup();
        }

        protected void btnNewUser_Clicked(object sender, EventArgs e)
        {
            pnlSearchForUser.Visible = true;
            UserIdSelected = 0;

            SetUserFields(null);
            ShowPopup();
        }

        private void SetUserFields(UserEntity ue)
        {
            if (ue == null)
            {
                tbRacfId.Text = string.Empty;
                tbEmployeeName.Text = string.Empty;
                tbEmployeeId.Text = string.Empty;
                ddlUserType.SelectedIndex = 0;

                ddlCompany.SelectedIndex = 0;
            }
            else
            {
                tbRacfId.Text = ue.RacfId;
                tbEmployeeName.Text = ue.UserName;
                tbEmployeeId.Text = ue.EmployeeId;
                ddlUserType.SelectedValue = ue.CompanyTypeId.ToString(CultureInfo.InvariantCulture);

                if (ue.CompanyCountryId.HasValue)
                {
                    ddlCompanyCountry.SelectedValue = ue.CompanyCountryId.Value.ToString(CultureInfo.InvariantCulture);
                    PopulateCompanyDropdown(ue.CompanyCountryId.Value, ue.CompanyTypeId, ddlCompany);
                    ddlCompany.SelectedValue = ue.CompanyId.ToString();
                }
                else
                {
                    PopulateCompanyDropdown(0, ue.CompanyTypeId, ddlCompany);
                    ddlCompanyCountry.SelectedIndex = 0;
                    ddlCompany.SelectedIndex = 0;
                }
            }


        }

        private void SetUserDetails()
        {
            pnlSearchForUser.Visible = false;
            //var ue = dataAccess.GetUserEntity(UserIdSelected);
            var userEntity = GridData.Single(d => d.MarsUserId == UserIdSelected);

            SetUserFields(userEntity);
            
            pnlPriviliges.Visible = true;
            ShowPopup();
        }

        public void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (UserIdSelected == 0)
            {
                CreateNewUser();
            }
            else
            {
                UpdateExistingUser();
            }

        }

        protected void ddlParameterCompanyCountry_SelectionChanged(object sender, EventArgs e)
        {
            var parameterCompanyCountryId = ddlParameterCompanyCountry.SelectedValue == string.Empty ? 0 
                                            : int.Parse(ddlParameterCompanyCountry.SelectedValue);
            var parameterUserTypeId = ddlParameterUserType.SelectedValue == string.Empty ? 0 : int.Parse(ddlParameterUserType.SelectedValue);
            PopulateCompanyDropdown(parameterCompanyCountryId, parameterUserTypeId, ddlParameterCompany, true);
            CurrentGvPage = 1;
        }

        protected void btnRefreshUsersList_Click(object sender, EventArgs e)
        {
            tbUserSearch.Text = string.Empty;
            using (var dataAccess = new UsersAndRolesDataAccess())
            {
                PopulateUserGrid(dataAccess);
            }
        }

        private void PopulateUserGrid(UsersAndRolesDataAccess dataAccess)
        {
            
            GridData = dataAccess.GetUserEntities();

        }

        private void FillUserTypes(UsersAndRolesDataAccess dataAccess)
        {
            var userTypes = dataAccess.GetCompanyTypes();
            var parameterUserTypes = dataAccess.GetCompanyTypes();
            ddlUserType.Items.AddRange(userTypes.ToArray());
            parameterUserTypes.Insert(0, ParameterDataAccess.EmptyItem);
            ddlParameterUserType.Items.AddRange(parameterUserTypes.ToArray());
        }

        private void BindUserRolesRepeater(List<RoleEntity> roles)
        {
            rptRolesForUser.DataSource = roles;
            rptRolesForUser.DataBind();   
        }

        protected void tbSearchBoxItemSelected(object sender, EventArgs e)
        {
            var selectedItems = tbSearchBox.Text.Split('-');
            if (selectedItems.Count() != 3) return;
            tbRacfId.Text = selectedItems[0].Trim();
            tbEmployeeId.Text = selectedItems[1].Trim();
            tbEmployeeName.Text = selectedItems[2].Trim();
            tbSearchBox.Text = string.Empty;
            ShowPopup();
        }

        public void ChangeRoleCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e != null)
            {
                var commandEventArgs = e as CommandEventArgs;
                if (commandEventArgs.CommandName == GrantCommand || commandEventArgs.CommandName == RevokeCommand)
                {
                    var roleId = int.Parse(commandEventArgs.CommandArgument.ToString());
                    var grantCommand = commandEventArgs.CommandName == GrantCommand;

                    using (var dataAccess = new UsersAndRolesDataAccess())
                    {
                        if (grantCommand)
                        {
                            dataAccess.GrantUserRole(UserIdSelected, roleId);
                        }
                        else
                        {
                            dataAccess.RevokeUserRole(UserIdSelected, roleId);
                        }
                        var roles = dataAccess.GetRolesForUser(UserIdSelected);
                        BindUserRolesRepeater(roles);
                    }

                    mpeUserUpdate.Show();
                }
            }
        }


        private void UpdateExistingUser()
        {
            var companyId = ddlCompany.SelectedIndex == -1
                            || ddlCompany.SelectedValue == "-1" ? (int?)null : int.Parse(ddlCompany.SelectedValue);
            var userEntity = new UserEntity
            {
                MarsUserId = UserIdSelected,
                CompanyTypeId = UserTypeIdSelected,
                CompanyId = companyId
            };
            using (var dataAccess = new UsersAndRolesDataAccess())
            {
                var updateResult = dataAccess.UpdateMarsUserEntry(userEntity);
                if (updateResult != string.Empty)
                {
                    lblMessage.Text = updateResult;
                    ShowPopup();
                }
                PopulateUserGrid(dataAccess);
            }
        }

        private void CreateNewUser()
        {
            var employeeId = tbEmployeeId.Text;
            var racfId = tbRacfId.Text;
            if (employeeId == string.Empty
                && !racfId.ToLower().Contains("itdemo"))
            {
                lblMessage.Text = NoEmployeeId;
                ShowPopup();
                return;
            }

            using (var dataAccess = new UsersAndRolesDataAccess())
            {

                var companyId = ddlCompany.SelectedIndex < 1 ? (int?) null : int.Parse(ddlCompany.SelectedValue);
                var userEntity = new UserEntity
                                     {
                                         EmployeeId = employeeId == string.Empty ? racfId : employeeId,
                                         CompanyTypeId = UserTypeIdSelected,
                                         CompanyId = companyId
                                     };
                var insertResult = dataAccess.AddMarsUserEntry(userEntity);
                if (insertResult != string.Empty)
                {
                    lblMessage.Text = insertResult;
                    ShowPopup();
                    return;
                }
                
                PopulateUserGrid(dataAccess);
            }
        }

        protected void ibClose_Click(object sender, EventArgs e)
        {
            tbSearchBox.Text = string.Empty;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == UserEntity.EditUserCommand)
                {
                    var userId = int.Parse(commandArgs.CommandArgument.ToString());
                    UserIdSelected = userId;
                    using (var dataAccess = new UsersAndRolesDataAccess())
                    {
                        var roles = dataAccess.GetRolesForUser(userId);
                        BindUserRolesRepeater(roles);
                        SetUserDetails();
                    }
                   
                }
                    handled = true;
            }
            return handled;
        }



        protected void ddlPageSize_SizeChange(object sender, EventArgs e)
        {
            var ddlPageSize = sender as DropDownList;
            if (ddlPageSize == null) return;
            var pageSize = int.Parse(ddlPageSize.SelectedValue);
            gvUserGrid.PageSize = pageSize;

            CurrentGvPage = 1;
        }

        protected void ResetCurrentPage(object sender, EventArgs e)
        {
            CurrentGvPage = 1;
        }
        protected void gvFirstButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage = 1;

        }

        protected void gvPreviousButton_Click(object sender, EventArgs e)
        {
            if (CurrentGvPage != 1)
            {
                CurrentGvPage--;
            }
        }

        protected void gvNextButton_Click(object sender, EventArgs e)
        {
            var totalEntities = int.Parse(hfRowCount.Value);
            var maxPages = (totalEntities + PageSize - 1)/PageSize;
            if (CurrentGvPage < maxPages)
            {
                CurrentGvPage++;
            }
        }

        protected void gvLastButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage = (GridData.Count() + PageSize - 1) / PageSize;
        }

    }
}