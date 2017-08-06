using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Entities.VehiclesAbroad;

namespace App.BLL.VehiclesAbroad.Abstract {
    public interface IFleetRepository {

        List<CarSearchDataEntity> getFleetData();
        List<CarSearchDataEntity> getFilteredFleetData(FilterEntity filters);
        List<CarSearchDataEntity> getCarFilteredFleetData(CarFilterEntity cf);
        List<CarSearchDataEntity> getSortedFleetData(string sortEcpression);
        List<IDataTableEntity> getGroupedData(int predicament);
    }
}
