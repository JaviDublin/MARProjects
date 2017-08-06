using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;


namespace App.DAL.VehiclesAbroad.Filters {
    public class CountryDescriptionRepository : ICountryDescriptionRepository, IFilterRepository {
        public string getCountryDescription(string countryCode) {
            //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");
            if (string.IsNullOrEmpty(countryCode)) return "";
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                try {
                    return (from p in db.COUNTRies where countryCode == p.country1 select p.country_description).FirstOrDefault<string>();
                }
                catch (Exception ex) {
                   // if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                return "";
            }
        }
        public IList<string> getList(params string[] dependants) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                return (from p in db.COUNTRies
                        where p.active
                        select p.country_description).ToList();
            }
        }
    }
}