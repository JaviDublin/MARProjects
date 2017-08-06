using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.DAL.VehiclesAbroad.Abstract {
    public interface IReservationBalanceRepository {
        DataTable getTable(int noOfDays);
    }
}
