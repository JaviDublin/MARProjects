using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Entities.Pooling;
using App.Classes.Entities.Pooling.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.DAL.Pooling.Queryables
{
    public class FeaAlertsQueryable
    {
        public IQueryable<AlertTempEntity> GetQueryable(MarsDBDataContext db, IMainFilterEntity filter, DateTime selectedDate)
        {
            int endOfDay = 24 - DateTime.Now.Hour;
            double maxHours = (selectedDate - DateTime.Now).TotalHours;

            var returned = from p in db.FLEET_EUROPE_ACTUALs
                           join loc in db.LOCATIONs on new { l1 = p.LSTWWD, l2 = p.COUNTRY } equals new { l1 = loc.location1, l2 = loc.country }
                           join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                           where ((p.FLEET_RAC_TTL ?? false) || (p.FLEET_CARSALES ?? false))
                           && (loc.COUNTRy1.active)
                           && (loc.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                           && (loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == filter.PoolRegion || loc.OPS_AREA.OPS_REGION.ops_region1 == filter.PoolRegion || String.IsNullOrEmpty(filter.PoolRegion))
                           && (loc.CMS_LOCATION_GROUP.cms_location_group1 == filter.LocationGrpArea || loc.OPS_AREA.ops_area1 == filter.LocationGrpArea || String.IsNullOrEmpty(filter.LocationGrpArea))
                           && (loc.location1 == filter.Branch || String.IsNullOrEmpty(filter.Branch))
                           && (cg.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                           && (cg.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                           && (cg.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                           && (p.CI_HOURS >= 0
                                    && p.CI_HOURS <= maxHours
                                    )
                           group p by new { p.CI_HOURS, cg.car_group1, loc.location1 } into g
                           select new AlertTempEntity
                           {
                               rsTime = g.Key.CI_HOURS == 0 ? 1 
                                        : g.Key.CI_HOURS >= 1 && g.Key.CI_HOURS < 5 ? 2
                                        : g.Key.CI_HOURS <= endOfDay ? 3 
                                        : 4,
                               rentLoc = g.Key.location1,
                               crGrp = g.Key.car_group1,
                               Amnt = g.Sum(p => p.MOVETYPE == "T-O" && p.TOTAL_FLEET == 1 ? 1 : 0) + g.Sum(p => p.RT) + g.Sum(p => p.ON_RENT ?? 0)
                           };


            return returned;
        }
    }
}