using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using App.Classes.DAL.Pooling.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Queryables;
using Mars.DAL.Reservations.Queryables;
using Mars.Entities.Pooling;

namespace Mars.App.Classes.DAL.Pooling.SharedDataAccess
{
    public static class DayActualEntityRetrievers
    {

        private static ResSiteCIQueryable _resCISiteQ = new ResSiteCIQueryable();
        private static PoolingCheckInReservations _resCIFleetQ = new PoolingCheckInReservations();
        private static ResSiteCOQueryable _resCOSiteQ = new ResSiteCOQueryable();
        private static PoolingCheckOutReservationsWithLabels _resCOFleetQ = new PoolingCheckOutReservationsWithLabels();

        
        private static FeaPoolingDataAccess _feq = new FeaPoolingDataAccess();
 
        private static ResActualCIQueryable _reqCi = new ResActualCIQueryable();
        private static PoolingCheckOutReservations _reqCo = new PoolingCheckOutReservations();

        private static readonly FeaFilteredQueryable FeaFilter = new FeaFilteredQueryable();
        private static readonly ReservationsFilterCar CarParametersFilter = new ReservationsFilterCar();
        private static readonly ReservationsSiteFilter SiteParametersFilter = new ReservationsSiteFilter();

        public static List<DayActualEntity> GetFeaData(IMainFilterEntity mfe, bool hourlyTimeSlots, MarsDBDataContext db)
        {
            var checkOutData = FeaFilter.GetFeaCheckOut(db, mfe);
            var checkInData = FeaFilter.GetFeaCheckIn(db, mfe);

            var feaData = _feq.GetFeaDataWithoutLabels(checkOutData, checkInData,
                            hourlyTimeSlots ? Enums.DayActualTime.THREE : Enums.DayActualTime.THIRTY, mfe,db).ToList();

            var returned = feaData;

            return returned;
        }

        public static IQueryable<DayActualEntity> GetFeaFullForAlerts(IMainFilterEntity mfe, MarsDBDataContext db)
        {
            var checkOutData = FeaFilter.GetFeaCheckOut(db, mfe);

            var checkInData = FeaFilter.GetFeaCheckIn(db, mfe);

            var feaData = _feq.GetFullFeaDataForAlerts(checkOutData, checkInData, mfe, db);

            return feaData;
            
        }

        public static List<DayActualEntity> GetFeaDataWithLabel(IMainFilterEntity mfe, bool hourlyTimeSlots
                                                                            , MarsDBDataContext db, bool siteQ)
        {
            var checkOutData = FeaFilter.GetFeaCheckOut(db, mfe);
            var checkInData = FeaFilter.GetFeaCheckIn(db, mfe);

            var feaData = _feq.GetFeaDataWithLabels(checkOutData, checkInData,
                            hourlyTimeSlots ? Enums.DayActualTime.THREE : Enums.DayActualTime.THIRTY, siteQ,mfe,db).ToList();

            var returned = feaData;

            return returned;
        }

        public static List<DayActualEntity> GetPoolingCheckInData(IMainFilterEntity mfe, bool hourlyTimeSlots
                                                                            , PoolingDataClassesDataContext db)
        {
            var q2And3 = CarParametersFilter.FilterByCarParameters(db, mfe, true);
            q2And3 = SiteParametersFilter.FilterByReturnLocation(q2And3, mfe);


            var checkinData = _reqCi.GetQueryable(q2And3, mfe,
                hourlyTimeSlots ? Enums.DayActualTime.THREE : Enums.DayActualTime.THIRTY);

            var returned = checkinData.ToList();
            return returned;
        }

        public static List<DayActualEntity> GetPoolingCheckInDataWithLabels(IMainFilterEntity mfe, bool hourlyTimeSlots
                                                    , PoolingDataClassesDataContext db, bool siteQ)
        {
            var q2And3 = CarParametersFilter.FilterByCarParameters(db, mfe, true);
            q2And3 = SiteParametersFilter.FilterByReturnLocation(q2And3, mfe);
            var checkinData = siteQ
                    ? _resCISiteQ.GetQueryableCI(q2And3, mfe, hourlyTimeSlots ? Enums.DayActualTime.THREE : Enums.DayActualTime.THIRTY, db)
                    : _resCIFleetQ.GetFleetReservationsWithLabels(q2And3, mfe, hourlyTimeSlots ? Enums.DayActualTime.THREE : Enums.DayActualTime.THIRTY, db);

            var returned = checkinData.ToList();
            return returned;
        }

        public static List<DayActualEntity> GetPoolingEmptyAlertsHolders(MarsDBDataContext db, IMainFilterEntity filter)
        {
            if (string.IsNullOrEmpty(filter.Country)) return null;
            var locations = (from l in db.LOCATIONs
                where l.active
                      && l.COUNTRy1.country_description == filter.Country || string.IsNullOrEmpty(filter.Country)
                      && ((filter.CmsLogic
                          ? filter.PoolRegion == l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                          : filter.PoolRegion == l.OPS_AREA.OPS_REGION.ops_region1) ||
                       string.IsNullOrEmpty(filter.PoolRegion))
                      && ((filter.CmsLogic
                          ? filter.LocationGrpArea == l.CMS_LOCATION_GROUP.cms_location_group1
                          : filter.LocationGrpArea == l.OPS_AREA.ops_area1) ||
                       string.IsNullOrEmpty(filter.LocationGrpArea))
                      && (filter.Branch == l.served_by_locn || string.IsNullOrEmpty(filter.Branch))
                select l).ToList();

            var carGroups = (from cg in db.CAR_GROUPs
                where cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description == filter.Country
                select cg).ToList();

            var returned = new List<DayActualEntity>();
            foreach (var l in locations)
            {
                var loc = l;
                returned.AddRange(carGroups.Select(cg => new DayActualEntity
                                                         {
                                                             Tme = 0,
                                                             Label = loc.served_by_locn + " " + cg.car_group1
                                                         }));
            }

            return returned;
        }

        public static List<DayActualEntity> GetPoolingCheckInDataForAlerts(PoolingDataClassesDataContext db, IMainFilterEntity filter)
        {
            var q2And3 = from p in db.Reservations
                         where (p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                         && (p.CAR_GROUP.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                         && (p.CAR_GROUP.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                         select p;
            if (filter.ExcludeLongterm)
            {
                //q2And3 = q2And3.Where(d => !(d.RentalLocation.served_by_locn.Substring(5, 1) != "5" &&
                //                               SqlMethods.DateDiffDay(d.RS_ARRIVAL_DATE, d.RTRN_DATE) > 27));
                q2And3 = q2And3.Except(q2And3.Where(d => d.RentalLocation.served_by_locn.Substring(5, 1) != "5" &&
                                               SqlMethods.DateDiffDay(d.RS_ARRIVAL_DATE, d.RTRN_DATE) > 27));

            }
            q2And3 = SiteParametersFilter.FilterByReturnLocation(q2And3, filter);
            var returned = _resCIFleetQ.GetCheckInReservationsOffsetForAlerts(q2And3, db, filter);
            
            return returned;
        }

        public static List<DayActualEntity> GetPoolingCheckOutDataForAlerts(PoolingDataClassesDataContext db, IMainFilterEntity mfe)
        {
            var q2And3 = CarParametersFilter.FilterByCarParameters(db, mfe, false);
            q2And3 = SiteParametersFilter.FilterByRentalLocation(q2And3, mfe);
            var returned = _resCOFleetQ.GetCheckOutReservationsForAlerts(q2And3, db, mfe);

            return returned;
        }

        public static List<DayActualEntity> GetPoolingCheckOutData(IMainFilterEntity mfe, bool hourlyTimeSlots
                                                                    , PoolingDataClassesDataContext db)
        {
            var q2And3 = CarParametersFilter.FilterByCarParameters(db, mfe, false);
            q2And3 = SiteParametersFilter.FilterByRentalLocation(q2And3, mfe);
            var l = _reqCo.GetReservationsWithoutLabels(q2And3, mfe,
                    hourlyTimeSlots ? Enums.DayActualTime.THREE : Enums.DayActualTime.THIRTY).ToList();

            var returned = l;
            return returned;
        }

        public static List<DayActualEntity> GetPoolingCheckOutDataWithLabels(IMainFilterEntity mfe, bool hourlyTimeSlots
                                                                    , PoolingDataClassesDataContext db, bool siteQ)
        {
            var q2And3 = CarParametersFilter.FilterByCarParameters(db, mfe, false);
            q2And3 = SiteParametersFilter.FilterByRentalLocation(q2And3, mfe);
            var l = siteQ
                ? _resCOSiteQ.GetQueryableCO(q2And3, mfe,
                    hourlyTimeSlots ? Enums.DayActualTime.THREE : Enums.DayActualTime.THIRTY).ToList()
                : _resCOFleetQ.GetQueryable(q2And3, mfe,
                    hourlyTimeSlots ? Enums.DayActualTime.THREE : Enums.DayActualTime.THIRTY).ToList();

            var returned = l;
            return returned;
        }

        public static List<DayActualEntity> GetCurrentAdditionDeletion(IMainFilterEntity filter
                                                                , bool hourly, MarsDBDataContext db, bool alertsRequest = false)
        {
            var startingTimePeriod = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            var endingTimePeriod = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            startingTimePeriod = startingTimePeriod.Date.AddHours(startingTimePeriod.Hour);
            if (hourly)
            {   
                endingTimePeriod = endingTimePeriod.Date.AddHours(endingTimePeriod.Hour);
                endingTimePeriod = endingTimePeriod.AddHours(1).AddSeconds(-1);
            }
            else
            {
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

            List<DayActualEntity> returned = alertsRequest 
                ? GetAdjustmentsWithLabel(startingTimePeriod, endingTimePeriod, additions, deletions)
                : GetAdjustmentsWithoutLabel(hourly, startingTimePeriod, endingTimePeriod, additions, deletions);
            

            return returned;
        }

        private static List<DayActualEntity> GetAdjustmentsWithoutLabel(bool hourly
                            , DateTime startingTimePeriod, DateTime endingTimePeriod
                            , List<MarsDBContext.ResAddition> additions, List<MarsDBContext.ResDeletion> deletions)
        {
            var returned = new List<DayActualEntity>();
            for (var i = 0; i <= (Int32)Enums.ThreeDayActuals.MAXNOOFCOLUMNS; i++)
            {
                int previousAdditions, previousDeletions;
                int timeAddition = i;
                string label;

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

                returned.Add(new DayActualEntity { Tme = i, AddditionDeletion = previousAdditions - previousDeletions });
            }
            return returned;
        }

        private static List<DayActualEntity> GetAdjustmentsWithLabel(DateTime startingTimePeriod, DateTime endingTimePeriod
                            , List<MarsDBContext.ResAddition> additions, List<MarsDBContext.ResDeletion> deletions)
        {
            var returned = new List<DayActualEntity>();
            for (var i = 0; i <= (Int32)Enums.ThreeDayActuals.MAXNOOFCOLUMNS; i++)
            {
                var timeAddition = i;

                var newEndTime = endingTimePeriod.AddHours(timeAddition);
                var newStartTime = startingTimePeriod.AddHours(timeAddition);

                var additionsToAdd = additions.Where(p => p.RepDate >= newStartTime
                                                && p.RepDate <= newEndTime)
                        .GroupBy(g=> g.LOCATION.location1 + " " + g.CAR_GROUP.car_group1)
                        .Select(p => new DayActualEntity
                            {
                                Tme = timeAddition,
                                Label = p.Key,
                                AddditionDeletion = p.Sum(d=> d.Value),
                                JustAdditions = p.Sum(d => d.Value),
                                JustDeletions = 0
                            });

                var deletionsToAddToAdd = deletions.Where(p => p.RepDate >= newStartTime
                                                && p.RepDate <= newEndTime)
                        .GroupBy(g => g.LOCATION.location1 + " " + g.CAR_GROUP.car_group1)
                        .Select(p => new DayActualEntity
                        {
                            Tme = timeAddition,
                            Label = p.Key,
                            AddditionDeletion = p.Sum(d => d.Value)  * -1,
                            JustAdditions = 0,
                            JustDeletions = p.Sum(d => d.Value) * -1
                        });


                var combinedList = (from cd in additionsToAdd.Union(deletionsToAddToAdd)
                    group cd by new {cd.Tme, cd.Label}
                    into g
                    select new DayActualEntity
                           {
                               Tme = g.Key.Tme,
                               Label = g.Key.Label,
                               AddditionDeletion = g.Sum(d => d.AddditionDeletion),
                               JustAdditions = g.Sum(d=> d.JustAdditions),
                               JustDeletions = g.Sum(d => d.JustDeletions)
                           }).ToList();

                returned.AddRange(combinedList);

            }
            return returned;
        }

        public static List<DayActualEntity> GetAdditionDeletionDataWithLabels(IMainFilterEntity filter, bool hourly
                                                    , MarsDBDataContext db, bool siteQ)
        {
            var additions = from ad in db.ResAdditions
                            where (ad.LOCATION.COUNTRy1.active)
                               && (ad.LOCATION.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                               && (ad.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == filter.PoolRegion ||
                                        ad.LOCATION.OPS_AREA.OPS_REGION.ops_region1 == filter.PoolRegion || String.IsNullOrEmpty(filter.PoolRegion))
                               && (ad.LOCATION.CMS_LOCATION_GROUP.cms_location_group1 == filter.LocationGrpArea ||
                                        ad.LOCATION.OPS_AREA.ops_area1 == filter.LocationGrpArea || String.IsNullOrEmpty(filter.LocationGrpArea))
                               && (ad.LOCATION.location1 == filter.Branch || String.IsNullOrEmpty(filter.Branch))
                               && (ad.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                               && (ad.CAR_GROUP.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                               && (ad.CAR_GROUP.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                               && ad.RepDate > DateTime.Now
                            group ad by new
                            {
                                k1 = siteQ ? (String.IsNullOrEmpty(filter.Country) ? ad.LOCATION.COUNTRy1.country_description :
                                        String.IsNullOrEmpty(filter.PoolRegion) ? filter.CmsLogic
                                            ? ad.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                                : ad.LOCATION.OPS_AREA.OPS_REGION.ops_region1 :
                                        String.IsNullOrEmpty(filter.LocationGrpArea) ? filter.CmsLogic
                                            ? ad.LOCATION.CMS_LOCATION_GROUP.cms_location_group1
                                            : ad.LOCATION.OPS_AREA.ops_area1 :
                                        ad.LOCATION.location1)
                                        : (String.IsNullOrEmpty(filter.Country) ? ad.LOCATION.COUNTRy1.country_description :
                                      String.IsNullOrEmpty(filter.CarSegment) ? ad.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                                      String.IsNullOrEmpty(filter.CarClass) ? ad.CAR_GROUP.CAR_CLASS.car_class1 :
                                        ad.CAR_GROUP.car_group1),
                                k2 = (hourly ? SqlMethods.DateDiffHour(DateTime.Now, ad.RepDate)
                                            : SqlMethods.DateDiffDay(DateTime.Now, ad.RepDate))
                            }
                                into g
                                select new DayActualEntity
                                {
                                    Tme = g.Key.k2,
                                    Label = g.Key.k1,
                                    AddditionDeletion = g.Sum(d => d.Value),
                                    JustAdditions = g.Sum(d => d.Value),
                                    JustDeletions = 0
                                };

            var deletions = from del in db.ResDeletions
                            where (del.LOCATION.COUNTRy1.active)
                               && (del.LOCATION.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                               && (del.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == filter.PoolRegion ||
                                        del.LOCATION.OPS_AREA.OPS_REGION.ops_region1 == filter.PoolRegion || String.IsNullOrEmpty(filter.PoolRegion))
                               && (del.LOCATION.CMS_LOCATION_GROUP.cms_location_group1 == filter.LocationGrpArea ||
                                        del.LOCATION.OPS_AREA.ops_area1 == filter.LocationGrpArea || String.IsNullOrEmpty(filter.LocationGrpArea))
                               && (del.LOCATION.location1 == filter.Branch || String.IsNullOrEmpty(filter.Branch))
                               && (del.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                               && (del.CAR_GROUP.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                               && (del.CAR_GROUP.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                               && del.RepDate > DateTime.Now
                            group del by new
                            {
                                k1 = siteQ ? (String.IsNullOrEmpty(filter.Country) ? del.LOCATION.COUNTRy1.country_description :
                                        String.IsNullOrEmpty(filter.PoolRegion) ? filter.CmsLogic
                                            ? del.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                                : del.LOCATION.OPS_AREA.OPS_REGION.ops_region1 :
                                        String.IsNullOrEmpty(filter.LocationGrpArea) ? filter.CmsLogic
                                            ? del.LOCATION.CMS_LOCATION_GROUP.cms_location_group1
                                            : del.LOCATION.OPS_AREA.ops_area1 :
                                        del.LOCATION.location1)
                                        : (String.IsNullOrEmpty(filter.Country) ? del.LOCATION.COUNTRy1.country_description :
                                      String.IsNullOrEmpty(filter.CarSegment) ? del.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                                      String.IsNullOrEmpty(filter.CarClass) ? del.CAR_GROUP.CAR_CLASS.car_class1 :
                                        del.CAR_GROUP.car_group1),
                                k2 = (hourly ? SqlMethods.DateDiffHour(DateTime.Now, del.RepDate)
                                            : SqlMethods.DateDiffDay(DateTime.Now, del.RepDate))
                            }
                                into g
                                select new DayActualEntity
                                {
                                    Tme = g.Key.k2,
                                    Label = g.Key.k1,
                                    AddditionDeletion = g.Sum(d => d.Value * -1),
                                    JustAdditions = 0,
                                    JustDeletions = g.Sum(d => d.Value * -1)
                                };
            var additionsAndDeletions = additions.Union(deletions).ToList();

            var returned = (from ad in additionsAndDeletions
                            group ad by new
                                        {
                                            k1 = ad.Tme,
                                            k2 = ad.Label
                                        }
                                into g
                                select new DayActualEntity
                                       {
                                           Tme = g.Key.k1,
                                           Label = g.Key.k2,
                                           AddditionDeletion = g.Sum(d => d.AddditionDeletion),
                                           JustAdditions = g.Sum(d => d.JustAdditions),
                                           JustDeletions = g.Sum(d => d.JustDeletions)
                                       }).ToList();

            return returned;
        }

        public static int GetBuffers(IMainFilterEntity filter, MarsDBDataContext db)
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

        public static List<DayActualEntity> GetBuffersWithLabels(IMainFilterEntity filter, MarsDBDataContext db, bool siteQ)
        {
            var buffers = from ad in db.ResBuffers
                            where (ad.LOCATION.COUNTRy1.active)
                               && (ad.LOCATION.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                               && (ad.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == filter.PoolRegion ||
                                        ad.LOCATION.OPS_AREA.OPS_REGION.ops_region1 == filter.PoolRegion || String.IsNullOrEmpty(filter.PoolRegion))
                               && (ad.LOCATION.CMS_LOCATION_GROUP.cms_location_group1 == filter.LocationGrpArea ||
                                        ad.LOCATION.OPS_AREA.ops_area1 == filter.LocationGrpArea || String.IsNullOrEmpty(filter.LocationGrpArea))
                               && (ad.LOCATION.location1 == filter.Branch || String.IsNullOrEmpty(filter.Branch))
                               && (ad.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                               && (ad.CAR_GROUP.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                               && (ad.CAR_GROUP.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                            group ad by new
                            {
                                k1 = siteQ ? (String.IsNullOrEmpty(filter.Country) ? ad.LOCATION.COUNTRy1.country_description :
                                        String.IsNullOrEmpty(filter.PoolRegion) ? filter.CmsLogic
                                            ? ad.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                                : ad.LOCATION.OPS_AREA.OPS_REGION.ops_region1 :
                                        String.IsNullOrEmpty(filter.LocationGrpArea) ? filter.CmsLogic
                                            ? ad.LOCATION.CMS_LOCATION_GROUP.cms_location_group1
                                            : ad.LOCATION.OPS_AREA.ops_area1 :
                                        ad.LOCATION.location1)
                                        : (String.IsNullOrEmpty(filter.Country) ? ad.LOCATION.COUNTRy1.country_description :
                                      String.IsNullOrEmpty(filter.CarSegment) ? ad.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                                      String.IsNullOrEmpty(filter.CarClass) ? ad.CAR_GROUP.CAR_CLASS.car_class1 :
                                        ad.CAR_GROUP.car_group1)
                            }
                                into g
                                select new DayActualEntity
                                {
                                    Label = g.Key.k1,
                                    Buffer = g.Sum(d => d.Value)
                                };



            if (buffers.Any())
            {
                return buffers.ToList();
            }
            return new List<DayActualEntity> { new DayActualEntity() };

        }

        public static List<DayActualEntity> GetBuffersForAlerts(IMainFilterEntity filter, MarsDBDataContext db)
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
                             select new DayActualEntity
                                    {
                                        Buffer = p.Value,
                                        Label = p.LOCATION.location1 + " " + p.CAR_GROUP.car_group1,
                                        Tme = 0
                                    };

            if (bufferData.Any())
            {
                return bufferData.ToList();
            }
            return new List<DayActualEntity> {new DayActualEntity()};

        }
    }
}