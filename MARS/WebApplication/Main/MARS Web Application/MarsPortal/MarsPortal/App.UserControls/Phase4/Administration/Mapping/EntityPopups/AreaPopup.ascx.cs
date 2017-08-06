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
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups
{
    public partial class AreaPopup : PopupEntityUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            ToggleEnableForValidators(false);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string message;
            var areaIdToDelete = int.Parse(hfAreaId.Value);
            using (var dataAccess = new MappingDeleteAndCreate())
            {
                message = dataAccess.DeleteArea(areaIdToDelete);
            }
            ProcessDatabaseReply(message, UpdateAreaDeleted, AdminMappingEnum.OpsArea, lblMessage);
            
        }

        protected void btnSavePopup_Click(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedValue == string.Empty)
            {
                ProcessDatabaseReply("A Region must be selected", "", AdminMappingEnum.OpsArea, lblMessage);
                return;
            }
            var ae = new AreaEntity
            {
                Id = int.Parse(hfAreaId.Value),
                RegionId = int.Parse(ddlRegion.SelectedValue),
                AreaName = tbAreaName.Text,
                Active = cbActive.Checked
            };

            SaveDataToDataBase(ae);
        }

        private void SaveDataToDataBase(AreaEntity ae)
        {
            string message;

            if (ae.Id == 0)
            {
                ae.CountryId = int.Parse(ddlCountry.SelectedValue);
                using (var dataAccess = new MappingDeleteAndCreate())
                {
                    message = dataAccess.CreateNewArea(ae);
                }
            }
            else
            {
                ae.Id = int.Parse(hfAreaId.Value);

                using (var dataAccess = new MappingSingleUpdate())
                {
                    message = dataAccess.UpdateArea(ae);
                }

            }
            ProcessDatabaseReply(message, UpdateAreaSuccess, AdminMappingEnum.OpsArea, lblMessage);
        }

        public override void ShowPopup()
        {
            mpRegion.Show();
            ToggleEnableForValidators(true);
        }

        public override void SetValues(int id)
        {
            hfAreaId.Value = id.ToString(CultureInfo.InvariantCulture);

            if (id == 0)
            {
                NewEnity();
                return;
            }
            AreaEntity pe;
            List<ListItem> regions;
            var parameters = new Dictionary<DictionaryParameter, string>();

            using (var dataAccess = new MappingSingleSelect())
            {
                pe = dataAccess.GetAreaEntity(id);
                parameters.Add(DictionaryParameter.LocationCountry, pe.CountryId.ToString(CultureInfo.InvariantCulture));
                regions = AdminParameterDataAccess.GetAdminRegionListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);
            }

            btnDelete.Visible = true;
            ddlCountry.Visible = false;
            lblCountryName.Visible = true;

            lblCountryName.Text = pe.CountryName;
            tbAreaName.Text = pe.AreaName;
            cbActive.Checked = pe.Active;
            cbActive.Enabled = true;
            hfAreaId.Value = id.ToString(CultureInfo.InvariantCulture);

            ddlRegion.Items.Clear();
            ddlRegion.Items.AddRange(regions.ToArray());
            ddlRegion.SelectedValue = pe.RegionId.ToString(CultureInfo.InvariantCulture);
        }

        private void NewEnity()
        {
            var entityParameters = GetEntityParameters(AdminMappingEnum.OpsArea);
            btnDelete.Visible = false;
            ddlCountry.Visible = true;
            lblCountryName.Visible = false;
            List<ListItem> countries, regions;

            var parameters = new Dictionary<DictionaryParameter, string>();
            var selectedEntityParamCountry = entityParameters[DictionaryParameter.LocationCountry];
            

            using (var dataAccess = new MappingSingleSelect())
            {
                countries = AdminParameterDataAccess.GetAllCountryListItems(dataAccess.DataContext, null, false);
                if (selectedEntityParamCountry == string.Empty)
                {
                    selectedEntityParamCountry = countries[0].Value;
                }
                parameters.Add(DictionaryParameter.LocationCountry, selectedEntityParamCountry);
                regions = AdminParameterDataAccess.GetAdminRegionListItems(parameters, dataAccess.DataContext, null, false);
            }

            ddlCountry.Items.Clear();
            ddlCountry.Items.AddRange(countries.ToArray());            
            if (entityParameters[DictionaryParameter.LocationCountry] == string.Empty)
            {
                ddlCountry.SelectedIndex = -1;
            }
            else
            {
                ddlCountry.SelectedValue = entityParameters[DictionaryParameter.LocationCountry];
            }

            ddlRegion.Items.Clear();
            ddlRegion.Items.AddRange(regions.ToArray());

            if (entityParameters[DictionaryParameter.Region] == string.Empty)
            {
                ddlRegion.SelectedIndex = -1;
            }
            else
            {
                ddlRegion.SelectedValue = entityParameters[DictionaryParameter.Region];
            }
            

            tbAreaName.Text = string.Empty;

            cbActive.Checked = true;
            cbActive.Enabled = false;
        }

        private void ToggleEnableForValidators(bool enable)
        {
            rfvAreaName.Enabled = enable;
        }

        protected void ddlCountry_SelectionChanged(object sender, EventArgs e)
        {
            List<ListItem> regions;
            var parameters = new Dictionary<DictionaryParameter, string>();
            var countryId = int.Parse(ddlCountry.SelectedValue);
            parameters.Add(DictionaryParameter.LocationCountry, countryId.ToString());
            using (var dataAccess = new MappingSingleSelect())
            {
                regions = AdminParameterDataAccess.GetAdminRegionListItems(parameters, dataAccess.DataContext, null, false);
            }

            ddlRegion.Items.Clear();
            ddlRegion.Items.AddRange(regions.ToArray());
            ShowPopup();
        }
    }
}