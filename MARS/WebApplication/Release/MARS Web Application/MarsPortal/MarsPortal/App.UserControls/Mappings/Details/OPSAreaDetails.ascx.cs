using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details {
    public partial class OPSAreaDetails : System.Web.UI.UserControl {
        #region Properties and Fields
        private string _validationGroup;
        private int _ops_area_id;
        private string _ops_area;
        private int _ops_region_id;

        private string _errorMessage;
        public int OPS_Area_Id {
            get { return _ops_area_id; }
            set { _ops_area_id = value; }
        }

        public string OPS_Area {
            get { return _ops_area; }
            set { _ops_area = value; }
        }

        public int OPS_Region_Id {
            get { return _ops_region_id; }
            set { _ops_region_id = value; }
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
            _validationGroup = SessionHandler.MappingOPSAreaValidationGroup;
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
            switch (SessionHandler.MappingOPSAreaDefaultMode) {
                case (int)App.BLL.Mappings.Mode.Insert:
                    this.LoadInsertMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:
                    this.LoadEditMode();

                    break;
            }


        }


        protected void LoadInsertMode() {
            this.TextBoxOPSArea.Text = string.Empty;
            this.DatabindOPSRegions(true);
            this.TextBoxOPSArea.Focus();


        }


        protected void LoadEditMode() {

            this.DatabindOPSRegions(false);
            this.LabelOPSAreaId.Text = _ops_area_id.ToString();
            this.TextBoxOPSArea.Text = _ops_area;
            this.DropDownListOPSRegions.SelectedValue = Convert.ToString(_ops_region_id);
            this.TextBoxOPSArea.Focus();


        }


        protected void DatabindOPSRegions(bool allSelected) {

            this.DropDownListOPSRegions.Items.Clear();
            this.DropDownListOPSRegions.DataSource = ReportLookups.GetOPSRegionsWithCountry();
            this.DropDownListOPSRegions.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListOPSRegions, allSelected);


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
                int ops_area_id = -1;
                string ops_area = this.TextBoxOPSArea.Text;
                int ops_region_id = Convert.ToInt32(this.DropDownListOPSRegions.SelectedValue);


                int result = -1;

                switch (SessionHandler.MappingOPSAreaDefaultMode) {
                    case (int)App.BLL.Mappings.Mode.Insert:
                        result = MappingsOPSAreas.InsertOPSArea(ops_area, ops_region_id);

                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:
                        ops_area_id = Convert.ToInt32(this.LabelOPSAreaId.Text);
                        result = MappingsOPSAreas.UpdateOPSArea(ops_area_id, ops_area, ops_region_id);
                        break;
                }

                if (result == 0) {
                    //Success
                    _errorMessage = Resources.lang.MessageOPSAreaSaved;
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