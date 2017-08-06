using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Dal.Administration.UserEntities;
using Mars.App.Classes.Phase4Dal.Administration.Users;
using Mars.App.Classes.BLL.ExtensionMethods;

namespace Mars.App.UserControls.Phase4.Administration.Users
{
    public partial class CompanyFleetOwnerMapping : UserControl
    {
        public const string RevokeCommand = "RevokeCommand";
        public const string GrantCommand = "GrantCommmand";

        public List<CompanyEntity> GridData
        {
            get { return (List<CompanyEntity>)Session[MarsCompanyGridData]; }
            set { Session[MarsCompanyGridData] = value; }
        }

        private int SelectedCompanyId
        {
            get { return hfSelectedCompanyId.Value == string.Empty ? 0 : int.Parse(hfSelectedCompanyId.Value); }
            set { hfSelectedCompanyId.Value = value.ToString(); }
        }

        private const string MarsCompanyGridData = "MarsCompanyGridData";
        private const string MarsCompanyGridSortDirection = "MarsCompanyGridSortDirection";
        private const string MarsCompanyGridSortColumn = "MarsCompanyGridSortColumn";

        private SortDirection OverviewSortDirection
        {
            get { return (SortDirection)Session[MarsCompanyGridSortDirection]; }
            set { Session[MarsCompanyGridSortDirection] = value; }
        }

        private PropertyInfo OverviewSortColumn
        {
            get { return Session[MarsCompanyGridSortColumn] as PropertyInfo; }
            set { Session[MarsCompanyGridSortColumn] = value; }
        }

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


        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            lblPopupMessage.Text = string.Empty;
            if(!IsPostBack)
            {
                OverviewSortDirection = SortDirection.Ascending;
                FillCountryDropdown();
                using (var dataAccess = new CompanyAndFleetOwnerDataAccess())
                {
                    FillCompanyTypeDropdownList(dataAccess);
                    GetCompanyEntities(dataAccess);
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var companiesToBind = RestrictCompanyEntitiesByParameters();
            
            pnlPager.Visible = companiesToBind != null && companiesToBind.Count != 0;
            gvCompanyGrid.DataSource = companiesToBind;
            gvCompanyGrid.DataBind();
        }
        
        private void FillCompanyTypeDropdownList(CompanyAndFleetOwnerDataAccess dataAccess)
        {
            var companyTypeEntities = dataAccess.GetCompanyTypeListItems();
            var parameterCompanyTypeEntities = dataAccess.GetCompanyTypeListItems();
            ddlCompanyType.Items.AddRange(companyTypeEntities.ToArray());
            parameterCompanyTypeEntities.Insert(0, ParameterDataAccess.EmptyItem);
            ddlParameterCompanyType.Items.AddRange(parameterCompanyTypeEntities.ToArray());
        }



        private List<CompanyEntity> RestrictCompanyEntitiesByParameters()
        {
            if (GridData == null) return null;
            var restrictedData = GridData.Select(d=> d);
            if(ddlParameterCountry.SelectedValue != string.Empty)
            {
                var countryId = int.Parse(ddlParameterCountry.SelectedValue);
                restrictedData = restrictedData.Where(d => d.CountryId == countryId);
            }
            if(tbParameterCompanyName.Text != string.Empty)
            {
                var companyNameString = tbParameterCompanyName.Text.ToLower();
                restrictedData = restrictedData.Where(d => d.CompanyName.ToLower().Contains(companyNameString));
            }
            if(ddlParameterCompanyType.SelectedValue != string.Empty)
            {
                var companyTypeId = int.Parse(ddlParameterCompanyType.SelectedValue);
                restrictedData = restrictedData.Where(d => d.CompanyTypeId == companyTypeId);
            }

            var returned = restrictedData.ToList();
            gvCompanyGrid.PageIndex = CurrentGvPage - 1;
            hfRowCount.Value = returned.Count.ToString(CultureInfo.InvariantCulture);
            lblRowCount.Text = string.Format("Total Companies: {0:##,##0}", returned.Count);
            lblPageAt.Text = string.Format("Page {0} of {1}", CurrentGvPage, (returned.Count + PageSize - 1) / PageSize);
            
            return returned;
        }

        private void FillCountryDropdown()
        {
            List<ListItem> countries;
            List<ListItem> companyCountries;
            
            using (var dataAccess = new MappingSingleSelect())
            {
                countries = AdminParameterDataAccess.GetAllCountryListItems(dataAccess.DataContext, false.ToString());
                companyCountries = AdminParameterDataAccess.GetAllCountryListItems(dataAccess.DataContext, false.ToString(), false);
            }
            ddlCompanyCountry.Items.AddRange(companyCountries.ToArray());
            ddlParameterCountry.Items.AddRange(countries.ToArray());
        }

        private void GetCompanyEntities(CompanyAndFleetOwnerDataAccess dataAccess)
        {
            GridData = dataAccess.GetCompanyEntities();
        }

        protected void btnLoadCompanies_Click(object sender, EventArgs e)
        {
            using(var dataAccess = new CompanyAndFleetOwnerDataAccess())
            {
                GetCompanyEntities(dataAccess);
            }
        }

        protected void CompanyGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            OverviewSortColumn = typeof(CompanyEntity).GetProperty(e.SortExpression);

            OverviewSortDirection = OverviewSortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            this.SortGrid(OverviewSortDirection, OverviewSortColumn, GridData);
        }

        protected void btnNewCompany_Click(object sender, EventArgs e)
        {
            tbCompanyName.Text = string.Empty;
            ddlCompanyType.Enabled = true;
            ddlCompanyCountry.Enabled = true;
            ddlCompanyCountry.SelectedIndex = 0;

            SelectedCompanyId = 0;

            mpeCompanyEdit.Show();
        }

        protected void btnSaveCompany_Click(object sender, EventArgs e)
        {
            var companyData = new CompanyEntity
                                  {
                                      CompanyId = SelectedCompanyId,
                                      CompanyName = tbCompanyName.Text,
                                      CountryId = int.Parse(ddlCompanyCountry.SelectedValue),
                                      CompanyTypeId = int.Parse(ddlCompanyType.SelectedValue),
                                  };
            
            using (var dataAccess = new CompanyAndFleetOwnerDataAccess())
            {
                
                var dataAccessMessage = SelectedCompanyId == 0 
                    ? dataAccess.CreateCompany(companyData) 
                    : dataAccess.UpdateCompany(companyData);

                if (dataAccessMessage == string.Empty)
                {
                    GetCompanyEntities(dataAccess);    
                }
                else
                {
                    lblPopupMessage.Text = dataAccessMessage;
                    mpeCompanyEdit.Show();
                }
                
            }
            
        }

        protected void bnDeletePopup_Click(object sender, EventArgs e)
        {
            using (var dataAccess = new CompanyAndFleetOwnerDataAccess())
            {
                var connectedCompanies = dataAccess.CheckCompanyNotConnectedToLocations(SelectedCompanyId);
                if (connectedCompanies != string.Empty)
                {
                    lblMessage.Text = connectedCompanies;
                    mpeCompanyEdit.Show();
                    return;
                }
                var areThereConnectedUsers = dataAccess.CheckCompanyNotConnectedToUsers(SelectedCompanyId);
                if (areThereConnectedUsers)
                {
                    lblMessage.Text = "Unable to delete this Company, there are Users Connected";
                    mpeCompanyEdit.Show();
                    return;
                }
                var areThereConnectedFleets = dataAccess.CheckCompanyNotConnectedToFleet(SelectedCompanyId);
                if (areThereConnectedFleets)
                {
                    lblMessage.Text = "Unable to delete this Company, there are Fleets Connected";
                    mpeCompanyEdit.Show();
                    return;
                }
                var deletedMessage = dataAccess.DeleteCompany(SelectedCompanyId);
                if (deletedMessage != string.Empty)
                {
                    lblMessage.Text = deletedMessage;
                    mpeCompanyEdit.Show();
                }

                GetCompanyEntities(dataAccess);
            }
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == CompanyEntity.EditUserCommand)
                {
                    var companyId = int.Parse(commandArgs.CommandArgument.ToString());
                    var companyEntity = GridData.Single(d => d.CompanyId == companyId);
                    UpdateCompany(companyEntity);
                    
                    handled = true;
                }

                if (commandArgs.CommandName == CompanyEntity.ViewUserCommand)
                {
                    var companyId = int.Parse(commandArgs.CommandArgument.ToString());
                    var companyEntity = GridData.Single(d => d.CompanyId == companyId);

                    
                    ViewComapnyDetails(companyEntity);
                    
                    handled = true;
                }
                
            }
            return handled;
        }

        private void UpdateCompany(CompanyEntity ce)
        {
            ddlCompanyType.Enabled = false;
            ddlCompanyCountry.Enabled = false;
            SelectedCompanyId = ce.CompanyId;
            tbCompanyName.Text = ce.CompanyName;
            ddlCompanyType.SelectedValue = ce.CompanyTypeId.ToString();
            ddlCompanyCountry.SelectedValue = ce.CountryId.ToString();
            mpeCompanyEdit.Show();
        }

        private void ViewComapnyDetails(CompanyEntity ce)
        {
            lblCompanyName.Text = ce.CompanyName;
            using (var dataAccess = new CompanyAndFleetOwnerDataAccess())
            {

                var usersAttachedToCompany = dataAccess.GetUserEntitiesByCompany(ce.CompanyId);
                var fleetsAttachedToCompany = dataAccess.GetFleetEntitiesByCompany(ce.CompanyId);
                var locationsAttachedToCompany = dataAccess.GetLocationEntitiesForCompany(ce.CompanyId);
                gvUserGrid.DataSource = usersAttachedToCompany;
                gvUserGrid.DataBind();
                gvFleetGrid.DataSource = fleetsAttachedToCompany;
                gvFleetGrid.DataBind();
                gvLocations.DataSource = locationsAttachedToCompany;
                gvLocations.DataBind();
            }
            mpeCompanyView.Show();
        }

        protected void ddlPageSize_SizeChange(object sender, EventArgs e)
        {
            var ddlPageSize = sender as DropDownList;
            if (ddlPageSize == null) return;
            var pageSize = int.Parse(ddlPageSize.SelectedValue);
            gvCompanyGrid.PageSize = pageSize;
            
            CurrentGvPage = 1;
        }
        protected void ResetCurrentPage(object sender, EventArgs e)
        {
            tbCompanyName.Focus();
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
            var maxPages = (totalEntities + PageSize - 1) / PageSize;
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