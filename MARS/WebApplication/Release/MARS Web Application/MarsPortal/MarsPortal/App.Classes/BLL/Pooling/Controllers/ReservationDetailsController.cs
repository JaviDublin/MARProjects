using System.Collections.Generic;
using System.IO;
using App.BLL;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.Classes.DAL.Reservations;
using App.Classes.BLL.Pooling.Models;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.Classes.DAL.Pooling;
using System;
using App.UserControls.Pooling;
using App.Classes.Entities.Pooling;
using Dundas.Charting.WebControl;
using Mars.App.Classes.DAL.Pooling.Queryables;
using Mars.Pooling.Controllers.Abstract;
using Mars.Entities.Pooling;
using System.Web.UI;
using App.Classes.DAL.Pooling.Abstract;
using Mars.DAL.Reservations;
using Mars.DAL.Pooling;
using Mars.Pooling.Services.Abstract;
using Mars.Pooling.Services;
using App.DAL.VehiclesAbroad.Filters;
using System.Diagnostics;
using Mars.Pooling.Models;

namespace Mars.Pooling.Controllers
{
    public class ReservationDetailsController : PoolingController
    {

        private readonly IFilterService _mainFilterService;
        public readonly string Textboxpostbackname = "theTextBoxCallback";
        public ReservationDetailsModal Modal { get; set; }
        public IFilterModel2 CheckInOutFilterModel { get; set; }
        public IFilterModel2 FilterModel { get; set; }
        public ITextFilterModel ResIdTextFilterModel { get; set; }
        public ITextFilterModel NameTextFilterModel { get; set; }
        public ITextFilterModel CdpTextFilterModel { get; set; }
        public ITextFilterModel GoldTextFilterModel { get; set; }
        public ITextFilterModel FlightNbrTextFilterModel { get; set; }
        public IDateRangeModel DateRangeModel { get; set; }
        public IFilterModel PagerMaxModel { get; set; }
        public System.Web.UI.WebControls.Label TotalsLabel { get; set; }
        IResTopicFilterService _resTopicFilterService;

        
        public ReservationDetailsController()
            : base()
        {
            PagerMaxModel = new PagerMaxRowsModel(new GridViewMaxRowsRepository());
            CheckInOutFilterModel = new FilterModel2(new CheckInOutRepository());
            FilterModel = new FilterModel2(new FilterRepository());
            GridViewModel = new ReservationGridViewModel(new ResDetailsRepository(), new DoPostBackRepository());
            GridViewModel.ResFilters = new ResevationDetailsFilterEntity();
            GridViewModel.MainFilters = new MainFilterEntity();
            _mainFilterService = new ResMainFilterService(GridViewModel, CmsOpsModel, CarCascadeModel);
            _resTopicFilterService = new ResTopicFilterService(FilterModel);
        }
        public override void Initialise(Page p)
        {
            base.Initialise(p);
            bool dateSet = false;
            UpdateStatistic(ReportStatistics.ReportName.PoolingReservations);
            if (p.Request.Params["__EVENTTARGET"] == Textboxpostbackname)
            {
                UpdateView();
            }
            if (!String.IsNullOrEmpty(p.Session["ResFilterDropDownValue"] as string))
            {
                _resTopicFilterService.SetTopic((p.Session["ResFilterDropDownValue"] as string));
                var resDataClicked = new DateTime(long.Parse(p.Session["ResDateClicked"].ToString()));
                p.Session["ResFilterDropDownValue"] = null;
                p.Session["ResDateClicked"] = null;
                DateRangeModel.StartDateModel.Text = resDataClicked.ToShortDateString();
                DateRangeModel.EndDateModel.Text = resDataClicked.ToShortDateString();
                dateSet = true;
            }

            if (p.IsPostBack) return;
            Modal.Visible = false;
            CheckInOutFilterModel.FirstItem = "";
            CheckInOutFilterModel.bind();
            FilterModel.bind();
            FilterModel.FilterDropDownList.Items.RemoveAt(0);
            PagerMaxModel.bind();
            if (ComparisonTopicSelected != string.Empty)
            {
                FilterModel.SelectedValue = ComparisonTopicSelected;
                ComparisonTopicSelected = null;
            }
            HeadingModel.setText(Enums.Headers.reservationDetails);

            int timeSlot;
            int.TryParse(p.Request.Params["q1"], out timeSlot);
            DateTime lastRequestDate;
            if (!DateTime.TryParse(p.Request.Params["q3"], out lastRequestDate))
            {
                lastRequestDate = DateTime.Now;
            }
            Enums.DayActualTime dayActualTime;
            Enum.TryParse(p.Request.Params["q4"], out dayActualTime);

            if (!dateSet)
            {
                SetStartAndEndDate(lastRequestDate, timeSlot, dayActualTime);    
            }
            

            GetSession();
            UpdateView();
        }

        private void SetStartAndEndDate(DateTime lastRequestDate, int timeSlotSelected, Enums.DayActualTime dayActualTime)
        {
            lastRequestDate = dayActualTime == Enums.DayActualTime.THIRTY
                ? lastRequestDate.AddDays(timeSlotSelected)
                : lastRequestDate.AddHours(timeSlotSelected);

            DateRangeModel.StartDateModel.Text = lastRequestDate.ToShortDateString();
            DateRangeModel.EndDateModel.Text = lastRequestDate.ToShortDateString();
        }

        public override void UpdateView()
        {
            setFeedback();
            setFilters();
            LabelUpdateModel.Update();
            if (!string.IsNullOrEmpty(PagerMaxModel.SelectedValue))
                GridViewModel.GridViewer.PageSize = Convert.ToInt32(PagerMaxModel.SelectedValue);
            GridViewModel.MainFilters.ExcludeLongterm = ExcludeLongterm;
            GridViewModel.bind();
            var specificModel = GridViewModel as ReservationGridViewModel;
            if (specificModel != null)
            {
                TotalsLabel.Text = string.Format("{0} Reservations", specificModel.DataItemsCount);
            }
                
        }

        public List<ResGridDisplay> GetDataForExcel()
        {
            var gvm = GridViewModel as ReservationGridViewModel;
            if (gvm == null) return null;
            return gvm.GetGridDataToBind();
        }

        protected override void setFeedback()
        {
            base.setFeedback();
            CheckInOutFilterModel.SetFeedback();
            FilterModel.SetFeedback();
            DateRangeModel.SetFeedback();
        }
        void setFilters()
        {
            _mainFilterService.FillFilter();
            GridViewModel.ResFilters.CheckInOut = CheckInOutFilterModel.SelectedValue;
            GridViewModel.ResFilters.Filter = FilterModel.SelectedValue;
            GridViewModel.ResFilters.StartDate = DateRangeModel.StartDate;
            GridViewModel.ResFilters.EndDate = DateRangeModel.EndDate;
            GridViewModel.ResFilters.ResId = ResIdTextFilterModel.Text;
            GridViewModel.ResFilters.CustomerName = NameTextFilterModel.Text;
            GridViewModel.ResFilters.Cdp = CdpTextFilterModel.Text;
            GridViewModel.ResFilters.Gold1 = GoldTextFilterModel.Text;
            GridViewModel.ResFilters.FlightNbr = FlightNbrTextFilterModel.Text;
        }
        public override void GridViewSelectRowSelected(int rowIndex)
        {
            Modal.SetReservationTable(GridViewModel.GetModalDetails(rowIndex));
            // Modal.SetTable(GridViewModel.GetModalDetails(rowIndex));
        }
        public void StartDateSelected()
        {
            DateRangeModel.SetDates(Enums.SelectedDates.startDate);
            UpdateView();
        }
        public void EndDateSelected()
        {
            DateRangeModel.SetDates(Enums.SelectedDates.endDate);
            UpdateView();
        }
    }
}