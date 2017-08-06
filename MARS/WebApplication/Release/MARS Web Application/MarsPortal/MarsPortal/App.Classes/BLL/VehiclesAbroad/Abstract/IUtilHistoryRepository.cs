using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Entities.VehiclesAbroad;

namespace App.BLL.VehiclesAbroad.Abstract {

    public interface IUtilHistoryRepository {

        IList<IDataTableEntity> getList(string owningCountry, DateTime startDate, DateTime endDate);

        IList<IDataTableEntity> getList(string destinationCountry, string owningCountry, DateTime startDate, DateTime endDate);

        IList<IDataTableEntity> getList(IList<UtilisationHistoryEntity> l);
    }
}
