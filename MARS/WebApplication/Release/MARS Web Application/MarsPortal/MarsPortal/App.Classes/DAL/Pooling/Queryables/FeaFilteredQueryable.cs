using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.DAL.Pooling.Queryables
{
    public class FeaFilteredQueryable
    {
        public IQueryable<FLEET_EUROPE_ACTUAL> GetFeaCheckOut(MarsDBDataContext db, IMainFilterEntity filter)
        {
            //db.Log = new DebugTextWriter();
            var returned = from p in db.FLEET_EUROPE_ACTUALs
                   join locA in db.LOCATIONs on p.LSTWWD equals locA.location1
                   join loc in db.LOCATIONs on locA.served_by_locn equals loc.location1
                   
                   join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals new 
                                        { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                   where p.FLEET_RAC_OPS.HasValue && p.FLEET_RAC_OPS.Value && p.TOTAL_FLEET > 0
                    //&& (loc.COUNTRy1.active)
                   //&& loc.active
                   && (loc.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                   && p.COUNTRY == loc.country
                   //&& (loc.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                   && (loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == filter.PoolRegion 
                    || loc.OPS_AREA.OPS_REGION.ops_region1 == filter.PoolRegion 
                    || String.IsNullOrEmpty(filter.PoolRegion))
                   && (loc.CMS_LOCATION_GROUP.cms_location_group1 == filter.LocationGrpArea 
                   || loc.OPS_AREA.ops_area1 == filter.LocationGrpArea 
                   || String.IsNullOrEmpty(filter.LocationGrpArea))
                   && (loc.served_by_locn == filter.Branch || String.IsNullOrEmpty(filter.Branch))
                   && (cg.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                   && (cg.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                   && (cg.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                   select p;

            if (filter.ExcludeLongterm)
            {
                returned = returned.Except(returned.Where(d => d.LSTWWD.Substring(5, 1) != "5" &&
                                               SqlMethods.DateDiffDay(d.LSTDATE, d.DUEDATE) > 27));
                //returned = returned.Where(d => !(d.LSTWWD.Substring(5, 1) != "5" &&     
                //                               SqlMethods.DateDiffDay(d.LSTDATE, d.DUEDATE) > 27));
            }
            //var ss = returned.ToList();

            return returned;
        }

        public IQueryable<FLEET_EUROPE_ACTUAL> GetFeaCheckIn(MarsDBDataContext db, IMainFilterEntity filter)
        {
            //db.Log = new DebugTextWriter();
            var returned = from p in db.FLEET_EUROPE_ACTUALs
                           join locA in db.LOCATIONs on p.DUEWWD equals locA.location1
                           join loc in db.LOCATIONs on locA.served_by_locn equals loc.location1
                           join c in db.COUNTRies on p.COUNTRY equals c.country1
                           join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                           where p.FLEET_RAC_OPS.HasValue && p.FLEET_RAC_OPS.Value && p.TOTAL_FLEET > 0
                           
                           //&& (loc.COUNTRy1.active)
                           //&& loc.active
                           && p.MOVETYPE == "R-O"
                           && p.COUNTRY == loc.country
                           && (loc.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                           //&& (c.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                           && (loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == filter.PoolRegion || loc.OPS_AREA.OPS_REGION.ops_region1 == filter.PoolRegion || String.IsNullOrEmpty(filter.PoolRegion))
                           && (loc.CMS_LOCATION_GROUP.cms_location_group1 == filter.LocationGrpArea || loc.OPS_AREA.ops_area1 == filter.LocationGrpArea || String.IsNullOrEmpty(filter.LocationGrpArea))
                           && (loc.served_by_locn == filter.Branch || String.IsNullOrEmpty(filter.Branch))
                           && (cg.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                           && (cg.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                           && (cg.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                           select p;

            if (filter.ExcludeLongterm)
            {
                returned = returned.Except(returned.Where(d => d.DUEWWD.Substring(5, 1) != "5" &&
                                               SqlMethods.DateDiffDay(d.LSTDATE, d.DUEDATE) > 27));
                //returned = returned.Where(d => !(d.DUEWWD.Substring(5, 1) != "5" &&
                //                               SqlMethods.DateDiffDay(d.LSTDATE, d.DUEDATE) > 27));
            }

            

            return returned;
        }
    }
}