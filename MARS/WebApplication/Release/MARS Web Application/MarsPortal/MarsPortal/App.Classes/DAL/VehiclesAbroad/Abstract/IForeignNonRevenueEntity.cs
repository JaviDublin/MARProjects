using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Entities.VehiclesAbroad;
using App.BLL.VehiclesAbroad.Abstract;

namespace App.DAL.VehiclesAbroad.Abstract {
    public interface IForeignNonRevenueRepository {
        IList<IDataTableEntity> getList(string country, int daysrev);
    }
}
