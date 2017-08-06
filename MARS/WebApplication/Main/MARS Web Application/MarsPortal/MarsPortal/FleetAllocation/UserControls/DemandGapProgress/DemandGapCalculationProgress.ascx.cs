using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.BusinessLogic;

namespace Mars.FleetAllocation.UserControls.DemandGapProgress
{
    public partial class DemandGapCalculationProgress : UserControl
    {
        public const string CalculationProgressSessionString = "CalculationProgressSessionString";

        public DemandGapCalculationStep CurrentProgress
        {
            get
            {
                if (Session[CalculationProgressSessionString] == null) return DemandGapCalculationStep.NotStarted;
               
                return (DemandGapCalculationStep)Enum.Parse(typeof(DemandGapCalculationStep), Session[CalculationProgressSessionString].ToString());
            }
            set { Session[CalculationProgressSessionString] = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetLabelsForecolour();
        }

        public void SetLabelsForecolour()
        {
            var step = CurrentProgress;

            SetAllGrey();
            if (step == DemandGapCalculationStep.NotStarted) return;

            lblCalculateCurrent.ForeColor = step >= DemandGapCalculationStep.CalculateCurrent ? Color.Green : Color.Red;
            

            lblCalculateMinAndMax.ForeColor = step >= DemandGapCalculationStep.CalculateMinAndMax ? Color.Green : Color.Red;
            imgMinMax.Visible = step == DemandGapCalculationStep.CalculateCurrent;

            lblFillDemandGapOneCarGroup.ForeColor = step >= DemandGapCalculationStep.StepOneCarGroup ? Color.Green : Color.Red;
            imgDgOneCg.Visible = step == DemandGapCalculationStep.CalculateMinAndMax;

            lblFillDemandGapOneCarClass.ForeColor = step >= DemandGapCalculationStep.StepOneCarClass ? Color.Green : Color.Red;
            imgDgOneCc.Visible = step == DemandGapCalculationStep.StepOneCarGroup;

            lblFillDemandGapTwoCarGroup.ForeColor = step >= DemandGapCalculationStep.StepTwoCarGroup ? Color.Green : Color.Red;
            imgDgTwoCg.Visible = step == DemandGapCalculationStep.StepOneCarClass;

            lblFillDemandGapTwoCarClass.ForeColor = step == DemandGapCalculationStep.Complete ? Color.Green : Color.Red;
            imgDgTwoCc.Visible = step == DemandGapCalculationStep.StepTwoCarGroup;
        }

        private void SetAllGrey()
        {
            lblCalculateCurrent.ForeColor = Color.Gray;
            lblCalculateMinAndMax.ForeColor = Color.Gray;
            lblCalculateCurrent.ForeColor = Color.Gray;
            lblFillDemandGapOneCarGroup.ForeColor = Color.Gray;
            lblFillDemandGapOneCarClass.ForeColor = Color.Gray;
            lblFillDemandGapTwoCarGroup.ForeColor = Color.Gray;
            lblFillDemandGapTwoCarClass.ForeColor = Color.Gray;
            imgCurrent.Visible = false;
            imgMinMax.Visible = false;
            imgDgOneCg.Visible = false;
            imgDgOneCc.Visible = false;
            imgDgTwoCg.Visible = false;
            imgDgTwoCc.Visible = false;
            
        }
    }
}