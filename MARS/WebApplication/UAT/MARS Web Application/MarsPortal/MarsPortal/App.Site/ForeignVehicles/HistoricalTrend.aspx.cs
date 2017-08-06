using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;
using HistoricalTrendDataAccess = Mars.App.Classes.Phase4Dal.ForeignVehicles.HistoricalTrendDataAccess;

namespace Mars.App.Site.ForeignVehicles
{
    public partial class HistoricalTrend : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var dataAccess = new AvailabilityDataAccess(null))
                {
                    lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            var mergedParameters = ucParameters.GetParameterDictionary();

            List<HistoricalTrendRow> historicalTrendData;
            using (var dataAccess = new HistoricalTrendDataAccess(mergedParameters))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                historicalTrendData = dataAccess.GetHistoricAgeRowData();
            }
            ucHistoricalTrendGrid.LoadGrid(historicalTrendData);
        }
    }
}