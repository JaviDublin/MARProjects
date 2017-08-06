using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Classes.BLL.VehiclesAbroad.Controllers.Abstract;
using App.Classes.BLL.VehiclesAbroad.Controllers;

namespace App.Site.VehiclesAbroad.Reservations {
    public partial class ReservationBalance2 : System.Web.UI.Page {

        IReservationBalanceController _controller {
            get {
                if (Session["IReservationBalanceController"] == null) Session["IReservationBalanceController"] = new ReservationBalanceController();
                return (IReservationBalanceController)Session["IReservationBalanceController"];
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            _controller._GridViewModel._GridView = GridViewReservationBalance;
            _controller.NoOfDaysModel.FilterDropDownList = DropDownListNoOfDays;
            _controller.Initialise(Page);
        }
        protected void DropDownListNoOfDays_SelectedIndexChanged(object sender, EventArgs e) {
            _controller.UpdateView();
        }
        protected void GridViewReservationBalance_RowDataBound(object sender, GridViewRowEventArgs e) {
            _controller._GridViewModel.bindRows(e);
        }
    }
}