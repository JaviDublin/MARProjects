using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;
using App.Classes.BLL.VehiclesAbroad.Models.Abstract;
using App.Classes.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.BLL.VehiclesAbroad.Models {
    public class GridViewReservationBalanceModel : IGridViewReservationBalanceModel {
        private IReservationBalanceRepository _repository;

        public GridViewReservationBalanceModel(IReservationBalanceRepository r) {
            _repository = r;
        }
        public GridView _GridView { get; set; }
        public void bind(int days) {
            _GridView.DataSource = _repository.getTable(days);
            _GridView.DataBind();
        }
        public void bindRows(GridViewRowEventArgs e) {
            // check its of type datarow
            if (e.Row.RowType == DataControlRowType.DataRow) {

                // check for the totals row 
                if (e.Row.Cells[0].Text.Equals("Totals")) {

                    // change the rows attributes
                    e.Row.BackColor = System.Drawing.Color.Black;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Font.Bold = true;
                }
            }
        }
    }
}