using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Forecast.Entities
{
    public class SummedByWeekForecastHolder
    {
        public short Year { get; set; }
        public byte Week { get; set; }
        public decimal ExpectedFleet { get; set; }
        public double NessesaryFleet { get; set; }
        public double Unconstrained { get; set; }

    }
}