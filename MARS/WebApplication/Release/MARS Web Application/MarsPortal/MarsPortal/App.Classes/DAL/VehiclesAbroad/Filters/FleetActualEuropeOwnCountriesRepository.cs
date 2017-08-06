using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Filters {
    public class FleetActualEuropeOwnCountriesRepository : IFilterRepository {
        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        public IList<string> getList(params string[] dependants) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                try {  // volatile db code

                    l.AddRange((from p in db.FLEET_EUROPE_ACTUALs
                                join tc in db.COUNTRies on p.COUNTRY equals tc.country1
                                where tc.active // only for Active corporate countries
                                orderby tc.country_description // put into alphabetical order
                                select tc.country_description).Distinct());
                }
                catch (Exception ex) {
                   // if (_logger != null) _logger.Error("Exception thrown in FleetActualEuropeOwnCountriesRepository, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}