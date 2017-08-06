using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.DAL.Pooling {
    public class GridviewNoOfDaysRepository:IFilterRepository {
        public IList<string> getList(params string[] dependants) {

            return new List<string> { "1 Day", "3 Days", "7 Days", "30 Days" };
        }
    }
}