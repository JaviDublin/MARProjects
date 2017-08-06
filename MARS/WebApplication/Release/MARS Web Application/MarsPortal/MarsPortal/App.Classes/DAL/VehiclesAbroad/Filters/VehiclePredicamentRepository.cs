using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.DAL.VehiclesAbroad.Filters {
    public class VehiclePredicamentRepository : IFilterRepository {
        public IList<string> getList(params string[] dependants) {
            IList<string> l = new List<string>();
            l.Add("On rent: Owning country to foreign country");
            l.Add("Transfer: Owning country to foreign country");
            l.Add("Idle in foreign country");
            l.Add("On rent: in or between foreign country(ies)");
            l.Add("On rent: Returning to owning country");
            l.Add("Transfer: Returning to owning country");
            l.Add("Non-Revenue Vehicles");
            return l;
        }
    }
}