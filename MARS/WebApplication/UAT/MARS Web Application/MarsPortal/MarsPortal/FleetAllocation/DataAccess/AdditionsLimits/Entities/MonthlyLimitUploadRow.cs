using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities
{
    public class MonthlyLimitUploadRow
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string CarGroup { get; set; }
        public int Additions { get; set; }

    }
}