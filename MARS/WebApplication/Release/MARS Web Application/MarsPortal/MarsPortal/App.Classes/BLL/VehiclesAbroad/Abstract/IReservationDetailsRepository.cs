using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Entities.VehiclesAbroad;

namespace App.BLL.VehiclesAbroad.Abstract {

    public interface IReservationDetailsRepository {

        /// <summary>
        /// get the vehicle details based on the reservation number
        /// </summary>
        ResVehiclesEntity getDetails(string resNo);
    }
}
