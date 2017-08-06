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
    public class PoolingCheckInReservations
    {
        public List<DayActualEntity> GetFleetReservationsWithLabels(IQueryable<Reservation> q, IMainFilterEntity filter,
            Enums.DayActualTime time, PoolingDataClassesDataContext db)
        {

            var now = DateTime.Now.GetDateAndHourOnlyByCountry(filter.Country);

            var actualData = from p in q
                             //join c in db.COUNTRies on p.COUNTRY equals c.country1
                             where (time == Enums.DayActualTime.THREE ? (SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour)) >= 0
                                                                    && SqlMethods.DateDiffHour(now, p.ArrivalDateTime) >= 0
                                                                  && SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour)) <= 71)
                                                                  : (SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour)) >= 0 && SqlMethods.DateDiffDay(now, p.RTRN_DATE.Value) <= 29))
                                    //&& (String.IsNullOrEmpty(filter.Country) 
                                    //       || c.country_description == filter.Country)
                             group p by new
                             {
                                 t1 = (String.IsNullOrEmpty(filter.Country) ? p.ReturnLocation.COUNTRy1.country_description :
                                        String.IsNullOrEmpty(filter.CarSegment) ? p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                                        String.IsNullOrEmpty(filter.CarClass) ? p.CAR_GROUP.CAR_CLASS.car_class1 :
                                        p.CAR_GROUP.car_group1),
                                 t2 = (time == Enums.DayActualTime.THREE ?
                                  SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour))
                                  : SqlMethods.DateDiffDay(now, p.RTRN_DATE.Value))
                             }
                                 into g
                                 select new DayActualEntity
                                 {
                                     Label = g.Key.t1,
                                     Tme = g.Key.t2,
                                     Checkin = g.Sum(p => p.RES_ID_NBR != null ? 1 : 0),
                                     OnewayCheckin = g.Sum(p => p.ONEWAY == "Y" ? 1 : 0),
                                     LocalCheckIn = g.Sum(p => p.ONEWAY != "Y" ? 1 : 0)
                                 };

            var offsetData = from p in q
                             //join c in db.COUNTRies on p.COUNTRY equals c.country1
                             where (time == Enums.DayActualTime.THREE
                                ? (SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value)) >= 0
                                && SqlMethods.DateDiffHour(now, p.ArrivalDateTime) >= 0
                                && SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value)) <= 71)
                                : (SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value)) >= 0
                                && SqlMethods.DateDiffDay(now, p.RTRN_DATE.Value) <= 29))
                                //&& (String.IsNullOrEmpty(filter.Country)
                                //            || c.country_description == filter.Country)
                             group p by new
                             {
                                 t1 = (String.IsNullOrEmpty(filter.Country) ? p.ReturnLocation.COUNTRy1.country_description :
                                        String.IsNullOrEmpty(filter.CarSegment) ? p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                                        String.IsNullOrEmpty(filter.CarClass) ? p.CAR_GROUP.CAR_CLASS.car_class1 :
                                        p.CAR_GROUP.car_group1),
                                 t2 = (time == Enums.DayActualTime.THREE ?
                                  SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value))
                                  : SqlMethods.DateDiffDay(now, p.RTRN_DATE.Value))
                             }
                                 into g
                                 select new DayActualEntity
                                 {
                                     Tme = g.Key.t2,
                                     Label = g.Key.t1,
                                     Offset = g.Sum(p => p.RES_ID_NBR != null ? 1 : 0),
                                 };



            var combinedData = from cd in actualData.ToList().Union(offsetData.ToList())
                               group cd by new { cd.Tme, cd.Label }
                                   into gd
                                   select new DayActualEntity
                                   {
                                       Tme = gd.Key.Tme,
                                       Label = gd.Key.Label,
                                       Checkin = gd.Sum(d => d.Checkin),
                                       OnewayCheckin = gd.Sum(d => d.OnewayCheckin),
                                       LocalCheckIn = gd.Sum(d => d.LocalCheckIn),
                                       Offset = gd.Sum(d => d.Offset)
                                   };

            var returned = combinedData.ToList();
            return returned;
        }

        public List<DayActualEntity> GetCheckInReservationsOffsetForAlerts(IQueryable<Reservation> q, PoolingDataClassesDataContext db, IMainFilterEntity filter)
        {
            
            var now = DateTime.Now.GetDateAndHourOnlyByCountry(filter.Country);

            var offsetData = from p in q
                             where (SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value)) >= 0
                                    && SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value)) <= 71)
                             group p by new
                             {
                                 Time = SqlMethods.DateDiffHour(now, p.RTRN_DATE.Value.AddHours(p.RTRN_TIME.Value.Hour).AddHours(p.CI_HOURS_OFFSET.Value)),
                                 CarGroup = p.CAR_GROUP.car_group1,
                                 Location = p.ReturnLocation.location1
                             }
                                 into g
                                 select new DayActualEntity
                                 {
                                     Label = g.Key.Location + " " + g.Key.CarGroup,
                                     Tme = g.Key.Time,
                                     Offset = g.Sum(p => p.RES_ID_NBR != null ? 1 : 0)
                                 };
            var returned = offsetData.ToList();
            return returned;
        }

    }
}