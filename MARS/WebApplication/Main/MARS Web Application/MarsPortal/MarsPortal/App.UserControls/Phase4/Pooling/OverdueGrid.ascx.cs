using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.App.UserControls.Phase4.Pooling
{
    public partial class OverdueGrid : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetOverdueValues(int collections, int openTrips)
        {
            lblCollections.Text = collections.ToString("#,0");
            lblOpenTrips.Text = openTrips.ToString("#,0");
        }
    }
}