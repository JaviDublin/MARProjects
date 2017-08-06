using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using DAL.VehiclesAbroad.Abstract;

namespace DAL.VehiclesAbroad.Queryables.Reservation {
    //public class ReservationMatchReturnQueryable:Queryable<RESERVATIONS_EUROPE_ACTUAL> {

    //    public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, params string[] s) {
    //        throw new NotImplementedException();
    //    }
    //    public override IQueryable<RESERVATIONS_EUROPE_ACTUAL> GetQueryable(MarsDBDataContext db, IQueryable<RESERVATIONS_EUROPE_ACTUAL> q, params string[] s) {
    //        if(string.IsNullOrEmpty(s[0])) return q;
    //        return from p in q
    //               join returnLocations in db.LOCATIONs on p.RTRN_LOC equals returnLocations.location1
    //               join returnLocationGroups in db.CMS_LOCATION_GROUPs on returnLocations.cms_location_group_id equals returnLocationGroups.cms_location_group_id
    //               join returnPools in db.CMS_POOLs on returnLocationGroups.cms_pool_id equals returnPools.cms_pool_id
    //               where (returnPools.country.Equals(s[0]))
    //               && (returnPools.cms_pool1.Equals(s[1]) || string.IsNullOrEmpty(s[1]))
    //               &&(returnLocationGroups.cms_location_group1.Equals(s[2]) || string.IsNullOrEmpty(s[2]))
    //               select p;
    //    }
    //}



    public class ReservationMatchReturnQueryable : Queryable<Reservations>
    {

        public override IQueryable<Reservations> GetQueryable(MarsDBDataContext db, params string[] s)
        {
            throw new NotImplementedException();
        }
        public override IQueryable<Reservations> GetQueryable(MarsDBDataContext db, IQueryable<Reservations> q, params string[] s)
        {
            if (string.IsNullOrEmpty(s[0])) return q;
            return from p in q
                   // Return Location
                   join returnloc in db.LOCATIONs on p.RTRN_LOC equals returnloc.dim_Location_id
                   join returnCmsLoc in db.CMS_LOCATION_GROUPs on returnloc.cms_location_group_id equals
                       returnCmsLoc.cms_location_group_id
                   join returnCmsP in db.CMS_POOLs on returnCmsLoc.cms_pool_id equals returnCmsP.cms_pool_id
                   where (returnCmsP.country.Equals(s[0]))
                   && (returnCmsP.cms_pool1.Equals(s[1]) || string.IsNullOrEmpty(s[1]))
                   && (returnCmsLoc.cms_location_group1.Equals(s[2]) || string.IsNullOrEmpty(s[2]))
                   select p;
        }
    }

}