using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;
using System.Data.Linq;

namespace App.DAL.VehiclesAbroad.Queryables {
    public class CarFilterQueryable {

        public IQueryable<ICarSearchDataEntity> getQueryable(ICarFilterEntity cf, DataContext db, IQueryable<ICarSearchDataEntity> q) {
            int mileage = Int32.TryParse(cf.Mileage, out mileage) ? mileage : -1;
            return from p in q
                   where (p.Unit.Contains(cf.Unit) || cf.Unit == "" || cf.Unit == null)
                          && (p.License.Contains(cf.License) || cf.License == "" || cf.License == null)
                          && (p.Model.Contains(cf.Model) || cf.Model == "" || cf.Model == null)
                          && (p.Moddesc.Contains(cf.ModelDesc) || cf.ModelDesc == "" || cf.ModelDesc == null)
                          && (p.Serial.Contains(cf.Vin) || cf.Vin == "" || cf.Vin == null)
                          && (p.Driver.Contains(cf.Name) || cf.Name == "" || cf.Name == null)
                          && (p.Colour.Contains(cf.Colour) || cf.Colour == "" || cf.Colour == null)
                          && (p.Lstmlg > mileage || mileage == -1)
                   select p;
        }
    }
}