using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups
{
    public partial class LocationGroupPopup : PopupEntityUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            ToggleEnableForValidators(false);
        }

        protected void btnSavePopup_Click(object sender, EventArgs e)
        {
            if (ddlPool.SelectedValue == string.Empty)
            {
                ProcessDatabaseReply("A Pool must be selected", "", AdminMappingEnum.CmsLocationGroup, lblMessage);
                return;
            }
            var lge = new LocationGroupEntity
            {                
                Id = int.Parse(hfLocationGroupId.Value),
                PoolId = int.Parse(ddlPool.SelectedValue),
                CountryId = int.Parse(ddlCountry.SelectedValue),
                LocationGroupName = tbLocationGroup.Text,
                Active = cbActive.Checked
            };

            SaveDataToDataBase(lge);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string message;
            var locationGroupIdToDelete = int.Parse(hfLocationGroupId.Value);
            using (var dataAccess = new MappingDeleteAndCreate())
            {
                message = dataAccess.DeleteLocationGroup(locationGroupIdToDelete);
            }
            ProcessDatabaseReply(message, UpdateLocationGroupDeleted, AdminMappingEnum.CmsLocationGroup, lblMessage);
        }

        private void SaveDataToDataBase(LocationGroupEntity lge)
        {
            string message;

            if (lge.Id == 0)
            {
                //lge.CountryId = int.Parse(ddlCountry.SelectedValue);
                using (var dataAccess = new MappingDeleteAndCreate())
                {
                   message = dataAccess.CreateNewLocationGroup(lge);
                }
            }
            else
            {
                lge.Id = int.Parse(hfLocationGroupId.Value);
                using (var dataAccess = new MappingSingleUpdate())
                {
                    message = dataAccess.UpdateLocationGroup(lge);
                }
            }
            ProcessDatabaseReply(message, UpdateLocationGroupSuccess, AdminMappingEnum.CmsLocationGroup, lblMessage);
        }

        public override void ShowPopup()
        {
            mpLocationGroup.Show();
            ToggleEnableForValidators(true);
        }

        private void ToggleEnableForValidators(bool enable)
        {
            rfvLocationGroup.Enabled = enable;
        }

        public override void SetValues(int id)
        {
            hfLocationGroupId.Value = id.ToString(CultureInfo.InvariantCulture);
            
            if (id == 0)
            {
                NewEnity();
                return;
            }
            LocationGroupEntity lge;
            List<ListItem> pools;
            var parameters = new Dictionary<DictionaryParameter, string>();

            using (var dataAccess = new MappingSingleSelect())
            {
                lge = dataAccess.GetLocationGroupEnitity(id);
                parameters.Add(DictionaryParameter.LocationCountry, lge.CountryId.ToString(CultureInfo.InvariantCulture));
                pools = AdminParameterDataAccess.GetAdminPoolListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);
            }

            btnDelete.Visible = true;
            tbLocationGroup.Text = lge.LocationGroupName;
            
            lblCountryName.Text = lge.CountryName;
            cbActive.Checked = lge.Active;
            cbActive.Enabled = true;

            ddlPool.Items.Clear();
            ddlPool.Items.AddRange(pools.ToArray());
            ddlPool.SelectedValue = lge.PoolId.ToString(CultureInfo.InvariantCulture);
        }

        private void NewEnity()
        {
            var entityParameters = GetEntityParameters(AdminMappingEnum.CmsLocationGroup);
            btnDelete.Visible = false;
            ddlCountry.Visible = true;
            lblCountryName.Visible = false;
            List<ListItem> countries, pools;

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
                pools = AdminParameterDataAccess.GetAdminPoolListItems(parameters, dataAccess.DataContext, null, false);
            }

            ddlCountry.Items.Clear();
            ddlCountry.Items.AddRange(countries.ToArray());
            if(entityParameters[DictionaryParameter.LocationCountry] == string.Empty)
            {
                ddlCountry.SelectedIndex = -1;
            }
            else
            {
                ddlCountry.SelectedValue = entityParameters[DictionaryParameter.LocationCountry];    
            }
            
            ddlPool.Items.Clear();
            ddlPool.Items.AddRange(pools.ToArray());
            if (entityParameters[DictionaryParameter.Pool] == string.Empty)
            {
                ddlPool.SelectedIndex = -1;
            }
            else
            {
                ddlPool.SelectedValue = entityParameters[DictionaryParameter.Pool];
            }

            tbLocationGroup.Text = string.Empty;
            cbActive.Checked = true;
            cbActive.Enabled = false;
        }

        protected void ddlCountry_SelectionChanged(object sender, EventArgs e)
        {
            List<ListItem> pools;
            var parameters = new Dictionary<DictionaryParameter, string>();
            var countryId = int.Parse(ddlCountry.SelectedValue);
            parameters.Add(DictionaryParameter.LocationCountry, countryId.ToString());
            using (var dataAccess = new MappingSingleSelect())
            {
                pools = AdminParameterDataAccess.GetAdminPoolListItems(parameters, dataAccess.DataContext, null, false);
            }
            
            ddlPool.Items.Clear();
            
            ddlPool.Items.AddRange(pools.ToArray());
            ShowPopup();
        }

    }
}