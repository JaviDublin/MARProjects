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
    public class PoolingCheckOutReservationsWithLabels
    {
        public IQueryable<DayActualEntity> GetQueryable(IQueryable<Reservation> q, IMainFilterEntity filter, Enums.DayActualTime time)
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
                              String.IsNullOrEmpty(filter.CarSegment) ? p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                              String.IsNullOrEmpty(filter.CarClass) ? p.CAR_GROUP.CAR_CLASS.car_class1 :
                              p.CAR_GROUP.car_group1),
                       t2 = (time == Enums.DayActualTime.THREE ?
                           SqlMethods.DateDiffHour(now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour)
                        .AddMinutes(p.RS_ARRIVAL_TIME.Value.Minute))
                           : SqlMethods.DateDiffDay(now, p.RS_ARRIVAL_DATE.Value))
                   }
                       into g
                       select new DayActualEntity
                       {
                           Label = g.Key.t1,
                           Tme = g.Key.t2,
                           Reservations = g.Sum(p => p.RES_ID_NBR != null ? 1 : 0),
                           OnewayRes = g.Sum(p => p.ONEWAY == "Y" ? 1 : 0),
                           GoldServiceReservations = g.Sum(p => p.GS == "Y" ? 1 : 0),
                           PrepaidReservations = g.Sum(p => p.PREPAID == "Y" ? 1 : 0),
                           Predelivery = g.Sum(p => p.PREDELIVERY == "Y" ? 1 : 0)
                       };
        }

        public List<DayActualEntity> GetCheckOutReservationsForAlerts(IQueryable<Reservation> q, PoolingDataClassesDataContext db, IMainFilterEntity filter)
        {
            var now = DateTime.Now.GetDateAndHourOnlyByCountry(filter.Country);

            var reservationData = from p in q
                                  where (SqlMethods.DateDiffHour(now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour)) >= 0
                                         && SqlMethods.DateDiffHour(now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour)) <= 71)
                                  group p by new
                                  {
                                      Time = SqlMethods.DateDiffHour(now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour)),
                                      CarGroup = p.CAR_GROUP.car_group1,
                                      Location = p.RentalLocation.location1
                                  }
                                      into g
                                      select new DayActualEntity
                                      {
                                          Label = g.Key.Location + " " + g.Key.CarGroup,
                                          Tme = g.Key.Time,
                                          Reservations = g.Sum(p => p.RES_ID_NBR != null ? 1 : 0)
                                      };

            var returned = reservationData.ToList();
            return returned;
        }
    }
}