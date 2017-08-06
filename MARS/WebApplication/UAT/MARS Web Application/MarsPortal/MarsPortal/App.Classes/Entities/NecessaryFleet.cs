
namespace App.Entities
{
    public class NecessaryFleet
    {
        public Country Country { get; set; }
        public LocationGroup LocationGroup { get; set; }
        public CarGroup CarGroup { get; set; }
        public decimal Utilization { get; set; }
        public decimal NonRevFleet { get; set; }

        public NecessaryFleet()
        {
            Country = new Country();
            LocationGroup = new LocationGroup();
            CarGroup = new CarGroup();
        }
    }
}