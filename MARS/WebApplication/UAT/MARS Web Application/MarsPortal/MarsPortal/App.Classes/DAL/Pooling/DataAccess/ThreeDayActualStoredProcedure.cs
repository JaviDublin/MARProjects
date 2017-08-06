using System.Data.Linq;
using Mars.Entities.Pooling;
using System.Reflection;
using System;

namespace Mars.DAL.Pooling.DataAccess {

    //[global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "MARSPortal")]
    //public class ThreeDayActualStoredProcedure : DataContext {
    //    public ThreeDayActualStoredProcedure() :
    //        base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString) { }

    //    [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.GetPoolingThreeDayActuals")]
    //    public ISingleResult<DayActualEntity> GetPoolingThreeDayActuals
    //        (String countryDescription, Int32 Logic, String PoolRegion, String LocationGrpArea, String Branch,
    //        String CarSegment,String CarClass,String CarGroup) {
    //        IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod()))
    //            ,countryDescription,Logic,PoolRegion,LocationGrpArea,Branch,CarSegment,CarClass,CarGroup);
    //        return ((ISingleResult<DayActualEntity>)(result.ReturnValue));
    //    }
    //}
}
