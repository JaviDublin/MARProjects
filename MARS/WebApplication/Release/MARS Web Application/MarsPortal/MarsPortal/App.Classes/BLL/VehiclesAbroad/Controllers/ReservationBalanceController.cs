using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.VehiclesAbroad.Controllers.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.Classes.DAL.VehiclesAbroad.Filters;
using App.BLL.VehiclesAbroad.Models;
using App.Classes.DAL.VehiclesAbroad;
using App.Classes.BLL.VehiclesAbroad.Models.Abstract;
using App.Classes.BLL.VehiclesAbroad.Models;

namespace App.Classes.BLL.VehiclesAbroad.Controllers {
    public class ReservationBalanceController : IReservationBalanceController {

        public ReservationBalanceController() {
            NoOfDaysModel = new NonRevFilterModel(new NoOfDaysRepository());
            NoOfDaysModel.FirstItem = "";
            _GridViewModel = new GridViewReservationBalanceModel(new ReservationBalanceRespository());
        }
        public IFilterModel NoOfDaysModel { get; set; }
        public IGridViewReservationBalanceModel _GridViewModel { get; set; }

        public void UpdateView() {
            _GridViewModel.bind(Convert.ToInt32(NoOfDaysModel.SelectedValue));
        }
        public void Initialise(System.Web.UI.Page p) {
            if (p.IsPostBack) return;
            NoOfDaysModel.bind();
            UpdateView();
        }
    }
}