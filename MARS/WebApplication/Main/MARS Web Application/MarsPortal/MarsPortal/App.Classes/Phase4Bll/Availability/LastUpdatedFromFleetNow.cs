using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.Phase4Dal;


namespace Mars.App.Classes.Phase4Bll.Availability
{
    public static class LastUpdatedFromFleetNow
    {
        public static string GetLastUpdatedDateTime(BaseDataAccess da)
        {
            return da.GetLastFleetNowUpdate().ToString("dd/MM/yy HH:mm:ss") + " GMT";
        }
    }
}