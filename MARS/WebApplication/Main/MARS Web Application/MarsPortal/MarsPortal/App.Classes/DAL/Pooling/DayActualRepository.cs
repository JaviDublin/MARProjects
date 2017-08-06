using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using App.Classes.DAL.Reservations.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using System.Data;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.App.Classes.DAL.Pooling.SharedDataAccess;
using Mars.Entities.Pooling;
using Mars.DAL.Pooling.Queryables;
using System.Data.SqlClient;

using Mars.DAL.Reservations.Queryables;
using System.Threading;

namespace Mars.DAL.Pooling
{
    public class DayActualRepository : IReservationDayActualsRepository
    {
        public delegate IList<DayActualEntity> GetDataDelegate(IMainFilterEntity mfe); // to run the queries async
        Int32 MAXNOOFCOLUMNS = (Int32)Enums.ThreeDayActuals.MAXNOOFCOLUMNS;
        Int32 MAXNOOFROWS = (Int32)Enums.ThreeDayActuals.MAXNOOFROWS;
        Int32 ZERO = (Int32)Enums.ThreeDayActuals.ZERO;
        private readonly string DEFAULTVALUE = "0";
        IJavaScriptRepository JavascriptRepository;
        Enums.DayActualTime _time;
        FeaPoolingDataAccess _feq;
        ResActualCIQueryable _reqCi;
        PoolingCheckOutReservations _reqCo;
        FeaFilteredQueryable _feaFilterQ;
        private ReservationsFilterCar _resCarFilterQ;
        private ReservationsSiteFilter _resFilterQ;
        public DataTable _DataTable { get; set; }
        private GetDataDelegate getFeaData, getPoolingCIData, getPoolingCOData;
        public DayActualRepository(IJavaScriptRepository j) : this(j, Enums.DayActualTime.THREE) { }
        public DayActualRepository(IJavaScriptRepository j, Enums.DayActualTime Time)
        {
            JavascriptRepository = j;
            _time = Time;
            _feq = new FeaPoolingDataAccess();
            _resCarFilterQ = new ReservationsFilterCar();
            _resFilterQ = new ReservationsSiteFilter();
            _reqCi = new ResActualCIQueryable();
            _reqCo = new PoolingCheckOutReservations();
            _feaFilterQ = new FeaFilteredQueryable();
            getFeaData = GetFeaData;
            getPoolingCIData = GetPoolingCIData;
            getPoolingCOData = GetPoolingCOData;
        }

        public DataTable GetTable(IMainFilterEntity mfe)
        {
            var hourlyColumns = _time == Enums.DayActualTime.THREE;
            var numberOfColumns = _time == Enums.DayActualTime.THIRTY ? 30 : MAXNOOFCOLUMNS;
            var topics = ReservationsDataAccess.CalculateTopics(hourlyColumns, numberOfColumns, mfe, false).ToDictionary(d=> d.Tme);

            var dataTable = new DataTable();

            var now = DateTime.Now.GetDateAndHourOnlyByCountry(mfe.Country);
            for (int i = 0; i < MAXNOOFCOLUMNS; i++)
            {
                dataTable.Columns.Add( hourlyColumns ? 
                    now.AddHours(i).ToString(CultureInfo.CurrentCulture)
                    : now.AddDays(i).ToString(CultureInfo.CurrentCulture), typeof(string)
                    );
            }

            dataTable.Rows.Add(topics.Values.Select(d => d.Balance.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.Available.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.Opentrips.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.Reservations.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.OnewayRes.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.GoldServiceReservations.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.PrepaidReservations.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.Checkin.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.OnewayCheckin.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.Offset.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.LocalCheckIn.ToString()).ToArray()); 
            dataTable.Rows.Add(topics.Values.Select(d => d.Buffer.ToString()).ToArray());
            dataTable.Rows.Add(topics.Values.Select(d => d.AddditionDeletion.ToString()).ToArray());

            _DataTable = dataTable;
            return dataTable;
        }


        public string GetJavascript(params string[] s)
        {
            return JavascriptRepository.getJavascript();
        }

        private static Dictionary<int, int> GetCurrentAdditionDeletion(IMainFilterEntity filter, MarsDBDataContext db, bool hourly)
        {
            var returned = new Dictionary<int, int>();
            var startingTimePeriod = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            var endingTimePeriod = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            if (hourly)
            {
                startingTimePeriod = startingTimePeriod.Date.AddHours(startingTimePeriod.Hour);
                endingTimePeriod = endingTimePeriod.Date.AddHours(endingTimePeriod.Hour);
                endingTimePeriod = endingTimePeriod.AddHours(1).AddSeconds(-1);
            }
            else
            {
                startingTimePeriod = startingTimePeriod.Date;
                endingTimePeriod = endingTimePeriod.Date.AddDays(1).AddSeconds(-1);
            }
            

            var additions = (from p in db.ResAdditions
                                    join loc in db.LOCATIONs on p.LocId equals loc.dim_Location_id
                                    where (loc.COUNTRy1.active)
                                    && p.RepDate >= startingTimePeriod
                                    && (loc.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                                    && (loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == filter.PoolRegion || loc.OPS_AREA.OPS_REGION.ops_region1 == filter.PoolRegion || String.IsNullOrEmpty(filter.PoolRegion))
                                    && (loc.CMS_LOCATION_GROUP.cms_location_group1 == filter.LocationGrpArea || loc.OPS_AREA.ops_area1 == filter.LocationGrpArea || String.IsNullOrEmpty(filter.LocationGrpArea))
                                    && (loc.location1 == filter.Branch || String.IsNullOrEmpty(filter.Branch))
                                    && (p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                                    && (p.CAR_GROUP.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                                    && (p.CAR_GROUP.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                                    select p).ToList();

            var deletions = (from p in db.ResDeletions
                                    join loc in db.LOCATIONs on p.LocId equals loc.dim_Location_id
                                    where (loc.COUNTRy1.active)
                                    && p.RepDate >= startingTimePeriod
                                    && (loc.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                                    && (loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == filter.PoolRegion || loc.OPS_AREA.OPS_REGION.ops_region1 == filter.PoolRegion || String.IsNullOrEmpty(filter.PoolRegion))
                                    && (loc.CMS_LOCATION_GROUP.cms_location_group1 == filter.LocationGrpArea || loc.OPS_AREA.ops_area1 == filter.LocationGrpArea || String.IsNullOrEmpty(filter.LocationGrpArea))
                                    && (loc.location1 == filter.Branch || String.IsNullOrEmpty(filter.Branch))
                                    && (p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                                    && (p.CAR_GROUP.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                                    && (p.CAR_GROUP.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                                    select p).ToList();


            for (var i = 0; i <= (Int32)Enums.ThreeDayActuals.MAXNOOFCOLUMNS; i++)
            {
                int previousAdditions, previousDeletions;
                int timeAddition = i;
                
                if (hourly)
                {
                    var newEndTime = endingTimePeriod.AddHours(timeAddition);
                    var newStartTime = startingTimePeriod.AddHours(timeAddition);

                    previousAdditions = additions.Where(p => p.RepDate >= newStartTime
                                                    && p.RepDate <= newEndTime)
                            .Select(p => p.Value).Sum();
                    previousDeletions = deletions.Where(p => p.RepDate >= newStartTime
                                                    && p.RepDate <= newEndTime)
                            .Select(p => p.Value).Sum();
                }
                else
                {
                    var newEndTime = endingTimePeriod.AddDays(timeAddition);
                    var newStartTime = startingTimePeriod.AddDays(timeAddition);
                    previousAdditions = additions.Where(p => p.RepDate >= newStartTime
                                                    && p.RepDate <= newEndTime)
                            .Select(p => p.Value).Sum();
                    previousDeletions = deletions.Where(p => p.RepDate >= newStartTime
                                                    && p.RepDate <= newEndTime)
                            .Select(p => p.Value).Sum();
                }

                returned.Add(i, previousAdditions - previousDeletions);
            }

            return returned;
        }

        private int GetBuffers(IMainFilterEntity filter, MarsDBDataContext db)
        {
            var bufferData = from p in db.ResBuffers
                           join loc in db.LOCATIONs on p.LocId equals loc.dim_Location_id
                           where (loc.COUNTRy1.active)
                           && (loc.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                           && (loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == filter.PoolRegion || loc.OPS_AREA.OPS_REGION.ops_region1 == filter.PoolRegion || String.IsNullOrEmpty(filter.PoolRegion))
                           && (loc.CMS_LOCATION_GROUP.cms_location_group1 == filter.LocationGrpArea || loc.OPS_AREA.ops_area1 == filter.LocationGrpArea || String.IsNullOrEmpty(filter.LocationGrpArea))
                           && (loc.location1 == filter.Branch || String.IsNullOrEmpty(filter.Branch))
                           && (p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                           && (p.CAR_GROUP.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                           && (p.CAR_GROUP.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                           select p;
            
            if (bufferData.Any())
            {
                var returned = bufferData.Sum(d => d.Value);
                return returned;    
            }
            return 0;

        }

        IList<DayActualEntity> GetFeaData(IMainFilterEntity mfe)
        {
            using (var db = new MarsDBDataContext())
            {
                IList<DayActualEntity> feaData = new List<DayActualEntity>();
                try
                {
                    //db.Log = new DebugTextWriter();
                    var checkInData = _feaFilterQ.GetFeaCheckOut(db, mfe);
                    
                    var checkoutData = _feaFilterQ.GetFeaCheckIn(db, mfe);
                    feaData = _feq.GetFeaDataWithoutLabels(checkInData, checkoutData, _time, mfe, db).ToList();
                }
                catch (SqlException ex)
                {
                    //ILog _logger = LogManager.GetLogger("Pooling");
                    //if (_logger != null) _logger.Error(" SQL Exception thrown in DayActualRepository accessing FLEET_EUROPE_ACTUAL table, message : " + ex.Message);
                }
                return feaData;
            }
        }
        IList<DayActualEntity> GetPoolingCIData(IMainFilterEntity mfe)
        {
            using (var db = new PoolingDataClassesDataContext())
            {
                IList<DayActualEntity> l = new List<DayActualEntity>();
                try
                {
                    IQueryable<App.Classes.DAL.Pooling.PoolingDataContext.Reservation> q2And3 = _resCarFilterQ.FilterByCarParameters(db, mfe, true);
                    q2And3 = _resFilterQ.FilterByReturnLocation(q2And3, mfe);
                    l = _reqCi.GetQueryable(q2And3, mfe, _time).ToList();
                }
                catch (SqlException ex)
                {
                    //ILog _logger = LogManager.GetLogger("Pooling");
                    //if (_logger != null) _logger.Error(" SQL Exception thrown in DayActualRepository accessing Reservations table, message : " + ex.Message);
                }
                return l;
            }
        }
        IList<DayActualEntity> GetPoolingCOData(IMainFilterEntity mfe)
        {
            using (var db = new PoolingDataClassesDataContext())
            {
                IList<DayActualEntity> l = new List<DayActualEntity>();
                try
                {
                    IQueryable<App.Classes.DAL.Pooling.PoolingDataContext.Reservation> q2And3 = _resCarFilterQ.FilterByCarParameters(db, mfe, false);
                    q2And3 = _resFilterQ.FilterByRentalLocation(q2And3, mfe);
                    l = _reqCo.GetReservationsWithoutLabels(q2And3, mfe, _time).ToList();
                }
                catch (SqlException ex)
                {
                   // ILog _logger = LogManager.GetLogger("Pooling");
                  //  if (_logger != null) _logger.Error(" SQL Exception thrown in DayActualRepository accessing Reservations table, message : " + ex.Message);
                }
                return l;
            }
        }
    }
}