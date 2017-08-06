using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.UserControls.AutoComplete
{
    public partial class AutoComplete : System.Web.UI.UserControl
    {
        public event EventHandler SearchTerm;

        public string SelectedText
        {
            get
            {
                this.EnsureChildControls();
                object o = this.AutoCompleteControl.SelectedText;
                return (o != null) ? o.ToString() : null;
            }
        }

        protected void OnAutoCompleteCommand(object sender, CommandEventArgs e)
        {
            if (SearchTerm != null)
            {
                SearchTerm(this, EventArgs.Empty);
            }

            this.UpdatePanelAutocomplete.Update();
        }

    }
}