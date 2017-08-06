using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.BusinessLogic
{
    public enum DemandGapCalculationStep
    {
        NotStarted,
        CalculateCurrent,
        CalculateMinAndMax,
        StepOneCarGroup,
        StepOneCarClass,
        StepTwoCarGroup,
        StepTwoCarClass,
        Complete,
        Error,
    }
}