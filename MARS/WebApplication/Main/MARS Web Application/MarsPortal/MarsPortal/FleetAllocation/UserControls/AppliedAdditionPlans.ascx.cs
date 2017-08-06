using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BusinessLogic.DropdownLists;
using Mars.FleetAllocation.BusinessLogic.ExcelExport;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess.Entities;
using Mars.FleetAllocation.DataAccess.Reporting.ForecastedFleetSize.Entities;

namespace Mars.FleetAllocation.UserControls
{
    public partial class AppliedAdditionPlans : UserControl
    {
        private const string FaoAppliedAdditionPlanHistoryHolder = "FaoAppliedAdditionPlanHistoryHolder";
        private const string FaoAppliedAdditionPlanEntryHolder = "FaoAppliedAdditionPlanEntryHolder";

        private int SelectedScenarioId
        {
            get { return int.Parse(hfSelectedScenarioId.Value); }
            set { hfSelectedScenarioId.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            agAdditionPlanHistory.GridItemType = typeof(AdditionPlanEntity);
            agAdditionPlanHistory.SessionNameForGridData = FaoAppliedAdditionPlanHistoryHolder;
            agAdditionPlanHistory.ColumnHeaders = AdditionPlanEntity.HeaderRows;

            agAdditionPlanDetails.GridItemType = typeof(AdditionEntity);
            agAdditionPlanDetails.ColumnFormats = AdditionEntity.Formats;
            agAdditionPlanDetails.SessionNameForGridData = FaoAppliedAdditionPlanEntryHolder;
            agAdditionPlanDetails.ColumnHeaders = AdditionEntity.HeaderRows;


            if (!IsPostBack)
            {
                ListGenerator.FillDropdownWithFaoCountries(ddlCountry);
                FillAdditionPlanHistory();
            }

        }

        protected void RefreshHistory(object sender, EventArgs e)
        {
            FillAdditionPlanHistory();
        }

        protected void ShowLessonPopup(object sender, EventArgs e)
        {
            mpeUpdateLesson.Show();
        }

        private void FillAdditionPlanHistory()
        {
            using (var dataAccess = new AdditionPlanDataAccess())
            {
                var appliedOnly = cbActiveOnly.Checked;
                var planData = dataAccess.GetAdditionPlanHistory(appliedOnly);
                agAdditionPlanHistory.GridData = planData;
            }
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == AutoGrid.ViewKeyword)
                {
                    var additionPlanId = int.Parse(commandArgs.CommandArgument.ToString());

                    SelectedScenarioId = additionPlanId;

                    PopulateAdditionPlanData();
                    agAdditionPlanDetails.Visible = true;
                    pnlAdditionPlanDetails.Visible = true;


                    handled = true;
                }
            }
            return handled;
        }

        private void SwitchApplyUnApplyButtons(bool showApply)
        {
            btnApplyAdditionPlan.Visible = !showApply;
            btnUnApplyAdditionPlan.Visible = showApply;
        }

        private void PopulateAdditionPlanData()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            using (var dataAccess = new AdditionPlanDataAccess(parameters))
            {
                var additionData = dataAccess.GetAdditionPlanEntryEntries(SelectedScenarioId, LocationLevelGroupings.Location, VehicleLevelGrouping.CarGroup);

                var additionPlan = dataAccess.GetAdditionPlan(SelectedScenarioId);

                SwitchApplyUnApplyButtons(additionPlan.Applied);


                lblUpdateLessonLabel.Text = string.Format(hfUpdateLessonLabel.Value, additionPlan.Name);
                lblAdditionPlanDateCreated.Text = additionPlan.DateCreated.ToString("dd/MM/yyyy HH:mm");
                lblAdditionPlanName.Text = additionPlan.Name;
                tbUpdatedLesson.Text = additionPlan.LessonLearnt;
                tbLessonsLearnt.Text = additionPlan.LessonLearnt;

                var additionEntities = from ad in additionData
                                       select new AdditionEntity
                                       {
                                           Year = ad.Year,
                                           IsoWeek = ad.IsoWeek,
                                           CarGroupId = ad.CarGroupId,
                                           LocationId = ad.LocationId,
                                           CarGroup = ad.CarGroup,
                                           Location = ad.Location,
                                           Amount = ad.Amount,
                                           Contribution = (double)ad.CpU
                                       };
                agAdditionPlanDetails.GridData = additionEntities.ToList();
            }
        }

        protected void SaveUpdatedLesson(object sender, EventArgs e)
        {
            var lessonLearnt = tbUpdatedLesson.Text;
            tbLessonsLearnt.Text = lessonLearnt;
            using (var dataAccess = new AdditionPlanDataAccess())
            {
                dataAccess.UpdateLessonsLearnt(SelectedScenarioId, lessonLearnt);
            }
        }

        private void ApplyAdditionPlan(bool applied)
        {
            using (var dataAccess = new AdditionPlanDataAccess())
            {
                dataAccess.ApplyAdditionPlan(SelectedScenarioId, applied);
            }
        }

        protected void btnApplyAdditionPlan_Click(object sender, EventArgs e)
        {
            ApplyAdditionPlan(true);
            SwitchApplyUnApplyButtons(true);
            FillAdditionPlanHistory();
        }

        protected void btnUnApplyAdditionPlan_Click(object sender, EventArgs e)
        {
            ApplyAdditionPlan(false);
            SwitchApplyUnApplyButtons(false);
            FillAdditionPlanHistory();
        }

    }
}