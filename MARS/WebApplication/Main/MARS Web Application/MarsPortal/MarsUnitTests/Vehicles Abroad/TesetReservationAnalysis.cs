using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using App.Classes.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace MarsUnitTests.Vehicles_Abroad
{
    [TestClass]
    public class TesetReservationAnalysis
    {
        [TestMethod]
        public void TestMethod1()
        {
            OldReservationAnalysisRepository oR = new OldReservationAnalysisRepository();
           var newData = oR.getDataTable("", "", 1);

            var oldData = oR.getOldDataTable("", "", 1);
        }



    }



    public class OldReservationAnalysisRepository : IReservationAnalysisRepository
    {

        public const string ROWHEADER = @"Country\Car_Class"; // this is the header text for the row column

        public DataTable getDataTable(string reservationCountry, string rtrnCountry, int noOfDays)
        {
            using (MarsDBDataContext db = new MarsDBDataContext())
            {

                DataTable dt = new DataTable("Reservation Analysis");

                // create a dictionary to store the totals at the bottom of the datatable
                Dictionary<string, int> totalsDictionary = new Dictionary<string, int>();
                
                // get the car classes (mapped to the car group?)
                var carClasses = (from res in db.Reservations
                    // rent Location
                    join rentloc in db.LOCATIONs on res.RENT_LOC equals rentloc.dim_Location_id
                    join rentCmsLoc in db.CMS_LOCATION_GROUPs on rentloc.cms_location_group_id equals
                        rentCmsLoc.cms_location_group_id
                    join rentCmsP in db.CMS_POOLs on rentCmsLoc.cms_pool_id equals rentCmsP.cms_pool_id
                    join rentCtry in db.COUNTRies on rentCmsP.country equals rentCtry.country1
                    // Car details
                    join carGp in db.CAR_GROUPs on res.GR_INCL_GOLDUPGR equals carGp.car_group_id
                    join carCs in db.CAR_CLASSes on carGp.car_class_id equals carCs.car_class_id
                    join carS in db.CAR_SEGMENTs on carCs.car_segment_id equals carS.car_segment_id
                    where
                        (reservationCountry == rentCtry.country_description || string.IsNullOrEmpty(reservationCountry) ||
                         reservationCountry == "***All***")
                    select carCs.car_class1).Distinct();

                // create columns starting with blank column
                dt.Columns.Add(ROWHEADER);
                foreach (var cc in carClasses)
                {
                    dt.Columns.Add(cc);
                    totalsDictionary.Add(cc.ToLower(), 0); // add the country with a total of 0
                }
                // calculate the date to calculate
                DateTime dateTo = DateTime.Now.Date.AddDays(noOfDays);

       // if the country argument has a valid value the just add the one country
                var countries = (from p in db.Reservations
                    join rentLoc in db.LOCATIONs on p.RENT_LOC equals rentLoc.dim_Location_id
                    join c in db.COUNTRies on rentLoc.country equals c.country1
                    where
                        (c.country_description.Equals(reservationCountry) || reservationCountry.Equals("***All***") ||
                         reservationCountry.Equals(""))
                    select new {p.COUNTRY, c.country_description}).Distinct();

                DataRow dr;
                foreach (var item1 in countries)
                {
                    dr = dt.NewRow();
                    dr[ROWHEADER] = item1.country_description;

                    // get the sums per car class for each country
                    var sums = (from p in db.Reservations
                        join rtnLoc in db.LOCATIONs on p.RTRN_LOC equals rtnLoc.dim_Location_id
                        join c in db.COUNTRies on rtnLoc.country equals c.country1
                        // Car details
                        join carGp in db.CAR_GROUPs on p.GR_INCL_GOLDUPGR equals carGp.car_group_id
                        join carCs in db.CAR_CLASSes on carGp.car_class_id equals carCs.car_class_id
                        where (p.COUNTRY != c.country1)
                              && (p.COUNTRY.Equals(item1.COUNTRY))
                              && (p.RS_ARRIVAL_DATE < dateTo && p.RS_ARRIVAL_DATE >= DateTime.Now.Date)
                            // the date to (1,3,7)
                              &&
                              (rtrnCountry == c.country_description || string.IsNullOrEmpty(rtrnCountry) ||
                               rtrnCountry == "***All***")
                        select new {p.RES_ID_NBR, p.COUNTRY, carCs.car_class1}).Distinct();
                    // selecting RES_ID_NUMBER is to ensure all entries are unique


                    var grp = from p in sums
                        group p by new {co = p.COUNTRY, cg = p.car_class1}
                        into g
                        select new {country = g.Key.co, carClass = g.Key.cg, sum = g.Key.cg.Count()};

                    // put each sum into the corresponding country row
                    foreach (var item2 in grp)
                    {
                        dr[item2.carClass] = item2.sum; // convert to string place a blank for zero

                        // add the number to the appropriate car classes total. some of the carclass are the same but with different capitals
                        totalsDictionary[item2.carClass.ToLower()] = totalsDictionary[item2.carClass.ToLower()] +
                                                                     item2.sum;
                    }
                    dt.Rows.Add(dr);
                }

                // add the final row of totals from the dictionary
                dr = dt.NewRow();
                dr[ROWHEADER] = "Totals";

                // loop through all the car classes in the dictionary to get the totals
                foreach (KeyValuePair<string, int> total in totalsDictionary)
                {
                    dr[total.Key] = total.Value == 0 ? "" : total.Value.ToString();
                }
                dt.Rows.Add(dr);

                return dt;
            }
        }


        public DataTable getOldDataTable(string reservationCountry, string rtrnCountry, int noOfDays)
        {
            using (MarsDBDataContext db = new MarsDBDataContext())
            {

                DataTable dt = new DataTable("Reservation Analysis");

                // create a dictionary to store the totals at the bottom of the datatable
                Dictionary<string, int> totalsDictionary = new Dictionary<string, int>();

                // removed  by Damien 25.06.2014
                // get the car classes (mapped to the car group?)
                var carClasses = (from res in db.RESERVATIONS_EUROPE_ACTUALs
                                  join c in db.COUNTRies on res.COUNTRY equals c.country1
                                  join cc in db.CAR_CLASSes on res.CAR_CLASS equals cc.car_class_id.ToString()
                                  where (reservationCountry == c.country_description || string.IsNullOrEmpty(reservationCountry) || reservationCountry == "***All***")
                                  select cc.car_class1).Distinct();

             

                // create columns starting with blank column
                dt.Columns.Add(ROWHEADER);
                foreach (var cc in carClasses)
                {
                    dt.Columns.Add(cc);
                    totalsDictionary.Add(cc.ToLower(), 0); // add the country with a total of 0
                }
                // calculate the date to calculate
                DateTime dateTo = DateTime.Now.Date.AddDays(noOfDays);

                //-- removed by Damien 25.06.2014
                //// if the country argument has a valid value the just add the one country
                var countries = (from p in db.RESERVATIONS_EUROPE_ACTUALs
                                 join c in db.COUNTRies on p.COUNTRY equals c.country1
                                 where (c.country_description.Equals(reservationCountry) || reservationCountry.Equals("***All***") || reservationCountry.Equals(""))
                                 select new { p.COUNTRY, c.country_description }).Distinct();


                              DataRow dr;
                foreach (var item1 in countries)
                {
                    dr = dt.NewRow();
                    dr[ROWHEADER] = item1.country_description;

                    // -- removed by Damien 25.06.2014
                    //// get the sums per car class for each country
                    var sums = (from p in db.RESERVATIONS_EUROPE_ACTUALs
                                join c in db.CAR_CLASSes on p.CAR_CLASS equals c.car_class_id.ToString()
                                join c1 in db.COUNTRies on p.RTRN_LOC.Substring(0, 2) equals c1.country1
                                where (p.COUNTRY != p.RTRN_LOC.Substring(0, 2))
                                && (p.COUNTRY.Equals(item1.COUNTRY))
                                && (p.RS_ARRIVAL_DATE < dateTo && p.RS_ARRIVAL_DATE >= DateTime.Now.Date) // the date to (1,3,7)
                                && (rtrnCountry == c1.country_description || string.IsNullOrEmpty(rtrnCountry) || rtrnCountry == "***All***")
                                select new { p.RES_ID_NBR, p.COUNTRY, c.car_class1 }).Distinct(); // selecting RES_ID_NUMBER is to ensure all entries are unique


              


                    var grp = from p in sums
                              group p by new { co = p.COUNTRY, cg = p.car_class1 }
                                  into g
                                  select new { country = g.Key.co, carClass = g.Key.cg, sum = g.Key.cg.Count() };

                    // put each sum into the corresponding country row
                    foreach (var item2 in grp)
                    {
                        dr[item2.carClass] = item2.sum; // convert to string place a blank for zero

                        // add the number to the appropriate car classes total. some of the carclass are the same but with different capitals
                        totalsDictionary[item2.carClass.ToLower()] = totalsDictionary[item2.carClass.ToLower()] +
                                                                     item2.sum;
                    }
                    dt.Rows.Add(dr);
                }

                // add the final row of totals from the dictionary
                dr = dt.NewRow();
                dr[ROWHEADER] = "Totals";

                // loop through all the car classes in the dictionary to get the totals
                foreach (KeyValuePair<string, int> total in totalsDictionary)
                {
                    dr[total.Key] = total.Value == 0 ? "" : total.Value.ToString();
                }
                dt.Rows.Add(dr);

                return dt;
            }
        }

    }
}
