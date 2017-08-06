using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using FastMember;

using Mars.App.Classes.DAL.Pooling.Queryables;
using Mars.Pooling.Controllers.Abstract;
using Mars.Pooling.Controllers;

namespace App.Site.Pooling
{
    public partial class ReservationDetails : System.Web.UI.Page
    {
        public string TheTextBoxLabel { get; set; }
        readonly ReservationDetailsController _controller = MarsPortal.Global.CastleContainer.Resolve<ReservationDetailsController>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cbRemoveLongterm.Checked = Session["PoolingExcludeLongTerm"] != null && (bool)Session["PoolingExcludeLongTerm"];
            }

            TheTextBoxLabel = _controller.Textboxpostbackname;
            _controller.GridViewModel.GridViewer = GridViewDetails;
            _controller.TotalsLabel = lblRowCount;
            _controller.PagerMaxModel.FilterDropDownList = DropDownListPagerMaxRows;
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
            _controller.Modal = ReservationDetailsModal1;
            _controller.CheckInOutFilterModel.FilterDropDownList = DropDownListCheckInOut;
            _controller.CheckInOutFilterModel.FeedbackLabel = TopFeedback1.labelCheckinOut;
            _controller.FilterModel.FilterDropDownList = DropDownListFilter;
            _controller.FilterModel.FeedbackLabel = TopFeedback1.labelFilter;
            _controller.DateRangeModel.EndDateModel._TextBox = TextBoxEndDate;
            _controller.DateRangeModel.StartDateModel._TextBox = textboxStartDate;
            _controller.DateRangeModel.StartDateFeedbackLabel = TopFeedback1.labelStartDate;
            _controller.DateRangeModel.EndDateFeedbackLabel = TopFeedback1.labelEndDate;
            _controller.DateRangeModel.EndDateModel.ErrorLabel = LabelEndDateError;
            _controller.DateRangeModel.StartDateModel.ErrorLabel = LabelStartDateError;
            _controller.ResIdTextFilterModel._TextBox = textboxResId;
            _controller.NameTextFilterModel._TextBox = textboxCustName;
            _controller.CdpTextFilterModel._TextBox = textboxCdp;
            _controller.GoldTextFilterModel._TextBox = textbox1Gold;
            _controller.FlightNbrTextFilterModel._TextBox = texboxFlightNbr;
            _controller.LabelUpdateModel.TextLabel = TopFeedback1.labelDBUpdate;
            _controller.LabelUpdateModel.ErrorLabel = TopFeedback1.labelDBUpdateError;
            _controller.HeadingModel.HeadingLabel = TopFeedback1.labelHeading;
            _controller.ExcludeLongterm = cbRemoveLongterm.Checked;
            _controller.Initialise(Page);
            //DropDownListFilter.Items.Remove("Predelivery");

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

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            _controller.UpdateView();
        }

        protected void GridViewDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            _controller.GridViewSelectRowSelected(e.NewSelectedIndex);
        }
        protected void GridViewDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            _controller.GridViewSortSelected(e.SortExpression, e.SortDirection.ToString());
        }
        protected void GridViewDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)

        {
            _controller.GridViewPageSelected(e.NewPageIndex);
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
        protected void textboxStartDate_TextChanged(object sender, EventArgs e)
        {
            _controller.StartDateSelected();
        }
        protected void TextBoxEndDate_TextChanged(object sender, EventArgs e)
        {
            _controller.EndDateSelected();
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
            var ss = _controller.GetDataForExcel();
            BuildCustomRemarksDt(ss);
        }

        private void BuildCustomRemarksDt(IEnumerable<ResGridDisplay> gridData)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine("<body>");
            sb.AppendLine("<table>");
            sb.Append("<tr>");


            sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td>" +
                            "<td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td>" +
                            "<td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td>" +
                            "<td>{15}</td>" 
                            , "Reservation Id Number"
                    , "Car Type", "Checkout Location", "Checkout Date", "Checkout Time", "Checkin Location"
                    , "Checkin Date", "Checkin Time", "Reservation Days", "Tariff", "Customer Name", "CDP",
                     "#1", "Flight Number", "Gold","PrePaid");

            
            sb.AppendLine("</tr>");


            foreach (var gd in gridData)
            {
                sb.Append("<tr>");


                sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td>" +
                            "<td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td>" +
                            "<td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td>" +
                            "<td>{15}</td>", gd.ResIdNumber, gd.CarType, gd.LocationOut, gd.DateOut, gd.TimeOut
                                 , gd.LocationIn, gd.DateIn, gd.TimeIn, gd.ReservationDays, gd.Tariff, gd.CustomerName
                                 , gd.Cdp, gd.No1, gd.FlightNumber, gd.Gold, gd.PrePaid);


                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            var s = sb.ToString();
            Session["ExportData"] = s;
            Session["ExportFileName"] = "ReservationsExport";
            Session["ExportFileType"] = "xls";
        }
    }
}