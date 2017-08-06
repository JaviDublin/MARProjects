using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.Pooling.Abstract;

namespace App.Classes.DAL.Pooling {
    public class HeaderRepository : IHeaderRepository {

        public string getHeader(Enums.Headers header) {
            return getDictionary()[header];
        }
        IDictionary<App.Classes.DAL.Pooling.Abstract.Enums.Headers, string> getDictionary() {
            IDictionary<App.Classes.DAL.Pooling.Abstract.Enums.Headers, string> dic = new Dictionary<App.Classes.DAL.Pooling.Abstract.Enums.Headers, string>();
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.Headers.alerts, "Alerts");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.Headers.threeFleetComparison, "Fleet Comparison - 3 Day Actuals");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.Headers.thirtyFleetComparison,"Fleet Comparison - 30 Day Actuals");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.Headers.reservationDetails, "Reservation Details");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.Headers.thirtySiteComparison, "Site Comparison - 30 Day Actuals");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.Headers.threeSiteComparison,"Site Comparison - 3 Day Actuals");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.Headers.thirtyDayActualStatus, "Status - 30 Day Actuals");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.Headers.threeDayActualStatus, "Status - 3 Day Actuals");
            return dic;
        }
    }
}