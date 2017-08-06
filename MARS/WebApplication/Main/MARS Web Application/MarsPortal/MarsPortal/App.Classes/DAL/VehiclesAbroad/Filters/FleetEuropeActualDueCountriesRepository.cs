using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Filters {
    public class FleetEuropeActualDueCountriesRepository : IFilterRepository {

        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        public IList<string> getList(params string[] dependants) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                List<string> l = new List<string>();
                try {
                    l.AddRange((from p in db.FLEET_EUROPE_ACTUALs
                                join tc in db.COUNTRies on (p.DUEWWD==null || p.DUEWWD =="" ? p.LSTWWD.Substring(0,2) : p.DUEWWD.Substring(0, 2)) equals tc.country1
                                select tc.country_description).Distinct());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in FleetEuropeActualDueCountriesRepository, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }
    }
}