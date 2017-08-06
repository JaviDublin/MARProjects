using System;

namespace App.Entities
{
    public class FleetPlanDetail
    {
        public int FleetPlanDetailID { get; set; }
        public int FleetPlanEntryID { get; set; }
        public string Country { get; set; }
        public LocationGroup LocationGroup { get; set; }
        public CarGroup CarGroup { get; set; }
        public int ScenarioID { get; set; }
        public int Addition { get; set; }
        public int Deletion { get; set; }
        public int Amount { get; set; }
        public DateTime DateOfMovement { get; set; }

        public FleetPlanDetail()
        {
            LocationGroup = new LocationGroup();
            CarGroup = new CarGroup();
        }
    }
}