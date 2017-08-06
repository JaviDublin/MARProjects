using App.BLL;
using App.Classes.BLL.Pooling.Models;
using App.Classes.DAL.Pooling;
using System.Web.UI;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.Pooling.Models;
using Mars.Pooling.Controllers.Abstract;
using Mars.Entities.Pooling;
using Mars.Pooling.Models.Abstract;
using Mars.DAL.Pooling;
using Mars.BLL.Pooling.Models;
using System;
using System.Web.UI.WebControls;
using System.Text;
using App.Classes.Entities.Pooling.Abstract;
using System.Web.UI.HtmlControls;
using Mars.Pooling.Services;
using Mars.Pooling.Services.Abstract;
using MarsPortal;
using System.Collections.Generic;
using System.Threading;

namespace Mars.Pooling.Controllers
{
    public class DayActualController : PoolingController
    {

        private String STATUSTYPE = "statusType";
        private String INFOTAG = "info";
        private String THREE = "three", THIRTY = "thirty";

        private Enums.Headers _headers; // dictates the type of page ie thirty day or three day
        public IButtonModel2 SwitchButton { get; set; }
        public IButtonModel2 ChartButton { get; set; }
        public ActualsModel ActualsGridModel { get; set; }
        public IBrowserParamsModel BrowserModel { get; set; }
        public Label ForeignCarsInCountry { get; set; }
        private LabelStatusModel _labelODCollectionsModel;

        public LabelStatusModel labelODCollectionsModel
        {
            get { return _labelODCollectionsModel; }
            set
            {
                _labelODCollectionsModel = Global.CastleContainer.Resolve<LabelStatusModel>("labelODCollectionsModel");
            }
        }

        private LabelStatusModel _labelODOpentripsModel;

        public LabelStatusModel LabelODOpentripsModel
        {
            get { return _labelODOpentripsModel; }
            set { _labelODOpentripsModel = Global.CastleContainer.Resolve<LabelStatusModel>("LabelODOpentripsModel"); }
        }

        public IPoolingChartModel ChartModel { get; set; }
        public HiddenField HiddenGridCrouchingChart { get; set; }
        public HtmlGenericControl GridviewPanel { get; set; }
        public HtmlGenericControl ChartviewPanel { get; set; }
        private Int32 _noOfCols;
        private static Int32 MINCOLS = 30, MAXCOLS = 72;
        public AlertsBreakdownModel _alertBreakdownModel { get; set; }
        private readonly IFilterService _mainFilterService;


        public DayActualController()
            : base()
        {
            _noOfCols = MINCOLS;
            ChartModel = new PoolingChartModel();
            ActualsGridModel = new ActualsModel();
            ActualsGridModel.MainFilters = new MainFilterEntity();
            _mainFilterService = new DayActualMainFilterService(ActualsGridModel, CmsOpsModel, CarCascadeModel);
        }

        public override void Initialise(Page p)
        {

            if (p.Request.Params["__EVENTTARGET"] == "FromFind")
                p.Response.Redirect(
                    new RedirectModel(_headers == Enums.Headers.thirtyDayActualStatus ? THIRTY : THREE,
                        "SiteComparison.aspx").RedirectString());
            if (p.Request.Params["__EVENTTARGET"] == "FromReservation")
            {
                var parametersOnClick = p.Request.Params["__EVENTARGUMENT"].Split(',');
                p.Session["ResFilterDropDownValue"] = parametersOnClick[0];
                p.Session["ResDateClicked"] = parametersOnClick[3];
                p.Response.Redirect("ReservationDetails.aspx");
            }
            base.Initialise(p);
            BrowserModel.SetJavaScript(p);
            UpdateStatistic(ReportStatistics.ReportName.PoolingStatus);
            if (p.IsPostBack)
            {
                return;
            }
            loadParticulars(p.Request.QueryString[STATUSTYPE] ?? "");
            ActualsGridModel.setJavascript(p);
            labelODCollectionsModel.Filter = ActualsGridModel.MainFilters;
            LabelODOpentripsModel.Filter = ActualsGridModel.MainFilters;
            GetSession();
            fromAlerts(p.Request.QueryString[INFOTAG] ?? "");
            UpdateView();
            ChartviewPanel.Visible = false;
        }

        private void fromAlerts(string p)
        {
            if (string.IsNullOrEmpty(p)) return;
            IMainFilterEntity Filter = _alertBreakdownModel.GetBreakdown(p, CmsOpsModel.isCMS);
            CmsOpsModel.CountryFilterModel.FilterDropDownList.SelectedValue = Filter.Country;
            CmsOpsModel.GeneralThreeFilterModel.TopModel.bind(Filter.Country);
            CmsOpsModel.GeneralThreeFilterModel.TopModel.FilterDropDownList.SelectedValue = Filter.PoolRegion;
            CmsOpsModel.GeneralThreeFilterModel.MiddleModel.bind(Filter.PoolRegion, Filter.Country);
            CmsOpsModel.GeneralThreeFilterModel.MiddleModel.FilterDropDownList.SelectedValue = Filter.LocationGrpArea;
            CmsOpsModel.GeneralThreeFilterModel.BottomModel.bind(Filter.LocationGrpArea, Filter.PoolRegion,
                Filter.Country);
            CmsOpsModel.CountryFilterModel.FeedbackLabel.Text = Filter.Country;
            CmsOpsModel.GeneralThreeFilterModel.TopModel.FeedbackLabel.Text = Filter.PoolRegion;
            CmsOpsModel.GeneralThreeFilterModel.MiddleModel.FeedbackLabel.Text = Filter.LocationGrpArea;
            CmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedValue = Filter.Branch;
            CmsOpsModel.GeneralThreeFilterModel.BottomModel.FeedbackLabel.Text = Filter.Branch;
            CarCascadeModel.TopModel.bind(Filter.Country);
            CarCascadeModel.TopModel.FilterDropDownList.SelectedValue = Filter.CarSegment;
            CarCascadeModel.TopModel.FeedbackLabel.Text = Filter.CarSegment;
            CarCascadeModel.MiddleModel.bind(Filter.Country, Filter.CarSegment);
            CarCascadeModel.MiddleModel.FilterDropDownList.SelectedValue = Filter.CarClass;
            CarCascadeModel.MiddleModel.FeedbackLabel.Text = Filter.CarClass;
            CarCascadeModel.BottomModel.bind(Filter.Country, Filter.CarSegment, Filter.CarClass);
            CarCascadeModel.BottomModel.FilterDropDownList.SelectedValue = Filter.CarGroup;
            CarCascadeModel.BottomModel.FeedbackLabel.Text = Filter.CarGroup;
            SetSession();
        }

        private void loadParticulars(string p)
        {
            if (p.ToLower() == THIRTY)
            {
                _headers = Enums.Headers.thirtyDayActualStatus;
                ActualsGridModel.Mode = _headers;
                SwitchButton.Repository = new ButtonModelRepository(Enums.buttons.ThreeDayActual);
                _noOfCols = MINCOLS;
            }
            else
            {
                _headers = Enums.Headers.threeDayActualStatus;
                ActualsGridModel.Mode = _headers;
                SwitchButton.Repository = new ButtonModelRepository(Enums.buttons.ThirtyDayActual);
                _noOfCols = MAXCOLS;
            }
        }

        public override void UpdateView()
        {
            setFeedback();
            _mainFilterService.FillFilter();
            LabelUpdateModel.Update();
            HeadingModel.setText(_headers);
            SwitchButton.setLabel();
            labelODCollectionsModel.Update();
            LabelODOpentripsModel.Update();
            ActualsGridModel.MainFilters.ExcludeLongterm = ExcludeLongterm;
            ActualsGridModel.bind(BrowserModel.BrowserWidth.Value, _noOfCols.ToString());
            ChartModel._DataTable = ActualsGridModel._htmlTable.Repository._DataTable;
            ChartModel.NoOfPoints = _noOfCols;
            ChartModel.bind();

        }



        public HtmlGenericControl GetHtmlTable()
        {
            return ActualsGridModel._HtmlControl;
        }

        public void onSwitchButtonClicked()
        {
            if (_headers == Enums.Headers.thirtyDayActualStatus) loadParticulars(THREE);
            else loadParticulars(THIRTY);
            UpdateView();
        }
        public void CXGridChart()
        {
            if (HiddenGridCrouchingChart.Value == "0")
            {
                GridviewPanel.Visible = true;
                ChartviewPanel.Visible = false;
                ChartButton._Button.Text = "Switch to Chart";
                HiddenGridCrouchingChart.Value = "1";
            }
            else
            {
                GridviewPanel.Visible = false;
                ChartviewPanel.Visible = true;
                HiddenGridCrouchingChart.Value = "0";
                ChartButton._Button.Text = "Switch to Grid";
            }
            UpdateView();
        }
        public void ChartClick(object o, System.Web.UI.WebControls.ImageMapEventArgs e)
        {
            ChartModel.ChartClick(o, e);
            UpdateView();
        }
    }
}
