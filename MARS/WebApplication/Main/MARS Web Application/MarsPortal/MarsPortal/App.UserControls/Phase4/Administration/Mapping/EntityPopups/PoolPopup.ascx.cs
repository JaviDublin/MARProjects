using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;
using Mars.App.Classes.Phase4Dal.Enumerators;


namespace Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups
{
    public partial class PoolPopup : PopupEntityUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            ToggleEnableForValidators(false);
        }

        protected void bnSavePopup_Click(object sender, EventArgs e)
        {
            var pe = new PoolEntity
            {
                Id = int.Parse(hfPoolId.Value),
                PoolName = tbPoolName.Text,
                Active = cbActive.Checked
            };

            SaveDataToDataBase(pe);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string message;
            var poolIdToDelete = int.Parse(hfPoolId.Value);
            using (var dataAccess = new MappingDeleteAndCreate())
            {
                message = dataAccess.DeletePool(poolIdToDelete);
            }
            ProcessDatabaseReply(message, UpdatePoolDeleted, AdminMappingEnum.CmsPool, lblMessage);
        }



        private void SaveDataToDataBase(PoolEntity pe)
        {
            string message;

            if (pe.Id == 0)
            {
                pe.CountryId = int.Parse(ddlCountry.SelectedValue);
                using (var dataAccess = new MappingDeleteAndCreate())
                {
                    message = dataAccess.CreateNewPool(pe);
                }
            }
            else
            {
                using (var dataAccess = new MappingSingleUpdate())
                {
                    message = dataAccess.UpdatePool(pe);
                }
            }

            ProcessDatabaseReply(message, UpdatePoolSuccess, AdminMappingEnum.CmsPool, lblMessage);
        }

        public override void ShowPopup()
        {
            mpPool.Show();
            ToggleEnableForValidators(true);
        }

        public override void SetValues(int id)
        {
            hfPoolId.Value = id.ToString(CultureInfo.InvariantCulture);

            if(id == 0)
            {
                NewEnity();
                return;
            }

            PoolEntity pe;
            using (var dataAccess = new MappingSingleSelect())
            {
                pe = dataAccess.GetPoolEnitity(id);
            }

            btnDelete.Visible = true;
            ddlCountry.Visible = false;
            
            lblCountryName.Visible = true;
            tbPoolName.Text = pe.PoolName;
            lblCountryName.Text = pe.CountryName;
            cbActive.Enabled = true;
            cbActive.Checked = pe.Active;
        }

        private void NewEnity()
        {
            var entityParameters = GetEntityParameters(AdminMappingEnum.CmsPool);
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


            tbPoolName.Text = string.Empty;
            cbActive.Checked = true;
            cbActive.Enabled = false;
        }

        private void ToggleEnableForValidators(bool enable)
        {
            rfvPoolName.Enabled = enable;
        }
    }
}