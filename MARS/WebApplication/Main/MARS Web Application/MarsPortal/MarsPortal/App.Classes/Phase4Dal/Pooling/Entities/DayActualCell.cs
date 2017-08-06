using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Mars.App.Classes.Phase4Dal.Pooling.Entities
{
    public class DayActualCell
    {
        public int CellValue { get; set; }

        public string ClickArgument { get; set; }
        public string ClickCommand { get; set; }
        public bool LinkButton { get; set; }
    }
}