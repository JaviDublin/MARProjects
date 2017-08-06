using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using App.DAL.VehiclesAbroad.Abstract;
using System.Data.Linq;
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Queryables {
    public class VehicleDetailsOROwnToForeign {

        public IQueryable<ICarSearchDataEntity> getQueryable(IFilterEntity f, MarsDBDataContext db, IQueryable<ICarSearchDataEntity> q) {

            return from p in q
                   where p.OwnCountry == p.Lstwwd.Substring(0, 2)
                   && p.OwnCountry != ((p.Duewwd == null || p.Duewwd == "") ? p.Lstwwd.Substring(0, 2) : p.Duewwd.Substring(0, 2))
                   && p.OnRent == 1
                   && (string.IsNullOrEmpty(f.DueCountry)
                        || ((p.Duewwd == null || p.Duewwd == "") ?
                        f.DueCountry.Equals(p.Lstwwd.Substring(0, 2)) : f.DueCountry.Equals(p.Duewwd.Substring(0, 2))))
                   select p;
        }
    }
}