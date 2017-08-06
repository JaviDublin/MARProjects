using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;


namespace App.DAL.VehiclesAbroad.Filters {
    public class LocationRepository : IFilterRepository {
        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");
        public IList<string> getList(params string[] dependants) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                string poolId = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                List<string> l = new List<string>();
                try {
                    l.AddRange((from p in db.CMS_LOCATION_GROUPs
                                join o in db.CMS_POOLs on p.cms_pool_id equals o.cms_pool_id
                                where o.cms_pool1.Equals(poolId)
                                select p.cms_location_group1).ToList<string>());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in LocationRepository, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}