using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.Reservations.Queryables.SortRepository.Abstract;

namespace App.Classes.DAL.Reservations.Queryables.SortRepository {
    public class DetailsSortRepository : ISortRepository {
        IDictionary<string, string> dic;
        public DetailsSortRepository() {
            dic = new Dictionary<string, string>();
            dic.Add("RES_ID", "RES_ID_NBR");
            dic.Add("GR", "GR_INCL_GOLDUPGR");
            dic.Add("LOC_OUT", "RENT_LOC");
            dic.Add("DATE_OUT", "RS_ARRIVAL_DATE");
            dic.Add("OUT", "RS_ARRIVAL_TIME");
            dic.Add("LOC_IN", "RTRN_LOC");
            dic.Add("DATE_IN", "RTRN_DATE");
            dic.Add("IN", "RTRN_TIME");
            dic.Add("LN", "RES_DAYS");
            dic.Add("TARIFF", "RATE_QUOTED");
            dic.Add("CUSTOMER", "CUST_NAME");
            dic.Add("CDP", "CDPID_NBR");
            dic.Add("NO1", "NO1_CLUB_GOLD");
            dic.Add("FLT_NBR", "FLIGHT_NBR");
            dic.Add("GO", "GS");
            dic.Add("PP", "PREPAID");
        }
        public string getValue(string key) {
            return dic[key];
        }
    }
}