using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Controllers.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.BLL.VehiclesAbroad.Abstract;
using App.DAL.VehiclesAbroad.Filters;
using App.BLL.VehiclesAbroad.Models;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.Classes.DAL.VehiclesAbroad;
using App.Entities.VehiclesAbroad;
using App.Classes.BLL.VehiclesAbroad.Models;
using App.Classes.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.BLL.VehiclesAbroad.Controllers {
    public class UtilisationHistoryController : IUtilisationHistoryController {

        public IFilterModel CountryFilter { get; set; }
        public ITextFilterModel StartDateFilterModel { get; set; }
        public ITextFilterModel EndDateFilter { get; set; }
        public IDataTableModel DataTableModel { get; set; }
        IFilterEntity _filter;
        public IChartModel ChartModel { get; set; }

        public UtilisationHistoryController() {
            CountryFilter = new FilterModel(new CountryDescriptionRepository());
            StartDateFilterModel = new TextFilterModel();
            EndDateFilter = new TextFilterModel();
            IUtilisationHistoryRepository uhr = new UtilisationHistoryRepository();
            DataTableModel = new UtilisationDataTableModel(uhr);
            DataTableModel.ColumnHeader = @"Date/Country";
            _filter = new FilterEntity();
            ChartModel = new ChartModel(uhr);
        }
        public void UpdateView() {
            _filter.OwnCountry = CountryFilter.SelectedValue;
            checkPageData();
            _filter.ReservationEndDate = DateTime.Parse(EndDateFilter.Text);
            _filter.ReservationStartDate = DateTime.Parse(StartDateFilterModel.Text);
            DataTableModel.Filter = _filter;
            DataTableModel.bind();
            ChartModel.bind();
        }
        public void Initialise(System.Web.UI.Page p) {
            string z = p.Request["__EVENTTARGET"] ?? "";
            if (z == "date") UpdateView();
            if (p.IsPostBack) return;
            CountryFilter.bind();
            EndDateFilter.Text = DateTime.Now.ToShortDateString();
            StartDateFilterModel.Text = DateTime.Now.AddDays(-6).ToShortDateString();
            UpdateView();
        }
        void checkPageData() {
            DateTime _endDate = new DateTime();
            DateTime _startDate = new DateTime();
            string endMessage = "";
            string startMessage = "";

            if (!DateTime.TryParse(EndDateFilter.Text, out _endDate)) {
                _endDate = DateTime.Now.Date;
                endMessage = "The end date is not in the correct format.";
            }
            if (_endDate > DateTime.Now.Date) {
                _endDate = DateTime.Now.Date;
                endMessage = "The end date can't be set to a day after today.";
            }

            if (!DateTime.TryParse(StartDateFilterModel.Text, out _startDate)) {
                _startDate = _endDate.AddDays(-6).Date;
                startMessage = "The start date is not in the correct format.";
            }
            if (_startDate > _endDate) {
                _startDate = _endDate.Date;
                startMessage = "The start date can't be before the end date.";
            }
            // set the textboxes and label to the appropriate values
            EndDateFilter.Text = _endDate.ToShortDateString();
            EndDateFilter.ErrorText = endMessage;
            StartDateFilterModel.Text = _startDate.ToShortDateString();
            StartDateFilterModel.ErrorText = startMessage;
        }
    }
}