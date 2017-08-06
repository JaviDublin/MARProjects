using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.VehiclesAbroad.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.DAL.VehiclesAbroad.Filters {
    public class NoOfDaysRepository : IFilterRepository {

        public IList<string> getList(params string[] s) {
            return new List<string> { "1 day", "3 days", "7 days" };
        }
    }
}