using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.DAL.Pooling {
    public class GridviewNoOfHoursRepository : IFilterRepository {

        public IList<string> getList(params string[] dependants) {
            IList<string> l = new List<string>();
            for (int i = 24; i <= 72; i = i + 24) {
                l.Add(i.ToString() + " Hours");
            }
            return l;
        }
    }
}