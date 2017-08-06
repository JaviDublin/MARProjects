using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.Entities.Pooling {
    public class FeaEntity {
        
        public string Country { get; set; }
        public string CmsPool { get; set; }
        public string CmsLocGrp { get; set; }
        public string OpsRegion { get; set; }
        public string OpsArea { get; set; }
        public string Branch { get; set; }
        public string CarSeg { get; set; }
        public string CarClass { get; set; }
        public string CarGrp { get; set; }
        public string Label { get; set; }
        public Int32 Tme;
        public Int32 Available;
        public Int32 Opentrips;
        public Int32 Reservations;
        public Int32 OnewayRes;
        public Int32 Gold;
        public Int32 Prepaid;
        public Int32 Predelivery;
        public Int32 Checkin;
        public Int32 OnewayCheckin;
        public Int32 OnewayLocal;
        public Int32 Balance;
        public Int32 CiHrs;
        public Int32 CiDays;
        public Int32 CoHrs;
        public Int32 CoDays;
    }
}