using System;
using System.Collections.Generic;
using System.Linq;
using App.MasterPages;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests.Availability
{
    [TestClass]
    public class ReportsTest
    {
        [TestMethod]
        public void CheckFigures()
        {
            //Arrange
            var parameters = new Dictionary<DictionaryParameter, string>
                           {
                                 {DictionaryParameter.OwningCountry,"GE"}
                               , {DictionaryParameter.LocationCountry, "GE"}
                               , {DictionaryParameter.Pool, "71"}
                               , {DictionaryParameter.LocationGroup, string.Empty}
                               , {DictionaryParameter.Region, string.Empty}
                               , {DictionaryParameter.Area, string.Empty}
                               , {DictionaryParameter.Location, string.Empty}
                               , {DictionaryParameter.CarSegment, string.Empty}
                               , {DictionaryParameter.CarClass, string.Empty}
                               , {DictionaryParameter.CarGroup, string.Empty}
                               , {DictionaryParameter.CmsSelected, true.ToString()}
                           };
            var daySelected = DateTime.Now.ToShortDateString();
            parameters.Add(DictionaryParameter.StartDate, daySelected);
            parameters.Add(DictionaryParameter.EndDate, daySelected);
            parameters.Add(DictionaryParameter.FleetTypes, "4,5,6");

            FleetStatusRow fleetData;
            List<FleetStatusRow> siteComparisonData;
            List<FleetStatusRow> fleetComparisonData;

            List<FleetStatusRow> historicalTrendData;

            
            //Act

            using (var dataAccess = new FleetStatusDataAccess(parameters))
            {
                fleetData = dataAccess.GetFleetStatus();
            }

            using (var dataAccess = new ComparisonDataAccess(parameters))
            {
                siteComparisonData = dataAccess.GetComparisonData(true);
                fleetComparisonData = dataAccess.GetComparisonData(false);
            }

            using (var dataAccess = new HistoricalTrendDataAccess(parameters))
            {
                historicalTrendData = dataAccess.GetCurrentTrend();
            }


            //Assert
            var totalSite = siteComparisonData.Sum(s => s.TotalFleet);
            var totalFleet = fleetComparisonData.Sum(s => s.TotalFleet);
            var totalTrendFleet = historicalTrendData.Last().TotalFleet;
            Assert.IsTrue(fleetData.TotalFleet == totalSite);
            Assert.IsTrue(fleetData.TotalFleet == totalFleet);
            Assert.IsTrue(fleetData.TotalFleet == totalTrendFleet);


        }
    }
}
