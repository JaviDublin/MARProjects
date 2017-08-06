using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Pooling.Abstract;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.DAL.Pooling {
    public class CheckInOutRepository : IFilterRepository {

        public IList<string> getList(params string[] dependants) {
            return new List<string> { "Check Out", "Check In" };
        }
    }
}