using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.FleetAllocation.DataAccess.Reporting.SiteComparison.Entities;

namespace Mars.FleetAllocation.UserControls.Reports
{
    public partial class SiteComparison : UserControl
    {
        public const string FaoSiteComparisonDataGridSessionHolder = "FaoSiteComparisonDataGridSessionHolder";

        protected void Page_Load(object sender, EventArgs e)
        {
            agSiteComparison.GridItemType = typeof(SiteComparisonEntity);
            agSiteComparison.SessionNameForGridData = FaoSiteComparisonDataGridSessionHolder;
            agSiteComparison.ColumnHeaders = SiteComparisonEntity.HeaderRows;
        }

        public void SetGridAndGraph(List<SiteComparisonEntity> graphingData)
        {
            ucSiteComparisonChart.LoadChart(graphingData);
            agSiteComparison.GridData = graphingData;
        }

    }
}