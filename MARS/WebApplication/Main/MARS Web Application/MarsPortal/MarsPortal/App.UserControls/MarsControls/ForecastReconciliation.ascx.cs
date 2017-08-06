using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.EventArgs;
using App.BLL.ExtensionMethods;
using App.BLL.Management;
using App.BLL.ReportParameters;
using App.BLL.Utilities;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.Entities;
using App.Entities.Graphing.Parameters;
using Mars.App.Classes.BLL.ExtensionMethods;

namespace App.UserControls
{
    public partial class ForecastReconciliation : System.Web.UI.UserControl
    {
        private readonly BLLManagement bll = new BLLManagement();
        private readonly BLLReportParameters bllParams = new BLLReportParameters();

        string currentPool = string.Empty;
        List<UserRole> roles = new List<UserRole>();

        public Dictionary<string, string> SelectedParameters { get; set; }

        private List<ReportParameter> ForecastReconciliationParameters
        {
            get
            {
                return (List<ReportParameter>)Session["ForecastReconciliationParameters"];
            }
            set
            {
                Session["ForecastReconciliationParameters"] = value;
            }
        }

        public List<ForecastAdjustmentEntity> ForecastReconciliationList
        {
            get
            {
                if (Session["ForecastReconciliationList"] != null)
                {
                    var adjustment = (List<ForecastAdjustmentEntity>)Session["ForecastReconciliationList"];
                    return adjustment;
                }
                return new List<ForecastAdjustmentEntity>();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rvReconcileFromDate.MinimumValue = DateTime.Today.AddDays(1).ToShortDateString();
                rvReconcileFromDate.MaximumValue = DateTime.Today.AddDays(90).ToShortDateString();
                rvReconcileFromDate.Text = "*";
                
                rvReconcileToDate.MinimumValue = DateTime.Today.AddDays(1).ToShortDateString();
                rvReconcileToDate.MaximumValue = DateTime.Today.AddDays(90).ToShortDateString();
                rvReconcileToDate.Text = "*";

                txtReconcileFromDate.Text = DateTime.Today.AddDays(1).ToShortDateString();
                txtReconcileToDate.Text = DateTime.Today.AddDays(1).ToShortDateString();

                SelectedParameters = new Dictionary<string, string>();
                Page.LoadComplete += new EventHandler(Page_LoadComplete);
            }
            roles = bll.UsersInRolesGet(this.Page.RadUserId());
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            PopulateDropDowns();
            BindGrid(null);
        }

        private void PopulateDropDowns()
        {
            Country dummy = new Country();
            dummy.CountryDescription = StaticStrings.SelectCountry;
            dummy.CountryID = StaticStrings.CountryDummy;

            List<Country> countries = bllParams.CountryGetAll();
            List<Country> accessCountries = new List<Country>();
            foreach (Country country in countries)
            {
                if (roles.Exists(r => (r.RoleCountry == country.CountryID) &&
                    ((RoleCodes)r.RoleID == RoleCodes.TDAdjustmentAndReconciliation)))
                    accessCountries.Add(country);
            }


            System.Web.UI.WebControls.DropDownList ddlCountryList = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlCountry"));
            ddlCountryList.DataTextField = "CountryDescription";
            ddlCountryList.DataValueField = "CountryID";
            accessCountries.Insert(0, dummy);
            ddlCountryList.DataSource = accessCountries;
            ddlCountryList.DataBind();

            if (accessCountries.Count == 2)
                ddlCountryList.SelectedIndex = 1;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (ForecastReconciliationParameters == null)
            {
                ForecastReconciliationParameters = new List<ReportParameter>
                {
                        new ReportParameter(0, 0, null, ParameterNames.Country),
                        new ReportParameter(1, 1, ParameterDataAccess.GetPoolParameterListItems,ParameterNames.Pool),
                        new ReportParameter(1, 2, ParameterDataAccess.GetLocationGroupParameterListItems,ParameterNames.LocationGroup),
                        new ReportParameter(2, 1, ParameterDataAccess.GetCarSegmentParameterListItems,ParameterNames.CarSegment),
                        new ReportParameter(2, 2, ParameterDataAccess.GetCarClassGroupParameterListItems,ParameterNames.CarClassGroup),
                        new ReportParameter(2, 3, ParameterDataAccess.GetCarClassParameterListItems,ParameterNames.CarClass)
                };
            }
            dpForecastReconciliation.ParamsHolder = ForecastReconciliationParameters;
        }

        private void BindGrid(string selectedDate)
        {
            var adjustmentsCLR = new List<ForecastAdjustmentEntity>();

            var country = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlCountry")).SelectedItem.Value;
            if (country != StaticStrings.CountryDummy)
            {

                var pool = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlPool")).SelectedItem.Value;
                var poolID = (pool == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(pool);

                var location = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlLocation Group")).SelectedItem.Value;
                var locationID = (location == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(location);

                var carSegment = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlCar Segment")).SelectedItem.Value;
                var carSegmentID = (carSegment == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carSegment);

                var carClassGroup = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlCar Class Group")).SelectedItem.Value;
                var carClassGroupID = (carClassGroup == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carClassGroup);

                var carClass = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlCar Class")).SelectedItem.Value;
                var carClassID = (carClass == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carClass);

                var datePicker = (DatePicker.DatePicker)dpForecastReconciliation.FindControl("ucDatePicker");
                var date = (selectedDate != null) ? Convert.ToDateTime(selectedDate) : Convert.ToDateTime(datePicker.FromDate);

                adjustmentsCLR = bll.ForecastAdjustmentGet(country, carSegmentID,
                    carClassGroupID, carClassID, poolID, locationID, date);

                txtReconcileFromDate.Text = date.ToShortDateString();
                txtReconcileToDate.Text = date.ToShortDateString();
            }            

            grdForecastReconciliation.DataSource = adjustmentsCLR;
            Session["ForecastReconciliationList"] = adjustmentsCLR;
            grdForecastReconciliation.DataBind();
        }

        protected void grdForecastReconciliation_DataBound(object sender, EventArgs e)
        {
            SetPagerItemCountDropDown();
        }

        protected void grdForecastReconciliation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                HideGridColumns(e);

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[grdForecastReconciliation.FindColumnIndex("Adjustment_TD")].BackColor = Color.LightCyan;
                    e.Row.Cells[grdForecastReconciliation.FindColumnIndex("Adjustment_BU1")].BackColor = Color.LightSkyBlue;
                    e.Row.Cells[grdForecastReconciliation.FindColumnIndex("Adjustment_BU2")].BackColor = Color.LightGray;
    
                    Table tbl = (Table)grdForecastReconciliation.Controls[0];
                    int newRowIndex = tbl.Rows.Count - 1;
                    ForecastAdjustmentEntity ForecastReconciliation = (ForecastAdjustmentEntity)e.Row.DataItem;

                    if ((ForecastReconciliation.CMSPool.PoolDescription != currentPool) && (currentPool != string.Empty)) //if new item and not first row
                    {
                        //reset current pool
                        currentPool = ForecastReconciliation.CMSPool.PoolDescription;
                    }
                    else
                    {
                        //if (currentPool != string.Empty)
                        //    e.Row.Cells[0].Text = "";
                        //else
                        //    currentPool = ForecastReconciliation.CMSPool.PoolDescription;
                    }
                }

            }
        }

        private void HideGridColumns(GridViewRowEventArgs e)
        {
            //hide id, count and date columns
            e.Row.Cells[grdForecastReconciliation.FindColumnIndex("PoolID")].Visible = false;
            e.Row.Cells[grdForecastReconciliation.FindColumnIndex("LocationGroupID")].Visible = false;
            e.Row.Cells[grdForecastReconciliation.FindColumnIndex("CarSegmentId")].Visible = false;
            e.Row.Cells[grdForecastReconciliation.FindColumnIndex("CarGroupID")].Visible = false;
            e.Row.Cells[grdForecastReconciliation.FindColumnIndex("CarClassID")].Visible = false;
            e.Row.Cells[grdForecastReconciliation.FindColumnIndex("Count")].Visible = false;

            var country = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlCountry")).SelectedItem.Value;

            //hide columns depending on selected criteria
            var pool = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlPool")).SelectedItem.Value;
            var poolID = (pool == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(pool);
            if (poolID <= 0)
            {
                e.Row.Cells[grdForecastReconciliation.FindColumnIndex("PoolDescription")].Font.Bold = true;
                e.Row.Cells[grdForecastReconciliation.FindColumnIndex("LocationGroupName")].Visible = false;
            }
            else
                e.Row.Cells[grdForecastReconciliation.FindColumnIndex("LocationGroupName")].Font.Bold = true;

            var carSegment = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlCar Segment")).SelectedItem.Value;
            var carSegmentID = (carSegment == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carSegment);
            if (carSegmentID <= 0)
                e.Row.Cells[grdForecastReconciliation.FindColumnIndex("CarGroupDescription")].Visible = false;

            var carClassGroup = ((System.Web.UI.WebControls.DropDownList)dpForecastReconciliation.FindControl("ddlCar Class Group")).SelectedItem.Value;
            var carClassGroupID = (carClassGroup == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carClassGroup);
            if (carClassGroupID <= 0)
                e.Row.Cells[grdForecastReconciliation.FindColumnIndex("CarClassDescription")].Visible = false;

            if (carSegmentID > 0)
            {
                if (carClassGroupID <= 0)
                    e.Row.Cells[grdForecastReconciliation.FindColumnIndex("CarGroupDescription")].Font.Bold = true;
                else
                    e.Row.Cells[grdForecastReconciliation.FindColumnIndex("CarclassDescription")].Font.Bold = true;
            }
            else
                e.Row.Cells[grdForecastReconciliation.FindColumnIndex("CarSegmentName")].Font.Bold = true;
        }

        protected void grdForecastReconciliation_DataBinding(object sender, EventArgs e)
        {

        }

        protected void ddlPageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pagerRow = grdForecastReconciliation.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                grdForecastReconciliation.PageIndex = Convert.ToInt32(ddl.SelectedItem.Value) - 1;
            }

            grdForecastReconciliation.DataSource = ForecastReconciliationList;
            grdForecastReconciliation.DataBind();
        }

        protected void grdForecastReconciliation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                grdForecastReconciliation.PageIndex = e.NewPageIndex;
                grdForecastReconciliation.DataSource = ForecastReconciliationList;
                grdForecastReconciliation.DataBind();
            }
        }

        private void SetPagerItemCountDropDown()
        {
            var pagerRow = grdForecastReconciliation.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddlItemsPerPageSelector = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                ddlItemsPerPageSelector.SelectedValue = grdForecastReconciliation.PageSize.ToString();

                var ddlPageSelect = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                for (int i = 0; i < grdForecastReconciliation.PageCount; i++)
                {
                    int pageNumber = i + 1;
                    var item = new ListItem(pageNumber.ToString());

                    if (i == grdForecastReconciliation.PageIndex)
                    {
                        item.Selected = true;
                    }

                    ddlPageSelect.Items.Add(item);
                }
            }
        }

        protected void ddlItemsPerPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPageSize();
        }

        private void SetPageSize()
        {
            var pagerRow = grdForecastReconciliation.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                grdForecastReconciliation.PageSize = Convert.ToInt32(ddl.SelectedItem.Value);

                List<ForecastAdjustmentEntity> list = ForecastReconciliationList;
                grdForecastReconciliation.DataSource = list;
                grdForecastReconciliation.DataBind();
            }
        }

        protected void grdForecastReconciliation_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<ForecastAdjustmentEntity> list = ForecastReconciliationList;

            //toggle sorting - note this toggle applies to all gridviews in the page
            if (Session["SortReconcileDirection"] == null || Session["SortReconcileDirection"].ToString() == "des")
            {
                Session["SortReconcileDirection"] = "asc";
                e.SortDirection = SortDirection.Ascending;
            }
            else
            {
                Session["SortReconcileDirection"] = "des";
                e.SortDirection = SortDirection.Descending;
            }

            list.Sort(new GenericSorter<ForecastAdjustmentEntity>(e.SortExpression, e.SortDirection));

            Session["ForecastReconciliationList"] = list;

            grdForecastReconciliation.DataSource = list;
            grdForecastReconciliation.DataBind();
        }

        protected void grdForecastReconciliation_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grdForecastReconciliation_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is ParameterChangeEventArgs)
            {
                ParameterChangeEventArgs pcea = (ParameterChangeEventArgs)args;
                BindGrid(pcea.date);
                handled = true;
            }

            return handled;
        }

        protected void btnReconcile_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grdForecastReconciliation.Rows.Count; i++)
            {                
                Adjustment from = (Adjustment)GetSelectedCheckbox(i);

                if (Enum.IsDefined(typeof(Adjustment), from))
                {
                    //for each date
                    DateTime start = Convert.ToDateTime(txtReconcileFromDate.Text);
                    DateTime end = Convert.ToDateTime(txtReconcileToDate.Text);
                    for (DateTime date = start; date <= end; date = date.AddDays(1))
                    {
                        var lblPoolID = (Label)grdForecastReconciliation.Rows[i].FindControl("lblPoolID");
                        var poolID = Convert.ToInt32(lblPoolID.Text);

                        var lblLocationGroupID = (Label)grdForecastReconciliation.Rows[i].FindControl("lblLocationGroupID");
                        var locationGroupID = Convert.ToInt32(lblLocationGroupID.Text);

                        var lblSegmentID = (Label)grdForecastReconciliation.Rows[i].FindControl("lblSegmentID");
                        var segmentID = Convert.ToInt32(lblSegmentID.Text);

                        var lblGroupID = (Label)grdForecastReconciliation.Rows[i].FindControl("lblCarClassID");
                        var groupID = Convert.ToInt32(lblGroupID.Text);

                        var lblCarClassID = (Label)grdForecastReconciliation.Rows[i].FindControl("lblGroupID");
                        var carClassID = Convert.ToInt32(lblCarClassID.Text);

                        bll.AdjustmentReconcile(segmentID, groupID, carClassID, poolID, locationGroupID, from, date);
                    }
                }
            }

            BindGrid(null);
        }

        private int GetSelectedCheckbox( int gridviewRowIndex)
        {
            int TDColumnIndex = grdForecastReconciliation.FindColumnIndex("Adjustment_TD");
            if (TDColumnIndex > -1)
            {
                var chkTD = (CheckBox)grdForecastReconciliation.Rows[gridviewRowIndex].Cells[TDColumnIndex].FindControl("chkAdjustment_TD");           
                if (chkTD != null && chkTD.Checked)
                    return (int)Adjustment.Adjustment_TD;
            }

            int TD1ColumnIndex = grdForecastReconciliation.FindColumnIndex("Adjustment_BU1");
            if (TD1ColumnIndex > -1)
            {
                var chkBU1 = (CheckBox)grdForecastReconciliation.Rows[gridviewRowIndex].Cells[TD1ColumnIndex].FindControl("chkAdjustment_BU1");
                if (chkBU1 != null && chkBU1.Checked)
                    return (int)Adjustment.Adjustment_BU1;
            }

            int TD2ColumnIndex = grdForecastReconciliation.FindColumnIndex("Adjustment_BU2");
            if (TD2ColumnIndex > -1)
            {
                var chkBU2 = (CheckBox)grdForecastReconciliation.Rows[gridviewRowIndex].Cells[TD2ColumnIndex].FindControl("chkAdjustment_BU2");
                if (chkBU2 != null && chkBU2.Checked)
                    return (int)Adjustment.Adjustment_BU2;
            }

            return -1;
        }
    }
}