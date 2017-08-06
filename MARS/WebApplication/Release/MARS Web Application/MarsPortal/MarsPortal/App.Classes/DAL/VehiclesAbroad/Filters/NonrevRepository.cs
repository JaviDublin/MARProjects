using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.DAL.VehiclesAbroad.Filters {
    public class NonrevRepository : IFilterRepository {
        public IList<string> getList(params string[] dependants) {
            IList<string> l = new List<string>();
            l.Add("0 days");
            l.Add("3 days");
            l.Add("7 days");
            l.Add("30 days");
            l.Add("60 days");
            return l;
        }
    }
}