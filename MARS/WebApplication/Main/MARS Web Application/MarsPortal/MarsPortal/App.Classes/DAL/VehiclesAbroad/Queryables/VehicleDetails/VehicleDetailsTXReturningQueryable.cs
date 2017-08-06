using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using App.DAL.VehiclesAbroad.Abstract;
using System.Data.Linq;

namespace App.DAL.VehiclesAbroad.Queryables {
    public class VehicleDetailsTXReturningQueryable {

        public IQueryable<ICarSearchDataEntity> getQueryable(IFilterEntity f, DataContext db, IQueryable<ICarSearchDataEntity> q) {

            return from p in q
                   where p.OwnCountry != p.Lstwwd.Substring(0, 2)
                   && p.OwnCountry == p.Duewwd.Substring(0, 2)
                   && (p.OnRent != 1)
                   && (p.Overdue != 1)
                   && (string.IsNullOrEmpty(f.DueCountry) || f.DueCountry.Equals(p.Lstwwd.Substring(0, 2)))
                   select p;
        }
    }
}