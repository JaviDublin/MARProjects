using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;
using Mars.App.Classes.Phase4Dal.Reservations;

namespace Mars.App.Site.ForeignVehicles
{
    public partial class FleetMatch : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                using (var dataAccess = new ReservationDataAccess(null))
                {
                    lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                    lblReservationUpdate.Text = dataAccess.GetLastGwdRequest();
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ucExportToExcel.Visible = pnlOverviewGrid.Visible;
        }
        

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            var mergedParameters = ucParameters.GetParameterDictionary();
            List<VehicleMatchGridRow> data;
            using (var dataAccess = new MatchDataAccess(mergedParameters))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                lblReservationUpdate.Text = dataAccess.GetLastGwdRequest();

                data = dataAccess.GetVehicleMatches();
            }
            ucFleetMatchGrid.GridData = data;
            ucReservationMatchGrid.GridData = null;
            pnlOverviewGrid.Visible = true;
            ucExportToExcel.Visible = true;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is CommandEventArgs)
            {
                var commandEventArgs = args as CommandEventArgs;
                if (commandEventArgs.CommandName == UserControls.Phase4.HelpIcons.ExportToExcel.ExportString)
                {
                    ExportToExcel();
                    handled = true;
                }
            }
            if (args is GridViewCommandEventArgs)
            {
                var commandArgs = args as GridViewCommandEventArgs;
                if (commandArgs.CommandName.StartsWith("ShowReservations"))
                {
                    var vehicleId = int.Parse(commandArgs.CommandArgument.ToString());
                    List<ReservationMatchGridRow> data;
                    using (var dataAccess = new MatchDataAccess(null))
                    {
                        data = dataAccess.GetReservationMatches(vehicleId);
                    }
                    ucReservationMatchGrid.GridData = data;
                    upnlMultiview.Update();
                }
                handled = true;
            }
            return handled;
        }



        private void ExportToExcel()
        {
            var mergedParameters = ucParameters.GetParameterDictionary();
            string csvString;
            using (var dataAccess = new MatchDataAccess(mergedParameters))
            {
                csvString = dataAccess.GetFleetMatchExcelExport();
            }



            Session["ExportData"] = csvString;
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("Fleet Match {0}", DateTime.Now);
        }
    }
}