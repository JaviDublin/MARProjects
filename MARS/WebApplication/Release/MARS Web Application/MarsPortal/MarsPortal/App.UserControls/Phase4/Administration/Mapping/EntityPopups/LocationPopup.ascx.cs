using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;
using Mars.App.Classes.Phase4Dal.Administration.Users;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups
{
    public partial class LocationPopup : PopupEntityUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
        }

        public static void AddCompanyDropDownItems(int countryId, int companyTypeId, DropDownList ddl, bool includeAll = false)
        {
            List<ListItem> companyItems;
            using (var dataAccess = new CompanyAndFleetOwnerDataAccess())
            {
                companyItems = dataAccess.GetCompanyListItemByCountry(countryId, companyTypeId);
            }
            ddl.Items.Clear();


            ddl.Items.AddRange(companyItems.ToArray());
            ddl.Items.Insert(0, new ListItem("None", "-1"));
            if (includeAll)
            {
                ddl.Items.Insert(0, new ListItem("All", string.Empty));
            }
        }

        protected void bnSavePopup_Click(object sender, EventArgs e)
        {
            var companyId = ddlCompany.SelectedValue == "-1" ? (int?) null : int.Parse(ddlCompany.SelectedValue);
            var le = new LocationEntity
            {
                Id = int.Parse(hfLocationId.Value),
                PoolId = int.Parse(ddlPool.SelectedValue),
                LocationGroupId = int.Parse(ddlLocationGroup.SelectedValue),
                RegionId = int.Parse(ddlRegion.SelectedValue),
                AreaId = int.Parse(ddlArea.SelectedValue),
                LocationCode = tbLocation.Text,
                LocationFullName = tbLocationName.Text,
                ServedBy = tbServedBy.Text,
                CorporateAgencyLicencee = ddlOwnerType.SelectedValue,
                AirportDowntownRailroad = ddlLocationType.SelectedValue,
                TurnaroundHours = int.Parse(ddlTurnaroundHours.SelectedValue),
                CompanyId = companyId,
                Active = cbActive.Checked
            };

            SaveDataToDataBase(le);
        }

        private void SaveDataToDataBase(LocationEntity le)
        {
            string message;

            if (le.Id == 0)
            {
                throw new NotImplementedException("Users can not create locations");
            }
            
            le.Id = int.Parse(hfLocationId.Value);
            using (var dataAccess = new MappingSingleUpdate())
            {
                message = dataAccess.UpdateLocation(le);
            }
            
            ProcessDatabaseReply(message, UpdateLocationSuccess, AdminMappingEnum.Location, lblMessage);
        }



        private void PopulateOwnerTypeListBox(bool includeCorporate, bool includeAgency, bool includeLicencee)
        {
            ddlOwnerType.Items.Clear();
            if (includeCorporate)
            {
                ddlOwnerType.Items.Add(new ListItem("Corporate", "C"));    
            }
            if (includeAgency)
            {
                ddlOwnerType.Items.Add(new ListItem("Agency", "A"));    
            }
            if (includeLicencee)
            {
                ddlOwnerType.Items.Add(new ListItem("Licencee", "L"));    
            }
        }

        public override void ShowPopup()
        {
            mpPool.Show();
        }

        public override void SetValues(int id)
        {
            LocationEntity le;
            List<ListItem> locationGroups;
            List<ListItem> areas;
            List<ListItem> pools;
            List<ListItem> regions;
            var parameters = new Dictionary<DictionaryParameter, string>();

            using (var dataAccess = new MappingSingleSelect())
            {

                le = dataAccess.GetLocationEntity(id);
                parameters.Add(DictionaryParameter.Region, le.RegionId.ToString(CultureInfo.InvariantCulture));
                parameters.Add(DictionaryParameter.Pool, le.PoolId.ToString(CultureInfo.InvariantCulture));
                parameters.Add(DictionaryParameter.LocationCountry, le.CountryId.ToString(CultureInfo.InvariantCulture));

                locationGroups = AdminParameterDataAccess.GetAdminLocationGroupListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);
                areas = AdminParameterDataAccess.GetAdminAreaListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);
                pools = AdminParameterDataAccess.GetAdminPoolListItems(parameters, dataAccess.DataContext, string.Empty,
                                                                       false);
                regions = AdminParameterDataAccess.GetAdminRegionListItems(parameters, dataAccess.DataContext, string.Empty,
                                                                       false);
            }
            acServedBy.ContextKey = le.CountryCode;
            lblCountryName.Text = le.CountryName;
            
            tbLocation.Text = le.LocationCode;
            ddlTurnaroundHours.SelectedValue = le.TurnaroundHours.ToString();
            cbActive.Checked = le.Active;
            hfLocationId.Value = id.ToString(CultureInfo.InvariantCulture);

            SetDropDownList(ddlLocationGroup, locationGroups, le.LocationGroupId);
            SetDropDownList(ddlArea, areas, le.AreaId);
            SetDropDownList(ddlPool, pools, le.PoolId);
            SetDropDownList(ddlRegion, regions, le.RegionId);
            ddlLocationType.SelectedValue = le.AirportDowntownRailroad;

            tbLocationName.Text = le.LocationFullName;
            tbServedBy.Text = le.ServedBy;
            AddCompanyDropDownItems(le.CountryId, 0, ddlCompany);


            SetCompanyTypeDropdown(le.CompanyId ?? -1, le.CorporateAgencyLicencee);
            ddlCompany.SelectedValue = le.CompanyId.HasValue ? le.CompanyId.Value.ToString(CultureInfo.InvariantCulture) : "-1";
        }

        private void SetCompanyTypeDropdown(int companyId , string selectedCal = null)
        {
            if (companyId == -1)
            {
                PopulateOwnerTypeListBox(true, true, true);
                if (selectedCal != null)
                {
                    ddlOwnerType.SelectedValue = selectedCal;    
                }
                return;
            }
            using (var dataAccess = new CompanyAndFleetOwnerDataAccess())
            {
                var isCorporateCompany = dataAccess.IsCompanyCorporate(companyId);
                if (isCorporateCompany)
                {   
                    PopulateOwnerTypeListBox(true, true, false);
                    if (selectedCal == "L")
                    {
                        ddlOwnerType.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlOwnerType.SelectedValue = selectedCal;     
                    }
                }
                else
                {
                    PopulateOwnerTypeListBox(false, false, true);
                }
            }
        }

        protected void ddlPool_SelectionChanged(object sender, EventArgs e)
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            List<ListItem> locationGroups;
            parameters.Add(DictionaryParameter.Pool, ddlPool.SelectedValue);
            using (var dataAccess = new MappingSingleSelect())
            {
                locationGroups = AdminParameterDataAccess.GetAdminLocationGroupListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);
            }
            SetDropDownList(ddlLocationGroup, locationGroups, -1);
            ShowPopup();
        }

        protected void ddlRegion_SelectionChanged(object sender, EventArgs e)
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            List<ListItem> areas;
            parameters.Add(DictionaryParameter.Region, ddlRegion.SelectedValue);
            using (var dataAccess = new MappingSingleSelect())
            {
                areas = AdminParameterDataAccess.GetAdminAreaListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);
            }
            SetDropDownList(ddlArea, areas, -1);
            ShowPopup();
        }

        protected void ddlCompany_SelectionChanged(object sender, EventArgs e)
        {
            var companyId = ddlCompany.SelectedValue == string.Empty ? -1 : int.Parse(ddlCompany.SelectedValue);
            SetCompanyTypeDropdown(companyId);
            ShowPopup();
        }
    }
}