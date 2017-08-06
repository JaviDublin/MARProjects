using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.Management.vehiclesLease
{
    public partial class Default : PageBase
    {
        #region "Page Events"
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Page title
            this.Page.Title = "MARS - Vehicles Lease";
            this.UserControlPageInformation.LastUpdateLabel.Visible = false;

            if (!Page.IsPostBack)
            {

                //Set page informtion on usercontrol
                // base.SetPageInformationTitle("VehiclesLease", this.UserControlPageInformation, false);

                SessionHandler.ClearMappingVehiclesLeaseSessions();
                //Settings for pager control
                //Set Default Sort Order
                SessionHandler.MappingVehiclesLeaseSortOrder = "ASC";
                //Set current Page number and size
                SessionHandler.MappingVehiclesLeaseCurrentPageNumber = 1;
                SessionHandler.MappingVehiclesLeasePageSize = 10;
                
                //Load Data
                SessionHandler.MappingSelectedCountryOwner = "-1";
                SessionHandler.MappingSelectedCountryRent = "-1";
                SessionHandler.MappingSelectedStartDate = "-1";
                SessionHandler.MappingSelectedModelDescription = "-1";
                
                RebindModelDescriptions();
                
                LoadVehiclesLease();
            }

            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("MappingValidation"))
            {
                this.Page.ClientScript.RegisterClientScriptInclude("MappingValidation", this.ResolveUrl("~/JScript/MappingValidation.js"));
            }
        }

        protected void LoadSerials_Filter(object sender, System.EventArgs e)
        {
            string country_owner = SessionHandler.MappingSelectedCountryOwner;
            string country_rent = SessionHandler.MappingSelectedCountryRent;
            string start_date = SessionHandler.MappingSelectedStartDate;
            string model_description = SessionHandler.MappingSelectedModelDescription;
            SessionHandler.ClearMappingVehiclesLeaseSessions();
            this.MappingVehiclesLeaseGridview.LoadVehiclesLeaseControl();
            RebindModelDescriptions();
            this.UpdatePanelVehiclesLease.Update();
        }

        protected void LoadSerials_Model_Filter(object sender, System.EventArgs e)
        {
            string country_owner = SessionHandler.MappingSelectedCountryOwner;
            string country_rent = SessionHandler.MappingSelectedCountryRent;
            string start_date = SessionHandler.MappingSelectedStartDate;
            string model_description = SessionHandler.MappingSelectedModelDescription;
            SessionHandler.ClearMappingVehiclesLeaseSessions();
            this.MappingVehiclesLeaseGridview.LoadVehiclesLeaseControl();
            this.UpdatePanelVehiclesLease.Update();
        }
 
        #endregion

        #region "Gridview Events"

        protected void LoadVehiclesLease()
        {
            string country_owner = SessionHandler.MappingSelectedCountryOwner;
            string country_rent = SessionHandler.MappingSelectedCountryRent;
            string start_date = SessionHandler.MappingSelectedStartDate;
            string model_description = SessionHandler.MappingSelectedModelDescription;
            SessionHandler.ClearMappingVehiclesLeaseSessions();
            this.MappingVehiclesLeaseGridview.LoadVehiclesLeaseControl();
            this.UpdatePanelVehiclesLease.Update();
            
        }

        protected void RebindModelDescriptions()
        {
            string country_owner = SessionHandler.MappingSelectedCountryOwner;
            string country_rent = SessionHandler.MappingSelectedCountryRent;
            string start_date = SessionHandler.MappingSelectedStartDate;
            System.Web.UI.WebControls.CheckBoxList checkBoxListModelDesc = (System.Web.UI.WebControls.CheckBoxList)this.ModelDescription.FindControl("CheckBoxListPopUp");
            checkBoxListModelDesc.DataTextField = "ModelDescription";
            checkBoxListModelDesc.DataValueField = "ModelDescription";
            checkBoxListModelDesc.Items.Clear();
            checkBoxListModelDesc.DataSource = MappingsVehiclesLease.SelectModelsDescription(country_owner, country_rent, start_date);
            checkBoxListModelDesc.DataBind();
            foreach (ListItem item in checkBoxListModelDesc.Items)
            {
                item.Selected = false;
            }
            //Load the check box list settings
            this.ModelDescription.LoadCheckBoxList();

        }

        protected void ShowDependents(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        { 
        
        }

        #endregion
    }
}