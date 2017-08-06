using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.Entities
{
    public class CarGroups
    {
        public int CarGroupId { get; set; }
        public string CarGroup { get; set; }
        public string Country { get; set; }
        public int CarSegmentId { get; set; }
        public string CarSegment { get; set; }
        public int CarClassId { get; set; }
        public string CarClass { get; set; }
        public string Gold { get; set; }
        public string FiveStar { get; set; }
        public string PresidentCircle { get; set; }
        public string Platinum { get; set; }
        public bool? IsActive { get; set; } 
    }
}