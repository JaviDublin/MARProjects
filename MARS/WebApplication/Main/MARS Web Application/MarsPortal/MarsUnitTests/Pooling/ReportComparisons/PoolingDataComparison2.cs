using System;
using System.Collections.Generic;
using System.Linq;
using App.Classes.DAL.Pooling.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.App.Classes.DAL.Pooling.SharedDataAccess;
using Mars.DAL.Pooling.Queryables;
using Mars.DAL.Reservations.Queryables;
using Mars.Entities.Pooling;
using MarsUnitTests.Pooling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests
{
    [TestClass]
    public class PoolingDataComparison2
    {
        private MainFilterEntity GetMainFilterExample()
        {
            var mfe = new MainFilterEntity { Country = "Germany", PoolRegion = "West", LocationGrpArea = "KOELN DT", CarSegment = "Car",
                                 CarClass = "Compact", CmsLogic = true};
            return mfe;
        }
        

        [TestMethod]
        public void TestCalculateTopics()
        {

            var mfe = GetMainFilterExample();
            var site = ReservationsDataAccess.CalculateTopics(true, 72, mfe, true, true);

            var fleet = ReservationsDataAccess.CalculateTopics(true, 72, mfe, true, false);
            var dayActuals = ReservationsDataAccess.CalculateTopics(true, 72, mfe, false);


            var ss = site.Where(d => d.Tme == 24);
            var ff = fleet.Where(d => d.Tme == 24);

            var ss2 = ss.Sum(d => d.Offset);
            var ff2 = ff.Sum(d => d.Offset);

            TestResultComparison.CheckAllPoolingValues(dayActuals, fleet, site);



        }

        [TestMethod]
        public void FeaDataAccess()
        {
            var feaQ = new FeaPoolingDataAccess();
            var feaFilterQ = new FeaFilteredQueryable();
            var mfe = GetMainFilterExample();

            List<DayActualEntity> site;
            List<DayActualEntity> fleet;
            List<DayActualEntity> dayActuals;
            using (var db = new MarsDBDataContext())
            {
                var checkOut = feaFilterQ.GetFeaCheckOut(db, mfe);

                var checkInData = feaFilterQ.GetFeaCheckIn(db, mfe);


                dayActuals = feaQ.GetFeaDataWithoutLabels(checkOut, checkInData, Enums.DayActualTime.THREE, mfe, db).ToList();

                site = feaQ.GetFeaDataWithLabels(checkOut, checkInData,
                    Enums.DayActualTime.THREE, true, mfe, db).ToList();

                fleet = feaQ.GetFeaDataWithLabels(checkOut, checkInData,
                    Enums.DayActualTime.THREE, false, mfe, db).ToList();
            }



            TestResultComparison.CheckAllPoolingValues(dayActuals, fleet, site);
        }

        [TestMethod]
        public void TestReservationCheckInDataAccess()
        {
            var mfe = GetMainFilterExample();
            var resCarFilterQ = new ReservationsFilterCar();
            var resFilterQ = new ReservationsSiteFilter();
            var reqCi = new ResActualCIQueryable();
            var resCiSiteQ = new ResSiteCIQueryable();
            var resCiFleetQ = new PoolingCheckInReservations();

            List<DayActualEntity> dayActuals;
            List<DayActualEntity> site;
            List<DayActualEntity> fleet;
            using (var db = new PoolingDataClassesDataContext())
            {
                var q2And3 = resCarFilterQ.FilterByCarParameters(db, mfe, true);
                q2And3 = resFilterQ.FilterByReturnLocation(q2And3, mfe);


                dayActuals = reqCi.GetQueryable(q2And3, mfe, Enums.DayActualTime.THREE);


                site = resCiSiteQ.GetQueryableCI(q2And3, mfe, Enums.DayActualTime.THREE, db);

                fleet = resCiFleetQ.GetFleetReservationsWithLabels(q2And3, mfe, Enums.DayActualTime.THREE, db);
            }



            TestResultComparison.CheckAllPoolingValues(dayActuals, fleet, site);
        }

        [TestMethod]
        public void TestReservationCheckOutDataAccess()
        {
            var mfe = GetMainFilterExample();
            var resCarFilterQ = new ReservationsFilterCar();
            var resFilterQ = new ReservationsSiteFilter();
            var reqCo = new PoolingCheckOutReservations();

            var resCOSiteQ = new ResSiteCOQueryable();
            var resCOFleetQ = new PoolingCheckOutReservationsWithLabels();

            List<DayActualEntity> dayActuals;
            List<DayActualEntity> fleet;
            List<DayActualEntity> site;

            using (var db = new PoolingDataClassesDataContext())
            {
                var q2And3 = resCarFilterQ.FilterByCarParameters(db, mfe, false);
                q2And3 = resFilterQ.FilterByRentalLocation(q2And3, mfe);

                dayActuals = reqCo.GetReservationsWithoutLabels(q2And3, mfe, Enums.DayActualTime.THREE).ToList();
                
                fleet = resCOFleetQ.GetQueryable(q2And3, mfe, Enums.DayActualTime.THREE).ToList();
                
                site = resCOSiteQ.GetQueryableCO(q2And3, mfe, Enums.DayActualTime.THREE).ToList();
            }

            TestResultComparison.CheckAllPoolingValues(dayActuals, fleet, site);
        }


    }
}
