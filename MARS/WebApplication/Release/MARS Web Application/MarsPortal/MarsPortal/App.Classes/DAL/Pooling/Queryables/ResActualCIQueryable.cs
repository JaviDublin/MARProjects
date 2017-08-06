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
    public class ResActualCIQueryable
    {
        public List<DayActualEntity> GetQueryable(IQueryable<Reservation> q, IMainFilterEntity filter, Enums.DayActualTime time)
        {
            var now = DateTime.Now.GetDateAndHourOnlyByCountry(filter.Country);
            
            var actualData = from p in q
                   where (time == Enums.DayActualTime.THREE ?
                    (SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour)) >= 0
                        && SqlMethods.DateDiffHour(now, p.ArrivalDateTime) >= 0
                        && SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour)) <= 71)
                    : (SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour)) >= 0
                        && SqlMethods.DateDiffDay(now, p.RTRN_DATE.Value) <= 29))
                             group p by (time == Enums.DayActualTime.THREE 
                                ? SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour)) 
                                : SqlMethods.DateDiffDay(now, p.RTRN_DATE.Value))
                       into g
                       select new DayActualEntity
                       {
                           Tme = g.Key,
                           Checkin = g.Sum(p => p.RES_ID_NBR != null ? 1 : 0),
                           OnewayCheckin = g.Sum(p => p.ONEWAY == "Y" ? 1 : 0),
                           LocalCheckIn = g.Sum(p => p.ONEWAY != "Y" ? 1 : 0)
                       };

            var offsetData = from p in q
                             where (time == Enums.DayActualTime.THREE
                                ? (SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value)) >= 0
                                && SqlMethods.DateDiffHour(now, p.ArrivalDateTime) >= 0
                                && SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value)) <= 71)
                                : (SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour)) >= 0
                                && SqlMethods.DateDiffDay(now, p.RTRN_DATE.Value) <= 29))
                             group p by (time == Enums.DayActualTime.THREE
                                ? SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value))
                                : SqlMethods.DateDiffDay(now, p.RTRN_DATE.Value))
                                 into g
                                 select new DayActualEntity
                                 {
                                     Tme = g.Key,
                                     Offset = g.Sum(p => p.RES_ID_NBR != null ? 1 : 0),
                                 };

            

            var combinedData = from cd in actualData.ToList().Union(offsetData.ToList())
                           group cd by cd.Tme
                               into gd
                               select new DayActualEntity
                               {
                                   Tme = gd.Key,
                                   Checkin = gd.Sum(d => d.Checkin),
                                   OnewayCheckin = gd.Sum(d => d.OnewayCheckin),
                                   LocalCheckIn = gd.Sum(d => d.LocalCheckIn),
                                   Offset = gd.Sum(d => d.Offset)
                               };

            var returned = combinedData.ToList();
            return returned;
        }
    }
}