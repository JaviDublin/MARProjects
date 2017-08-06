using System;
using System.Collections.Generic;
using System.Linq;
using App.Classes.DAL.Reservations.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.DAL.Reservations.Queryables;
using App.Classes.Entities.Pooling.Abstract;
using App.Classes.DAL.Reservations.Queryables.SortRepository;

using Mars.Entities.Reservations.Abstract;
using Mars.Entities.Reservations;
using System.Data.SqlClient;

namespace App.Classes.DAL.Reservations
{
    // class doesn't appear to be used so I renamed class from ReservationRepository to ReservationRepository1 - dmaien 23.06.2014
    public class ReservationRepository1 : IReservationRepository
    {

        IList<IReservationDetailsEntity> _repository;
       // ILog _logger = log4net.LogManager.GetLogger("Pooling");

        public IList<IReservationDetailsEntity> getList(IMainFilterEntity mfe, IReservationDetailsFilterEntity rdfe, string sortExpression, string sortDirection)
        {
            using (MarsDBDataContext db = new MarsDBDataContext())
            {
                try
                {
                 
                    IQueryable<Mars.App.Classes.DAL.MarsDBContext.Reservations> iq;

                    if (rdfe.CheckInOut == "Check In")
                        iq = new ReservationQueryableCheckIn().getQueryable(db, mfe);
                    else
                        iq = new ReservationQueryableCheckOut().getQueryable(db, mfe);

                   iq = new ReservationQueryableCars().getQueryable(db, mfe, iq);

                


                    iq = new ReservationQueryableDetails().getQueryable(db, rdfe, iq);
                    iq = new ReservationQueryableSort(new DetailsSortRepository()).getQueryable(sortExpression, sortDirection, db, iq);

                    
                    _repository = iq.Select(p => new ReservationDetailsEntity
                    {
                        CAR_CLASS = p.CAR_GROUP.CAR_CLASS.car_class1,
                        CAR_SEGMENT = p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment_id.ToString(),
                        CARVAN = p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1.Substring(0,1),
                        CDPID_NBR = p.CDPID_NBR,
                        CI_DAYS = p.CI_DAYS,
                        CI_HOURS = p.CI_HOURS,
                        CI_HOURS_OFFSET = p.CI_HOURS_OFFSET,
                        CMS_LOC_GRP = "p.CMS_LOC_GRP",
                        CMS_POOL = "p.CMS_POOL",
                        CNTID_NBR = p.CNTID_NBR,
                        CO_DAYS = p.CO_DAYS,
                        CO_HOURS = p.CO_HOURS,
                        COUNTRY = p.COUNTRY,
                        CUST_NAME = p.CUST_NAME,
                        DATE_SOLD = p.DATE_SOLD,
                        FLIGHT_NBR = p.FLIGHT_NBR,
                        GR_INCL_GOLDUPGR = "p.GR_INCL_GOLDUPGR",
                        GS = p.GS,
                        ICIND = p.ICIND,
                        IMPORTTIME = new DateTime(2012, 12, 12),//"p.IMPORTTIME"
                        MOP = p.MOP,
                        N1TYPE = p.N1TYPE,
                        NEVERLOST = p.NEVERLOST,
                        NO1_CLUB_GOLD = p.NO1_CLUB_GOLD,
                        ONEWAY = p.ONEWAY,
                        OPS_AREA = "p.OPS_AREA",
                        OPS_REGION = "p.OPS_REGION",
                        PHONE = p.PHONE,
                        PREDELIVERY = p.PREDELIVERY,
                        PREPAID = p.PREPAID,
                        R1 = "p.R1",
                        R2 = "p.R2",
                        R3 = "p.R3",
                        RATE_QUOTED = p.RATE_QUOTED,
                        REMARKS = "p.REMARKS",
                        RENT_LOC = "p.RENT_LOC",
                        REP_MONTH = "p.REP_MONTH",
                        REP_YEAR = "p.REP_YEAR",
                        RES_DAYS = p.RES_DAYS,
                        RES_ID_NBR = p.RES_ID_NBR,
                        RES_LOC = "p.RES_LOC",
                        RES_VEH_CLASS = "p.RES_VEH_CLASS",
                        RS_ARRIVAL_DATE = p.RS_ARRIVAL_DATE,
                        RS_ARRIVAL_TIME = p.RS_ARRIVAL_TIME,
                        RTRN_DATE = p.RTRN_DATE,
                        RTRN_LOC = "p.RTRN_LOC",
                        RTRN_TIME = p.RTRN_TIME,
                        SUBTOTAL_2 = p.SUBTOTAL_2,
                        TACO = p.TACO,
                        TS =  new DateTime()//"p.TS"
                    }).ToList<IReservationDetailsEntity>();
                    return _repository;
                }
                catch (SqlException ex)
                {
                    //if (_logger != null) _logger.Error("Exception thrown in ReservationRepository, message : " + ex.Message);
                }
                return new List<IReservationDetailsEntity>();
            }
        }
        public IReservationDetailsEntity getItem(string resId)
        {
            return _repository.Where(p => p.RES_ID_NBR == resId).FirstOrDefault();
        }
    }
}
