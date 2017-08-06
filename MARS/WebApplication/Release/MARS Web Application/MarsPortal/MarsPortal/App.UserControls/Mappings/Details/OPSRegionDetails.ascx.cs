using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details {
    public partial class OPSRegionDetails : System.Web.UI.UserControl {
        #region Properties and Fields
        private string _validationGroup;
        private int _ops_region_id;
        private string _ops_region;
        private string _country;

        private string _errorMessage;
        public int OPS_region_Id {
            get { return _ops_region_id; }
            set { _ops_region_id = value; }
        }

        public string OPS_Region {
            get { return _ops_region; }
            set { _ops_region = value; }
        }

        public string Country {
            get { return _country; }
            set { _country = value; }
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
            _validationGroup = SessionHandler.MappingOPSRegionValidationGroup;
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

            switch (SessionHandler.MappingOPSRegionDefaultMode) {
                case (int)App.BLL.Mappings.Mode.Insert:
                    this.LoadInsertMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:
                    this.LoadEditMode();

                    break;
            }


        }


        protected void LoadInsertMode() {

            this.TextBoxOPSRegion.Text = string.Empty;
            this.DatabindCountries(true);
            this.TextBoxOPSRegion.Focus();


        }


        protected void LoadEditMode() {
            this.DatabindCountries(false);
            this.LabelOPSRegionId.Text = _ops_region_id.ToString();
            this.TextBoxOPSRegion.Text = _ops_region;
            this.DropDownListCountries.SelectedValue = _country;

            this.TextBoxOPSRegion.Focus();


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
                int ops_region_id = -1;
                string ops_region = this.TextBoxOPSRegion.Text;
                string country = Convert.ToString(this.DropDownListCountries.SelectedValue);

                int result = -1;

                switch (SessionHandler.MappingOPSRegionDefaultMode) {
                    case (int)App.BLL.Mappings.Mode.Insert:
                        result = MappingsOPSRegions.InsertOPSRegion(ops_region, country);

                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:
                        ops_region_id = Convert.ToInt32(this.LabelOPSRegionId.Text);
                        result = MappingsOPSRegions.UpdateOPSRegion(ops_region_id, ops_region, country);

                        break;
                }

                if (result == 0) {
                    //Success
                    _errorMessage = Resources.lang.MessageOPSRegionSaved;
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