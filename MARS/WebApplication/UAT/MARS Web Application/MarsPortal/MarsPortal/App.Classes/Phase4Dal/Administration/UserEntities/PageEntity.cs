using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.UserEntities
{
    public class PageEntity
    {
        public int UrlId { get; set; }

        public string Url { get; set; }
        public string PageName { get; set; }

        public bool IsBranch { get; set; }
        public bool Assigned { get; set; }

        public int ParentId { get; set; }
        public int MenuId { get; set; }
        public Color ForeColour { get { return Assigned ? Color.Green : Color.Red; } }
        public bool BaseHasAccess { get; set; }

        public bool Enabled { get; set; }

    }
}