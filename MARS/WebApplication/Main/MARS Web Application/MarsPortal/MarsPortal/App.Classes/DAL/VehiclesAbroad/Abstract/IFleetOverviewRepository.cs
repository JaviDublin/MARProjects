using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;

namespace App.DAL.VehiclesAbroad.Abstract {
    public interface IFleetOverviewRepository {
        IList<IDataTableEntity> getList(IFilterEntity filters);
    }
}
