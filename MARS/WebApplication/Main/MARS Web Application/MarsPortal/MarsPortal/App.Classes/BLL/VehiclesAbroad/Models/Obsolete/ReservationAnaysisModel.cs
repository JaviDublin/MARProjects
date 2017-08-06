using System;
using System.Collections.Generic;
using System.Data; // added for datatable class
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext; // added

namespace App.BLL.VehiclesAbroad
{

    public class ReservationAnaysisModel
    {

        public const string ROWHEADER = @"Country\Car_Class"; // this is the header text for the row column

        // - # removed by Damien 19.06.2014 
        // this entire class is unused


        ///// <summary>
        ///// get the reservation analysis from the Reservation_Europe_Actual table
        ///// which is a count of the car class group (mapped by group reservation)
        ///// the rows are the destination countries and the columns the car class groups
        ///// two filters: string country and int number of days (1/3/7 days)
        ///// returns a datatable that is bound to a GridView on the Reservation Analysis page
        ///// </summary>
        ///// <param name="country"></param>
        ///// <param name="noOfDays"></param>
        ///// <returns>DataTable</returns>
        //public DataTable getReservationAnalysis(string country, int noOfDays) {

        //    using (MarsDBDataContext db = new MarsDBDataContext()) {

        //        DataTable dt = new DataTable("Reservation Analysis");

        //        // create a dictionary to store the totals at the bottom of the datatable
        //        Dictionary<string, int> totalsDictionary = new Dictionary<string, int>();

        //        // get the car classes (mapped to the car group?)
        //        var carClasses = (from res in db.RESERVATIONS_EUROPE_ACTUALs
        //                          join c in db.COUNTRies on res.COUNTRY equals c.country1
        //                          join cc in db.CAR_CLASSes on res.CAR_CLASS equals cc.car_class_id.ToString()
        //                          where (country == c.country_description || string.IsNullOrEmpty(country) || country == "***All***")
        //                          select cc.car_class1).Distinct();

        //        // create columns starting with blank column
        //        dt.Columns.Add(ROWHEADER);
        //        foreach (var cc in carClasses) {
        //            dt.Columns.Add(cc);
        //            totalsDictionary.Add(cc.ToLower(), 0); // add the country with a total of 0
        //        }

        //        // calculate the date to calculate
        //        DateTime dateTo = DateTime.Now.Date.AddDays(noOfDays);

        //        // if the country argument has a valid value the just add the one country
        //        var countries = (from p in db.RESERVATIONS_EUROPE_ACTUALs
        //                         join c in db.COUNTRies on p.COUNTRY equals c.country1
        //                         where (c.country_description.Equals(country) || country.Equals("***All***") || country.Equals(""))
        //                         select new { p.COUNTRY, c.country_description }).Distinct();
        //        DataRow dr;
        //        foreach (var item1 in countries) {
        //            dr = dt.NewRow();
        //            dr[ROWHEADER] = item1.country_description;

        //            // get the sums per car class for each country
        //            var sums = (from p in db.RESERVATIONS_EUROPE_ACTUALs
        //                        join c in db.CAR_CLASSes on p.CAR_CLASS equals c.car_class_id.ToString()
        //                        join returnLocations in db.LOCATIONs on p.RTRN_LOC equals returnLocations.location1
        //                        join returnLocationGroups in db.CMS_LOCATION_GROUPs on returnLocations.cms_location_group_id equals returnLocationGroups.cms_location_group_id
        //                        join returnPools in db.CMS_POOLs on returnLocationGroups.cms_pool_id equals returnPools.cms_pool_id
        //                        // ======
        //                        where
        //                           (p.COUNTRY != returnPools.country) // used the country from the join
        //                        && (p.COUNTRY.Equals(item1.COUNTRY))
        //                        && (p.RS_ARRIVAL_DATE < dateTo && p.RS_ARRIVAL_DATE >= DateTime.Now.Date) // the date to (1,3,7)
        //                        select new { p.RES_ID_NBR, p.COUNTRY, c.car_class1 }).Distinct(); // selecting RES_ID_NUMBER is to ensure all entries are unique

        //            var grp = from p in sums
        //                      group p by new { co = p.COUNTRY, cg = p.car_class1 } into g
        //                      select new { country = g.Key.co, carClass = g.Key.cg, sum = g.Key.cg.Count() };

        //            // put each sum into the corresponding country row
        //            foreach (var item2 in grp) {
        //                dr[item2.carClass] = item2.sum; // convert to string place a blank for zero

        //                // add the number to the appropriate car classes total. some of the carclass are the same but with different capitals
        //                totalsDictionary[item2.carClass.ToLower()] = totalsDictionary[item2.carClass.ToLower()] + item2.sum;
        //            }
        //            dt.Rows.Add(dr);
        //        }
        //        // add the final row of totals from the dictionary
        //        dr = dt.NewRow();
        //        dr[ROWHEADER] = "Totals";

        //        // loop through all the car classes in the dictionary to get the totals
        //        foreach (KeyValuePair<string, int> total in totalsDictionary) {
        //            dr[total.Key] = total.Value == 0 ? "" : total.Value.ToString();
        //        }
        //        dt.Rows.Add(dr);

        //        return dt;
        //    }
        //}

        // - # removed by Damien 19.06.2014
        //public List<string> getReservationCountries() {
        //    // Get the reservation origins, distinct from the reservations table

        //    using (MarsDBDataContext db = new MarsDBDataContext()) {
        //        List<string> l = new List<string>();
        //        l.Add("***All***");
        //        l.AddRange((from p in db.RESERVATIONS_EUROPE_ACTUALs
        //                    join o in db.COUNTRies on p.RES_LOC.Substring(0, 2) equals o.country1
        //                    select o.country_description).Distinct().ToList<string>());
        //        return l;
        //    }
        //}
    }
}