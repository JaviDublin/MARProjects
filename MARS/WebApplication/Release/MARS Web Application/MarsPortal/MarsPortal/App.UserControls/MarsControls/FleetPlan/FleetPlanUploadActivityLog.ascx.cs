using System;
using System.Collections.Generic;
using App.BLL.Management;
using App.BLL.ReportParameters;
using App.BLL.Utilities;
using App.Entities;
using System.Web.UI.WebControls;

namespace App.UserControls
{
    public partial class FleetPlanUploadActivityLog : System.Web.UI.UserControl
    {
        private BLLReportParameters bllParameters = new BLLReportParameters();
        private BLLManagement bllManagement = new BLLManagement();
        private static readonly string countryDummy = StaticStrings.CountryDummy;

        public List<FleetPlanEntryArchive> FleetPlanActivity
        {
            get
            {
                if (Session["FleetPlanActivity"] != null)
                {
                    var fleetPlanActivity = (List<FleetPlanEntryArchive>)Session["FleetPlanActivity"];
                    return fleetPlanActivity;
                }
                return new List<FleetPlanEntryArchive>();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindGridView(string country)
        {
            List<FleetPlanEntryArchive> activityLog = new List<FleetPlanEntryArchive>();

            if (country != countryDummy)
            {
                activityLog = bllManagement.FleetPlanEntryUploadArchiveGetByCountry(country);
                if (activityLog.Count == 0)
                    grdfleetPlanActivity.EmptyDataText = @"No Data available for selected country";

                Session["FleetPlanActivity"] = activityLog;
            }
            else
                grdfleetPlanActivity.EmptyDataText = @"Please select a country.";
            
            grdfleetPlanActivity.DataSource = activityLog;
            grdfleetPlanActivity.DataBind();
        }

        protected void grdfleetPlanActivity_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            List<FleetPlanEntryArchive> list = FleetPlanActivity;

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
            
            list.Sort(new GenericSorter<FleetPlanEntryArchive>(e.SortExpression, e.SortDirection));

            Session["FleetPlanActivity"] = list;

            grdfleetPlanActivity.DataSource = list;
            grdfleetPlanActivity.DataBind();
        }
    }
}