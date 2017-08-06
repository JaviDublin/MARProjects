using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Bll.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BusinessLogic;
using Mars.FleetAllocation.BusinessLogic.DropdownLists;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess.Entities;
using Mars.FleetAllocation.DataAccess.AdditionsLimits;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.ParameterFiltering;
using Mars.FleetAllocation.DataAccess.ScenarioAccess;
using Mars.FleetAllocation.DataAccess.VehicleDistribution;
using Rad.Security;


namespace Mars.FleetAllocation.UserControls
{
    public partial class AdditionPlanGenerator : UserControl
    {
        private const string DemandGapDisplaySession = "FaoDemandGapDisplaySession";
        public const string FaoCalculationStarted = "FaoCalculationStarted";
        private const string FaoCalculationTextSummary = "FaoCalculationTextSummary";
        public const string AdditionPlanSessionHolder = "AdditionPlanSessionHolder";

        private string SummaryText
        {
            get { return Session[FaoCalculationTextSummary] == null ? string.Empty : Session[FaoCalculationTextSummary].ToString(); }
            set { Session[FaoCalculationTextSummary] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            lblErrorMessage.Visible = false;
            lblProcessMessage.Text = string.Empty;
            if (!IsPostBack)
            {
                hfLoggedOnUser.Value = ApplicationAuthentication.GetGlobalId();
                ucDgcProgress.CurrentProgress = DemandGapCalculationStep.NotStarted;
                PopulateScenarios();
                PopulateMinAndMaxSpreadDates();
                pnlGapFill.Visible = false;
            }

            if (ucDgcProgress.CurrentProgress == DemandGapCalculationStep.Complete ||
                ucDgcProgress.CurrentProgress == DemandGapCalculationStep.Error)
            {
                hfLoading.Value = string.Empty;
                btnCaluclate.Enabled = true;
                pnlGapFill.Visible = true;
                tbSummary.Text = ucDgcProgress.CurrentProgress == DemandGapCalculationStep.Complete
                    ? SummaryText : string.Empty;
                ucDgcProgress.CurrentProgress = DemandGapCalculationStep.NotStarted;

            }

            ucDemanGapStageOneGrid.GridItemType = typeof (WeeklyMaxMinValues);
            ucDemanGapStageOneGrid.SessionNameForGridData = DemandGapDisplaySession;
            ucDemanGapStageOneGrid.ColumnHeaders = WeeklyMaxMinValues.HeaderRows;
            ucDemanGapStageOneGrid.ColumnFormats = WeeklyMaxMinValues.Formats;

            ucResult.GridItemType = typeof (AdditionEntity);
            ucResult.SessionNameForGridData = AdditionPlanSessionHolder;
            ucResult.ColumnHeaders = AdditionEntity.HeaderRows;
            ucResult.ColumnFormats = AdditionEntity.Formats;
        }

        public delegate void MethodInvoker();

        private void PopulateMinAndMaxSpreadDates()
        {
            using (var dataAccess = new BaseDataAccess())
            {
                var minDate = dataAccess.MinCommercialDataDate();
                var maxDate = dataAccess.MaxCommercialDataDate();
                lblSpreadRange.Text = string.Format("{0:MMMM yyyy} - {1:MMMM yyyy}", minDate, maxDate);
                //dataAccess.MaxCommercialDataDate().ToString("MMMM yyyy") ;
                lblLastFleetHistory.Text = dataAccess.GetLastHistoryTimestamp().ToShortDateString();
            }
        }

        private void PopulateScenarios()
        {
            int countryIdSelected;
            using (var dataAccess = new MinCommercialSegmentDataAccess())
            {
                ListGenerator.FillDropdownWithFaoCountries(ddlCountry, dataAccess);

                countryIdSelected = int.Parse(ddlCountry.SelectedValue);
                var scenarioListItems = dataAccess.GetMinCommSegScenarios(countryIdSelected);
                ddlMinCommSeg.Items.AddRange(scenarioListItems.ToArray());

                SetMinScenDescription(dataAccess);
            }
            using (var dataAccess = new MaxFleetFactorDataAccess())
            {
                var scenarioListItems = dataAccess.GetMaxFleetFactorScenarios(countryIdSelected);
                ddlMaxFleetFactor.Items.AddRange(scenarioListItems.ToArray());

                SetMaxScenDescription(dataAccess);
            }
        }

        protected void ddlMinCommSeg_SelectionChanged(object sender, EventArgs e)
        {
            using (var dataAccess = new MinCommercialSegmentDataAccess())
            {
                SetMinScenDescription(dataAccess);
            }
        }

        protected void ddlMaxFleetFactor_SelectionChanged(object sender, EventArgs e)
        {
            using (var dataAccess = new MaxFleetFactorDataAccess())
            {
                SetMaxScenDescription(dataAccess);
            }
        }

        private void SetMinScenDescription(MinCommercialSegmentDataAccess dataAccess)
        {
            var minScenId = int.Parse(ddlMinCommSeg.SelectedValue);
            var maxDescription = dataAccess.GetScenarioDescription(minScenId);
            imgMinFleetDescription.ToolTip = maxDescription;
        }

        private void SetMaxScenDescription(MaxFleetFactorDataAccess dataAccess)
        {
            var minScenId = int.Parse(ddlMaxFleetFactor.SelectedValue);
            var maxDescription = dataAccess.GetScenarioDescription(minScenId);
            imgMaxFleetDescription.ToolTip = maxDescription;
        }

        public void btnCalculate_Click(object sender, EventArgs e)
        {

            if (ucDgcProgress.CurrentProgress == DemandGapCalculationStep.NotStarted
                || ucDgcProgress.CurrentProgress == DemandGapCalculationStep.Complete)
            {
                ucDgcProgress.CurrentProgress = DemandGapCalculationStep.NotStarted;

                hfLoading.Value = "Loading";
                btnCaluclate.Enabled = false;
                var simpleDelegate = new MethodInvoker(CalcualteLimits);


                ucResult.GridData = new List<object>();
                //ucDgVis.GridData = new List<object>();
                ucDemanGapStageOneGrid.GridData = new List<object>();
                ucMonthlyLimit.LoadData(new List<MonthlyLimitRow>());
                ucWeeklyLimit.LoadData(new List<WeeklyLimitRow>());

                ucDgcProgress.CurrentProgress = DemandGapCalculationStep.CalculateCurrent;
                simpleDelegate.BeginInvoke(null, null);                
            }
        }

        public void CalcualteLimits()
        {
            try
            {
                var parameters = ucParameters.GetParameters();
                List<WeeklyMaxMinValues> demandGapData;
                var sb = new StringBuilder();
                var stepToCalculateTo = int.Parse(ddlStage.SelectedValue);
                DateTime currentDay;

                using (var dataAccess = new DemandGapDataAccess(parameters))
                {
                    currentDay = dataAccess.GetLastHistoryTimestamp();
                    var weeksToCalculate = int.Parse(tbWeeks.Text);

                    //Constant 12 weeks for history calculations
                    parameters[DictionaryParameter.StartDate] = (currentDay.AddDays(-84)).ToShortDateString();
                    parameters[DictionaryParameter.EndDate] = (currentDay).ToShortDateString();
                    parameters[DictionaryParameter.ForecastStartDate] = (currentDay).ToShortDateString();

                    var endDate = currentDay.AddDays(weeksToCalculate*7).Next(DayOfWeek.Sunday);
                    parameters[DictionaryParameter.ForecastEndDate] = (endDate).ToShortDateString();

                    var minComSegScenarioId = int.Parse(ddlMinCommSeg.SelectedValue);
                    var maxFleetFactorScenarioId = int.Parse(ddlMaxFleetFactor.SelectedValue);

                    demandGapData = dataAccess.CalculateMinMax(minComSegScenarioId, maxFleetFactorScenarioId, 0);


                    ucDgcProgress.CurrentProgress = DemandGapCalculationStep.CalculateMinAndMax;
                    var sw = new Stopwatch();
                    sw.Start();
                    DemandGapOneCalculations.CalculateGap(demandGapData);
                    
                    demandGapData = demandGapData.ToList();
                    sw.Stop();
                    var xx = sw.Elapsed;

                    sw.Reset();
                    sw.Start();
                    var rankingData = dataAccess.GetFinanceEntities();
                    sw.Stop();
                    var yy = sw.Elapsed;
                    sw.Reset();
                    sw.Start();
                    DemandGapOneCalculations.AttachRankingsToDemandGaps(demandGapData, rankingData);

                    sw.Stop();
                    var zz = sw.Elapsed;
                    sw.Reset();
                    sw.Start();

                    demandGapData = demandGapData.OrderBy(d => d.RankFromRevenue).ToList();
                    sw.Stop();
                    var qq = sw.Elapsed;
                    ucDemanGapStageOneGrid.GridData = demandGapData;
                    
                }

                List<MonthlyLimitRow> monthlyData;
                List<WeeklyLimitRow> weeklyData;

                int? carSegmentId = null;
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
                {
                    carSegmentId = int.Parse(parameters[DictionaryParameter.CarSegment]);
                }

                using (var dataAccess = new MonthlyAddLimitDataAccess())
                {
                    monthlyData = dataAccess.GetMonthlyLimitRows(carSegmentId, currentDay);
                    weeklyData = dataAccess.GetWeekLyLimitRows(carSegmentId, currentDay);
                }

                using (var dataAccess = new VehicleDistributionDataAccess())
                {
                    if (parameters[DictionaryParameter.CarGroup] != string.Empty
                        && (!parameters[DictionaryParameter.CarClass].Contains(LocationQueryable.Separator)))
                    {
                        var carClassId = int.Parse(parameters[DictionaryParameter.CarClass]);
                        using (var lookup = new LookupDataAccess())
                        {
                            var splitCarGroupIds = lookup.FindCarGroupsInCarClass(carClassId);
                            monthlyData = monthlyData.Where(d => splitCarGroupIds.Contains(d.GetCarGroupId())).ToList();
                        }
                    }

                    ucDgcProgress.CurrentProgress = DemandGapCalculationStep.StepOneCarGroup;
                    var calculatedAdds = DemandGapOneCalculations.FillGapOneCarGroup(demandGapData, monthlyData,
                        weeklyData);

                    sb.AppendLine("Additions Step 1 Generated:" + calculatedAdds.Sum(d => d.Amount));

                    if (stepToCalculateTo > 1)
                    {
                        ucDgcProgress.CurrentProgress = DemandGapCalculationStep.StepOneCarClass;
                        DemandGapOneCalculations.FillGapOneCarClass(demandGapData, monthlyData, weeklyData,
                            calculatedAdds);
                        sb.AppendLine("Additions Step 1.1 Generated:" + calculatedAdds.Sum(d => d.Amount));
                    }

                    if (stepToCalculateTo > 2)
                    {
                        ucDgcProgress.CurrentProgress = DemandGapCalculationStep.StepTwoCarGroup;
                        DemandGapOneCalculations.FillGapTwoCarGroup(demandGapData, monthlyData, weeklyData,
                            calculatedAdds);
                        sb.AppendLine("Additions Step 2 Generated:" + calculatedAdds.Sum(d => d.Amount));
                    }

                    if (stepToCalculateTo > 3)
                    {
                        ucDgcProgress.CurrentProgress = DemandGapCalculationStep.StepTwoCarClass;
                        DemandGapOneCalculations.FillGapTwoCarClass(demandGapData, monthlyData, weeklyData,
                            calculatedAdds);
                        sb.AppendLine("Additions Step 2.1 Generated:" + calculatedAdds.Sum(d => d.Amount));
                    }


                    dataAccess.FillNames(calculatedAdds);


                    var distinctContributionData = from dgd in demandGapData
                                                   group dgd by new {CarGroupId = dgd.GetCarGroupId(), LocationId = dgd.GetLocationId()}
                                                   into groupedData
                                                select new
                                                       {
                                                           groupedData.Key.CarGroupId,
                                                           groupedData.Key.LocationId,
                                                           groupedData.First().Contribution
                                                       };
                    var additionEntities = from ca in calculatedAdds
                                           join dgd in distinctContributionData on new { ca.CarGroupId, ca.LocationId }
                                                                equals new { dgd.CarGroupId,    dgd.LocationId }
                                           into jdgd
                                           from joinedDcd in jdgd.DefaultIfEmpty()
                        select new AdditionEntity
                               {
                                   Year = ca.Year,
                                   IsoWeek = ca.IsoWeek,
                                   CarGroupId = ca.CarGroupId,
                                   LocationId = ca.LocationId,
                                   CarGroup = ca.CarGroup,
                                   Location = ca.Location,
                                   Amount = ca.Amount,
                                   Contribution = joinedDcd == null ? 0 : (double) joinedDcd.Contribution
                               };

                    ucResult.GridData = additionEntities.ToList();

                }
                ucMonthlyLimit.LoadData(monthlyData);
                ucWeeklyLimit.LoadData(weeklyData);

                sb.AppendLine("Total Monthly assigned: " + monthlyData.Sum(d => d.Assigned));
                sb.AppendLine("Total Weekly assigned: " + weeklyData.Sum(d => d.Assigned));
                SummaryText = sb.ToString();

                ucDgcProgress.CurrentProgress = DemandGapCalculationStep.Complete;
            }
            catch (Exception e)
            {
                SummaryText = e.ToString();
                ucDgcProgress.CurrentProgress = DemandGapCalculationStep.Error;
            }
        }

        protected void btnSaveAdditionPlan_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAdditionPlan();   
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.ToString();
                ucDgcProgress.CurrentProgress = DemandGapCalculationStep.Error;
            }
        
        }

        private void SaveAdditionPlan()
        {
            AdditionPlanEntity ape;
            using (var dataAccess = new BaseDataAccess())
            {
                ape = new AdditionPlanEntity
                {
                    Name = tbAdditionPlanName.Text,
                    MinComSegScenarioName = ddlMinCommSeg.SelectedItem.Text,
                    MinComSegScenarioDescription = imgMinFleetDescription.ToolTip,
                    MaxFleetScenarioName = ddlMaxFleetFactor.SelectedItem.Text,
                    MaxFleetScenarioDescription = imgMaxFleetDescription.ToolTip,
                    WeeksCalculated = int.Parse(tbWeeks.Text),
                    CurrentDate = dataAccess.GetLastHistoryTimestamp(),
                    StartRevenueDate = dataAccess.MinCommercialDataDate(),
                    EndRevenueDate = dataAccess.MinCommercialDataDate(),
                };
            }



            var additions = (List<AdditionEntity>)Session[AdditionPlanSessionHolder];
            if (additions == null) return;

            var minMaxValues = (List<WeeklyMaxMinValues>)Session[DemandGapDisplaySession];


            
            var countryId = int.Parse(ddlCountry.SelectedValue);

            using (var additionPlanDataAccess = new AdditionPlanDataAccess())
            {
                additionPlanDataAccess.CreateAdditionPlan(ape, additions, minMaxValues, countryId);
            }
            pnlGapFill.Visible = false;

        }
        
    }
}