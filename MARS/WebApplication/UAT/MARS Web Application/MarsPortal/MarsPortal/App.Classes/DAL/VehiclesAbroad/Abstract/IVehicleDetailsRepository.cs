using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Entities.VehiclesAbroad;
using App.Entities.VehiclesAbroad;
using System.Data.Linq;
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Abstract {
    public interface IVehicleDetailsRepository {

        IList<ICarSearchDataEntity> getQueryable(IFilterEntity filters, ICarFilterEntity cf, string sortExpression, string sortDirection);

        ICarSearchDataEntity getVehicleDetail(string License);
    }
}
