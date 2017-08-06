using System;
using System.Web.UI.WebControls;
// added

namespace MarsV2.VehiclesAbroad {
    public partial class FleetReservationMatch : System.Web.UI.Page {
        // ..

        protected void Page_Load(object sender, EventArgs e) {
            // Page load event handler apparently

            // not postback make the vehhicle predicament be Idle Vehicles
            if (!IsPostBack) {

                DropDownListVehiclePredicament.SelectedIndex = 3;
                DropDownListVehiclePredicament.Enabled = false;

                // set the start date to todays short date and the end date to two days (effectively three) later
                TextBoxStartDate.Text = DateTime.Now.ToShortDateString();
                TextBoxEndDate.Text = DateTime.Now.AddDays(2).ToShortDateString();
            }
        }

        protected void GridViewFea_DataBound(object sender, EventArgs e) {
            // Event handler that enforces Matching Reservations databinding
            GridView2.DataBind();
        }

        protected void DropDownListPagerMaxRows_SelectedIndexChanged(object sender, EventArgs e) {
            // used to load the amount of records shown
            GridViewFea.PageSize = Convert.ToInt32(DropDownListPagerMaxRows.SelectedValue);
        }

        protected void DropDownListReservationMatch_SelectedIndexChanged(object sender, EventArgs e) {
            // used to load different number of rows from the dropdownlist
            GridView2.PageSize = Convert.ToInt32(DropDownListReservationMatch.SelectedValue);
        }

        protected void TextBoxStartDate_TextChanged(object sender, EventArgs e) {

            // check the date isn't before today and is a valid date
            DateTime dt = new DateTime();

            if (!DateTime.TryParse(TextBoxStartDate.Text, out dt)) {

                // set label to error message and ensure the date is set todays date
                LabelStartDate.Text = "Not a valid date format.";
                TextBoxStartDate.Text = DateTime.Now.ToShortDateString();
            }
            else if (dt < DateTime.Now.Date) {

                // set label to error message and ensure the date is set todays date
                LabelStartDate.Text = "Choose a date today or in the future.";
                TextBoxStartDate.Text = DateTime.Now.ToShortDateString();
            }
            else {

                // clear the error message on the label
                LabelStartDate.Text = "";

                //if the end date is behind then move the end date on by a few days
                DateTime endDate = Convert.ToDateTime(TextBoxEndDate.Text);
                if (endDate <= dt) {
                    TextBoxEndDate.Text = dt.AddDays(2).ToShortDateString();
                }
            }
        }

        protected void TextBoxEndDate_TextChanged(object sender, EventArgs e) {

            // check the chosen date isn't invalid and before the start date
            DateTime dt = new DateTime();

            // get the start date in-case of an error - can assume that its correct
            DateTime startDate = DateTime.Parse(TextBoxStartDate.Text);

            if (!DateTime.TryParse(TextBoxEndDate.Text, out dt)) { // check is valid format

                // set the error feedback label to appropriate error and set the end date to one day more than the start date
                LabelEndDate.Text = "Not a valid date format.";
                TextBoxEndDate.Text = startDate.AddDays(2).ToShortDateString();
            }
            else if (dt < startDate.AddDays(1)) { // check that the end date is one more day than the start date

                LabelEndDate.Text = "Choose a date on or after the start date.";
                TextBoxEndDate.Text = startDate.AddDays(2).ToShortDateString();
            }
            else { // date is great

                LabelEndDate.Text = ""; // clear any error messages
            }
        }

        protected void GridViewFea_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e) {
            // check its of type datarow
            if (e.Row.RowType == DataControlRowType.DataRow) {

                e.Row.Cells[9].Font.Bold = true;
                e.Row.Cells[9].ForeColor = System.Drawing.Color.Green;
                if (e.Row.Cells[9].Text == "0")
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;

            }
        }
    }
}