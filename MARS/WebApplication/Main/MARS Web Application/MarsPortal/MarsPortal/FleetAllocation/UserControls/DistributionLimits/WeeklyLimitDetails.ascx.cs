using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;
using Mars.FleetAllocation.DataAccess.VehicleDistribution;

namespace Mars.FleetAllocation.UserControls.DistributionLimits
{
    public partial class WeeklyLimitDetails : UserControl
    {
        private const string WeeklyLimitSession = "FaoWeeklyLimit";
        protected void Page_Load(object sender, EventArgs e)
        {
            ucWeeklyLimit.GridItemType = typeof(WeeklyLimitRow);
            ucWeeklyLimit.SessionNameForGridData = WeeklyLimitSession;
            ucWeeklyLimit.ColumnHeaders = WeeklyLimitRow.HeaderRows;


        }

        public void LoadData(List<WeeklyLimitRow> data)
        {
            ucWeeklyLimit.GridData = data;
        }
    }
}