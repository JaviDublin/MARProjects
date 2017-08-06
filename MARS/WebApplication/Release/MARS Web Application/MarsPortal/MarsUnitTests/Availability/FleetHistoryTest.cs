using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using App.BLL;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests.Availability
{
    [TestClass]
    public class FleetHistoryTest
    {
        private DateTime FromDate = (new DateTime(2014, 08, 07));
        private DateTime ToDate = (new DateTime(2014, 08, 12));

        private Dictionary<DictionaryParameter, string> GetDayParameters()
        {
            var returned = new Dictionary<DictionaryParameter, string>
                           {
                               {DictionaryParameter.OwningCountry, "GE"}, 
                               {DictionaryParameter.StartDate, FromDate.ToShortDateString()}, 
                               {DictionaryParameter.EndDate, ToDate.ToShortDateString()},
                               

                           };
            
            returned[DictionaryParameter.FleetTypes] = "4";
            
             // 4 RAC OPS, 5 RAC NON OPS, 6 CARSALES, 7 Licensee, 8 Firefly, 9 Hertz on Demand
            
            return returned;
        }

        private DataTable GetOldDayData()
        {
            var fleetTypes = "RAC OPS";
            // CARSALES, RAC TTL, RAC OPS, HERTZ ON DEMAND (Firefly), LICENSEE
            var dataTable = AvailabilityHistoricalTrend.GetHistoricalTrendData("GE", -1, -1, -1, -1, "-1", fleetTypes, -1, -1, -1,
                    FromDate, ToDate, -1, "DAY", "NUMERIC", "-1");
            return dataTable;
        }
            
        [TestMethod]
        public void TestDays()
        {
            List<FleetStatusRow> fleetData;
            using (var dataAccess = new HistoricalTrendDataAccess(GetDayParameters()))
            {
                
                fleetData = dataAccess.GetHistoricalTrend();
            }

            var dt = GetOldDayData();

            fleetData = fleetData.OrderBy(d => d.Day).ToList();

            Assert.AreNotEqual(dt.Rows.Count, 0, "No Data from old system");
            
            foreach (DataRow row in dt.Rows)
            {
                var repDateOld = DateTime.Parse(row["REP_DATE"].ToString());

                var newData = fleetData.FirstOrDefault(d => d.Day == repDateOld);

                if(newData == null) continue;

                var tfOld = int.Parse(row["TOTAL_FLEET"].ToString());
                var tfNew = newData.TotalFleet;

                Assert.AreEqual(tfOld, tfNew);

                var orOld = int.Parse(row["ON_RENT"].ToString());
                var orNew = newData.OnRent;

                Assert.AreEqual(orOld, orNew);

                var idleOld = int.Parse(row["RT"].ToString());
                var idleNew = newData.Idle;

                Assert.AreEqual(idleOld, idleNew);


                
            }


            Assert.IsTrue(true);

        }
    }
}
