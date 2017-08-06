using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Controllers.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.DAL.VehiclesAbroad.Filters;
using App.DAL.VehiclesAbroad;
using App.BLL.VehiclesAbroad.Models;
using App.Entities.VehiclesAbroad;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.BLL.VehiclesAbroad.Controllers {
    public class ForeignNonRevenueController : IForeignNonRevenueController {
        private System.Web.UI.Page _page;

        public IDataTableModel DataTableModel { get; set; }
        public IFilterModel NonRevDaysModel { get; set; }
        public IFilterModel OwnCountryModel { get; set; }
        public string Condition { get; private set; }
        public string OwnCountry { get; private set; }
        public string DueCountry { get; private set; }
        ICountryDescriptionRepository _country;

        public ForeignNonRevenueController() {
            NonRevDaysModel = new NonRevFilterModel(new NonrevRepository());
            OwnCountryModel = new FilterModel(new FleetActualEuropeOwnCountriesRepository());
            DataTableModel = new DataTableModel(new ForeignNonRevenueRepository());
            DataTableModel.Filter = new FilterEntity();
            _country = new CountryDescriptionRepository();
        }
        public void UpdateView() {
            DataTableModel.Filter.OwnCountry = OwnCountryModel.SelectedValue;
            DataTableModel.Filter.nonRev = Convert.ToInt32(NonRevDaysModel.SelectedValue);
            DataTableModel.bind();
        }
        public void Initialise(System.Web.UI.Page p) {
            _page = p;
            if (p.IsPostBack) { checkPostBack(); return; }
            NonRevDaysModel.FirstItem = "";
            NonRevDaysModel.bind();
            OwnCountryModel.bind();
            DataTableModel.bind();
        }
        private void checkPostBack() {
            string eventArgument = _page.Request["__EVENTARGUMENT"] ?? ""; // the args are: due country, own country
            string eventTarget = _page.Request["__EVENTTARGET"] ?? "";
            if (eventTarget.Contains("DataTableOverview")) { // Been called from clicking the data table
                DueCountry = _country.getCountryDescription(eventArgument.Split(',')[0]);
                OwnCountry = _country.getCountryDescription((eventArgument.Split(',')[1]).Split('_')[0]);
                Condition = (eventArgument.Split(',')[1]).Split('_')[1];
                _page.Response.Redirect("~/VehiclesAbroad/VehicleDetails"); // redirect to Vehicle details page
            }
        }
    }
}