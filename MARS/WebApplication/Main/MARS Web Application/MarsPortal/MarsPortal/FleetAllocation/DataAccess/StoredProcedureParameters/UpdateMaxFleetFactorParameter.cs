using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.StoredProcedureParameters
{
    public class UpdateMaxFleetFactorParameter
    {
        public int ScenarioId { get; set; }
        public int? LocationId { get; set; }
        public int? LocationGroupId { get; set; }
        public int? PoolId { get; set; }
        public string LocationCountry { get; set; }
        public int? CarGroupId { get; set; }
        public int? CarClassId { get; set; }
        public int? CarSegmentId { get; set; }
        public string OwningCountry { get; set; }
        public string DayOfWeekIds { get; set; }
        public int MarsUserId { get; set; }
        public float? NonRevPercentage { get; set; }
        public float? UtilizationPercentage { get; set; } 

    }

}