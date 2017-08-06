using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class LifecycleHoldingCostRow
    {

        public static readonly string[] HeaderRows =
        {
            "Month", "Year", "Owning Country", "Car Segment", "Car Class", "Car Group", "Cost"
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,string.Empty, "#,#.00"
        };

        public LifecycleHoldingCostRow(DateTime month)
        {
            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Month);
            Year = month.Year.ToString(CultureInfo.InvariantCulture);
            MonthDate = month;
        }
        public string Month { get; private set; }
        public string Year { get; private set; }

        public string OwningCountry { get; set; }
        public string CarSegment { get; set; }
        public string CarClass { get; set; }
        public string CarGroup { get; set; }

        public double Cost { get; set; }

        public DateTime MonthDate { private get; set; }

        public DateTime GetMonthDate()
        {
            return MonthDate;
        }
    }
}