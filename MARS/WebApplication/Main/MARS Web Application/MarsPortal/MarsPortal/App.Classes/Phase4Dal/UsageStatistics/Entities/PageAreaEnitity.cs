using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.UsageStatistics.Entities
{
    public class PageAreaEnitity
    {
        public int MenuId { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }

        public Color ForeColour { get { return Selected ? Color.Green : Color.Red; } }
    }
}