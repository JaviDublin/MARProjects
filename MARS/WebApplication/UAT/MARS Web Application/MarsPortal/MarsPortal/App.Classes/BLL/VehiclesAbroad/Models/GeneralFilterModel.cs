using System.Collections.Generic;
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext; // Added
using App.Entities.VehiclesAbroad;
using System;


namespace App.BLL.VehiclesAbroad {

    // a class that handles the general filter model and in inherited by the other classes
    public class GeneralFilterModel {

        //ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        // conditions the filters and takes care of null
        // returns the 'cleaned' filters as an entity
        public virtual FilterEntity conditionFilters(string destinationCountry, int vehiclePredicament, string _owningCountry, string pool, string locationGroup,
                                    string carSegment, string carClass, string carGroup) {

            FilterEntity fe = new FilterEntity();

            // helper to condition the filter input
            fe.DueCountry = processString(destinationCountry);
            fe.OwnCountry = processString(_owningCountry);
            fe.VehiclePredicament = vehiclePredicament;

            // Presentation logic - this arrangement is for the pool and location group to be populated from the destination country
            fe.Pool = fe.DueCountry == "" ? "" : processString(pool); // BEWARE!
            fe.Location = fe.Pool == "" ? "" : processString(locationGroup);
            fe.CarSegment = fe.OwnCountry == "" ? "" : processString(carSegment);
            fe.CarClass = fe.CarSegment == "" ? "" : processString(carClass);
            fe.CarGroup = fe.CarClass == "" ? "" : processString(carGroup);

            return fe;
        }

        // return the destination countries from the Fleet_Europe_Actual 
        // careful returns the full country name
        public List<string> getFeaDueCountries() {

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                l.Add("***All***");

                try {
                    l.AddRange((from p in db.FLEET_EUROPE_ACTUALs
                                join tc in db.COUNTRies on p.DUEWWD.Substring(0, 2) equals tc.country1
                                select tc.country_description).Distinct());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }

        // returns a list of the reservation location countries from the reservation_europe_actual table
        // returns the list as country descriptions
        public List<string> getReservationLocationCountries() {

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                l.Add("***All***");

                try {
                    l.AddRange((from p in db.Reservations
                                join c in db.COUNTRies on p.COUNTRY equals c.country1
                                where c.active
                                select c.country_description).Distinct());

                    // removed by Danmien 25.06.2014
                    //l.AddRange((from p in db.RESERVATIONS_EUROPE_ACTUALs
                    //            join c in db.COUNTRies on p.COUNTRY equals c.country1
                    //            where c.active
                    //            select c.country_description).Distinct());

                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }

        public List<string> getFeaOwnCountries() {
            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                l.Add("***All***");

                try {  // volatile db code

                    l.AddRange((from p in db.FLEET_EUROPE_ACTUALs
                                join tc in db.COUNTRies on p.COUNTRY equals tc.country1
                                where tc.active // only for Active corporate countries
                                select tc.country_description).Distinct());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }

        public List<string> getReservationReturnCountry() {
            // returns, as full country name, a list of the return countries from the reservation_europe_actual table

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                l.Add("***All***");

                try { // volatile db code (and just in case for the date strings but will not throw and exception)
                    

                    l.AddRange((from p in db.Reservations
                                // Return country
                                join rtnloc in db.LOCATIONs on p.RTRN_LOC equals rtnloc.dim_Location_id
                                join returnCountries in db.COUNTRies on rtnloc.country equals returnCountries.country1
                                // Start Country
                                join startloc in db.LOCATIONs on p.RENT_LOC equals startloc.dim_Location_id
                                join startCountries in db.COUNTRies on startloc.country equals startCountries.country1
                                where
                                    // ensure that the return is not the start country
                                 (p.COUNTRY != returnCountries.country1)
                                  &&
                                    (startCountries.active)
                                select returnCountries.country_description).Distinct());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }

        public List<string> getPoolList(string country) {
            // returns the pool list according to the country

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                l.Add("***All***");

                try {
                    l.AddRange((from p in db.CMS_POOLs
                                join c in db.COUNTRies on p.country equals c.country1
                                where c.country_description.Equals(processString(country))
                                select p.cms_pool1).ToList<string>());
                }
                catch (Exception ex) {
                   // if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }

        public List<string> getLocationList(string poolId, string country) {
            // Note the country parameter isn't used it's just to ensure this method is updated
            // when the Owning country is changed

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();

                l.Add("***All***");
                try {
                    l.AddRange((from p in db.CMS_LOCATION_GROUPs
                                join o in db.CMS_POOLs on p.cms_pool_id equals o.cms_pool_id
                                where o.cms_pool1.Equals(poolId)
                                select p.cms_location_group1).ToList<string>());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }

        public List<string> getCarSegment(string country) {

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                l.Add("***All***");
                try {
                    l.AddRange((from p in db.CAR_SEGMENTs
                                join c in db.COUNTRies on p.country equals c.country1
                                where c.country_description.Equals(country)
                                select p.car_segment1).ToList<string>());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }

        public List<string> getCarClass(string country, string carSegment) {

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                l.Add("***All***");

                try {
                    l.AddRange((from p in db.CAR_CLASSes
                                join o in db.CAR_SEGMENTs on p.car_segment_id equals o.car_segment_id
                                where o.country.Equals(getCountryCode(country))
                                && o.car_segment1.Equals(carSegment)
                                select p.car_class1).ToList<string>());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }

        public List<string> getCarGroup(string country, string carSegment, string carClass) {

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<string> l = new List<string>();
                l.Add("***All***");

                try {
                    l.AddRange((from p in db.CAR_GROUPs
                                join o in db.CAR_CLASSes on p.car_class_id equals o.car_class_id
                                join i in db.CAR_SEGMENTs on o.car_segment_id equals i.car_segment_id
                                where i.country.Equals(getCountryCode(country))
                                && i.car_segment1.Equals(carSegment)
                                && o.car_class1.Equals(carClass) // Note it seems the database or GUI has car class and car group mixed
                                select p.car_group1).ToList<string>());
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                l.Sort();
                return l;
            }
        }

        protected string processString(string s) {
            // returns the string and empty string if null or equal to "***All***"

            return s == "***All***" ? string.Empty : s ?? "";
        }

        protected string getCountryCode(string country) {
            // returns the two digit code for a country
            if (string.IsNullOrEmpty(country)) return "";
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                string returnString = string.Empty;
                try {
                    returnString = (from p in db.COUNTRies where country.Equals(p.country_description) select p.country1).FirstOrDefault<string>();
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                }
                return returnString ?? "";
            }
        }
        public string getLocationGroup(string location) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                try {
                    string rtrnString = (from loc in db.LOCATIONs
                                         join clg in db.CMS_LOCATION_GROUPs on loc.cms_location_group_id equals clg.cms_location_group_id
                                         where loc.location1 == location
                                         select clg.cms_location_group1).FirstOrDefault();
                    return rtrnString ?? "";
                }
                catch (Exception ex) {
                    //if (_logger != null) _logger.Error("Exception thrown in GeneralFilterModel, message : " + ex.Message);
                    return "";
                }
            }
        }
        protected int getNumberOfDays(string arg) {
            if (string.IsNullOrEmpty(arg)) return 0;
            int x = 0;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] s = arg.Split(delimiterChars);
            for (int i = 0; i < s.Count(); i++)
                if (int.TryParse(s[i], out x)) break;
            return x;
        }
    }
}