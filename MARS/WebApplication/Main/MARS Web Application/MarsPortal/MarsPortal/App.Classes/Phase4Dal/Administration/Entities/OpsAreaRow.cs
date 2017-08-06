using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.Entities
{
    public class OpsAreaRow
    {
        public int OpsAreaId { get; set; }
        public string OpsArea { get; set; }
        public int? OpsRegionId { get; set; }
        public string OpsRegion { get; set; }
        public string Country { get; set; }
        public bool? IsActive { get; set; }
      
    }
}