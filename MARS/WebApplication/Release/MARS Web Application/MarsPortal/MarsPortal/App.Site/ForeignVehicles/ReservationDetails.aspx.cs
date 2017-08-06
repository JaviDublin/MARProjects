using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.Reservations;
using Mars.App.Classes.Phase4Dal.Reservations.Entities;

namespace Mars.App.Site.ForeignVehicles
{
    public partial class ReservationDetails : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                    using (var dataAccess = new ReservationDataAccess(null))
                    {
                        lblLastUpdate.Text = dataAccess.GetLastGwdRequest();
                    }
                }
            }
            Page.LoadComplete += Page_LoadComplete;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session[ReservationOverview.ReservationOverviewSessionTransferName] == null) return;
            RefreshGrid();
            Session[ReservationOverview.ReservationOverviewSessionTransferName] = null;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            List<ReservationData> data;
            var mergedParameters = ucParameters.GetParameterDictionary();

            using (var dataAccess = new ReservationDataAccess(mergedParameters))
            {
                lblLastUpdate.Text = dataAccess.GetLastGwdRequest();
                data = dataAccess.GetReservations();
            }
            ucReservationsGrid.GridData = data;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;

            if (args is GridViewCommandEventArgs)
            {
                var commandArgs = args as GridViewCommandEventArgs;
                if (commandArgs.CommandName == "ShowReservation")
                {
                    var reservationId = int.Parse(commandArgs.CommandArgument.ToString());

                    var resData = ucReservationsGrid.GridData.FirstOrDefault(d => d.ReservationId == reservationId);
                    if (resData == null) return false;
                    ucReservationDetails.SetReservationDetails(resData);

                    upnlMultiview.Update();
                    mpeCarSearch.Show();
                }
                handled = true;
            }

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == "RefreshGrid")
                {
                    using (var dataAccess = new AvailabilityDataAccess(null))
                    {
                        lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                    }
                    RefreshGrid();
                    handled = true;
                }

            }
            return handled;
        }

    }
}