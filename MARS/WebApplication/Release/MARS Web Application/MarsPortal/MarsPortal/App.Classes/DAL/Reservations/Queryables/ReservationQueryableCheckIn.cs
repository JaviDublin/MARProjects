using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;

namespace App.Classes.DAL.Reservations.Queryables
{
    public class ReservationQueryableCheckIn
    { // the return location
        //public IQueryable<RESERVATIONS_EUROPE_ACTUAL> getQueryable(MarsDBDataContext db, IMainFilterEntity f)
        //{
        //    return from p in db.RESERVATIONS_EUROPE_ACTUALs
        //        join c in db.COUNTRies on p.RTRN_LOC.Substring(0, 2) equals c.country1
        //        join loc in db.LOCATIONs on p.RTRN_LOC equals loc.location1
        //        join clg in db.CMS_LOCATION_GROUPs on loc.cms_location_group_id equals clg.cms_location_group_id
        //        join pl in db.CMS_POOLs on clg.cms_pool_id equals pl.cms_pool_id
        //        join oa in db.OPS_AREAs on loc.ops_area_id equals oa.ops_area_id
        //        join ors in db.OPS_REGIONs on oa.ops_region_id equals ors.ops_region_id
        //        where (f.Country == c.country_description || string.IsNullOrEmpty(f.Country))
        //              &&
        //              ((f.CmsLogic ? f.PoolRegion == pl.cms_pool1 : f.PoolRegion == ors.ops_region1) ||
        //               string.IsNullOrEmpty(f.PoolRegion))
        //              &&
        //              ((f.CmsLogic ? f.LocationGrpArea == clg.cms_location_group1 : f.LocationGrpArea == oa.ops_area1) ||
        //               string.IsNullOrEmpty(f.LocationGrpArea))
        //              && (f.Branch == p.RTRN_LOC || string.IsNullOrEmpty(f.Branch))
        //        select p;
        //}

        public IQueryable<Mars.App.Classes.DAL.MarsDBContext.Reservations> getQueryable(MarsDBDataContext db, IMainFilterEntity f)
        {
            return from p in db.Reservations
                // Return Location
                join returnloc in db.LOCATIONs on p.RTRN_LOC equals returnloc.dim_Location_id
                join returnCmsLoc in db.CMS_LOCATION_GROUPs on returnloc.cms_location_group_id equals
                    returnCmsLoc.cms_location_group_id
                join returnCmsP in db.CMS_POOLs on returnCmsLoc.cms_pool_id equals returnCmsP.cms_pool_id
                join returnCtry in db.COUNTRies on returnCmsP.country equals returnCtry.country1
                join opR in db.OPS_REGIONs on returnCtry.country1 equals opR.country
                join opA in db.OPS_AREAs on opR.ops_region_id equals opA.ops_region_id

                where (f.Country == returnCtry.country_description || string.IsNullOrEmpty(f.Country))
                      &&
                      ((f.CmsLogic ? f.PoolRegion == returnCmsP.cms_pool1 : f.PoolRegion == opR.ops_region1) ||
                       string.IsNullOrEmpty(f.PoolRegion))
                      &&
                      ((f.CmsLogic
                          ? f.LocationGrpArea == returnCmsLoc.cms_location_group1
                          : f.LocationGrpArea == opA.ops_area1) ||
                       string.IsNullOrEmpty(f.LocationGrpArea))
                      && (f.Branch == returnloc.served_by_locn || string.IsNullOrEmpty(f.Branch))
                select p;
        }
    }
}