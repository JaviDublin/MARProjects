using System.Collections.Generic;

namespace App.Entities
{
    public class FleetPlanDetailListContainer
    {
        public List<FleetPlanEntry> FleetPlanEntryList { get; set; }
        public List<FleetPlanDetail> FleetPlanDetailList { get; set; }


        public FleetPlanDetailListContainer()
        {
            FleetPlanDetailList = new List<FleetPlanDetail>();
            FleetPlanEntryList = new List<FleetPlanEntry>();
        }
    }
}