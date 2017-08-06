using System;
using System.Collections.Generic;
using Mars.App.Classes.Phase4Dal.Administration.Membership;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests.DavidTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            var parameters = new Dictionary<DictionaryParameter, string>();
            //parameters[DictionaryParameter.LocationCountry] = "GE";
            parameters[DictionaryParameter.StartDate] = new DateTime(2013, 09, 25).ToString();
            using (var dataAccess = new AgeingDataAccess(parameters))
            {
                var d = dataAccess.GetAgeingEntries();
            }
            Assert.IsTrue(DateTime.Now.Year == 2014);
        }
    }
}
