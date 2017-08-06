using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details {
    public partial class LocationDetails : System.Web.UI.UserControl {
        #region Properties and Fields
        private string _validationGroup;
        private string _location;
        private string _location_dw;
        private string _real_location_name;
        private string _location_name;
        private string _location_name_dw;
        private bool _active;
        private string _ap_dt_rr;
        private string _cal;
        private int _cms_location_group_id;
        private int _ops_area_id;
        private string _served_by_locn;
        private int _turnaround_hours;

        private string _errorMessage;

        public string Location {
            get { return _location; }
            set { _location = value; }
        }

        public string Location_DW {
            get { return _location_dw; }
            set { _location_dw = value; }
        }

        public string Real_Location_Name {
            get { return _real_location_name; }
            set { _real_location_name = value; }
        }

        public string Location_Name {
            get { return _location_name; }
            set { _location_name = value; }
        }

        public string Location_Name_DW {
            get { return _location_name_dw; }
            set { _location_name_dw = value; }
        }

        public bool Active {
            get { return _active; }
            set { _active = value; }
        }

        public string AP_DT_RR {
            get { return _ap_dt_rr; }
            set { _ap_dt_rr = value; }
        }

        public string CAL {
            get { return _cal; }
            set { _cal = value; }
        }

        public int CMS_Location_Group_Id {
            get { return _cms_location_group_id; }
            set { _cms_location_group_id = value; }
        }

        public int OPS_Area_Id {
            get { return _ops_area_id; }
            set { _ops_area_id = value; }
        }

        public string Served_By_Locn {
            get { return _served_by_locn; }
            set { _served_by_locn = value; }
        }

        public int Turnaround_Hours {
            get { return _turnaround_hours; }
            set { _turnaround_hours = value; }
        }

        public AjaxControlToolkit.ModalPopupExtender ModalExtenderMapping {
            get { return this.ModalPopupExtenderMappingDetails; }
        }
        public string ErrorMessage {
            get { return _errorMessage; }
        }
        #endregion

        #region Page Events
        public void LoadDetails() {
            _validationGroup = SessionHandler.MappingLocationValidationGroup;
            this.ButtonSave.ValidationGroup = _validationGroup;

            foreach (Control cntrl in this.PanelPopupMappingDetails.Controls) {
                if (cntrl is TextBox) {
                    TextBox txtBox = (TextBox)cntrl;
                    txtBox.ValidationGroup = _validationGroup;
                }
                if (cntrl is RequiredFieldValidator) {
                    RequiredFieldValidator reqFieldValidator = (RequiredFieldValidator)cntrl;
                    reqFieldValidator.ValidationGroup = _validationGroup;
                }
                if (cntrl is CustomValidator) {
                    CustomValidator custValidator = (CustomValidator)cntrl;
                    custValidator.ValidationGroup = _validationGroup;
                }
                if (cntrl is System.Web.UI.WebControls.DropDownList) {
                    System.Web.UI.WebControls.DropDownList ddList = (System.Web.UI.WebControls.DropDownList)cntrl;
                    ddList.ValidationGroup = _validationGroup;
                }

            }

            switch (SessionHandler.MappingLocationDefaultMode) {
                case (int)App.BLL.Mappings.Mode.Insert:
                    this.LoadInsertMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:
                    this.LoadEditMode();

                    break;
            }


        }


        protected void LoadInsertMode() {

            this.TextBoxLocation.Enabled = true;
            this.CustomValidatorLocation.Enabled = true;

            this.CheckBoxIsActive.Checked = false;

            foreach (Control cntrl in this.PanelPopupMappingDetails.Controls) {
                if (cntrl is TextBox) {
                    TextBox txtbox = (TextBox)cntrl;
                    txtbox.Text = string.Empty;
                }
            }

            this.DatabindCMSLocationGroups(true);
            this.DatabindOPSAreas(true);

            this.TextBoxLocation.Focus();


        }


        protected void LoadEditMode() {

            this.TextBoxLocation.Enabled = false;
            this.CustomValidatorLocation.Enabled = false;

            this.DatabindCMSLocationGroups(false);
            this.DatabindOPSAreas(false);

            this.TextBoxLocation.Text = _location;
            this.TextBoxLocationDW.Text = _location_dw;
            this.TextBoxLocationName.Text = _location_name;
            this.TextBoxLocationNameDW.Text = _location_name_dw;
            this.TextBoxRealLocationName.Text = _real_location_name;
            this.TextBoxServedByLocn.Text = _served_by_locn;
            this.TextBoxTurnaroundHours.Text = _turnaround_hours.ToString();
            this.TextBoxAPDTRR.Text = _ap_dt_rr;
            this.TextBoxCal.Text = _cal;
            this.CheckBoxIsActive.Checked = _active;

            this.DropDownListCMSLocationGroupCode.SelectedValue = _cms_location_group_id.ToString();
            this.DropDownListOPSAreas.SelectedValue = Convert.ToString(_ops_area_id);

            this.TextBoxLocationDW.Focus();


        }


        protected void DatabindCMSLocationGroups(bool allSelected) {
            this.DropDownListCMSLocationGroupCode.Items.Clear();
            this.DropDownListCMSLocationGroupCode.DataSource = ReportLookups.GetCMSLocationGroupsWithCMSPools();
            this.DropDownListCMSLocationGroupCode.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListCMSLocationGroupCode, allSelected);

        }


        protected void DatabindOPSAreas(bool allSelected) {

            this.DropDownListOPSAreas.Items.Clear();
            this.DropDownListOPSAreas.DataSource = ReportLookups.GetOPSAreasWithOPSRegions();
            this.DropDownListOPSAreas.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListOPSAreas, allSelected);


        }

        /// <summary>
        /// Add "ALL" item to drop down lists
        /// </summary>
        /// <param name="dropDownListOption"></param>
        /// <remarks></remarks>

        private void AddInitialValueToDropDown(System.Web.UI.WebControls.DropDownList dropDownListOption, bool selected) {
            ListItem item = new ListItem();
            item.Text = Resources.lang.MappingDropDownListInitialValue;
            item.Selected = selected;
            item.Value = "-1";
            dropDownListOption.Items.Insert(0, item);


        }
        #endregion

        #region Validation
        protected void Location_Validate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args) {

            if (args.Value.Length >= 1) {
                string location = Convert.ToString(args.Value);
                //Check if country already exists

                int result = MappingsLocations.CheckLocationExists(location);

                if (result == 0) {
                    //Does not exist continue
                    args.IsValid = true;

                }
                else {
                    //Does exist cancel
                    args.IsValid = false;
                    this.CustomValidatorLocation.Text = Resources.lang.MappingDetailsErrorMessageLocationDuplicate;
                }

            }
            else {
                //Not valid
                args.IsValid = false;
                this.CustomValidatorLocation.Text = Resources.lang.MappingDetailsErrorMessageLocation;
            }

        }


        protected void TurnaroundHours_Validate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args) {
            if (args.Value.Length >= 0) {
                //Is valid
                args.IsValid = true;
            }
            else {
                //Not valid
                args.IsValid = false;

            }

        }
        #endregion

        #region Click Events
        public event EventHandler SaveMappingDetails;


        protected void ButtonSave_Click(object sender, System.EventArgs e) {



            if (Page.IsValid) {
                string location = null;
                string location_dw = this.TextBoxLocationDW.Text;
                string real_location_name = this.TextBoxRealLocationName.Text;
                string location_name = this.TextBoxLocationName.Text;
                string location_name_dw = this.TextBoxLocationNameDW.Text;
                bool active = Convert.ToBoolean(this.CheckBoxIsActive.Checked);
                string ap_dt_rr = this.TextBoxAPDTRR.Text.Trim();
                string cal = this.TextBoxCal.Text.Trim();
                int cms_location_group_id = int.Parse(this.DropDownListCMSLocationGroupCode.SelectedValue);
                int ops_area_id = Convert.ToInt32(this.DropDownListOPSAreas.SelectedValue);
                string served_by_locn = this.TextBoxServedByLocn.Text;
                int turnaround_hours = Convert.ToInt32(this.TextBoxTurnaroundHours.Text.Trim());


                int result = -1;

                switch (SessionHandler.MappingLocationDefaultMode) {
                    case (int)App.BLL.Mappings.Mode.Insert:
                        location = this.TextBoxLocation.Text;
                        result = MappingsLocations.InsertLocation(location, location_dw, real_location_name, location_name, location_name_dw, active, ap_dt_rr, cal, cms_location_group_id, ops_area_id,
                        served_by_locn, turnaround_hours);

                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:
                        location = this.TextBoxLocation.Text;
                        result = MappingsLocations.UpdateLocation(location, location_dw, real_location_name, location_name, location_name_dw, active, ap_dt_rr, cal, cms_location_group_id, ops_area_id,
                        served_by_locn, turnaround_hours);

                        break;

                }

                if (result == 0) {
                    //Success
                    _errorMessage = Resources.lang.MessageLocationSaved;
                }
                else {
                    //Failed
                    _errorMessage = Resources.lang.ErrorMessageAdministrator;
                }

                //Raise custom event from parent page
                if (SaveMappingDetails != null) {
                    SaveMappingDetails(this, EventArgs.Empty);
                }
            }
            else {
                //Keep the modal popup form show
                this.ModalPopupExtenderMappingDetails.Show();
            }


        }
        #endregion
    }
}