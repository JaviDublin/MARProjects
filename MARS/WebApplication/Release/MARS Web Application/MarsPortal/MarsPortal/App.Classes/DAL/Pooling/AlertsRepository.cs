using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.App.Classes.DAL.Pooling.SharedDataAccess;
using Mars.DAL.Pooling.Abstract;
using Mars.Entities.Pooling;
using Mars.DAL.Pooling.Queryables;
using Mars.App.Classes.DAL.MarsDBContext;

using System.Data.SqlClient;
using Mars.DAL.Reservations.Queryables;
using System.Threading;

namespace Mars.DAL.Pooling
{

    public class AlertsRepository : HtmlTableRepository<AlertEntity>
    {
        public delegate IList<AlertTempEntity> GetDataDelegate(); // to run the queries async
        ResAlertsCOQueryable _resQCo;
        ResAlertsCIQueryable _resQCi;
        AlertsReturnQueryable _alertsRetQ;
        FeaAlertsQueryable _feaAlertsQ;
        private ReservationsFilterCar _resCarFilterQ;
        private ReservationsSiteFilter _resFilterQ;
        private GetDataDelegate d1, d2, d3;

        

        public AlertsRepository()
        {
            
            _resCarFilterQ = new ReservationsFilterCar();
            _resFilterQ = new ReservationsSiteFilter();
            _resQCo = new ResAlertsCOQueryable();
            _resQCi = new ResAlertsCIQueryable();
            _alertsRetQ = new AlertsReturnQueryable();
            _feaAlertsQ = new FeaAlertsQueryable();
            d1 = GetFeaData;
            d2 = GetPoolingCIData;
            d3 = GetPoolingCOData;
        }

        public override IList<AlertEntity> GetTable(params String[] s)
        {
            if (string.IsNullOrEmpty(Filter.Country))
                return new List<AlertEntity>();

            var alertClasses = AlertsDataAccess.GetLocationCarGroupsWithNegativeBalance(Filter, DateSelected);

            var alertsEnities = new List<AlertEntity>();
            var nextHours = new List<string>();
            var following4Hours = new List<string>();
            var restOfDay = new List<string>();
            var custom = new List<string>();

            var now = DateTime.Now;
            var hoursUntilEndOfToday = (now.Date.AddDays(1) - now).TotalHours + 1;
            foreach (var ac in alertClasses.OrderBy(d=> d.Balance))
            {
                var formattedEntity = string.Format("{0}|{1}", ac.Label, ac.Balance);
                if(ac.Tme == 0)
                    nextHours.Add(formattedEntity);
                if(ac.Tme > 0 && ac.Tme < 5)
                    following4Hours.Add(formattedEntity);
                if (ac.Tme >= 5 && ac.Tme <= hoursUntilEndOfToday)
                    restOfDay.Add(formattedEntity);
                if (ac.Tme > hoursUntilEndOfToday)
                    custom.Add(formattedEntity);
            }

            var maxEntities = Math.Max(nextHours.Count, Math.Max(following4Hours.Count, Math.Max(custom.Count,restOfDay.Count)));

            for (int i = 0; i < maxEntities; i++)
            {
                var ae = new AlertEntity();
                if (nextHours.Count > i)
                    ae.NextHour = nextHours[i];
                if (following4Hours.Count > i)
                    ae.Follow4Hours = following4Hours[i];
                if (restOfDay.Count > i)
                    ae.RestOfDay = restOfDay[i];
                if (custom.Count > i)
                    ae.Custom = custom[i];
                alertsEnities.Add(ae);
            }

            return alertsEnities;
        }

        public IList<AlertEntity> GetTable2(params String[] s)
        {
            DateSelected = DateSelected.AddDays(1).AddHours(-1);
            IAsyncResult ar1 = d1.BeginInvoke(null, null);
            IAsyncResult ar2 = d2.BeginInvoke(null, null);
            IAsyncResult ar3 = d3.BeginInvoke(null, null);
            while (!ar1.IsCompleted && !ar2.IsCompleted && !ar3.IsCompleted) Thread.Sleep(50); //wait
            IList<AlertTempEntity> l1 = d1.EndInvoke(ar1);
            IList<AlertTempEntity> l2 = d2.EndInvoke(ar2);
            IList<AlertTempEntity> l3 = d3.EndInvoke(ar3);
            IList<AlertTempEntity> l4 = new List<AlertTempEntity>();
            


            foreach (AlertTempEntity AmntRes in l3)
            {
                int AmtResCi = l2.Where(p => p.rsTime <= AmntRes.rsTime
                                    && p.rentLoc == AmntRes.rentLoc && p.crGrp == AmntRes.crGrp).Sum(p => p.Amnt);
                int AmntFea = l1.Where(p => p.rsTime <= AmntRes.rsTime
                                    && p.rentLoc == AmntRes.rentLoc && p.crGrp == AmntRes.crGrp).Sum(p => p.Amnt);

                if ((AmntFea - AmntRes.Amnt + AmtResCi) < 0)
                    l4.Add(new AlertTempEntity
                           {
                               rsTime = AmntRes.rsTime,
                               crGrp = AmntRes.crGrp,
                               rentLoc = AmntRes.rentLoc,
                               Amnt = AmntFea + AmtResCi - AmntRes.Amnt
                           });
            }
            l4 = l4.OrderBy(d => d.Amnt).ThenBy(d => d.rentLoc).ThenBy(d => d.crGrp).ToList();
            AlertEntity[] ax = _alertsRetQ.GetQueryable(l4).ToArray();

            var lx = new List<AlertEntity>();

            String[] a1 = ax.Where(p => !String.IsNullOrEmpty(p.NextHour)).Select(p => p.NextHour).ToArray();
            String[] a2 = ax.Where(p => !String.IsNullOrEmpty(p.Follow4Hours)).Select(p => p.Follow4Hours).ToArray();
            String[] a3 = ax.Where(p => !String.IsNullOrEmpty(p.RestOfDay)).Select(p => p.RestOfDay).ToArray();
            String[] a4 = ax.Where(p => !String.IsNullOrEmpty(p.Custom)).Select(p => p.Custom).ToArray();

            Boolean b = true;
            Int32[] i = { 0, 0, 0, 0};
            while (b)
            {
                string s1 = String.Empty, s2 = String.Empty, s3 = String.Empty, s4 = string.Empty;
                b = false;
                if (i[0] < a1.Length)
                {
                    s1 = a1[i[0]++];
                    b = true;
                }
                if (i[1] < a2.Length)
                {
                    s2 = a2[i[1]++];
                    b = true;
                }
                if (i[2] < a3.Length)
                {
                    s3 = a3[i[2]++];
                    b = true;
                }
                if (i[3] < a4.Length)
                {
                    s4 = a4[i[3]++];
                    b = true;
                }

                if (b) lx.Add(new AlertEntity
                {
                    NextHour = s1,
                    Follow4Hours = s2,
                    RestOfDay = s3,
                    Custom = s4,
                });
            }
            return lx;
        }

        IList<AlertTempEntity> GetFeaData()
        {
            using (var db = new MarsDBDataContext())
            {
                IList<AlertTempEntity> l = new List<AlertTempEntity>();
                try
                {
                    l = _feaAlertsQ.GetQueryable(db, Filter, DateSelected).ToList();
                }
                catch (SqlException ex)
                {
                   // ILog _logger = log4net.LogManager.GetLogger("Pooling");
                   // if (_logger != null) _logger.Error(" SQL Exception thrown in AlertsRepository accessing FLEET_EUROPE_ACTUAL table, message : " + ex.Message);
                }
                return l;
            }
        }
        IList<AlertTempEntity> GetPoolingCIData()
        {
            using (var db = new PoolingDataClassesDataContext())
            {
                var l = new List<AlertTempEntity>();
                try
                {


                    IQueryable<App.Classes.DAL.Pooling.PoolingDataContext.Reservation> q1 = _resCarFilterQ.FilterByCarParameters(db, Filter, true);

                    q1 = _resFilterQ.FilterByReturnLocation(q1, Filter);
                    l = _resQCi.GetQueryable(q1, DateSelected).ToList();
                    //l = l.OrderByDescending(d => d.Amnt).ToList();
                }
                catch (SqlException ex)
                {
                   // ILog _logger = log4net.LogManager.GetLogger("Pooling");
                   // if (_logger != null) _logger.Error(" SQL Exception thrown in AlertsRepository accessing Reservations table, message : " + ex.Message);
                }
                return l;
            }
        }
        IList<AlertTempEntity> GetPoolingCOData()
        {
            using (var db = new PoolingDataClassesDataContext())
            {
                //db.Log = new DebugTextWriter();
                var l = new List<AlertTempEntity>();
                try
                {
                    IQueryable<App.Classes.DAL.Pooling.PoolingDataContext.Reservation> q1 = _resCarFilterQ.FilterByCarParameters(db, Filter, false);
                    q1 = _resFilterQ.FilterByRentalLocation(q1, Filter);
                    l = _resQCo.GetQueryable(q1, DateSelected).ToList();
                    //l = l.OrderByDescending(d => d.Amnt).ToList();
                }
                catch (SqlException ex)
                {
                  //  ILog _logger = log4net.LogManager.GetLogger("Pooling");
//if (_logger != null) _logger.Error(" SQL Exception thrown in AlertsRepository accessing Reservations table, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}