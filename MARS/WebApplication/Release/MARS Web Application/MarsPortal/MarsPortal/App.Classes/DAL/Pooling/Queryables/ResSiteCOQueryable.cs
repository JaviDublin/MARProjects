using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.Entities.Pooling;
using App.Classes.Entities.Pooling.Abstract;
using App.Classes.DAL.Pooling.Abstract;

namespace Mars.DAL.Pooling.Queryables
{
    public class ResSiteCOQueryable
    {
        public IQueryable<DayActualEntity> GetQueryableCO(IQueryable<Reservation> q, IMainFilterEntity filter, Enums.DayActualTime time)
        {
            var now = DateTime.Now.GetDateAndHourOnlyByCountry(filter.Country);

            return from p in q
                   where (time == Enums.DayActualTime.THREE ? (
                                    SqlMethods.DateDiffHour(now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour)) >= 0 &&
                                    SqlMethods.DateDiffHour(now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour)) <= 71)
                                    : (SqlMethods.DateDiffHour(now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffDay(now, p.RS_ARRIVAL_DATE.Value) <= 29))
                   group p by new
                   {
                       t1 = (String.IsNullOrEmpty(filter.Country) ? p.LOCATION1.COUNTRy1.country_description :
                              String.IsNullOrEmpty(filter.PoolRegion) ? filter.CmsLogic ? p.LOCATION1.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 : p.LOCATION1.OPS_AREA.OPS_REGION.ops_region1 :
                              String.IsNullOrEmpty(filter.LocationGrpArea) ? filter.CmsLogic ? p.LOCATION1.CMS_LOCATION_GROUP.cms_location_group1 : p.LOCATION1.OPS_AREA.ops_area1 :
                              p.LOCATION1.served_by_locn),
                       t2 = (time == Enums.DayActualTime.THREE 
                        ? SqlMethods.DateDiffHour( now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour)
                                                        .AddMinutes(p.RS_ARRIVAL_TIME.Value.Minute))
                                    : SqlMethods.DateDiffDay(now, p.RS_ARRIVAL_DATE.Value))
                   }
                       into g
                       select new DayActualEntity
                       {
                           Label = g.Key.t1,
                           Tme = g.Key.t2 ,
                           Reservations = g.Sum(p => p.RES_ID_NBR != null ? 1 : 0),
                           OnewayRes = g.Sum(p => p.ONEWAY == "Y" ? 1 : 0),
                           GoldServiceReservations = g.Sum(p => p.GS == "Y" ? 1 : 0),
                           PrepaidReservations = g.Sum(p => p.PREPAID == "Y" ? 1 : 0),
                           Predelivery = g.Sum(p => p.PREDELIVERY == "Y" ? 1 : 0),
                       };
        }
    }
}