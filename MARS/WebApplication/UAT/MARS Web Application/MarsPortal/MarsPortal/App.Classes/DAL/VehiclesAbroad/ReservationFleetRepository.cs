using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using DAL.VehiclesAbroad.Abstract;
using App.Classes.Entities.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;
using Mars.App.Classes.DAL.MarsDBContext;
using DAL.VehiclesAbroad.Queryables.Reservation;

namespace DAL.VehiclesAbroad {
    public class ReservationFleetRepository:ReservationRepository<ReservationMatchEntity> {

        public override IList<ReservationMatchEntity> GetList(IFilterEntity Filters, ICarFilterEntity CarFilters, string SortExpression) {
            using(MarsDBDataContext db = new MarsDBDataContext())
                return GetQueryable(db, Filters, CarFilters, SortExpression).ToList();
        }
        public override IQueryable<ReservationMatchEntity> GetQueryable(MarsDBDataContext db, IFilterEntity Filters, ICarFilterEntity CarFilters, string SortExpression) {
            try {
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
                q1=new ReservationMatchPoolQueryable().GetQueryable(db, q1, Filters.Pool);
                q1=new ReservationMatchReturnQueryable().GetQueryable(db, q1, Filters.DueCountry, Filters.DuePool, Filters.DueLocationGroup);

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
                    join t1 in tmp on
                        new {x0 = returnloc.served_by_locn.Substring(0, 2), x1 = startCmsLoc.cms_location_group1} equals
                        new {x0 = t1.OwningCountry, x1 = t1.CurrentLocation} into tj
                    from t in tj.DefaultIfEmpty()
                    where
                        (returnCmsLoc.cms_location_group1 == Filters.Location || Filters.Location == "" ||
                         Filters.Location == null)
                    select new ReservationMatchEntity
                           {
                               ResLocation = startloc.served_by_locn,
                               ResGroup = carGp.car_group1,
                               ResCheckoutDate =
                                   new DateTime(p.RS_ARRIVAL_DATE.Value.Year, p.RS_ARRIVAL_DATE.Value.Month,
                                   p.RS_ARRIVAL_DATE.Value.Day, p.RS_ARRIVAL_TIME.Value.Hour,
                                   p.RS_ARRIVAL_TIME.Value.Minute, 0),
                               ResCheckoutLoc = startloc.served_by_locn,
                               ResCheckinLoc = returnloc.served_by_locn,
                               ResId = p.RES_ID_NBR,
                               ResNoDaysUntilCheckout =
                                   SqlMethods.DateDiffDay(DateTime.Now, p.RS_ARRIVAL_DATE.Value).ToString(),
                               ResNoDaysReserved = ((int) p.RES_DAYS).ToString(),
                               ResDriverName = p.CUST_NAME,
                               Matches = t.Count == null ? "0" : t.Count.ToString()
                           };

                switch(SortExpression) {
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
            } catch(Exception ex) {
                //.Error("Exception thrown in ReservationDetails Model, exception = " + ex);
                throw new Exception("Exception thrown in ReservationDetails Model, exception = " + ex);
            }
        }

        public override IQueryable<ReservationMatchEntity> GetQueryable(MarsDBDataContext db, params string[] s) {
            throw new NotImplementedException();
        }

        public override IQueryable<ReservationMatchEntity> GetQueryable(MarsDBDataContext db, IQueryable<ReservationMatchEntity> q, params string[] s) {
            throw new NotImplementedException();
        }
    }
}