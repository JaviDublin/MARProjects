using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace App.Classes.DAL.VehiclesAbroad.Abstract {
    public interface IReservationAnalysisRepository {
        DataTable getDataTable(string reservationCountry, string rtrnCountry, int noOfDays);
    }
}
