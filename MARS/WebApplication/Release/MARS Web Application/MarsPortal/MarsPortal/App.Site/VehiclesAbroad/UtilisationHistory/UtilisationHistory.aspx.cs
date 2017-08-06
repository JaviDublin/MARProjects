using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.VehiclesAbroad.Controllers.Abstract;
using App.Classes.BLL.VehiclesAbroad.Controllers;


namespace App.Site.VehiclesAbroad.UtilisationHistory {
    public partial class UtilisationHistory2 : System.Web.UI.Page {

        IUtilisationHistoryController Controller {
            get {
                if (Session["IUtilisationHistoryController"] == null) Session["IUtilisationHistoryController"] = new UtilisationHistoryController();
                return (IUtilisationHistoryController)Session["IUtilisationHistoryController"];
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            Controller.ChartModel._Chart = Chart1;
            Controller.CountryFilter.FilterDropDownList = DropDownListCountry;
            Controller.DataTableModel.DataTable = dataTableUtilisation;
            Controller.StartDateFilterModel._TextBox = textBoxStartDate;
            Controller.StartDateFilterModel.ErrorLabel = labelErrorStartDate;
            Controller.EndDateFilter._TextBox = textboxEndDate;
            Controller.EndDateFilter.ErrorLabel = labelErrorEndDate;
            Controller.Initialise(Page);
        }
        protected void UpdateView(object sender, EventArgs e) {
            Controller.UpdateView();
        }
    }
}