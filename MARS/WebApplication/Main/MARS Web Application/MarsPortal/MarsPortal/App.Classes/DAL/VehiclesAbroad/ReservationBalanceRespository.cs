using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.VehiclesAbroad.Abstract;
using System.Data;
using Mars.App.Classes.DAL.MarsDBContext;
using System.Data.Linq;
using System.Reflection;
using App.Classes.Entities.VehiclesAbroad;
using App.Entities.VehiclesAbroad.Abstract;

namespace App.Classes.DAL.VehiclesAbroad {
    public class ReservationBalanceRespository : IReservationBalanceRepository {

        public DataTable getTable(int noOfDays) {
            using (ReservationBalanceDataContext db = new ReservationBalanceDataContext()) {
                DataTable dt = new DataTable();
                DataRow dr;
                Dictionary<string, int> totalsDictionary = new Dictionary<string, int>();
                IList<ReservationBalanceEntity> l = db.ReservationBalance(DateTime.Now.Date, DateTime.Now.Date.AddDays(noOfDays - 1)).ToList();

                dt.Columns.Add("Destination/Owning");
                foreach (var owningCountry in (from p in l select p.owning).Distinct()) {
                    dt.Columns.Add(owningCountry);
                    totalsDictionary.Add(owningCountry, 0); // add the country with a total of 0
                }
                foreach (var desCountry in (from p in l select p.destination).Distinct()) {
                    dr = dt.NewRow();
                    dr["Destination/Owning"] = desCountry;
                    foreach (var owningCountry in (from p in l where p.destination == desCountry select p).Distinct()) {
                        dr[owningCountry.owning] = owningCountry.result;
                        totalsDictionary[owningCountry.owning] = totalsDictionary[owningCountry.owning] + owningCountry.result;
                    }
                    dt.Rows.Add(dr);
                }
                dr = dt.NewRow();
                dr["Destination/Owning"] = "Totals";
                foreach (var owningCountry in (from p in l select p).Distinct())
                    dr[owningCountry.owning] = totalsDictionary[owningCountry.owning];
                dt.Rows.Add(dr);
                return dt;
            }
        }
    }

    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "MARSPortal")]
    public class ReservationBalanceDataContext : DataContext {
        public ReservationBalanceDataContext() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString) {
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "VehiclesAbroad.ReservationBalance")]
        public ISingleResult<ReservationBalanceEntity> ReservationBalance([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> startDate, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> endDate) {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startDate, endDate);
            return ((ISingleResult<ReservationBalanceEntity>)(result.ReturnValue));
        }
    }
}