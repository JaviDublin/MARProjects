using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.App.UserControls.Phase4.HelpIcons
{
    public partial class ExportToExcel : UserControl
    {
        public const string ExportString = "ExportToExcel";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ibExportToExcel_Click(object sender, EventArgs e)
        {
            RaiseBubbleEvent(this, new CommandEventArgs(ExportString, null));
        }

    }
}