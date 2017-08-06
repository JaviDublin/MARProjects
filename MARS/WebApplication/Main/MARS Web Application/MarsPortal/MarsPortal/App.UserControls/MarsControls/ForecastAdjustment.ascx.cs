using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.Management;
using App.BLL.ReportParameters;
using App.Entities;
using App.Entities.Graphing.Parameters;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.BLL.Utilities;
using App.BLL.EventArgs;
using System.Drawing;
using App.BLL.ExtensionMethods;
using Mars.App.Classes.BLL.ExtensionMethods;

namespace App.UserControls
{
    public partial class ForecastAdjustment : System.Web.UI.UserControl
    {
        private readonly BLLManagement bll = new BLLManagement();
        private readonly BLLReportParameters bllParams = new BLLReportParameters();

        string currentPool = string.Empty;
        List<UserRole> roles = new List<UserRole>();
                
        public Dictionary<string, string> SelectedParameters { get; set; }

        private List<ReportParameter> ForecastAdjustmentParameters
        {
            get
            {
                return (List<ReportParameter>)Session["ForecastAdjustmentParameters"];
            }
            set 
            { 
                Session["ForecastAdjustmentParameters"] = value; 
            }
        }

        public List<ForecastAdjustmentEntity> ForecastAdjustmentList
        {
            get
            {
                if (Session["ForecastAdjustmentList"] != null)
                {
                    var adjustment = (List<ForecastAdjustmentEntity>)Session["ForecastAdjustmentList"];
                    return adjustment;
                }
                return new List<ForecastAdjustmentEntity>();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SelectedParameters = new Dictionary<string, string>();
                Page.LoadComplete += new EventHandler(Page_LoadComplete);

                rvAdjustmentStartDate.MinimumValue = DateTime.Today.AddDays(1).ToShortDateString();
                rvAdjustmentStartDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
                rvAdjustmentStartDate.Text = "Historical Start Date is not allowed";

                rvAdaptFromDate.MinimumValue = DateTime.Today.AddDays(1).ToShortDateString();
                rvAdaptFromDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
                rvAdaptFromDate.Text = "Historical Start Date is not allowed";  
            }
            roles = bll.UsersInRolesGet(this.Page.RadUserId());
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            PopulateCountyDropDown();
            PopulateRoleBasedControls();
            BindGrid(null);
        }

        private void PopulateCountyDropDown()
        {            
            Country dummy = new Country();
            dummy.CountryDescription = StaticStrings.SelectCountry;
            dummy.CountryID = StaticStrings.CountryDummy;

            List<Country> countries = bllParams.CountryGetAll();
            List<Country> accessCountries = new List<Country>();

            
            foreach (Country country in countries)
            {
                //if (roles.Exists(r => (r.RoleCountry == country.CountryID) &&
                //    ((RoleCodes)r.RoleID == RoleCodes.BU1Adjustment ||
                //    (RoleCodes)r.RoleID == RoleCodes.BU2Adjustment ||
                //    (RoleCodes)r.RoleID == RoleCodes.TDAdjustmentAndReconciliation)))
                    accessCountries.Add(country);
            }

            var ddlCountryList = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlCountry"));
            ddlCountryList.DataTextField = "CountryDescription";
            ddlCountryList.DataValueField = "CountryID";
            accessCountries.Insert(0, dummy);
            ddlCountryList.DataSource = accessCountries;
            ddlCountryList.DataBind();

            if (accessCountries.Count == 2)
                ddlCountryList.SelectedIndex = 1;

            ddlAdaptFrom.DataSource = Enum.GetValues(typeof(AdjustmentForecast));
            ddlAdaptFrom.DataBind();

        }

        private void PopulateRoleBasedControls()
        {
            System.Web.UI.WebControls.DropDownList ddlCountryList = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlCountry"));
            ListItemCollection collection = new ListItemCollection();

            roles = roles.Where( r => r.RoleCountry == ddlCountryList.SelectedItem.Value).ToList();

            foreach (UserRole r in roles)
            {
                if ((RoleCodes)r.RoleID == RoleCodes.BU1Adjustment ||
                    (RoleCodes)r.RoleID == RoleCodes.BU2Adjustment ||
                    (RoleCodes)r.RoleID == RoleCodes.TDAdjustmentAndReconciliation)
                {
                    collection.Add(new ListItem(Enum.GetName(typeof(Adjustment), r.RoleID), r.RoleID.ToString()));
                }
            }

            lstAdjustments.DataSource = collection;
            lstAdjustments.DataTextField = "text";
            lstAdjustments.DataValueField = "value";           
            lstAdjustments.DataBind();
            if (lstAdjustments.Items.Count > 0)
                lstAdjustments.SelectedIndex = 0;

            ddlAdaptTo.DataSource = collection;
            ddlAdaptTo.DataTextField = "text";
            ddlAdaptTo.DataValueField = "value";            
            ddlAdaptTo.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (ForecastAdjustmentParameters == null)
            {
                ForecastAdjustmentParameters = new List<ReportParameter>
                {
                        new ReportParameter(0, 0, null, ParameterNames.Country),
                        new ReportParameter(1, 1, ParameterDataAccess.GetPoolParameterListItems,ParameterNames.Pool),
                        new ReportParameter(1, 2, ParameterDataAccess.GetLocationGroupParameterListItems,ParameterNames.LocationGroup),
                        new ReportParameter(2, 1, ParameterDataAccess.GetCarSegmentParameterListItems,ParameterNames.CarSegment),
                        new ReportParameter(2, 2, ParameterDataAccess.GetCarClassGroupParameterListItems,ParameterNames.CarClassGroup),
                        new ReportParameter(2, 3, ParameterDataAccess.GetCarClassParameterListItems,ParameterNames.CarClass)
                };
            }
            dpForecastAdjustments.ParamsHolder = ForecastAdjustmentParameters;
        }

        private void BindGrid(string selectedDate)
        {
            var adjustmentsCLR = new List<ForecastAdjustmentEntity>();
            var country = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlCountry")).SelectedItem.Value;
            if (country != StaticStrings.CountryDummy)
            {

                var pool = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlPool")).SelectedItem.Value;
                var poolID = (pool == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(pool);

                var location = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlLocation Group")).SelectedItem.Value;
                var locationID = (location == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(location);

                var carSegment = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlCar Segment")).SelectedItem.Value;
                var carSegmentID = (carSegment == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carSegment);

                var carClassGroup = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlCar Class Group")).SelectedItem.Value;
                var carClassGroupID = (carClassGroup == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carClassGroup);

                var carClass = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlCar Class")).SelectedItem.Value;
                var carClassID = (carClass == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carClass);

                var datePicker = (DatePicker.DatePicker)dpForecastAdjustments.FindControl("ucDatePicker");
                var date = (selectedDate != null) ? Convert.ToDateTime(selectedDate) : Convert.ToDateTime(datePicker.FromDate);

                adjustmentsCLR = bll.ForecastAdjustmentGet(country, carSegmentID,
                    carClassGroupID, carClassID, poolID, locationID, date);
            }

            grdForecastAdjustment.DataSource = adjustmentsCLR;
            Session["ForecastAdjustmentList"] = adjustmentsCLR;
            grdForecastAdjustment.DataBind();
        }

        protected void grdForecastAdjustment_DataBound(object sender, EventArgs e)
        {
            SetPagerItemCountDropDown();
        }
    
        protected void grdForecastAdjustment_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            if (e.Row.Cells.Count > 1)
            {
                HideGridColumns(e);

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //e.Row.Attributes.Add("onmouseover", "this.style.cursor='hand'; this.style.backgroundColor='ActiveBorder';");
                    //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='';");
                    //e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackClientHyperlink(this.grdForecastAdjustment, "Select$" + e.Row.RowIndex));
                   
                    Table tbl = (Table)grdForecastAdjustment.Controls[0];
                    int newRowIndex = tbl.Rows.Count - 1;
                    ForecastAdjustmentEntity forecastadjustment = (ForecastAdjustmentEntity)e.Row.DataItem;

                    if ((forecastadjustment.CMSPool.PoolDescription != currentPool) && (currentPool != string.Empty)) //if new item and not first row
                    {
                        //reset current pool
                        currentPool = forecastadjustment.CMSPool.PoolDescription;
                    }
                    else
                    {
                        //if (currentPool != string.Empty)
                        //    e.Row.Cells[0].Text = "";
                        //else
                        //    currentPool = forecastadjustment.CMSPool.PoolDescription;
                    }
                }

            }
        }

        private void HideGridColumns(GridViewRowEventArgs e)
        {
            //hide id, count and date columns
            e.Row.Cells[grdForecastAdjustment.FindColumnIndex("PoolID")].Visible = false;
            e.Row.Cells[grdForecastAdjustment.FindColumnIndex("LocationGroupID")].Visible = false;
            e.Row.Cells[grdForecastAdjustment.FindColumnIndex("CarSegmentId")].Visible = false;
            e.Row.Cells[grdForecastAdjustment.FindColumnIndex("CarGroupID")].Visible = false;
            e.Row.Cells[grdForecastAdjustment.FindColumnIndex("CarClassID")].Visible = false;
            e.Row.Cells[grdForecastAdjustment.FindColumnIndex("Count")].Visible = false;
            e.Row.Cells[grdForecastAdjustment.FindColumnIndex("Adjustment_RC")].Visible = false;

            var country = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlCountry")).SelectedItem.Value;

            //hide columns depending on selected criteria
            var pool = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlPool")).SelectedItem.Value;
            var poolID = (pool == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(pool);
            if (poolID <= 0)
            {
                e.Row.Cells[grdForecastAdjustment.FindColumnIndex("PoolDescription")].Font.Bold = true;
                e.Row.Cells[grdForecastAdjustment.FindColumnIndex("LocationGroupName")].Visible = false;
            }
            else
                e.Row.Cells[grdForecastAdjustment.FindColumnIndex("LocationGroupName")].Font.Bold = true;

            var carSegment = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlCar Segment")).SelectedItem.Value;
            var carSegmentID = (carSegment == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carSegment);
            if (carSegmentID <= 0)
                e.Row.Cells[grdForecastAdjustment.FindColumnIndex("CarGroupDescription")].Visible = false;

            var carClassGroup = ((System.Web.UI.WebControls.DropDownList)dpForecastAdjustments.FindControl("ddlCar Class Group")).SelectedItem.Value;
            var carClassGroupID = (carClassGroup == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carClassGroup);
            if (carClassGroupID <= 0)
                e.Row.Cells[grdForecastAdjustment.FindColumnIndex("CarClassDescription")].Visible = false;

            if (carSegmentID > 0)
            {
                if (carClassGroupID <= 0)
                    e.Row.Cells[grdForecastAdjustment.FindColumnIndex("CarGroupDescription")].Font.Bold = true;
                else
                    e.Row.Cells[grdForecastAdjustment.FindColumnIndex("CarclassDescription")].Font.Bold = true;
            }
            else
                e.Row.Cells[grdForecastAdjustment.FindColumnIndex("CarSegmentName")].Font.Bold = true;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (roles.Exists(r => (r.RoleCountry == country) &&
                        (RoleCodes)r.RoleID == RoleCodes.TDAdjustmentAndReconciliation))
                    e.Row.Cells[grdForecastAdjustment.FindColumnIndex("Adjustment_TD")].BackColor = Color.LightCyan;

                if (roles.Exists(r => (r.RoleCountry == country) &&
                        (RoleCodes)r.RoleID == RoleCodes.BU1Adjustment))
                    e.Row.Cells[grdForecastAdjustment.FindColumnIndex("Adjustment_BU1")].BackColor = Color.LightCyan;

                if (roles.Exists(r => (r.RoleCountry == country) &&
                        (RoleCodes)r.RoleID == RoleCodes.BU2Adjustment))
                    e.Row.Cells[grdForecastAdjustment.FindColumnIndex("Adjustment_BU2")].BackColor = Color.LightCyan;
            }
            
                
        }
                
        protected void grdForecastAdjustment_DataBinding(object sender, EventArgs e)
        {
             
        }

        protected void ddlPageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pagerRow = grdForecastAdjustment.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                grdForecastAdjustment.PageIndex = Convert.ToInt32(ddl.SelectedItem.Value) - 1;
            }

            grdForecastAdjustment.DataSource = ForecastAdjustmentList;
            grdForecastAdjustment.DataBind();
        }

        protected void grdForecastAdjustment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                grdForecastAdjustment.PageIndex = e.NewPageIndex;
                grdForecastAdjustment.DataSource = ForecastAdjustmentList;
                grdForecastAdjustment.DataBind();
            }
        }

        private void SetPagerItemCountDropDown()
        {
            var pagerRow = grdForecastAdjustment.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddlItemsPerPageSelector = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                ddlItemsPerPageSelector.SelectedValue = grdForecastAdjustment.PageSize.ToString();

                var ddlPageSelect = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                for (int i = 0; i < grdForecastAdjustment.PageCount; i++)
                {
                    int pageNumber = i + 1;
                    var item = new ListItem(pageNumber.ToString());

                    if (i == grdForecastAdjustment.PageIndex)
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
            var pagerRow = grdForecastAdjustment.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                grdForecastAdjustment.PageSize = Convert.ToInt32(ddl.SelectedItem.Value);

                List<ForecastAdjustmentEntity> list = ForecastAdjustmentList;
                grdForecastAdjustment.DataSource = list;
                grdForecastAdjustment.DataBind();
            }
        }

        protected void grdForecastAdjustment_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<ForecastAdjustmentEntity> list = ForecastAdjustmentList;

            //toggle sorting - note this toggle applies to all gridviews in the page
            if (Session["SortAdjustmentDirection"] == null || Session["SortAdjustmentDirection"].ToString() == "des")
            {
                Session["SortAdjustmentDirection"] = "asc";
                e.SortDirection = SortDirection.Ascending;
            }
            else
            {
                Session["SortAdjustmentDirection"] = "des";
                e.SortDirection = SortDirection.Descending;
            }

            list.Sort(new GenericSorter<ForecastAdjustmentEntity>(e.SortExpression, e.SortDirection));

            Session["ForecastAdjustmentList"] = list;

            grdForecastAdjustment.DataSource = list;
            grdForecastAdjustment.DataBind();
        }

        protected void grdForecastAdjustment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var datePicker = (DatePicker.DatePicker)dpForecastAdjustments.FindControl("ucDatePicker");
            if (SelectedRowExists())
            {
                if (e.CommandName == "Edit")
                {
                    txtAdjustmentFromDate.Text = datePicker.FromDate;
                    txtAdjustmentToDate.Text = datePicker.FromDate;
                    ajaxEditAdjustmentPopup.Show();
                }

                if (e.CommandName == "Adapt")
                {
                    txtAdaptFromDate.Text = datePicker.FromDate;
                    txtAdaptToDate.Text = datePicker.FromDate;
                    ajaxEditAdaptPopup.Show();
                }
            }
        }

        private bool SelectedRowExists()
        {
            for (int i = 0; i < grdForecastAdjustment.Rows.Count; i++)
            {
                int selectedColumnIndex = grdForecastAdjustment.FindColumnIndex("SelectedItem");
                if (selectedColumnIndex > -1)
                {
                    var chkSelected = (CheckBox)grdForecastAdjustment.Rows[i].Cells[selectedColumnIndex].FindControl("chkSelected");
                    if (chkSelected != null)
                    {
                        if (chkSelected.Checked)
                            return true;
                    }
                }
            }
            return false;
        }

        protected void btnApplyChanges_Click(object sender, EventArgs e)
        {
            if (rvAdjustmentStartDate.IsValid)
            {
                foreach (int index in lstAdjustments.GetSelectedIndices())
                {
                    var ajustmentToUpdate = Convert.ToInt32(lstAdjustments.Items[index].Value);

                    var direction = ddlDirection.SelectedItem.Value;
                    bool addition = (direction == "+") ? true : false;

                    var value = Convert.ToDecimal(txtValue.Text);

                    var adjustmentTypeValue = ddlOperation.SelectedItem.Value;
                    var adjustmentType = (int)Enum.Parse(typeof(App.BLL.Utilities.AdjustmentType), adjustmentTypeValue);

                    for (int i = 0; i < grdForecastAdjustment.Rows.Count; i++)
                    {
                        int selectedColumnIndex = grdForecastAdjustment.FindColumnIndex("SelectedItem");
                        if (selectedColumnIndex > -1)
                        {
                            var chkSelected = (CheckBox)grdForecastAdjustment.Rows[i].Cells[selectedColumnIndex].FindControl("chkSelected");
                            if (chkSelected != null)
                            {
                                if (chkSelected.Checked)
                                {
                                    //for each date
                                    DateTime start = Convert.ToDateTime(txtAdjustmentFromDate.Text);
                                    DateTime end = Convert.ToDateTime(txtAdjustmentToDate.Text);
                                    for (DateTime date = start; date <= end; date = date.AddDays(1))
                                    {
                                        var lblPoolID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblPoolID");
                                        var poolID = Convert.ToInt32(lblPoolID.Text);

                                        var lblLocationGroupID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblLocationGroupID");
                                        var locationGroupID = Convert.ToInt32(lblLocationGroupID.Text);

                                        var lblSegmentID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblSegmentID");
                                        var segmentID = Convert.ToInt32(lblSegmentID.Text);

                                        var lblGroupID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblCarClassID");
                                        var groupID = Convert.ToInt32(lblGroupID.Text);

                                        var lblCarClassID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblGroupID");
                                        var carClassID = Convert.ToInt32(lblCarClassID.Text);

                                        bll.AdjustmentUpdate(segmentID, groupID, carClassID, poolID, locationGroupID, ajustmentToUpdate,
                                            adjustmentType, addition, value, date);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            BindGrid(null);
        }

        protected void grdForecastAdjustment_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is ParameterChangeEventArgs)
            {
                ParameterChangeEventArgs pcea = (ParameterChangeEventArgs)args;
                Page.Validate();
                if (Page.IsValid)
                {
                    PopulateRoleBasedControls();
                    BindGrid(pcea.date);
                }
                handled = true;
            }

            return handled;
        }

        protected void btnAdaptApply_Click(object sender, EventArgs e)
        {
            var from = (AdjustmentForecast)Enum.Parse(typeof(AdjustmentForecast), ddlAdaptFrom.SelectedItem.Value);
            var to = (Adjustment)Enum.Parse(typeof(Adjustment), ddlAdaptTo.SelectedItem.Value);

            for (int i = 0; i < grdForecastAdjustment.Rows.Count; i++)
            {
                int selectedColumnIndex = grdForecastAdjustment.FindColumnIndex("SelectedItem");
                if (selectedColumnIndex > -1)
                {
                    var chkSelected = (CheckBox)grdForecastAdjustment.Rows[i].Cells[selectedColumnIndex].FindControl("chkSelected");
                    if (chkSelected != null)
                    {
                        if (chkSelected.Checked)
                        {
                            //for each date
                            DateTime start = Convert.ToDateTime(txtAdaptFromDate.Text);
                            DateTime end = Convert.ToDateTime(txtAdaptToDate.Text);
                            for (DateTime date = start; date <= end; date = date.AddDays(1))
                            {
                                var lblPoolID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblPoolID");
                                var poolID = Convert.ToInt32(lblPoolID.Text);

                                var lblLocationGroupID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblLocationGroupID");
                                var locationGroupID = Convert.ToInt32(lblLocationGroupID.Text);

                                var lblSegmentID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblSegmentID");
                                var segmentID = Convert.ToInt32(lblSegmentID.Text);

                                var lblGroupID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblCarClassID");
                                var groupID = Convert.ToInt32(lblGroupID.Text);

                                var lblCarClassID = (Label)grdForecastAdjustment.Rows[i].FindControl("lblGroupID");
                                var carClassID = Convert.ToInt32(lblCarClassID.Text);

                                bll.AdjustmentAdapt(segmentID, groupID, carClassID, poolID, locationGroupID, from, to, date);
                            }
                        }
                    }
                }
            }

            BindGrid(null);
        }
    }
}