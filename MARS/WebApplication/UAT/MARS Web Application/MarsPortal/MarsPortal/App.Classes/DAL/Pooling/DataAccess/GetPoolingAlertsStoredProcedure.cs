using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using Mars.Entities.Pooling;
using System.Reflection;

namespace Mars.DAL.Pooling.DataAccess {

    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "MARSPortal")]
    public class GetPoolingAlertsStoredProcedure : DataContext {

        public GetPoolingAlertsStoredProcedure() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString) { }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.GetPoolingAlerts")]
        public ISingleResult<AlertsReturnEntity> GetPoolingAlerts(String hour,String fourHours,String endOfDay) {
            IExecuteResult result = this.ExecuteMethodCall(this,((MethodInfo)(MethodInfo.GetCurrentMethod())),hour,fourHours,endOfDay);
            return ((ISingleResult<AlertsReturnEntity>)(result.ReturnValue));
        }
    }
}