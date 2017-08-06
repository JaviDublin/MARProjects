using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Entities.Pooling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests.Pooling
{
    public static class TestResultComparison
    {
        public static void CheckAllPoolingValues(List<DayActualEntity> dayActuals, List<DayActualEntity> fleet, List<DayActualEntity> site, string branch = null)
        {

            for (int i = 0; i < 30; i++)
            {
                var siteHour1 = site.Where(d => d.Tme == i && (branch == null || d.Label == branch)).ToList();
                var fleetHour1 = fleet.Where(d => d.Tme == i).ToList();
                var dayActuals1 = dayActuals.FirstOrDefault(d => d.Tme == i);

                int res1, res2, res3;

                //Balance is cascading and includes buffers so it can't be compared across pages
                //res1 = siteHour1.Sum(d => d.Balance);
                //res2 = fleetHour1.Sum(d => d.Balance);
                //res3 = dayActuals1 == null ? 0 : dayActuals1.Balance;

                //Assert.IsTrue(res1 == res2 && res1 == res3,
                //    string.Format("Balance - For Time: {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));


                res1 = siteHour1.Sum(d => d.Reservations);
                res2 = fleetHour1.Sum(d => d.Reservations);
                res3 = dayActuals1 == null ? 0 : dayActuals1.Reservations;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("Reservations - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));

                res1 = siteHour1.Sum(d => d.Available);
                res2 = fleetHour1.Sum(d => d.Available);
                res3 = dayActuals1 == null ? 0 : dayActuals1.Available;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("Available - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));


                res1 = siteHour1.Sum(d => d.Opentrips);
                res2 = fleetHour1.Sum(d => d.Opentrips);
                res3 = dayActuals1 == null ? 0 : dayActuals1.Opentrips;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("Opentrips - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));




                res1 = siteHour1.Sum(d => d.OnewayRes);
                res2 = fleetHour1.Sum(d => d.OnewayRes);
                res3 = dayActuals1 == null ? 0 : dayActuals1.OnewayRes;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("OnewayRes - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));

                res1 = siteHour1.Sum(d => d.GoldServiceReservations);
                res2 = fleetHour1.Sum(d => d.GoldServiceReservations);
                res3 = dayActuals1 == null ? 0 : dayActuals1.GoldServiceReservations;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("Gold Service - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));


                res1 = siteHour1.Sum(d => d.PrepaidReservations);
                res2 = fleetHour1.Sum(d => d.PrepaidReservations);
                res3 = dayActuals1 == null ? 0 : dayActuals1.PrepaidReservations;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("Prepaid Reservations - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));


                res1 = siteHour1.Sum(d => d.Checkin);
                res2 = fleetHour1.Sum(d => d.Checkin);
                res3 = dayActuals1 == null ? 0 : dayActuals1.Checkin;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("Checkin - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));


                res1 = siteHour1.Sum(d => d.OnewayCheckin);
                res2 = fleetHour1.Sum(d => d.OnewayCheckin);
                res3 = dayActuals1 == null ? 0 : dayActuals1.OnewayCheckin;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("OnewayCheckin - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));


                res1 = siteHour1.Sum(d => d.LocalCheckIn);
                res2 = fleetHour1.Sum(d => d.LocalCheckIn);
                res3 = dayActuals1 == null ? 0 : dayActuals1.LocalCheckIn;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("LocalCheckIn - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));


                res1 = siteHour1.Sum(d => d.AddditionDeletion);
                res2 = fleetHour1.Sum(d => d.AddditionDeletion);
                res3 = dayActuals1 == null ? 0 : dayActuals1.AddditionDeletion;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("AddditionDeletion - For Time {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));



                res1 = siteHour1.Sum(d => d.Offset);
                res2 = fleetHour1.Sum(d => d.Offset);
                res3 = dayActuals1 == null ? 0 : dayActuals1.Offset;

                Assert.IsTrue(res1 == res2 && res1 == res3,
                    string.Format("Offset - For Time: {3}, SiteComp: {0}, FleetComp: {1}, DayActuals: {2}", res1, res2, res3, i));

            }
        }
    }
}
