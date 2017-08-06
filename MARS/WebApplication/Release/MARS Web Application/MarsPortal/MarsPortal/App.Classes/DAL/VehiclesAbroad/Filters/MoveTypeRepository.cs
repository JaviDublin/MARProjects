using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Filters {
    public class MoveTypeRepository : IFilterRepository {
        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        public IList<string> getList(params string[] dependants) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                IList<string> l = new List<string>();
                try {
                    var q = (from p in db.FLEET_EUROPE_ACTUALs select p.MOVETYPE).Distinct().ToList();
                    foreach (var item in q) l.Add(item);
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in MoveTypeRepository, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}