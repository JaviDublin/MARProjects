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
    public partial class RegionPopup : PopupEntityUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
        }

        protected void bnSavePopup_Click(object sender, EventArgs e)
        {
            var re = new RegionEntity
            {
                Id = int.Parse(hfRegionId.Value),
                RegionName = tbRegionName.Text,
                Active = cbActive.Checked
            };
            SaveDataToDataBase(re);
        }

        private void SaveDataToDataBase(RegionEntity re)
        {
            string message;

            if (re.Id == 0)
            {
                re.CountryId = int.Parse(ddlCountry.SelectedValue);
                using (var dataAccess = new MappingDeleteAndCreate())
                {
                    message = dataAccess.CreateNewRegion(re);
                }
            }
            else
            {
                using (var dataAccess = new MappingSingleUpdate())
                {
                    message = dataAccess.UpdateRegion(re);
                }
            }


            ProcessDatabaseReply(message, UpdateRegionSuccess, AdminMappingEnum.OpsRegion, lblMessage);
            if (message == string.Empty)
            {
                ToggleEnableForValidators(false);
            }
        }

        protected void ibClose_Click(object sender, EventArgs e)
        {
            ToggleEnableForValidators(false);
        }

        public override void ShowPopup()
        {
            mpRegion.Show();
            ToggleEnableForValidators(true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string message;
            var regionIdToDelete = int.Parse(hfRegionId.Value);
            using (var dataAccess = new MappingDeleteAndCreate())
            {
                message = dataAccess.DeleteRegion(regionIdToDelete);
            }
            ProcessDatabaseReply(message, UpdateRegionDeleted, AdminMappingEnum.OpsRegion, lblMessage);
            if (message == string.Empty)
            {
                ToggleEnableForValidators(false);
            }
        }

        public override void SetValues(int id)
        {
            hfRegionId.Value = id.ToString(CultureInfo.InvariantCulture);

            if (id == 0)
            {
                NewEnity();
                return;
            }
            RegionEntity re;
            using (var dataAccess = new MappingSingleSelect())
            {
                re = dataAccess.GetRegionEntity(id);
            }
            btnDelete.Visible = true;
            ddlCountry.Visible = false;
            lblCountryName.Visible = true;

            tbRegionName.Text = re.RegionName;
            lblCountryName.Text = re.CountryName;
            cbActive.Checked = re.Active;
            cbActive.Enabled = true;
            hfRegionId.Value = id.ToString(CultureInfo.InvariantCulture);
        }

        private void NewEnity()
        {
            var entityParameters = GetEntityParameters(AdminMappingEnum.OpsRegion);
            btnDelete.Visible = false;
            ddlCountry.Visible = true;
            lblCountryName.Visible = false;
            List<ListItem> countries;

            using (var dataAccess = new MappingSingleSelect())
            {
                countries = AdminParameterDataAccess.GetAllCountryListItems(dataAccess.DataContext, null, false);
            }

            ddlCountry.Items.Clear();
            ddlCountry.Items.AddRange(countries.ToArray());
            if (entityParameters[DictionaryParameter.LocationCountry] == string.Empty)
            {
                ddlCountry.SelectedIndex = 0;
            }
            else
            {
                ddlCountry.SelectedValue = entityParameters[DictionaryParameter.LocationCountry];
            }


            tbRegionName.Text = string.Empty;
            cbActive.Checked = true;
            cbActive.Enabled = false;
        }

        private void ToggleEnableForValidators(bool enable)
        {
            rfvRegionName.Enabled = enable;
        }
    }
}