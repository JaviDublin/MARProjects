using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;
using System.Data.Linq;
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Queryables {
    public class VehiclesFilterQueryable {

        public IQueryable<ICarSearchDataEntity> getQueryable(IFilterEntity filters, MarsDBDataContext db, IQueryable<ICarSearchDataEntity> q) {
            return from p in q
                   where (p.Pool == filters.Pool || filters.Pool == "" || filters.Pool == null)
                          && (p.LocGroup == filters.Location || filters.Location == "" || filters.Location == null)
                          && (p.Vc == filters.CarGroup || filters.CarGroup == "" || filters.CarGroup == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               where cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.Vc
                               select cc.car_class1).Contains(filters.CarClass) || filters.CarClass == "" || filters.CarClass == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               from cs in db.CAR_SEGMENTs
                               where cs.car_segment_id == cc.car_segment_id
                               && cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.Vc
                               select cs.car_segment1).Contains(filters.CarSegment) || filters.CarSegment == "" || filters.CarSegment == null)
                          && (p.Nonrev >= filters.nonRev) // non Revenue selection
                          && (p.Mt.Contains(filters.MoveType) || string.IsNullOrEmpty(filters.MoveType))
                          && (p.Op == filters.Operstat
                                || string.IsNullOrEmpty(filters.Operstat)
                                || (filters.Operstat == "Shop" ? p.Op == "BD" || p.Op == "MM" || p.Op == "TW" : false)
                                || (filters.Operstat == "Other" ? p.Op != "RT" && p.Op != "BD" && p.Op != "MM" && p.Op != "TW" : false)
                             )
                          && (filters.OwnCountry == p.OwnCountry || string.IsNullOrEmpty(filters.OwnCountry))
                   select p;
        }
    }
}