using System;
using System.Web.UI;
using App.BLL;

namespace App.Management.Mappings
{
    public partial class Default : PageBase
    {
        // alterations by gavin for MarsV3 19-4-12 - line 96

        #region "Page Events"

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //Set Page title
            this.Page.Title = "MARS - Maintenance Mapping Table";

            this.UserControlPageInformation.LastUpdateLabel.Visible = false;

            if (!Page.IsPostBack)
            {

                //Set page informtion on usercontrol
                base.SetPageInformationTitle("MappingTableMaintenance", this.UserControlPageInformation, false);

                SessionHandler.ClearMappingSelectionSessions();
            }

            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("MappingValidation"))
            {
                this.Page.ClientScript.RegisterClientScriptInclude("MappingValidation", this.ResolveUrl("~/JScript/MappingValidation.js"));
            }
        }

        protected void LoadMappingTables_Filter(object sender, System.EventArgs e)
        {
            int mappingType = Convert.ToInt32(SessionHandler.MappingSelectedTable);
            string country = SessionHandler.MappingSelectedCountry;

            switch (mappingType)
            {

                case (int)App.BLL.Mappings.Type.Country:

                    SessionHandler.ClearMappingCountrySessions();
                    this.MappingCountryGridview.LoadCountryControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.Country);

                    break;
                case (int)App.BLL.Mappings.Type.AreaCode:
                    SessionHandler.ClearMappingAreaCodeSessions();
                    this.MappingAreaCodeGridview.LoadAreaCodesControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.AreaCode);

                    break;
                case (int)App.BLL.Mappings.Type.CMSPools:
                    SessionHandler.ClearMappingCMSPoolSessions();
                    this.MappingCMSPoolGridview.LoadCMSPoolsControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.CMSPools);

                    break;
                case (int)App.BLL.Mappings.Type.CMSLocationGroups:
                    SessionHandler.MappingSelectedCMSPoolId = null;
                    SessionHandler.ClearMappingCMSLocationSessions();
                    this.MappingCMSLocationGridview.LoadCMSLocationsControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.CMSLocationGroups);

                    break;
                case (int)App.BLL.Mappings.Type.OPSRegions:
                    SessionHandler.ClearMappingOPSRegionSessions();
                    this.MappingOPSRegionGridviews.LoadOPSRegionsControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.OPSRegions);

                    break;
                case (int)App.BLL.Mappings.Type.OPSAreas:
                    SessionHandler.ClearMappingOPSAreaSessions();
                    SessionHandler.MappingSelectedOPSRegionId = null;
                    this.MappingOPSAreaGridview.LoadOPSAreasControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.OPSAreas);

                    break;
                case 941:
                case (int)App.BLL.Mappings.Type.Locations:
                    App.BLL.MappingsLocations.Loc = mappingType == 941 ? "UNMAPPED" : ""; // added by Gavin to track UNMAPPED locations
                    SessionHandler.ClearMappingLocationSessions();
                    SessionHandler.MappingSelectedOPSAreaId = null;
                    SessionHandler.MappingSelectedCMSLocationGroupCode = null;
                    this.MappingLocationGridview.LoadLocationsControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.Locations);

                    break;
                case (int)App.BLL.Mappings.Type.CarSegment:
                    SessionHandler.ClearMappingCarSegmentSessions();
                    this.MappingCarSegmentGridview.LoadCarSegmentControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.CarSegment);

                    break;
                case (int)App.BLL.Mappings.Type.CarClass:
                    SessionHandler.ClearMappingCarClassSessions();
                    SessionHandler.MappingSelectedCarSegmentId = null;
                    this.MappingCarClassGridview.LoadCarClassControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.CarClass);

                    break;
                case 940:
                case (int)App.BLL.Mappings.Type.CarGroup:
                    App.BLL.MappingsCarGroup.carClass = mappingType == 940 ? "UNMAPPED" : "";// search for unmapped classes
                    SessionHandler.ClearMappingCarGroupSessions();
                    SessionHandler.MappingSelectedCarClassId = null;
                    this.MappingCarGroupGridview.LoadCarGroupControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.CarGroup);

                    break;
                case (int)App.BLL.Mappings.Type.ModelCodes:
                    SessionHandler.ClearMappingModelCodeSessions();
                    this.MappingModelCodesGridview.LoadModelCodeControl();
                    this.ShowGridviews((int)App.BLL.Mappings.Type.ModelCodes);

                    break;
                default:
                    //Select all was selected
                    this.HideGridviews();

                    break;
            }

            this.ClearMessageLabels();
            this.UpdatePanelMaintenanceMappingTable.Update();




        }


        protected void ClearMessageLabels()
        {
            this.MappingCountryGridview.MessageLabel.Text = string.Empty;
            this.MappingAreaCodeGridview.MessageLabel.Text = string.Empty;
            this.MappingCMSPoolGridview.MessageLabel.Text = string.Empty;
            this.MappingCMSLocationGridview.MessageLabel.Text = string.Empty;
            this.MappingOPSRegionGridviews.MessageLabel.Text = string.Empty;
            this.MappingOPSAreaGridview.MessageLabel.Text = string.Empty;
            this.MappingLocationGridview.MessageLabel.Text = string.Empty;
            this.MappingCarSegmentGridview.MessageLabel.Text = string.Empty;
            this.MappingCarGroupGridview.MessageLabel.Text = string.Empty;
            this.MappingCarClassGridview.MessageLabel.Text = string.Empty;


        }


        protected void HideGridviews()
        {
            this.MappingCountryGridview.Visible = false;
            this.MappingAreaCodeGridview.Visible = false;
            this.MappingCMSPoolGridview.Visible = false;
            this.MappingCMSLocationGridview.Visible = false;
            this.MappingOPSRegionGridviews.Visible = false;
            this.MappingOPSAreaGridview.Visible = false;
            this.MappingLocationGridview.Visible = false;
            this.MappingCarSegmentGridview.Visible = false;
            this.MappingCarClassGridview.Visible = false;
            this.MappingCarGroupGridview.Visible = false;
            this.MappingModelCodesGridview.Visible = false;

        }


        protected void ShowGridviews(int gridviewToShow)
        {

            switch (gridviewToShow)
            {

                case (int)App.BLL.Mappings.Type.Country:
                    this.MappingCountryGridview.Visible = true;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.AreaCode:

                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = true;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.CMSPools:
                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = true;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.CMSLocationGroups:
                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = true;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.OPSRegions:
                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = true;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.OPSAreas:
                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = true;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.Locations:
                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = true;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.CarSegment:
                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = true;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.CarClass:
                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = true;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.CarGroup:
                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = true;
                    this.MappingModelCodesGridview.Visible = false;

                    break;
                case (int)App.BLL.Mappings.Type.ModelCodes:
                    this.MappingCountryGridview.Visible = false;
                    this.MappingAreaCodeGridview.Visible = false;
                    this.MappingCMSPoolGridview.Visible = false;
                    this.MappingCMSLocationGridview.Visible = false;
                    this.MappingOPSRegionGridviews.Visible = false;
                    this.MappingOPSAreaGridview.Visible = false;
                    this.MappingLocationGridview.Visible = false;
                    this.MappingCarSegmentGridview.Visible = false;
                    this.MappingCarClassGridview.Visible = false;
                    this.MappingCarGroupGridview.Visible = false;
                    this.MappingModelCodesGridview.Visible = true;
                    break;
            }


        }

        #endregion

        #region "Gridview Events"


        protected void ShowDependents(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int mappingType = Convert.ToInt32(SessionHandler.MappingSelectedTable);
            string country = null;
            int? cms_pool_id = null;
            int? ops_region_id = null;
            int? ops_area_id = null;
            int? car_segment_id = null;
            int? car_class_id = null;


            switch (mappingType)
            {

                case (int)App.BLL.Mappings.Type.Country:

                    switch (this.MappingCountryGridview.Selection)
                    {

                        case (int)App.BLL.Mappings.Type.AreaCode:
                            country = this.MappingCountryGridview.Country;
                            SessionHandler.ClearMappingAreaCodeSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.AreaCode;
                            this.MappingAreaCodeGridview.LoadAreaCodesControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.AreaCode);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.AreaCode);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;
                        case (int)App.BLL.Mappings.Type.CMSPools:
                            country = this.MappingCountryGridview.Country;
                            SessionHandler.ClearMappingCMSPoolSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.CMSPools;
                            this.MappingCMSPoolGridview.LoadCMSPoolsControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.CMSPools);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.CMSPools);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;

                        case (int)App.BLL.Mappings.Type.OPSRegions:
                            country = this.MappingCountryGridview.Country;
                            SessionHandler.ClearMappingOPSRegionSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.OPSRegions;
                            this.MappingOPSRegionGridviews.LoadOPSRegionsControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.OPSRegions);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.OPSRegions);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;
                        case (int)App.BLL.Mappings.Type.CarSegment:
                            country = this.MappingCountryGridview.Country;
                            SessionHandler.ClearMappingCarSegmentSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.CarSegment;
                            this.MappingCarSegmentGridview.LoadCarSegmentControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.CarSegment);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.CarSegment);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;
                        case (int)App.BLL.Mappings.Type.ModelCodes:
                            country = this.MappingCountryGridview.Country;
                            SessionHandler.ClearMappingModelCodeSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.ModelCodes;
                            this.MappingModelCodesGridview.LoadModelCodeControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.ModelCodes);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.ModelCodes);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;


                    }

                    break;
                case (int)App.BLL.Mappings.Type.CMSPools:

                    switch (this.MappingCMSPoolGridview.Selection)
                    {

                        case (int)App.BLL.Mappings.Type.CMSLocationGroups:
                            country = this.MappingCMSPoolGridview.Country;
                            cms_pool_id = this.MappingCMSPoolGridview.CMS_Pool_Id;
                            SessionHandler.ClearMappingCMSLocationSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedCMSPoolId = cms_pool_id;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.CMSLocationGroups;
                            this.MappingCMSLocationGridview.LoadCMSLocationsControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.CMSLocationGroups);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.CMSLocationGroups);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;
                    }

                    break;
                case (int)App.BLL.Mappings.Type.CMSLocationGroups:

                    switch (this.MappingCMSLocationGridview.Selection)
                    {
                        case (int)App.BLL.Mappings.Type.Locations:
                            country = this.MappingOPSAreaGridview.Country;
                            int? cms_location_group_id = this.MappingCMSLocationGridview.CMS_Location_Group_Id;
                            SessionHandler.ClearMappingLocationSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedCMSLocationGroupCode = cms_location_group_id;
                            SessionHandler.MappingSelectedOPSAreaId = null;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.Locations;
                            this.MappingLocationGridview.LoadLocationsControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.Locations);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.Locations);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;
                    }

                    break;
                case (int)App.BLL.Mappings.Type.OPSRegions:
                    switch (this.MappingOPSRegionGridviews.Selection)
                    {
                        case (int)App.BLL.Mappings.Type.OPSAreas:
                            country = this.MappingOPSRegionGridviews.Country;
                            ops_region_id = this.MappingOPSRegionGridviews.OPS_Region_Id;
                            SessionHandler.ClearMappingOPSAreaSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedOPSRegionId = ops_region_id;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.OPSAreas;
                            this.MappingOPSAreaGridview.LoadOPSAreasControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.OPSAreas);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.OPSAreas);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;
                            break;
                    }
                    break;
                case (int)App.BLL.Mappings.Type.OPSAreas:

                    switch (this.MappingOPSAreaGridview.Selection)
                    {

                        case (int)App.BLL.Mappings.Type.Locations:
                            country = this.MappingOPSAreaGridview.Country;
                            ops_area_id = this.MappingOPSAreaGridview.OPS_Area_Id;
                            SessionHandler.ClearMappingLocationSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedOPSAreaId = ops_area_id;
                            SessionHandler.MappingSelectedCMSLocationGroupCode = null;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.Locations;
                            this.MappingLocationGridview.LoadLocationsControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.Locations);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.Locations);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;

                    }

                    break;
                case (int)App.BLL.Mappings.Type.CarSegment:

                    switch (this.MappingCarSegmentGridview.Selection)
                    {

                        case (int)App.BLL.Mappings.Type.CarClass:
                            country = this.MappingCarSegmentGridview.Country;
                            car_segment_id = this.MappingCarSegmentGridview.Car_Segment_Id;
                            SessionHandler.ClearMappingCarClassSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedCarSegmentId = car_segment_id;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.CarClass;
                            this.MappingCarClassGridview.LoadCarClassControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.CarClass);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.CarClass);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;
                    }

                    break;
                case (int)App.BLL.Mappings.Type.CarClass:

                    switch (this.MappingCarClassGridview.Selection)
                    {

                        case (int)App.BLL.Mappings.Type.CarGroup:
                            country = this.MappingCarClassGridview.Country;
                            car_class_id = this.MappingCarClassGridview.Car_Class_Id;
                            SessionHandler.ClearMappingCarGroupSessions();
                            SessionHandler.MappingSelectedCountry = country;
                            SessionHandler.MappingSelectedCarClassId = car_class_id;
                            SessionHandler.MappingSelectedTable = (int)App.BLL.Mappings.Type.CarGroup;
                            this.MappingCarGroupGridview.LoadCarGroupControl();
                            this.ShowGridviews((int)App.BLL.Mappings.Type.CarGroup);
                            this.MappingSelection.DropDownListMapping.SelectedValue = Convert.ToString((int)App.BLL.Mappings.Type.CarGroup);
                            this.MappingSelection.DropDownListCountry.SelectedValue = country;
                            this.MappingSelection.DropDownListCountry.Enabled = true;

                            break;
                    }

                    break;

            }



            this.UpdatePanelMaintenanceMappingTable.Update();

        }

        #endregion
    }
}