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
    public class PoolingDataComparison
    {
        private MainFilterEntity GetMainFilterExample()
        {
            var mfe = new MainFilterEntity { Country = "Germany", PoolRegion = "Ost", CmsLogic = true, LocationGrpArea = "LEIPZIG"};
            return mfe;
        }
        

        [TestMethod]
        public void TestCalculateTopics()
        {
            //var p = new List<string>
            //                 {
            //                     "GEBRA63",
            //                     "GEBRA91",
            //                     "GECLL61",
            //                     "GEESW60",
            //                     "GEGTT91",
            //                     "GEHAJ61",
            //                     "GEHAJ63",
            //                     "GEHAJ66",
            //                     "GEHDM60",
            //                     "GEKSL60",
            //                     "GEKSL90",
            //                     "GENOH61",
            //                     "GEPDB61",
            //                     "GESGR60",
            //                     "GEWER60",
            //                     "GEWUN60",
            //                     "GEGTN61"
            //                 };

            var p = new List<string>
                             {
                                 "GELEJ50",
                             };
            var mfe = GetMainFilterExample();

            foreach (var s in p)
            {

                mfe.Branch = "";
                var site = ReservationsDataAccess.CalculateTopics(true, 72, mfe, true, true);
                mfe.Branch = s;
                var fleet = ReservationsDataAccess.CalculateTopics(true, 72, mfe, true, false);
                var dayActuals = ReservationsDataAccess.CalculateTopics(true, 72, mfe, false);

                TestResultComparison.CheckAllPoolingValues(dayActuals, fleet, site, s);
            }
            

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


                mfe.Branch = "";

                site = feaQ.GetFeaDataWithLabels(checkOut, checkInData,
                    Enums.DayActualTime.THREE, true, mfe, db).ToList();
                mfe.Branch = "GELEJ50";
                fleet = feaQ.GetFeaDataWithLabels(checkOut, checkInData,
                    Enums.DayActualTime.THREE, false, mfe, db).ToList();

                dayActuals = feaQ.GetFeaDataWithoutLabels(checkOut, checkInData, Enums.DayActualTime.THREE, mfe, db).ToList();
            }

            TestResultComparison.CheckAllPoolingValues(dayActuals, fleet, site, "GELEJ50");
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

                mfe.Branch = "";
                site = resCiSiteQ.GetQueryableCI(q2And3, mfe, Enums.DayActualTime.THREE, db);
                mfe.Branch = "GELEJ50";
                fleet = resCiFleetQ.GetFleetReservationsWithLabels(q2And3, mfe, Enums.DayActualTime.THREE, db);

                dayActuals = reqCi.GetQueryable(q2And3, mfe, Enums.DayActualTime.THREE);
            }


            TestResultComparison.CheckAllPoolingValues(dayActuals, fleet, site, "GELEJ50");
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


                mfe.Branch = "";
                site = resCOSiteQ.GetQueryableCO(q2And3, mfe, Enums.DayActualTime.THREE).ToList();
                mfe.Branch = "GELEJ50";
                fleet = resCOFleetQ.GetQueryable(q2And3, mfe, Enums.DayActualTime.THREE).ToList();
                
                
                dayActuals = reqCo.GetReservationsWithoutLabels(q2And3, mfe, Enums.DayActualTime.THREE).ToList();
            }

            TestResultComparison.CheckAllPoolingValues(dayActuals, fleet, site, "GELEJ50");
        }





    }
}
