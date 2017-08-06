using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Entities.VehiclesAbroad {

    public class ResVehiclesEntity {

        public string ResIdNumber { get; set; }
        public string RentLoc { get; set; }
        public string ResArrivalTime { get; set; }
        public string Oneway { get; set; }
        public string RtrnLoc { get; set; }
        public string RtrnTime { get; set; }
        public string ResDays { get; set; }
        public string ResVehClass { get; set; }
        public string GrInclGoldUpr { get; set; }
        public string CustName { get; set; }
        public string PhoneNbr { get; set; }
        public string No1ClubGold { get; set; }
        public string N1Type { get; set; }
        public string CdpidNbr { get; set; }
        public string Taco { get; set; }
        public string Rate { get; set; }
        public string FlightNbr { get; set; }
        public string Remarks { get; set; }
    }
}