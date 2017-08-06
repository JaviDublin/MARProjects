using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using App.Classes.Entities.Pooling.Abstract;
using App.DAL.VehiclesAbroad.Abstract;

namespace Mars.DAL.Reservations.Queryables
{
    public class ResDetailsBottomFilterQueryable
    {
        
        public ResDetailsBottomFilterQueryable(IFilterRepository r)
        {
        
        }
        public IQueryable<Reservation> GetQueryable(IQueryable<Reservation> q, IReservationDetailsFilterEntity f)
        {
            var returned = from p in q
                           where (p.RES_ID_NBR.Contains(f.ResId) || string.IsNullOrEmpty(f.ResId))
                           && (p.CUST_NAME.Contains(f.CustomerName) || string.IsNullOrEmpty(f.CustomerName))
                           && (p.CDPID_NBR.Contains(f.Cdp) || string.IsNullOrEmpty(f.Cdp))
                           && (p.NO1_CLUB_GOLD.Contains(f.Gold1) || string.IsNullOrEmpty(f.Gold1))
                           && (p.FLIGHT_NBR.Contains(f.FlightNbr) || string.IsNullOrEmpty(f.FlightNbr))
                           && (f.Filter == "Oneway Reservations" ? p.ONEWAY == "Y" : f.Filter == "Gold Service Reservations" ? p.GS == "Y" : f.Filter != "Prepaid Reservations" || p.PREPAID == "Y")
                           select p;
            
            return returned;
        }
    }
}