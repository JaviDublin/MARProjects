using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;


namespace App.DAL.VehiclesAbroad.Filters {
    public class ReservationCountryRepository : IFilterRepository {
       // ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        // returns a list of the reservation location countries from the reservation_europe_actual table
        // returns the list as country descriptions
        public IList<string> getList(params string[] dependants) {

            using (MarsDBDataContext db = new MarsDBDataContext())
            {

                List<string> l = new List<string>();

                try
                {
                    l.AddRange((from p in db.Reservations
                                join c in db.COUNTRies on p.COUNTRY equals c.country1
                                where c.active
                                orderby c.country_description
                                select c.country_description).Distinct());

                }
                catch (Exception ex)
                {
                    //if (_logger != null)
                      //  _logger.Error("Exception thrown in ReservationCountryRepository, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}