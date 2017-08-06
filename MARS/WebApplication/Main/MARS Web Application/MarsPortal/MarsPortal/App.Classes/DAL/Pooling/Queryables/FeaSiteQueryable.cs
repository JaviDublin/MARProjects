using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;
using Mars.Entities.Pooling;
using App.Classes.DAL.Pooling.Abstract;

namespace Mars.DAL.Pooling.Queryables
{
    public class FeaSiteQueryable
    {
        public IQueryable<DayActualEntity> GetQueryable(MarsDBDataContext db, IMainFilterEntity filter, Enums.DayActualTime tm)
        {
            return from p in db.FLEET_EUROPE_ACTUALs
                   join loc in db.LOCATIONs on p.LSTWWD equals loc.location1
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
                   where (tm == Enums.DayActualTime.THREE ? (p.CI_HOURS >= 0 && p.CI_HOURS <= 71) : (p.CI_DAYS >= 0 && p.CI_DAYS <= 29))
                   group p by new
                   {
                       t1 = (String.IsNullOrEmpty(filter.Country) ? loc.COUNTRy1.country_description :
                              String.IsNullOrEmpty(filter.PoolRegion) ? filter.CmsLogic ? loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 : loc.OPS_AREA.OPS_REGION.ops_region1 :
                              String.IsNullOrEmpty(filter.LocationGrpArea) ? filter.CmsLogic ? loc.CMS_LOCATION_GROUP.cms_location_group1 : loc.OPS_AREA.ops_area1 :
                              loc.location1),
                       t2 = (tm == Enums.DayActualTime.THREE ? p.CI_HOURS : p.CI_DAYS)
                   }
                       into g
                       select new DayActualEntity
                       {
                           Tme = g.Key.t2 ?? 0,
                           Label = g.Key.t1,
                           Available = g.Sum(p => p.RT) + (g.Sum(p => p.OVERDUE) ?? 0) + g.Sum(p => p.MOVETYPE.ToUpper() == "T-O" ? p.TOTAL_FLEET : 0) ?? 0,
                           Opentrips = g.Sum(p => p.MOVETYPE.ToUpper() == "T-O" ? p.TOTAL_FLEET : 0) ?? 0,
                           Checkin = g.Sum(p => p.ON_RENT) ?? 0,
                           OnewayCheckin = g.Sum(p => p.LSTWWD != p.DUEWWD ? p.ON_RENT : 0) ?? 0,
                           LocalCheckIn = g.Sum(p => p.LSTWWD == p.DUEWWD ? p.ON_RENT : 0) ?? 0
                       };
        }
    }
}