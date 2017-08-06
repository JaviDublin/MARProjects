using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;



namespace DAL.VehiclesAbroad.Queryables.Reservation
{
    public class ReservationMatchCarClassQueryable : Queryable<Reservations>
    {

        public override IQueryable<Reservations> GetQueryable(MarsDBDataContext db, params string[] s)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Reservations> GetQueryable(MarsDBDataContext db, IQueryable<Reservations> q, params string[] s)
        {
            if (string.IsNullOrEmpty(s[0])) return q;
            return from p in q
                // Car details
                join carGp in db.CAR_GROUPs on p.GR_INCL_GOLDUPGR equals carGp.car_group_id
                join carCs in db.CAR_CLASSes on carGp.car_class_id equals carCs.car_class_id
                join carS in db.CAR_SEGMENTs on carCs.car_segment_id equals carS.car_segment_id
                where s[0] == carCs.car_class1
                select p;
        }
    }
}