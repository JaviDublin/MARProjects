using System;
using System.Collections.Generic;
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext; // Added
using App.Entities.VehiclesAbroad; // Added

using App.DAL.VehiclesAbroad.Abstract;
using App.DAL.VehiclesAbroad;
using App.Classes.DAL.VehiclesAbroad;

namespace App.BLL.VehiclesAbroad {

    public class FleetReservationModel : GeneralFilterModel {

        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        public List<FleetMatchEntity> getLicenseRecord(string license, string startdate, string endDate) {
            // Overloaded to get all reservations
            return getFleetReservations(license, "", "", "", "", "", "", "", 99, startdate, endDate,"");
        }

        public List<FleetMatchEntity> getFleetReservations(string dueCountry, string ownCountry, string pool, string locationGroup,
                                                            string carSegment, string carClass, string carGroup, int vehiclePredicament, 
                                                            string startDate, string endDate, string sortExpression) {
            // Overloaded to get all reservations
            dueCountry = processString(dueCountry);
            ownCountry = processString(ownCountry);

            // Presentation logic
            pool = dueCountry == "" ? "" : processString(pool);
            locationGroup = pool == "" ? "" : processString(locationGroup);
            carSegment = ownCountry == "" ? "" : processString(carSegment);
            carClass = carSegment == "" ? "" : processString(carClass);
            carGroup = carClass == "" ? "" : processString(carGroup);

            return getFleetReservations("", dueCountry, ownCountry, pool, locationGroup, carSegment, carClass, carGroup, vehiclePredicament,startDate, endDate, sortExpression);
        }

        public List<FleetMatchEntity> getFleetReservations(string license, string dueCountry, string ownCountry, string pool, string locationGroup,
                                                            string carSegment, string carClass, string carGroup, int vehiclePredicament,
                                                            string startDate, string endDate, string sortExpression) {
            IVehicleDetailsRepository vdr = new VehicleDetailsRepository();
            IFilterEntity f = new FilterEntity {
                DueCountry = dueCountry,
                OwnCountry = ownCountry,
                Pool = pool,
                DueLocationGroup = locationGroup,
                CarSegment = carSegment,
                CarClass = carClass,
                CarGroup = carGroup,
                VehiclePredicament = vehiclePredicament,
                ReservationStartDate = Convert.ToDateTime(startDate),
                ReservationEndDate = Convert.ToDateTime(endDate)
            };
            return new FleetReservationRepository().getList(f, license, sortExpression);
        }

        public List<ReservationMatchEntity> getReservationMatches(string license, string startDateString, string endDateString, string sortExpression) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                List<ReservationMatchEntity> l = new List<ReservationMatchEntity>();
                // the start and end dates should be valid
                DateTime startDate = Convert.ToDateTime(startDateString);
                DateTime endDate = Convert.ToDateTime(endDateString);
                try { // fragile db
                    if (license != null) {
                        FleetMatchEntity fme = getLicenseRecord(license, startDateString, endDateString).FirstOrDefault(); // There should only be one
                        if (fme != null) {

                            // Get all reservations 
                            var q = from res in db.Reservations
                                    join rentLoc in db.LOCATIONs on res.RENT_LOC equals rentLoc.dim_Location_id
                                    join rentClg in db.CMS_LOCATION_GROUPs on rentLoc.cms_location_group_id equals
                                        rentClg.cms_location_group_id
                                    join returnLoc in db.LOCATIONs on res.RTRN_LOC equals returnLoc.dim_Location_id
                                    join returnCMSLocG in db.CMS_LOCATION_GROUPs on returnLoc.cms_location_group_id equals
                                        returnCMSLocG.cms_location_group_id
                                    // Car details
                                    join carGp in db.CAR_GROUPs on res.GR_INCL_GOLDUPGR equals carGp.car_group_id
                                    join carCs in db.CAR_CLASSes on carGp.car_class_id equals carCs.car_class_id
                                    join carS in db.CAR_SEGMENTs on carCs.car_segment_id equals carS.car_segment_id
                                    where
                                        (rentClg.cms_location_group1 == getLocationGroup(fme.Location))
                                        && (returnLoc.served_by_locn.Substring(0, 2) == fme.OwnCountry)
                                        && (carS.car_segment1.Substring(0, 1).ToLower() == fme.CarVan.ToLower())
                                        && (res.RS_ARRIVAL_DATE >= startDate && res.RS_ARRIVAL_DATE <= endDate)
                                    select res;


                            switch (sortExpression)
                            {
                                case "ResLocation": q = q.OrderBy(p => p.LOCATION1.location_dw); break;
                                case "ResLocation DESC": q = q.OrderByDescending(p => p.LOCATION1.location_dw); break;
                                case "ResGroup": q = q.OrderBy(p => p.GR_INCL_GOLDUPGR); break;
                                case "ResGroup DESC": q = q.OrderByDescending(p => p.GR_INCL_GOLDUPGR); break;
                                case "ResCheckoutDate": q = q.OrderBy(p => p.RS_ARRIVAL_DATE); break;
                                case "ResCheckoutDate DESC": q = q.OrderByDescending(p => p.RS_ARRIVAL_DATE); break;
                                case "ResCheckoutLoc": q = q.OrderBy(p => p.RENT_LOC); break;
                                case "ResCheckoutLoc DESC": q = q.OrderByDescending(p => p.RENT_LOC); break;
                                case "ResCheckinLoc": q = q.OrderBy(p => p.RTRN_LOC); break;
                                case "ResCheckinLoc DESC": q = q.OrderByDescending(p => p.RTRN_LOC); break;
                                case "ResId": q = q.OrderBy(p => p.RES_ID_NBR); break;
                                case "ResId DESC": q = q.OrderByDescending(p => p.RES_ID_NBR); break;
                                case "ResNoDaysUntilCheckout": q = q.OrderBy(p => p.CO_DAYS); break;
                                case "ResNoDaysUntilCheckout DESC": q = q.OrderByDescending(p => p.CO_DAYS); break;
                                case "ResNoDaysReserved": q = q.OrderBy(p => p.RES_DAYS); break;
                                case "ResNoDaysReserved DESC": q = q.OrderByDescending(p => p.RES_DAYS); break;
                                case "ResDriverName": q = q.OrderBy(p => p.CUST_NAME); break;
                                case "ResDriverName DESC": q = q.OrderByDescending(p => p.CUST_NAME); break;
                                default: q = q.OrderBy(p => p.CO_DAYS).ThenBy(p => p.LOCATION1.location_dw); break;
                            }

                            foreach (var item in q)
                            {
                                l.Add(new ReservationMatchEntity
                                {
                                    ResLocation = item.LOCATION1.location_dw,
                                    ResGroup = item.GR_INCL_GOLDUPGR.ToString(),
                                    ResCheckoutDate = item.RS_ARRIVAL_DATE,
                                    ResCheckoutLoc = (from loc in db.LOCATIONs
                                                      where loc.dim_Location_id == item.RENT_LOC
                                                      select loc.location1).FirstOrDefault(),
                                    ResCheckinLoc = (from loc in db.LOCATIONs
                                                     where loc.dim_Location_id == item.RTRN_LOC
                                                     select loc.location1).FirstOrDefault()
                                    ,
                                    ResId = item.RES_ID_NBR,
                                    ResNoDaysUntilCheckout = item.CO_DAYS.ToString(),
                                    ResNoDaysReserved = item.RES_DAYS.ToString(),
                                    ResDriverName = item.CUST_NAME
                                });
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    //if (_logger != null)
                    //    _logger.Error("Exception thrown in FleetReservationModel, message : " + ex.Message);
                }
                return l;
            }
        }

    }
}