﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.DataAccess;

namespace Mars.FleetAllocation.UserControls
{
    public partial class MissingHoldingCosts : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateMissingRevenueEntries();
            }
            
        }

        private void PopulateMissingRevenueEntries()
        {

            List<string> missingCarGroups;
            using (var dataAccess = new RevenueDataAccess(null))
            {
                missingCarGroups = dataAccess.GetMissingRevenueCarGroups(22);
            }
            lblMissingList.Text = string.Join(", ", missingCarGroups);

        }

        public void ReloadMissingEntries(object sender, EventArgs e)
        {
            PopulateMissingRevenueEntries();
        }
    }
}