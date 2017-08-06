using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Availability.Entities;

using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Site.Availability.HistoricalTrend
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

            var startDate = mergedParameters.GetDateFromDictionary(DictionaryParameter.StartDate);
            var endDate = mergedParameters.GetDateFromDictionary(DictionaryParameter.EndDate);
            var todaysData = startDate.Date == DateTime.Now.Date && endDate == DateTime.Now.Date;
            ucHistoricalTrendChart.HourlySeries = todaysData;
            ucHistoricalTrendGrid.HourlySeries = todaysData;
            using (var dataAccess = new HistoricalTrendDataAccess(mergedParameters))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                fleetData = todaysData ? dataAccess.GetCurrentTrend() : dataAccess.GetHistoricalTrend();
            }
            upnlUpdatedTime.Update();
            ucHistoricalTrendChart.LoadChart(fleetData, type);
            ucHistoricalTrendGrid.LoadGrid(fleetData, type);

        }
    }
}