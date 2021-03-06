﻿using System;
using System.Collections.Generic;
using System.Linq;
using App.BLL.VehiclesAbroad;
using App.Entities.VehiclesAbroad;
using DAL.VehiclesAbroad;
using Mars.App.Classes.DAL.MarsDBContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarsUnitTests.VehiclesAbroad
{
    [TestClass]
    public class CompareReservationDetailsModel
    {
        [TestMethod]
        public void CompareNewGetReservationDetailstoOldQuery()
        {
             ReservationDetailsModel rModel = new ReservationDetailsModel();

            var newQuery = rModel.getVehicleDetails(null, 0, null, null, null, null, null, null, null, null, null, "ResDetails", null,
                null, null, null, null, null, "19/06/2014", "21/06/2014", "").OrderBy(d => d.ResId);
            var oldQuery = getVehicleDetailsWithOldQuery(null, 0, null, null, null, null, null, null, null, null, null, "ResDetails",
                null,
                null, null, null, null, null, "19/06/2014", "21/06/2014", "").OrderBy(d => d.ResId);



            var q2 = oldQuery.Select(t => t.ResId)
                              .Except(newQuery.Select(e => e.ResId))
                              .ToList();

        }

        protected string processString(string s)
        {
            // returns the string and empty string if null or equal to "***All***"

            return s == "***All***" ? string.Empty : s ?? "";
        }


        public FilterEntity conditionFilters(string destinationCountry, int vehiclePredicament, string _owningCountry, string pool, string locationGroup,
                          string carSegment, string carClass, string carGroup,
                          string destinationPool, string destinationLocationGroup)
        {
            // conditions the filters and takes care of null
            // returns the 'cleaned' filters as an entity

            FilterEntity fe = new FilterEntity();

            // helper to condition the filter input
            fe.DueCountry = processString(destinationCountry);
            fe.OwnCountry = processString(_owningCountry);
            fe.VehiclePredicament = vehiclePredicament;

            // Presentation logic - this arrangement is for the pool and location group to be populated from the owning country
            fe.Pool = fe.OwnCountry == "" ? "" : processString(pool); // BEWARE!
            fe.Location = fe.Pool == "" ? "" : processString(locationGroup);
            fe.CarSegment = fe.OwnCountry == "" ? "" : processString(carSegment);
            fe.CarClass = fe.CarSegment == "" ? "" : processString(carClass);
            fe.CarGroup = fe.CarClass == "" ? "" : processString(carGroup);

            fe.DuePool = string.IsNullOrEmpty(fe.DueCountry) ? string.Empty : processString(destinationPool);
            fe.DueLocationGroup = string.IsNullOrEmpty(fe.DuePool) ? string.Empty : processString(destinationLocationGroup);

            return fe;
        }



        public List<ReservationMatchEntity> getVehicleDetailsWithOldQuery(string dueCountry, int vehiclePredicament,
                                                   string ownCountry, string pool, string locationGroup,
                                                   string carSegment, string carClass, string carGroup,
                                                   string unit, string license, string model, string modelDescription,
                                                   string vin, string customerName, string colour, string mileage,
                                                   string destinationPool, string destinationLocationGroup,
                                                   string reservationStartdate, string reservationEnddate,
                                                   string sortExpression)
        {

            dueCountry = processString(dueCountry);
            ownCountry = processString(ownCountry);

            //// Presentation logic
            pool = ownCountry == "" ? "" : processString(pool);
            locationGroup = pool == "" ? "" : processString(locationGroup);
            carSegment = ownCountry == "" ? "" : processString(carSegment);
            carClass = carSegment == "" ? "" : processString(carClass);
            carGroup = carClass == "" ? "" : processString(carGroup);

            FilterEntity fe = conditionFilters(dueCountry,
                                                0,
                                                ownCountry,
                                                pool,
                                                locationGroup,
                                                carSegment,
                                                carClass,
                                                carGroup,
                                                destinationPool,
                                                destinationLocationGroup
                                                );

            // check the dates are valid
            if (string.IsNullOrEmpty(reservationStartdate) || string.IsNullOrEmpty(reservationEnddate))
            {
                fe.ReservationStartDate = DateTime.Now.Date;
                fe.ReservationEndDate = DateTime.Now.AddDays(2).Date;
            }
            else
            {
                fe.ReservationStartDate = Convert.ToDateTime(reservationStartdate);
                fe.ReservationEndDate = Convert.ToDateTime(reservationEnddate);
            }

            CarFilterEntity cfe = new CarFilterEntity
            {
                Unit = unit,
                License = license,
                Model = model,
                ModelDesc = modelDescription,
                Vin = vin,
                Name = customerName,
                Colour = colour,
                Mileage = mileage
            };
            if (cfe.ModelDesc != "ResDetails")
                return getReservationDetailsMatches(fe, cfe, sortExpression);
            else
                return OldgetReservationDetails(fe, cfe, sortExpression);
        }

        private List<ReservationMatchEntity> getReservationDetailsMatches(FilterEntity filters, CarFilterEntity cf, string sortExpression)
        {
            return new ReservationFleetRepository().GetList(filters, cf, sortExpression).ToList();
        }

        private List<ReservationMatchEntity> OldgetReservationDetails(FilterEntity filters, CarFilterEntity cf, string sortExpression)
        {

            using (MarsDBDataContext db = new MarsDBDataContext())
            {

                try
                {  // volatile db code


                    var q = from p in db.Reservations
                        join startloc in db.LOCATIONs on p.RENT_LOC equals startloc.dim_Location_id
                        // Return Location
                        join returnloc in db.LOCATIONs on p.RTRN_LOC equals returnloc.dim_Location_id
                        join returnCmsLoc in db.CMS_LOCATION_GROUPs on returnloc.cms_location_group_id equals
                            returnCmsLoc.cms_location_group_id
                        join returnCmsP in db.CMS_POOLs on returnCmsLoc.cms_pool_id equals returnCmsP.cms_pool_id
                        join returnCtry in db.COUNTRies on returnCmsP.country equals returnCtry.country1
                        // Car details
                        join carGp in db.CAR_GROUPs on p.GR_INCL_GOLDUPGR equals carGp.car_group_id
                        join carCs in db.CAR_CLASSes on carGp.car_class_id equals carCs.car_class_id
                        join carS in db.CAR_SEGMENTs on carCs.car_segment_id equals carS.car_segment_id
                        // ======
                        where
                            (p.COUNTRY != returnCmsP.country) // used the country from the join
                            &&
                            (filters.OwnCountry == p.COUNTRY || filters.OwnCountry == "" || filters.OwnCountry == null)
                            && (returnCmsP.cms_pool1 == filters.Pool || filters.Pool == "" || filters.Pool == null)
                            &&
                            (returnCmsLoc.cms_location_group1 == filters.Location || filters.Location == "" ||
                             filters.Location == null)
                            &&
                            (p.ReservedCarGroup == filters.CarGroup || filters.CarGroup == "" ||
                             filters.CarGroup == null)
                            && (returnCtry.active) // only corporate countries
                            && (filters.CarSegment == carS.car_segment1 || string.IsNullOrEmpty(filters.CarSegment))
                            && (filters.CarClass == carCs.car_class1 || string.IsNullOrEmpty(filters.CarClass))
                            && (filters.CarGroup == carGp.car_group1 || string.IsNullOrEmpty(filters.CarGroup))
                            &&
                            (p.RS_ARRIVAL_DATE >= filters.ReservationStartDate &&
                             p.RS_ARRIVAL_DATE <= filters.ReservationEndDate) // reservation start dates
                            &&
                            (returnCmsP.country.Equals(filters.DueCountry) || string.IsNullOrEmpty(filters.DueCountry))
                            && (returnCmsP.cms_pool1.Equals(filters.DuePool) || string.IsNullOrEmpty(filters.DuePool))
                            &&
                            (returnCmsLoc.cms_location_group1.Equals(filters.DueLocationGroup) ||
                             string.IsNullOrEmpty(filters.DueLocationGroup))
                        select new ReservationMatchEntity
                               {
                                   ResLocation = startloc.served_by_locn,
                                   ResGroup = carGp.car_group1,
                                   ResCheckoutDate =
                                       new DateTime(p.RS_ARRIVAL_DATE.Value.Year, p.RS_ARRIVAL_DATE.Value.Month,
                                       p.RS_ARRIVAL_DATE.Value.Day, p.RS_ARRIVAL_TIME.Value.Hour,
                                       p.RS_ARRIVAL_TIME.Value.Minute, 0),
                                   ResCheckoutLoc = startloc.served_by_locn,
                                   ResCheckinLoc = returnloc.served_by_locn,
                                   ResId = p.RES_ID_NBR,
                                   ResNoDaysUntilCheckout = p.CO_DAYS.ToString(),
                                   ResNoDaysReserved = ((int) p.RES_DAYS).ToString(),
                                   ResDriverName = p.CUST_NAME,
                                   Matches = ""
                               };
             


                    switch (sortExpression)
                    {
                        case "ResLocation": q = q.OrderBy(p => p.ResLocation); break;
                        case "ResLocation DESC": q = q.OrderByDescending(p => p.ResLocation); break;
                        case "ResGroup": q = q.OrderBy(p => p.ResGroup); break;
                        case "ResGroup DESC": q = q.OrderByDescending(p => p.ResGroup); break;
                        case "ResCheckoutDate": q = q.OrderBy(p => p.ResCheckoutDate); break;
                        case "ResCheckoutDate DESC": q = q.OrderByDescending(p => p.ResCheckoutDate); break;
                        case "ResCheckinLoc": q = q.OrderBy(p => p.ResCheckinLoc); break;
                        case "ResCheckinLoc DESC": q = q.OrderByDescending(p => p.ResCheckinLoc); break;
                        case "ResNoDaysUntilCheckout": q = q.OrderBy(p => p.ResNoDaysUntilCheckout); break;
                        case "ResNoDaysUntilCheckout DESC": q = q.OrderByDescending(p => p.ResNoDaysUntilCheckout); break;
                        case "ResNoDaysReserved": q = q.OrderBy(p => p.ResNoDaysReserved); break;
                        case "ResNoDaysReserved DESC": q = q.OrderByDescending(p => p.ResNoDaysReserved); break;
                        case "ResDriverName": q = q.OrderBy(p => p.ResDriverName); break;
                        case "ResDriverName DESC": q = q.OrderByDescending(p => p.ResDriverName); break;
                        case "ResId": q = q.OrderBy(p => p.ResId); break;
                        case "ResId DESC": q = q.OrderByDescending(p => p.ResId); break;
                        case "Matches": q = q.OrderBy(p => p.Matches); break;
                        case "Matches DESC": q = q.OrderByDescending(p => p.Matches); break;
                        default: q = q.OrderBy(p => p.ResNoDaysUntilCheckout).ThenBy(p => p.ResLocation); break;
                    }

                    return q.ToList();
                }
                catch (Exception ex)
                {
                    //    _logger.Error("Exception thrown in ReservationDetails Model, exception = " + ex);
                    return new List<ReservationMatchEntity>();
                }
            }
        }


    }
}
