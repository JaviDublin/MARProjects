using System;
using System.Text;
using System.Web.UI;

namespace App.UserControls.Parameters
{
    public partial class QuickSelectLocationGroup : UserControl
    {
        internal string AccessMethod
        {
            set { AutoCompleteMain.ServiceMethod = value; }
            get { return AutoCompleteMain.ServiceMethod; }
        }

        internal string QuickSelectedValue
        {
            get { return tbQuickLocationGroup.Text; }
            set { tbQuickLocationGroup.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var javascript = new StringBuilder();
            javascript.Append(string.Format(@"
            $('.QuickLocationGroupInput').keyup(function(event){{
                        if (event.keyCode == 13) {{
                            __doPostBack('{0}', '');   
                            
                    }}
                    }});
                ", upQuickSelect.ClientID));

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "registerInitializer", javascript.ToString(), true);
        }

    }
}