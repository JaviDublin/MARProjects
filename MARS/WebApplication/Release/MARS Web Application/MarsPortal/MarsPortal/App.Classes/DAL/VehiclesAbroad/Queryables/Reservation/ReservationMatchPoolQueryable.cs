using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;

namespace DAL.VehiclesAbroad.Queryables.Reservation {

    public class ReservationMatchPoolQueryable : Queryable<Reservations>
    {
        public override IQueryable<Reservations> GetQueryable(MarsDBDataContext db, params string[] s)
        {
            throw new NotImplementedException();
        }
        public override IQueryable<Reservations> GetQueryable(MarsDBDataContext db, IQueryable<Reservations> q, params string[] s)
        {
            if (string.IsNullOrEmpty(s[0])) return q;
            return from p in q
                   join startloc in db.LOCATIONs on p.RENT_LOC equals startloc.dim_Location_id
                   join startCmsLoc in db.CMS_LOCATION_GROUPs on startloc.cms_location_group_id equals
                       startCmsLoc.cms_location_group_id
                   join startCmsP in db.CMS_POOLs on startCmsLoc.cms_pool_id equals startCmsP.cms_pool_id
                   where startCmsP.cms_pool1 == s[0]
                   select p;
        }
    }
}