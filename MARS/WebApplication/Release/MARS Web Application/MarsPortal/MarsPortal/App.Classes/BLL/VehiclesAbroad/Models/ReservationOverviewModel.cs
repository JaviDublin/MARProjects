﻿using System.Collections.Generic;
using System.Linq;
// added for datatable class
using Mars.App.Classes.DAL.MarsDBContext; // added
using App.Entities.VehiclesAbroad; // added
using App.BLL.VehiclesAbroad.Abstract;

namespace App.BLL.VehiclesAbroad {

    public class ReservationOverviewModel : GeneralFilterModel {

        public string getReservationOverview(FilterEntity fe) {
            // Returns a string that consists of a HTML table with the Owning country vehicles in a foreign country

         
            // create an instance of DataTableFactory which generates the required HTML 
            // according the the data from the LINQ generated by this getList method
            Factories.DataTableHTMLFactory dtf = new Factories.DataTableHTMLFactory(getList(fe));

            // return the required string
            return dtf.getDataTableAsString("Return/Start Country");
        }

        private IList<IDataTableEntity> getList(FilterEntity filters) {

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                IList<IDataTableEntity> l = new List<IDataTableEntity>();

                try
                {
                    // this is volatile code


                    // Gets reservations based on selected criteria
                    var q = (from p in db.Reservations
                        join startloc in db.LOCATIONs on p.RENT_LOC equals startloc.dim_Location_id
                        join startCmsLoc in db.CMS_LOCATION_GROUPs on startloc.cms_location_group_id equals
                            startCmsLoc.cms_location_group_id
                        join startCmsP in db.CMS_POOLs on startCmsLoc.cms_pool_id equals startCmsP.cms_pool_id
                        join startCtry in db.COUNTRies on startCmsP.country equals startCtry.country1
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
                            && (startCmsP.cms_pool1 == filters.Pool || filters.Pool == "" || filters.Pool == null)
                            &&
                            (startCmsLoc.cms_location_group1 == filters.Location || filters.Location == "" ||
                             filters.Location == null)
                            &&
                            (p.ReservedCarGroup == filters.CarGroup || filters.CarGroup == "" ||
                             filters.CarGroup == null)
                            && (startCtry.active) // only corporate countries
                            && (filters.CarSegment == carS.car_segment1 || string.IsNullOrEmpty(filters.CarSegment))
                            && (filters.CarClass == carCs.car_class1 || string.IsNullOrEmpty(filters.CarClass))
                            && (filters.CarGroup == carGp.car_group1 || string.IsNullOrEmpty(filters.CarGroup))
                            &&
                            (p.RS_ARRIVAL_DATE >= filters.ReservationStartDate &&
                             p.RS_ARRIVAL_DATE <= filters.ReservationEndDate) // reservation start dates
                                // ====== filtering for the destination(start) location
                            &&
                            (returnCmsP.country.Equals(filters.DueCountry) || string.IsNullOrEmpty(filters.DueCountry))
                            && (returnCmsP.cms_pool1.Equals(filters.DuePool) || string.IsNullOrEmpty(filters.DuePool))
                            &&
                            (returnCmsLoc.cms_location_group1.Equals(filters.DueLocationGroup) ||
                             string.IsNullOrEmpty(filters.DueLocationGroup))
                        // ======
                        select new {p.RES_ID_NBR, startCtry, returnCtry}).Distinct();
                        // selecting RES_ID_NUMBER is to ensure all entries are unique

                    var grp = from p in q
                        group p by new {o = p.startCtry.country_description, d = p.returnCtry.country_description}
                        into g
                        select new {ownwwd = g.Key.o, duewwd = g.Key.d, sum = g.Key.o.Count()};

                    foreach (var item in grp)
                    {
                        l.Add(new DataTableEntity
                              {
                                  header = item.ownwwd,
                                  rowDefinition = item.duewwd,
                                  theValue = item.sum.ToString()
                              });
                    }
                }
                catch
                {
                    // do nothing
                }
                return l;
            }
        }

        public FilterEntity conditionFilters(string destinationCountry, int vehiclePredicament, string _owningCountry, string pool, string locationGroup,
                            string carSegment, string carClass, string carGroup,
                            string destinationPool, string destinationLocationGroup) {
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
    }
}