using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.DAL.VehiclesAbroad.Filters {

    public class FakeRepository : IFilterRepository {

        public IList<string> getList(params string[] dependants) {
            IList<string> l = new List<string>();
            for (int i = 0; i < 10; i++)
                l.Add("TestData" + i);
            return l;
        }
    }
}