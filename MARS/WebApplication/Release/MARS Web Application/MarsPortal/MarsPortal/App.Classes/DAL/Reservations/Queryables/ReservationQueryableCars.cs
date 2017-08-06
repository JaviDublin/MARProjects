using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;

namespace App.Classes.DAL.Reservations.Queryables {
    public class ReservationQueryableCars {
        public IQueryable<Mars.App.Classes.DAL.MarsDBContext.Reservations> getQueryable(MarsDBDataContext db, IMainFilterEntity f, IQueryable<Mars.App.Classes.DAL.MarsDBContext.Reservations> q)
        {
            return from p in q
                // Car details
                join carGp in db.CAR_GROUPs on p.GR_INCL_GOLDUPGR equals carGp.car_group_id
                join carCs in db.CAR_CLASSes on carGp.car_class_id equals carCs.car_class_id
                join carS in db.CAR_SEGMENTs on carCs.car_segment_id equals carS.car_segment_id
                where (f.CarSegment == carS.car_segment1 || string.IsNullOrEmpty(f.CarSegment))
                      && (f.CarClass == carCs.car_class1 || string.IsNullOrEmpty(f.CarClass))
                      && (f.CarGroup == carGp.car_group1 || string.IsNullOrEmpty(f.CarGroup))
                select p;
        }
    }
}