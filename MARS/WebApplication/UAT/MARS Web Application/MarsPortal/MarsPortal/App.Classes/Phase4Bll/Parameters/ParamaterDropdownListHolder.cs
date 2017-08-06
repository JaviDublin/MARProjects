using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Mars.App.Classes.Phase4Bll.Parameters
{
    public class ParamaterDropdownListHolder
    {
        public DropDownList OwningCountry { get; set; }
        public DropDownList CarSegment { get; set; }
        public DropDownList CarClass { get; set; }
        public DropDownList CarGroup { get; set; }
        public DropDownList LocationCountry { get; set; }
        public DropDownList Pool { get; set; }
        public DropDownList Region { get; set; }
        public DropDownList LocationGroup { get; set; }
        public DropDownList Area { get; set; }
        public DropDownList Location { get; set; }

        public DropDownList CheckOutCountry { get; set; }
        public DropDownList CheckOutPool { get; set; }
        public DropDownList CheckOutRegion { get; set; }
        public DropDownList CheckOutLocationGroup { get; set; }
        public DropDownList CheckOutArea { get; set; }
        public DropDownList CheckOutLocation { get; set; }

        public ListBox LocationCountryMultiple { get; set; }
        public ListBox PoolMultiple { get; set; }
        public ListBox RegionMultiple { get; set; }
        public ListBox AreaMultiple { get; set; }
        public ListBox LocationGroupMultiple { get; set; }
        public ListBox LocationMultiple { get; set; }

        
        public ListBox OwningCountryMultiple { get; set; }
        public ListBox PoolOutMultiple { get; set; }
        public ListBox RegionOutMultiple { get; set; }
        public ListBox AreaOutMultiple { get; set; }
        public ListBox LocationGroupOutMultiple { get; set; }
        public ListBox LocationOutMultiple { get; set; }


        public ListBox CarSegmentMultiple { get; set; }
        public ListBox CarClassMultiple { get; set; }
        public ListBox CarGroupMultiple { get; set; }

    }
}