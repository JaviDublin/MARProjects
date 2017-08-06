using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;

namespace Mars.App.Site.ForeignVehicles
{
    public partial class AgeingReport : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var dataAccess = new AvailabilityDataAccess(null))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            var mergedParameters = ucParameters.GetParameterDictionary();
            
            List<AgeingRow> ageingData;
            using (var dataAccess = new AgeingDataAccess(mergedParameters))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                ageingData = dataAccess.GetAgeingEntries();
            }
            ucAgeingGrid.LoadGrid(ageingData);
        }
    }
}