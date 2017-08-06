using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Common {
    public partial class MappingSelection : System.Web.UI.UserControl {
        // alterations by Gavin for MarsV3 19-4-12 lines 48 and 53

        #region Properties and Fields

        public System.Web.UI.WebControls.DropDownList DropDownListMapping {
            get { return this.DropDownListMappingType; }
        }

        public System.Web.UI.WebControls.DropDownList DropDownListCountry {
            get { return this.DropDownListCountries; }
        }
        
        #endregion

        #region Page Events

        protected void Page_Load(object sender, System.EventArgs e) {
            if (!Page.IsPostBack) {
                this.LoadMappingTypes();
                this.LoadCountries();
                this.DropDownListCountries.Enabled = false;
            }
        }

        #endregion

        #region DropDownList Events

        public event EventHandler LoadMappingTables;


        protected void LoadMappingTypes() {
            this.DropDownListMappingType.Items.Clear();
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.PleaseSelect, "-1");
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingCountry, Convert.ToString((int)App.BLL.Mappings.Type.Country));
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingAreaCodes, Convert.ToString((int)App.BLL.Mappings.Type.AreaCode));
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingCMSPools, Convert.ToString((int)App.BLL.Mappings.Type.CMSPools));
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingCMSLocationGroups, Convert.ToString((int)App.BLL.Mappings.Type.CMSLocationGroups));
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingOPSRegions, Convert.ToString((int)App.BLL.Mappings.Type.OPSRegions));
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingOPSAreas, Convert.ToString((int)App.BLL.Mappings.Type.OPSAreas));
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingLocations, Convert.ToString((int)App.BLL.Mappings.Type.Locations));
            CreateNewListItem(DropDownListMappingType, "Unmapped Locations", "941");  // code 94 (ga) 1 - added by Gavin
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingCarSegments, Convert.ToString((int)App.BLL.Mappings.Type.CarSegment));
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingCarClasses, Convert.ToString((int)App.BLL.Mappings.Type.CarClass));
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingCarGroups, Convert.ToString((int)App.BLL.Mappings.Type.CarGroup));
            this.CreateNewListItem(this.DropDownListMappingType, Resources.lang.MappingModelCode, Convert.ToString((int)App.BLL.Mappings.Type.ModelCodes));
            CreateNewListItem(DropDownListMappingType, "Unmapped Car Groups", "940"); // code 94 (ga) 0 - added by gavin 19-4
        }


        protected void LoadCountries() {
            this.DropDownListCountries.Items.Clear();
            this.DropDownListCountries.DataSource = ReportLookups.GetCountriesAll();
            this.DropDownListCountries.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListCountries, true);

        }


        protected void DropDownListMappingType_SelectedIndexChanged(object sender, System.EventArgs e) {

            if (this.DropDownListMappingType.SelectedValue == Convert.ToString((int)App.BLL.Mappings.Type.Country) || this.DropDownListMappingType.SelectedValue == "-1")
            {
                this.DropDownListCountries.Enabled = false;
                this.DropDownListCountries.SelectedValue = "-1";
            }
            else {
                this.DropDownListCountries.Enabled = true;
            }
            SessionHandler.MappingSelectedTable = Convert.ToInt32(this.DropDownListMappingType.SelectedValue);
            SessionHandler.MappingSelectedCountry = this.DropDownListCountries.SelectedValue.ToString();

            //Raise custom event from parent page
            if (LoadMappingTables != null) {
                LoadMappingTables(this, EventArgs.Empty);
            }


        }


        protected void DropDownListCountries_SelectedIndexChanged(object sender, System.EventArgs e) {
            SessionHandler.MappingSelectedCountry = this.DropDownListCountries.SelectedValue.ToString();
            SessionHandler.MappingSelectedTable = Convert.ToInt32(this.DropDownListMappingType.SelectedValue);
            //Raise custom event from parent page
            if (LoadMappingTables != null) {
                LoadMappingTables(this, EventArgs.Empty);
            }


        }

        /// <summary>
        /// Add "ALL" item to drop down lists
        /// </summary>
        /// <param name="dropDownListOption"></param>
        /// <remarks></remarks>

        private void AddInitialValueToDropDown(System.Web.UI.WebControls.DropDownList dropDownListOption, bool selected) {
            ListItem item = new ListItem();
            item.Text = Resources.lang.ReportSettingsALL;
            item.Selected = selected;
            item.Value = "-1";
            dropDownListOption.Items.Insert(0, item);



        }


        protected void CreateNewListItem(System.Web.UI.WebControls.DropDownList dropDownListToAddItem, string text, string value) {
            ListItem item = new ListItem();
            item.Value = value;
            item.Text = text;
            dropDownListToAddItem.Items.Add(item);

        }

        #endregion
    }
}