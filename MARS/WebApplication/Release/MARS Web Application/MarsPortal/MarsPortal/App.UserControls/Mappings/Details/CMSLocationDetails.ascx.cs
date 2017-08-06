using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details
{
    public partial class CMSLocationDetails : System.Web.UI.UserControl
    {
        #region Properties and Fields
        private string _validationGroup;
        private int _cms_location_group_id;
        private string _cms_location_group_code_dw;
        private string _cms_location_group;
        private int _cms_pool_id;

        private string _errorMessage;
        public int CMS_Location_Group_Id
        {
            get { return _cms_location_group_id; }
            set { _cms_location_group_id = value; }
        }

        public string CMS_Location_Group_Code_DW
        {
            get { return _cms_location_group_code_dw; }
            set { _cms_location_group_code_dw = value; }
        }

        public string CMS_Location_Group
        {
            get { return _cms_location_group; }
            set { _cms_location_group = value; }
        }

        public int CMS_Pool_Id
        {
            get { return _cms_pool_id; }
            set { _cms_pool_id = value; }
        }

        public AjaxControlToolkit.ModalPopupExtender ModalExtenderMapping
        {
            get { return this.ModalPopupExtenderMappingDetails; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }
        #endregion

        #region Page Events
        public void LoadDetails()
        {

            _validationGroup = SessionHandler.MappingCMSLocationValidationGroup;
            this.ButtonSave.ValidationGroup = _validationGroup;

            foreach (Control cntrl in this.PanelPopupMappingDetails.Controls)
            {
                if (cntrl is TextBox)
                {
                    TextBox txtBox = (TextBox)cntrl;
                    txtBox.ValidationGroup = _validationGroup;
                }
                if (cntrl is RequiredFieldValidator)
                {
                    RequiredFieldValidator reqFieldValidator = (RequiredFieldValidator)cntrl;
                    reqFieldValidator.ValidationGroup = _validationGroup;
                }
                if (cntrl is CustomValidator)
                {
                    CustomValidator custValidator = (CustomValidator)cntrl;
                    custValidator.ValidationGroup = _validationGroup;
                }
                if (cntrl is System.Web.UI.WebControls.DropDownList)
                {
                    System.Web.UI.WebControls.DropDownList ddList = (System.Web.UI.WebControls.DropDownList)cntrl;
                    ddList.ValidationGroup = _validationGroup;
                }

            }

            switch (SessionHandler.MappingCMSLocationDefaultMode)
            {
                case (int)App.BLL.Mappings.Mode.Insert:
                    this.LoadInsertMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:
                    this.LoadEditMode();

                    break;
            }


        }


        protected void LoadInsertMode()
        {


            this.TextBoxLocationGroupCode.Enabled = true;
         
            foreach (Control cntrl in this.PanelPopupMappingDetails.Controls)
            {
                if (cntrl is TextBox)
                {
                    TextBox txtBox = (TextBox)cntrl;
                    txtBox.Text = string.Empty;
                }
            }


            this.DatabindCMSPools(true);
            this.TextBoxLocationGroupCode.Focus();

        }


        protected void LoadEditMode()
        {

            this.TextBoxLocationGroupCode.Enabled = false;
           
            this.DatabindCMSPools(false);

            this.TextBoxLocationGroupCode.Text = _cms_location_group_id.ToString();
            this.TextBoxLocationGroupCodeDW.Text = _cms_location_group_code_dw;
            this.TextBoxLocationGroup.Text = _cms_location_group;
            this.DropDownListCMSPool.SelectedValue = Convert.ToString(_cms_pool_id);

            this.TextBoxLocationGroup.Focus();

        }




        protected void DatabindCMSPools(bool allSelected)
        {
            this.DropDownListCMSPool.Items.Clear();
            this.DropDownListCMSPool.DataSource = ReportLookups.GetCMSPoolsWithCountry();
            this.DropDownListCMSPool.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListCMSPool, allSelected);

        }

        /// <summary>
        /// Add "ALL" item to drop down lists
        /// </summary>
        /// <param name="dropDownListOption"></param>
        /// <remarks></remarks>

        private void AddInitialValueToDropDown(System.Web.UI.WebControls.DropDownList dropDownListOption, bool selected)
        {
            ListItem item = new ListItem();
            item.Text = Resources.lang.MappingDropDownListInitialValue;
            item.Selected = selected;
            item.Value = "-1";
            dropDownListOption.Items.Insert(0, item);


        }
        #endregion

        #region Validation
        protected void CMSLocationGroup_Validate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {

            if (args.Value.Length >= 1)
            {
                int cms_location_group_id = int.Parse(args.Value);
                //Check if country already exists

                int result = MappingsCMSLocations.CheckCMSLocationGroupExists(cms_location_group_id);

                if (result == 0)
                {
                    //Does not exist continue
                    args.IsValid = true;

                }
                else
                {
                    //Does exist cancel
                    args.IsValid = false;
                  
                }

            }
            else
            {
                //Not valid
                args.IsValid = false;

            }

        }
        #endregion

        #region Click Events
        public event EventHandler SaveMappingDetails;


        protected void ButtonSave_Click(object sender, System.EventArgs e)
        {



            if (Page.IsValid)
            {
                int cms_location_group_id = 0;
                string cms_location_group_code_dw = this.TextBoxLocationGroupCodeDW.Text;
                string cms_location_group = this.TextBoxLocationGroup.Text;
                int cms_pool_id = Convert.ToInt32(this.DropDownListCMSPool.SelectedValue);

                int result = -1;

                switch (SessionHandler.MappingCMSLocationDefaultMode)
                {
                    case (int)App.BLL.Mappings.Mode.Insert:
                      
                        result = MappingsCMSLocations.InsertCMSLocationGroup( cms_location_group_code_dw, cms_location_group, cms_pool_id);

                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:
                        cms_location_group_id = int.Parse(TextBoxLocationGroupCode.Text);
                        result = MappingsCMSLocations.UpdateCMSLocationGroup(cms_location_group_id, cms_location_group_code_dw, cms_location_group, cms_pool_id);
                        break;
                }

                if (result == 0)
                {
                    //Success
                    _errorMessage = Resources.lang.MessageCMSLocationGroupCodeSaved;
                }
                else
                {
                    //Failed
                    _errorMessage = Resources.lang.ErrorMessageAdministrator;
                }

                //Raise custom event from parent page
                if (SaveMappingDetails != null)
                {
                    SaveMappingDetails(this, EventArgs.Empty);
                }
            }
            else
            {
                //Keep the modal popup form show
                this.ModalPopupExtenderMappingDetails.Show();
            }


        }
        #endregion
    }
}