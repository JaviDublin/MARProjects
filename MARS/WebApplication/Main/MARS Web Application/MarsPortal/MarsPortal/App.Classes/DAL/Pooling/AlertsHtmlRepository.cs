using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Pooling.Abstract;
using Mars.Entities.Pooling;
using Mars.DAL.Pooling.DataAccess;

namespace Mars.DAL.Pooling
{
    public class AlertsHtmlRepository : HtmlTableRepository<AlertEntity>
    {

        IList<AlertEntity> _l;
        public override IList<AlertEntity> GetTable(params String[] s)
        {
            using (GetPoolingAlertsStoredProcedure db = new GetPoolingAlertsStoredProcedure())
            {
                _l = new List<AlertEntity>();
                IList<AlertsReturnEntity> q = (from p in db.GetPoolingAlerts(s[0], s[1], s[2]) select p).ToList();
                var orderedByBalance = q.OrderByDescending(d => d.Balance);
                
                foreach (AlertsReturnEntity e in orderedByBalance)
                {
                    String s1 = e.Location + " " + e.VehicleGroup + " " + e.Balance;
                    if (e.resType == queryValues.NEXTHOUR.ToString()) insertList(s1, queryValues.NEXTHOUR);
                    if (e.resType == queryValues.FOLLOW4HOURS.ToString()) insertList(s1, queryValues.FOLLOW4HOURS);
                    if (e.resType == queryValues.RESTOFDAY.ToString()) insertList(s1, queryValues.RESTOFDAY);
                }
                return _l;
            }
        }
        void insertList(String s, queryValues qv)
        {
            var q = from p in _l select p;
            if (qv == queryValues.NEXTHOUR)
                foreach (var i in q)
                    if (String.IsNullOrEmpty(i.NextHour)) { i.NextHour = s; return; }
            if (qv == queryValues.FOLLOW4HOURS)
                foreach (var i in q)
                    if (String.IsNullOrEmpty(i.Follow4Hours)) { i.Follow4Hours = s; return; }
            if (qv == queryValues.RESTOFDAY)
                foreach (var i in q)
                    if (String.IsNullOrEmpty(i.RestOfDay)) { i.RestOfDay = s; return; }

            AlertEntity x = new AlertEntity();
            x.NextHour = qv == queryValues.NEXTHOUR ? s : String.Empty;
            x.Follow4Hours = qv == queryValues.FOLLOW4HOURS ? s : String.Empty;
            x.RestOfDay = qv == queryValues.RESTOFDAY ? s : String.Empty;
            _l.Add(x);
        }
    }
}