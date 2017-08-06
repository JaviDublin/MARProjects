using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Entities.VehiclesAbroad;
using App.Classes.Entities.VehiclesAbroad.Abstract;

namespace App.Classes.DAL.VehiclesAbroad.Abstract {
    public interface IFleetReservationRepository {
        List<FleetMatchEntity> getList(IFilterEntity f, string license, string sortExpression);
    }
}
