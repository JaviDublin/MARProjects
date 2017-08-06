using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.BLL.VehiclesAbroad.Models {

    public class NonrevModel : INonrevModel {

        public string getCountryCode(string s) {
            if (string.IsNullOrEmpty(s)) return "***All***";
            s = s.ToUpper().Split('_')[0];
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                return (from p in db.COUNTRies
                        where s == p.country1
                        select p.country_description).FirstOrDefault() ?? "***All***";
            }
        }

        public string getArgument(string s) {
            if (string.IsNullOrEmpty(s)) return "";
            return s.ToUpper().Split('_')[1];
        }
    }
}