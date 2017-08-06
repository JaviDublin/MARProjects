using System;
using System.Text;
using Mars.Pooling.Controllers.Abstract;
using Mars.Pooling.Controllers;
using System.Web.UI.WebControls;

namespace App.Site.Pooling
{
    public partial class ThreeDayActuals : System.Web.UI.Page
    {
        readonly DayActualController _controller = MarsPortal.Global.CastleContainer.Resolve<DayActualController>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cbRemoveLongterm.Checked = Session["PoolingExcludeLongTerm"] != null && (bool)Session["PoolingExcludeLongTerm"];
            }

            _controller.CmsOpsModel.CountryFilterModel.FeedbackLabel = TopFeedback1.labelCountry;
            _controller.CmsOpsModel.CountryFilterModel.FilterDropDownList = BottomDropDown1.DropDownListCountry;
            _controller.CarCascadeModel.TopModel.FilterDropDownList = BottomDropDown1.DropDownListCarSegment;
            _controller.CarCascadeModel.TopModel.FeedbackLabel = TopFeedback1.labelCarSegment;
            _controller.CarCascadeModel.MiddleModel.FilterDropDownList = BottomDropDown1.DropDownListCarClass;
            _controller.CarCascadeModel.MiddleModel.FeedbackLabel = TopFeedback1.labelCarClass;
            _controller.CarCascadeModel.BottomModel.FilterDropDownList = BottomDropDown1.DropDownListCarGroup;
            _controller.CarCascadeModel.BottomModel.FeedbackLabel = TopFeedback1.labelCarGroup;
            _controller.CmsOpsModel.GeneralThreeFilterModel.TopModel.FilterDropDownList = BottomDropDown1.DropDownListPool;
            _controller.CmsOpsModel.GeneralThreeFilterModel.TopModel.FeedbackLabel = TopFeedback1.labelCMSPool;
            _controller.CmsOpsModel.GeneralThreeFilterModel.MiddleModel.FilterDropDownList = BottomDropDown1.DropDownListLocationGroup;
            _controller.CmsOpsModel.GeneralThreeFilterModel.MiddleModel.FeedbackLabel = TopFeedback1.labelLocationGroup;
            _controller.CmsOpsModel.GeneralThreeFilterModel.BottomModel.FilterDropDownList = BottomDropDown1.DropDownListBranch;
            _controller.CmsOpsModel.GeneralThreeFilterModel.BottomModel.FeedbackLabel = TopFeedback1.labelBranch;
            _controller.CmsOpsModel.CMSRadioButton = BottomDropDown1.RadioButtonCMS;
            _controller.CmsOpsModel.OPSRadioButton = BottomDropDown1.RadioButtonOPS;
            _controller.CmsOpsModel.TopLabel0 = TopFeedback1.labelStaticTopCMS;
            _controller.CmsOpsModel.TopLabel1 = TopFeedback1.labelStaticTopLocation;
            _controller.CmsOpsModel.BottomLabel0 = BottomDropDown1.labelBottomStaticPool;
            _controller.CmsOpsModel.BottomLabel1 = BottomDropDown1.labelBottomStaticLocation;
            _controller.LabelUpdateModel.TextLabel = TopFeedback1.labelDBUpdate;
            _controller.LabelUpdateModel.ErrorLabel = TopFeedback1.labelDBUpdateError;
            _controller.HeadingModel.HeadingLabel = TopFeedback1.labelHeading;
            _controller.SwitchButton._Button = TopFeedback1.buttonSwitch;
            _controller.ActualsGridModel._HtmlControl = dataTable;
            _controller.ActualsGridModel.GridChartHidden = chartviewHidden;
            _controller.BrowserModel.BrowserHeight = BrowserHeight;
            _controller.BrowserModel.BrowserWidth = BrowserWidth;
            _controller.labelODCollectionsModel.TextLabel = lableODCollectionsValue;
            _controller.LabelODOpentripsModel.TextLabel = LabelODOpentripsValue;
            _controller.ChartModel._Chart = ChartDayActuals;
            _controller.HiddenGridCrouchingChart = ShowGrid;
            _controller.GridviewPanel = GridviewPanel;
            _controller.ChartviewPanel = ChartviewPanel;
            _controller.ChartButton._Button = buttonChart;
            _controller.ExcludeLongterm = cbRemoveLongterm.Checked;
            
            ChartDayActuals.Click += _controller.ChartClick;
            _controller.Initialise(Page);

        }

        protected void cbRemoveLongterm_SelectionChanged(object sender, EventArgs e)
        {
            Session["PoolingExcludeLongTerm"] = cbRemoveLongterm.Checked;
            _controller.UpdateView();
        }

        protected void UpdateController(object sender, EventArgs e)
        {
            _controller.UpdateView();
        }
        protected void DropDownListCarSegment_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.CarSegmentSelected();
        }
        protected void DropDownListCarClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.CarClassSelected();
        }
        protected void DropDownListPool_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.PoolSelected();
        }
        protected void DropDownListLocationGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.LocationGroupSelected();
        }

        protected void DropDownListBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.BranchSelected();
            _controller.UpdateView();
        }

        protected void DropDownListCarGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.CarGroupSelected();
            _controller.UpdateView();
        }
        

        protected void btnExport_Clicked(object sender, EventArgs e)
        {
            _controller.UpdateView();
            var sb = new StringBuilder();

            sb.AppendLine("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            sb.AppendLine("<!--[if gte mso 9]><xml>");
            sb.AppendLine("<x:ExcelWorkbook>");
            sb.AppendLine("<x:ExcelWorksheets>");
            sb.AppendLine("<x:ExcelWorksheet>");
            sb.AppendLine("<x:Name>Report Data</x:Name>");
            sb.AppendLine("<x:WorksheetOptions>");
            sb.AppendLine("<x:Print>");
            sb.AppendLine("<x:ValidPrinterInfo/>");
            sb.AppendLine("</x:Print>");
            sb.AppendLine("</x:WorksheetOptions>");
            sb.AppendLine("</x:ExcelWorksheet>");
            sb.AppendLine("</x:ExcelWorksheets>");
            sb.AppendLine("</x:ExcelWorkbook>");
            sb.AppendLine("</xml>");
            sb.AppendLine("<![endif]--> ");
            sb.AppendLine(_controller.GetHtmlTable().InnerHtml);
            sb.AppendLine("</head>");

            Session["ExportData"] = sb.ToString();
            Session["ExportFileName"] = "DayActualsExport";
            Session["ExportFileType"] = "xls";
            
            
        }
 
        protected void DropDownListCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.CountrySelected();
        }
        protected void CMSRadioButtonLogic_Changed(object sender, EventArgs e)
        {
            _controller.CmsLogicSelected();
        }
        protected void OPSRadioButtonLogic_Changed(object sender, EventArgs e)
        {
            _controller.OpsLogicSelected();
        }
        protected void SwitchButtonClicked(object sender, EventArgs e)
        {
            _controller.onSwitchButtonClicked();
        }
        protected void buttonChartClicked(object sender, EventArgs e)
        {
            _controller.CXGridChart();
        }
    }
}