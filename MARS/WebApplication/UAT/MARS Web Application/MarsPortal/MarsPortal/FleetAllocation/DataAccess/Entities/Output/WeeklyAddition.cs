using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities.Output
{
    public class WeeklyAddition
    {

        public int Year { get; set; }

        public int IsoWeek { get; set; }

        
        public int CarGroupId {  get; set; }
        public int LocationId { get; set; }

        public string CarGroup {  get; set; }
        public string Location {  get; set; }

        public int Amount { get; set; }

        public decimal CpU { get; set; }
    }
}