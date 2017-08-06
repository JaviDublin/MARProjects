using System;
using System.Collections.Generic;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.Reservations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests.ForeignVehicles
{
    [TestClass]
    public class ForeignVehiclesTesting
    {



        [TestMethod]
        public void TestMethod3()
        {

            var parameters = new Dictionary<DictionaryParameter, string>();
            parameters[DictionaryParameter.LocationCountry] = "GE";
            using (var dataAccess = new MatchDataAccess(parameters))
            {
                var d = dataAccess.GetVehicleMatches();
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            using (var dataAccess = new VehicleOverviewDataAccess(null))
            {
                var data = dataAccess.GetForeignVehicleOverviewGrid();
                var dict = dataAccess.GetCountryDescriptionDictionary();
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            parameters[DictionaryParameter.ReservationCheckOutInDateLogic] = false.ToString();


            using (var dataAccess = new ReservationOverviewDataAccess(parameters))
            {
                var data = dataAccess.GetReservationOverviewGrid();
                var dict = dataAccess.GetCountryDescriptionDictionary();
            }
        }
    }
}
