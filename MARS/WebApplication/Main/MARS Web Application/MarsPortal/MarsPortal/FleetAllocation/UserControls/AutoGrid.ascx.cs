using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.BusinessLogic.ExcelExport;
using Label = System.Web.UI.WebControls.Label;

namespace Mars.FleetAllocation.UserControls
{
    public partial class AutoGrid : UserControl
    {
        private const string GridSortDirection = "GridSortDirection";

        public const string ViewKeyword = "View";
        public const string EditKeyword = "Edit ";
        public const string EditCommand = "AutoGridChange ";

        public bool ShowHeaderWhenEmpty { set { gvAutoGrid.ShowHeaderWhenEmpty = value; } }
        
        private SortDirection OverviewSortDirection
        {
            get
            {
                var returned = (SortDirection)(ViewState[GridSortDirection + SessionNameForGridData] ?? SortDirection.Ascending);
                return returned;
            }
            set { ViewState[GridSortDirection + SessionNameForGridData] = value; }
        }

        public string[] ColumnHeaders { private get; set; }
        public string[] ColumnFormats { private get; set; }

        public string ExportDataFileName
        {
            private get { return hfExportDataFileName.Value; }
            set { hfExportDataFileName.Value = value; }
        }

        public HorizontalAlign HorizontalAlignment { set { gvAutoGrid.HorizontalAlign = value; } }

        private const string SortColumn = "SortColumn";
        private PropertyInfo OverviewSortColumn
        {
            get { return ViewState[SortColumn + SessionNameForGridData] as PropertyInfo; }
            set { ViewState[SortColumn + SessionNameForGridData] = value; }
        }

        public bool HideLastColumn
        {
            set { hfHideLastColumn.Value = value.ToString(); }
            private get { return bool.Parse(hfHideLastColumn.Value); }

        }

        public GridView GetGridview
        {
            get { return gvAutoGrid; }
        }

        public int AutoGridWidth
        {
            set
            {
                pnlGrid.Width = value;
                gvAutoGrid.Width = value;
            }
        }

        private const string CurrentPage = "CurrentPage";
        private int CurrentGvPage
        {
            get { return ViewState[CurrentPage + SessionNameForGridData] == null ? 0 : int.Parse(ViewState[CurrentPage + SessionNameForGridData].ToString()); }
            set { ViewState[CurrentPage + SessionNameForGridData] = value.ToString(CultureInfo.InvariantCulture); }
        }

        public IEnumerable<object> GridData
        {
            get { return (IEnumerable<object>)Session[SessionNameForGridData]; }
            set { Session[SessionNameForGridData] = value; }
        }

     
        public string SessionNameForGridData { get; set; }

        public bool ShowSideExportButton
        {
            set { ucSideExportToExcel.Visible = value; }
        }

        public bool ShowBottomExportButton
        {
            set { ucBottomExportToExcel.Visible = value; }
        }


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
            pnlPager.Visible = GridData != null && GridData.Any();
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


        protected override bool OnBubbleEvent(object source, EventArgs args)
        {

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == ViewKeyword)
                {
                    hfHighlightedLineId.Value = commandArgs.CommandArgument.ToString();
                }
            }
            return false;
        }

        protected void gvAutoGrid_DataRowBound(object sender, GridViewRowEventArgs e)
        {
            if (HideLastColumn)
            {
                var colCount = e.Row.Cells.Count;
                e.Row.Cells[colCount - 1].Visible = false;
            }

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
                if (ColumnHeaders[lastColumnHeader] == ViewKeyword )
                {
                    var commandParameter = e.Row.Cells[lastColumnHeader].Text;
                    e.Row.Cells.RemoveAt(lastColumnHeader);
                    var tc = new TableCell();
                    

                    var lb = new LinkButton { Text = ViewKeyword
                                        , ID = ViewKeyword + commandParameter
                                        , CommandName = ViewKeyword
                                        , CommandArgument = commandParameter };

                    if (hfHighlightedLineId.Value == commandParameter)
                    {
                        e.Row.CssClass = "StandardBorder";
                    }

                    tc.Controls.Add(lb);
                    tc.HorizontalAlign = HorizontalAlign.Center;
                    tc.ForeColor = Color.DarkBlue;
                    e.Row.Cells.Add(tc);
                }

                if (ColumnHeaders[lastColumnHeader] == EditKeyword )
                {
                    var commandParameter = e.Row.Cells[lastColumnHeader].Text;
                    e.Row.Cells.RemoveAt(lastColumnHeader);
                    var tc = new TableCell();
                    

                    var lb = new LinkButton { Text = EditKeyword
                                        , ID = EditKeyword + commandParameter
                                        , CommandName = EditCommand
                                        , CommandArgument = commandParameter };

                    if (hfHighlightedLineId.Value == commandParameter)
                    {
                        e.Row.CssClass = "StandardBorder";
                    }
                    
                    tc.Controls.Add(lb);
                    tc.HorizontalAlign = HorizontalAlign.Center;
                    tc.ForeColor = Color.DarkGreen;
                    e.Row.Cells.Add(tc);
                }

            }
            
        }

        public int PageSize
        {
            private get
            {
                var size = int.Parse(ddlPageSize.SelectedValue);
                return size;
            }
            set
            {   
                ddlPageSize.SelectedValue = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void SetGridviewPage()
        {
            var numberOfItemsInList = GridData == null ? 0 : GridData.ToList().Count;


            if (lblPageAt == null ||
                lblRowCount == null) return;


            if (lbgvFirst == null || lbgvPrevious == null || lbgvNext == null || lbgvLast == null)
            {
                lblRowCount.Text = string.Format("Total Entries: {0:##,##0}", numberOfItemsInList);

                lblPageAt.Text = string.Format("Page {0} of {1}", CurrentGvPage, (numberOfItemsInList + PageSize - 1) / PageSize);
                return;
            }
                
            var firstPage = CurrentGvPage == 1;
            var lastPage = GridData != null && CurrentGvPage == ((numberOfItemsInList + PageSize - 1) / PageSize);
            lbgvFirst.Enabled = !firstPage;
            lbgvFirst.ImageUrl = firstPage ? "~/App.Images/pager-first-dis.png" : "~/App.Images/pager-first.png";
            lbgvPrevious.Enabled = !firstPage;
            lbgvPrevious.ImageUrl = firstPage ? "~/App.Images/pager-previous-dis.png" : "~/App.Images/pager-previous.png";
            lbgvNext.Enabled = !lastPage;
            lbgvNext.ImageUrl = lastPage ? "~/App.Images/pager-next-dis.png" : "~/App.Images/pager-next.png";
            lbgvLast.Enabled = !lastPage;
            lbgvLast.ImageUrl = lastPage ? "~/App.Images/pager-last-dis.png" : "~/App.Images/pager-last.png";

            if (GridData == null) return;

            lblRowCount.Text = string.Format("Total Entries: {0:##,##0}", numberOfItemsInList);

            lblPageAt.Text = string.Format("Page {0} of {1}", CurrentGvPage, (numberOfItemsInList + PageSize - 1) / PageSize);
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

        protected void ExportToExcel()
        {
            var dataToExport = GridData.ToList();
            ExportAutoGrid.ExportToExcel(dataToExport, ExportDataFileName, ColumnHeaders);
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