using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.VehiclesAbroad.Controllers.Abstract;
using App.BLL.VehiclesAbroad.Controllers;

namespace App.Site.VehiclesAbroad.Overview {
    public partial class FleetOverview2 : System.Web.UI.Page {
        IFleetOverviewController Controller {
            get {
                if (Session["IFleetOverviewController"] == null) Session["IFleetOverviewController"] = new FleetOverviewController();
                return (IFleetOverviewController)Session["IFleetOverviewController"];
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            App.Classes.BLL.Workers.VehiclesAbroadWorker.addCSS(this.Page);
            Controller.ThreeCascadeModel.DueCountryModel.FilterDropDownList = DropDownListDueCountry;
            Controller.ThreeCascadeModel.DueCountryModel.FeedbackLabel = labelDueCountry;
            Controller.ThreeCascadeModel.PoolModel.FilterDropDownList = DropDownListPool;
            Controller.ThreeCascadeModel.PoolModel.FeedbackLabel = labelPool;
            Controller.ThreeCascadeModel.LocationGroupModel.FilterDropDownList = DropDownListLocationGroup;
            Controller.ThreeCascadeModel.LocationGroupModel.FeedbackLabel = labelLocationGroup;
            Controller.OwnCountryModel.FilterDropDownList = DropDownListOwnCountry;
            Controller.OwnCountryModel.FeedbackLabel = labelOwnCounty;
            Controller.CarSegmentModel.FilterDropDownList = DropDownListCarSegment;
            Controller.CarSegmentModel.FeedbackLabel = labelCarSegment;
            Controller.CarClassModel.FilterDropDownList = DropDownListCarClass;
            Controller.CarClassModel.FeedbackLabel = labelCarClass;
            Controller.CarGroupModel.FilterDropDownList = DropDownListCarGroup;
            Controller.CarGroupModel.FeedbackLabel = labelCarGroup;
            Controller.VehiclePredicamentModel.FilterDropDownList = DropDownListVehiclePredicament;
            Controller.VehiclePredicamentModel.FeedbackLabel = labelVehiclePredicament;
            Controller.DataTableModel.DataTable = DataTableFleetOverview;
            Controller.Initialise(Page);
        }
        protected void DropDownListDueCountry_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.DueCountrySelected();
        }
        protected void DropDownListPool_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.PoolSelected();
        }
        protected void DropDownListOwnCountry_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.OwnCountrySelected();
        }
        protected void DropDownListCarSegment_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.CarSegmentSelected();
        }
        protected void DropDownListCarClass_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.CarClassSelected();
        }
        protected void UpdateController(object sender, EventArgs e) {
            Controller.UpdateView();
        }
    }
}