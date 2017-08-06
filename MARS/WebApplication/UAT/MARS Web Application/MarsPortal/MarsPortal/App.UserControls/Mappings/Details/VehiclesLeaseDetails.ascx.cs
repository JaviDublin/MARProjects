using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;
using System.Linq;

namespace App.UserControls.Mappings.Details
{
    public partial class VehiclesLeaseDetails : System.Web.UI.UserControl
    {
        #region Properties and Fields
        
        private string _validationGroup;
        
        private string _serial;       
        private string _country_owner;
        private string _country_rent;
        private string _start_date;
        
        private string _errorMessage;
        
        public string Serial
        {
            get { return _serial; }
            set { _serial = value; }
        }

        public string Country_Owner
        {
            get { return _country_owner; }
            set { _country_owner = value; }
        }

        public string Country_Rent
        {
            get { return _country_rent; }
            set { _country_rent = value; }
        }

        public string StartDate
        {
            get { return _start_date; }
            set { _start_date = value; }
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
            _validationGroup = SessionHandler.MappingVehiclesLeaseValidationGroup;
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
                if (cntrl is App.UserControls.DatePicker.DatePickerTextBox)
                {
                    App.UserControls.DatePicker.DatePickerTextBox dtpb = (App.UserControls.DatePicker.DatePickerTextBox)cntrl;
                    dtpb.ValidationGroup = _validationGroup;
                }

            }

            switch (SessionHandler.MappingVehiclesLeaseDefaultMode)
            {
                case (int)App.BLL.Mappings.Mode.Insert:
                    this.LoadInsertMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:
                    this.LoadEditMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Delete:
                    this.LoadDeleteMode();

                    break;
            }

        }

        protected void LoadInsertMode()
        {
            this.TextBoxSerials.Enabled = true;
            this.LoadControls(true);
            this.DropDownListCountriesOwner.Enabled = true;
            this.ButtonSave.Text = "Add";
            this.DatabindCountries();

            foreach (Control cntrl in this.PanelPopupMappingDetails.Controls)
            {
                if (cntrl is TextBox)
                {
                    TextBox txtBox = (TextBox)cntrl;
                    txtBox.Text = string.Empty;
                }  
            }
            this.TextBoxSerials.Focus();
        }

        protected void LoadEditMode()
        {
            this.TextBoxSerials.Enabled = false;
            this.LoadControls(true);
            this.DropDownListCountriesOwner.Enabled = false;
            this.TextBoxSerials.Text = _serial;
            this.ButtonSave.Text = "Save";
            this.DatabindCountries();
            this.DropDownListCountriesOwner.SelectedValue = _country_owner;
            this.DropDownListCountriesRent.SelectedValue = _country_rent;
            this.dptbStartDate.DefaultDate = Convert.ToDateTime(StartDate);
            this.TextBoxSerials.Focus();

        }

        protected void LoadDeleteMode()
        {
            this.TextBoxSerials.Enabled = true;
            this.LoadControls(false);
            this.ButtonSave.Text = "Delete";
            this.TextBoxSerials.Focus();
        }

        protected void LoadControls(bool visible)
        {
            this.LabelMessage.Text = String.Empty;

            if (visible == true)
            {
                this.LabelCountryOwner.Visible = true;
                this.DropDownListCountriesOwner.Visible = true;
                this.LabelCountryRent.Visible = true;
                this.DropDownListCountriesRent.Visible = true;
                this.LabelStartDate.Visible = true;
                this.dptbStartDate.Visible = true;
            }
            else
            {
                this.LabelCountryOwner.Visible = false;
                this.DropDownListCountriesOwner.Visible = false;
                this.LabelCountryRent.Visible = false;
                this.DropDownListCountriesRent.Visible = false;
                this.LabelStartDate.Visible = false;
                this.dptbStartDate.Visible = false;
            }
        }

        protected void DatabindCountries()
        {
            this.DropDownListCountriesOwner.Items.Clear();
            this.DropDownListCountriesOwner.DataSource = ReportLookups.GetCountries();
            this.DropDownListCountriesOwner.DataBind();

            this.DropDownListCountriesRent.Items.Clear();
            this.DropDownListCountriesRent.DataSource = ReportLookups.GetCountries();
            this.DropDownListCountriesRent.DataBind();
        }

        #endregion

        #region Validation

        public List<MappingsVehiclesLease.FleetSerials> default_values = new List<MappingsVehiclesLease.FleetSerials>();

        public List<string> valids = new List<string>();

        public List<string> invalids = new List<string>();

        private void Validation(string value, string parameter)
        {
            bool invalid = true;

            switch (SessionHandler.MappingVehiclesLeaseDefaultMode)
            {
                case (int)App.BLL.Mappings.Mode.Insert:
                    
                    default_values = MappingsVehiclesLease.SelectFleetSerialsByCountry(parameter);

                    for (int i = 0; i < default_values.Count; i++)
                    {
                        if (value == default_values[i].Serial)
                        {
                            invalid = false;

                            valids.Add(value);

                            break;
                        }
                    }
            
                    if (invalid == true)
                    {
                        invalids.Add(value);
                    }

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:
                    
                    default_values = MappingsVehiclesLease.SelectFleetSerialsByCountry(parameter);

                    for (int i = 0; i < default_values.Count; i++)
                    {
                        if (value == default_values[i].Serial)
                        {
                            invalid = false;

                            valids.Add(value);

                            break;
                        }
                    }
            
                    if (invalid == true)
                    {
                        invalids.Add(value);
                    }

                    break;
                case (int)App.BLL.Mappings.Mode.Delete:
                    
                    invalid = false;

                    valids.Add(value);
                    
                    break;
            }
        }

        #endregion

        #region Click Events
        
        public event EventHandler SaveMappingDetails;

        protected void ButtonSave_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string country_owner = this.DropDownListCountriesOwner.SelectedValue.ToString();

                    string countr_rent = this.DropDownListCountriesRent.SelectedValue.ToString();

                    string start_date = this.dptbStartDate.getDate().ToString();

                    string textbox_content = TextBoxSerials.Text;

                    textbox_content = textbox_content.Replace(";", String.Empty);

                    textbox_content = textbox_content.Replace(" -Invalid", String.Empty);

                    textbox_content = textbox_content.Replace(System.Environment.NewLine, ";");

                    string[] content;

                    int counter = textbox_content.Split(';').Length - 1;

                    int lenght = textbox_content.Length;

                    string message = String.Empty;

                    if (lenght > 0)
                    {
                        if (counter > 0)
                        {
                            content = textbox_content.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                            for (int i = 0; i < content.Length; i++)
                            {
                                Validation(content[i], country_owner);
                            }

                            valids = valids.Distinct().ToList();

                            invalids = invalids.Distinct().ToList();
                        }
                        else
                        {
                            Validation(textbox_content, country_owner);
                        }

                        message = "Valids = " + valids.Count.ToString() + " - " + "Invalids = " + invalids.Count.ToString();
                    }
                    else
                    {
                        message = "There is no text in the Text Box";
                    }

                    this.LabelMessage.Text = message;

                    TextBoxSerials.Text = String.Empty;

                    TextBoxSerials.Text = TextBox_List();

                    if (invalids.Count == 0)
                    {
                        int result = -1;

                        switch (SessionHandler.MappingVehiclesLeaseDefaultMode)
                        {
                            case (int)App.BLL.Mappings.Mode.Insert:
                                
                                for (int i = 0; i < valids.Count; i++)
                                {
                                    MappingsVehiclesLease.InsertVehicleLease(valids[i], country_owner,countr_rent,start_date);
                                }
                                
                                result = 0;
                                
                                break;

                            case (int)App.BLL.Mappings.Mode.Edit:
                                 for (int i = 0; i < valids.Count; i++)
                                {
                                    MappingsVehiclesLease.UpdateVehicleLease(valids[i], country_owner,countr_rent,start_date);
                                }
                                
                                result = 0;
                                
                                break;
                            case (int)App.BLL.Mappings.Mode.Delete:

                                for (int i = 0; i < valids.Count; i++)
                                {
                                    MappingsVehiclesLease.DeleteVehicleLease(valids[i]);
                                }

                                result = 0;

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
                        this.ModalPopupExtenderMappingDetails.Show();
                    }
                }
                catch (Exception ex)
                {
                    this.LabelMessage.Text = ex.Message;
                }
            }
            else
            {
                //Keep the modal popup form show
                this.ModalPopupExtenderMappingDetails.Show();
            }

        }

        private string TextBox_List()
        {
            string textbox_list = String.Empty;

            for (int i = 0; i < invalids.Count; i++)
            {
                if (i == 0)
                {
                    textbox_list = invalids[i] + " -Invalid" + System.Environment.NewLine;
                }
                else
                {
                    textbox_list = textbox_list + invalids[i] + " -Invalid" + System.Environment.NewLine;
                }
            }


            for (int v = 0; v < valids.Count; v++)
            {
                if (textbox_list == String.Empty)
                {
                    textbox_list = valids[v] + System.Environment.NewLine;
                }
                else
                {
                    textbox_list = textbox_list + valids[v] + System.Environment.NewLine;
                }
            }


            return textbox_list;
        }

        #endregion
    }
}