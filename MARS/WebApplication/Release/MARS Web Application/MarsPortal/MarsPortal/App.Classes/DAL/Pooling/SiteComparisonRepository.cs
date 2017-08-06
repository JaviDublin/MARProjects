using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.Linq;
using App.BLL.ExtensionMethods;
using Castle.Windsor.Installer;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.App.Classes.DAL.Pooling.SharedDataAccess;
using Mars.DAL.Pooling.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using Mars.Entities.Pooling;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.DAL.Pooling.Queryables;
using App.Classes.DAL.Pooling.Abstract;
using System.Data.SqlClient;

using Mars.DAL.Reservations.Queryables;
using System.Threading;
using Mars.DAL.Pooling.Filters;

namespace Mars.DAL.Pooling
{
    public class SiteComparisonRepository : IComparisonRepository
    {
        public delegate IList<DayActualEntity> GetDataDelegate(Enums.DayActualTime tme); // to run the queries async
        readonly int NoInArray = 73;
        public IMainFilterEntity Filter { get; set; }
        FeaSiteQueryable _feaSiteQ;
        ResSiteCIQueryable _resCISiteQ;
        ResSiteCOQueryable _resCOSiteQ;
        private readonly int DEFAULTVALUE = 0, FLAG = -999999;
        private ReservationsFilterCar _resCarFilterQ;
        private ReservationsSiteFilter _resFilterQ;
        private GetDataDelegate getFeaData, getPoolingCIData, getPoolingCOData;

        private static IList<ResTopic> _resTopics = TopicRepository.GetResTopicList();

        public SiteComparisonRepository()
        {
            _resCarFilterQ = new ReservationsFilterCar();
            _resFilterQ = new ReservationsSiteFilter();
            _feaSiteQ = new FeaSiteQueryable();
            _resCISiteQ = new ResSiteCIQueryable();
            _resCOSiteQ = new ResSiteCOQueryable();
            getFeaData = GetFeaData;
            getPoolingCIData = GetPoolingCIData;
            getPoolingCOData = GetPoolingCOData;
        }

        public IList<String[]> GetList(Enums.DayActualTime tme)
        {
            var hourlyColumns = tme == Enums.DayActualTime.THREE;
            var returned = new List<String[]>();
            var numberOfColumns = tme == Enums.DayActualTime.THIRTY ? 30 : (Int32)Enums.ThreeDayActuals.MAXNOOFCOLUMNS;
            var topicData = ReservationsDataAccess.CalculateTopics(hourlyColumns, numberOfColumns, Filter, true, true);
            int topicId = (from topic in _resTopics where topic.Name.Equals(Filter.Topic) select topic.Id).Single();

            foreach (string lbl in topicData.Select(p => p.Label).Distinct())
            {
                var s = new String[NoInArray];
                s[0] = lbl;

                for (int i = 0; i < NoInArray - 1; i++)
                {
                    var data = topicData.FirstOrDefault(d => d.Tme == i && d.Label == lbl);
                    int val = 0;
                    if (data != null)
                    {
                        switch (topicId)
                        {
                            case 1: val = data.Available; break;
                            case 2: val = data.Opentrips; break;
                            case 3: val = data.Reservations; break;
                            case 4: val = data.OnewayRes; break;
                            case 5: val = data.GoldServiceReservations; break;
                            case 6: val = data.PrepaidReservations; break;
                            //case 7: val = e3 == null ? 0 : e3.Predelivery; break;
                            case 8: val = data.Checkin; break;
                            case 9: val = data.OnewayCheckin; break;
                            case 10: val = data.LocalCheckIn; break;
                            case 11: val = data.Balance; break;
                            case 12: val = data.JustAdditions; break;
                            case 13: val = data.JustDeletions; break;
                        }    
                    }
                    
                    s[i + 1] = val.ToString(CultureInfo.InvariantCulture);
                }
                returned.Add(s);

            }

            var totals = new string[NoInArray];
            for (int i = 1; i < NoInArray; i++)
            {
                totals[i] = returned.Sum(p => Convert.ToDecimal(p.ElementAt(i))).ToString();
            }
            totals[0] = "Totals";
            returned.Add(totals);

            return returned;
        }


        public List<DayActualEntity> GetAdditionDeletionData(Enums.DayActualTime tme)
        {
            List<DayActualEntity> returned;

            using (var db = new MarsDBDataContext())
            {
                var additions = from ad in db.ResAdditions
                                where (ad.LOCATION.COUNTRy1.active)
                                   && (ad.LOCATION.COUNTRy1.country_description == Filter.Country || String.IsNullOrEmpty(Filter.Country))
                                   && (ad.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == Filter.PoolRegion || 
                                            ad.LOCATION.OPS_AREA.OPS_REGION.ops_region1 == Filter.PoolRegion || String.IsNullOrEmpty(Filter.PoolRegion))
                                   && (ad.LOCATION.CMS_LOCATION_GROUP.cms_location_group1 == Filter.LocationGrpArea || 
                                            ad.LOCATION.OPS_AREA.ops_area1 == Filter.LocationGrpArea || String.IsNullOrEmpty(Filter.LocationGrpArea))
                                   && (ad.LOCATION.location1 == Filter.Branch || String.IsNullOrEmpty(Filter.Branch))
                                   && (ad.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == Filter.CarSegment || String.IsNullOrEmpty(Filter.CarSegment))
                                   && (ad.CAR_GROUP.CAR_CLASS.car_class1 == Filter.CarClass || String.IsNullOrEmpty(Filter.CarClass))
                                   && (ad.CAR_GROUP.car_group1 == Filter.CarGroup || String.IsNullOrEmpty(Filter.CarGroup))
                                   && ad.RepDate > DateTime.Now
                                    group ad by new
                                    {
                                        k1 = (String.IsNullOrEmpty(Filter.Country) ? ad.LOCATION.COUNTRy1.country_description :
                                                String.IsNullOrEmpty(Filter.PoolRegion) ? Filter.CmsLogic 
                                                    ? ad.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 
                                                        : ad.LOCATION.OPS_AREA.OPS_REGION.ops_region1 :
                                                String.IsNullOrEmpty(Filter.LocationGrpArea) ? Filter.CmsLogic 
                                                    ? ad.LOCATION.CMS_LOCATION_GROUP.cms_location_group1 
                                                    : ad.LOCATION.OPS_AREA.ops_area1 :
                                                ad.LOCATION.location1),
                                        k2 = (tme == Enums.DayActualTime.THREE ? SqlMethods.DateDiffHour(DateTime.Now, ad.RepDate) 
                                                    : SqlMethods.DateDiffDay(DateTime.Now, ad.RepDate))
                                    }
                                    into g
                                select new DayActualEntity
                                {
                                    Tme = g.Key.k2,
                                    Label = g.Key.k1,
                                    AddditionDeletion = g.Sum(d=> d.Value),
                                    JustAdditions = g.Sum(d => d.Value),
                                    JustDeletions = 0,
                                };

                var deletions = from del in db.ResDeletions
                                where (del.LOCATION.COUNTRy1.active)
                                   && (del.LOCATION.COUNTRy1.country_description == Filter.Country || String.IsNullOrEmpty(Filter.Country))
                                   && (del.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == Filter.PoolRegion ||
                                            del.LOCATION.OPS_AREA.OPS_REGION.ops_region1 == Filter.PoolRegion || String.IsNullOrEmpty(Filter.PoolRegion))
                                   && (del.LOCATION.CMS_LOCATION_GROUP.cms_location_group1 == Filter.LocationGrpArea ||
                                            del.LOCATION.OPS_AREA.ops_area1 == Filter.LocationGrpArea || String.IsNullOrEmpty(Filter.LocationGrpArea))
                                   && (del.LOCATION.location1 == Filter.Branch || String.IsNullOrEmpty(Filter.Branch))
                                   && (del.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == Filter.CarSegment || String.IsNullOrEmpty(Filter.CarSegment))
                                   && (del.CAR_GROUP.CAR_CLASS.car_class1 == Filter.CarClass || String.IsNullOrEmpty(Filter.CarClass))
                                   && (del.CAR_GROUP.car_group1 == Filter.CarGroup || String.IsNullOrEmpty(Filter.CarGroup))
                                   && del.RepDate > DateTime.Now
                                group del by new
                                    {
                                        k1 = (String.IsNullOrEmpty(Filter.Country) ? del.LOCATION.COUNTRy1.country_description :
                                                String.IsNullOrEmpty(Filter.PoolRegion) ? Filter.CmsLogic 
                                                    ? del.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 
                                                        : del.LOCATION.OPS_AREA.OPS_REGION.ops_region1 :
                                                String.IsNullOrEmpty(Filter.LocationGrpArea) ? Filter.CmsLogic 
                                                    ? del.LOCATION.CMS_LOCATION_GROUP.cms_location_group1 
                                                    : del.LOCATION.OPS_AREA.ops_area1 :
                                                del.LOCATION.location1),
                                        k2 = (tme == Enums.DayActualTime.THREE ? SqlMethods.DateDiffHour(DateTime.Now, del.RepDate) 
                                                    : SqlMethods.DateDiffDay(DateTime.Now, del.RepDate))
                                    }
                                    into g
                                select new DayActualEntity
                                {
                                    Tme = g.Key.k2,
                                    Label = g.Key.k1,
                                    AddditionDeletion = g.Sum(d=> d.Value * -1),
                                    JustAdditions = 0,
                                    JustDeletions = g.Sum(d=> d.Value * -1)
                                };
                var additionsAndDeletions = additions.Union(deletions).ToList();

                returned = (from ad in additionsAndDeletions
                    group ad by new
                                {
                                    k1 = ad.Tme,
                                    k2 = ad.Label
                                }
                    into g
                    select new DayActualEntity
                           {
                               Tme= g.Key.k1,
                               Label = g.Key.k2,
                               AddditionDeletion = g.Sum( d=> d.AddditionDeletion),
                               JustAdditions = g.Sum(d=> d.JustAdditions),
                               JustDeletions = g.Sum(d => d.JustDeletions)
                           }).ToList();

                
            }

            return returned;
        }

        IList<DayActualEntity> GetFeaData(Enums.DayActualTime tme)
        {
            using (MarsDBDataContext db = new MarsDBDataContext())
            {
                IList<DayActualEntity> l = new List<DayActualEntity>();
                try
                {
                    l = _feaSiteQ.GetQueryable(db, Filter, tme).OrderBy(p => p.Label).ToList();
                }
                catch (SqlException ex)
                {
                    //ILog _logger = log4net.LogManager.GetLogger("Pooling");
                    //if (_logger != null) _logger.Error(" SQL Exception thrown in SiteComparisonRepository accessing FLEET_EUROPE_ACTUAL table, message : " + ex.Message);
                }
                return l;
            }
        }

        IList<DayActualEntity> GetPoolingCIData(Enums.DayActualTime tme)
        {
            using (PoolingDataClassesDataContext db = new PoolingDataClassesDataContext())
            {
                IList<DayActualEntity> l = new List<DayActualEntity>();
                try
                {
                    IQueryable<App.Classes.DAL.Pooling.PoolingDataContext.Reservation> q2And3 = _resCarFilterQ.FilterByCarParameters(db, Filter, true);
                    q2And3 = _resFilterQ.FilterByReturnLocation(q2And3, Filter);
                    l = _resCISiteQ.GetQueryableCI(q2And3, Filter, tme, db).ToList();
                }
                catch (SqlException ex)
                {
                    //ILog _logger = log4net.LogManager.GetLogger("Pooling");
                   // if (_logger != null) _logger.Error(" SQL Exception thrown in SiteComparisonRepository accessing Reservations table, message : " + ex.Message);
                }
                return l;
            }
        }
        IList<DayActualEntity> GetPoolingCOData(Enums.DayActualTime tme)
        {
            using (PoolingDataClassesDataContext db = new PoolingDataClassesDataContext())
            {
                IList<DayActualEntity> l = new List<DayActualEntity>();
                try
                {
                    IQueryable<App.Classes.DAL.Pooling.PoolingDataContext.Reservation> q2And3 = _resCarFilterQ.FilterByCarParameters(db, Filter, false);
                    q2And3 = _resFilterQ.FilterByRentalLocation(q2And3, Filter);
                    l = _resCOSiteQ.GetQueryableCO(q2And3, Filter, tme).ToList();
                }
                catch (SqlException ex)
                {
                    //ILog _logger = log4net.LogManager.GetLogger("Pooling");
                   // if (_logger != null) _logger.Error(" SQL Exception thrown in SiteComparisonRepository accessing Reservations table, message : " + ex.Message);
                }
                return l;
            }
        }

    }
}