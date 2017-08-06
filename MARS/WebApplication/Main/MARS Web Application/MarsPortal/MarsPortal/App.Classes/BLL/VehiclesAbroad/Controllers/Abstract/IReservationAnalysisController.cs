using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Controllers.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;

namespace App.Classes.BLL.VehiclesAbroad.Controllers.Abstract {
    public interface IReservationAnalysisController : IController {
        IFilterModel DropDownListRtrnCountry { get; set; }
        IFilterModel DropDownListReservationCountry { get; set; }
        IFilterModel DropDownNoOfDays { get; set; }
        IGridviewModel Gridview { get; set; }
        void updateView();
        void calcGridviewTotals(object sender, GridViewRowEventArgs e);
    }
}
