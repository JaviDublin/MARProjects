using System;
using System.Web.UI.WebControls;
using App.BLL.CMSReportType;
using App.BLL.ReportEnums.FleetSize;
using App.DAL.MarsDataAccess.ParameterAccess;

namespace App.UserControls.Parameters
{
    public partial class ReportTypeParameters : System.Web.UI.UserControl
    {
        internal bool HasReportTypeChanged { get; set; }
        private readonly BLLcmsReportType _bll = new BLLcmsReportType();

        internal System.Web.UI.WebControls.DropDownList SelectedTimeZone { get { return ddlReportingTimeZone; } }
        internal System.Web.UI.WebControls.DropDownList SelectedForecastType { get { return ddlForecastType; } }
        internal System.Web.UI.WebControls.DropDownList SelectedScenario { get { return ddlFleetPlan; } }

        internal ListItem SelectedTopic { get { return ddlTopic.SelectedItem; } }
        internal ListItem SelectedKpi { get { return ddlKpiCalculation.SelectedItem; } }

        internal bool ShowTimeZoneParameter { set { trTimeZone.Visible = value; } }
        internal bool ShowForecastTypeParameter { set { trForecastType.Visible = value; } }
        internal bool ShowFleetPlanParameter { set { trFleetPlan.Visible = value; } }
        internal bool ShowKpiParameter { set { trKpiCalculation.Visible = value; } }
        internal bool ShowTopicParameter { set { trTopic.Visible = value; } }

        internal bool HideAlreadyBooked { get; set; }

        internal bool ShowKpiAsPercentage
        {
            get { return rblKpiPercentageSelection.SelectedValue == "%"; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HasReportTypeChanged = false;

            rblKpiPercentageSelection.Visible = ddlKpiCalculation.SelectedIndex == 1;


            DataType dataType;
            Enum.TryParse(ddlReportingTimeZone.SelectedValue, out dataType);
            FutureTrendDataType ftDataType;
            Enum.TryParse(ddlForecastType.SelectedValue, out ftDataType);

            if (!IsPostBack)
            {
                PopulateDropDowns();
            }

            var hideAlreadyBooked = dataType == DataType.FrozenZone || HideAlreadyBooked;

            var selectedForecastIndex = ddlForecastType.SelectedIndex;
            var cmsForecastTypeList = _bll.CMSForecastTypeGetAll(hideAlreadyBooked);
            ddlForecastType.DataSource = cmsForecastTypeList;
            ddlForecastType.DataBind();

            var selectedTypeIndex = ddlFleetPlan.SelectedIndex;
            var cmsFleetPlanList = _bll.CMSFleetPlanGetAll(dataType == DataType.FrozenZone);
            ddlFleetPlan.DataSource = cmsFleetPlanList;
            ddlFleetPlan.DataBind();

            if (ftDataType == FutureTrendDataType.AlreadyBooked && dataType == DataType.FrozenZone)
            {
                return;
            }
            ddlForecastType.SelectedIndex = selectedForecastIndex;

            ddlFleetPlan.SelectedIndex = dataType == DataType.FrozenZone ? 0 : selectedTypeIndex;

        }

        private void PopulateDropDowns()
        {
            ddlReportingTimeZone.DataTextField = "ZoneDescription";
            ddlReportingTimeZone.DataValueField = "ZoneID";
            var cmsReportingTimeZone = _bll.CMSReportingTimeZoneGetAll();
            ddlReportingTimeZone.DataSource = cmsReportingTimeZone;
            ddlReportingTimeZone.DataBind();
            ddlReportingTimeZone.SelectedIndex = 0;

            ddlForecastType.DataTextField = "FCType";
            ddlForecastType.DataValueField = "FCTypeID";

            ddlFleetPlan.DataTextField = "PlanDescription";
            ddlFleetPlan.DataValueField = "PlanID";

            ddlTopic.Items.AddRange(ParameterDataAccess.GetComparisonTopics().ToArray());
            ddlKpiCalculation.Items.AddRange(ParameterDataAccess.GetKpiCalculationTypes().ToArray());

        }

        protected void ReportTypeChanged(object sender, EventArgs e)
        {
            HasReportTypeChanged = true;
        }
    }
}