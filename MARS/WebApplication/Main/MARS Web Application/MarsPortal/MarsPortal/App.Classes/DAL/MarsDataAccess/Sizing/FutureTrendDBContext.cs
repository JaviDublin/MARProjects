using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Reflection;
using Mars.Entities.Sizing;

namespace Mars.DAL.MarsDataAccess.Sizing {
    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "MARSPortal")]
    public class FutureTrendDBContext:DataContext {

        public FutureTrendDBContext() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString) { }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.FutureTrendDataAccess")]
        public ISingleResult<FutureTrendEntity> FutureTrendDataAccess(String country,String pool,String locGrp,
            String carSeg,String carCls,String carGrp,
            DateTime StartDate,DateTime EndDate,Int32 fleetPlanId) {
            IExecuteResult result = this.ExecuteMethodCall(this,((MethodInfo)(MethodInfo.GetCurrentMethod()))
                ,country,pool,locGrp,carSeg,carCls,carGrp,StartDate,EndDate,fleetPlanId);
            return ((ISingleResult<FutureTrendEntity>)(result.ReturnValue));
        }
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.FutureTrendDataAccessExcel")]
        public ISingleResult<FutureTrendExcelEntity> FutureTrendDataAccessExcel(String country,String pool,String locGrp,
            String carSeg,String carCls,String carGrp,
            DateTime StartDate,DateTime EndDate,Int32 fleetPlanId) {
            IExecuteResult result = this.ExecuteMethodCall(this,((MethodInfo)(MethodInfo.GetCurrentMethod()))
                ,country,pool,locGrp,carSeg,carCls,carGrp,StartDate,EndDate,fleetPlanId);
            return ((ISingleResult<FutureTrendExcelEntity>)(result.ReturnValue));
        }

        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.SupplyAnalysisDataAccessExcel")]
        public ISingleResult<SupplyAnalysisExcelEntity> SupplyAnalysisDataAccessExcel(String country, String pool, String locGrp,
            String carSeg, String carCls, String carGrp,
            DateTime StartDate, DateTime EndDate, Int32 fleetPlanId, bool weeklyGrouping)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod()))
                , country, pool, locGrp, carSeg, carCls, carGrp, StartDate, EndDate, fleetPlanId, weeklyGrouping);
            return ((ISingleResult<SupplyAnalysisExcelEntity>)(result.ReturnValue));
        }
    }
}