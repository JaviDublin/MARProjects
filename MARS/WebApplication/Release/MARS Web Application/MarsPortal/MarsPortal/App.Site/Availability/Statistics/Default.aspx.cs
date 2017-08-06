using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.AvailabilityTool.Statistics
{
    public partial class Default : PageBase
    {
        #region "Page Events"

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //Set Page title
            this.Page.Title = "MARS - Availability Tool";

            //Set report settings tool
            this.UserControlReportSettings.MarsTool = (int)ReportSettings.ReportSettingsTool.Statistics;
            this.UserControlReportSettings.MarsPage = (int)ReportSettings.ReportSettingsPage.ATStatistics;

            //Settings for pager controls
            this.PagerControlStatisticsSelection.GridviewToPage = this.GridviewStatisticsSelection;
            this.PagerControlStatisticsSelection.GridviewSessionValues = (int)Gridviews.GridviewToPage.AvailabilityStatisticsSelection;

            this.PagerControlStatisticsDate.GridviewToPage = this.GridviewStatisticsDate;
            this.PagerControlStatisticsDate.GridviewSessionValues = (int)Gridviews.GridviewToPage.AvailabilityStatisticsDates;

            if (!Page.IsPostBack)
            {
                ddlAvailabilityOrPooling.Enabled = Mars.Properties.Settings.Default.UatRelease;

                //Set page informtion on usercontrol
                base.SetPageInformationTitle("ATStatistics", this.UserControlPageInformation, false);

                //Set Last update / next update labels
                base.SetDataImportUpdateLabel((int)ImportDetails.ImportType.Availability, this.UserControlPageInformation);

                //Check if user as prefences
                CheckReportPreferencesSettings();

                //Set Default Sort Order
                SessionHandler.AvailabilityStatisticsSelectionSortOrder = "ASC";
                //Set current Page number and size
                SessionHandler.AvailabilityStatisticsSelectionCurrentPageNumber = 1;
                SessionHandler.AvailabilityStatisticsSelectionPageSize = 10;
                this.PagerControlStatisticsSelection.PagerDropDownListRows.SelectedValue = "10";

                //Set Default Sort Order
                SessionHandler.AvailabilityStatisticsDateSortOrder = "ASC";
                //Set current Page number and size
                SessionHandler.AvailabilityStatisticsDateCurrentPageNumber = 1;
                SessionHandler.AvailabilityStatisticsDatePageSize = 10;
                this.PagerControlStatisticsDate.PagerDropDownListRows.SelectedValue = "10";

            }

            //Set validation group
            this.ModalConfirmStatistics.ValidationGroup = "GenerateReport";

            //Set Empty data text
            this.EmptyDataTemplateStatisticsDate.NoResultGridview = (int)Gridviews.GridviewNoDataMessage.StatisticsDate;
            this.EmptyDataTemplateStatisticsSelection.NoResultGridview = (int)Gridviews.GridviewNoDataMessage.StatisticsSelection;


        }


        protected void CheckReportPreferencesSettings()
        {
            List<ReportPreferences> preferences = base.GetPreferencesSettings();

            if ((preferences != null))
            {
                //There should be only one row
                foreach (ReportPreferences row in preferences)
                {
                    this.UserControlReportSettings.LoadReportSettingsControl(false);
                    this.UserControlReportSettings.SetUserDefaultSettingsStatistics(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id, row.OPS_Region_Id, row.OPS_Area_Id, row.UserPreference, row.Location);

                }
            }
            else
            {
                this.UserControlReportSettings.LoadReportSettingsControl(true);
            }

            //Update Update Panel
            this.UpdatePanelStatistics.Update();

        }
        #endregion

        #region "Click Events"

        protected void ButtonGenerateReport_Click(object sender, System.EventArgs e)
        {
            //Clear Sorting and Direction Session Values
            SessionHandler.ClearAvailabilityStatisticsSelectionGridviewSessions();
            SessionHandler.ClearAvailabilityStatisticsDateGridviewSessions();
            //Generarte Report
            GenerateReport();
        }


        protected void GenerateReport()
        {
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
            switch (selectedLogic)
            {

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
            string racfId = this.UserControlReportSettings.RacfId;


            //Statistics values
            DateTime startDate = this.UserControlReportSettings.StatisticsStartDate;
            DateTime endDate = this.UserControlReportSettings.StatisticsEndDate;


            //Set Selection Session Values
            this.SetSelectionSessionValues(country, selectedLogic, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, startDate, endDate, racfId);

            var marsTool = int.Parse(ddlAvailabilityOrPooling.SelectedValue);
            var poolingSelected = marsTool == 4;

            GridviewStatisticsDate.Visible = !poolingSelected;
            GridviewStatisticsDatePooling.Visible = poolingSelected;

            GridviewStatisticsSelection.Visible = !poolingSelected;
            GridviewStatisticsSelectionPooling.Visible = poolingSelected;

            GridviewStatisticsSelectionTotals.Visible = !poolingSelected;
            GridviewStatisticsSelectionTotalsPooling.Visible = poolingSelected;

            GridviewStatisticsDateTotals.Visible = !poolingSelected;
            GridviewStatisticsDateTotalsPooling.Visible = poolingSelected;

            var gridViewStatisticsSelection = !poolingSelected ? GridviewStatisticsSelection : GridviewStatisticsSelectionPooling;
            var gridViewStatisticsDate = !poolingSelected ? GridviewStatisticsDate : GridviewStatisticsDatePooling;
            var gridViewStatisticsSelectionTotal = !poolingSelected ? GridviewStatisticsSelectionTotals : GridviewStatisticsSelectionTotalsPooling;
            var gridViewStatisticsDateTotal = !poolingSelected ? GridviewStatisticsDateTotals : GridviewStatisticsDateTotalsPooling;


            //Load Data By Selection
            ReportStatistics.SelectStatistics(marsTool, selectedLogic, country, cms_pool_id, cms_location_group_id, racfId, ops_region_id, ops_area_id, location, startDate,
            endDate, Convert.ToInt32(SessionHandler.AvailabilityStatisticsSelectionPageSize), Convert.ToInt32(SessionHandler.AvailabilityStatisticsSelectionCurrentPageNumber), null, this.PagerControlStatisticsSelection.PagerButtonFirst, this.PagerControlStatisticsSelection.PagerButtonNext, this.PagerControlStatisticsSelection.PagerButtonPrevious, this.PagerControlStatisticsSelection.PagerButtonLast, this.PagerControlStatisticsSelection.PagerLabelTotalPages, this.PagerControlStatisticsSelection.PagerDropDownListPage,
            gridViewStatisticsSelection, gridViewStatisticsSelectionTotal, this.LabelTotalRecordsDisplayStatisticsSelection, (int)ReportStatistics.StatisticsType.BySelection, this.PanelStatisticsSelection, this.PanelStatisticsSelectionEmptyDataTemplate);

            //Load Data By Date
            ReportStatistics.SelectStatistics(marsTool, selectedLogic, country, cms_pool_id, cms_location_group_id, racfId, ops_region_id, ops_area_id, location, startDate,
            endDate, Convert.ToInt32(SessionHandler.AvailabilityStatisticsDatePageSize), Convert.ToInt32(SessionHandler.AvailabilityStatisticsDateCurrentPageNumber), null, this.PagerControlStatisticsDate.PagerButtonFirst, this.PagerControlStatisticsDate.PagerButtonNext, this.PagerControlStatisticsDate.PagerButtonPrevious, this.PagerControlStatisticsDate.PagerButtonLast, this.PagerControlStatisticsDate.PagerLabelTotalPages, this.PagerControlStatisticsDate.PagerDropDownListPage,
            gridViewStatisticsDate, gridViewStatisticsDateTotal, this.LabelTotalRecordsDisplayStatisticsDate, (int)ReportStatistics.StatisticsType.ByDate, this.PanelStatisticsDate, this.PanelStatisticsDateEmptyDataTemplate);

            //Set Report Selection User Control
            SetReportSelection(selectedLogic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, startDate, endDate, racfId);


            //Set Last Selection Value
            base.SaveLastSelectionToSession(selectedLogic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, null, null, null);


            this.UpdatePanelStatistics.Update();

        }


        #endregion

        #region "Gridview Events"

        #region "Gridview Selection"


        protected void GridviewStatisticsSelection_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if ((e.Row != null) && e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {

                    if (cell.HasControls())
                    {
                        LinkButton button = (LinkButton)cell.Controls[0];
                        if ((button != null))
                        {
                            Image image = new Image();

                            string sortExpression = SessionHandler.AvailabilityStatisticsSelectionSortExpression;
                            if (!(sortExpression == null))
                            {
                                string sortColumn = null;
                                if (sortExpression.Contains("DESC"))
                                {
                                    sortColumn = sortExpression.Remove(sortExpression.Length - 5, 5);
                                }
                                else
                                {
                                    sortColumn = sortExpression;
                                }
                                if (sortColumn == button.CommandArgument)
                                {
                                    if (SessionHandler.AvailabilityStatisticsSelectionSortDirection == (int)ReportStatistics.SortDirection.Ascending)
                                    {
                                        image.ImageUrl = "~/App.Images/sort-ascending.gif";
                                    }
                                    else
                                    {
                                        image.ImageUrl = "~/App.Images/sort-descending.gif";
                                    }
                                    cell.Controls.Add(image);
                                }
                            }


                        }
                    }
                }
            }
            this.UpdatePanelStatistics.Update();


        }


        protected void GridviewStatisticsSelection_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            string sortExpression = string.Empty;
            if ((SessionHandler.AvailabilityStatisticsSelectionSortOrder == " ASC"))
            {
                SessionHandler.AvailabilityStatisticsSelectionSortOrder = " DESC";
                SessionHandler.AvailabilityStatisticsSelectionSortDirection = (int)ReportStatistics.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.AvailabilityStatisticsSelectionSortOrder;
            }
            else
            {
                SessionHandler.AvailabilityStatisticsSelectionSortOrder = " ASC";
                SessionHandler.AvailabilityStatisticsSelectionSortDirection = (int)ReportStatistics.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.AvailabilityStatisticsSelectionSortExpression = sortExpression.ToString();

            //Generarte Report
            GridviewSortingAndPagingSelection(sortExpression);


        }


        protected void GetPageIndexSelection(object sender, System.EventArgs e)
        {
            //Generarte Report
            GridviewSortingAndPagingSelection(SessionHandler.AvailabilityStatisticsSelectionSortExpression);


        }


        protected void DropDownListRowsSelection_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplayStatisticsSelection.Text);
            if (totalRecords > 10)
            {
                //Generarte Report
                GridviewSortingAndPagingSelection(SessionHandler.AvailabilityStatisticsSelectionSortExpression);
            }


        }


        protected void DropDownListPageSelection_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Generarte Report
            GridviewSortingAndPagingSelection(SessionHandler.AvailabilityStatisticsSelectionSortExpression);


        }


        protected void GridviewSortingAndPagingSelection(string sortExpression)
        {
            var marsTool = int.Parse(ddlAvailabilityOrPooling.SelectedValue);

            var gridViewStatisticsSelection = marsTool != 4 ? GridviewStatisticsSelection : GridviewStatisticsSelectionPooling;
            
            var gridViewStatisticsSelectionTotal = marsTool != 4 ? GridviewStatisticsSelectionTotals : GridviewStatisticsSelectionTotalsPooling;
            

            ReportStatistics.SelectStatistics(marsTool, Convert.ToInt32(SessionHandler.AvailabilityStatisticsLogic),
                                                SessionHandler.AvailabilityStatisticsCountry, Convert.ToInt32(SessionHandler.AvailabilityStatisticsCMSPoolId),
                                                SessionHandler.AvailabilityStatisticsCMSLocationGroupCode ?? 0, SessionHandler.AvailabilityStatisticsRacfID,
                                                Convert.ToInt32(SessionHandler.AvailabilityStatisticsOPSRegionId), Convert.ToInt32(SessionHandler.AvailabilityStatisticsOPSAreaId),
                                                SessionHandler.AvailabilityStatisticsLocation, Convert.ToDateTime(SessionHandler.AvailabilityStatisticsStartDate),
                                                 Convert.ToDateTime(SessionHandler.AvailabilityStatisticsEndDate), Convert.ToInt32(SessionHandler.AvailabilityStatisticsSelectionPageSize),
                                                 Convert.ToInt32(SessionHandler.AvailabilityStatisticsSelectionCurrentPageNumber), sortExpression,
                                                 this.PagerControlStatisticsSelection.PagerButtonFirst, this.PagerControlStatisticsSelection.PagerButtonNext,
                                                 this.PagerControlStatisticsSelection.PagerButtonPrevious, this.PagerControlStatisticsSelection.PagerButtonLast,
                                                 this.PagerControlStatisticsSelection.PagerLabelTotalPages, this.PagerControlStatisticsSelection.PagerDropDownListPage,
                                                  gridViewStatisticsSelection, gridViewStatisticsSelectionTotal, this.LabelTotalRecordsDisplayStatisticsSelection,
                                                  (int)ReportStatistics.StatisticsType.BySelection, this.PanelStatisticsSelection, this.PanelStatisticsSelectionEmptyDataTemplate);

            this.UpdatePanelStatistics.Update();

        }


        #endregion

        #region "Gridview Dates"


        protected void GridviewStatisticsDate_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if ((e.Row != null) && e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {

                    if (cell.HasControls())
                    {
                        LinkButton button = (LinkButton)cell.Controls[0];
                        if ((button != null))
                        {
                            Image image = new Image();

                            string sortExpression = SessionHandler.AvailabilityStatisticsDateSortExpression;
                            if (!(sortExpression == null))
                            {
                                string sortColumn = null;
                                if (sortExpression.Contains("DESC"))
                                {
                                    sortColumn = sortExpression.Remove(sortExpression.Length - 5, 5);
                                }
                                else
                                {
                                    sortColumn = sortExpression;
                                }
                                if (sortColumn == button.CommandArgument)
                                {
                                    if (SessionHandler.AvailabilityStatisticsDateSortDirection == (int)ReportStatistics.SortDirection.Ascending)
                                    {
                                        image.ImageUrl = "~/App.Images/sort-ascending.gif";
                                    }
                                    else
                                    {
                                        image.ImageUrl = "~/App.Images/sort-descending.gif";
                                    }
                                    cell.Controls.Add(image);
                                }
                            }


                        }
                    }
                }
            }
            this.UpdatePanelStatistics.Update();


        }


        protected void GridviewStatisticsDate_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            string sortExpression = string.Empty;
            if ((SessionHandler.AvailabilityStatisticsDateSortOrder == " ASC"))
            {
                SessionHandler.AvailabilityStatisticsDateSortOrder = " DESC";
                SessionHandler.AvailabilityStatisticsDateSortDirection = (int)ReportStatistics.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.AvailabilityStatisticsDateSortOrder;
            }
            else
            {
                SessionHandler.AvailabilityStatisticsDateSortOrder = " ASC";
                SessionHandler.AvailabilityStatisticsDateSortDirection = (int)ReportStatistics.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.AvailabilityStatisticsDateSortExpression = sortExpression.ToString();

            //Generarte Report
            GridviewSortingAndPagingDate(sortExpression);


        }


        protected void GetPageIndexDate(object sender, System.EventArgs e)
        {

            //Generarte Report
            GridviewSortingAndPagingDate(SessionHandler.AvailabilityStatisticsDateSortExpression);


        }


        protected void DropDownListRowsDate_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplayStatisticsDate.Text);
            if (totalRecords > 10)
            {
                //Generarte Report
                GridviewSortingAndPagingDate(SessionHandler.AvailabilityStatisticsDateSortExpression);
            }


        }


        protected void DropDownListPageDate_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Generarte Report
            GridviewSortingAndPagingDate(SessionHandler.AvailabilityStatisticsDateSortExpression);


        }


        protected void GridviewSortingAndPagingDate(string sortExpression)
        {
            var marsTool = int.Parse(ddlAvailabilityOrPooling.SelectedValue);

            var gridViewStatisticsDate = marsTool != 4 ? GridviewStatisticsDate : GridviewStatisticsDatePooling;
            var gridViewStatisticsDateTotal = marsTool != 4 ? GridviewStatisticsDateTotals : GridviewStatisticsDateTotalsPooling;


            ReportStatistics.SelectStatistics(marsTool, Convert.ToInt32(SessionHandler.AvailabilityStatisticsLogic),
                                                SessionHandler.AvailabilityStatisticsCountry, Convert.ToInt32(SessionHandler.AvailabilityStatisticsCMSPoolId),
                                                  SessionHandler.AvailabilityStatisticsCMSLocationGroupCode ?? 0, SessionHandler.AvailabilityStatisticsRacfID,
                                                    Convert.ToInt32(SessionHandler.AvailabilityStatisticsOPSRegionId), Convert.ToInt32(SessionHandler.AvailabilityStatisticsOPSAreaId),
                                                        SessionHandler.AvailabilityStatisticsLocation, Convert.ToDateTime(SessionHandler.AvailabilityStatisticsStartDate),
                                                         Convert.ToDateTime(SessionHandler.AvailabilityStatisticsEndDate), Convert.ToInt32(SessionHandler.AvailabilityStatisticsDatePageSize),
                                                            Convert.ToInt32(SessionHandler.AvailabilityStatisticsDateCurrentPageNumber), sortExpression,
                                                                this.PagerControlStatisticsDate.PagerButtonFirst, this.PagerControlStatisticsDate.PagerButtonNext,
                                                                    this.PagerControlStatisticsDate.PagerButtonPrevious, this.PagerControlStatisticsDate.PagerButtonLast,
                                                                        this.PagerControlStatisticsDate.PagerLabelTotalPages, this.PagerControlStatisticsDate.PagerDropDownListPage,
                                                                            gridViewStatisticsDate, gridViewStatisticsDateTotal, this.LabelTotalRecordsDisplayStatisticsDate,
                                                                                (int)ReportStatistics.StatisticsType.ByDate, this.PanelStatisticsDate, this.PanelStatisticsDateEmptyDataTemplate);

            this.UpdatePanelStatistics.Update();


        }
        #endregion

        #region "Session Values"

        protected void SetSelectionSessionValues(string country, int selectedLogic, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id, string location, DateTime startDate, DateTime endDate, string racfId)
        {


            SessionHandler.AvailabilityStatisticsCountry = country;
            SessionHandler.AvailabilityStatisticsLogic = selectedLogic;
            SessionHandler.AvailabilityStatisticsCMSPoolId = cms_pool_id;
            SessionHandler.AvailabilityStatisticsCMSLocationGroupCode = cms_location_group_id;
            SessionHandler.AvailabilityStatisticsOPSRegionId = ops_region_id;
            SessionHandler.AvailabilityStatisticsOPSAreaId = ops_area_id;
            SessionHandler.AvailabilityStatisticsLocation = location;
            SessionHandler.AvailabilityStatisticsStartDate = startDate;
            SessionHandler.AvailabilityStatisticsEndDate = endDate;
            SessionHandler.AvailabilityStatisticsRacfID = racfId;


        }
        #endregion

        #endregion

        #region "Report Selection"

        protected void SetReportSelection(int selectedLogic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id, string location, DateTime startDate, DateTime endDate, string racfId)
        {
            //Set Report Selection User Control
            //Logic
            this.UserControlReportSelections.Logic = selectedLogic;

            //Country
            if (country == "-1")
            {
                this.UserControlReportSelections.Country = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Country = country;
            }

            //Select logic
            switch (selectedLogic)
            {

                case (int)ReportSettings.OptionLogic.CMS:
                    //CMS Pool
                    if (cms_pool_id == -1)
                    {
                        this.UserControlReportSelections.CMS_Pool = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.CMS_Pool = this.UserControlReportSettings.CMS_Pool;
                    }
                    //CMS Location Group
                    if (cms_location_group_id == -1)
                    {
                        this.UserControlReportSelections.CMS_Location_Group = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.CMS_Location_Group = this.UserControlReportSettings.CMS_Location_Group;
                    }

                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    //OPS Region
                    if (ops_region_id == -1)
                    {
                        this.UserControlReportSelections.OPS_Region = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.OPS_Region = this.UserControlReportSettings.OPS_Region;
                    }
                    //OPS Area
                    if (ops_area_id == -1)
                    {
                        this.UserControlReportSelections.OPS_Area = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.OPS_Area = this.UserControlReportSettings.OPS_Area;
                    }
                    break;
            }

            //Location
            if (location == "-1")
            {
                this.UserControlReportSelections.Location = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Location = location;
            }

            //Dates
            this.UserControlReportSelections.StartDate = string.Format("{0:dd/MM/yyyy}", startDate);
            this.UserControlReportSelections.EndDate = string.Format("{0:dd/MM/yyyy}", endDate);

            if ((racfId == null || racfId == string.Empty))
            {
                this.UserControlReportSelections.RacfID = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.RacfID = racfId;
            }


            //Set Report Selection for tool and page
            this.UserControlReportSelections.MarsTool = (int)ReportSettings.ReportSettingsTool.Statistics;
            this.UserControlReportSelections.MarsPage = (int)ReportSettings.ReportSettingsPage.ATStatistics;
            //Display report selection
            this.UserControlReportSelections.ShowReportSelections();
        }

        #endregion
    }
}