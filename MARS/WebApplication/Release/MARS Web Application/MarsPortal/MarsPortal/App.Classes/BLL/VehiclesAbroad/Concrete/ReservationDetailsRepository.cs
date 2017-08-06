using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.BLL.VehiclesAbroad.Concrete {

    public class ReservationDetailsRepository : IReservationDetailsRepository {

        ResVehiclesEntity _rve;

        public ResVehiclesEntity getDetails(string resNo) {

            _rve = new ResVehiclesEntity();

            if (string.IsNullOrEmpty(resNo)) { throw new ArgumentNullException(); }
            else {
                using (MarsDBDataContext db = new MarsDBDataContext()) {
                    try {
                        // Get reservations and remarks
                        var q = (from p in db.Reservations
                                 join r in db.ResRemarks on p.RES_ID_NBR equals r.ResIdNbr into resR
                                 from r1 in resR.DefaultIfEmpty()
                                 where p.RES_ID_NBR == resNo
                                 select new
                                 {
                                     p.RES_ID_NBR,
                                     p.CDPID_NBR,
                                     p.CUST_NAME,
                                     p.FLIGHT_NBR,
                                     p.GR_INCL_GOLDUPGR,
                                     p.N1TYPE,
                                     p.NO1_CLUB_GOLD,
                                     p.ONEWAY,
                                     p.PHONE,
                                     p.RATE_QUOTED,
                                     r1.Remark,
                                     p.RENT_LOC,
                                     p.RES_DAYS,
                                     p.ReservedCarGroup,
                                     p.RTRN_LOC,
                                     p.RTRN_DATE,
                                     p.RS_ARRIVAL_DATE,
                                     p.RS_ARRIVAL_TIME,
                                     p.TACO
                                 }).FirstOrDefault();

                        // Get remarks - 
                        var remarks = from r in db.ResRemarks
                                      where r.ResIdNbr == resNo
                                      select new { r.SeqNbr, r.Remark };

                        // Join remarks into one string
                        string fullRemark = Enumerable.Aggregate(remarks.OrderBy(r => r.SeqNbr), string.Empty,
                            (current, r) => current + r.Remark);

                        // Get Location Id
                        var pickUplocation = (from l in db.LOCATIONs
                                              where l.dim_Location_id == q.RENT_LOC
                                              select l.location1).FirstOrDefault();

                        // Get Return Location Id
                        var rtnlocation = (from l in db.LOCATIONs
                                           where l.dim_Location_id == q.RTRN_LOC
                                           select l.location1).FirstOrDefault();


                        _rve.CdpidNbr = q.CDPID_NBR;
                        _rve.CustName = q.CUST_NAME;
                        _rve.FlightNbr = q.FLIGHT_NBR;
                        _rve.GrInclGoldUpr = "N/A"; //q.GR_INCL_GOLDUPGR.ToString();
                        _rve.N1Type = q.N1TYPE;
                        _rve.No1ClubGold = q.NO1_CLUB_GOLD;
                        _rve.Oneway = q.ONEWAY;
                        _rve.PhoneNbr = q.PHONE;
                        _rve.Rate = q.RATE_QUOTED;
                        _rve.Remarks = fullRemark;
                        _rve.RentLoc = pickUplocation;
                        _rve.ResArrivalTime = q.RS_ARRIVAL_DATE.Value.ToShortDateString() + " " + q.RS_ARRIVAL_TIME.Value.ToShortTimeString();
                        _rve.ResDays = q.RES_DAYS.ToString() ?? "";
                        _rve.ResIdNumber = q.RES_ID_NBR; // already know this as it's part of the query
                        _rve.ResVehClass = q.ReservedCarGroup; // q.RES_VEH_CLASS;
                        _rve.RtrnLoc = rtnlocation;
                        _rve.RtrnTime = q.RTRN_DATE.Value.ToShortDateString() ?? "";
                        _rve.Taco = q.TACO;
                    }
                    catch {
                        // do nothing 
                    }
                }
            }
            return _rve;
        }
    }
}