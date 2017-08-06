using System;
using System.Web.UI.WebControls;
using App.Entities.VehiclesAbroad;
using System.Collections.Generic;
using App.BLL.VehiclesAbroad.Abstract;
using App.BLL.VehiclesAbroad.Concrete;
using App.BLL.Utilities;
using App.BLL;

namespace App.VehiclesAbroad {

    public partial class RerservationDetails : System.Web.UI.Page {

        // the filter entity used to load the filters
        FilterEntity _filterEntity {
            get {
                if (Session["ReservationOverviewfilters"] == null) Session["ReservationOverviewfilters"] = new FilterEntity();
                return (FilterEntity)Session["ReservationOverviewfilters"];
            }
        }
        protected void Page_Load(object sender, System.EventArgs e) {

            App.Classes.BLL.Workers.VehiclesAbroadWorker.addCSS(this.Page);
            VehiclesAbroadReservations1.ReservationRepository = new ReservationDetailsRepository();

            if (!IsPostBack) {
                TextBoxReservationStartDate.Text = DateTime.Now.ToShortDateString();
                TextBoxReservationEndDate.Text = DateTime.Now.AddDays(2).ToShortDateString();

                if (_filterEntity.nonRev == 99) {

                    DropDownListDueCountry.SelectedValue = (string)Session["ReservationOverviewDueCountry"] ?? "";
                    DropDownListOwnCountry.SelectedValue = (string)Session["ReservationOverviewOwnCountry"] ?? "";
                    TextBoxReservationStartDate.Text = _filterEntity.ReservationStartDate.ToShortDateString();
                    TextBoxReservationEndDate.Text = _filterEntity.ReservationEndDate.ToShortDateString();
                    labelStartCountry.Text = (string)Session["ReservationOverviewOwnCountry"] ?? "";
                    labelRtrnCountry.Text = (string)Session["ReservationOverviewDueCountry"] ?? "";
                }
                labelEndDate.Text = TextBoxReservationEndDate.Text;
                labelStartDate.Text = TextBoxReservationStartDate.Text;
            }
            else {
                _filterEntity.nonRev = 0;
                setFeedback();
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

        protected void ObjectDataSourceReservationDetails_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
            // when the page is not a postback and arrived from the ReservationOverview then the ObjectDataSource
            // has to be set to the values from the filters, which is stored in session variables

            // check if this page was from clicking on the grid from the reservation overview  
            if (_filterEntity.nonRev == 99) {

                e.InputParameters["dueCountry"] = (string)Session["ReservationOverviewDueCountry"] ?? "";
                e.InputParameters["ownCountry"] = (string)Session["ReservationOverviewOwnCountry"] ?? "";
                e.InputParameters["pool"] = _filterEntity.Pool;
                e.InputParameters["locationGroup"] = _filterEntity.Location;
                e.InputParameters["carSegment"] = _filterEntity.CarSegment;
                e.InputParameters["carClass"] = _filterEntity.CarClass;
                e.InputParameters["carGroup"] = _filterEntity.CarGroup;

            }
        }

        protected void DropDownListPagerMaxRows_SelectedIndexChanged(object sender, System.EventArgs e) {
            // used to load the amount of records shown
            GridViewReservationDetails.PageSize = Convert.ToInt32(DropDownListPagerMaxRows.SelectedValue);
        }

        protected void TextBoxReservationStartDate_TextChanged(object sender, System.EventArgs e) {

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

        protected void TextBoxReservationEndDate_TextChanged(object sender, System.EventArgs e) {

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

        // ============
        // All the databound event handlers are to put the filters to the correct value

        protected void DropDownListDestinationPool_DataBound(object sender, System.EventArgs e) {

            if (_filterEntity.nonRev == 99) {
                DropDownListDestinationPool.SelectedValue = _filterEntity.DuePool;
                labelRtrnPool.Text = DropDownListDestinationPool.SelectedValue;
            }
        }

        protected void DropDownListDestinationLocationGroup_DataBound(object sender, System.EventArgs e) {

            if (_filterEntity.nonRev == 99) {
                DropDownListDestinationLocationGroup.SelectedValue = _filterEntity.DueLocationGroup;
                labelRtrnLocationGrp.Text = DropDownListDestinationLocationGroup.SelectedValue;
            }
        }

        protected void DropDownListPool_DataBound(object sender, System.EventArgs e) {

            if (_filterEntity.nonRev == 99) {
                DropDownListPool.SelectedValue = _filterEntity.Pool;
                labelStartPool.Text = DropDownListPool.SelectedValue;
            }

        }

        protected void DropDownListLocationGroup_DataBound(object sender, System.EventArgs e) {

            if (_filterEntity.nonRev == 99) {
                DropDownListLocationGroup.SelectedValue = _filterEntity.Location;
                labelStartLocGrp.Text = DropDownListLocationGroup.SelectedValue;
            }
        }

        protected void DropDownListCarSegment_DataBound(object sender, System.EventArgs e) {

            if (_filterEntity.nonRev == 99) {
                DropDownListCarSegment.SelectedValue = _filterEntity.CarSegment;
                labelCarSegment.Text = DropDownListCarSegment.SelectedValue;
            }
        }

        protected void DropDownListCarClass_DataBound(object sender, System.EventArgs e) {

            if (_filterEntity.nonRev == 99) {
                DropDownListCarClass.SelectedValue = _filterEntity.CarClass;
                labelCarClass.Text = DropDownListCarClass.SelectedValue;
            }
        }

        protected void DropDownListCarGroup_DataBound(object sender, System.EventArgs e) {

            if (_filterEntity.nonRev == 99) {
                DropDownListCarGroup.SelectedValue = _filterEntity.CarGroup;
                labelCarGroup.Text = DropDownListCarGroup.SelectedValue;
            }
        }
        // ============

        protected void buttonSave_Click(object sender, System.EventArgs e) {

            ICSVDownload<ReservationMatchEntity> csv = new SaveCSVReservationDetails(new CSVGenerator());// can change the CSVGenerator here

            // get the current model
            csv.csvList = (IEnumerable<ReservationMatchEntity>)ObjectDataSourceReservationDetails.Select();

            //Set directory variable
            string tempDirectory = Mars.Properties.Settings.Default.TempDirectory;

            //Clear Files from directory
            Helper.DeleteTempFiles(tempDirectory);

            if (csv.downloadFile(Server.MapPath(tempDirectory), "Reservations")) {

                // direct to the download file
                App.Classes.BLL.Workers.VehiclesAbroadWorker.downloadFile(csv.FileName, tempDirectory, this);
            }
        }

        protected void GridViewReservationDetails_SelectedIndexChanged(object sender, System.EventArgs e) {

            string resNumber = GridViewReservationDetails.SelectedRow.Cells[5].Text;
            VehiclesAbroadReservations1.setTable(resNumber);
        }
    }
}