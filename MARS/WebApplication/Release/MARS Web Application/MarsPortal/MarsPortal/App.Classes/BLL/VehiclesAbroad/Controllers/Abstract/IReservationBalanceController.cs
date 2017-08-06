using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.BLL.VehiclesAbroad.Controllers.Abstract;
using App.Classes.BLL.VehiclesAbroad.Models.Abstract;

namespace App.Classes.BLL.VehiclesAbroad.Controllers.Abstract {
    public interface IReservationBalanceController : IController {
        IFilterModel NoOfDaysModel { get; set; }
        IGridViewReservationBalanceModel _GridViewModel { get; set; }
        void UpdateView();
    }
}
