using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities
{
    public class HistoricalTrendRow
    {
        public DateTime Date { get; set; }
        public string ColumnCode { get; set; }
        public int CodeCount { get; set; }
    }
}