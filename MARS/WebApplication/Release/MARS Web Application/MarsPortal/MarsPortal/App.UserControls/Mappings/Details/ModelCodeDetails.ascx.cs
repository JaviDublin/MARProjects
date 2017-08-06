using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details {
    public partial class ModelCodeDetails : System.Web.UI.UserControl {
        #region Properties and Fields

        private string _validationGroup;
        private int _model_id;
        private string _model;
        private string _country;
        private bool _active;

        private string _errorMessage;

        public int Model_Id {
            get { return _model_id; }
            set { _model_id = value; }
        }

        public string Model {
            get { return _model; }
            set { _model = value; }
        }

        public string Country {
            get { return _country; }
            set { _country = value; }
        }

        public bool Active {
            get { return _active; }
            set { _active = value; }
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
            _validationGroup = SessionHandler.MappingModelCodeValidationGroup;
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
            switch (SessionHandler.MappingModelCodeDefaultMode) {
                case (int)App.BLL.Mappings.Mode.Insert:
                    this.LoadInsertMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:
                    this.LoadEditMode();

                    break;
            }


        }


        protected void LoadInsertMode() {
            this.TextBoxModel.Text = string.Empty;

            this.CheckBoxIsActive.Checked = false;
            this.DatabindCountries(true);
            this.TextBoxModel.Focus();


        }


        protected void LoadEditMode() {

            this.DatabindCountries(false);
            this.LabelModelId.Text = _model_id.ToString();
            this.TextBoxModel.Text = _model;
            this.CheckBoxIsActive.Checked = _active;
            this.DropDownListCountries.SelectedValue = Convert.ToString(_country);
            this.TextBoxModel.Focus();


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

        #region Click Events
        public event EventHandler SaveMappingDetails;


        protected void ButtonSave_Click(object sender, System.EventArgs e) {


            if (Page.IsValid) {
                int model_id = -1;
                string model = this.TextBoxModel.Text;
                string country = Convert.ToString(this.DropDownListCountries.SelectedValue);
                bool active = Convert.ToBoolean(this.CheckBoxIsActive.Checked);

                int result = -1;

                switch (SessionHandler.MappingModelCodeDefaultMode) {
                    case (int)App.BLL.Mappings.Mode.Insert:
                        result = MappingsModelCodes.InsertModelCode(country, model, active);

                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:
                        model_id = Convert.ToInt32(this.LabelModelId.Text);
                        result = MappingsModelCodes.UpdateModelCode(model_id, country, model, active);
                        break;
                }

                if (result == 0) {
                    //Success
                    _errorMessage = Resources.lang.MessageModelCodeSaved;
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