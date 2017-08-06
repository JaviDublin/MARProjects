using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Pooling.Abstract;
using System.Data;

namespace App.Classes.DAL.Reservations.Abstract {
    public interface IReservationDayActualsRepository {
        DataTable _DataTable { get; set; }
        DataTable GetTable(IMainFilterEntity mfe);
        string GetJavascript(params string[] s);
    }
}