using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Mars.FleetAllocation.UserControls;

namespace Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities
{
    public class WeeklyEditRow
    {
        public static readonly string[] HeaderRows =
        {
            "Year", "Week", "Month", "Car Segment", "Additons", AutoGrid.EditKeyword
        };

        public int GetMonth()
        {
            return Month;
        }

        public int GetCarSegmentId()
        {
            return CarSegmentId;
        }

        public int Year { get; set; }
        public int Week { get; set; }

        public int Month { private get; set; }
        public string MonthName { get { return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month); } }

        public int CarSegmentId { private get; set; }
        public string CarSegmentName { get; set; }

        public int AdditionsLimit { get; set; }

        public int WeeklyLimitOnCarSegmentId { get; set; }
    }
}