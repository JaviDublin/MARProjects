using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.UsageStatistics;
using Mars.App.Classes.Phase4Dal.UsageStatistics.Entities;

namespace Mars.App.Site.Administration.Statistics
{
    public partial class PageStatistics : Page
    {

        public const string SelectCommand = "SelectCommand";
        public const string UnSelectCommand = "UnSelectCommand";

        public const string PageStatisticsAreasSelected = "PageStatisticsAreasSelected";
        public List<PageAreaEnitity> MenuItems
        {
            get { return (List<PageAreaEnitity>)Session[PageStatisticsAreasSelected]; }
            set { Session[PageStatisticsAreasSelected] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulatePageAreas();
            }
            PopulateRepeater();
        }

        private void PopulatePageAreas()
        {
            using (var dataAccess = new UsageStatisticsDataAccess(null))
            {
                MenuItems = dataAccess.GetMarsPageAreas();
            }
            
        }

        public void RepeaterCommand(object sender, EventArgs e)
        {
            var areas = MenuItems;
            if (e is CommandEventArgs)
            {
                var commandEventArgs = e as CommandEventArgs;
                if (commandEventArgs.CommandName == SelectCommand || commandEventArgs.CommandName == UnSelectCommand)
                {
                    var selectedId = int.Parse(commandEventArgs.CommandArgument.ToString());
                    var selectCommand = commandEventArgs.CommandName == SelectCommand;
                    var selectedArea = areas.Single(d => d.MenuId == selectedId);
                    selectedArea.Selected = selectCommand;

                }
                PopulateRepeater();
            }
        }

        private void PopulateRepeater()
        {
            lvPages.DataSource = MenuItems;
            lvPages.DataBind();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            var dt1 = DateTime.Parse(tbFromDate.Text);
            var dt2 = DateTime.Parse(tbToDate.Text).AddDays(1).AddSeconds(-1);

            int takeCount = int.Parse(ddlTakeCount.SelectedValue);
            var parameters = new Dictionary<DictionaryParameter, string>();
            parameters[DictionaryParameter.StartDate] = dt1.ToString();
            parameters[DictionaryParameter.EndDate] = dt2.ToString();

            var selectedMenuIds = MenuItems.Where(d => d.Selected).Select(d => d.MenuId).ToList();
            using (var dataAccess = new UsageStatisticsDataAccess(parameters))
            {
                var usageData = dataAccess.GetPageUsage(takeCount, selectedMenuIds);
                ucPageUsageChart.LoadUseageData(usageData);

            }
        }
    }
}