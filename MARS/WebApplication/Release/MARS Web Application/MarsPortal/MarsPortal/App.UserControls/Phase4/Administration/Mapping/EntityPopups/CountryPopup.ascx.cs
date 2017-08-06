using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;

namespace Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups
{
    public partial class CountryPopup : PopupEntityUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            ToggleEnableForValidators(false);
        }

        protected void bnSavePopup_Click(object sender, EventArgs e)
        {
            var ce = new CountryEntity
            {
                Id = int.Parse(hfCountryId.Value),
                CountryName = tbCountryName.Text,
                CountryDw = tbCountryDw.Text,
                CountryCode = tbCountryCode.Text,
                Active = cbActive.Checked
            };

            SaveDataToDataBase(ce);
            
        }        

        private void SaveDataToDataBase(CountryEntity ce)
        {
            string message;

            if (ce.Id == 0)
            {
                using (var dataAccess = new MappingDeleteAndCreate())
                {
                    message = dataAccess.CreateNewCountry(ce);
                }
            }
            else
            {
                using (var dataAccess = new MappingSingleUpdate())
                {
                    message = dataAccess.UpdateCountry(ce);
                }
            }
            ProcessDatabaseReply(message, UpdateCountrySuccess, AdminMappingEnum.Country, lblMessage);
        }

        public override void ShowPopup()
        {
            mpCountry.Show();
            ToggleEnableForValidators(true);
        }

        public override void SetValues(int id)
        {
            hfCountryId.Value = id.ToString(CultureInfo.InvariantCulture);
            if (id == 0)
            {
                NewEnity();
                return;
            }
            CountryEntity ce;
            using (var dataAccess = new MappingSingleSelect())
            {
                ce = dataAccess.GetCountryEnitity(id);
            }

            tbCountryCode.Enabled = false;
            tbCountryCode.Text = ce.CountryCode;
            tbCountryDw.Text = ce.CountryDw;
            tbCountryName.Text = ce.CountryName;
            cbActive.Checked = ce.Active;
            cbActive.Enabled = true;
        }

        private void NewEnity()
        {
            tbCountryCode.Text = string.Empty;
            tbCountryDw.Text = string.Empty;
            tbCountryName.Text = string.Empty;

            tbCountryCode.Enabled = true;
            cbActive.Checked = true;
            cbActive.Enabled = false;
        }

        private void ToggleEnableForValidators(bool enable)
        {
            rfvCountryCode.Enabled = enable;
            rfvCountryName.Enabled = enable;
        }
    }
}