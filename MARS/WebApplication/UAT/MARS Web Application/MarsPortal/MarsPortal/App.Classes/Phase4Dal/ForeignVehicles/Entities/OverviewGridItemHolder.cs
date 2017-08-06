using System.Collections.Generic;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities
{
    public class OverviewGridItemHolder
    {
        public OverviewGridItemHolder()
        {
            ForeignVehiclesHolder = new List<LocationIdHolder>();
        }

        public string CountryId { get; set; }
        public string CountryName { get; set; }   

        public List<LocationIdHolder> ForeignVehiclesHolder { get; set; }
    }
}