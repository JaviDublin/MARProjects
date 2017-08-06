using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.VehiclesAbroad.Controllers.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.DAL.VehiclesAbroad.Filters;
using App.Classes.DAL.VehiclesAbroad.Filters;
using App.Classes.BLL.VehiclesAbroad.Models;
using App.Classes.DAL.VehiclesAbroad;
using System.Web.UI.WebControls;

namespace App.Classes.BLL.VehiclesAbroad.Controllers {
    public class ReservationAnalysisController : IReservationAnalysisController {

        public IFilterModel DropDownListRtrnCountry { get; set; }
        public IFilterModel DropDownListReservationCountry { get; set; }
        public IFilterModel DropDownNoOfDays { get; set; }
        public IGridviewModel Gridview { get; set; }

        public ReservationAnalysisController() {
            DropDownListReservationCountry = new FilterModel(new ReservationCountryRepository());
            DropDownListRtrnCountry = new FilterModel(new ReservationReturnCountryRepository());
            DropDownNoOfDays = new NonRevFilterModel(new NoOfDaysRepository());
            DropDownNoOfDays.FirstItem = "";
            Gridview = new ReservationAnalysisGridViewModel(new ReservationAnalysisRepository());
        }
        public void Initialise(System.Web.UI.Page p) {
            if (p.IsPostBack) return;
            DropDownListReservationCountry.bind();
            DropDownListRtrnCountry.bind();
            DropDownNoOfDays.bind();
            updateView();
        }
        public void updateView() {
            Gridview.bind(DropDownListReservationCountry.SelectedValue, DropDownListRtrnCountry.SelectedValue, DropDownNoOfDays.SelectedValue);
        }
        public void calcGridviewTotals(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e) {
            // Row data bound handle is used to high light the totals row

            // check its of type datarow
            if (e.Row.RowType == DataControlRowType.DataRow) {
                if (e.Row.Cells[0].Text.Equals("Totals")) {
                    e.Row.BackColor = System.Drawing.Color.Black;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Font.Bold = true;
                }
            }
        }
    }
}