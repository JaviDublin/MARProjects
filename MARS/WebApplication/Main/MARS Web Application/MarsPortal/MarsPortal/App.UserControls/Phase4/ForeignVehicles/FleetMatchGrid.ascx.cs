using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;

namespace Mars.App.UserControls.Phase4.ForeignVehicles
{
    public partial class FleetMatchGrid : UserControl
    {
        private const string SessionGridData = "FleetMatchGridSessionData";

        private const string FleetMatchGridSortDirectionSessionString = "FleetMatchGridSortDirectionSessionString";
        private const string FleetMatchGridSortColumnSessionString = "FleetMatchGridSortColumnSessionString";

        public List<VehicleMatchGridRow> GridData
        {
            private get { return Session[SessionGridData] as List<VehicleMatchGridRow>; }
            set { Session[SessionGridData] = value; }
        }

        private SortDirection OverviewSortDirection
        {
            get { return (SortDirection)Session[FleetMatchGridSortDirectionSessionString]; }
            set { Session[FleetMatchGridSortDirectionSessionString] = value; }
        }

        private PropertyInfo OverviewSortColumn
        {
            get { return Session[FleetMatchGridSortColumnSessionString] as PropertyInfo; }
            set { Session[FleetMatchGridSortColumnSessionString] = value; }
        }

        public bool ShowSelectColumn
        {
            get { return hfShowSelectColumn.Value == true.ToString(); }
            set { hfShowSelectColumn.Value = value.ToString(); }
        }

        public int HighlightedVehicleId
        {
            get { return int.Parse(hfHighlightedVehicleId.Value); }
            set { hfHighlightedVehicleId.Value = value.ToString(); }
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

            gvOverview.Columns[gvOverview.Columns.Count - 1].Visible = true;
            gvOverview.PageIndex = CurrentGvPage - 1;
            BindGridView(PageSize);

            lblHeader.Text = ShowSelectColumn ? "Fleet" : "Matching Fleet";

            gvOverview.Columns[gvOverview.Columns.Count - 1].Visible = false;
            gvOverview.Columns[gvOverview.Columns.Count - 2].Visible = ShowSelectColumn;
            gvOverview.Columns[gvOverview.Columns.Count - 3].Visible = ShowSelectColumn;

            foreach(GridViewRow gr in gvOverview.Rows)
            {
                if(gr.Cells[10].Text == string.Empty) continue;
                var s = int.Parse(gr.Cells[10].Text);
                if (HighlightedVehicleId == s)
                {
                    gr.CssClass = "HighLighted";
                }
            }


            
            

            SetGridviewPage();

            gvOverview.PagerSettings.Visible = true;

        }

        protected void Overview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowReservations")
            {
                HighlightedVehicleId = int.Parse(e.CommandArgument.ToString());    
            }
            
            
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

            if (GridData == null) return;

            lblRowCount.Text = string.Format("Total Vehicles: {0:##,##0}", GridData.Count);

            lblPageAt.Text = string.Format("Page {0} of {1}", CurrentGvPage, (GridData.Count + PageSize - 1) / PageSize);
        }
        protected void GridviewOverview_Sorting(object sender, GridViewSortEventArgs e)
        {
            OverviewSortColumn = typeof(VehicleMatchGridRow).GetProperty(e.SortExpression);

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

        private int CurrentGvPage
        {
            get { return int.Parse(string.IsNullOrEmpty(hfCurrentGvPage.Value) ? "0" : hfCurrentGvPage.Value); }
            set { hfCurrentGvPage.Value = value.ToString(CultureInfo.InvariantCulture); }
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
            gvOverview.PageSize = pageSize;
            BindGridView(pageSize);
            CurrentGvPage = 1;
        }

        private void BindGridView(int pageSize)
        {
            gvOverview.DataSource = GridData;
            gvOverview.DataBind();
            gvOverview.PageSize = pageSize;
            PageSize = pageSize;
        }


        private Control GetPagerControl(string controlName)
        {
            var pagerRow = gvOverview.BottomPagerRow;
            if (pagerRow == null) return null;
            var returned = pagerRow.FindControl(controlName);
            return returned;
        }

        
    }
}