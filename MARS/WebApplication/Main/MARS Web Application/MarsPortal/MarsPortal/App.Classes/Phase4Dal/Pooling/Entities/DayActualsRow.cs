using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Mars.App.Classes.Phase4Dal.Pooling.Entities
{
    public class DayActualsRow
    {
        public DayActualsRow()
        {
            CellValues = new List<DayActualCell>();
        }
        public string RowName { get; set; }
        public List<DayActualCell> CellValues { get; set; }

 

    }
}