using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Reflection;
using App.BLL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;
using App.Classes.Entities.VehiclesAbroad;
using App.Classes.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.DAL.VehiclesAbroad {
    public class UtilisationHistoryRepository : IUtilisationHistoryRepository {
        //log4net.ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");

        IList<IDataTableEntity> _storedList = new List<IDataTableEntity>();
        public IList<IDataTableEntity> getList(IFilterEntity filters) {

            using (UtilisationRepositoryDataContext db = new UtilisationRepositoryDataContext()) {
                string owningCountry = filters.OwnCountry;
                DateTime startDate = filters.ReservationStartDate; // cheat!!!!
                DateTime endDate = filters.ReservationEndDate;
                _storedList.Clear(); // thank goodness for .net 4
                try
                {
                    var q = from p in db.GetVehiclesAbroadUtilization(owningCountry, startDate, endDate)
                        select new UtilisationHistoryEntity
                               {
                                   Local = (decimal) p.UTILIZATION_IN_COUNTRY*100,
                                   OutOfCountry = (decimal) p.UTILIZATION_OUT_OF_COUNTRY*100,
                                   OwnCountry = p.ownCountry,
                                   Rep_Date = p.REPDATE ?? DateTime.Now
                               };
                    foreach (var item in q)
                    {
                        _storedList.Add(new DataTableEntity
                                        {
                                            header = item.OwnCountry + "_out",
                                            rowDefinition = item.Rep_Date.ToShortDateString(),
                                            theValue = item.OutOfCountry.ToString("0.##")
                                        });
                        _storedList.Add(new DataTableEntity
                                        {
                                            header = item.OwnCountry + "_local",
                                            rowDefinition = item.Rep_Date.ToShortDateString(),
                                            theValue = item.Local.ToString("0.##")
                                        });
                    }
                }
                catch (Exception ex)
                {
                    //_logger.Error("Exception thrown in UtilisationHistoryRepository, message = " + ex.Message);
                }
                return _storedList;
            }
        }
        public IList<IDataTableEntity> getList4Chart() {
            return _storedList;
        }
    }
    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "MARSPortal")]
    public class UtilisationRepositoryDataContext : System.Data.Linq.DataContext {
        public UtilisationRepositoryDataContext() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString) {
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "VehiclesAbroad.GetUtilizationData")]
        public ISingleResult<UtilisationHistoryStoredProcEntity> GetVehiclesAbroadUtilization([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(10)")] string country, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> startDate, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "DateTime")] System.Nullable<System.DateTime> endDate) {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), country, startDate, endDate);
            return ((ISingleResult<UtilisationHistoryStoredProcEntity>)(result.ReturnValue));
        }
    }
}