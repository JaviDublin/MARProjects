using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.FleetAllocation.DataAccess.Reporting.FleetComparison.Entities;

namespace Mars.FleetAllocation.UserControls.Reports
{
    public partial class FleetComparison : UserControl
    {
        public const string FaoFleetComparisonDataGridSessionHolder = "FaoFleetComparisonDataGridSessionHolder";

        protected void Page_Load(object sender, EventArgs e)
        {
            agFleetComparison.GridItemType = typeof(FleetComparisonEntity);
            agFleetComparison.SessionNameForGridData = FaoFleetComparisonDataGridSessionHolder;
            agFleetComparison.ColumnHeaders = FleetComparisonEntity.HeaderRows;
        }

        public void SetGridAndGraph(List<FleetComparisonEntity> graphingData)
        {
            ucFleetComparisonChart.LoadChart(graphingData);
            agFleetComparison.GridData = graphingData;
        }


    }
}