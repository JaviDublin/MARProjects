﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Pooling.Controllers;

namespace Mars.App.Site.Pooling
{
    public partial class FleetComparison : System.Web.UI.Page
    {
        readonly FleetComparisonController _controller = MarsPortal.Global.CastleContainer.Resolve<FleetComparisonController>();

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
            _controller.TopicDropDownList.FilterDropDownList = DropDownListTopic;
            _controller.DatagridModel.DataTable = scdivTable;
            _controller.SwitchBtn._Button = TopFeedback1.buttonSwitch;
            _controller.BrowserModel.BrowserHeight = BrowserHeight;
            _controller.BrowserModel.BrowserWidth = BrowserWidth;
            _controller.Initialise(Page);
            _controller.ExcludeLongterm = cbRemoveLongterm.Checked;


            //DropDownListTopic.Items.Remove("Predelivery");        
            BottomDropDown1.lblCarGroup.Visible = false;

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
            _controller.SwitchBtnClicked();
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
            Session["ExportData"] = _controller.DatagridModel.DataTable.InnerHtml;
            Session["ExportFileName"] = "FleetComparisonExport";
            Session["ExportFileType"] = "xls";
        }
    }
}