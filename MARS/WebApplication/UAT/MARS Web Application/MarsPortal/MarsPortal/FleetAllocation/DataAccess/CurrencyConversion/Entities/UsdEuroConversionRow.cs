using Mars.FleetAllocation.UserControls;

namespace Mars.FleetAllocation.DataAccess.CurrencyConversion.Entities
{
    public class UsdEuroConversionRow
    {
        public static readonly string[] HeaderRows =
        {
            "Year", "EURO Rate", "GBP Rate", AutoGrid.EditKeyword
        };

        public static readonly string[] Formats =
        {
            string.Empty, "#,0.000", "#,0.000", ""
        };

        public int Year { get; set; }
        public double EuroRate { get; set; }
        public double GbpRate { get; set; }

        public int EditYearParameter { get; set; }
    }
}