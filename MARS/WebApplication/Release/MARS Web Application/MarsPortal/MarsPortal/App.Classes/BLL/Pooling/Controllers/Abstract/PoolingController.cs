using System;
using System.Web.UI.HtmlControls;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.Classes.BLL.Pooling.Models;
using App.Classes.DAL.Pooling.Abstract;
using App.DAL.VehiclesAbroad.Filters;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.Pooling.Models;
using App.Classes.DAL.Reservations;
using System.Web.UI;
using Mars.DAL.Pooling.Abstract;
using App.Classes.DAL.Pooling.Filters;
using Mars.DAL.Pooling.Filters;
using App.BLL;
using System.Data.SqlClient;

using Rad.Web;

namespace Mars.Pooling.Controllers.Abstract
{
    abstract public class PoolingController : IPoolingController
    {
        private Page _page;
        static String POOLINGFILTERS = "poolingfilters";
        static int NOOFFILTERS = 8;
        IThreeFilterRepository _cmsRepository;
        IThreeFilterRepository _opsRepository;
        public IReservationGridViewModel GridViewModel { get; set; }
        public IThreeFilterCascadeModel CarCascadeModel { get; set; }
        public ICmsOpsLogicModel CmsOpsModel { get; set; }
        public Label HeadingLabel { get; set; }
        public LabelUpdateDBModel LabelUpdateModel { get; set; }
        public IJavaScriptModel JavascriptModel { get; set; }
        public IHeadingModel HeadingModel { get; set; }

        public bool ExcludeLongterm { get; set; }

    
        private const string ComparisonTopicSessionHolder = "ComparisonTopicSessionHolder";

        

        protected string ComparisonTopicSelected
        {
            get { return _page.Session[ComparisonTopicSessionHolder] == null ? string.Empty : _page.Session[ComparisonTopicSessionHolder].ToString(); }
            set { _page.Session[ComparisonTopicSessionHolder] = value; }
        }


        public PoolingController()
        {
            CarCascadeModel = new ThreeFilterCascadeModel(new CarSegRepository(), new CarClsRepository(), new CarGrpRepository());
            _cmsRepository = new ThreeFilterRepository(new DAL.Pooling.Filters.PoolRepository(), new LocGrpRepository(), new BranchRepository());
            _opsRepository = new ThreeFilterRepository(new RegionsRepository(), new AreasRepository(), new BranchOPSRepository());
            CmsOpsModel = new CmsOpsLogicModel(new CountryRepository(), _cmsRepository, _opsRepository);
        }
        public virtual void Initialise(Page p)
        {
            _page = p;
            loadCSS(p);
            if (p.IsPostBack) return;
            JavascriptModel.SetServiceReference(p);
            JavascriptModel.SetJavaScriptService(p);
            CmsOpsModel.Initialise(p);
            CarCascadeModel.bind(CmsOpsModel.CountryFilterModel.SelectedValue);
        }
        public void loadCSS(System.Web.UI.Page p)
        {
            HtmlLink myHtmlLink = new HtmlLink();
            myHtmlLink.Href = "~/App.Styles/Pooling.css";
            myHtmlLink.Attributes.Add("rel", "stylesheet");
            myHtmlLink.Attributes.Add("type", "text/css");
            if (p.Header != null) p.Header.Controls.Add(myHtmlLink);
        }
        public virtual void UpdateView()
        {
            throw new NotImplementedException("This virtual method is not implemented in the abstraction");
        }
        protected virtual void setFeedback()
        {
            CarCascadeModel.BottomModel.SetFeedback();
            CmsOpsModel.GeneralThreeFilterModel.BottomModel.SetFeedback();
        }
        public virtual void CountrySelected()
        {
            CmsOpsModel.CountryFilterModel.SetFeedback();
            CarCascadeModel.SuperSelected(CmsOpsModel.CountryFilterModel.SelectedValue);
            CmsOpsModel.GeneralThreeFilterModel.SuperSelected(CmsOpsModel.CountryFilterModel.SelectedValue);
            SetSession();
            UpdateView();
        }
        public virtual void CarSegmentSelected()
        {
            CarCascadeModel.TopSelected(CmsOpsModel.CountryFilterModel.SelectedValue, CarCascadeModel.TopModel.SelectedValue);
            SetSession();
            UpdateView();
        }
        public virtual void CarClassSelected()
        {
            CarCascadeModel.MiddleSelected(CmsOpsModel.CountryFilterModel.SelectedValue, CarCascadeModel.TopModel.SelectedValue, CarCascadeModel.MiddleModel.SelectedValue);
            SetSession();
            UpdateView();
        }
        public virtual void PoolSelected()
        {
            CmsOpsModel.GeneralThreeFilterModel.TopSelected(CmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedValue, CmsOpsModel.CountryFilterModel.SelectedValue);
            SetSession();
            UpdateView();
        }

        public void BranchSelected()
        {
            SetSession();
        }

        public void CarGroupSelected()
        {
            SetSession();
        }

        public virtual void LocationGroupSelected()
        {
            CmsOpsModel.GeneralThreeFilterModel.MiddleSelected(CmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedValue
                    , CmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedValue
                    , CmsOpsModel.CountryFilterModel.SelectedValue);
            SetSession();
            UpdateView();
        }


        public virtual void CmsLogicSelected()
        {
            SetSession();
            CmsOpsModel.SetToCms();
            UpdateView();
        }
        public virtual void OpsLogicSelected()
        {
            CmsOpsModel.SetToOps();
            SetSession();
            UpdateView();
        }
        public virtual void GridViewPageSelected(int pageIndex)
        {
            GridViewModel.GridViewer.PageIndex = pageIndex;
            UpdateView();
        }
        public virtual void GridViewSortSelected(string sortExpression, string sortDirection)
        {
            GridViewModel.SortExpression(sortExpression);
            UpdateView();
        }
        public virtual void GridViewSelectRowSelected(int rowIndex)
        {
            throw new NotImplementedException();
        }
        protected void SetSession()
        {
            int[] x = new int[NOOFFILTERS];
            x[0] = CmsOpsModel.CountryFilterModel.FilterDropDownList.SelectedIndex;
            x[1] = CmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedIndex;
            x[2] = CmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedIndex;
            x[3] = CmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedIndex;
            x[4] = Convert.ToInt32(CmsOpsModel.isCMS);
            x[5] = CarCascadeModel.TopModel.SelectedIndex;
            x[6] = CarCascadeModel.MiddleModel.SelectedIndex;
            x[7] = CarCascadeModel.BottomModel.SelectedIndex;
            _page.Session[POOLINGFILTERS] = x;
        }
        protected bool GetSession()
        {
            if (_page.Session[POOLINGFILTERS] == null) return false;
            int[] x = (int[])_page.Session[POOLINGFILTERS];
            if (Convert.ToBoolean(x[4])) CmsOpsModel.SetToCms(); else CmsOpsModel.SetToOps();
            CmsOpsModel.CountryFilterModel.rebind(x[0]);
            CmsOpsModel.GeneralThreeFilterModel.rebind(x[1], x[2], x[3], CmsOpsModel.CountryFilterModel.SelectedValue);
            CarCascadeModel.CascadeRebind(x[5], x[6], x[7], CmsOpsModel.CountryFilterModel.SelectedValue);
            return true;
        }
        public void UpdateStatistic(ReportStatistics.ReportName reportName)
        {
            string country = CmsOpsModel.CountryFilterModel.GetCode();
            int poolId = CmsOpsModel.GeneralThreeFilterModel.TopModel.GetId();
            int locationId = CmsOpsModel.GeneralThreeFilterModel.MiddleModel.GetId();
            var branch = CmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedValue;
            try
            {
                ReportStatistics.InsertStatistics((int)reportName
                    , country, poolId, locationId, -1, -1, branch, Users.GetUserRACFID(), DateTime.Now);
            }
            catch (SqlException ex)
            {
                //ILog _logger = log4net.LogManager.GetLogger("Pooling");
                //if (_logger != null) _logger.Error(" SQL Exception thrown in PoolingController(Abstract) accessing ReportStatistics.InsertStatistics, message : " + ex.Message);
            }
        }
    }
}