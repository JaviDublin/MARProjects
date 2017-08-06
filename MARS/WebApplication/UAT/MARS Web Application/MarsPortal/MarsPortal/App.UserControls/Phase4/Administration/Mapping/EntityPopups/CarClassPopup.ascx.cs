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
    public partial class CarClassPopup : PopupEntityUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            ToggleEnableForValidators(false);
        }

        protected void btnSavePopup_Click(object sender, EventArgs e)
        {
            
            var cce = new CarClassEntity
            {
                Id = int.Parse(hfCarClassId.Value),
                CarClassName = tbCarClass.Text,
                Active = cbActive.Checked
            };

            if(ddlCarSegment.Visible)
            {
                if (ddlCarSegment.SelectedValue == string.Empty) return;
                cce.CarSegmentId = int.Parse(ddlCarSegment.SelectedValue);
            }

            SaveDataToDataBase(cce);
        }

        private void SaveDataToDataBase(CarClassEntity cce)
        {
            string message;

            if (cce.Id == 0)
            {
                cce.CountryId = int.Parse(ddlCountry.SelectedValue);
                using (var dataAccess = new MappingDeleteAndCreate())
                {
                    message = dataAccess.CreateNewCarClass(cce);
                }
            }
            else
            {
                using (var dataAccess = new MappingSingleUpdate())
                {
                    message = dataAccess.UpdateCarClass(cce);
                }
            }

            ProcessDatabaseReply(message, UpdateCarClassSuccess, AdminMappingEnum.CarClass, lblMessage);
        }

        protected void ddlCountry_SelectionChanged(object sender, EventArgs e)
        {
            List<ListItem> carSegments;
            var parameters = new Dictionary<DictionaryParameter, string>();
            var countryId = int.Parse(ddlCountry.SelectedValue);
            parameters.Add(DictionaryParameter.LocationCountry, countryId.ToString());
            using (var dataAccess = new MappingSingleSelect())
            {
                carSegments = AdminParameterDataAccess.GetAdminCarSegmentListItems(parameters, dataAccess.DataContext, null, false);
            }

            ddlCarSegment.Items.Clear();

            ddlCarSegment.Items.AddRange(carSegments.ToArray());
            ShowPopup();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string message;
            var classIdToDelete = int.Parse(hfCarClassId.Value);
            using (var dataAccess = new MappingDeleteAndCreate())
            {
                message = dataAccess.DeleteCarClass(classIdToDelete);
            }
            ProcessDatabaseReply(message, UpdateCarClassDeleted, AdminMappingEnum.CarClass, lblMessage);
        }

        public override void ShowPopup()
        {
            mpCarClass.Show();
            ToggleEnableForValidators(true);
        }

        public override void SetValues(int id)
        {
            hfCarClassId.Value = id.ToString(CultureInfo.InvariantCulture);

            if (id == 0)
            {
                NewEnity();
                return;
            }

            CarClassEntity pe;
            //List<ListItem> carSegments;
            var parameters = new Dictionary<DictionaryParameter, string>();

            using (var dataAccess = new MappingSingleSelect())
            {
                pe = dataAccess.GetCarClassEntity(id);
                parameters.Add(DictionaryParameter.LocationCountry, pe.CountryId.ToString());
                //carSegments = AdminParameterDataAccess.GetAdminCarSegmentListItems(
                //    parameters, dataAccess.DataContext, string.Empty, false);
            }

            lblCountryName.Visible = true;
            ddlCountry.Visible = false;
            lblCarSegment.Visible = true;
            ddlCarSegment.Visible = false;


            btnDelete.Visible = true;
            lblCountryName.Text = pe.CountryName;
            lblCarSegment.Text = pe.CarSegmentName;
            
            tbCarClass.Text = pe.CarClassName;
            cbActive.Checked = pe.Active;
            
            //ddlCarSegment.Items.Clear();
            //ddlCarSegment.Items.AddRange(carSegments.ToArray());
            //ddlCarSegment.SelectedValue = pe.Id.ToString(CultureInfo.InvariantCulture);
        }

        private void NewEnity()
        {
            var entityParameters = GetEntityParameters(AdminMappingEnum.CarClass);
            btnDelete.Visible = false;
            ddlCountry.Visible = true;
            lblCountryName.Visible = false;
            lblCarSegment.Visible = false;
            ddlCarSegment.Visible = true;

            List<ListItem> countries, carSegments;
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
                carSegments = AdminParameterDataAccess.GetAdminCarSegmentListItems(parameters, dataAccess.DataContext, null, false);
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

            ddlCarSegment.Items.Clear();
            ddlCarSegment.Items.AddRange(carSegments.ToArray());
            if (entityParameters[DictionaryParameter.CarSegment] == string.Empty)
            {
                ddlCarSegment.SelectedIndex = -1;
            }
            else
            {
                ddlCarSegment.SelectedValue = entityParameters[DictionaryParameter.CarSegment];
            }

            tbCarClass.Text = string.Empty;
            cbActive.Checked = true;
            cbActive.Enabled = false;
        }

        private void ToggleEnableForValidators(bool enable)
        {
            rfvClassName.Enabled = enable;
        }
    }
}