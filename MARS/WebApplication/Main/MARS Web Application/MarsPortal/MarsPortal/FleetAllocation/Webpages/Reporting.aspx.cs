using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.MarsDataAccess.Sizing;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BusinessLogic.DropdownLists;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess;
using Mars.FleetAllocation.DataAccess.Reporting.ForecastContribution;
using Mars.FleetAllocation.DataAccess.Reporting.ForecastedFleetSize;
using FleetComparisonDataAccess = Mars.FleetAllocation.DataAccess.Reporting.FleetComparison.FleetComparisonDataAccess;
using SiteComparisonDataAccess = Mars.FleetAllocation.DataAccess.Reporting.SiteComparison.SiteComparisonDataAccess;

namespace Mars.FleetAllocation.Webpages
{
    public partial class Reporting : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropdowns();
                using (var dataAccess = new AdditionPlanDataAccess())
                {
                    var countryId = int.Parse(ddlCountry.SelectedValue);
                    var additionPlansA = dataAccess.GetAdditionPlanListItems(countryId);
                    var additionPlansB = dataAccess.GetAdditionPlanListItems(countryId);
                    ddlAdditionPlanA.Items.AddRange(additionPlansA.ToArray());
                    ddlAdditionPlanB.Items.AddRange(additionPlansB.ToArray());
                    if (additionPlansB.Count > 1)
                    {
                        ddlAdditionPlanB.SelectedIndex = 1;
                    }
                }
            }
        }

        private void PopulateDropdowns()
        {
            for (var i = 2; i < 12; i++)
            {
                ddlWeeksSelection.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            ddlWeeksSelection.Items[4].Selected = true;

            ListGenerator.FillDropdownWithFaoCountries(ddlCountry);
        }

        protected void lbForecastedFleedTab_Click(object sender, EventArgs e)
        {
            var x = 0;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {

            var parameters = FaoParameter.GetParameters();

            int tabSelected = int.Parse(hfSelectedPanel.Value);

            switch (tabSelected)
            {
                case 0:
                    LoadForecastedFleet(parameters);
                    break;
                case 1:
                    LoadFleetComparison(parameters);
                    break;
                case 2:
                    LoadSiteComparison(parameters);
                    break;
                case 3:
                    LoadForecastContribution(parameters);
                    break;
            }

        }

        private void LoadForecastContribution(Dictionary<DictionaryParameter, string> parameters)
        {
            using (var dataAccess = new ForecastContributionDataAccess(parameters))
            {
                var lastTimeStamp = dataAccess.GetLastHistoryTimestamp();


                //Constant 12 weeks for history calculations
                parameters[DictionaryParameter.StartDate] = (lastTimeStamp.AddDays(-84)).ToShortDateString();
                parameters[DictionaryParameter.EndDate] = (lastTimeStamp).ToShortDateString();

                parameters[DictionaryParameter.ForecastStartDate] = lastTimeStamp.ToShortDateString();
                var weeksSelected = int.Parse(ddlWeeksSelection.SelectedValue);
                parameters[DictionaryParameter.ForecastEndDate] = lastTimeStamp.AddDays(weeksSelected * 7).ToShortDateString();

                var additionPlanASelected = int.Parse(ddlAdditionPlanA.SelectedValue);
                var additionPlanBSelected = int.Parse(ddlAdditionPlanB.SelectedValue);

                var contributionData = dataAccess.GetForecastContribution(additionPlanASelected, additionPlanBSelected);
                ucForecastContribution.SetContribution(contributionData);
            }
        }

        private void LoadFleetComparison(Dictionary<DictionaryParameter, string> parameters)
        {
            var weeksSelected = int.Parse(ddlWeeksSelection.SelectedValue);
            using (var dataAccess = new FleetComparisonDataAccess(parameters))
            {
                var data = dataAccess.GetComparisonEntities(int.Parse(ddlAdditionPlanA.SelectedValue), weeksSelected);
                ucFleetComparison.SetGridAndGraph(data);
            }
        }

        private void LoadSiteComparison(Dictionary<DictionaryParameter, string> parameters)
        {
            var weeksSelected = int.Parse(ddlWeeksSelection.SelectedValue);
            using (var dataAccess = new SiteComparisonDataAccess(parameters))
            {
                var data = dataAccess.GetComparisonEntities(int.Parse(ddlAdditionPlanA.SelectedValue), weeksSelected);
                ucSiteComparison.SetGridAndGraph(data);
            }
        }

        private void LoadForecastedFleet(Dictionary<DictionaryParameter, string> parameters)
        {
            using (var dataAccess = new ForecastedFleetSizeDataAccess(parameters))
            {
                var lastTimeStamp = dataAccess.GetLastHistoryTimestamp();
                ucForecastedFleetSize.ValuesChart = ddlForecastTypes.SelectedValue == string.Empty;

                parameters[DictionaryParameter.ForecastStartDate] = lastTimeStamp.ToShortDateString();
                var weeksSelected = int.Parse(ddlWeeksSelection.SelectedValue);
                parameters[DictionaryParameter.ForecastEndDate] = lastTimeStamp.AddDays(weeksSelected * 7).ToShortDateString();

                var additionPlanASelected = int.Parse(ddlAdditionPlanA.SelectedValue);
                var additionPlanBSelected = int.Parse(ddlAdditionPlanB.SelectedValue);

                var weeklyForecast = dataAccess.GetWeeklyForecast();

                var dataForA = dataAccess.AttachAdditionPlans(weeklyForecast, additionPlanASelected);
                var dataForB = dataAccess.AttachAdditionPlans(weeklyForecast, additionPlanBSelected);

                ucForecastedFleetSize.SetGridAndGraph(dataForA, dataForB);

            }
        }
    }
}