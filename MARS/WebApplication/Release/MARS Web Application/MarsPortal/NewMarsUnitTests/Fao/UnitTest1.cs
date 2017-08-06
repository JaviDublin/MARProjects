using System;
using System.Collections.Generic;
using System.Linq;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BusinessLogic;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.VehicleDistribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewMarsUnitTests.Fao
{
    [TestClass]
    public class FaoTest
    {
        [TestMethod]
        public void Test1()
        {
            var parameters = new Dictionary<DictionaryParameter, string>
                             {
                                 {DictionaryParameter.LocationCountry, "GE"},
                                 {DictionaryParameter.Pool, "71"},
                                 {DictionaryParameter.OwningCountry, "GE"}
                             };

            parameters[DictionaryParameter.StartDate] = (new DateTime(2015, 1, 4)).ToShortDateString();

            List<DemandGapOneRow> demandGapData;
            using (var dataAccess = new DemandGapDataAccess(parameters))
            {
                var data = dataAccess.GetDemandGapStepOne();
                demandGapData = data.ToList();

            }

            //List<MonthlyLimitRow> monthlyData;
            //List<WeeklyLimitRow> weeklyData;
            //using (var dataAccess = new VehicleDistributionDataAccess())
            //{
            //    monthlyData = dataAccess.GetMonthlyLimitRows(2);
            //    weeklyData = dataAccess.GetWeekLyLimitRows(3);
            //}
            //DemandGapCalculations.AssignGroupOne(demandGapData, monthlyData, weeklyData);
        }


        [TestMethod]
        public void Test2()
        {
            using (var dataAccess = new MaxFleetDataAccess())
            {
                dataAccess.GetMaxFleetSize();
            }
        }
    }
}
