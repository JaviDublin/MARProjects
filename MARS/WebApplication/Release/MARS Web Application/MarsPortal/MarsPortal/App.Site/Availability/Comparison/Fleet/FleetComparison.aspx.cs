using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Site.Availability.Comparison.Fleet
{
    public partial class FleetComparison : Page
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
            RefreshData();
        }

        private void RefreshData()
        {
            List<FleetStatusRow> fleetData;
            var mergedParameters = generalParams.GetParameterDictionary();

            var type = PercentageDivisorType.Values;
            if (mergedParameters.ContainsKey(DictionaryParameter.PercentageCalculation)
                && mergedParameters[DictionaryParameter.PercentageCalculation] != string.Empty)
            {
                type = (PercentageDivisorType)Enum.Parse(typeof(PercentageDivisorType), mergedParameters[DictionaryParameter.PercentageCalculation]);

            }
            using (var dataAccess = new ComparisonDataAccess(mergedParameters))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                fleetData = dataAccess.GetComparisonData(false);
            }

            upnlUpdatedTime.Update();
            fleetData = fleetData.OrderBy(d => d.Key).ToList();

            ucComparisonChart.LoadChart(fleetData, type);
            ucComparisonTrendGrid.LoadGrid(fleetData, type);

        }
    }
}