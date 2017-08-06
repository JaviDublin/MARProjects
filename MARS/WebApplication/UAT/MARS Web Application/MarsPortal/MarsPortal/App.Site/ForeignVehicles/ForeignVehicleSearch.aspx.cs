using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.UserControls.Phase4.NonRev;

namespace Mars.App.Site.ForeignVehicles
{
    public partial class ForeignVehicleSearch : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(!IsPostBack)
            {
                using (var dataAccess = new AvailabilityDataAccess(null))
                {
                    lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                }    
            }
            
            
            Page.LoadComplete += Page_LoadComplete;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (Session[Availability.FleetStatus.FleetStatus.FleetStatusSessionParameters] != null)
            {
                var transferredParameters = (Dictionary<DictionaryParameter, string>)Session[Availability.FleetStatus.FleetStatus.FleetStatusSessionParameters];
                generalParams.TransferParameters(transferredParameters);

                Session[Availability.FleetStatus.FleetStatus.FleetStatusSessionParameters] = null;
                RefreshOverviewGrid();
            }

            if (Session[FleetOverview.FleetOverviewSessionTransferName] == null) return;
            RefreshOverviewGrid();
            Session[FleetOverview.FleetOverviewSessionTransferName] = null;
        }


        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;

            if (args is GridViewCommandEventArgs)
            {
                var commandArgs = args as GridViewCommandEventArgs;
                if (commandArgs.CommandName == "ShowVehicle")
                {
                    var vehicleId = int.Parse(commandArgs.CommandArgument.ToString());

                    using (var dataAccess = new OverviewDataAccess())
                    {
                        Session[OverviewVehicleHistory.OverviewVehicleHistoryDetails] = dataAccess.GetVehicleHistoryDetails(vehicleId);
                    }

                    ucOverviewVehicle.SetVehicleDetails(vehicleId);

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
                    RefreshOverviewGrid();
                }
                handled = true;
            }
            return handled;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshOverviewGrid();
        }

        private void RefreshOverviewGrid()
        {
            using (var dataAccess = new AvailabilityDataAccess(null))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
            }

            var mergedParameters = generalParams.GetParameterDictionary();
            ucOverviewGrid.LoadGrid(mergedParameters);

        }
    }
}