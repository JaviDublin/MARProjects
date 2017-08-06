using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Dal.Administration.FleetOwner;
using Mars.App.Classes.Phase4Dal.Administration.UserEntities;
using Mars.App.Classes.Phase4Dal.Administration.Users;
using Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups;

namespace Mars.App.UserControls.Phase4.Administration.Fleet
{
    public partial class FleetAdmin : UserControl
    {
        private const string MarsFleetOwnerAdminGridData = "MarsFleetOwnerAdminGridData";
        private const string MarsFleetOwnerAdminGridSortDirection = "MarsFleetOwnerAdminGridSortDirection";
        private const string MarsFleetOwnerAdminGridSortColumn = "MarsFleetOwnerAdminGridSortColumn";

        public List<FleetOwnerEntity> GridData
        {
            get { return (List<FleetOwnerEntity>)Session[MarsFleetOwnerAdminGridData]; }
            set { Session[MarsFleetOwnerAdminGridData] = value; }
        }

        private SortDirection OverviewSortDirection
        {
            get { return (SortDirection)Session[MarsFleetOwnerAdminGridSortDirection]; }
            set { Session[MarsFleetOwnerAdminGridSortDirection] = value; }
        }

        private PropertyInfo OverviewSortColumn
        {
            get { return Session[MarsFleetOwnerAdminGridSortColumn] as PropertyInfo; }
            set { Session[MarsFleetOwnerAdminGridSortColumn] = value; }
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
            if (!IsPostBack)
            {
                OverviewSortDirection = SortDirection.Descending;
                using (var dataAccess = new FleetOwnerDataAccess())
                {
                    LoadFleetData(dataAccess);    
                }
                
                FillCountryDropdown();

                var countryId = ddlParameterCountry.SelectedValue == string.Empty ? 0 : int.Parse(ddlParameterCountry.SelectedValue);
                PopulateCompanyDropdown(countryId, 0, ddlParameterCompany,true);

                
                
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var fleetsToBind = RestrictFleetByParameters();

            pnlPager.Visible = fleetsToBind != null && fleetsToBind.Count != 0;
            gvFleetGrid.DataSource = fleetsToBind;
            gvFleetGrid.DataBind();
        }



        private List<FleetOwnerEntity> RestrictFleetByParameters()
        {
            if (GridData == null) return null;
            var restrictedData = GridData.Select(d => d);
            if (ddlParameterCountry.SelectedValue != string.Empty)
            {
                var countryId = int.Parse(ddlParameterCountry.SelectedValue);
                restrictedData = restrictedData.Where(d => d.CountryId == countryId);
            }
            if (tbParameterFleetName.Text != string.Empty)
            {
                var companyNameString = tbParameterFleetName.Text.ToLower();
                restrictedData = restrictedData.Where(d => d.FleetOwnerName.ToLower().Contains(companyNameString)
                                                            || d.FleetOwnerCode.ToLower().Contains(companyNameString));
            }

            
            if (ddlParameterCompany.SelectedValue != string.Empty)
            {
                var companyId = int.Parse(ddlParameterCompany.SelectedValue);
                restrictedData = ddlParameterCompany.SelectedValue == "-1" 
                    ? restrictedData.Where(d => !d.CompanyId.HasValue) 
                    : restrictedData.Where(d => d.CompanyId.HasValue && d.CompanyId == companyId);
            }

            var returned = restrictedData.ToList();
            gvFleetGrid.PageIndex = CurrentGvPage - 1;
            hfRowCount.Value = returned.Count.ToString();
            lblRowCount.Text = string.Format("Total Fleets: {0:##,##0}", returned.Count);
            lblPageAt.Text = string.Format("Page {0} of {1}", CurrentGvPage, (returned.Count + PageSize - 1) / PageSize);
            
            return returned;
        }

        private void PopulateCompanyDropdown(int countryId, int companyTypeId, DropDownList ddl, bool includeAll = false)
        {
            LocationPopup.AddCompanyDropDownItems(countryId, companyTypeId, ddl, includeAll);
        }

        private void FillCountryDropdown()
        {
            List<ListItem> countries;
            using (var dataAccess = new MappingSingleSelect())
            {
                countries = AdminParameterDataAccess.GetAllCountryListItems(dataAccess.DataContext, false.ToString());
            }
            ddlParameterCountry.Items.AddRange(countries.ToArray());
        }

        protected void btnLoadFleets_Click(object sender, EventArgs e)
        {
            using (var dataAccess = new FleetOwnerDataAccess())
            {
                LoadFleetData(dataAccess);
            }
        }

        protected void CountryChanged(object sender, EventArgs e)
        {
            var countryId = ddlParameterCountry.SelectedValue == string.Empty ? 0 : int.Parse(ddlParameterCountry.SelectedValue);
            PopulateCompanyDropdown(countryId, 0, ddlParameterCompany, true);
            CurrentGvPage = 1;
        }

        protected void ResetCurrentPage(object sender, EventArgs e)
        {
            CurrentGvPage = 1;
        }

        private void LoadFleetData( FleetOwnerDataAccess dataAccess)
        {
            GridData = dataAccess.GetFleetOwnerEntities();
        }

        protected void FleetGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            OverviewSortColumn = typeof(FleetOwnerEntity).GetProperty(e.SortExpression);
            OverviewSortDirection = OverviewSortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            this.SortGrid(OverviewSortDirection, OverviewSortColumn, GridData);
        }

        protected void ddlPageSize_SizeChange(object sender, EventArgs e)
        {
            var ddlPageSize = sender as DropDownList;
            if (ddlPageSize == null) return;
            var pageSize = int.Parse(ddlPageSize.SelectedValue);
            gvFleetGrid.PageSize = pageSize;

            CurrentGvPage = 1;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == UserEntity.EditUserCommand)
                {
                    var fleetId = int.Parse(commandArgs.CommandArgument.ToString());

                    var companyEntitiy = GridData.Single(d => d.FleetOwnerId == fleetId);
                    UpdateFleetPopup(companyEntitiy);
                    mpeFleetUpdate.Show();

                }
                handled = true;
            }
            return handled;
        }

        protected void btnSaveFleet_Click(object sender, EventArgs e)
        {
            var fleetOwnerId = int.Parse(hfSelectedFleetId.Value);
            var companyId = ddlCompany.SelectedValue == string.Empty ? (int?) null : int.Parse(ddlCompany.SelectedValue);
            var entityToUpdate = new FleetOwnerEntity
                                 {
                                     FleetOwnerId = fleetOwnerId,
                                     CompanyId = companyId,
                                     FleetOwnerName = tbFleetName.Text,
                                 };
            using (var dataAccess = new FleetOwnerDataAccess())
            {
                dataAccess.UpdateFleetOwner(entityToUpdate);
                LoadFleetData(dataAccess);
            }
        }

        private void UpdateFleetPopup(FleetOwnerEntity foe)
        {
            lblFleetCountry.Text = foe.CountryName;
            lblFleetCode.Text = foe.FleetOwnerCode;
            tbFleetName.Text = foe.FleetOwnerName;
            hfSelectedFleetId.Value = foe.FleetOwnerId.ToString(CultureInfo.InvariantCulture);

            PopulateCompanyDropdown(foe.CountryId, 0, ddlCompany);
            if (foe.CompanyId.HasValue)
            {
                ddlCompany.SelectedValue = foe.CompanyId.ToString();
            }
            else
            {
                ddlCompany.SelectedIndex = 0;
            }
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