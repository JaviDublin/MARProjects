using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.VehiclesAbroad.Controllers.Abstract;
using App.BLL.VehiclesAbroad.Controllers;

namespace MarsV2.VehiclesAbroad {
    public partial class ForeignNonRevenue2 : System.Web.UI.Page {
        IForeignNonRevenueController Controller {
            get {
                if (Session["IForeignNonRevenueController"] == null) Session["IForeignNonRevenueController"] = new ForeignNonRevenueController();
                return (IForeignNonRevenueController)Session["IForeignNonRevenueController"];
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            App.Classes.BLL.Workers.VehiclesAbroadWorker.addCSS(this.Page);
            Controller.DataTableModel.DataTable = gridNonRevenue;
            Controller.NonRevDaysModel.FilterDropDownList = DropDownListDays;
            Controller.OwnCountryModel.FilterDropDownList = DropDownListCountries;
            Controller.Initialise(Page);
        }
        protected void DropDownListCountries_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.UpdateView();
        }
        protected void DropDownListDays_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.UpdateView();
        }
    }
}