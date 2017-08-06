using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;

namespace App.Classes.DAL.Reservations.Queryables {
    public class ReservationQueryableCheckOut { // returns for filters in that country
        

        public IQueryable<Mars.App.Classes.DAL.MarsDBContext.Reservations> getQueryable(MarsDBDataContext db, IMainFilterEntity f)
        {
            return from p in db.Reservations
                   // rent Location
                   join rentloc in db.LOCATIONs on p.RENT_LOC equals rentloc.dim_Location_id
                   join rentCmsLoc in db.CMS_LOCATION_GROUPs on rentloc.cms_location_group_id equals
                       rentCmsLoc.cms_location_group_id
                   join rentCmsP in db.CMS_POOLs on rentCmsLoc.cms_pool_id equals rentCmsP.cms_pool_id
                   join rentCtry in db.COUNTRies on rentCmsP.country equals rentCtry.country1
                   join opR in db.OPS_REGIONs on rentCtry.country1 equals opR.country
                   join opA in db.OPS_AREAs on opR.ops_region_id equals opA.ops_region_id
                   where (f.Country == rentCtry.country_description || string.IsNullOrEmpty(f.Country))
                      && ((f.CmsLogic ? f.PoolRegion == rentCmsP.cms_pool1 : f.PoolRegion == opR.ops_region1) || string.IsNullOrEmpty(f.PoolRegion))
                      && ((f.CmsLogic ? f.LocationGrpArea == rentCmsLoc.cms_location_group1 : f.LocationGrpArea == opA.ops_area1) || string.IsNullOrEmpty(f.LocationGrpArea))
                      && (f.Branch == rentloc.served_by_locn || string.IsNullOrEmpty(f.Branch))
                   select p;
        }
    }
}