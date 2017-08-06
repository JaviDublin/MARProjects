using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using App.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Filters {
    public class CarSegmentRepository : IFilterRepository {
        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        public IList<string> getList(params string[] dependants) {

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                string country = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";

                try {
                    l.AddRange((from p in db.CAR_SEGMENTs
                                join c in db.COUNTRies on p.country equals c.country1
                                where c.country_description.Equals(country)
                                select p.car_segment1).ToList<string>());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in CarSegmentRepository, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}