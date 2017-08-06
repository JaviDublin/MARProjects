using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.DataAccess;

namespace Mars.FleetAllocation.UserControls
{
    public partial class MissingRevenueEntries : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMissingHoldingCosts();
            }
            
        }
        public void LoadMissingHoldingCosts()
        {
            List<string> missingCarGroups;
            using (var dataAccess = new LifecycleHoldingCostDataAccess(null))
            {
                missingCarGroups = dataAccess.GetMissingHoldingCostCarGroups(22);
            }
            lblMissingList.Text = string.Join(", ", missingCarGroups);
        }

        public void ReloadMissingEntries(object sender, EventArgs e)
        {
            LoadMissingHoldingCosts();
        }
 
    }
}