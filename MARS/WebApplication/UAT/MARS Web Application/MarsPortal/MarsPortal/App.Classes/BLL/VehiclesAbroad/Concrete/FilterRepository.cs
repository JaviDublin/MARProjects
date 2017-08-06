using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.BLL.VehiclesAbroad.Concrete {
    public class FilterRepository : IFilterRepository {

        public IList<string> getOperstat() {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                IList<string> l = new List<string>();
                l.Add("***All***");
                var q = (from p in db.FLEET_EUROPE_ACTUALs select p.OPERSTAT).Distinct().ToList();
                foreach (var item in q) l.Add(item);
                return l;
            }
        }
        public IList<string> getMoveType() {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                IList<string> l = new List<string>();
                l.Add("***All***");
                var q = (from p in db.FLEET_EUROPE_ACTUALs select p.MOVETYPE).Distinct().ToList();
                foreach (var item in q) l.Add(item);
                return l;
            }
        }
    }
}