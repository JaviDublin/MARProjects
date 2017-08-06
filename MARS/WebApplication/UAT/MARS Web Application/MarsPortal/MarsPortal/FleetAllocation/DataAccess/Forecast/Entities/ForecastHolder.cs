using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Forecast.Entities
{
    public class ForecastHolder
    {
        public DateTime ReportDate { get; set; }

        public int CarGroupId { get; set; }
        public int LocationGroupId { get; set; }

        public decimal? Expected { get; set; }
        public double UnConstrained { get; set; }
        public double Nessesary { get; set; }

    }
}