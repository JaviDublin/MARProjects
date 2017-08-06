using System;
using App.UserControls;

namespace App.BLL.VehiclesAbroad.Models.Abstract {
    public interface IVehicleDetailsUserControlModel {
        void bind(string License);
        bool visible { get; set; }
        VehiclesAbroadDetails VehiclesAbroadDetailsModal { get; set; }
    }
}
