using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.DAL.VehiclesAbroad.Filters {
    public class GridViewMaxRowsRepository : IFilterRepository {

        public IList<string> getList(params string[] dependants) {
            IList<string> l = new List<string>();
            for (int i = 10; i < 60; i = i + 10) {
                l.Add(i.ToString() + " Rows");
            }
            return l;
        }
    }
}