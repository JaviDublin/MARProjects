using System;
using System.Web.UI.WebControls;

namespace MarsV2.VehiclesAbroad {

    public partial class ReservationFleetMatch : System.Web.UI.Page {

        protected void Page_Load(object sender, EventArgs e) {

            if (!IsPostBack) {

                // set the start date to todays short date and the end date to two (effectively three) day later
                TextBoxReservationStartDate.Text = DateTime.Now.ToShortDateString();
                TextBoxReservationEndDate.Text = DateTime.Now.AddDays(2).ToShortDateString();
            }
        }

        protected void DropDownListPagerMaxRows_SelectedIndexChanged(object sender, EventArgs e) {
            // used to load the amount of records shown
            GridViewReservation.PageSize = Convert.ToInt32(DropDownListPagerMaxRows.SelectedValue);
        }

        protected void DropDownListFleetMax_SelectedIndexChanged(object sender, EventArgs e) {
            GridViewMatches.PageSize = Convert.ToInt32(DropDownListFleetMax.SelectedValue);
        }

        protected void TextBoxReservationStartDate_TextChanged(object sender, EventArgs e) {

            // check the date isn't before today and is a valid date
            DateTime dt = new DateTime();

            if (!DateTime.TryParse(TextBoxReservationStartDate.Text, out dt)) {

                // set label to error message and ensure the date is set todays date
                LabelReservsationStartDate.Text = "<br />Not a valid date format.";
                TextBoxReservationStartDate.Text = DateTime.Now.ToShortDateString();
            }
            else if (dt < DateTime.Now.Date) {

                // set label to error message and ensure the date is set todays date
                LabelReservsationStartDate.Text = "<br />Choose a date today or in the future.";
                TextBoxReservationStartDate.Text = DateTime.Now.ToShortDateString();
            }
            else {

                // clear the error message on the label
                LabelReservsationStartDate.Text = "";

                //if the end date is behind then move the end date on by few days
                DateTime endDate = Convert.ToDateTime(TextBoxReservationEndDate.Text);
                if (endDate <= dt) {
                    TextBoxReservationEndDate.Text = dt.AddDays(2).ToShortDateString();
                }
            }
        }

        protected void TextBoxReservationEndDate_TextChanged(object sender, EventArgs e) {

            // check the chosen date isn't invalid and before the start date
            DateTime dt = new DateTime();

            // get the start date in-case of an error - can assume that its correct
            DateTime startDate = DateTime.Parse(TextBoxReservationStartDate.Text);

            if (!DateTime.TryParse(TextBoxReservationEndDate.Text, out dt)) { // check is valid format

                // set the error feedback label to appropriate error and set the end date to one day more than the start date
                LabelReservationEndDate.Text = "<br />Not a valid date format.";
                TextBoxReservationEndDate.Text = startDate.AddDays(2).ToShortDateString();
            }
            else if (dt < startDate) { // check that the end date is one more day than the start date

                LabelReservationEndDate.Text = "<br />Choose a date on or after the start date.";
                TextBoxReservationEndDate.Text = startDate.AddDays(2).ToShortDateString();
            }
            else { // date is great

                LabelReservationEndDate.Text = ""; // clear any error messages
            }
        }
        protected void GridViewReservation_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e) {
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