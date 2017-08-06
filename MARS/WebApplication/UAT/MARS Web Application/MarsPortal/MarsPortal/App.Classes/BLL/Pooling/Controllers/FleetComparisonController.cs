using System.Web.UI.WebControls;
using App.BLL;
using App.Classes.DAL.Pooling.Abstract;
using System.Web.UI;
using App.Classes.Entities.Pooling.Abstract;
using App.Entities;
using Mars.Pooling.Models;
using Mars.Pooling.Services;
using System.Web.UI.HtmlControls;

namespace Mars.Pooling.Controllers
{
    public class FleetComparisonController : ComparisonController
    {

        public AlertsBreakdownModel _alertBreakdownModel { get; set; }

        public FleetComparisonController()
            : base()
        {
            headers[0] = Enums.Headers.thirtyFleetComparison;
            headers[1] = Enums.Headers.threeFleetComparison;
            DatagridModel = new FleetCompGridModel();
            _mainFilterService = new CompMainFilterService(DatagridModel, CmsOpsModel, CarCascadeModel, TopicDropDownList);
        }
        public override void Initialise(Page p)
        {
            base.Initialise(p);
            UpdateStatistic(ReportStatistics.ReportName.PoolingFleetComparison);
            if (p.IsPostBack) return;
            CarCascadeModel.BottomModel.FilterDropDownList.Visible = false;
            GetSession();
            FromAlerts(p.Request.QueryString["info"] ?? "");
            UpdateView();

        }

        protected override void processBalanceClicked(string s, bool countryClicked = false, bool sessionOnly = false)
        {

            if (CmsOpsModel.CountryFilterModel.SelectedIndex == 0)
            {
                CmsOpsModel.CountryFilterModel.SelectedValue = s;
                CmsOpsModel.CountryFilterModel.FeedbackLabel.Text = s;
                CarCascadeModel.SuperSelected(s);
                CmsOpsModel.GeneralThreeFilterModel.SuperSelected(s);
            }
            else if (CarCascadeModel.TopModel.SelectedIndex == 0)
            {
                CarCascadeModel.TopModel.SelectedValue = s;
                CarCascadeModel.TopModel.FeedbackLabel.Text = s;
                CarCascadeModel.MiddleModel.bind(CmsOpsModel.CountryFilterModel.SelectedValue, s);
                
            }
            else if (CarCascadeModel.MiddleModel.SelectedIndex == 0)
            {
                CarCascadeModel.MiddleModel.SelectedValue = s;
                CarCascadeModel.MiddleModel.FeedbackLabel.Text = s;
                CarCascadeModel.BottomModel.bind(CmsOpsModel.CountryFilterModel.SelectedValue,
                    CarCascadeModel.TopModel.SelectedValue, s);
                
            }
            else if (CarCascadeModel.BottomModel.SelectedIndex == 0)
            {
                //CarCascadeModel.BottomModel.SelectedValue = s;
                //CarCascadeModel.BottomModel.FeedbackLabel.Text = s;
                //CarCascadeModel.BottomModel.bind(CmsOpsModel.CountryFilterModel.SelectedValue
                //    , CarCascadeModel.TopModel.SelectedValue, CarCascadeModel.MiddleModel.SelectedValue, s);
            }
            

            //if (CmsOpsModel.CountryFilterModel.SelectedIndex == 0 && CarCascadeModel.TopModel.SelectedIndex == 0)
            //{
            //    CarCascadeModel.TopModel.bind(CmsOpsModel.CountryFilterModel.SelectedValue);    
            //}
            if (!sessionOnly)
            {
                UpdateView();
            }
            

            SetSession();
        }

        private void FromAlerts(string p)
        {
            if (string.IsNullOrEmpty(p)) return;
            var filter = _alertBreakdownModel.GetBreakdown(p, CmsOpsModel.isCMS);
            CmsOpsModel.CountryFilterModel.FilterDropDownList.SelectedValue = filter.Country;
            CmsOpsModel.GeneralThreeFilterModel.TopModel.bind(filter.Country);
            CmsOpsModel.GeneralThreeFilterModel.TopModel.FilterDropDownList.SelectedValue = filter.PoolRegion;
            CmsOpsModel.GeneralThreeFilterModel.MiddleModel.bind(filter.PoolRegion, filter.Country);
            CmsOpsModel.GeneralThreeFilterModel.MiddleModel.FilterDropDownList.SelectedValue = filter.LocationGrpArea;
            CmsOpsModel.GeneralThreeFilterModel.BottomModel.bind(filter.LocationGrpArea, filter.PoolRegion, filter.Country);
            CmsOpsModel.CountryFilterModel.FeedbackLabel.Text = filter.Country;
            CmsOpsModel.GeneralThreeFilterModel.TopModel.FeedbackLabel.Text = filter.PoolRegion;
            CmsOpsModel.GeneralThreeFilterModel.MiddleModel.FeedbackLabel.Text = filter.LocationGrpArea;
            CmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedValue = filter.Branch;
            CmsOpsModel.GeneralThreeFilterModel.BottomModel.FeedbackLabel.Text = filter.Branch;
            CarCascadeModel.TopModel.bind(filter.Country);
            CarCascadeModel.TopModel.FilterDropDownList.SelectedValue = filter.CarSegment;
            CarCascadeModel.TopModel.FeedbackLabel.Text = filter.CarSegment;
            CarCascadeModel.MiddleModel.bind(filter.Country, filter.CarSegment);
            CarCascadeModel.MiddleModel.FilterDropDownList.SelectedValue = filter.CarClass;
            CarCascadeModel.MiddleModel.FeedbackLabel.Text = filter.CarClass;

            //CarCascadeModel.BottomModel.bind(Filter.Country, Filter.CarSegment, Filter.CarClass);
            //CarCascadeModel.BottomModel.FilterDropDownList.SelectedValue = Filter.CarGroup;
            //CarCascadeModel.BottomModel.FeedbackLabel.Text = Filter.CarGroup;
            TopicDropDownList.SelectedValue = "Balance";
            SetSession();
        }

    }
}