using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Globalization;
using App.BLL.ReportParameters;
using App.Entities;
using App.BLL.Management;
using App.BLL.Utilities;
using App.BLL.ExtensionMethods;
using Mars.App.Classes.BLL.ExtensionMethods;

namespace App.UserControls
{
    public partial class FrozenZoneManagement : System.Web.UI.UserControl
    {
        private BLLReportParameters bllParameters = new BLLReportParameters();
        private BLLManagement bllManagement = new BLLManagement();
        Helper helper = new Helper();
        private static readonly string countryDummy = StaticStrings.CountryDummy;

        public List<FrozenZoneAcceptance> FrozenZoneAcceptance
        {
            get
            {
                if (Session["FrozenZoneAcceptance"] != null)
                {
                    var frozenZoneAcceptance = (List<FrozenZoneAcceptance>)Session["FrozenZoneAcceptance"];
                    return frozenZoneAcceptance;
                }
                return new List<FrozenZoneAcceptance>();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropdowns();
                ReloadGridView();
            }
        }

        private void PopulateDropdowns()
        {
            ddlCountryList.DataTextField = "CountryDescription";
            ddlCountryList.DataValueField = "CountryID";

            Country dummy = new Country();
            dummy.CountryDescription = StaticStrings.SelectCountry;
            dummy.CountryID = countryDummy;

            List<Country> countries = bllManagement.CountryGetAllByRole(this.Page.RadUserId());

            countries.Insert(0, dummy);

            ddlCountryList.DataSource = countries;
            ddlCountryList.DataBind();

            if (countries.Count == 2)
                ddlCountryList.SelectedIndex = 1;
        }

        protected void btnApproveFrozenZone_Click(object sender, EventArgs e)
        {
            try
            {
                var country = ddlCountryList.SelectedItem.Value;

                if (country != countryDummy)
                {
                    CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("no");
                    var start = cultureInfo.Calendar.AddWeeks(DateTime.Today, 1);
                    while (start.Date.DayOfWeek != DayOfWeek.Monday)
                        start = start.AddDays(-1);

                    var end = cultureInfo.Calendar.AddWeeks(DateTime.Today, 1);
                    while (end.Date.DayOfWeek != DayOfWeek.Sunday)
                        end = end.AddDays(1);
                    end = end.AddDays(1).AddMilliseconds(-1);
                    var currentWeekNumber = helper.GetWeekNumber(DateTime.Now) + 1; //next week

                    var user = this.Page.RadUsername() + " (" + this.Page.RadUserId() + ")";
                    bllManagement.UpdateFrozenForecast(country, start, end, user, currentWeekNumber);
                    lblInfo.Text = "Frozen Zone accepted!";
                    ReloadGridView();
                }
                else
                    lblInfo.Text = " Please select a country";
            }
            catch (Exception ex)
            {
                lblInfo.Text = " Error Updating Frozen section: " + ex.Message;
            }
        }


        protected void DayRenderEventHandler(object sender, DayRenderEventArgs e)
        {
            e.Day.IsSelectable = false;
            var currentWeekNumber = helper.GetWeekNumber(DateTime.UtcNow);
            var dayWeekNumber = helper.GetWeekNumber(e.Day.Date);
            if (dayWeekNumber == (currentWeekNumber + 1))
            {
                e.Cell.BackColor = System.Drawing.Color.Yellow;
                calWeekSelector.SelectedDates.Add(e.Day.Date);
            }

            lblSelectedWeekNumber.Text = "Approval Week #: " + (currentWeekNumber + 1).ToString();
        }

        protected void calWeekSelector_SelectionChanged(object sender, EventArgs e)
        {
            var currentWeekNumber = helper.GetWeekNumber(DateTime.UtcNow);
            lblSelectedWeekNumber.Text = "Approval Week #: " + currentWeekNumber.ToString();
        }

        protected void grdFrozenZone_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                grdFrozenZone.PageIndex = e.NewPageIndex;
                grdFrozenZone.DataSource = FrozenZoneAcceptance;
                grdFrozenZone.DataBind();
            }
        }

        protected void grdFrozenZone_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            List<FrozenZoneAcceptance> list = FrozenZoneAcceptance;

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

            list.Sort(new GenericSorter<FrozenZoneAcceptance>(e.SortExpression, e.SortDirection));

            Session["FrozenZoneAcceptance"] = list;

            grdFrozenZone.DataSource = list;
            grdFrozenZone.DataBind();
        }

        protected void grdFrozenZone_DataBound(object sender, EventArgs e)
        {
            SetPagerItemCountDropDown();
        }

        protected void ddlItemsPerPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPageSize();
        }

        private void SetPageSize()
        {
            var pagerRow = grdFrozenZone.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                grdFrozenZone.PageSize = Convert.ToInt32(ddl.SelectedItem.Value);

                List<FrozenZoneAcceptance> list = FrozenZoneAcceptance;
                grdFrozenZone.DataSource = list;
                grdFrozenZone.DataBind();
            }
        }

        private void SetPagerItemCountDropDown()
        {
            var pagerRow = grdFrozenZone.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddlItemsPerPageSelector = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                ddlItemsPerPageSelector.SelectedValue = grdFrozenZone.PageSize.ToString();

                var ddlPageSelect = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                for (int i = 0; i < grdFrozenZone.PageCount; i++)
                {
                    int pageNumber = i + 1;
                    var item = new ListItem(pageNumber.ToString());

                    if (i == grdFrozenZone.PageIndex)
                    {
                        item.Selected = true;
                    }

                    ddlPageSelect.Items.Add(item);
                }
            }
        }

        protected void ddlPageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pagerRow = grdFrozenZone.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                grdFrozenZone.PageIndex = Convert.ToInt32(ddl.SelectedItem.Value) - 1;
            }

            grdFrozenZone.DataSource = FrozenZoneAcceptance;
            grdFrozenZone.DataBind();
        }

        protected void grdFrozenZone_PreRender(object sender, EventArgs e)
        {
            GridViewRow pagerRow = (GridViewRow)this.grdFrozenZone.BottomPagerRow;
            if (pagerRow != null && pagerRow.Visible == false)
                pagerRow.Visible = true;
        }
              
        protected void ddlCountryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadGridView();
        }

        private void ReloadGridView()
        {
            List<FrozenZoneAcceptance> frozenZoneAcceptanceLog = new List<FrozenZoneAcceptance>();
            var country = ddlCountryList.SelectedItem.Value;
            if (country != countryDummy)
            {
                frozenZoneAcceptanceLog = bllManagement.FrozenZoneAcceptanceGetBy(country);
                if (frozenZoneAcceptanceLog.Count == 0)
                    grdFrozenZone.EmptyDataText = @"No Data available for selected country";

                Session["FrozenZoneAcceptance"] = frozenZoneAcceptanceLog;
            }
            else
                grdFrozenZone.EmptyDataText = @"Please select a country";

            grdFrozenZone.DataSource = frozenZoneAcceptanceLog;
            grdFrozenZone.DataBind();
        }
    }
}