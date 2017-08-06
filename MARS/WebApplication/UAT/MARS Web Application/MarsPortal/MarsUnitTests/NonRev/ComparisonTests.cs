using System;
using System.Collections.Generic;
using System.Linq;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests.NonRev
{
    [TestClass]
    public class ComparisonTests
    {
        [TestMethod]
        public void CompareAllHistoryReports()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            parameters[DictionaryParameter.LocationCountry] = "GE";
            
            parameters[DictionaryParameter.StartDate] = DateTime.Now.AddDays(0).ToShortDateString();
            parameters[DictionaryParameter.KciGrouping] = "True";

            List<ComparisonRow> ageingData;
            List<ComparisonRow> siteCompData;
            List<ComparisonRow> fleetCompData;
            using (var dc = new ComparisonDataAccess(parameters))
            {
                fleetCompData = dc.GetComparisonEntries(false);
                siteCompData = dc.GetComparisonEntries(true);
                ageingData = dc.GetComparisonByStatusEntries();
            }
            


            var totalAgeing = ageingData.First(d => d.Key == NonRevBaseDataAccess.TotalKeyName).FleetCount;
            var totalSiteComp = siteCompData.First(d => d.Key == NonRevBaseDataAccess.TotalKeyName).FleetCount;
            var totalFleetComp = fleetCompData.First(d => d.Key == NonRevBaseDataAccess.TotalKeyName).FleetCount;


            Assert.IsTrue(totalAgeing == totalSiteComp);
            Assert.IsTrue(totalAgeing == totalFleetComp);
        }


        [TestMethod]
        public void CheckHistoricAgeRowData()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            parameters[DictionaryParameter.LocationCountry] = "GE";

            parameters[DictionaryParameter.StartDate] = DateTime.Now.AddDays(-4).ToShortDateString();
            parameters[DictionaryParameter.EndDate] = DateTime.Now.AddDays(-3).ToShortDateString();
            parameters[DictionaryParameter.KciGrouping] = "True";

            using (var dc = new HistoricalTrendDataAccess(parameters))
            {
                var historicalData = dc.GetHistoricalTrendEntries();
            }

        }

        [TestMethod]
        public void CheckHistoricalTrend()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            parameters[DictionaryParameter.LocationCountry] = "GE";

            parameters[DictionaryParameter.EndDate] = DateTime.Now.AddDays(0).ToShortDateString();
            parameters[DictionaryParameter.KciGrouping] = "True";

            using (var dc = new AgeingDataAccess(parameters))
            {
                //var ageingData = dc.GetAgeingEntries(DictionaryParameter.KciGrouping);
            }

        }

        [TestMethod]
        public void CheckNonRevComparison()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            parameters[DictionaryParameter.LocationCountry] = "GE";
            parameters[DictionaryParameter.StartDate] = DateTime.Now.AddDays(-4).ToShortDateString();
            parameters[DictionaryParameter.EndDate] = DateTime.Now.ToShortDateString();
            parameters[DictionaryParameter.DayOfWeek] = DayOfWeek.Friday.ToString();
            using (var dc = new ComparisonDataAccess(parameters))
            {
                var data = dc.GetComparisonEntries();
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CheckNonRevAging()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            parameters[DictionaryParameter.LocationCountry] = "GE";
            //parameters[DictionaryParameter.StartDate] = DateTime.Now.AddDays(-4).ToShortDateString();
            parameters[DictionaryParameter.EndDate] = DateTime.Now.AddDays(-1).ToShortDateString();
            parameters[DictionaryParameter.KciGrouping] = "True";
            //parameters[DictionaryParameter.DayOfWeek] = DayOfWeek.Friday.ToString();
            using (var dc = new ComparisonDataAccess(parameters))
            {
                var data = dc.GetComparisonByStatusEntries();
            }
            Assert.IsTrue(true);
        }
    }
}
