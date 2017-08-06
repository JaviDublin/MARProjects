using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.App.UserControls.DatePicker
{
    public partial class SingleDateTimePicker : UserControl
    {
        public DateTime? SelectedDateTime
        {
            get
            {
                DateTime parsedDateTime;

                var succeeded = DateTime.TryParse(string.Format("{0} {1}", tbDateSelection.Text, tbTimeSelection.Text)
                    , out parsedDateTime);
                if (succeeded)
                    return parsedDateTime;
                return null;
            }
            set
            {
                if (value == null) return;
                tbDateSelection.Text = value.Value.ToString("dd-MM-yyyy");
                tbTimeSelection.Text = value.Value.ToString("h:mm tt");
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ClearFields()
        {
            tbDateSelection.Text = string.Empty;
            tbTimeSelection.Text = string.Empty;
        }
    }
}