using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.Utilities;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BusinessLogic;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.AdditionsLimits;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.Entities.Output;
using Mars.FleetAllocation.DataAccess.ParameterFiltering;
using Mars.FleetAllocation.DataAccess.VehicleDistribution;

namespace Mars.FleetAllocation.UserControls
{


    public partial class DemandGapDisplay : UserControl
    {
        private const string DemandGapDisplaySession = "FaoDemandGapDisplaySession";
        public const string FaoCalculationStarted = "FaoCalculationStarted";
        private const string FaoCalculationTextSummary = "FaoCalculationTextSummary";
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[FaoCalculationStarted] != null &&
                Session[FaoCalculationStarted].ToString() == "Finished")
            {
                Session[FaoCalculationStarted] = null;
                hfLoading.Value = string.Empty;
                btnCaluclate.Enabled = true;
                tbSummary.Text = Session[FaoCalculationTextSummary].ToString();
            }

            ucDemanGapStageOneGrid.GridItemType = typeof(DemandGapOneRow);
            ucDemanGapStageOneGrid.SessionNameForGridData = DemandGapDisplaySession;
            ucDemanGapStageOneGrid.ColumnHeaders = DemandGapOneRow.HeaderRows;



            ucDgVis.GridItemType = typeof(DemandGapVisualization);
            ucDgVis.SessionNameForGridData = "faoDgViz";
            ucDgVis.ColumnHeaders = DemandGapVisualization.HeaderRows;

            ucResult.GridItemType = typeof (WeeklyAddition);
            ucResult.SessionNameForGridData = "FaoResultData";
            ucResult.ColumnHeaders = WeeklyAddition.HeaderRows;

        }

        public delegate void MethodInvoker();


        protected void btnLoad_Click(object sender, EventArgs e)
        {
            
        }

        public void btnCalculate_Click(object sender, EventArgs e)
        {

            if (Session[FaoCalculationStarted] != "Started")
            {
                Session[FaoCalculationStarted] = "Started";

                hfLoading.Value = "Loading";
                btnCaluclate.Enabled = false;
                var simpleDelegate = new MethodInvoker(CalcualteLimits);


                ucResult.GridData = new List<object>();
                ucDgVis.GridData = new List<object>();
                ucDemanGapStageOneGrid.GridData = new List<object>();
                ucMonthlyLimit.LoadData(new List<MonthlyLimitRow>());
                ucWeeklyLimit.LoadData(new List<WeeklyLimitRow>());

                simpleDelegate.BeginInvoke(null, null);
            }
        }



        public void CalcualteLimits()
        {
            var parameters = ucParameters.GetParameters();
            parameters[DictionaryParameter.StartDate] = (new DateTime(2015, 2, 7)).ToShortDateString();
            List<DemandGapOneRow> demandGapData;

            var sb = new StringBuilder();

            using (var dataAccess = new DemandGapDataAccess(parameters))
            {
                var data = dataAccess.GetDemandGapStepOne();
                //var data2 = dataAccess.GetDemandGapOne(data);
                //var data2 = dataAccess.GetDemandGapStepOneViz();
                //ucDgVis.GridData = data2.ToList();

                demandGapData = data.ToList();
                DemandGapCalculations.CalculateGap(demandGapData);
                demandGapData = demandGapData.ToList();
                ucDemanGapStageOneGrid.GridData = demandGapData;


                //ucDemandGapOne.GridData = data2;

                //var spread = dataAccess.GetSpreadForLocation();
                // ucSpread.GridData = spread;
            }

            List<MonthlyLimitRow> monthlyData;
            List<WeeklyLimitRow> weeklyData;

            using (var dataAccess = new MonthlyAddLimitDataAccess())
            {
                monthlyData = dataAccess.GetMonthlyLimitRows(2);
                weeklyData = dataAccess.GetWeekLyLimitRows(3);
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

                var calculatedAdds = DemandGapCalculations.AssignGroupOne(demandGapData, monthlyData, weeklyData);
                sb.AppendLine("Additions Step 1 Generated:" + calculatedAdds.Sum(d => d.Amount));
                DemandGapCalculations.FillGapsFromMonthlyLimits(demandGapData, monthlyData, weeklyData, calculatedAdds);
                sb.AppendLine("Additions Step 1.1 Generated:" + calculatedAdds.Sum(d => d.Amount));

                dataAccess.FillNames(calculatedAdds);

                ucResult.GridData = calculatedAdds;
                
            }

            //ucDemanGapStageOneGrid.UpdateUpdatePanel();
            //ucDemandGapOne.UpdateUpdatePanel();

            ucMonthlyLimit.LoadData(monthlyData);
            ucWeeklyLimit.LoadData(weeklyData);

            
            sb.AppendLine("Total Monthly assigned:" + monthlyData.Sum(d=> d.Assigned));
            sb.AppendLine("Total Weekly assigned:" + weeklyData.Sum(d => d.Assigned));
            Session[FaoCalculationTextSummary] = sb.ToString();
            
            Session[FaoCalculationStarted] = "Finished";

        }
        
    }
}