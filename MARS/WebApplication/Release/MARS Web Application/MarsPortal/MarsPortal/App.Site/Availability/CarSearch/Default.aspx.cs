using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.AvailabilityTool.CarSearch {
    public partial class Default : PageBase {
        // Altered: 2/4/12 by Gavin
        // line 418 with CarFilter usercontrol allocated to static in AvailabilityCarSearch
        // line 515 added event from CarFilter user control
        // line 519 added event from CarFilter user control, which calls itself!

        #region "Page Events"

        protected void Page_Load(object sender, System.EventArgs e) {
            //Set report settings tool
            this.UserControlReportSettings.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSettings.MarsPage = (int)ReportSettings.ReportSettingsPage.ATCarSearch;

            //Set Page title
            this.Page.Title = "MARS - Availability Tool";

            //Settings for pager control
            this.PagerControlCarSearch.GridviewToPage = this.GridviewCarSearch;
            this.PagerControlCarSearch.GridviewSessionValues = (int)Gridviews.GridviewToPage.AvailabilityCarSearch;

            if (!Page.IsPostBack) {

                //Set page informtion on usercontrol
                base.SetPageInformationTitle("ATCarSearch", this.UserControlPageInformation, false);

                //Set Last update / next update labels
                base.SetDataImportUpdateLabel((int)ImportDetails.ImportType.Availability, this.UserControlPageInformation);



                //Check for query string
                List<ReportPreferences> queryStringResults = base.CheckForQueryString();


                if ((queryStringResults != null)) {

                    foreach (ReportPreferences row in queryStringResults) {
                        this.UserControlReportSettings.LoadReportSettingsControl(false);
                        this.UserControlReportSettings.SetUserDefaultSettingsAvailabilityTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id,
                            row.OPS_Region_Id, row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference,
                              row.Location, row.FleetName, row.Operstat, Convert.ToInt32(row.FleetStatus), Conversions.ConvertStringToDate(row.DateValue), Convert.ToInt32(row.DayOfWeek), Convert.ToInt32(row.DateRange));

                    }


                    UserControlReportSettings.ClickAvailabilityReportButton();

                    //Load data
                    GenerateReport();

                }
                else {
                    //Check if user as prefences
                    CheckReportPreferencesSettings();

                }

                //Set Default Sort Order
                SessionHandler.AvailabilityCarSearchSortOrder = "ASC";
                //Set current Page number and size
                SessionHandler.AvailabilityCarSearchCurrentPageNumber = 1;
                SessionHandler.AvailabilityCarSearchPageSize = 10;
                this.PagerControlCarSearch.PagerDropDownListRows.SelectedValue = "10";


            }

            //Set validation group
            this.ModalConfirmCarSearch.ValidationGroup = "GenerateReport";


        }


        protected void CheckReportPreferencesSettings() {
            List<ReportPreferences> preferences = base.GetPreferencesSettings();

            if ((preferences != null)) {
                //There should be only one row
                foreach (ReportPreferences row in preferences) {
                    this.UserControlReportSettings.LoadReportSettingsControl(false);
                    this.UserControlReportSettings.SetUserDefaultSettingsAvailabilityTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id,
                            row.OPS_Region_Id, row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference,
                                row.Location, row.FleetName, row.Operstat, Convert.ToInt32(row.FleetStatus), DateTime.Now, -1, -1);

                }
            }
            else {
                this.UserControlReportSettings.LoadReportSettingsControl(true);
            }

            //Update Update Panel
            this.UpdatePanelCarSearch.Update();

        }

        #endregion

        #region "Click Events"

        protected void ButtonGenerateReport_Click(object sender, System.EventArgs e) {
            //Clear Sorting and Direction Session Values
            SessionHandler.ClearAvailabilityCarSearchGridviewSessions();
            //Generarte Report
            GenerateReport();
        }


        protected void GenerateReport() {
            //Get Selection Values from reportselection user controls
            int selectedLogic = this.UserControlReportSettings.Logic;

            //Set values from report selection
            string country = this.UserControlReportSettings.Country;

            //Set default values for CMS / OPS logic
            int cms_pool_id = -1;
            string cms_pool = null;
            int cms_location_group_id = -1;
            string cms_location_group = null;

            int ops_region_id = -1;
            string ops_region = null;
            int ops_area_id = -1;
            string ops_area = null;

            //Check option logic
            switch (selectedLogic) {

                case (int)ReportSettings.OptionLogic.CMS:
                    //Set Values for CMS
                    cms_pool_id = this.UserControlReportSettings.CMS_Pool_Id;
                    cms_pool = this.UserControlReportSettings.CMS_Pool;
                    cms_location_group_id = this.UserControlReportSettings.CMS_Location_Group_Id;
                    cms_location_group = this.UserControlReportSettings.CMS_Location_Group;

                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    //Set Values for OPS
                    ops_region_id = this.UserControlReportSettings.OPS_Region_Id;
                    ops_region = this.UserControlReportSettings.OPS_Region;
                    ops_area_id = this.UserControlReportSettings.OPS_Area_Id;
                    ops_area = this.UserControlReportSettings.OPS_Area;

                    break;
            }

            //Set common values
            string location = this.UserControlReportSettings.Location;
            int car_segment_id = this.UserControlReportSettings.Car_Segment_Id == 0 ? -1 : UserControlReportSettings.Car_Segment_Id;
            string car_segment = this.UserControlReportSettings.Car_Segment;
            int car_class_id = this.UserControlReportSettings.Car_Class_Id == 0 ? -1 : UserControlReportSettings.Car_Class_Id;
            string car_class = this.UserControlReportSettings.Car_Class;
            int car_group_id = this.UserControlReportSettings.Car_Group_Id == 0 ? -1 : UserControlReportSettings.Car_Group_Id;
            string car_group = this.UserControlReportSettings.Car_Group;
            int noRev = this.UserControlReportSettings.NoRev == 0 ? -1 : UserControlReportSettings.NoRev;
            string ownArea = this.UserControlReportSettings.OwnArea;
            string modelCode = this.UserControlReportSettings.ModelCode;
            string status = this.UserControlReportSettings.Status;
            string selectBy = this.UserControlReportSettings.CarSearch_SelectBy;
            string fleet_name = this.UserControlReportSettings.Fleet_Name ?? "-1";

            //Set Selection Session Values
            this.SetSelectionSessionValues(country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id, fleet_name,
            ownArea, modelCode, status, noRev, selectBy);

            //Set Last Selection Value
            base.SaveLastSelectionToSession(selectedLogic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id);

            //Load Data
            AvailabilityCarSearch.GetCarSearchResult(country ?? "-1", cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location ?? "-1", fleet_name, car_segment_id, car_class_id, car_group_id,
            status, ownArea, modelCode, noRev, selectBy, Convert.ToInt32(SessionHandler.AvailabilityCarSearchPageSize), Convert.ToInt32(SessionHandler.AvailabilityCarSearchCurrentPageNumber),
                        null, this.PanelCarSearch, this.PagerControlCarSearch.PagerButtonFirst, this.PagerControlCarSearch.PagerButtonNext, this.PagerControlCarSearch.PagerButtonPrevious, this.PagerControlCarSearch.PagerButtonLast, this.PagerControlCarSearch.PagerLabelTotalPages, this.PagerControlCarSearch.PagerDropDownListPage, this.GridviewCarSearch, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateCarSearch);


            //Log Usage Statistics
            base.LogUsageStatistics((int)ReportStatistics.ReportName.AvailabilityCarSearch, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location);

            //Set Report Selection
            this.SetReportSelection(selectedLogic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, car_segment_id, car_class_id, car_group_id, location);


            this.UpdatePanelCarSearch.Update();

        }


        protected void ButtonSaveRemarks_Click(object sender, System.EventArgs e) {
            string serial = this.CarSearchDetails.VehicleIdentNbr;
            DateTime expectedResolutionDate = this.CarSearchDetails.NextOnRentDate;
            string remarks = this.CarSearchDetails.Remark;
            this.CarSearchDetails.ModalExtenderCarDetails.Hide();
            AvailabilityCarSearch.SaveVehicleRemark(serial, remarks, expectedResolutionDate);
            this.GridviewSortingAndPaging(null);
            this.UpdatePanelCarSearch.Update();

        }
        #endregion

        #region "Gridview Events"


        protected void GridviewCarSearch_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e) {

            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow) {
                Label labelErdPassed = (Label)e.Row.Cells[16].FindControl("LabelERDPASSED");
                Label labelRemark = (Label)e.Row.Cells[16].FindControl("LabelRemark");
                ImageButton imageButtonRemarks = (ImageButton)e.Row.Cells[16].FindControl("ImageButtonRemarks");

                if ((((labelErdPassed != null)) && ((labelRemark != null)))) {
                    if (labelRemark.Text == string.Empty) {
                        imageButtonRemarks.ImageUrl = "~/App.Images/pin-yellow.png";
                    }
                    else {
                        imageButtonRemarks.ImageUrl = "~/App.Images/pin-green.png";
                    }
                    //Change pin colour to red if the ERDPASSED field shows 1 (past the date)
                    if (labelErdPassed.Text == "1") {
                        imageButtonRemarks.ImageUrl = "~/App.Images/pin-red.png";
                    }

                }

            }

        }


        protected void GridviewCarSearch_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e) {
            if (e.CommandName.ToString() == "SelectVehicle") {
                string serial = e.CommandArgument.ToString();
                this.CarSearchDetails.LabelSerial.Text = serial;

                List<AvailabilityCarSearch.VehicleRemarks> results = AvailabilityCarSearch.GetVehicleRemarks(serial);


                foreach (AvailabilityCarSearch.VehicleRemarks item in results) {
                    this.CarSearchDetails.BDDays = item.BDDays;
                    this.CarSearchDetails.BlockDate = item.BlockDate;
                    this.CarSearchDetails.Carhold = item.carhold;
                    this.CarSearchDetails.GroupCharged = item.GroupCharged;
                    this.CarSearchDetails.DriverName = item.DriverName;
                    this.CarSearchDetails.DueDate = item.DueDate;
                    this.CarSearchDetails.Duetime = item.duetime;
                    this.CarSearchDetails.Duewwd = item.Duewwd;
                    this.CarSearchDetails.Group = item.Group;
                    this.CarSearchDetails.Kilometers = item.Kilometers;
                    this.CarSearchDetails.LastDocument = item.lastDocument;
                    this.CarSearchDetails.License = item.License;
                    this.CarSearchDetails.LstDate = item.LstDate;
                    this.CarSearchDetails.Lsttime = item.Lsttime;
                    this.CarSearchDetails.Lstwwd = item.Lstwwd;
                    this.CarSearchDetails.MMDays = item.MMDays;
                    this.CarSearchDetails.Model = item.Model;
                    this.CarSearchDetails.ModelCode = item.ModelCode;
                    this.CarSearchDetails.Movetype = item.Movetype;
                    this.CarSearchDetails.NonRev = item.NonRev;
                    this.CarSearchDetails.Operstat = item.Operstat;
                    this.CarSearchDetails.OwnArea = item.OwnArea;
                    this.CarSearchDetails.Prevwwd = item.Prevwwd;
                    this.CarSearchDetails.RegDate = item.RegDate;
                    this.CarSearchDetails.Unit = item.Unit;
                    this.CarSearchDetails.VehicleIdentNbr = item.VehicleIdentNbr;
                    this.CarSearchDetails.Remark = item.Remark;
                    this.CarSearchDetails.NextOnRentDate = Convert.ToDateTime(item.NextOnRentDate);

                }

                this.CarSearchDetails.LoadDetails();
                this.CarSearchDetails.ModalExtenderCarDetails.Show();

            }

        }


        protected void GridviewCarSearch_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e) {
            string sortExpression = string.Empty;
            if ((SessionHandler.AvailabilityCarSearchSortOrder == " ASC")) {
                SessionHandler.AvailabilityCarSearchSortOrder = " DESC";
                SessionHandler.AvailabilityCarSearchSortDirection = (int)AvailabilityCarSearch.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.AvailabilityCarSearchSortOrder;
            }
            else {
                SessionHandler.AvailabilityCarSearchSortOrder = " ASC";
                SessionHandler.AvailabilityCarSearchSortDirection = (int)AvailabilityCarSearch.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.AvailabilityCarSearchSortExpression = sortExpression.ToString();

            //Generarte Report
            GridviewSortingAndPaging(sortExpression);


        }


        protected void GridviewCarSearch_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e) {

            if ((e.Row != null) && e.Row.RowType == DataControlRowType.Header) {
                foreach (TableCell cell in e.Row.Cells) {

                    if (cell.HasControls()) {
                        LinkButton button = (LinkButton)cell.Controls[0];
                        if ((button != null)) {
                            Image image = new Image();
                            image.ImageUrl = "";

                            string sortExpression = SessionHandler.AvailabilityCarSearchSortExpression;
                            if (!(sortExpression == null)) {
                                string sortColumn = null;
                                if (sortExpression.Contains("DESC")) {
                                    sortColumn = sortExpression.Remove(sortExpression.Length - 5, 5);
                                }
                                else {
                                    sortColumn = sortExpression;
                                }
                                if (sortColumn == button.CommandArgument) {
                                    if (SessionHandler.AvailabilityCarSearchSortDirection == (int)AvailabilityCarSearch.SortDirection.Ascending) {
                                        image.ImageUrl = "";
                                    }
                                    else {
                                        image.ImageUrl = "";
                                    }
                                }
                            }

                            //cell.Controls.Add(image);
                        }
                    }
                }
            }
            this.UpdatePanelCarSearch.Update();

        }


        protected void GetPageIndex(object sender, System.EventArgs e) {
            //Generarte Report
            GridviewSortingAndPaging(SessionHandler.AvailabilityCarSearchSortExpression);

        }


        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e) {
            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10) {
                //Generarte Report
                GridviewSortingAndPaging(SessionHandler.AvailabilityCarSearchSortExpression);
            }

        }


        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e) {

            //Generarte Report
            GridviewSortingAndPaging(SessionHandler.AvailabilityCarSearchSortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression) {
            if (sortExpression == null) {
                sortExpression = SessionHandler.AvailabilityCarSearchSortExpression;
            }

            //--Added by Gavin 2/4/12--
            AvailabilityCarSearch.TheCarFilter = CarFilter1;

            AvailabilityCarSearch.GetCarSearchResult(SessionHandler.AvailabilityCarSearchCountry, Convert.ToInt32(SessionHandler.AvailabilityCarSearchCMSPoolId),
                    SessionHandler.AvailabilityCarSearchCMSLocationGroupCode ?? 0, Convert.ToInt32(SessionHandler.AvailabilityCarSearchOPSRegionId), Convert.ToInt32(SessionHandler.AvailabilityCarSearchOPSAreaId),
                        SessionHandler.AvailabilityCarSearchLocation, SessionHandler.AvailabilityCarSearchFleetName, Convert.ToInt32(SessionHandler.AvailabilityCarSearchCarSegmentId),
                            Convert.ToInt32(SessionHandler.AvailabilityCarSearchCarClassId), Convert.ToInt32(SessionHandler.AvailabilityCarSearchCarGroupId),
                                SessionHandler.AvailabilityCarSearchStatus, SessionHandler.AvailabilityCarSearchOwnArea, SessionHandler.AvailabilityCarSearchModelCode,
                                    Convert.ToInt32(SessionHandler.AvailabilityCarSearchNoRev), SessionHandler.AvailabilityCarSearchSelectBy, Convert.ToInt32(SessionHandler.AvailabilityCarSearchPageSize),
                                        Convert.ToInt32(SessionHandler.AvailabilityCarSearchCurrentPageNumber), sortExpression, this.PanelCarSearch, this.PagerControlCarSearch.PagerButtonFirst,
                                            this.PagerControlCarSearch.PagerButtonNext, this.PagerControlCarSearch.PagerButtonPrevious, this.PagerControlCarSearch.PagerButtonLast,
                                                this.PagerControlCarSearch.PagerLabelTotalPages, this.PagerControlCarSearch.PagerDropDownListPage, this.GridviewCarSearch,
                                                    this.LabelTotalRecordsDisplay, this.EmptyDataTemplateCarSearch);


            this.UpdatePanelCarSearch.Update();

        }

        protected void SetSelectionSessionValues(string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id,
                                                    string location, int car_segment_id, int car_class_id, int car_group_id, string fleet_name,
                                                        string ownArea, string modelCode, string status, int noRev, string selectBy) {



            SessionHandler.AvailabilityCarSearchCountry = country;
            SessionHandler.AvailabilityCarSearchCMSPoolId = cms_pool_id;
            SessionHandler.AvailabilityCarSearchCMSLocationGroupCode = cms_location_group_id;
            SessionHandler.AvailabilityCarSearchOPSRegionId = ops_region_id;
            SessionHandler.AvailabilityCarSearchOPSAreaId = ops_area_id;
            SessionHandler.AvailabilityCarSearchLocation = location;
            SessionHandler.AvailabilityCarSearchCarSegmentId = car_segment_id;
            SessionHandler.AvailabilityCarSearchCarClassId = car_class_id;
            SessionHandler.AvailabilityCarSearchCarGroupId = car_group_id;
            SessionHandler.AvailabilityCarSearchFleetName = fleet_name;
            SessionHandler.AvailabilityCarSearchNoRev = noRev;
            SessionHandler.AvailabilityCarSearchSelectBy = selectBy;
            SessionHandler.AvailabilityCarSearchStatus = status;
            SessionHandler.AvailabilityCarSearchModelCode = modelCode;
            SessionHandler.AvailabilityCarSearchOwnArea = ownArea;



        }
        #endregion

        #region "Report Selection"

        protected void SetReportSelection(int selectedLogic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id,
                                            int car_segment_id, int car_class_id, int car_group_id, string location) {
            //Set Report Selection User Control
            //Logic
            this.UserControlReportSelections.Logic = selectedLogic;

            //Country
            if (country == "-1") {
                this.UserControlReportSelections.Country = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Country = country;
            }

            //Select logic
            switch (selectedLogic) {

                case (int)ReportSettings.OptionLogic.CMS:
                    //CMS Pool
                    if (cms_pool_id == -1) {
                        this.UserControlReportSelections.CMS_Pool = Resources.lang.ReportSettingsALLSelection;
                    }
                    else {
                        this.UserControlReportSelections.CMS_Pool = this.UserControlReportSettings.CMS_Pool;
                    }
                    //CMS Location Group
                    if (cms_location_group_id == -1) {
                        this.UserControlReportSelections.CMS_Location_Group = Resources.lang.ReportSettingsALLSelection;
                    }
                    else {
                        this.UserControlReportSelections.CMS_Location_Group = this.UserControlReportSettings.CMS_Location_Group;
                    }

                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    //OPS Region
                    if (ops_region_id == -1) {
                        this.UserControlReportSelections.OPS_Region = Resources.lang.ReportSettingsALLSelection;
                    }
                    else {
                        this.UserControlReportSelections.OPS_Region = this.UserControlReportSettings.OPS_Region;
                    }
                    //OPS Area
                    if (ops_area_id == -1) {
                        this.UserControlReportSelections.OPS_Area = Resources.lang.ReportSettingsALLSelection;
                    }
                    else {
                        this.UserControlReportSelections.OPS_Area = this.UserControlReportSettings.OPS_Area;
                    }
                    break;
            }

            //Location
            if (location == "-1") {
                this.UserControlReportSelections.Location = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Location = location;
            }

            //Car Segment
            if (car_segment_id == -1) {
                this.UserControlReportSelections.Car_Segment = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Car_Segment = this.UserControlReportSettings.Car_Segment;
            }

            //Car Class
            if (car_class_id == -1) {
                this.UserControlReportSelections.Car_Class = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Car_Class = this.UserControlReportSettings.Car_Class;
            }

            //Car Group
            if (car_group_id == -1) {
                this.UserControlReportSelections.Car_Group = Resources.lang.ReportSettingsALLSelection;
            }
            else {
                this.UserControlReportSelections.Car_Group = this.UserControlReportSettings.Car_Group;
            }


            //Set Report Selection for tool and page
            this.UserControlReportSelections.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSelections.MarsPage = (int)ReportSettings.ReportSettingsPage.ATCarSearch;

            //Display report selection
            this.UserControlReportSelections.ShowReportSelections();
        }
        protected void OnFilterClicked(object o, EventArgs e) {
            GridviewSortingAndPaging(SessionHandler.AvailabilityCarSearchSortExpression);
        }

        protected void OnDownloadClicked(object o, EventArgs e) {
            CarFilter1.generateExcel(getCardSearchList());
        }
        #endregion

        private List<App.BLL.AvailabilityCarSearch.CarSearchDetails> getCardSearchList() {
            AvailabilityCarSearch.TheCarFilter = CarFilter1;

            return AvailabilityCarSearch.GetCarSearchList(SessionHandler.AvailabilityCarSearchCountry, Convert.ToInt32(SessionHandler.AvailabilityCarSearchCMSPoolId),
                    SessionHandler.AvailabilityCarSearchCMSLocationGroupCode ?? 0, Convert.ToInt32(SessionHandler.AvailabilityCarSearchOPSRegionId), Convert.ToInt32(SessionHandler.AvailabilityCarSearchOPSAreaId),
                        SessionHandler.AvailabilityCarSearchLocation, SessionHandler.AvailabilityCarSearchFleetName, Convert.ToInt32(SessionHandler.AvailabilityCarSearchCarSegmentId),
                            Convert.ToInt32(SessionHandler.AvailabilityCarSearchCarClassId), Convert.ToInt32(SessionHandler.AvailabilityCarSearchCarGroupId),
                                SessionHandler.AvailabilityCarSearchStatus, SessionHandler.AvailabilityCarSearchOwnArea, SessionHandler.AvailabilityCarSearchModelCode,
                                    Convert.ToInt32(SessionHandler.AvailabilityCarSearchNoRev), SessionHandler.AvailabilityCarSearchSelectBy, Convert.ToInt32(SessionHandler.AvailabilityCarSearchPageSize),
                                        Convert.ToInt32(SessionHandler.AvailabilityCarSearchCurrentPageNumber), SessionHandler.AvailabilityCarSearchSortExpression, this.PanelCarSearch, this.PagerControlCarSearch.PagerButtonFirst,
                                            this.PagerControlCarSearch.PagerButtonNext, this.PagerControlCarSearch.PagerButtonPrevious, this.PagerControlCarSearch.PagerButtonLast,
                                                this.PagerControlCarSearch.PagerLabelTotalPages, this.PagerControlCarSearch.PagerDropDownListPage, this.GridviewCarSearch,
                                                    this.LabelTotalRecordsDisplay, this.EmptyDataTemplateCarSearch);
        }
    }
}