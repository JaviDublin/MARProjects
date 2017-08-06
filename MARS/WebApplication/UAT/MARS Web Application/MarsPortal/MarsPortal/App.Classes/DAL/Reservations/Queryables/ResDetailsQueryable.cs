using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Internal;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.Entities.Reservations.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using Mars.Entities.Reservations;

namespace Mars.DAL.Reservations.Queryables
{
    public class ResDetailsQueryable
    {
        public IQueryable<IReservationDetailsEntity> getQueryable(PoolingDataClassesDataContext db, IQueryable<Reservation> q)
        {
            var returned = from p in q
                           select new ReservationDetailsEntity
                           {
                               CAR_CLASS = p.CAR_GROUP.CAR_CLASS.car_class1,
                               CAR_SEGMENT = p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1,
                               CARVAN = "",
                               CDPID_NBR = p.CDPID_NBR,
                               CI_DAYS = p.CI_DAYS,
                               CI_HOURS = p.CI_HOURS,
                               CI_HOURS_OFFSET = p.CI_HOURS_OFFSET,
                               CMS_LOC_GRP = p.LOCATION1.CMS_LOCATION_GROUP.cms_location_group1,
                               CMS_POOL = p.LOCATION1.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1,
                               CNTID_NBR = p.CNTID_NBR,
                               CO_DAYS = p.CO_DAYS,
                               CO_HOURS = p.CO_HOURS,
                               COUNTRY = p.COUNTRY,
                               CUST_NAME = p.CUST_NAME,
                               DATE_SOLD = p.DATE_SOLD,
                               FLIGHT_NBR = p.FLIGHT_NBR,
                               GR_INCL_GOLDUPGR = p.CAR_GROUP.car_group1,
                               ReservedCarGroup = p.ReservedCarGroup,
                               GS = p.GS,
                               ICIND = p.ICIND,
                               IMPORTTIME = DateTime.Now,
                               N1TYPE = p.N1TYPE,
                               NEVERLOST = p.NEVERLOST,
                               NO1_CLUB_GOLD = p.NO1_CLUB_GOLD,
                               ONEWAY = p.ONEWAY,
                               OPS_AREA = p.LOCATION1.OPS_AREA.ops_area1,
                               OPS_REGION = p.LOCATION1.OPS_AREA.OPS_REGION.ops_region1,
                               PHONE = p.PHONE,
                               PREDELIVERY = p.PREDELIVERY,
                               PREPAID = p.PREPAID,
                               R1 = "",
                               R2 = "",
                               R3 = "",
                               RATE_QUOTED = p.RATE_QUOTED,
                               REMARKS = string.Join("<br/><br/>", p.ResRemarks.Where(o => o.ResRmkType == "FLDRMK").Select(d => d.Remark)),
                               RENT_LOC = p.RentalLocation.location1,
                               REP_MONTH = "",
                               REP_YEAR = "",
                               RES_DAYS = p.RES_DAYS,
                               RES_ID_NBR = p.RES_ID_NBR,
                               RES_LOC = p.RentalLocation.location1,
                               RES_VEH_CLASS = p.CAR_GROUP.car_group1,
                               RS_ARRIVAL_DATE = p.RS_ARRIVAL_DATE,
                               RS_ARRIVAL_TIME = p.RS_ARRIVAL_TIME,
                               RTRN_DATE = p.RTRN_DATE,
                               RTRN_LOC = p.ReturnLocation.location1,
                               RTRN_TIME = p.RTRN_TIME,
                               SUBTOTAL_2 = p.SUBTOTAL_2,
                               TACO = p.TACO,
                               TS = DateTime.Now
                           };
            return returned;
        }
    }
}