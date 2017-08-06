using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using App.UserControls;
using App.DAL.VehiclesAbroad.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;

namespace App.BLL.VehiclesAbroad.Models {
    public class VehicleDetailsUserControlModel : IVehicleDetailsUserControlModel {

        IVehicleDetailsRepository _vdr;
        public VehiclesAbroadDetails VehiclesAbroadDetailsModal { get; set; }

        public VehicleDetailsUserControlModel(IVehicleDetailsRepository vdr) {
            _vdr = vdr;
        }
        public bool visible {
            get { return VehiclesAbroadDetailsModal.UpdatePanelVehicleDetailsModal.Visible; }
            set { VehiclesAbroadDetailsModal.UpdatePanelVehicleDetailsModal.Visible = value; }
        }
        public void bind(string License) {
            VehiclesAbroadDetailsModal.setTable(_vdr.getVehicleDetail(License));
            visible = true;
        }
    }
}