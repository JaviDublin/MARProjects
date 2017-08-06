using System;
using System.Web.UI.WebControls;
namespace App.Classes.BLL.VehiclesAbroad.Models.Abstract {
    public interface IGridViewReservationBalanceModel {
        GridView _GridView { get; set; }
        void bind(int days);
        void bindRows(GridViewRowEventArgs e);
    }
}
