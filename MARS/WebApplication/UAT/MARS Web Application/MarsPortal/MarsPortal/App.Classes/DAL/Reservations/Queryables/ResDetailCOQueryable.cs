using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.DAL.Reservations.Queryables
{
    public class ResDetailCOQueryable
    {
        public IQueryable<Reservation> GetQueryable(IQueryable<Reservation> q, IReservationDetailsFilterEntity rdfe, IMainFilterEntity filter)
        {

            var now = DateTime.Now.GetDateAndHourOnlyByCountry(filter.Country);
            var returned = from p in q
                   where (p.RS_ARRIVAL_DATE >= rdfe.StartDate && p.RS_ARRIVAL_DATE <= rdfe.EndDate)
                   && SqlMethods.DateDiffHour(now, p.RS_ARRIVAL_DATE.Value.AddHours(p.RS_ARRIVAL_TIME.Value.Hour))
                       >= 0
                   select p;
            return returned;

        }
    }
}