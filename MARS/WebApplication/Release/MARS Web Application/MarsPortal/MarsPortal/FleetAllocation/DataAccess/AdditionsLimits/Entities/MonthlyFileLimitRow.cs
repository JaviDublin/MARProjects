using System;
using Mars.FleetAllocation.UserControls;

namespace Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities
{
    public class MonthlyFileLimitRow
    {
        public static readonly string[] HeaderRows =
        {
            "Country", "File Name", "DateUploaded", "Uploaded By", AutoGrid.ViewKeyword
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
        };

        public string Country { get; set; }
        public string FileName { get; set; }
        public DateTime DateUploaded { get; set; }
        public string UploadedBy { get; set; }

        public int ViewParameterId { get; set; }

    }
}