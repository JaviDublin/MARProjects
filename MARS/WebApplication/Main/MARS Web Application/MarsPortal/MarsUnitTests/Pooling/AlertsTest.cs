using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.App.Classes.DAL.Pooling.SharedDataAccess;
using Mars.Entities.Pooling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests.Pooling
{
    [TestClass]
    public class AlertsTest
    {
        [TestMethod]
        public void TestMethod0()
        {
            var mfe = new MainFilterEntity
            {
                Country = "Germany",
                PoolRegion = "Sud",
                CmsLogic = true,
                LocationGrpArea = "MUENCHEN CITY",
                Branch = "GEMUC73",
                CarSegment = "Van",
                CarClass = "Transporter",
                CarGroup = "A4"
            };

            //var ss = AlertsDataAccess.GetLocationCarGroupsWithNegativeBalance(mfe, DateTime.Now.AddDays(2).Date);

            var alertClasses = AlertsDataAccess.GetLocationCarGroups(mfe, DateTime.Now.AddDays(2).Date);
            //var dd = alertClasses.Where(d => d.Label.Substring(0,7) == "GEAAC60").ToList();

            var actualsClasses = ReservationsDataAccess.CalculateTopics(true, 20, mfe, false, false);

            foreach (var ac in alertClasses)
            {
                var time = ac.Tme;
                var data = actualsClasses.FirstOrDefault(d => d.Tme == time);
                //var labelBits = ac.Label.Split(' ');
                if (data != null)
                {
                    Assert.IsTrue(ac.Balance == data.Balance);
                }

                //var branch = labelBits[0];
                // var carGroup = labelBits[1];
                //var balance = ac.Balance;
            }


            Assert.IsTrue(true);
        }


        [TestMethod]
        public void TestMethod1()
        {
            //var mfe = new MainFilterEntity { Country = "United Kingdom", PoolRegion = "South England"
            //        , CmsLogic = true, LocationGrpArea = "HEATHROW"
            //        , CarSegment = "Car", CarClass = @"CompactMan", CarGroup = "C"};
            var mfe = new MainFilterEntity
            {
                Country = "Germany", PoolRegion = "Nord"
                , CmsLogic = true, LocationGrpArea = "OSNABRUECK", Branch = "GEOSB61",
                CarSegment = "Car", CarClass = "Compact", CarGroup = "D"
            };

            //var ss = AlertsDataAccess.GetLocationCarGroupsWithNegativeBalance(mfe, DateTime.Now.AddDays(2).Date);

            var alertClasses = AlertsDataAccess.GetLocationCarGroups(mfe, DateTime.Now.AddDays(1).Date);
            //var dd = alertClasses.Where(d => d.Label.Substring(0,7) == "GEAAC60").ToList();

            var actualsClasses = ReservationsDataAccess.CalculateTopics(true, 20, mfe, false, false);

            foreach (var ac in alertClasses)
            {
                var time = ac.Tme;
                var data = actualsClasses.FirstOrDefault(d => d.Tme == time);
                //var labelBits = ac.Label.Split(' ');
                if (data != null)
                {
                    Assert.IsTrue(ac.Balance == data.Balance);
                }

                //var branch = labelBits[0];
                // var carGroup = labelBits[1];
                //var balance = ac.Balance;


            }


            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var mfe = new MainFilterEntity
            {
                Country = "United Kingdom",
                PoolRegion = "South England"
                ,
                CmsLogic = true,
                LocationGrpArea = "HEATHROW"
                ,
                CarSegment = "Car",
                CarClass = @"CompactMan",
                CarGroup = "C"
            };


            //var ss = AlertsDataAccess.GetLocationCarGroupsWithNegativeBalance(mfe, DateTime.Now.AddDays(2).Date);

            var alertClasses = AlertsDataAccess.GetLocationCarGroups(mfe, DateTime.Now.AddDays(1).Date);
            //var dd = alertClasses.Where(d => d.Label.Substring(0,7) == "GEAAC60").ToList();

            var actualsClasses = ReservationsDataAccess.CalculateTopics(true, 20, mfe, false, false);

            foreach (var ac in alertClasses)
            {
                var time = ac.Tme;
                var data = actualsClasses.FirstOrDefault(d => d.Tme == time);
                //var labelBits = ac.Label.Split(' ');
                if (data != null)
                {
                    Assert.IsTrue(ac.Balance == data.Balance);
                }

                //var branch = labelBits[0];
                // var carGroup = labelBits[1];
                //var balance = ac.Balance;


            }


            Assert.IsTrue(true);
        }
    }
}
