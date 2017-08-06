using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Entities.Reservations.Abstract;

namespace Mars.Entities.Reservations {
    public class ReservationDetailsEntity : IReservationDetailsEntity {
        public DateTime IMPORTTIME { get; set; }
        public string REP_YEAR { get; set; }
        public string REP_MONTH { get; set; }
        public string COUNTRY { get; set; }
        public string CMS_POOL { get; set; }
        public string CMS_LOC_GRP { get; set; }
        public string OPS_REGION { get; set; }
        public string OPS_AREA { get; set; }
        public string CAR_SEGMENT { get; set; }
        public string CAR_CLASS { get; set; }
        public string CARVAN { get; set; }
        public string RES_ID_NBR { get; set; }
        public string RES_LOC { get; set; }
        public string RENT_LOC { get; set; }
        public string RTRN_LOC { get; set; }
        public string ICIND { get; set; }
        public string ONEWAY { get; set; }
        public DateTime? RS_ARRIVAL_DATE { get; set; }
        public DateTime? RS_ARRIVAL_TIME { get; set; }
        public DateTime? RTRN_DATE { get; set; }
        public DateTime? RTRN_TIME { get; set; }
        public double? RES_DAYS { get; set; }
        public string RES_VEH_CLASS { get; set; }
        public string GR_INCL_GOLDUPGR { get; set; }
        public string ReservedCarGroup { get; set; }
        public string RATE_QUOTED { get; set; }
        public double? SUBTOTAL_2 { get; set; }
        public string MOP { get; set; }
        public string PREPAID { get; set; }
        public string NEVERLOST { get; set; }
        public string PREDELIVERY { get; set; }
        public string CUST_NAME { get; set; }
        public string PHONE { get; set; }
        public string CDPID_NBR { get; set; }
        public string CNTID_NBR { get; set; }
        public string NO1_CLUB_GOLD { get; set; }
        public string TACO { get; set; }
        public string FLIGHT_NBR { get; set; }
        public string REMARKS { get; set; }
        public string GS { get; set; }
        public string N1TYPE { get; set; }
        public DateTime? DATE_SOLD { get; set; }
        public string R1 { get; set; }
        public string R2 { get; set; }
        public string R3 { get; set; }
        public DateTime? TS { get; set; }
        public int? CO_HOURS { get; set; }
        public int? CO_DAYS { get; set; }
        public int? CI_HOURS { get; set; }
        public int? CI_HOURS_OFFSET { get; set; }
        public int? CI_DAYS { get; set; }
    }
}