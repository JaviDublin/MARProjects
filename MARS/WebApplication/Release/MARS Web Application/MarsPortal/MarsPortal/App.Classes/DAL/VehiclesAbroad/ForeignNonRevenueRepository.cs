using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using System.Data.Linq;
using System.Reflection;
using App.DAL.VehiclesAbroad.Abstract;
using App.BLL.VehiclesAbroad.Abstract;

namespace App.DAL.VehiclesAbroad {

    public class ForeignNonRevenueRepository : IFleetOverviewRepository {

        public IList<IDataTableEntity> getList(IFilterEntity filters) {
            return getList(filters.OwnCountry, filters.nonRev);
        }

        // helper to convert the data returned into a datatable format
        private IList<IDataTableEntity> getList(string country2, int daysrev) {
            IList<IDataTableEntity> l = new List<IDataTableEntity>();
            IList<ForeignNonRevenueEntity> listNonRev = getQueryable(country2, daysrev).ToList();
            List<string> owningCountries = (from p in listNonRev select p.OWNING_COUNTRY).Distinct().ToList();
            List<string> dueCountry = (from p in listNonRev where p.IN_COUNTRY != "TOTAL" select p.IN_COUNTRY).Distinct().ToList();

            // create the headers rent, shop and other
            foreach (var country in owningCountries) {
                foreach (var due in dueCountry) {
                    var numbers = (from p in listNonRev where p.IN_COUNTRY == due && p.OWNING_COUNTRY == country select new { rent = p.RENTABLE.ToString(), other = p.OTHER.ToString(), shop = p.SHOP.ToString() }).FirstOrDefault();
                    string sc = string.IsNullOrEmpty(country) ? "" : country;
                    string sd = string.IsNullOrEmpty(due) ? "" : due;
                    if (numbers != null) {
                        string rent = string.IsNullOrEmpty(numbers.rent) ? "" : numbers.rent;
                        string shop = string.IsNullOrEmpty(numbers.shop) ? "" : numbers.shop;
                        string other = string.IsNullOrEmpty(numbers.other) ? "" : numbers.other;
                        if (!(rent.Contains("0") && shop.Contains("0") && other.Contains("0"))) {
                            l.Add(new DataTableEntity { header = sc + "_Rent", rowDefinition = sd, theValue = rent });
                            l.Add(new DataTableEntity { header = sc + "_Shop", rowDefinition = sd, theValue = shop });
                            l.Add(new DataTableEntity { header = sc + "_Other", rowDefinition = sd, theValue = other });
                        }
                    }
                }
            }
            return l;
        }
        IList<ForeignNonRevenueEntity> getQueryable(string country, int daysrev) {
            using (ForeignNonRevenueRepositoryDataContext db = new ForeignNonRevenueRepositoryDataContext()) {
                return (from p in db.GetNonRevData(country, daysrev)
                        select new ForeignNonRevenueEntity { IN_COUNTRY = p.IN_COUNTRY, OTHER = p.OTHER, OWNING_COUNTRY = p.OWNING_COUNTRY, RENTABLE = p.RENTABLE, SHOP = p.SHOP }).ToList<ForeignNonRevenueEntity>();
            }
        }
    }
    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "MARSPortal")]
    public class ForeignNonRevenueRepositoryDataContext : System.Data.Linq.DataContext {
        public ForeignNonRevenueRepositoryDataContext() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString) {
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "VehiclesAbroad.GetNonRevData")]
        public ISingleResult<ForeignNonRevenueEntity> GetNonRevData([global::System.Data.Linq.Mapping.ParameterAttribute(Name = "COUNTRY", DbType = "VarChar(10)")] string cOUNTRY, [global::System.Data.Linq.Mapping.ParameterAttribute(Name = "DAYSREV", DbType = "Float")] System.Nullable<double> dAYSREV) {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), cOUNTRY, dAYSREV);
            return ((ISingleResult<ForeignNonRevenueEntity>)(result.ReturnValue));
        }
    }
}