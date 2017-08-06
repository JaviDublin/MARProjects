using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.UserControls.Pooling
{
    public partial class GeneralTopFeedback : System.Web.UI.UserControl
    {
        public event EventHandler SwitchButtonEvent;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void buttonSwitch_Click(object sender, EventArgs e)
        {
            if (SwitchButtonEvent != null) SwitchButtonEvent(sender, e);
        }
    }
}