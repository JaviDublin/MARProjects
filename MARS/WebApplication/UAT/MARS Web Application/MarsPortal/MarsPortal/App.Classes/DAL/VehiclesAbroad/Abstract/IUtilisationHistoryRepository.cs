using System;
using System.Collections.Generic;
using App.BLL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;
namespace App.Classes.DAL.VehiclesAbroad.Abstract {
    public interface IUtilisationHistoryRepository {
        IList<IDataTableEntity> getList(IFilterEntity filters);
        IList<IDataTableEntity> getList4Chart();
    }
}
