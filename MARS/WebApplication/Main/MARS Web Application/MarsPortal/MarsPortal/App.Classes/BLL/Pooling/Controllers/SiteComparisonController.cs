using App.BLL;
using System.Web.UI;
using App.Classes.DAL.Pooling.Abstract;
using Mars.Pooling.Models;
using Mars.Pooling.Services;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Mars.Pooling.Controllers
{
    public class SiteComparisonController : ComparisonController
    {
        public SiteComparisonController()
            : base()
        {
            headers[0] = Enums.Headers.thirtySiteComparison;
            headers[1] = Enums.Headers.threeSiteComparison;
            DatagridModel = new SiteCompGridModel();
            _mainFilterService = new CompMainFilterService(DatagridModel, CmsOpsModel, CarCascadeModel, TopicDropDownList);
        }
        public override void Initialise(Page p)
        {
            base.Initialise(p);
            UpdateStatistic(ReportStatistics.ReportName.PoolingSiteComaprison);
            if (p.IsPostBack) return;
            //CmsOpsModel.GeneralThreeFilterModel.BottomModel.FilterDropDownList.Visible = false;
            GetSession();
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
            else if (CmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedIndex == 0)
            {
                CmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedValue = s;
                CmsOpsModel.GeneralThreeFilterModel.MiddleModel.bind(s, CmsOpsModel.CountryFilterModel.SelectedValue);
                CmsOpsModel.GeneralThreeFilterModel.TopModel.FeedbackLabel.Text = s;
            }
            else if (CmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedIndex == 0)
            {
                CmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedValue = s;
                CmsOpsModel.GeneralThreeFilterModel.BottomModel.bind(CmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedValue
                    , CmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedValue
                    , CmsOpsModel.CountryFilterModel.SelectedValue
                    , s);
                CmsOpsModel.GeneralThreeFilterModel.MiddleModel.FeedbackLabel.Text = s;
            }
            else if (CmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedIndex == 0)
            {
                CmsOpsModel.GeneralThreeFilterModel.BottomModel.bind(CmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedValue
                        , CmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedValue
                        , CmsOpsModel.CountryFilterModel.SelectedValue, s);
                CmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedValue = s;
                CmsOpsModel.GeneralThreeFilterModel.BottomModel.FeedbackLabel.Text = s;
            }


            if (!sessionOnly)
            {
                UpdateView();    
            }
            
            SetSession();
        }

    }
}