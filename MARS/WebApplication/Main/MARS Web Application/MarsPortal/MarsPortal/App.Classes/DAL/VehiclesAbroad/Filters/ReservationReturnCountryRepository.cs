using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;


namespace App.DAL.VehiclesAbroad.Filters {
    public class ReservationReturnCountryRepository : IFilterRepository {
        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        public IList<string> getList(params string[] dependants) {
            // returns, as full country name, a list of the return countries from the reservation_europe_actual table

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();

                try { // volatile db code (and just in case for the date strings but will not throw and exception)

                    l.AddRange((from p in db.Reservations
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
                                where (p.COUNTRY != returnCtry.country_dw) // ensure that the return is not the start country
                                && (startCtry.active)
                                orderby returnCtry.country_description
                                select returnCtry.country_description).Distinct());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in ReservationReturnCountryRepository, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}