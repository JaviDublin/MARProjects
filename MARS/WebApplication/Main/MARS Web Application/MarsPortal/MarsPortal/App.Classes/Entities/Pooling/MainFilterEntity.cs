using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.Entities.Pooling {
    public class MainFilterEntity : IMainFilterEntity {
        public string Country { get; set; }
        public bool CmsLogic { get; set; }
        public string PoolRegion { get; set; }
        public string LocationGrpArea { get; set; }
        public string Branch { get; set; }
        public string CarSegment { get; set; }
        public string CarClass { get; set; }
        public string CarGroup { get; set; }
        public string LocAndGoldCar { get; set; }
        public string Topic { get; set; }
        public bool ExcludeLongterm { get; set; }
    }
}