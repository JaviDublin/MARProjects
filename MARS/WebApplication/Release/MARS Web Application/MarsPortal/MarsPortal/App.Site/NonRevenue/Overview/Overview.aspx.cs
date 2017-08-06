using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;


namespace Mars.App.Site.NonRevenue.Overview
{
    public partial class Overview : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ucOverviewGrid.GridData = null;
                using (var dataAccess = new AvailabilityDataAccess(null))
                {
                    lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                }
            }            
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
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
                    
                    ucOverviewVehicleHistory.SetVehicleHistoryDetails(vehicleId);
                    
                    ucOverviewVehicle.SetVehicleDetails(vehicleId);    
                    
                    upnlMultiview.Update();
                    mpeNonRevOverview.Show();
                    
                }
                handled = true;
            }
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == "KeepPopupOpen")
                {
                    mpeNonRevOverview.Show();
                    handled = true;
                }

                if (commandArgs.CommandName == "KeepMultipleEntryPopupOpen")
                {
                    ucOverviewGrid.ShowMultiReasonEntryPopup();
                    handled = true;
                }
                if (commandArgs.CommandName == "RefreshGrid")
                {
                    RefreshOverviewGrid();
                    handled = true;
                }
                
            }


            return handled;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshOverviewGrid();
        }

        private void RefreshOverviewGrid()
        {
            var mergedParameters = nrParams.GetParameterDictionary();

            using (var dataAccess = new AvailabilityDataAccess(null))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
            }
            upnlUpdatedTime.Update();

            ucOverviewGrid.LoadGrid(mergedParameters);
 
        }



    }
}