using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Controllers.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.DAL.VehiclesAbroad.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.DAL.VehiclesAbroad.Filters;
using App.Entities.VehiclesAbroad;
using System.Web.UI.HtmlControls;
using App.BLL.VehiclesAbroad.Models;
using App.DAL.VehiclesAbroad;
using App.BLL.VehiclesAbroad.Models.Filters.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.Classes.BLL.Pooling.Models;

namespace App.BLL.VehiclesAbroad.Controllers {
    public class FleetOverviewController : IFleetOverviewController {
        public IFilterModel2 OperstatModel { get; set; }
        public IFilterModel2 MoveTypeModel { get; set; }
        public IThreeCascadeFilterModel ThreeCascadeModel { get; set; }
        public IFilterModel2 OwnCountryModel { get; set; }
        public IFilterModel2 CarSegmentModel { get; set; }
        public IFilterModel2 CarClassModel { get; set; }
        public IFilterModel2 CarGroupModel { get; set; }
        public IFilterModel2 VehiclePredicamentModel { get; set; }
        public IFilterModel2 NonRevDaysModel { get; set; }
        public IDataTableModel DataTableModel { get; set; }
        IFilterEntity _filter;
        System.Web.UI.Page _page;

        public FleetOverviewController() {
            ThreeCascadeModel = new ThreeCascadeFilterModel(new FleetEuropeActualDueCountriesRepository(), new PoolRepository(), new LocationRepository());
            OwnCountryModel = new FilterModel2(new FleetActualEuropeOwnCountriesRepository());
            CarSegmentModel = new FilterModel2(new CarSegmentRepository());
            CarClassModel = new FilterModel2(new CarClassRepository());
            CarGroupModel = new FilterModel2(new CarGroupRepository());
            VehiclePredicamentModel = new FilterModel2(new VehiclePredicamentRepository());
            DataTableModel = new DataTableModel(new FleetOverviewRepository());
            _filter = new FilterEntity();
        }
        void setFeedback() {
            ThreeCascadeModel.DueCountryModel.SetFeedback();
            ThreeCascadeModel.PoolModel.SetFeedback();
            ThreeCascadeModel.LocationGroupModel.SetFeedback();
            OwnCountryModel.SetFeedback();
            CarSegmentModel.SetFeedback();
            CarClassModel.SetFeedback();
            CarGroupModel.SetFeedback();
            VehiclePredicamentModel.SetFeedback();
        }
        public void DueCountrySelected() {
            ThreeCascadeModel.DueCountrySelected();
            UpdateView();
        }
        public void PoolSelected() {
            ThreeCascadeModel.PoolSelected();
            UpdateView();
        }
        public void OwnCountrySelected() {
            if (OwnCountryModel.SelectedIndex <= 0) CarSegmentModel.clear();
            else CarSegmentModel.bind(OwnCountryModel.SelectedValue);
            CarClassModel.clear();
            CarGroupModel.clear();
            UpdateView();
        }
        public void CarSegmentSelected() {
            if (CarSegmentModel.SelectedIndex <= 0) CarClassModel.clear();
            else CarClassModel.bind(OwnCountryModel.SelectedValue, CarSegmentModel.SelectedValue);
            CarGroupModel.clear();
            UpdateView();
        }
        public void CarClassSelected() {
            if (CarClassModel.SelectedIndex <= 0) CarGroupModel.clear();
            else CarGroupModel.bind(OwnCountryModel.SelectedValue, CarSegmentModel.SelectedValue, CarClassModel.SelectedValue);
            UpdateView();
        }
        public void UpdateView() {
            _filter.CarClass = CarClassModel.SelectedValue;
            _filter.CarGroup = CarGroupModel.SelectedValue;
            _filter.CarSegment = CarSegmentModel.SelectedValue;
            _filter.DueCountry = ThreeCascadeModel.DueCountryModel.SelectedValue;
            _filter.OwnCountry = OwnCountryModel.SelectedValue;
            _filter.VehiclePredicament = VehiclePredicamentModel.SelectedIndex;
            _filter.Pool = ThreeCascadeModel.PoolModel.SelectedValue;
            _filter.Location = ThreeCascadeModel.LocationGroupModel.SelectedValue;
            setFeedback();
            DataTableModel.Filter = _filter;
            DataTableModel.ColumnHeader = getColumnHeader();
            DataTableModel.bind();
        }
        public void Initialise(System.Web.UI.Page p) {
            _page = p;
            if (p.IsPostBack) { checkPostback(); return; }
            ThreeCascadeModel.bind();
            OwnCountryModel.bind();
            CarSegmentModel.clear();
            CarClassModel.clear();
            CarGroupModel.clear();
            VehiclePredicamentModel.bind();
            VehiclePredicamentModel.SelectedIndex = 3;
            UpdateView();
        }
        private void checkPostback() {
            // if the postback is from a click from the data table then save session and redirect to the Vehicle details page
            string eventArgument = _page.Request["__EVENTARGUMENT"] ?? ""; // the args are: due country, own country
            string eventTarget = _page.Request["__EVENTTARGET"] ?? "";
            if (eventTarget.Contains("DataTableOverview")) { // Been called from clicking the data table
                ThreeCascadeModel.DueCountryModel.SelectedValue = eventArgument.Split(',')[0];
                OwnCountryModel.SelectedValue = eventArgument.Split(',')[1];
                _page.Response.Redirect("~/VehiclesAbroad/VehicleDetails"); // redirect to Vehicle details page
            }
        }
        private string getColumnHeader() {
            switch (_filter.VehiclePredicament) {
                case 0:
                case 4:
                case 5:
                case 6: return @"In Country\Owning Country";
                default: return @"Destination\Owning Country";
            }
        }
    }
}