using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Classes.Entities.Reservations.Abstract;

namespace App.Classes.DAL.Reservations.Abstract {
    public interface IReservationLogRepository {
        IReservationDBUpdateEntity getItem();
    }
}
