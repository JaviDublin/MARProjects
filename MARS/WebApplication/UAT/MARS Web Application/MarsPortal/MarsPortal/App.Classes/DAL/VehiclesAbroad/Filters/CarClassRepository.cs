using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Filters {

    public class CarClassRepository : IFilterRepository {
        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");
        public IList<string> getList(params string[] dependants) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                string country = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                string carSegment = dependants[1] == "***All***" ? string.Empty : dependants[1] ?? "";
                try {
                    l.AddRange((from p in db.CAR_CLASSes
                                join o in db.CAR_SEGMENTs on p.car_segment_id equals o.car_segment_id
                                join c in db.COUNTRies on o.country equals c.country1
                                where c.country_description == country
                                && o.car_segment1.Equals(carSegment)
                                select p.car_class1).ToList<string>());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in CarClassRepository, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}