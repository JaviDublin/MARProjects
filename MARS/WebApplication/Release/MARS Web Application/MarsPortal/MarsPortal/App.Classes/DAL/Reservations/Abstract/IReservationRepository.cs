using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;
using Mars.Entities.Reservations.Abstract;

namespace App.Classes.DAL.Reservations.Abstract {
    public interface IReservationRepository {
        IList<IReservationDetailsEntity> getList(IMainFilterEntity mfe, IReservationDetailsFilterEntity rdfe, string sortExpression, string sortDirection);
        IReservationDetailsEntity getItem(string resId);
    }
}
