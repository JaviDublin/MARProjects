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
    public partial class MonthlyLimitDetails : UserControl
    {
        private const string MonthlyLimitSession = "FaoMontlyLimit";
        protected void Page_Load(object sender, EventArgs e)
        {
            ucMonthlyLimit.GridItemType = typeof(MonthlyLimitRow);
            ucMonthlyLimit.SessionNameForGridData = MonthlyLimitSession;
            ucMonthlyLimit.ColumnHeaders = MonthlyLimitRow.HeaderRows;

        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            ucMonthlyLimit.DataBind();
        }

        public void LoadData(List<MonthlyLimitRow> data)
        {
            ucMonthlyLimit.GridData = data;
            ucMonthlyLimit.DataBind();
        }
    }
}