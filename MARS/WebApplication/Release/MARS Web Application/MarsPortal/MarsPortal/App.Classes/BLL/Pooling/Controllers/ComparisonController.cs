using System;
using Mars.Pooling.Controllers.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;
using Mars.Pooling.Models.Abstract;
using Mars.Pooling.Services.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using App.Classes.BLL.Pooling.Models;
using Mars.DAL.Pooling.Filters;
using Mars.Pooling.Models;
using Mars.DAL.Pooling;
using Rad.Web;

namespace Mars.Pooling.Controllers
{
    public abstract class ComparisonController : PoolingController
    {
        protected String THREE = "three", THIRTY = "thirty";
        private Enums.Headers _header;
        public IFilterModel2 TopicDropDownList { get; set; }
        public ICompGridModel DatagridModel { get; set; }
        public IButtonModel2 SwitchBtn { get; set; }
        public IBrowserParamsModel BrowserModel { get; set; }
        protected IFilterService _mainFilterService;
        protected Enums.Headers[] headers;

        

        public ComparisonController()
            : base()
        {
            headers = new Enums.Headers[2];
            TopicDropDownList = new FilterModel2(new TopicRepository());
            BrowserModel = new BrowserParamsModel(new BrowserJavascriptRepository());
        }
        public override void Initialise(System.Web.UI.Page p)
        {
            
            base.Initialise(p);
            BrowserModel.SetJavaScript(p);
            if (p.Request.Params["__EVENTTARGET"] == "Balance_Clicked")
            {
                processBalanceClicked(p.Request.Params["__EVENTARGUMENT"], true); return;
            }
            if (p.Request.Params["__EVENTTARGET"] == "FromCompTable")
            {
                if(!TopicDropDownList.SelectedValue.Contains("Reservations")) return;
                string[] s = p.Request.Params["__EVENTARGUMENT"].Split(',');
                
                processBalanceClicked(s[2],false,true);
                
                ComparisonTopicSelected = TopicDropDownList.SelectedValue;
                
                               
                p.Response.Redirect(new RedirectModel(p.Request.Params["__EVENTARGUMENT"], "ReservationDetails.aspx").RedirectString());
            }
            if (p.IsPostBack) return;
            loadParticulars("");
            TopicDropDownList.FirstItem = String.Empty; // removes: '*** All ***' item
            TopicDropDownList.SelectedIndex = 9; 
            TopicDropDownList.bind();
            
        }

        protected virtual void processBalanceClicked(string p, bool countryClicked = false, bool sessionOnly = false)
        {
            throw new NotImplementedException("This method need to be implemented in the inherent classes.");
        }

        public override void UpdateView()
        {
            LabelUpdateModel.Update();
            DatagridModel._HtmlTable.Filter.ExcludeLongterm = ExcludeLongterm;
            
            _mainFilterService.FillFilter();
            HeadingModel.setText(_header);
            DatagridModel.Bind(BrowserModel.BrowserWidth.Value);

            setFeedback();

        }
        protected void loadParticulars(string p)
        {
            if (p.ToLower() == THIRTY)
            {
                _header = headers[0];
                DatagridModel.Mode = _header;
                SwitchBtn._Text = "Switch to 3 Day Actuals";
            }
            else
            {
                _header = headers[1];
                DatagridModel.Mode = _header;
                SwitchBtn._Text = "Switch to 30 Day Actuals";
            }
        }
        public void SwitchBtnClicked()
        {
            if (_header == headers[0]) loadParticulars(THREE);
            else loadParticulars(THIRTY);
            UpdateView();
        }
    }
}