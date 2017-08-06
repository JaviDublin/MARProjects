using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using Mars.Entities.Pooling;
using System.Reflection;

namespace Mars.DAL.Pooling.DataAccess {

    //[global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "MARSPortal")]
    //public class GeneralDayActualStoredProcedure : DataContext {
    //    public GeneralDayActualStoredProcedure ():
    //        base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString) { }

    //    [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.GetPoolingDayActuals")]
    //    public ISingleResult<DayActualEntity> GetPoolingDayActuals
    //        (String countryDescription, Int32 Logic, String PoolRegion, String LocationGrpArea, String Branch,
    //        String CarSegment, String CarClass, String CarGroup, String ThreeOrThirtyDays) {
    //        IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod()))
    //            , countryDescription, Logic, PoolRegion, LocationGrpArea, Branch, CarSegment, CarClass, CarGroup,ThreeOrThirtyDays);
    //        return ((ISingleResult<DayActualEntity>)(result.ReturnValue));
    //    }
    //}
}