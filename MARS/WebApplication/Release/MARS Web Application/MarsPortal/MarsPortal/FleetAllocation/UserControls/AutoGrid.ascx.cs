using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;
using Label = System.Web.UI.WebControls.Label;

namespace Mars.FleetAllocation.UserControls
{
    public partial class AutoGrid : UserControl
    {
        private const string GridSortDirection = "GridSortDirection";

        public const string ViewKeyword = "View";

        public const string ViewStateCommandName = "ViewStateCommandName";
        
        private SortDirection OverviewSortDirection
        {
            get
            {
                var returned = (SortDirection)(ViewState[GridSortDirection] ?? SortDirection.Ascending);
                return returned;
            }
            set { ViewState[GridSortDirection] = value; }
        }

        public string[] ColumnHeaders { private get; set; }
        public string[] ColumnFormats { private get; set; }


        private const string SortColumn = "SortColumn";
        private PropertyInfo OverviewSortColumn
        {
            get { return ViewState[SortColumn] as PropertyInfo; }
            set { ViewState[SortColumn] = value; }
        }

        public int AutoGridWidth { set { gvAutoGrid.Width = value; } }

        private const string CurrentPage = "CurrentPage";
        private int CurrentGvPage
        {
            get { return ViewState[CurrentPage] == null ? 0 : int.Parse(ViewState[CurrentPage].ToString()); }
            set { ViewState[CurrentPage] = value.ToString(CultureInfo.InvariantCulture); }
        }

        public IEnumerable<object> GridData
        {
            get { return (IEnumerable<object>)Session[SessionNameForGridData]; }
            set { Session[SessionNameForGridData] = value; }
        }

     
        public string SessionNameForGridData { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OverviewSortDirection = SortDirection.Descending;
                CurrentGvPage = 1;
            }
            BindGridView(PageSize);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            gvAutoGrid.PageIndex = CurrentGvPage - 1;
            BindGridView(PageSize);
            SetGridviewPage();
        }
        private void BindGridView(int pageSize)
        {
            gvAutoGrid.DataSource = GridData;
            gvAutoGrid.DataBind();
            gvAutoGrid.PageSize = pageSize;
            PageSize = pageSize;
        }

        public void UpdateUpdatePanel()
        {
            upnlGrid.Update();
        }

        private int _linkColumnNumber;
        protected void gvAutoGrid_DataRowBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                var col = 0;
                foreach (TableCell c in e.Row.Cells)
                {
                    var link = c.Controls[0] as LinkButton;
                    if (link == null) return;
                    link.Text = ColumnHeaders[col];
                    col++;
                }
                
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ColumnFormats != null)
                {
                    
                    for(var i = 0; i < ColumnFormats.Length; i++)
                    {
                        if (ColumnFormats[i] == string.Empty) continue;

                    
                        e.Row.Cells[i].Text = e.Row.Cells[i].Text == "&nbsp;" ? string.Empty : (double.Parse(e.Row.Cells[i].Text)).ToString(ColumnFormats[i]);
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    }

                }
                var lastColumnHeader = ColumnHeaders.Count() - 1;
                if (ColumnHeaders[lastColumnHeader] == ViewKeyword)
                {
                    var commandParameter = e.Row.Cells[lastColumnHeader].Text;
                    e.Row.Cells.RemoveAt(lastColumnHeader);
                    var tc = new TableCell();
                    

                    var lb = new LinkButton { Text = ViewKeyword
                                        , ID = ViewKeyword + commandParameter
                                        , CommandName = ViewKeyword
                                        , CommandArgument = commandParameter };
                    //lb.Command += LinkButton_Command;
                    
                    tc.Controls.Add(lb);
                    tc.HorizontalAlign = HorizontalAlign.Center;
                    tc.ForeColor = Color.DarkBlue;
                    e.Row.Cells.Add(tc);
                }
            }
            
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

        private void SetGridviewPage()
        {
            var numberOfItemsInList = GridData == null ? 0 : GridData.ToList().Count;
            var btnFirst = GetPagerControl("lbgvFirst") as ImageButton;
            var btnPrevious = GetPagerControl("lbgvPrevious") as ImageButton;
            var btnNext = GetPagerControl("lbgvNext") as ImageButton;
            var btnLast = GetPagerControl("lbgvLast") as ImageButton;
            var lblPageAt = GetPagerControl("lblPageAt") as Label;
            var lblRowCount = GetPagerControl("lblRowCount") as Label;


            if (btnFirst == null || btnPrevious == null || btnNext == null || btnLast == null || lblPageAt == null || lblRowCount == null) return;
            var firstPage = CurrentGvPage == 1;
            var lastPage = GridData != null && CurrentGvPage == ((numberOfItemsInList + PageSize - 1) / PageSize);
            btnFirst.Enabled = !firstPage;
            btnFirst.ImageUrl = firstPage ? "~/App.Images/pager-first-dis.png" : "~/App.Images/pager-first.png";
            btnPrevious.Enabled = !firstPage;
            btnPrevious.ImageUrl = firstPage ? "~/App.Images/pager-previous-dis.png" : "~/App.Images/pager-previous.png";
            btnNext.Enabled = !lastPage;
            btnNext.ImageUrl = lastPage ? "~/App.Images/pager-next-dis.png" : "~/App.Images/pager-next.png";
            btnLast.Enabled = !lastPage;
            btnLast.ImageUrl = lastPage ? "~/App.Images/pager-last-dis.png" : "~/App.Images/pager-last.png";

            if (GridData == null) return;

            lblRowCount.Text = string.Format("Total Entries: {0:##,##0}", numberOfItemsInList);

            lblPageAt.Text = string.Format("Page {0} of {1}", CurrentGvPage, (numberOfItemsInList + PageSize - 1) / PageSize);
        }

        private Control GetPagerControl(string controlName)
        {
            var pagerRow = gvAutoGrid.BottomPagerRow;
            if (pagerRow == null) return null;
            var returned = pagerRow.FindControl(controlName);
            return returned;
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
            CurrentGvPage = (GridData.ToList().Count + PageSize - 1) / PageSize;
        }

        protected void ddlPageSize_SizeChange(object sender, EventArgs e)
        {
            var ddlPageSize = sender as DropDownList;
            if (ddlPageSize == null) return;
            var pageSize = int.Parse(ddlPageSize.SelectedValue);
            gvAutoGrid.PageSize = pageSize;
            ChangeGridViewSize(pageSize);
            CurrentGvPage = 1;
        }


        private void ChangeGridViewSize(int pageSize)
        {
            //BindGridView();
            gvAutoGrid.PageSize = pageSize;
            PageSize = pageSize;
            
        }

        private const string GridTypeItem = "GridTypeItem";
        public Type GridItemType
        {
            get { return (Type)ViewState[GridTypeItem]; }
            set { ViewState[GridTypeItem] = value; }
        }

        protected void Gridview_Sorting(object sender, GridViewSortEventArgs e)
        {
            OverviewSortColumn = GridItemType.GetProperty(e.SortExpression);

            OverviewSortDirection = OverviewSortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            SortGrid(OverviewSortDirection, OverviewSortColumn);
            //RaiseBubbleEvent(this, new CommandEventArgs("AutoGridSort", ""));
        }


        public void SortGrid(SortDirection direction, PropertyInfo sortByColumnName)
        {
            var isInt = sortByColumnName.PropertyType == typeof(int);
            var isDouble = sortByColumnName.PropertyType == typeof (double);
            
            
            var isDateTime = sortByColumnName.PropertyType == typeof(DateTime?);

            var sessionGridData = ((IEnumerable<object>)Session[SessionNameForGridData]).ToList();
            
            if (direction == SortDirection.Ascending)
            {


                sessionGridData.Sort((x, y) =>
                                     {
                                         if (isDateTime)
                                         {
                                             var dt1 =
                                                 (DateTime) (sortByColumnName.GetValue(x, null) ?? DateTime.MinValue);
                                             var dt2 =
                                                 (DateTime) (sortByColumnName.GetValue(y, null) ?? DateTime.MinValue);

                                             return dt1.CompareTo(dt2);
                                         }

                                         if (isInt)
                                         {
                                             var int1 = (int) sortByColumnName.GetValue(x, null);
                                             var int2 = (int) sortByColumnName.GetValue(y, null);
                                             return int1.CompareTo(int2);
                                         }
                                         if (isDouble)
                                         {
                                             var dbl1 = (double) sortByColumnName.GetValue(x, null);
                                             var dbl2 = (double) sortByColumnName.GetValue(y, null);
                                             return dbl1.CompareTo(dbl2);
                                         }
                                         return String.Compare(sortByColumnName.GetValue(x, null).ToString()
                                             , sortByColumnName.GetValue(y, null).ToString());
                                     }
                    );
                
            }
            else
            {
                sessionGridData.Sort(
                    (x, y) =>
                    {
                        if (isDateTime)
                        {
                            var dt1 =
                                (DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue);
                            var dt2 =
                                (DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue);

                            return dt1.CompareTo(dt2);
                        }

                        if (isInt)
                        {
                            var int1 = (int)sortByColumnName.GetValue(y, null);
                            var int2 = (int)sortByColumnName.GetValue(x, null);
                            return int1.CompareTo(int2);
                        }
                        if (isDouble)
                        {
                            var dbl1 = (double)sortByColumnName.GetValue(y, null);
                            var dbl2 = (double)sortByColumnName.GetValue(x, null);
                            return dbl1.CompareTo(dbl2);
                        }
                        return String.Compare(sortByColumnName.GetValue(y, null).ToString()
                            , sortByColumnName.GetValue(x, null).ToString());
                    }
                    );
                
            }
            
            GridData = sessionGridData;
        }

    }
}