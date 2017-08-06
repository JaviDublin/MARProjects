using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
//
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Filters {

    public class PoolRepository : IFilterRepository {
        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        public IList<string> getList(params string[] dependants) {

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                string country = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";

                List<string> l = new List<string>();
                try {
                    l.AddRange((from p in db.CMS_POOLs
                                join c in db.COUNTRies on p.country equals c.country1
                                where c.country_description == country
                                orderby p.cms_pool1
                                select p.cms_pool1).ToList<string>());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in PoolRepository, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}