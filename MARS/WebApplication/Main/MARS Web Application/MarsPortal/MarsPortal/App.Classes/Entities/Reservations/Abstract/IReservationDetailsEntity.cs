using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Entities.Reservations.Abstract {
    public interface IReservationDetailsEntity {
        System.DateTime IMPORTTIME { get; set; }
        String REP_YEAR { get; set; }
        String REP_MONTH { get; set; }
        String COUNTRY { get; set; }
        String CMS_POOL { get; set; }
        String CMS_LOC_GRP { get; set; }
        String OPS_REGION { get; set; }
        String OPS_AREA { get; set; }
        String CAR_SEGMENT { get; set; }
        String CAR_CLASS { get; set; }
        String CARVAN { get; set; }
        String RES_ID_NBR { get; set; }
        String RES_LOC { get; set; }
        String RENT_LOC { get; set; }
        String RTRN_LOC { get; set; }
        String ICIND { get; set; }
        String ONEWAY { get; set; }
        System.Nullable<System.DateTime> RS_ARRIVAL_DATE { get; set; }
        System.Nullable<System.DateTime> RS_ARRIVAL_TIME { get; set; }
        System.Nullable<System.DateTime> RTRN_DATE { get; set; }
        System.Nullable<System.DateTime> RTRN_TIME { get; set; }
        System.Nullable<Double> RES_DAYS { get; set; }
        String RES_VEH_CLASS { get; set; }
        String GR_INCL_GOLDUPGR { get; set; }
        string ReservedCarGroup { get; set; }
        String RATE_QUOTED { get; set; }
        System.Nullable<Double> SUBTOTAL_2 { get; set; }
        String MOP { get; set; }
        String PREPAID { get; set; }
        String NEVERLOST { get; set; }
        String PREDELIVERY { get; set; }
        String CUST_NAME { get; set; }
        String PHONE { get; set; }
        String CDPID_NBR { get; set; }
        String CNTID_NBR { get; set; }
        String NO1_CLUB_GOLD { get; set; }
        String TACO { get; set; }
        String FLIGHT_NBR { get; set; }
        String REMARKS { get; set; }
        String GS { get; set; }
        String N1TYPE { get; set; }
        System.Nullable<System.DateTime> DATE_SOLD { get; set; }
        String R1 { get; set; }
        String R2 { get; set; }
        String R3 { get; set; }
        System.Nullable<System.DateTime> TS { get; set; }
        System.Nullable<Int32> CO_HOURS { get; set; }
        System.Nullable<Int32> CO_DAYS { get; set; }
        System.Nullable<Int32> CI_HOURS { get; set; }
        System.Nullable<Int32> CI_HOURS_OFFSET { get; set; }
        System.Nullable<Int32> CI_DAYS { get; set; }

    }
}
