using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details {
    public partial class CMSPoolDetails : System.Web.UI.UserControl {
        #region Properties and Fields
        private string _validationGroup;
        private int _cms_pool_id;
        private string _cms_pool;
        private string _country;

        private string _errorMessage;
        public int CMS_Pool_Id {
            get { return _cms_pool_id; }
            set { _cms_pool_id = value; }
        }

        public string CMS_Pool {
            get { return _cms_pool; }
            set { _cms_pool = value; }
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

            _validationGroup = SessionHandler.MappingCMSPoolValidationGroup;
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

            switch (SessionHandler.MappingCMSPoolDefaultMode) {
                case (int)App.BLL.Mappings.Mode.Insert:
                    this.LoadInsertMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:
                    this.LoadEditMode();

                    break;
            }


        }


        protected void LoadInsertMode() {

            this.TextBoxCMSPool.Text = string.Empty;
            this.DatabindCountries(true);
            this.TextBoxCMSPool.Focus();


        }


        protected void LoadEditMode() {

            this.DatabindCountries(false);
            this.LabelCMSPoolId.Text = _cms_pool_id.ToString();
            this.TextBoxCMSPool.Text = _cms_pool;
            this.DropDownListCountries.SelectedValue = _country;
            this.TextBoxCMSPool.Focus();

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
                int cms_pool_id = -1;
                string cms_pool = this.TextBoxCMSPool.Text;
                string country = Convert.ToString(this.DropDownListCountries.SelectedValue);

                int result = -1;

                switch (SessionHandler.MappingCMSPoolDefaultMode) {
                    case (int)App.BLL.Mappings.Mode.Insert:
                        result = MappingsCMSPools.InsertCMSPool(cms_pool, country);

                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:
                        cms_pool_id = Convert.ToInt32(this.LabelCMSPoolId.Text);
                        result = MappingsCMSPools.UpdateCMSPool(cms_pool_id, cms_pool, country);

                        break;
                }

                if (result == 0) {
                    //Success
                    _errorMessage = Resources.lang.MessageCMSPoolSaved;
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