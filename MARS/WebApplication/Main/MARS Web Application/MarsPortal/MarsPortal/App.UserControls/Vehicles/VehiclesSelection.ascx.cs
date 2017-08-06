using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Vehicles
{
    public partial class VehiclesSelection : System.Web.UI.UserControl
    {
        public System.Web.UI.WebControls.DropDownList DropDownListCountry
        {
            get { return this.DropDownListCountries; }
        }

        private string _label_message;
        private string _control_name;

        public string LabelMessage
        {
            get { return _label_message; }
            set { _label_message = value; } 
        }

        public string ControlName
        {
            get { return _control_name; }
            set { _control_name = value; } 
        }

        #region Page Events
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LabelCountry.Text = _label_message;
                this.LoadCountries();
            }
        }

        #endregion

        #region DropDownList Events

        public event EventHandler LoadSerials;

        protected void LoadCountries()
        {
            this.DropDownListCountries.Items.Clear();
            this.DropDownListCountries.DataSource = ReportLookups.GetCountries();
            this.DropDownListCountries.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListCountries, true);

        }

        protected void DropDownListCountries_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ControlName == "CountryOwner")
            {
                SessionHandler.MappingSelectedCountryOwner = this.DropDownListCountries.SelectedValue.ToString();
            }
            else if (ControlName == "CountryRent")
            {
                SessionHandler.MappingSelectedCountryRent = this.DropDownListCountries.SelectedValue.ToString();
            }
            
            if (LoadSerials != null)
            {
                LoadSerials(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Add "ALL" item to drop down lists
        /// </summary>
        /// <param name="dropDownListOption"></param>
        /// <remarks></remarks>

        private void AddInitialValueToDropDown(System.Web.UI.WebControls.DropDownList dropDownListOption, bool selected)
        {
            ListItem item = new ListItem();
            item.Text = Resources.lang.ReportSettingsALL;
            item.Selected = selected;
            item.Value = "-1";
            dropDownListOption.Items.Insert(0, item);
        }


        protected void CreateNewListItem(System.Web.UI.WebControls.DropDownList dropDownListToAddItem, string text, string value)
        {
            ListItem item = new ListItem();
            item.Value = value;
            item.Text = text;
            dropDownListToAddItem.Items.Add(item);
        }

        #endregion
    }
}