using System;
using System.Web.UI.WebControls;
using App.BLL.VehiclesAbroad; // added
using App.Entities.VehiclesAbroad;
using System.Web.Routing; // added

namespace MarsV2.VehiclesAbroad {

    public partial class ReservationOverview : System.Web.UI.Page {

        // the filter entity used to load the filters
        FilterEntity _filterEntity {
            set {
                Session["ReservationOverviewfilters"] = value;
            }
            get {
                return (FilterEntity)Session["ReservationOverviewfilters"];
            }
        }

        protected void Page_Load(object sender, EventArgs e) {

            // add the availability CSS for a similar look and feel
            App.Classes.BLL.Workers.VehiclesAbroadWorker.addCSS(this.Page);

            if (!IsPostBack) {

                // set the start date to todays short date and the end date to two (effectively three) day later
                TextBoxReservationStartDate.Text = DateTime.Now.ToShortDateString();
                TextBoxReservationEndDate.Text = DateTime.Now.AddDays(2).ToShortDateString();
            }
            else setFeedback();
            Session["ReservationOverviewfilters"] = getFilters();

            // if the postback is from a click from the data table then save session and redirect to the Vehicle details page
            string eventArgument = Request["__EVENTARGUMENT"] ?? ""; // the args are: due country, own country
            string eventTarget = Request["__EVENTTARGET"] ?? "";

            // Been called from clicking the data table
            if (eventTarget.Contains("DataTableOverview")) {
                Session["ReservationOverviewDueCountry"] = eventArgument.Split(',')[0];
                Session["ReservationOverviewOwnCountry"] = eventArgument.Split(',')[1];
                _filterEntity.nonRev = 99; // flag to indicate to the Reservation details that the page has come from here
                Response.Redirect("~/VehiclesAbroad/ReservationDetails"); // redirect to Vehicle details page
            }
            else {
                // Populate the data grid
                populateDataTable();
            }
        }
        void setFeedback() {
            labelRtrnCountry.Text = DropDownListDueCountry.SelectedValue;
            labelRtrnPool.Text = DropDownListDestinationPool.SelectedValue;
            labelRtrnLocationGrp.Text = DropDownListDestinationLocationGroup.SelectedValue;
            labelEndDate.Text = TextBoxReservationEndDate.Text;
            labelStartDate.Text = TextBoxReservationStartDate.Text;
            labelStartCountry.Text = DropDownListOwnCountry.Text;
            labelStartPool.Text = DropDownListPool.SelectedValue;
            labelStartLocGrp.Text = DropDownListLocationGroup.SelectedValue;
            labelCarClass.Text = DropDownListCarClass.SelectedValue;
            labelCarGroup.Text = DropDownListCarGroup.SelectedValue;
            labelCarSegment.Text = DropDownListCarSegment.SelectedValue;
        }
        private void populateDataTable() {
            // helper to populate the data table with the vehicle information

            // create an instance of the ReservationOverviewModel and get the code to generate the innerHTML for the div tag
            ReservationOverviewModel fom = new ReservationOverviewModel();

            // condition the filter to clean out ***All*** and nulls
            _filterEntity = fom.conditionFilters(DropDownListDueCountry.SelectedValue,
                                                    0,
                                                    DropDownListOwnCountry.SelectedValue,
                                                    DropDownListPool.SelectedValue,
                                                    DropDownListLocationGroup.SelectedValue,
                                                    DropDownListCarSegment.SelectedValue,
                                                    DropDownListCarClass.SelectedValue,
                                                    DropDownListCarGroup.SelectedValue,
                                                    DropDownListDestinationPool.SelectedValue,
                                                    DropDownListDestinationLocationGroup.SelectedValue);

            // potentially volatile
            try {
                // set the filter with the start and end dates
                _filterEntity.ReservationStartDate = Convert.ToDateTime(TextBoxReservationStartDate.Text);
                _filterEntity.ReservationEndDate = Convert.ToDateTime(TextBoxReservationEndDate.Text);
            }
            catch {
                // do nothing
            }

            DataTableReservationOverview.InnerHtml = fom.getReservationOverview(_filterEntity);
        }

        private FilterEntity getFilters() {
            // returns a new version of the current filters state
            // note: not used for the populateDataTable method as there seems to be be some sort of referencing problem

            return new FilterEntity {
                DueCountry = DropDownListDueCountry.SelectedValue,
                VehiclePredicament = 0,
                OwnCountry = DropDownListOwnCountry.SelectedValue,
                Pool = DropDownListPool.SelectedValue,
                Location = DropDownListLocationGroup.SelectedValue,
                CarSegment = DropDownListCarSegment.SelectedValue,
                CarClass = DropDownListCarClass.SelectedValue,
                CarGroup = DropDownListCarGroup.SelectedValue,
                DuePool = DropDownListDestinationPool.SelectedValue,
                DueLocationGroup = DropDownListDestinationLocationGroup.SelectedValue,
                ReservationStartDate = Convert.ToDateTime(TextBoxReservationStartDate.Text),
                ReservationEndDate = Convert.ToDateTime(TextBoxReservationEndDate.Text)
            };
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
                LabelReservsationStartDate.Text = "<br />Choose a date today or in the future";
                TextBoxReservationStartDate.Text = DateTime.Now.ToShortDateString();
            }
            else {

                // clear the error message on the label
                LabelReservsationStartDate.Text = "";

                //if the end date is behind then move the end date on by a few days
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
                LabelReservationEndDate.Text = "<br />Not a valid date format";
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
    }
}