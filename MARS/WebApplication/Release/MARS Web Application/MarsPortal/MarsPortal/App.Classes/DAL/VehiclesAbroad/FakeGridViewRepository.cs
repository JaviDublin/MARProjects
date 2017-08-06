using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using App.DAL.VehiclesAbroad.Abstract;
using System.Data.Linq;
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad {
    public class FakeGridViewRepository : IVehicleDetailsRepository {


        public IList<ICarSearchDataEntity> getQueryable(IFilterEntity filters, ICarFilterEntity cf, string sortExpression = "", string sortDirection = "") {
            IList<ICarSearchDataEntity> l = new List<ICarSearchDataEntity>();
            for (int i = 0; i < 100; i++)
                l.Add(new CarSearchDataEntity {
                    License = "A000" + i,
                    Lstwwd = "GE00" + i,
                    OwnCountry = "FR",
                    Pool = i.ToString(),
                    LocGroup = i.ToString(),
                    Vc = i.ToString(),
                    Unit = i.ToString(),
                    Model = "Tank" + i,
                    Moddesc = "Sherman" + i,
                    Driver = "Number " + i,
                    Colour = "Red",
                    Lstmlg = i
                });
            return l;
        }


        public ICarSearchDataEntity getVehicleDetail(string License) {
            throw new NotImplementedException();
        }
    }
}