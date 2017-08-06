using System;
using System.Collections.Generic;
using System.Data; // added for datatable class
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext; // added

namespace App.BLL.VehiclesAbroad {

    public class ReservationBalanceModel {

        /// <summary>
        /// get the reservation balance according to the following criteria
        /// the calculation is: Available Fleet + Known Oneway Check-Ins in the next 1, 3 or 7 days by owning country in destination country
        /// MINUS Reservations in the next X days + Known Open Oneway Rentals out of the destination country into the owning country
        /// the columns will be owning country and the rows will be destination country
        /// args is the number of days from todays date to take the calculation
        /// returns a datatable that can be bound at runtime to the Reservation Balance
        /// </summary>
        /// <param name="noOfDays">integer</param>
        /// <returns>DataTable</returns>
        public DataTable getReservationBalance(int noOfDays) {

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                DataTable dt = new DataTable("Reservation Balance");

                // create a dictionary to store the totals to add at the end of the table <string countries, int totals>
                Dictionary<string, int> totalsDictionary = new Dictionary<string, int>();

                // get the country names from the table
                var ownCountry = (from fea in db.FLEET_EUROPE_ACTUALs
                                  join c in db.COUNTRies on fea.COUNTRY equals c.country1
                                  where (!fea.COUNTRY.Trim().Equals("")) // get rid of any blanks
                                  && (c.active) // only a corporate country
                                  select new { fea.COUNTRY, c.country_description }).Distinct();

                // create columns starting with blank column
                dt.Columns.Add(" ");
                foreach (var owningCountry in ownCountry) {
                    dt.Columns.Add(owningCountry.country_description);
                    totalsDictionary.Add(owningCountry.country_description, 0); // add the country with a total of 0
                }

                // get all the destination countries
                var dueCountry = (from fea in db.FLEET_EUROPE_ACTUALs
                                  join c in db.COUNTRies on fea.DUEWWD.Substring(0, 2) equals c.country1
                                  where (!fea.DUEWWD.Substring(0, 2).Trim().Equals("")) // get rid of any blanks
                                  select new { duewwd = fea.DUEWWD.Substring(0, 2), c.country_description })
                                  .Distinct()
                                  .OrderBy(p => p.country_description);

                // calculate the date to calculate
                DateTime dateTo = DateTime.Now.Date.AddDays(noOfDays);

                DataRow dr;

                var dataset = from fea in db.FLEET_EUROPE_ACTUALs
                              join c in db.COUNTRies on fea.COUNTRY equals c.country1
                              where (fea.OPERSTAT.Equals("RT")) // rentable fleet
                              && (fea.DUEDATE >= dateTo) // the date to (1,3,7)
                              select new { fea, c };


                // loop through all the destination countries
                foreach (var destinationCountry in dueCountry) {
                    dr = dt.NewRow();
                    dr[" "] = destinationCountry.country_description;

                    // calculate the reservation balances

                    var outGoingBalance = dataset.Where(ds => ds.fea.LSTWWD.Substring(0, 2).Equals(ds.fea.COUNTRY)
                                                            && (ds.fea.DUEWWD.Substring(0, 2).Equals(destinationCountry.duewwd))
                                                            && (ds.c.active)) // corporate country
                                                            .GroupBy(ds => ds.c.country_description)
                                                            .Select(ogb => new { count = ogb.Count(), country = ogb.Key });

                    var inComingBalance = dataset.Where(ds => ds.fea.LSTWWD.Substring(0, 2).Equals(destinationCountry.duewwd)
                                                            && (ds.fea.DUEWWD.Substring(0, 2).Equals(ds.fea.COUNTRY)))
                                                            .GroupBy(ds => ds.c.country_description)
                                                            .Select(ogb => new { count = ogb.Count(), country = ogb.Key });

                    // work out the balance
                    foreach (var co in outGoingBalance) {

                        int? icb = inComingBalance.Where(i => i.country.Equals(co.country)).Select(p => p.count).FirstOrDefault();

                        int balance = co.count - icb ?? 0;
                        dr[co.country] = balance == 0 ? "" : balance.ToString(); // convert to string place a blank for zero 

                        // add to the total value of the own country dictionary
                        totalsDictionary[co.country] = totalsDictionary[co.country] + balance;
                    }

                    dt.Rows.Add(dr);
                }

                // add the total row
                dr = dt.NewRow();
                dr[" "] = "Totals";

                // loop through all the countries in the dictionary to get the totals
                foreach (KeyValuePair<string, int> total in totalsDictionary) {
                    dr[total.Key] = total.Value == 0 ? "" : total.Value.ToString();
                }
                dt.Rows.Add(dr);

                return dt;
            }
        }
    }
}