using System;
using System.Collections.Generic;
using Mars.App.Classes.Phase4Dal.Administration.Membership;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewMarsUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            
            //parameters[DictionaryParameter.LocationCountry] = "GE";
            parameters[DictionaryParameter.StartDate] = new DateTime(2014, 09, 25).ToShortDateString();
            using (var dataAccess = new AgeingDataAccess(parameters))
            {
                var d = dataAccess.GetAgeingEntries();
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            //parameters[DictionaryParameter.LocationCountry] = "GE";
            parameters[DictionaryParameter.StartDate] = new DateTime(2014, 09, 20).ToShortDateString();
            parameters[DictionaryParameter.EndDate] = new DateTime(2014, 09, 25).ToShortDateString();
            using (var dataAccess = new HistoricalTrendDataAccess(parameters))
            {
                var d = dataAccess.GetHistoricAgeRowData();
            }
            Assert.IsTrue(true);
        }
    }
}
