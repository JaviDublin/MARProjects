using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.ExcelExport;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.Entities;
using App.BLL.ReportParameters;
using App.BLL.Management;
using App.BLL.Utilities;
using App.BLL.EventArgs;
using App.Entities.Graphing.Parameters;
using App.BLL.ExtensionMethods;
using Mars.App.Classes.BLL.ExtensionMethods;

namespace App.UserControls
{
    public partial class NecessaryFleetManagement : System.Web.UI.UserControl
    {
        public Dictionary<string, string> SelectedParameters { get; set; }
        
        private BLLReportParameters bllParameters = new BLLReportParameters();
        private BLLManagement bllManagement = new BLLManagement();
        private static readonly string countryDummy = StaticStrings.CountryDummy;
        
        private List<ReportParameter> NecessaryFleetParameters
        {
            get
            {
                return (List<ReportParameter>)Session["NecessaryFleetParameters"];
            }
            set { Session["NecessaryFleetParameters"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                SelectedParameters = new Dictionary<string, string>();                
                Page.LoadComplete += new EventHandler(Page_LoadComplete);
            }
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            PopulateDropDowns();
            ReloadGridView();
        }

        private void PopulateDropDowns()
        {

            Country dummy = new Country();
            dummy.CountryDescription = StaticStrings.SelectCountry;
            dummy.CountryID = StaticStrings.CountryDummy;

            List<Country> countries = bllManagement.CountryGetAllByRole(this.Page.RadUserId());
            System.Web.UI.WebControls.DropDownList ddlCountryList = ((System.Web.UI.WebControls.DropDownList)DynamicParams.FindControl("ddlCountry"));

            ddlCountryList.DataTextField = "CountryDescription";
            ddlCountryList.DataValueField = "CountryID";
            ddlCountryListReset.DataTextField = "CountryDescription";
            ddlCountryListReset.DataValueField = "CountryID";

            countries.Insert(0, dummy);
            
            ddlCountryList.DataSource = countries;
            ddlCountryList.DataBind();

            ddlCountryListReset.DataSource = countries;
            ddlCountryListReset.DataBind();

            if (countries.Count == 2)
            {
                ddlCountryList.SelectedIndex = 1;
                ddlCountryListReset.SelectedIndex = 1;
            }

            ddlResetType.Items.Add(new ListItem("Utilisation", "1"));
            ddlResetType.Items.Add(new ListItem("Non/Rev", "2"));
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (NecessaryFleetParameters == null)
            {
                NecessaryFleetParameters = new List<ReportParameter>
                {
                    new ReportParameter(0, 0, null, ParameterNames.Country),
                    new ReportParameter(1, 1, ParameterDataAccess.GetLocationGroupParameterListItemsAllCountry, ParameterNames.LocationGroup),
                    new ReportParameter(2, 1, ParameterDataAccess.GetCarClassParameterListItemsAllCountry, ParameterNames.CarClass)
                };
            }
            DynamicParams.ParamsHolder = NecessaryFleetParameters;
        }

        public List<NecessaryFleet> NecessaryFleet
        {
            get
            {
                if (Session["NecessaryFleet"] != null)
                {
                    var necessaryFleet = (List<NecessaryFleet>)Session["NecessaryFleet"];
                    return necessaryFleet;
                }
                return new List<NecessaryFleet>();
            }
        }

        private void ReloadGridView()
        {
            List<NecessaryFleet> necessaryFleetList = new List<NecessaryFleet>();

            var country = ((System.Web.UI.WebControls.DropDownList)DynamicParams.FindControl("ddlCountry")).SelectedItem.Value;
            var locationGroup = ((System.Web.UI.WebControls.DropDownList)DynamicParams.FindControl("ddlLocation Group")).SelectedItem.Value;
            var carClass = ((System.Web.UI.WebControls.DropDownList)DynamicParams.FindControl("ddlCar Class")).SelectedItem.Value;
            if (country != StaticStrings.CountryDummy)
            {
                necessaryFleetList = bllManagement.GetNecessaryFleetByCountryID(country);
                if (necessaryFleetList.Count == 0)
                    grdNecessaryFleet.EmptyDataText = @"No Data available for selected country";

                if (locationGroup != StaticStrings.DDLDefault)
                    necessaryFleetList = necessaryFleetList.Where(p => p.LocationGroup.LocationGroupID ==
                        Convert.ToInt32(locationGroup)).ToList();

                if (carClass != StaticStrings.DDLDefault)
                    necessaryFleetList = necessaryFleetList.Where(p => p.CarGroup.CarGroupID ==
                        Convert.ToInt32(carClass)).ToList();

                Session["NecessaryFleet"] = necessaryFleetList;
            }
            else
                grdNecessaryFleet.EmptyDataText = @"Please select a country";

            grdNecessaryFleet.DataSource = necessaryFleetList;
            grdNecessaryFleet.DataBind();
        }

        protected void grdNecessaryFleet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                HideGridColumns(e);
            }
        }

        private void HideGridColumns(GridViewRowEventArgs e)
        {
            e.Row.Cells[grdNecessaryFleet.FindColumnIndex("CountryID")].Visible = false;
            e.Row.Cells[grdNecessaryFleet.FindColumnIndex("LocationGroupID")].Visible = false;
            e.Row.Cells[grdNecessaryFleet.FindColumnIndex("CarGroupID")].Visible = false;
        }

        protected void grdNecessaryFleet_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                grdNecessaryFleet.PageIndex = e.NewPageIndex;
                grdNecessaryFleet.DataSource = NecessaryFleet;
                grdNecessaryFleet.DataBind();
            }
        }

        protected void grdNecessaryFleet_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            List<NecessaryFleet> list = NecessaryFleet;

            //toggle sorting - note this toggle applies to all gridviews in the page
            if (Session["SortActivityDirection"] == null || Session["SortActivityDirection"].ToString() == "des")
            {
                Session["SortActivityDirection"] = "asc";
                e.SortDirection = SortDirection.Ascending;
            }
            else
            {
                Session["SortActivityDirection"] = "des";
                e.SortDirection = SortDirection.Descending;
            }

            list.Sort(new GenericSorter<NecessaryFleet>(e.SortExpression, e.SortDirection));

            Session["NecessaryFleet"] = list;

            grdNecessaryFleet.DataSource = list;
            grdNecessaryFleet.DataBind();
        }

        protected void grdNecessaryFleet_DataBound(object sender, EventArgs e)
        {
            SetPagerItemCountDropDown();
        }

        protected void ddlItemsPerPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPageSize();
        }

        private void SetPageSize()
        {
            var pagerRow = grdNecessaryFleet.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                grdNecessaryFleet.PageSize = Convert.ToInt32(ddl.SelectedItem.Value);

                List<NecessaryFleet> list = NecessaryFleet;
                grdNecessaryFleet.DataSource = list;
                grdNecessaryFleet.DataBind();
            }
        }

        private void SetPagerItemCountDropDown()
        {
            var pagerRow = grdNecessaryFleet.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddlItemsPerPageSelector = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                ddlItemsPerPageSelector.SelectedValue = grdNecessaryFleet.PageSize.ToString();

                var ddlPageSelect = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                for (int i = 0; i < grdNecessaryFleet.PageCount; i++)
                {
                    int pageNumber = i + 1;
                    var item = new ListItem(pageNumber.ToString());

                    if (i == grdNecessaryFleet.PageIndex)
                    {
                        item.Selected = true;
                    }

                    ddlPageSelect.Items.Add(item);
                }
            }
        }

        protected void ddlPageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pagerRow = grdNecessaryFleet.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                grdNecessaryFleet.PageIndex = Convert.ToInt32(ddl.SelectedItem.Value) - 1;
            }

            grdNecessaryFleet.DataSource = NecessaryFleet;
            grdNecessaryFleet.DataBind();
        }

        protected void grdNecessaryFleet_PreRender(object sender, EventArgs e)
        {
            GridViewRow pagerRow = (GridViewRow)this.grdNecessaryFleet.BottomPagerRow;
            if (pagerRow != null && pagerRow.Visible == false)
                pagerRow.Visible = true;
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is ParameterChangeEventArgs)
            {
                ReloadGridView();
                handled = true;
            }

            return handled;
        }

        protected void btnMultiUpdate_Click(object sender, EventArgs e)
        {
            var country = ((System.Web.UI.WebControls.DropDownList)DynamicParams.FindControl("ddlCountry")).SelectedItem.Value;
            if (country == StaticStrings.DDLDefault)
                return;

            var location = ((System.Web.UI.WebControls.DropDownList)DynamicParams.FindControl("ddlLocation Group")).SelectedItem.Value;
            var locationGroupID = (location == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(location);

            var carClass = ((System.Web.UI.WebControls.DropDownList)DynamicParams.FindControl("ddlCar Class")).SelectedItem.Value;
            var carClassID = (carClass == StaticStrings.DDLDefault) ? -1 : Convert.ToInt32(carClass);
            
            
            var utilizationValue = (txtUtilisation.Text.Length == 0) ? -1 : Convert.ToDouble(txtUtilisation.Text);
            var nonRevValue = (txtNonRev.Text.Length == 0) ? -1 : Convert.ToDouble(txtNonRev.Text);
            if (utilizationValue == -1 && nonRevValue == -1)
                return;

            bllManagement.NecessaryFleetMultipleUpdate(country, locationGroupID, carClassID, utilizationValue, nonRevValue);

            ReloadGridView();
        }

        protected void grdNecessaryFleet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdNecessaryFleet.Rows[index];

                var lblCountryID = (Label)row.FindControl("lblCountryID");
                var countryID = lblCountryID.Text;
                hdnNecFleetCountryID.Value = countryID;

                var lblLocationGroupID = (Label)row.FindControl("lblLocationGroupID");
                var locationGroupID = lblLocationGroupID.Text;
                hdnNecFleetLocationGroupID.Value = locationGroupID;

                var lblCarClassID = (Label)row.FindControl("lblCarClassID");
                var carClassID = lblCarClassID.Text;
                hdnNecFleetCarClassID.Value = carClassID;

                var lblCountryDescription = (Label)row.FindControl("lblCountryDescription");
                txtCountry.Text = lblCountryDescription.Text;

                var lblLocationGroupName = (Label)row.FindControl("lblLocationGroupName");
                txtLocationGroup.Text = lblLocationGroupName.Text;

                var lblCarclassDescription = (Label)row.FindControl("lblCarclassDescription");
                txtCarClass.Text = lblCarclassDescription.Text;

                var lblNonRevFleet = (Label)row.FindControl("lblNonRevFleet");
                txtNonRevEdit.Text = lblNonRevFleet.Text;

                var lblUtilization = (Label)row.FindControl("lblUtilization");
                txtUtilisationEdit.Text = lblUtilization.Text;

                ajaxNecessaryFleetPopup.Show();               
            }
        }

        protected void grdNecessaryFleet_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnSaveNecessaryFleetUpdate_Click(object sender, EventArgs e)
        {
            var utilizationValue = Convert.ToDecimal(txtUtilisationEdit.Text);
            var nonRevValue = Convert.ToDecimal(txtNonRevEdit.Text);

            var countryID = hdnNecFleetCountryID.Value;
            var locationGroupID = Convert.ToInt32(hdnNecFleetLocationGroupID.Value);
            var carGroupID = Convert.ToInt32(hdnNecFleetCarClassID.Value);

            var updatedNecessaryFleet = new NecessaryFleet();
            updatedNecessaryFleet.CarGroup.CarGroupID = carGroupID;
            updatedNecessaryFleet.LocationGroup.LocationGroupID = locationGroupID;
            updatedNecessaryFleet.Country.CountryID = countryID;
            updatedNecessaryFleet.NonRevFleet = nonRevValue;
            updatedNecessaryFleet.Utilization = utilizationValue;

            bllManagement.NecessaryFleetUpdate(updatedNecessaryFleet);

            ReloadGridView();
        }

        private void InitializeGridViews()
        {
            grdNecessaryFleet.DataSource = new List<NecessaryFleet>();
            grdNecessaryFleet.DataBind();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            var country =  ddlCountryListReset.SelectedItem.Value;
            var countryname = ddlCountryListReset.SelectedItem.Text;

            var resetType = Convert.ToInt32(ddlResetType.SelectedItem.Value);

            var fromDate = Convert.ToDateTime(drpNecFleetReset.FromDate);
            var toDate = Convert.ToDateTime(drpNecFleetReset.ToDate);

            if (country == StaticStrings.CountryDummy)
            {
                lblNecFleetMessage.Text = "You must first select a country";
            }
            else
            {
                if(resetType == 1)
                    bllManagement.NecessaryFleetUtilisationUpdate(country, fromDate, toDate);
                else
                    bllManagement.NecessaryFleetNonRevUpdate(country, fromDate, toDate);
                
                ReloadGridView();
                lblNecFleetMessage.Text = "Values reset for " + countryname;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var country = ((System.Web.UI.WebControls.DropDownList)DynamicParams.FindControl("ddlCountry")).SelectedItem.Value;

            if (country != countryDummy)
            {
                var bll = new BLLExcelExport();

                Session["ExportData"] = bll.GetNecessaryFleetExport(country);
                Session["ExportFileName"] = "NecessaryFleet_" + country + "_";

                //Response.Redirect("../ExcelGenerator.aspx?country=" + country + "&reportType=9");
            }
        }
    }
}