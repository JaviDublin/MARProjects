﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using App.DAL.VehiclesAbroad.Abstract;
using System.Data.Linq;

namespace App.DAL.VehiclesAbroad.Queryables {

    public class VehicleDetailsAllQueryable {

        public IQueryable<ICarSearchDataEntity> getQueryable(IFilterEntity f, DataContext db, IQueryable<ICarSearchDataEntity> q) {

            return from p in q
                   where p.OwnCountry != p.Lstwwd.Substring(0, 2)
                   && (string.IsNullOrEmpty(f.DueCountry) || f.DueCountry.Equals(p.Lstwwd.Substring(0, 2)))
                   select p;
        }
    }
}