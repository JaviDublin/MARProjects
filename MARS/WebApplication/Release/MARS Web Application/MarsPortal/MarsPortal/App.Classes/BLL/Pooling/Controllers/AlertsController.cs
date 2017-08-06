using System;
using Mars.Pooling.Controllers.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using System.Web.UI.HtmlControls;
using Mars.Pooling.HTMLFactories;
using Mars.Pooling.HTMLFactories.Abstract;
using System.Web.UI;
using Mars.BLL.Pooling.Models;
using App.BLL;
using Mars.Pooling.Services;
using Mars.Pooling.Services.Abstract;

namespace Mars.Pooling.Controllers
{

    public class AlertsController : PoolingController
    {

        public AlertGridviewModel GridviewModel;
        private readonly IFilterService _filterService;

        public DateTime CustomDateMax{get; set;}

        public AlertsController()
        {
            GridviewModel = new AlertGridviewModel();
            _filterService = new AlertsMainFilterService(GridviewModel, CmsOpsModel, CarCascadeModel);
        }

        public override void Initialise(Page p)
        {
            base.Initialise(p);
            GridviewModel.SelectedDate = CustomDateMax;
            UpdateStatistic(ReportStatistics.ReportName.PoolingAlerts);
            if (p.IsPostBack) return;
            GetSession();
            HeadingModel.setText(Enums.Headers.alerts);
            UpdateView();
        }

        public override void UpdateView()
        {
            setFeedback();
            LabelUpdateModel.Update();
            
            _filterService.FillFilter();
            GridviewModel._HtmlTable.Filter.ExcludeLongterm = ExcludeLongterm;
            
        }

        public void UpdateGrid()
        {
            GridviewModel._HtmlTable.Filter.ExcludeLongterm = ExcludeLongterm;
            GridviewModel.Bind();   
        }
    }
}