using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details {
    public partial class AreaCodeDetails : System.Web.UI.UserControl 
    {
        #region Properties and Fields
        private string _validationGroup;
        private string _ownarea;
        private string _country;
        private string _area_name;
        private bool _opco;
        private bool _fleetco;
        private bool _carsales;
        private bool _licensee;

        private string _errorMessage;
        public string OwnArea {
            get { return _ownarea; }
            set { _ownarea = value; }
        }

        public string Country {
            get { return _country; }
            set { _country = value; }
        }

        public string Area_Name {
            get { return _area_name; }
            set { _area_name = value; }
        }

        public bool OPCO {
            get { return _opco; }
            set { _opco = value; }
        }

        public bool FleetCo {
            get { return _fleetco; }
            set { _fleetco = value; }
        }

        public bool CarSales {
            get { return _carsales; }
            set { _carsales = value; }
        }

        public bool Licensee {
            get { return _licensee; }
            set { _licensee = value; }
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
            //Set Validation group
            _validationGroup = SessionHandler.MappingAreaCodeValidationGroup;

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

            switch (SessionHandler.MappingAreaCodeDefaultMode) {
                case (int)App.BLL.Mappings.Mode.Insert:

                    this.LoadInsertMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:

                    this.LoadEditMode();

                    break;
            }


        }


        protected void LoadInsertMode() {



            foreach (Control cntrl in this.PanelPopupMappingDetails.Controls) {
                if (cntrl is CheckBox) {
                    CheckBox chkBox = (CheckBox)cntrl;
                    chkBox.Checked = false;
                }
            }
            this.TextBoxOwnArea.Enabled = true;
            this.CustomValidatorOwnArea.Enabled = true;
            this.TextBoxOwnArea.Focus();
            this.TextBoxOwnArea.Text = string.Empty;
            this.TexBoxAreaName.Text = string.Empty;



            this.DatabindCountries(true);


        }


        protected void LoadEditMode() {

            this.TextBoxOwnArea.Enabled = false;
            this.CustomValidatorOwnArea.Enabled = false;
            this.DatabindCountries(false);
            this.TextBoxOwnArea.Text = _ownarea;
            this.TexBoxAreaName.Text = _area_name;
            this.CheckBoxCarSales.Checked = _carsales;
            this.CheckBoxFleetCo.Checked = _fleetco;
            this.CheckBoxLicensee.Checked = _licensee;
            this.CheckBoxOPCO.Checked = _opco;
            this.DropDownListCountries.SelectedValue = _country;
            this.TexBoxAreaName.Focus();


        }


        protected void DatabindCountries(bool allSelected) {
            this.DropDownListCountries.Items.Clear();
            this.DropDownListCountries.DataSource = ReportLookups.GetCountriesAll();
            this.DropDownListCountries.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListCountries, allSelected);

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
        protected void OwnArea_Validate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args) {

            if (args.Value.Length >= 1) {
                string ownarea = Convert.ToString(args.Value);
                //Check if areacode already exists

                int result = MappingsAreaCodes.CheckAreaCodeExists(ownarea);

                if (result == 0) {
                    //Does not exist continue
                    args.IsValid = true;

                }
                else {
                    //Does exist cancel
                    args.IsValid = false;
                    this.CustomValidatorOwnArea.Text = Resources.lang.MappingDetailsErrorMessageOwnAreaDuplicate;
                }

            }
            else {
                //Not valid
                args.IsValid = false;
                this.CustomValidatorOwnArea.Text = Resources.lang.MappingDetailsErrorMessageOwnArea;
            }

        }
        #endregion

        #region Click Events
        public event EventHandler SaveMappingDetails;


        protected void ButtonSave_Click(object sender, System.EventArgs e) {



            if (Page.IsValid) {
                string ownArea = null;
                string country = Convert.ToString(this.DropDownListCountries.SelectedValue);
                string area_name = this.TexBoxAreaName.Text;
                bool opco = Convert.ToBoolean(this.CheckBoxOPCO.Checked);
                bool fleetco = Convert.ToBoolean(this.CheckBoxFleetCo.Checked);
                bool carsales = Convert.ToBoolean(this.CheckBoxCarSales.Checked);
                bool licensee = Convert.ToBoolean(this.CheckBoxLicensee.Checked);

                int result = -1;

                switch (SessionHandler.MappingAreaCodeDefaultMode) {


                    case (int)App.BLL.Mappings.Mode.Insert:
                        ownArea = this.TextBoxOwnArea.Text;

                        result = MappingsAreaCodes.InsertAreaCode(ownArea, country, area_name, opco, fleetco, carsales, licensee);

                        break;

                    case (int)App.BLL.Mappings.Mode.Edit:
                        ownArea = this.TextBoxOwnArea.Text;

                        result = MappingsAreaCodes.UpdateAreaCode(ownArea, country, area_name, opco, fleetco, carsales, licensee);

                        break;
                }

                if (result == 0) {
                    //Success
                    _errorMessage = Resources.lang.MessageAreaCodeSaved;
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