using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Classes.BLL.VehiclesAbroad.Controllers.Abstract;
using App.Classes.BLL.VehiclesAbroad.Controllers;

namespace MarsV2.VehiclesAbroad {

    public partial class ReservationAnalysis : System.Web.UI.Page {
        IReservationAnalysisController _controller {
            get {
                if (Session["IReservationAnalysisController"] == null) Session["IReservationAnalysisController"] = new ReservationAnalysisController();
                return (IReservationAnalysisController)Session["IReservationAnalysisController"];
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            _controller.DropDownListReservationCountry.FilterDropDownList = DropDownListReservationCountry;
            _controller.DropDownListRtrnCountry.FilterDropDownList = DropDownListReturnCountry;
            _controller.DropDownNoOfDays.FilterDropDownList = DropDownListNoOfDays;
            _controller.Gridview.GridViewer = GridViewReservationAnalysis;
            _controller.Initialise(Page);
        }
        public void updateController(object sender, EventArgs e) {
            _controller.updateView();
        }
        protected void GridViewReservationAnalysis_RowDataBound(object sender, GridViewRowEventArgs e) {
            _controller.calcGridviewTotals(sender, e);
        }
    }
}