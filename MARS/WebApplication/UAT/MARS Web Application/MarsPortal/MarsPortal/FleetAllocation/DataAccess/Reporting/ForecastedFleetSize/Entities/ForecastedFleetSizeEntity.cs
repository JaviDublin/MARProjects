namespace Mars.FleetAllocation.DataAccess.Reporting.ForecastedFleetSize.Entities
{
    public class ForecastedFleetSizeEntity
    {
        public static readonly string[] HeaderRows =
        {
            "Year", "Week", "Expected Fleet", "Nessesary", "Unconstrained", "Expected With Additions" ,"Max Fleet", "Min Fleet"
        };

        public int Year { get; set; }
        public int Week { get; set; }

        public decimal ExpectedFleet { get; set; }
        public double Nessesary { get; set; }
        public double UnConstrained { get; set; }

        public decimal ExpectedWithAdditionPlan { get; set; }
        public int MaxFleet { get; set; }
        public int MinFleet { get; set; }

    }
}