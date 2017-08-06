using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details
{
    public partial class CountryDetails : System.Web.UI.UserControl
    {
        #region Properties and Fields
        private string _validationGroup;
        private string _country;
        private string _country_dw;
        private string _country_description;
        private bool _active;

        private string _errorMessage;
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string Country_DW
        {
            get { return _country_dw; }
            set { _country_dw = value; }
        }

        public string Country_Description
        {
            get { return _country_description; }
            set { _country_description = value; }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
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
            _validationGroup = SessionHandler.MappingCountryValidationGroup;
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

            switch (SessionHandler.MappingCountryDefaultMode)
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
            this.TextBoxCountry.Enabled = true;
            this.CustomValidatorCountry.Enabled = true;

            foreach (Control cntrl in this.PanelPopupMappingDetails.Controls)
            {
                if (cntrl is TextBox)
                {
                    TextBox txtBox = (TextBox)cntrl;
                    txtBox.Text = string.Empty;
                }
            }

            this.TextBoxCountry.Focus();


        }


        protected void LoadEditMode()
        {
            this.TextBoxCountry.Enabled = false;
            this.CustomValidatorCountry.Enabled = false;

            this.TextBoxCountry.Text = _country;
            this.TextBoxCountryDescription.Text = _country_description;
            this.TextBoxCountryDW.Text = _country_dw;
            this.CheckBoxIsActive.Checked = _active;

            this.TextBoxCountry.Focus();

        }
        #endregion

        #region Validation
        protected void Country_Validate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {

            if (args.Value.Length >= 1)
            {
                string country = Convert.ToString(args.Value);
                //Check if country already exists

                int result = MappingsCountry.CheckIfCountryExists(country);

                if (result == 0)
                {
                    //Does not exist continue
                    args.IsValid = true;

                }
                else
                {
                    //Does exist cancel
                    args.IsValid = false;
                    this.CustomValidatorCountry.Text = Resources.lang.MappingDetailsErrorMessageCountryDuplicate;
                }

            }
            else
            {
                //Not valid
                args.IsValid = false;
                this.CustomValidatorCountry.Text = Resources.lang.MappingDetailsErrorMessageCountry;
            }

        }
        #endregion

        #region Click Events
        public event EventHandler SaveMappingDetails;


        protected void ButtonSave_Click(object sender, System.EventArgs e)
        {


            if (Page.IsValid)
            {
                string country = null;
                string countryDW = this.TextBoxCountryDW.Text;
                string countryDescription = this.TextBoxCountryDescription.Text;
                bool active = Convert.ToBoolean(this.CheckBoxIsActive.Checked);


                int result = -1;

                switch (SessionHandler.MappingCountryDefaultMode)
                {
                    case (int)App.BLL.Mappings.Mode.Insert:
                        country = this.TextBoxCountry.Text;
                        result = MappingsCountry.InsertCountry(country, countryDW, countryDescription, active);

                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:
                        country = this.TextBoxCountry.Text;
                        result = MappingsCountry.UpdateCountry(country, countryDW, countryDescription, active);

                        break;
                }

                if (result == 0)
                {
                    //Success
                    _errorMessage = Resources.lang.MessageCountrySaved;
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