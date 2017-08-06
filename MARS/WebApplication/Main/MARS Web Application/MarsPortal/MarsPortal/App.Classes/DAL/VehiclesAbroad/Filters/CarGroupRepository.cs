using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;

using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Filters {

    public class CarGroupRepository : IFilterRepository {
        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        public IList<string> getList(params string[] dependants) {

            using (MarsDBDataContext db = new MarsDBDataContext()) {
                List<string> l = new List<string>();
                string country = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                string carSegment = dependants[1] == "***All***" ? string.Empty : dependants[1] ?? "";
                string carClass = dependants[2] == "***All***" ? string.Empty : dependants[2] ?? "";

                try {
                    l.AddRange((from p in db.CAR_GROUPs
                                join o in db.CAR_CLASSes on p.car_class_id equals o.car_class_id
                                join i in db.CAR_SEGMENTs on o.car_segment_id equals i.car_segment_id
                                join c in db.COUNTRies on i.country equals c.country1
                                where c.country_description == country
                                && i.car_segment1.Equals(carSegment)
                                && o.car_class1.Equals(carClass) // Note it seems the database or GUI has car class and car group mixed
                                select p.car_group1).ToList<string>());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in CarGroupRepository, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}