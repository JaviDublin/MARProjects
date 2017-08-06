using System.Collections.Generic;
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext; // Added
using App.Entities.VehiclesAbroad; // Added

namespace App.BLL.VehiclesAbroad {

    public class ReservationFleetModel:ReservationDetailsModel {

        public List<ReservationMatchEntity> getReservations(string dueCountry, string ownCountry, string pool, string locationGroup,
                                                            string carSegment, string carClass, string carGroup,
                                                            string destinationPool, string destinationLocationGroup,
                                                            string reservationStartdate, string reservationEnddate,
                                                            string sortExpression) {
            // inheriting from ReservationDetailsModel can us the public method to get a ReservationMatchEntity

            return getVehicleDetails(dueCountry, 99, ownCountry,
                                        pool, locationGroup,
                                        carSegment, carClass, carGroup,
                                        "", "", "", "", "", "", "", "",
                                        destinationPool, destinationLocationGroup,
                                        reservationStartdate, reservationEnddate,
                                        sortExpression);
        }

        public List<FleetMatchEntity> getFleetMatches(string resId, string sortExpression) {
            // requires the resId from the webpage to find a corresponding match
            // and a sortExpression so for the orderby 
            // returns a list of type FleetMatchEntity

            using(MarsDBDataContext db = new MarsDBDataContext()) {

                List<FleetMatchEntity> l = new List<FleetMatchEntity>();

                if(resId != null) {

                    ReservationMatchEntity thisRes = getReservationMatch(resId);

                    var q = from fea in db.FLEET_EUROPE_ACTUALs
                            join clg in db.CMS_LOCATION_GROUPs on fea.LOC_GROUP equals clg.cms_location_group_id
                            where clg.cms_location_group1 == getLocationGroup(thisRes.ResCheckoutLoc)
                            && fea.COUNTRY.Equals(thisRes.ResCheckinLoc.Substring(0, 2))
                            && fea.COUNTRY != fea.LSTWWD.Substring(0, 2)
                            && fea.COUNTRY != ((fea.DUEWWD == null || fea.DUEWWD == "") ? fea.LSTWWD.Substring(0, 2) : fea.DUEWWD.Substring(0, 2))
                            && (fea.ON_RENT != 1)
                            select new { fea, location = clg.cms_location_group1 };

                    // sort the return set by it's sortExpression
                    switch(sortExpression) {
                        case "Dayrev": q = q.OrderBy(p => p.fea.DAYSREV); break;
                        case "Dayrev DESC": q = q.OrderByDescending(p => p.fea.DAYSREV); break;
                        case "License": q = q.OrderBy(p => p.fea.LICENSE); break;
                        case "License DESC": q = q.OrderByDescending(p => p.fea.LICENSE); break;
                        case "Location": q = q.OrderBy(p => p.location); break;
                        case "Location DESC": q = q.OrderByDescending(p => p.location); break;
                        case "ModelDesc": q = q.OrderBy(p => p.fea.MODDESC); break;
                        case "ModelDesc DESC": q = q.OrderByDescending(p => p.fea.MODDESC); break;
                        case "Operstat": q = q.OrderBy(p => p.fea.OPERSTAT); break;
                        case "Operstat DESC": q = q.OrderByDescending(p => p.fea.OPERSTAT); break;
                        case "OwnCountry": q = q.OrderBy(p => p.fea.COUNTRY); break;
                        case "OwnCountry DESC": q = q.OrderByDescending(p => p.fea.COUNTRY); break;
                        case "Unit": q = q.OrderBy(p => p.fea.UNIT); break;
                        case "Unit DESC": q = q.OrderByDescending(p => p.fea.UNIT); break;
                        case "Vc": q = q.OrderBy(p => p.fea.VC); break;
                        case "Vc DESC": q = q.OrderByDescending(p => p.fea.VC); break;
                    }

                    foreach(var item in q.Distinct()) {

                        l.Add(new FleetMatchEntity {
                            Daysrev = item.fea.DAYSREV.ToString(),
                            License = item.fea.LICENSE,
                            Location = item.fea.LSTWWD,
                            ModelDesc = item.fea.MODDESC,
                            Operstat = item.fea.OPERSTAT,
                            OwnCountry = item.fea.COUNTRY,
                            Unit = item.fea.UNIT,
                            Vc = item.fea.VC
                        });
                    }
                }
                return l;
            }
        }

        private ReservationMatchEntity getReservationMatch(string resId) {
            // requires the reservation Id (resId) from the GridView
            // returns a single record from the Reservations_Europe_Actual

            using(MarsDBDataContext db = new MarsDBDataContext()) {

                List<ReservationMatchEntity> l = new List<ReservationMatchEntity>();

           
                var q = from res in db.Reservations
                        // Rent Location Details
                        join startloc in db.LOCATIONs on res.RENT_LOC equals startloc.dim_Location_id
                        // Return Location
                        join returnloc in db.LOCATIONs on res.RTRN_LOC equals returnloc.dim_Location_id
                        // Car details
                        join carGp in db.CAR_GROUPs on res.GR_INCL_GOLDUPGR equals carGp.car_group_id
                        where res.RES_ID_NBR.Equals(resId)
                        select res;

                foreach(var item in q) {

                    l.Add(new ReservationMatchEntity {
                        ResLocation = item.LOCATION1.location1,
                        ResGroup = item.CAR_GROUP.car_group1,
                        ResCheckoutDate = item.RS_ARRIVAL_DATE,
                        ResCheckoutLoc = item.LOCATION1.location1,
                        ResCheckinLoc = item.LOCATION.location1,
                        ResId = item.RES_ID_NBR,
                        ResNoDaysUntilCheckout = item.CO_DAYS.ToString(),
                        ResNoDaysReserved = item.RES_DAYS.ToString()
                    });
                }
                return l.FirstOrDefault();
            }
        }
    }
}