using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;
using Mars.App.Classes.Phase4Dal.Reservations.Entities;

namespace Mars.App.UserControls.Phase4.Reservations
{
    public partial class ReservationsGrid : UserControl
    {
        private const string SessionGridData = "ReservationGridData";

        private const string ReservationSortDirectionSessionString = "ReservationSortDirectionSessionString";
        private const string ReservationSortColumnSessionString = "ReservationSortColumnSessionString";

        public List<ReservationData> GridData
        {
            get { return Session[SessionGridData] as List<ReservationData>; }
            set { Session[SessionGridData] = value; }
        }

        private int PageSize
        {
            get
            {
                var ddlPageSize = GetPagerControl("ddlPageSize") as DropDownList;
                if (ddlPageSize == null) return 10;
                var size = int.Parse(ddlPageSize.SelectedValue);
                return size;
            }
            set
            {
                var ddlPageSize = GetPagerControl("ddlPageSize") as DropDownList;
                if (ddlPageSize == null) return;
                ddlPageSize.SelectedValue = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        private int CurrentGvPage
        {
            get { return int.Parse(string.IsNullOrEmpty(hfCurrentGvPage.Value) ? "0" : hfCurrentGvPage.Value); }
            set { hfCurrentGvPage.Value = value.ToString(CultureInfo.InvariantCulture); }
        }

        private SortDirection OverviewSortDirection
        {
            get { return (SortDirection)Session[ReservationSortDirectionSessionString]; }
            set { Session[ReservationSortDirectionSessionString] = value; }
        }

        private PropertyInfo OverviewSortColumn
        {
            get { return Session[ReservationSortColumnSessionString] as PropertyInfo; }
            set { Session[ReservationSortColumnSessionString] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OverviewSortColumn = null;
                OverviewSortDirection = SortDirection.Descending;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            gvReservations.PageIndex = CurrentGvPage - 1;
            BindGridView(PageSize);
            SetGridviewPage();
        }

        protected void GridviewOverview_Sorting(object sender, GridViewSortEventArgs e)
        {
            OverviewSortColumn = typeof(ReservationData).GetProperty(e.SortExpression);

            OverviewSortDirection = OverviewSortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            SortGrid(OverviewSortDirection, OverviewSortColumn);


        }

        private void SortGrid(SortDirection direction, PropertyInfo sortByColumnName)
        {
            var isInt = sortByColumnName.PropertyType == typeof(int);
            var isDateTime = sortByColumnName.PropertyType == typeof(DateTime?);

            if (direction == SortDirection.Ascending)
            {
                GridData.Sort((x, y) => isDateTime ? (int)(((DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue)).Date
                                                - ((DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue)).Date
                                        ).TotalDays
                                    : isInt ? (int)sortByColumnName.GetValue(x, null) - (int)sortByColumnName.GetValue(y, null)
                                    : String.CompareOrdinal(sortByColumnName.GetValue(x, null).ToString()
                                    , sortByColumnName.GetValue(y, null).ToString())
                            );
            }
            else
            {
                GridData.Sort(
                    (x, y) =>
                        isDateTime
                            ? (int)
                                (((DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue)).Date -
                                 ((DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue)).Date).TotalDays
                            : isInt
                                ? (int)sortByColumnName.GetValue(y, null) - (int)sortByColumnName.GetValue(x, null)
                                : String.CompareOrdinal(sortByColumnName.GetValue(y, null).ToString()
                                    , sortByColumnName.GetValue(x, null).ToString()));

            }
        }

        private void BindGridView(int pageSize)
        {
            gvReservations.DataSource = GridData;
            gvReservations.DataBind();
            gvReservations.PageSize = pageSize;
            PageSize = pageSize;
        }

        private Control GetPagerControl(string controlName)
        {
            var pagerRow = gvReservations.BottomPagerRow;
            if (pagerRow == null) return null;
            var returned = pagerRow.FindControl(controlName);
            return returned;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            var pagerRow = gvReservations.BottomPagerRow;

            if (pagerRow != null && pagerRow.Visible == false)
                pagerRow.Visible = true;
        }

        private void SetGridviewPage()
        {
            var btnFirst = GetPagerControl("lbgvFirst") as ImageButton;
            var btnPrevious = GetPagerControl("lbgvPrevious") as ImageButton;
            var btnNext = GetPagerControl("lbgvNext") as ImageButton;
            var btnLast = GetPagerControl("lbgvLast") as ImageButton;
            var lblPageAt = GetPagerControl("lblPageAt") as Label;
            var lblRowCount = GetPagerControl("lblRowCount") as Label;


            if (btnFirst == null || btnPrevious == null || btnNext == null || btnLast == null || lblPageAt == null || lblRowCount == null) return;
            var firstPage = CurrentGvPage == 1;
            var lastPage = GridData != null && CurrentGvPage == ((GridData.Count + PageSize - 1) / PageSize);
            btnFirst.Enabled = !firstPage;
            btnFirst.ImageUrl = firstPage ? "~/App.Images/pager-first-dis.png" : "~/App.Images/pager-first.png";
            btnPrevious.Enabled = !firstPage;
            btnPrevious.ImageUrl = firstPage ? "~/App.Images/pager-previous-dis.png" : "~/App.Images/pager-previous.png";
            btnNext.Enabled = !lastPage;
            btnNext.ImageUrl = lastPage ? "~/App.Images/pager-next-dis.png" : "~/App.Images/pager-next.png";
            btnLast.Enabled = !lastPage;
            btnLast.ImageUrl = lastPage ? "~/App.Images/pager-last-dis.png" : "~/App.Images/pager-last.png";


            lblRowCount.Text = string.Format("Total Reservations: {0:##,##0}", GridData.Count);

            lblPageAt.Text = string.Format("Page {0} of {1}", CurrentGvPage, (GridData.Count + PageSize - 1) / PageSize);
        }

        protected void gvFirstButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage = 1;

        }

        protected void gvPreviousButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage--;
        }

        protected void gvNextButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage++;
        }

        protected void gvLastButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage = (GridData.Count + PageSize - 1) / PageSize;
        }

        protected void ddlPageSize_SizeChange(object sender, EventArgs e)
        {
            var ddlPageSize = sender as DropDownList;
            if (ddlPageSize == null) return;
            var pageSize = int.Parse(ddlPageSize.SelectedValue);
            gvReservations.PageSize = pageSize;
            BindGridView(pageSize);
            CurrentGvPage = 1;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == HelpIcons.ExportToExcel.ExportString)
                {
                    ExportToExcel();
                    handled = true;
                }
            }
            return handled;
        }

        protected void ExportToExcel()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
                "Res ID", "Check Out Country", "Group Reserved", "Group Upgraded", "Check Out Location", "Check Out Date"
                            , "Check In Country", "Check In Location", "Check In Date"
                            , "Days Reserved", "Customer Name"
                            , "CDP", "Tariff"
                            , "Flight Number"
                            , "Neverlost"
                            , "Booking Date"));

            //var dataTable = HtmlTableGenerator.GenerateHtmlTableFromGridview(gvOverview);

            foreach (var gd in GridData)
            {
                sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}"
                                            , gd.ExternalId, gd.PickupLocation.Substring(0, 2), gd.CarGroupReserved,
                                            gd.CarGroupUpgraded
                                            , gd.PickupLocation, gd.PickupDate
                                            , gd.ReturnLocation.Substring(0, 2), gd.ReturnLocation
                                            , gd.ReturnDate
                                            , gd.DaysReserved
                                            , gd.CustomerName
                                            , string.Empty
                                            , gd.Tariff
                                            , gd.FlightNumber
                                            , gd.NeverLost.HasValue && gd.NeverLost.Value ? "Yes" : "No"
                                            , gd.BookedDate));
            }

            Session["ExportData"] = sb.ToString();
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("{0} {1}", "Reservation Details Export", DateTime.Now);
        }
    }
}