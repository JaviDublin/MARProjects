using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Forecast.Entities
{
    public class AddDelRow
    {
        public DateTime TargetDate { get; set; }
        public int CarGroupId { get; set; }
        public int LocationId { get; set; }
        public int AddDels { get; set; }
    }
}