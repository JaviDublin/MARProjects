using System;
using System.Data.Linq.SqlClient;
using System.Linq;
using App.Entities.VehiclesAbroad;
using DAL.VehiclesAbroad.Abstract;
using DAL.VehiclesAbroad.Queryables.Reservation;
using Mars.App.Classes.DAL.MarsDBContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mars.App.Classes.DAL.MarsDBContext; // added
using App.Entities.VehiclesAbroad; // added
using App.BLL.VehiclesAbroad.Abstract;

namespace MarsUnitTests.Vehicles_Abroad
{
    [TestClass]
    public class CompareResFleet
    {
        [TestMethod]
        public void CompareTwoQueries()
        {
            FilterEntity fe = new FilterEntity("", 0, "", "", "", "", "", "");
            fe.ReservationStartDate = new DateTime(2014, 06, 27);
            fe.ReservationEndDate = new DateTime(2014, 06, 29);

            ICarFilterEntity carF = new CarFilterEntity();

            var q1 = GetNewDataQueryable(new MarsDBDataContext(), fe, carF, "");

            var c = q1.Count();


            var q2 = GetOldDataQueryable(new MarsDBDataContext(), fe, carF, "");

            var c2 = q1.Count();
            
            
        }


        public IQueryable<ReservationMatchEntity> GetNewDataQueryable(MarsDBDataContext db, IFilterEntity Filters, ICarFilterEntity CarFilters, string SortExpression)
        {
            try
            {
                var tmp = new FleetEuropeActualQueryable().GetQueryable(db);

        IQueryable<Reservations> q1 = from p in db.Reservations
                                              join startloc in db.LOCATIONs on p.RENT_LOC equals startloc.dim_Location_id
                                              join startCmsLoc in db.CMS_LOCATION_GROUPs on startloc.cms_location_group_id equals
                                                  startCmsLoc.cms_location_group_id
                                              join startCmsP in db.CMS_POOLs on startCmsLoc.cms_pool_id equals startCmsP.cms_pool_id
                                              join startCtry in db.COUNTRies on startCmsP.country equals startCtry.country1
                                              // Return Location
                                              join returnloc in db.LOCATIONs on p.RTRN_LOC equals returnloc.dim_Location_id

                                              // Car details
                                              join carGp in db.CAR_GROUPs on p.GR_INCL_GOLDUPGR equals carGp.car_group_id
                                              where (p.COUNTRY != returnloc.country)
                                                    && (Filters.OwnCountry == p.COUNTRY || Filters.OwnCountry == "" || Filters.OwnCountry == null)
                                                    &&
                                                    (p.ReservedCarGroup == Filters.CarGroup || Filters.CarGroup == "" || Filters.CarGroup == null)
                                                    && (startCtry.active) // only corporate countries
                                                    && (Filters.CarGroup == carGp.car_group1 || string.IsNullOrEmpty(Filters.CarGroup))
                                                    &&
                                                    (p.RS_ARRIVAL_DATE >= Filters.ReservationStartDate &&
                                                     p.RS_ARRIVAL_DATE <= Filters.ReservationEndDate)
                                              // reservation start dates
                                              select p;

                q1 = new ReservationMatchCarSegmentQueryable().GetQueryable(db, q1, Filters.CarSegment);
                q1 = new ReservationMatchCarClassQueryable().GetQueryable(db, q1, Filters.CarClass);
                q1 = new ReservationMatchPoolQueryable().GetQueryable(db, q1, Filters.Pool);
                q1 = new ReservationMatchReturnQueryable().GetQueryable(db, q1, Filters.DueCountry, Filters.DuePool, Filters.DueLocationGroup);

                IQueryable<ReservationMatchEntity> q = from p in q1
                                                       // Start Location
                                                       join startloc in db.LOCATIONs on p.RENT_LOC equals startloc.dim_Location_id
                                                       join startCmsLoc in db.CMS_LOCATION_GROUPs on startloc.cms_location_group_id equals
                                                           startCmsLoc.cms_location_group_id
                                                       join startCmsP in db.CMS_POOLs on startCmsLoc.cms_pool_id equals startCmsP.cms_pool_id
                                                       join startCtry in db.COUNTRies on startCmsP.country equals startCtry.country1
                                                       // Return Location
                                                       join returnloc in db.LOCATIONs on p.RTRN_LOC equals returnloc.dim_Location_id
                                                       join returnCmsLoc in db.CMS_LOCATION_GROUPs on returnloc.cms_location_group_id equals
                                                           returnCmsLoc.cms_location_group_id
                                                       join returnCmsP in db.CMS_POOLs on returnCmsLoc.cms_pool_id equals returnCmsP.cms_pool_id
                                                       join returnCtry in db.COUNTRies on returnCmsP.country equals returnCtry.country1
                                                       // Car details
                                                       join carGp in db.CAR_GROUPs on p.GR_INCL_GOLDUPGR equals carGp.car_group_id
                                                       join carCs in db.CAR_CLASSes on carGp.car_class_id equals carCs.car_class_id
                                                       join carS in db.CAR_SEGMENTs on carCs.car_segment_id equals carS.car_segment_id
                                                       join t1 in tmp on new { x0 = returnloc.served_by_locn.Substring(0, 2), x1 = returnCmsLoc.cms_location_group1 } equals new { x0 = t1.OwningCountry, x1 = t1.CurrentLocation } into tj
                                                       from t in tj.DefaultIfEmpty()
                                                       where (returnCmsLoc.cms_location_group1 == Filters.Location || Filters.Location == "" || Filters.Location == null)
                                                       select new ReservationMatchEntity
                                                       {
                                                           ResLocation = startloc.served_by_locn,
                                                           ResGroup = carGp.car_group1,
                                                           ResCheckoutDate = new DateTime(p.RS_ARRIVAL_DATE.Value.Year, p.RS_ARRIVAL_DATE.Value.Month, p.RS_ARRIVAL_DATE.Value.Day, p.RS_ARRIVAL_TIME.Value.Hour, p.RS_ARRIVAL_TIME.Value.Minute, 0),
                                                           ResCheckoutLoc = startloc.served_by_locn,
                                                           ResCheckinLoc = returnloc.served_by_locn,
                                                           ResId = p.RES_ID_NBR,
                                                           ResNoDaysUntilCheckout = SqlMethods.DateDiffDay(DateTime.Now, p.RS_ARRIVAL_DATE.Value).ToString(),
                                                           ResNoDaysReserved = ((int)p.RES_DAYS).ToString(),
                                                           ResDriverName = p.CUST_NAME,
                                                           Matches = t.Count == null ? "0" : t.Count.ToString()
                                                       }; 

                switch (SortExpression)
                {
                    case "ResLocation": q = q.OrderBy(p => p.ResLocation); break;
                    case "ResLocation DESC": q = q.OrderByDescending(p => p.ResLocation); break;
                    case "ResGroup": q = q.OrderBy(p => p.ResGroup); break;
                    case "ResGroup DESC": q = q.OrderByDescending(p => p.ResGroup); break;
                    case "ResCheckoutDate": q = q.OrderBy(p => p.ResCheckoutDate); break;
                    case "ResCheckoutDate DESC": q = q.OrderByDescending(p => p.ResCheckoutDate); break;
                    case "ResCheckinLoc": q = q.OrderBy(p => p.ResCheckinLoc); break;
                    case "ResCheckinLoc DESC": q = q.OrderByDescending(p => p.ResCheckinLoc); break;
                    case "ResNoDaysUntilCheckout": q = q.OrderBy(p => p.ResNoDaysUntilCheckout); break;
                    case "ResNoDaysUntilCheckout DESC": q = q.OrderByDescending(p => p.ResNoDaysUntilCheckout); break;
                    case "ResNoDaysReserved": q = q.OrderBy(p => p.ResNoDaysReserved); break;
                    case "ResNoDaysReserved DESC": q = q.OrderByDescending(p => p.ResNoDaysReserved); break;
                    case "ResDriverName": q = q.OrderBy(p => p.ResDriverName); break;
                    case "ResDriverName DESC": q = q.OrderByDescending(p => p.ResDriverName); break;
                    case "ResId": q = q.OrderBy(p => p.ResId); break;
                    case "ResId DESC": q = q.OrderByDescending(p => p.ResId); break;
                    case "Matches": q = q.OrderBy(p => p.Matches); break;
                    case "Matches DESC": q = q.OrderByDescending(p => p.Matches); break;
                    default: q = q.OrderBy(p => p.ResNoDaysUntilCheckout).ThenBy(p => p.ResLocation); break;
                }
                return q;
            }
            catch (Exception ex)
            {
              //  _logger.Error("Exception thrown in ReservationDetails Model, exception = " + ex);
                throw new Exception("Exception thrown in ReservationDetails Model, exception = " + ex);
            }
        }



        public  IQueryable<ReservationMatchEntity> GetOldDataQueryable(MarsDBDataContext db, IFilterEntity Filters, ICarFilterEntity CarFilters, string SortExpression)
        {
            try
            {
                var tmp = new FleetEuropeActualQueryable().GetQueryable(db);

                IQueryable<RESERVATIONS_EUROPE_ACTUAL> q1 = from p in db.RESERVATIONS_EUROPE_ACTUALs
                                                            join tc1 in db.COUNTRies on p.COUNTRY equals tc1.country1
                                                            where (p.COUNTRY != p.RTRN_LOC.Substring(0, 2))
                                                            && (Filters.OwnCountry == p.COUNTRY || Filters.OwnCountry == "" || Filters.OwnCountry == null)
                                                            && (p.RES_VEH_CLASS == Filters.CarGroup || Filters.CarGroup == "" || Filters.CarGroup == null)
                                                            && (tc1.active) // only corporate countries
                                                            && (Filters.CarGroup == p.GR_INCL_GOLDUPGR || string.IsNullOrEmpty(Filters.CarGroup))
                                                            && (p.RS_ARRIVAL_DATE >= Filters.ReservationStartDate && p.RS_ARRIVAL_DATE <= Filters.ReservationEndDate) // reservation start dates
                                                            select p;
                q1 = new OldReservationMatchCarSegmentQueryable().GetQueryable(db, q1, Filters.CarSegment);
                q1 = new OldReservationMatchCarClassQueryable().GetQueryable(db, q1, Filters.CarClass);
                q1 = new OldReservationMatchPoolQueryable().GetQueryable(db, q1, Filters.Pool);
                q1 = new OldReservationMatchReturnQueryable().GetQueryable(db, q1, Filters.DueCountry, Filters.DuePool, Filters.DueLocationGroup);

                IQueryable<ReservationMatchEntity> q = from p in q1
                                                       join tLoc in db.CMS_LOCATION_GROUPs on p.CMS_LOC_GRP equals tLoc.cms_location_group_id.ToString()
                                                       join t1 in tmp on new { x0 = p.RTRN_LOC.Substring(0, 2), x1 = tLoc.cms_location_group1 } equals new { x0 = t1.OwningCountry, x1 = t1.CurrentLocation } into tj
                                                       from t in tj.DefaultIfEmpty()
                                                       where (tLoc.cms_location_group1 == Filters.Location || Filters.Location == "" || Filters.Location == null)
                                                       select new ReservationMatchEntity
                                                       {
                                                           ResLocation = p.RES_LOC,
                                                           ResGroup = p.GR_INCL_GOLDUPGR,
                                                           ResCheckoutDate = new DateTime(p.RS_ARRIVAL_DATE.Value.Year, p.RS_ARRIVAL_DATE.Value.Month, p.RS_ARRIVAL_DATE.Value.Day, p.RS_ARRIVAL_TIME.Value.Hour, p.RS_ARRIVAL_TIME.Value.Minute, 0),
                                                           ResCheckoutLoc = p.RENT_LOC,
                                                           ResCheckinLoc = p.RTRN_LOC,
                                                           ResId = p.RES_ID_NBR,
                                                           ResNoDaysUntilCheckout = SqlMethods.DateDiffDay(DateTime.Now, p.RS_ARRIVAL_DATE.Value).ToString(),
                                                           ResNoDaysReserved = ((int)p.RES_DAYS).ToString(),
                                                           ResDriverName = p.CUST_NAME,
                                                           Matches = t.Count == null ? "0" : t.Count.ToString()
                                                       };

                switch (SortExpression)
                {
                    case "ResLocation": q = q.OrderBy(p => p.ResLocation); break;
                    case "ResLocation DESC": q = q.OrderByDescending(p => p.ResLocation); break;
                    case "ResGroup": q = q.OrderBy(p => p.ResGroup); break;
                    case "ResGroup DESC": q = q.OrderByDescending(p => p.ResGroup); break;
                    case "ResCheckoutDate": q = q.OrderBy(p => p.ResCheckoutDate); break;
                    case "ResCheckoutDate DESC": q = q.OrderByDescending(p => p.ResCheckoutDate); break;
                    case "ResCheckinLoc": q = q.OrderBy(p => p.ResCheckinLoc); break;
                    case "ResCheckinLoc DESC": q = q.OrderByDescending(p => p.ResCheckinLoc); break;
                    case "ResNoDaysUntilCheckout": q = q.OrderBy(p => p.ResNoDaysUntilCheckout); break;
                    case "ResNoDaysUntilCheckout DESC": q = q.OrderByDescending(p => p.ResNoDaysUntilCheckout); break;
                    case "ResNoDaysReserved": q = q.OrderBy(p => p.ResNoDaysReserved); break;
                    case "ResNoDaysReserved DESC": q = q.OrderByDescending(p => p.ResNoDaysReserved); break;
                    case "ResDriverName": q = q.OrderBy(p => p.ResDriverName); break;
                    case "ResDriverName DESC": q = q.OrderByDescending(p => p.ResDriverName); break;
                    case "ResId": q = q.OrderBy(p => p.ResId); break;
                    case "ResId DESC": q = q.OrderByDescending(p => p.ResId); break;
                    case "Matches": q = q.OrderBy(p => p.Matches); break;
                    case "Matches DESC": q = q.OrderByDescending(p => p.Matches); break;
                    default: q = q.OrderBy(p => p.ResNoDaysUntilCheckout).ThenBy(p => p.ResLocation); break;
                }
                return q;
            }
            catch (Exception ex)
            {
          //      _logger.Error("Exception thrown in ReservationDetails Model, exception = " + ex);
                throw new Exception("Exception thrown in ReservationDetails Model, exception = " + ex);
            }
        }



            public class OldReservationMatchReturnQueryable:Queryable<RESERVATIONS_EUROPE_ACTUAL> {

        public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, params string[] s) {
            throw new NotImplementedException();
        }
        public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, IQueryable<RESERVATIONS_EUROPE_ACTUAL> q, params string[] s) {
            if(string.IsNullOrEmpty(s[0])) return q;
            return from p in q
                   join returnLocations in db.LOCATIONs on p.RTRN_LOC equals returnLocations.location1
                   join returnLocationGroups in db.CMS_LOCATION_GROUPs on returnLocations.cms_location_group_id equals returnLocationGroups.cms_location_group_id
                   join returnPools in db.CMS_POOLs on returnLocationGroups.cms_pool_id equals returnPools.cms_pool_id
                   where (returnPools.country.Equals(s[0]))
                   && (returnPools.cms_pool1.Equals(s[1]) || string.IsNullOrEmpty(s[1]))
                   &&(returnLocationGroups.cms_location_group1.Equals(s[2]) || string.IsNullOrEmpty(s[2]))
                   select p;
        }
    }



            public class OldReservationMatchPoolQueryable:Queryable<RESERVATIONS_EUROPE_ACTUAL> {
        public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, params string[] s) {
            throw new NotImplementedException();
        }
        public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, IQueryable<RESERVATIONS_EUROPE_ACTUAL> q, params string[] s) {
            if(string.IsNullOrEmpty(s[0])) return q;
            return from p in q
                   join tPool in db.CMS_POOLs on p.CMS_POOL equals tPool.cms_pool_id.ToString()
                   where tPool.cms_pool1 == s[0]
                   select p;
        }
    }

        public class OldReservationMatchCarSegmentQueryable : Queryable<RESERVATIONS_EUROPE_ACTUAL>
        {

            public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, IQueryable<RESERVATIONS_EUROPE_ACTUAL> q, params string[] s)
            {
                if (string.IsNullOrEmpty(s[0])) return q;
                return from p in q
                       join carSegment in db.CAR_SEGMENTs on p.CAR_SEGMENT equals carSegment.car_segment_id.ToString()
                       where s[0] == carSegment.car_segment1
                       select p;
            }
            public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, params string[] s)
            {
                throw new NotImplementedException();
            }
        }



        public class OldReservationMatchCarClassQueryable:Queryable<RESERVATIONS_EUROPE_ACTUAL> {

            public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, params string[] s) {
                throw new NotImplementedException();
            }
            public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, IQueryable<RESERVATIONS_EUROPE_ACTUAL> q, params string[] s) {
                if(string.IsNullOrEmpty(s[0])) return q;
                return from p in q
                       join carClass in db.CAR_CLASSes on p.CAR_CLASS equals carClass.car_class_id.ToString()
                       where s[0] == carClass.car_class1
                       select p;
            }
        }
         }


       
    }

