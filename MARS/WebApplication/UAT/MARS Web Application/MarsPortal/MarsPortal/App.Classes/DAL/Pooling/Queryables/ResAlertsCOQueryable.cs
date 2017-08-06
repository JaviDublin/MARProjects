using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.Entities.Pooling;

namespace Mars.DAL.Pooling.Queryables
{
    public class ResAlertsCOQueryable
    {
        public IQueryable<AlertTempEntity> GetQueryable(IQueryable<Reservation> q, DateTime selectedDate)
        {
            int endOfDay = 24 - DateTime.Now.Hour;
            double maxHours = (selectedDate - DateTime.Now).TotalHours;;
            var returned = from p in q
                           where p.GR_INCL_GOLDUPGR != null
                           && p.RENT_LOC != null
                           && (SqlMethods.DateDiffHour(DateTime.Now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour))
                            >= 0 &&
                            SqlMethods.DateDiffHour(DateTime.Now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour))
                            <= maxHours)
                           group p by new
                           {
                               CO_HOURS = SqlMethods.DateDiffHour(DateTime.Now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour))
                                         ,
                               p.CAR_GROUP.car_group1,
                               p.LOCATION1.location1
                           } into g
                           select new AlertTempEntity
                           {
                               rsTime = g.Key.CO_HOURS == 0 ? 1
                                    : g.Key.CO_HOURS >= 1 && g.Key.CO_HOURS < 5 ? 2
                                    : g.Key.CO_HOURS <= endOfDay ? 3 : 4,
                               rentLoc = g.Key.location1,
                               crGrp = g.Key.car_group1,
                               Amnt = g.Sum(p => p.RES_ID_NBR == null ? 0 : 1)
                           };

            return returned;
        }
    }
}