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
    public partial class CarSegmentPopup : PopupEntityUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            ToggleEnableForValidators(false);
        }

        protected void btnSavePopup_Click(object sender, EventArgs e)
        {
            var cse = new CarSegmentEntity
            {
                Id = int.Parse(hfCarSegmentId.Value),
                CarSegmentName = tbCarSegment.Text,
                Active = cbActive.Checked
            };

            SaveDataToDataBase(cse);
        }

        private void SaveDataToDataBase(CarSegmentEntity cse)
        {
            string message;

            if (cse.Id == 0)
            {
                cse.CountryId = int.Parse(ddlCountry.SelectedValue);
                using (var dataAccess = new MappingDeleteAndCreate())
                {
                    message = dataAccess.CreateNewCarSegment(cse);
                }
            }
            else
            {
                using (var dataAccess = new MappingSingleUpdate())
                {
                    message = dataAccess.UpdateCarSegment(cse);
                }
            }

            ProcessDatabaseReply(message, UpdateCarSegmentSuccess, AdminMappingEnum.CarSegment, lblMessage);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string message;
            var segmentIdToDelete = int.Parse(hfCarSegmentId.Value);
            using (var dataAccess = new MappingDeleteAndCreate())
            {
                message = dataAccess.DeleteCarSegment(segmentIdToDelete);
            }
            ProcessDatabaseReply(message, UpdateCarSegmentDeleted, AdminMappingEnum.CarSegment, lblMessage);
        }

        public override void ShowPopup()
        {
            mpCarSegment.Show();
            ToggleEnableForValidators(true);
        }

        public override void SetValues(int id)
        {
            hfCarSegmentId.Value = id.ToString(CultureInfo.InvariantCulture);

            if (id == 0)
            {
                NewEnity();
                return;
            }

            CarSegmentEntity cse;
            using (var dataAccess = new MappingSingleSelect())
            {
                cse = dataAccess.GetCarSegmentEntity(id);
            }
            btnDelete.Visible = true;
            ddlCountry.Visible = false;

            lblCountryName.Visible = true;
            tbCarSegment.Text = cse.CarSegmentName;
            lblCountryName.Text = cse.CountryName;
            cbActive.Enabled = true;
            cbActive.Checked = cse.Active;
        }

        private void NewEnity()
        {
            var entityParameters = GetEntityParameters(AdminMappingEnum.CarSegment);
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

            tbCarSegment.Text = string.Empty;
            cbActive.Checked = true;
            cbActive.Enabled = false;
        }

        private void ToggleEnableForValidators(bool enable)
        {
            rfvSegmentName.Enabled = enable;
        }
    }
}