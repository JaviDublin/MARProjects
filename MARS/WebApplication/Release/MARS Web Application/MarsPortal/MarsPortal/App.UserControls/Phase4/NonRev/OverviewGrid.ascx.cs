using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;
using App.DAL;
using Mars.App.Classes.Phase4Bll;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class OverviewGrid : UserControl
    {
        private const string SessionGridData = "OverviewGridData";

        private const string OverviewSortDirectionSessionString = "OverviewSortDirectionSessionString";
        private const string OverviewSortColumnSessionString = "OverviewSortColumnSessionString";

        public List<OverviewGridRow> GridData
        {
            private get { return Session[SessionGridData] as List<OverviewGridRow>; }
            set { Session[SessionGridData] = value; }
        }

        public bool ShowMultiSelectTickBoxes
        {
            get { return hfShowMultiSelectTickBoxes.Value == "1"; }
            set { hfShowMultiSelectTickBoxes.Value = value ? "1" : "0"; }
        }

        public bool ShowApproveButton
        {
            get { return hfShowApproveButton.Value == "1"; }
            set { hfShowApproveButton.Value = value ? "1" : "0"; }
        }

        public bool ShowNonRevFields
        {
            get { return hfShowNonRevFields.Value == true.ToString(); }
            set { hfShowNonRevFields.Value = value ? true.ToString() : false.ToString(); }
        }

        public bool ShowForeignVehicleFields
        {
            get { return hfShowForeignVehicleFields.Value == true.ToString(); }
            set { hfShowForeignVehicleFields.Value = value ? true.ToString() : false.ToString(); }
        }

        public bool ShowAvailabilityFields
        {
            get { return hfShowAvailabilityFields.Value == true.ToString(); }
            set { hfShowAvailabilityFields.Value = value ? true.ToString() : false.ToString(); }
        }

        private SortDirection OverviewSortDirection
        {
            get { return (SortDirection)Session[OverviewSortDirectionSessionString]; }
            set { Session[OverviewSortDirectionSessionString] = value; }
        }

        private PropertyInfo OverviewSortColumn
        {
            get { return Session[OverviewSortColumnSessionString] as PropertyInfo; }
            set { Session[OverviewSortColumnSessionString] = value; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OverviewSortColumn = null;
                OverviewSortDirection = SortDirection.Descending;
            }
        }

        public void ShowMultiReasonEntryPopup()
        {
            mpeMultiReasonEntry.Show();
        }

        public List<Tuple<int?,int>> LoadGrid(Dictionary<DictionaryParameter, string> parameters)
        {
            List<OverviewGridRow> data;
            using (var dataAccess = new OverviewDataAccess(parameters))
            {
                data = dataAccess.GetVehicles();
            }

            hfEmptyGrid.Value = data.Count > 0 ? "0" : "1";
            GridData = data;
            if (OverviewSortColumn != null)
            {
                SortGrid(OverviewSortDirection, OverviewSortColumn);    
            }
            
            //ucExportToExcel.Visible = data.Count > 0;
            CurrentGvPage = 1;
            var lastPeriodAndDaysNonRev =
                data.Select(d => new Tuple<int?, int>(d.LastPeriodEntryId, d.NonRevDays)).ToList();
            
            return lastPeriodAndDaysNonRev;
        }


        protected void GridviewOverview_Sorting(object sender, GridViewSortEventArgs e)
        {
            OverviewSortColumn = typeof(OverviewGridRow).GetProperty(e.SortExpression);

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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            gvOverview.PageIndex = CurrentGvPage - 1;

            BindGridView(PageSize);
            //ucExportToExcel.Visible = GridData != null && GridData.Count > 0;
            
            SetGridviewPage();

            //gvOverview.Columns[gvOverview.Columns.Count - 3].Visible = ShowNonRevFields;
            //gvOverview.Columns[gvOverview.Columns.Count - 4].Visible = ShowNonRevFields;
            //gvOverview.Columns[gvOverview.Columns.Count - 5].Visible = ShowNonRevFields;
            //gvOverview.Columns[gvOverview.Columns.Count - 6].Visible = !ShowNonRevFields;
            HideFields();
            var btn = GetPagerControl("btnAddGroupRemark") as Button;
            if (btn == null) return;
            btn.Visible = ShowMultiSelectTickBoxes;
            btn = GetPagerControl("btnApproveAll") as Button;
            if (btn == null) return;
            btn.Visible = ShowApproveButton;
        }

        private void HideFields()
        {
            //foreach (DataControlField c in gvOverview.Columns)
            //{
            //    c.Visible = true;
            //}
            if (ShowAvailabilityFields)
            {
                gvOverview.Columns[0].Visible = false;
                gvOverview.Columns[11].Visible = false;
                gvOverview.Columns[14].Visible = false;
                gvOverview.Columns[18].Visible = false;
                gvOverview.Columns[19].Visible = false;
                gvOverview.Columns[20].Visible = false;
                //gvOverview.Columns[21].Visible = false;
                gvOverview.Columns[22].Visible = false;
            }
            if (ShowNonRevFields)
            {
                gvOverview.Columns[9].Visible = false;
                gvOverview.Columns[10].Visible = false;
                
                gvOverview.Columns[14].Visible = false;
                gvOverview.Columns[17].Visible = false;
            }
            if (ShowForeignVehicleFields)
            {
                gvOverview.Columns[0].Visible = false;
                
                
                gvOverview.Columns[18].Visible = false;
                gvOverview.Columns[19].Visible = false;
                gvOverview.Columns[20].Visible = false;
                //gvOverview.Columns[21].Visible = false;
                gvOverview.Columns[22].Visible = false;
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            var pagerRow = gvOverview.BottomPagerRow;

            if (pagerRow != null && pagerRow.Visible == false)
                pagerRow.Visible = true;
        }

        protected void GridviewOverview_DataBound(object sender, EventArgs e)
        {
            gvOverview.Columns[0].Visible = ShowMultiSelectTickBoxes;
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

        protected void btnApproveAll_Click(object sender, EventArgs e)
        {
            RaiseBubbleEvent(this, new CommandEventArgs("Approve Vehicles", GridData.Count));
            GridData = new List<OverviewGridRow>();
            BindGridView(PageSize);
            
        }

        protected void btnAddGroupRemark_Click(object sender, EventArgs e)
        {
            var selectedVehicleIds = (from GridViewRow gvRow in gvOverview.Rows 
                                      let cb = (CheckBox) gvRow.Cells[0].FindControl("cbVehicle") 
                                      where cb.Checked 
                                      select (HiddenField) gvRow.Cells[0].FindControl("hfVehicleId") 
                                      into hfVehicle 
                                      select int.Parse(hfVehicle.Value)).ToList();

            if (selectedVehicleIds.Count > 0)
            {
                ucMultiReasonEntry.SetVehicleDetails(selectedVehicleIds);
                mpeMultiReasonEntry.Show();    
            }
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
        
        protected void ButtonEditClick(object sender, EventArgs e)
        {

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

            sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27} ",
                "Check Out Location", "Last Change", "Check In Location", "Check In Date", "Car Group", "Car Group Charged"
                            , "Model", "Model Description", "Unit Number"
                            , "License", "Serial"
                            , "Operational Status", "Movement Type"
                            , "Document Number"
                            , "Owning Area"
                            , "Customer"
                            , "Last Mileage"
                            , "Installation Date"
                            , "MSO Date"
                            , "CAP Date"
                            , "Car Hold"
                            , "BD Days"
                            , "MM Days"
                            , "CarSearch Comment"
                            , "Non Revenue Days", "Reason Code", "Reason Remark"
                            , "Estimated Resolution"));

            //var dataTable = HtmlTableGenerator.GenerateHtmlTableFromGridview(gvOverview);

            foreach (var gd in GridData)
            {
                sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}, {18}, {19}, {20}, {21}, {22}, {23}, {24}, {25}, {26}, {27}"
                    , gd.LastLocationCode, gd.LastChangeDateTime, gd.ExpectedLocationCode, gd.ExpectedDateString
                    , gd.CarGroup, gd.CarGroupCharged
                    , gd.ModelCode, gd.ModelDescription
                    , gd.UnitNumber
                    , gd.LicensePlate, gd.Serial
                    , gd.OperationalStatusCode, gd.MovementTypeCode
                    , gd.DocumentNumber
                    , gd.OwningArea
                    , gd.LastDriverName
                    , gd.LastMilage
                    , gd.InstallationDateTime
                    , gd.InstallationMsoDateTime
                    , gd.BlockDateTime
                    , gd.HoldFlag1
                    , gd.BdDays
                    , gd.MmDays
                    , gd.Comment == null ? string.Empty : gd.Comment.Replace(",", string.Empty).Replace("\r\n", " ").Replace("\n", " ")
                    , gd.NonRevDays, gd.LastReason,
                    gd.LastRemarkFull == null ? string.Empty : gd.LastRemarkFull.Replace(",", string.Empty).Replace("\r\n", " ").Replace("\n", " ")
                    , gd.EstimatedResultion == DateTime.MinValue ? string.Empty : gd.EstimatedResultion.ToShortDateString()));
            }
            var reportName = "Availability";
            if (ShowNonRevFields)
            {
                reportName = "Non Rev";
            }
            if (ShowForeignVehicleFields)
            {
                reportName = "Foreign Vehicles";
            }

            Session["ExportData"] = sb.ToString();
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("{0} Export {1}", reportName, DateTime.Now);
        }



    }
}