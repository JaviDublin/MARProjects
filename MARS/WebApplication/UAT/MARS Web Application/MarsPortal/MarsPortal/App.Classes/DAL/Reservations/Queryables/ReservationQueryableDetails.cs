using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;

namespace App.Classes.DAL.Reservations.Queryables {
    public class ReservationQueryableDetails {
        
        public IQueryable<Mars.App.Classes.DAL.MarsDBContext.Reservations> getQueryable(MarsDBDataContext db, IReservationDetailsFilterEntity f, IQueryable<Mars.App.Classes.DAL.MarsDBContext.Reservations> q)
        {
            return from p in q
                   where (f.CheckInOut == "Check In" ? (p.RTRN_DATE >= f.StartDate && p.RTRN_DATE <= f.EndDate) : (p.RS_ARRIVAL_DATE >= f.StartDate && p.RS_ARRIVAL_DATE <= f.EndDate))
                   && (string.IsNullOrEmpty(f.ResId) || p.RES_ID_NBR.Contains(f.ResId))
                   && (string.IsNullOrEmpty(f.CustomerName) || p.CUST_NAME.Contains(f.CustomerName))
                   && (string.IsNullOrEmpty(f.Cdp) || p.CDPID_NBR.Contains(f.Cdp))
                   && (string.IsNullOrEmpty(f.Gold1) || p.NO1_CLUB_GOLD.Contains(f.Gold1))
                   && (string.IsNullOrEmpty(f.FlightNbr) || p.FLIGHT_NBR.Contains(f.FlightNbr))
                   select p;
        }
    }
}