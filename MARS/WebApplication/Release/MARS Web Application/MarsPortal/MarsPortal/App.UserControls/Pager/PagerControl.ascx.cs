using System;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Pager
{
    public partial class PagerControl : System.Web.UI.UserControl
    {
        #region "Properties and Fields"

        private GridView _gridViewToPage;

        private int _gridViewSessionValues;
        public GridView GridviewToPage
        {
            get { return _gridViewToPage; }
            set { _gridViewToPage = value; }
        }

        public int GridviewSessionValues
        {
            get { return _gridViewSessionValues; }
            set { _gridViewSessionValues = value; }
        }

        public System.Web.UI.WebControls.DropDownList PagerDropDownListPage
        {
            get { return this.DropDownListPage; }
        }

        public Button PagerButtonFirst
        {
            get { return this.ButtonFirst; }
        }

        public Button PagerButtonNext
        {
            get { return this.ButtonNext; }
        }

        public Button PagerButtonPrevious
        {
            get { return this.ButtonPrevious; }
        }

        public Button PagerButtonLast
        {
            get { return this.ButtonLast; }
        }

        public Label PagerLabelTotalPages
        {
            get { return this.LabelTotalPages; }
        }

        public System.Web.UI.WebControls.DropDownList PagerDropDownListRows
        {
            get { return this.DropDownListRows; }
        }

        #endregion

        #region "Click Events"

        public event EventHandler PageIndexCommand;
        public event EventHandler DropDownListRowsSelectedIndexChanged;
        public event EventHandler DropDownListPageSelectedIndexChanged;


        protected void GetPageIndex(object sender, CommandEventArgs e)
        {

            switch (e.CommandName)
            {

                case "First":
                    switch (GridviewSessionValues)
                    {

                        case (int)Gridviews.GridviewToPage.AvailabilityCarSearch:
                            SessionHandler.AvailabilityCarSearchCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.AvailabilityStatisticsSelection:
                            SessionHandler.AvailabilityStatisticsSelectionCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.AvailabilityStatisticsDates:
                            SessionHandler.AvailabilityStatisticsDateCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MaintenanceUsers:
                            SessionHandler.MaintanenceUsersCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCountry:
                            SessionHandler.MappingCountryCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingAreaCode:
                            SessionHandler.MappingAreaCodeCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCMSPool:
                            SessionHandler.MappingCMSPoolCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCMSLocation:
                            SessionHandler.MappingCMSLocationCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingOPSRegion:
                            SessionHandler.MappingOPSRegionCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingOPSArea:
                            SessionHandler.MappingOPSAreaCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingLocation:
                            SessionHandler.MappingLocationCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarSegment:
                            SessionHandler.MappingCarSegmentCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarClass:
                            SessionHandler.MappingCarClassCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarGroup:
                            SessionHandler.MappingCarGroupCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingModelCode:
                            SessionHandler.MappingModelCodeCurrentPageNumber = 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingVehiclesLease:
                            SessionHandler.MappingVehiclesLeaseCurrentPageNumber = 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueCarSearch:
                            SessionHandler.NonRevCarSearchCurrentPageNumber = 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueReportStart:
                            SessionHandler.NonRevReportStartCurrentPageNumber = 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApproval:
                            SessionHandler.NonRevApprovalCurrentPageNumber = 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueComparison:
                             SessionHandler.NonRevComparisonCurrentPageNumber = 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueHistoricalTrend:
                            SessionHandler.NonRevHTrendCurrentPageNumber = 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsers:
                            SessionHandler.NonRevApprovalUsersCurrentPageNumber = 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsersList:
                            SessionHandler.NonRevApprovalUsersListCurrentPageNumber = 1;
                            break;

                    }
                    break;
                case "Previous":
                    switch (GridviewSessionValues)
                    {

                        case (int)Gridviews.GridviewToPage.AvailabilityCarSearch:
                            SessionHandler.AvailabilityCarSearchCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.AvailabilityStatisticsSelection:
                            SessionHandler.AvailabilityStatisticsSelectionCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.AvailabilityStatisticsDates:
                            SessionHandler.AvailabilityStatisticsDateCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MaintenanceUsers:
                            SessionHandler.MaintanenceUsersCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCountry:
                            SessionHandler.MappingCountryCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingAreaCode:
                            SessionHandler.MappingAreaCodeCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCMSPool:
                            SessionHandler.MappingCMSPoolCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCMSLocation:
                            SessionHandler.MappingCMSLocationCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingOPSRegion:
                            SessionHandler.MappingOPSRegionCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingOPSArea:
                            SessionHandler.MappingOPSAreaCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingLocation:
                            SessionHandler.MappingLocationCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarSegment:
                            SessionHandler.MappingCarSegmentCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarClass:
                            SessionHandler.MappingCarClassCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarGroup:
                            SessionHandler.MappingCarGroupCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingModelCode:
                            SessionHandler.MappingModelCodeCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingVehiclesLease:
                            SessionHandler.MappingVehiclesLeaseCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueCarSearch:
                            SessionHandler.NonRevCarSearchCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueReportStart:
                            SessionHandler.NonRevReportStartCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApproval:
                            SessionHandler.NonRevApprovalCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueComparison:
                            SessionHandler.NonRevComparisonCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueHistoricalTrend:
                            SessionHandler.NonRevHTrendCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsers:
                            SessionHandler.NonRevApprovalUsersCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsersList:
                            SessionHandler.NonRevApprovalUsersListCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) - 1;
                            break;

                    }
                    break;
                case "Next":
                    switch (GridviewSessionValues)
                    {

                        case (int)Gridviews.GridviewToPage.AvailabilityCarSearch:
                            SessionHandler.AvailabilityCarSearchCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.AvailabilityStatisticsSelection:
                            SessionHandler.AvailabilityStatisticsSelectionCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.AvailabilityStatisticsDates:
                            SessionHandler.AvailabilityStatisticsDateCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MaintenanceUsers:
                            SessionHandler.MaintanenceUsersCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCountry:
                            SessionHandler.MappingCountryCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingAreaCode:
                            SessionHandler.MappingAreaCodeCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCMSPool:
                            SessionHandler.MappingCMSPoolCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCMSLocation:
                            SessionHandler.MappingCMSLocationCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingOPSRegion:
                            SessionHandler.MappingOPSRegionCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingOPSArea:
                            SessionHandler.MappingOPSAreaCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingLocation:
                            SessionHandler.MappingLocationCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarSegment:
                            SessionHandler.MappingCarSegmentCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarClass:
                            SessionHandler.MappingCarClassCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarGroup:
                            SessionHandler.MappingCarGroupCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingModelCode:
                            SessionHandler.MappingModelCodeCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;

                            break;
                        case (int)Gridviews.GridviewToPage.MappingVehiclesLease:
                            SessionHandler.MappingVehiclesLeaseCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueCarSearch:
                            SessionHandler.NonRevCarSearchCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueReportStart:
                            SessionHandler.NonRevReportStartCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1; ;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApproval:
                            SessionHandler.NonRevApprovalCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueComparison:
                            SessionHandler.NonRevComparisonCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueHistoricalTrend:
                            SessionHandler.NonRevHTrendCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsers:
                            SessionHandler.NonRevApprovalUsersCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsersList:
                            SessionHandler.NonRevApprovalUsersListCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue) + 1;
                            break;

                    }

                    break;
                case "Last":
                    switch (GridviewSessionValues)
                    {

                        case (int)Gridviews.GridviewToPage.AvailabilityCarSearch:
                            SessionHandler.AvailabilityCarSearchCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.AvailabilityStatisticsSelection:
                            SessionHandler.AvailabilityStatisticsSelectionCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.AvailabilityStatisticsDates:
                            SessionHandler.AvailabilityStatisticsDateCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MaintenanceUsers:
                            SessionHandler.MaintanenceUsersCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCountry:
                            SessionHandler.MappingCountryCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingAreaCode:
                            SessionHandler.MappingAreaCodeCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCMSPool:
                            SessionHandler.MappingCMSPoolCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCMSLocation:
                            SessionHandler.MappingCMSLocationCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingOPSRegion:
                            SessionHandler.MappingOPSRegionCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingOPSArea:
                            SessionHandler.MappingOPSAreaCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingLocation:
                            SessionHandler.MappingLocationCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarSegment:
                            SessionHandler.MappingCarSegmentCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarClass:
                            SessionHandler.MappingCarClassCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingCarGroup:
                            SessionHandler.MappingCarGroupCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingModelCode:
                            SessionHandler.MappingModelCodeCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);

                            break;
                        case (int)Gridviews.GridviewToPage.MappingVehiclesLease:
                            SessionHandler.MappingVehiclesLeaseCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueCarSearch:
                            SessionHandler.NonRevCarSearchCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueReportStart:
                            SessionHandler.NonRevReportStartCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApproval:
                            SessionHandler.NonRevApprovalCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueComparison:
                            SessionHandler.NonRevComparisonCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueHistoricalTrend:
                            SessionHandler.NonRevHTrendCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsers:
                            SessionHandler.NonRevApprovalUsersCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);
                            break;
                        case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsersList:
                            SessionHandler.NonRevApprovalUsersListCurrentPageNumber = Convert.ToInt32(this.LabelTotalPages.Text);
                            break;

                    }

                    break;
            }
            //Raise custom event from parent page
            if (PageIndexCommand != null)
            {
                PageIndexCommand(this, EventArgs.Empty);
            }

        }


        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (GridviewSessionValues)
            {

                case (int)Gridviews.GridviewToPage.AvailabilityCarSearch:
                    SessionHandler.AvailabilityCarSearchCurrentPageNumber = 1;
                    SessionHandler.AvailabilityCarSearchPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.AvailabilityStatisticsSelection:
                    SessionHandler.AvailabilityStatisticsSelectionCurrentPageNumber = 1;
                    SessionHandler.AvailabilityStatisticsSelectionPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.AvailabilityStatisticsDates:
                    SessionHandler.AvailabilityStatisticsDateCurrentPageNumber = 1;
                    SessionHandler.AvailabilityStatisticsDatePageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MaintenanceUsers:
                    SessionHandler.MaintanenceUsersCurrentPageNumber = 1;
                    SessionHandler.MaintanenceUsersPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCountry:
                    SessionHandler.MappingCountryCurrentPageNumber = 1;
                    SessionHandler.MappingCountryPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingAreaCode:
                    SessionHandler.MappingAreaCodeCurrentPageNumber = 1;
                    SessionHandler.MappingAreaCodePageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCMSPool:
                    SessionHandler.MappingCMSPoolCurrentPageNumber = 1;
                    SessionHandler.MappingCMSPoolPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCMSLocation:
                    SessionHandler.MappingCMSLocationCurrentPageNumber = 1;
                    SessionHandler.MappingCMSLocationPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingOPSRegion:
                    SessionHandler.MappingOPSRegionCurrentPageNumber = 1;
                    SessionHandler.MappingOPSRegionPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingOPSArea:
                    SessionHandler.MappingOPSAreaCurrentPageNumber = 1;
                    SessionHandler.MappingOPSAreaPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingLocation:
                    SessionHandler.MappingLocationCurrentPageNumber = 1;
                    SessionHandler.MappingLocationPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCarSegment:
                    SessionHandler.MappingCarSegmentCurrentPageNumber = 1;
                    SessionHandler.MappingCarSegmentPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCarClass:
                    SessionHandler.MappingCarClassCurrentPageNumber = 1;
                    SessionHandler.MappingCarClassPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCarGroup:
                    SessionHandler.MappingCarGroupCurrentPageNumber = 1;
                    SessionHandler.MappingCarGroupPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingModelCode:
                    SessionHandler.MappingModelCodeCurrentPageNumber = 1;
                    SessionHandler.MappingModelCodePageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingVehiclesLease:
                    SessionHandler.MappingVehiclesLeaseCurrentPageNumber = 1;
                    SessionHandler.MappingVehiclesLeasePageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueCarSearch:
                    SessionHandler.NonRevCarSearchCurrentPageNumber = 1;
                    SessionHandler.NonRevCarSearchPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueReportStart:
                    SessionHandler.NonRevReportStartCurrentPageNumber = 1;
                    SessionHandler.NonRevReportStartPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueApproval:
                    SessionHandler.NonRevApprovalCurrentPageNumber = 1;
                    SessionHandler.NonRevApprovalPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueComparison:
                    SessionHandler.NonRevComparisonCurrentPageNumber = 1;
                    SessionHandler.NonRevComparisonPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueHistoricalTrend:
                    SessionHandler.NonRevHTrendCurrentPageNumber =1;
                    SessionHandler.NonRevHTrendPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsers:
                    SessionHandler.NonRevApprovalUsersCurrentPageNumber = 1;
                    SessionHandler.NonRevApprovalUsersPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsersList:
                    SessionHandler.NonRevApprovalUsersListCurrentPageNumber = 1;
                    SessionHandler.NonRevApprovalUsersListPageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    _gridViewToPage.PageSize = Convert.ToInt32(this.DropDownListRows.SelectedValue);
                    
                    break;
            }


            //Raise custom event from parent page
            if (DropDownListRowsSelectedIndexChanged != null)
            {
                DropDownListRowsSelectedIndexChanged(this, EventArgs.Empty);
            }

        }


        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (GridviewSessionValues)
            {

                case (int)Gridviews.GridviewToPage.AvailabilityCarSearch:
                    SessionHandler.AvailabilityCarSearchCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.AvailabilityStatisticsSelection:
                    SessionHandler.AvailabilityStatisticsSelectionCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.AvailabilityStatisticsDates:
                    SessionHandler.AvailabilityStatisticsDateCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MaintenanceUsers:
                    SessionHandler.MaintanenceUsersCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCountry:
                    SessionHandler.MappingCountryCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingAreaCode:
                    SessionHandler.MappingAreaCodeCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCMSPool:
                    SessionHandler.MappingCMSPoolCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCMSLocation:
                    SessionHandler.MappingCMSLocationCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingOPSRegion:
                    SessionHandler.MappingOPSRegionCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingOPSArea:
                    SessionHandler.MappingOPSAreaCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingLocation:
                    SessionHandler.MappingLocationCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCarSegment:
                    SessionHandler.MappingCarSegmentCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCarClass:
                    SessionHandler.MappingCarClassCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingCarGroup:
                    SessionHandler.MappingCarGroupCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingModelCode:
                    SessionHandler.MappingModelCodeCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.MappingVehiclesLease:
                    SessionHandler.MappingVehiclesLeaseCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);

                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueCarSearch:
                    SessionHandler.NonRevCarSearchCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueReportStart:
                    SessionHandler.NonRevReportStartCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueApproval:
                    SessionHandler.NonRevApprovalCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueComparison:
                    SessionHandler.NonRevComparisonCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);;
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueHistoricalTrend:
                    SessionHandler.NonRevHTrendCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);;
                    
                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsers:
                    SessionHandler.NonRevApprovalUsersCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);;

                    break;
                case (int)Gridviews.GridviewToPage.NonRevenueApprovalUsersList:
                    SessionHandler.NonRevApprovalUsersListCurrentPageNumber = Convert.ToInt32(this.DropDownListPage.SelectedValue);;
                    
                    break;

            }

            //Raise custom event from parent page
            if (DropDownListPageSelectedIndexChanged != null)
            {
                DropDownListPageSelectedIndexChanged(this, EventArgs.Empty);
            }

        }

        #endregion
    }
}