using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Castle.Core.Internal;

using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.Entities.Pooling;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;
using App.Classes.DAL.Pooling.Abstract;

namespace Mars.DAL.Pooling.Queryables
{
    public class FeaPoolingDataAccess
    {
        public IQueryable<DayActualEntity> GetFeaDataWithoutLabels(IQueryable<FLEET_EUROPE_ACTUAL> availableData
                                                , IQueryable<FLEET_EUROPE_ACTUAL> checkInData
                                                , Enums.DayActualTime time
                                                , IMainFilterEntity mfe
                                                , MarsDBDataContext db)
        {
            var availableEntity = from p in availableData
                                    where p.RT == 1
                                    group p by 0
                                    into g
                                    select new DayActualEntity
                                    {
                                        Tme = 0,
                                        Available = g.Sum(d=> d.TOTAL_FLEET.HasValue ? d.TOTAL_FLEET.Value : 0),
                                        Checkin = 0,
                                        OnewayCheckin = 0,
                                        LocalCheckIn = 0,
                                        Opentrips = 0,
                                        Offset = 0
                                    };

            

            var now = DateTime.Now.GetDateAndHourOnlyByCountry(mfe.Country);


            var openTripsEntity = from p in availableData
                                  where (time == Enums.DayActualTime.THREE ?
                                    (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) <= 71)
                                    : (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffDay(now, p.DUEDATE.Value) <= 29))
                                  && p.RT == 1
                                  group p by (time == Enums.DayActualTime.THREE
                                    ? SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour))
                                    : SqlMethods.DateDiffDay(now, p.DUEDATE.Value)
                                        )
                                      into g
                                      select new DayActualEntity
                                      {
                                          Tme = g.Key,
                                          Available = 0,
                                          Checkin = 0,
                                          OnewayCheckin = 0,
                                          LocalCheckIn = 0,
                                          Opentrips = g.Sum(d => (d.MOVETYPE.ToUpper() == "T-O"
                                                              || d.MOVETYPE.ToUpper() == "L-O"
                                                                  )
                                                                  && d.TOTAL_FLEET.HasValue ? d.TOTAL_FLEET.Value : 0),
                                          Offset = 0
                                      };

            var checkInEntities = from p in checkInData
                                  join loc in db.LOCATIONs on p.DUEWWD equals loc.location1
                                  join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals
                                      new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  where (time == Enums.DayActualTime.THREE ?
                                    (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) <= 71)
                                    : (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffDay(now, p.DUEDATE.Value) <= 29))
                                  group p by (time == Enums.DayActualTime.THREE
                                    ? SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour))
                                    : SqlMethods.DateDiffDay(now, p.DUEDATE.Value)
                                        )
                                      into g
                                      select new DayActualEntity
                                      {
                                          Tme = g.Key,
                                          Available = 0,
                                          Checkin = g.Sum(p => p.TOTAL_FLEET) ?? 0,
                                          OnewayCheckin = g.Sum(p => p.LSTWWD != p.DUEWWD ? p.TOTAL_FLEET : 0) ?? 0,
                                          LocalCheckIn = g.Sum(p => p.LSTWWD == p.DUEWWD ? p.TOTAL_FLEET : 0) ?? 0,
                                          Opentrips = 0,
                                          Offset = 0
                                      };

                     

            //var yy = checkInEntities.ToList();

            
            var offsetEntities = from p in checkInData
                                 join loc in db.LOCATIONs on p.DUEWWD equals loc.served_by_locn
                                 where (time == Enums.DayActualTime.THREE ?
                                   (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) <= 71)
                                    : (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffDay(now, p.DUEDATE.Value) <= 29))
                                        && loc.location1 == loc.served_by_locn
                                 group p by (time == Enums.DayActualTime.THREE
                                    ? SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)
                                            .AddHours(loc.turnaround_hours.HasValue ? loc.turnaround_hours.Value : 0))
                                    : SqlMethods.DateDiffDay(now, p.DUEDATE.Value.AddHours(loc.turnaround_hours.HasValue ? loc.turnaround_hours.Value : 0))
                                        )
                                     into g
                                     select new DayActualEntity
                                     {
                                         Tme = g.Key,
                                         Available = 0,
                                         Checkin = 0,
                                         OnewayCheckin = 0,
                                         LocalCheckIn = 0,
                                         Opentrips = 0,
                                         Offset = g.Sum(p => p.TOTAL_FLEET) ?? 0
                                     };
           // var xx = offsetEntities.ToList();


            var combinedData = from cd in availableEntity.Union(openTripsEntity).Union(checkInEntities).Union(offsetEntities)
                               group cd by cd.Tme
                                   into gd
                                   select new DayActualEntity
                                          {
                                              Tme = gd.Key,
                                              Available = gd.Sum(d => d.Available),
                                              Opentrips = gd.Sum(d => d.Opentrips),
                                              Checkin = gd.Sum(d => d.Checkin),
                                              OnewayCheckin = gd.Sum(d => d.OnewayCheckin),
                                              LocalCheckIn = gd.Sum(d => d.LocalCheckIn),
                                              Offset = gd.Sum(d => d.Offset)
                                          };

            return combinedData;
        }

        public IQueryable<DayActualEntity> GetFullFeaDataForAlerts(IQueryable<FLEET_EUROPE_ACTUAL> availableData
                                                , IQueryable<FLEET_EUROPE_ACTUAL> checkInData
                                                , IMainFilterEntity mfe
                                                , MarsDBDataContext db)
        {

            var now = DateTime.Now.GetDateAndHourOnlyByCountry(mfe.Country);

            var availableEntities = from p in availableData
                                    //join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } 
                                    //    equals new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                    where 
                                        p.RT == 1
                                    group p by  new
                                                {
                                                    Hours = ! p.DUEDATE.HasValue || p.DUEDATE <= now ? 0
                                                     : SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour))
                                                    , p.VC
                                                    , p.LSTWWD
                                                }
                                        into g
                                        select new DayActualEntity
                                        {
                                            Tme = g.Key.Hours,
                                            Label = g.Key.LSTWWD + " " + g.Key.VC,
                                            Available = g.Sum(d => d.TOTAL_FLEET.HasValue ? d.TOTAL_FLEET.Value : 0),
                                            Checkin = 0,
                                            OnewayCheckin = 0,
                                            LocalCheckIn = 0,
                                            Opentrips = 0,
                                            Offset = 0
                                        };

            //var ss = availableEntities.Where(d => d.Available > 0).ToList();


            var offsetEntities = from p in checkInData
                                 join loc in db.LOCATIONs on p.DUEWWD equals loc.served_by_locn
                                 join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals
                                     new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                 where SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)
                                                .AddHours(loc.turnaround_hours.HasValue ? loc.turnaround_hours.Value : 0)) >= 0
                                 group p by new { HoursOffset = SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)
                                                .AddHours(loc.turnaround_hours.HasValue ? loc.turnaround_hours.Value : 0))
                                                , p.VC, p.DUEWWD }
                                     into g
                                     select new DayActualEntity
                                     {
                                         Tme = g.Key.HoursOffset,
                                         Label = g.Key.DUEWWD + " " + g.Key.VC,
                                         Available = 0,
                                         Checkin = 0,
                                         OnewayCheckin = 0,
                                         LocalCheckIn = 0,
                                         Opentrips = 0,
                                         Offset = g.Sum(p => p.TOTAL_FLEET) ?? 0
                                     };


            var combinedData = from cd in availableEntities.Union(offsetEntities)
                               group cd by new {cd.Tme, cd.Label}
                                   into gd
                                   select new DayActualEntity
                                   {
                                       Tme = gd.Key.Tme,
                                       Label = gd.Key.Label,
                                       Available = gd.Sum(d => d.Available),
                                       Opentrips = gd.Sum(d => d.Opentrips),
                                       Checkin = gd.Sum(d => d.Checkin),
                                       OnewayCheckin = gd.Sum(d => d.OnewayCheckin),
                                       LocalCheckIn = gd.Sum(d => d.LocalCheckIn),
                                       Offset = gd.Sum(d => d.Offset)
                                   };
            return combinedData;
        }

        public IQueryable<DayActualEntity> GetFeaDataWithLabels(IQueryable<FLEET_EUROPE_ACTUAL> availableData
                                                , IQueryable<FLEET_EUROPE_ACTUAL> checkInData
                                                , Enums.DayActualTime time, bool siteQ
                                                , IMainFilterEntity filter, MarsDBDataContext db)
        {
            IQueryable<DayActualEntity> availableEntities;
            IQueryable<DayActualEntity> checkInEntities;
            IQueryable<DayActualEntity> offsetEntities;
            IQueryable<DayActualEntity> openTripsEntities;

            var now = DateTime.Now.GetDateAndHourOnlyByCountry(filter.Country);

            if (siteQ)
            {
                availableEntities = from p in availableData
                                    join loc in db.LOCATIONs on p.LSTWWD equals loc.location1
                                    join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals
                                        new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                    where p.RT == 1
                                    group p by (String.IsNullOrEmpty(filter.Country)
                                                       ? loc.COUNTRy1.country_description
                                                       : String.IsNullOrEmpty(filter.PoolRegion)
                                                           ? filter.CmsLogic
                                                               ? loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                                               : loc.OPS_AREA.OPS_REGION.ops_region1
                                                           : String.IsNullOrEmpty(filter.LocationGrpArea)
                                                               ? filter.CmsLogic
                                                                   ? loc.CMS_LOCATION_GROUP.cms_location_group1
                                                                   : loc.OPS_AREA.ops_area1
                                                               : loc.served_by_locn)
                                        into g
                                        select new DayActualEntity
                                               {
                                                   Tme = 0,
                                                   Label = g.Key,
                                                   Available = g.Sum(d => d.TOTAL_FLEET.HasValue ? d.TOTAL_FLEET.Value : 0),
                                                   Checkin = 0,
                                                   OnewayCheckin = 0,
                                                   LocalCheckIn = 0,
                                                   Opentrips = 0,
                                                   Offset = 0
                                               };

                

                openTripsEntities = from p in availableData
                                    join loc in db.LOCATIONs on p.LSTWWD equals loc.location1
                                    join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals
                                        new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                    where
                                        (time == Enums.DayActualTime.THREE ?
                                   (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) <= 71)
                                    : (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffDay(now, p.DUEDATE.Value) <= 29))
                                        && p.RT == 1
                                    group p by new
                                    {
                                        t1 = (String.IsNullOrEmpty(filter.Country)
                                            ? loc.COUNTRy1.country_description
                                            : String.IsNullOrEmpty(filter.PoolRegion)
                                                ? filter.CmsLogic
                                                    ? loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                                    : loc.OPS_AREA.OPS_REGION.ops_region1
                                                : String.IsNullOrEmpty(filter.LocationGrpArea)
                                                    ? filter.CmsLogic
                                                        ? loc.CMS_LOCATION_GROUP.cms_location_group1
                                                        : loc.OPS_AREA.ops_area1
                                                    : loc.served_by_locn),
                                        t2 = (time == Enums.DayActualTime.THREE
                                                ? SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour))
                                                : SqlMethods.DateDiffDay(now, p.DUEDATE.Value))
                                    }
                                        into g
                                        select new DayActualEntity
                                        {
                                            Tme = g.Key.t2,
                                            Label = g.Key.t1,
                                            Available = 0,
                                            Checkin = 0,
                                            OnewayCheckin = 0,
                                            LocalCheckIn = 0,
                                            Opentrips = g.Sum(p => (p.MOVETYPE.ToUpper() == "T-O"
                                                                 || p.MOVETYPE.ToUpper() == "L-O")  
                                                                 && p.TOTAL_FLEET.HasValue ? p.TOTAL_FLEET.Value : 0 ),
                                            Offset = 0
                                        };


                checkInEntities = from p in checkInData
                                  join loc in db.LOCATIONs on p.DUEWWD equals loc.location1
                                  join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals
                                      new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  where
                                     (time == Enums.DayActualTime.THREE ?
                                    (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) <= 71)
                                    : (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffDay(now, p.DUEDATE.Value) <= 29))
                                  group p by new
                                             {
                                                 t1 = (String.IsNullOrEmpty(filter.Country)
                                                     ? loc.COUNTRy1.country_description
                                                     : String.IsNullOrEmpty(filter.PoolRegion)
                                                         ? filter.CmsLogic
                                                             ? loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                                             : loc.OPS_AREA.OPS_REGION.ops_region1
                                                         : String.IsNullOrEmpty(filter.LocationGrpArea)
                                                             ? filter.CmsLogic
                                                                 ? loc.CMS_LOCATION_GROUP.cms_location_group1
                                                                 : loc.OPS_AREA.ops_area1
                                                             : loc.served_by_locn),
                                                 t2 = (time == Enums.DayActualTime.THREE
                                                        ? SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour))
                                                        : SqlMethods.DateDiffDay(now, p.DUEDATE.Value))
                                             }
                                      into g
                                      select new DayActualEntity
                                             {
                                                 Tme = g.Key.t2,
                                                 Label = g.Key.t1,
                                                 Available = 0,
                                                 Checkin = g.Sum(p => p.ON_RENT) ?? 0,
                                                 OnewayCheckin = g.Sum(p => p.LSTWWD != p.DUEWWD ? p.ON_RENT : 0) ?? 0,
                                                 LocalCheckIn = g.Sum(p => p.LSTWWD == p.DUEWWD ? p.ON_RENT : 0) ?? 0,
                                                 Opentrips = 0,
                                                 Offset = 0
                                             };


                offsetEntities = from p in checkInData
                                 join loc in db.LOCATIONs on p.DUEWWD equals loc.served_by_locn
                                 join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals
                                     new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                 where
                                     (time == Enums.DayActualTime.THREE ?
                                       (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                            && SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) <= 71)
                                        : (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                            && SqlMethods.DateDiffDay(now, p.DUEDATE.Value) <= 29))
                                            && loc.location1 == loc.served_by_locn
                                 group p by new
                                            {
                                                t1 = (String.IsNullOrEmpty(filter.Country)
                                                    ? loc.COUNTRy1.country_description
                                                    : String.IsNullOrEmpty(filter.PoolRegion)
                                                        ? filter.CmsLogic
                                                            ? loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                                            : loc.OPS_AREA.OPS_REGION.ops_region1
                                                        : String.IsNullOrEmpty(filter.LocationGrpArea)
                                                            ? filter.CmsLogic
                                                                ? loc.CMS_LOCATION_GROUP.cms_location_group1
                                                                : loc.OPS_AREA.ops_area1
                                                            : loc.served_by_locn),
                                                t2 = (time == Enums.DayActualTime.THREE
                                                    ? SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)
                                                            .AddHours(loc.turnaround_hours.HasValue ? loc.turnaround_hours.Value : 0))
                                                    : SqlMethods.DateDiffDay(now, p.DUEDATE.Value.AddHours(loc.turnaround_hours.HasValue ? loc.turnaround_hours.Value : 0))
                                                    )
                                            }
                                     into g
                                     select new DayActualEntity
                                            {
                                                Tme = g.Key.t2,
                                                Label = g.Key.t1,
                                                Available = 0,
                                                Checkin = 0,
                                                OnewayCheckin = 0,
                                                LocalCheckIn = 0,
                                                Opentrips = 0,
                                                Offset = g.Sum(p => p.TOTAL_FLEET) ?? 0
                                            };
            }
            else
            { // Fleet Comparison Query
                availableEntities = from p in availableData
                                    join loc in db.LOCATIONs on p.LSTWWD equals loc.location1
                                    join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                    where p.RT == 1
                                    group p by (String.IsNullOrEmpty(filter.Country) ? loc.COUNTRy1.country_description :
                                               String.IsNullOrEmpty(filter.CarSegment) ? cg.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                                               String.IsNullOrEmpty(filter.CarClass) ? cg.CAR_CLASS.car_class1 :
                                               cg.car_group1)
                                    into g
                                    select new DayActualEntity
                                        {
                                            Tme = 0,
                                            Label = g.Key,
                                            Available = g.Sum(d => d.TOTAL_FLEET.HasValue ? d.TOTAL_FLEET.Value : 0),
                                            Checkin = 0,
                                            OnewayCheckin = 0,
                                            LocalCheckIn = 0,
                                            Opentrips = 0,
                                            Offset = 0
                                        };

                openTripsEntities = from p in availableData
                                      join loc in db.LOCATIONs on p.LSTWWD equals loc.location1
                                      join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals
                                          new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                      where
                                          (time == Enums.DayActualTime.THREE ?
                                           (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                                && SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) <= 71)
                                            : (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                                && SqlMethods.DateDiffDay(now, p.DUEDATE.Value) <= 29))
                                        && p.RT == 1
                                      group p by new
                                      {
                                          t1 = (String.IsNullOrEmpty(filter.Country) ? loc.COUNTRy1.country_description :
                                               String.IsNullOrEmpty(filter.CarSegment) ? cg.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                                               String.IsNullOrEmpty(filter.CarClass) ? cg.CAR_CLASS.car_class1 :
                                               cg.car_group1),
                                          t2 = (time == Enums.DayActualTime.THREE
                                                ? SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour))
                                                : SqlMethods.DateDiffDay(now, p.DUEDATE.Value))
                                      }
                                          into g
                                          select new DayActualEntity
                                          {
                                              Tme = g.Key.t2,
                                              Label = g.Key.t1,
                                              Available = 0,
                                              Checkin = 0,
                                              OnewayCheckin = 0,
                                              LocalCheckIn = 0,
                                              Opentrips = g.Sum(p => (p.MOVETYPE.ToUpper() == "T-O"
                                                                   || p.MOVETYPE.ToUpper() == "L-O") 
                                                                   &&  p.TOTAL_FLEET.HasValue ? p.TOTAL_FLEET.Value : 0 ),
                                              Offset = 0

                                          };

                checkInEntities = from p in checkInData
                                  join loc in db.LOCATIONs on p.DUEWWD equals loc.location1
                                  join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } equals new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  where
                                      (time == Enums.DayActualTime.THREE ?
                                    (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) <= 71)
                                    : (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                        && SqlMethods.DateDiffDay(now, p.DUEDATE.Value) <= 29))
                                  group p by new
                                  {
                                      t1 = (String.IsNullOrEmpty(filter.Country) ? loc.COUNTRy1.country_description :
                                             String.IsNullOrEmpty(filter.CarSegment) ? cg.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                                             String.IsNullOrEmpty(filter.CarClass) ? cg.CAR_CLASS.car_class1 :
                                             cg.car_group1),
                                      t2 = (time == Enums.DayActualTime.THREE
                                                        ? SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour))
                                                        : SqlMethods.DateDiffDay(now, p.DUEDATE.Value))
                                  }
                                      into g
                                      select new DayActualEntity
                                      {
                                          Tme = g.Key.t2,
                                          Label = g.Key.t1,
                                          Available = 0,
                                          Checkin = g.Sum(p => p.ON_RENT) ?? 0,
                                          OnewayCheckin = g.Sum(p => p.LSTWWD != p.DUEWWD ? p.ON_RENT : 0) ?? 0,
                                          LocalCheckIn = g.Sum(p => p.LSTWWD == p.DUEWWD ? p.ON_RENT : 0) ?? 0,
                                          Opentrips = 0,
                                          Offset = 0
                                      };
                offsetEntities = from p in checkInData
                                 //join c in db.COUNTRies on p.COUNTRY equals c.country1
                                 join loc in db.LOCATIONs on p.DUEWWD equals loc.served_by_locn
                                 join cg in db.CAR_GROUPs on new { vc = p.VC, c = p.COUNTRY } 
                                    equals new { vc = cg.car_group1, c = cg.CAR_CLASS.CAR_SEGMENT.country }

                                 where
                                     (time == Enums.DayActualTime.THREE ?
                                       (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                            && SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) <= 71)
                                        : (SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)) >= 0
                                            && SqlMethods.DateDiffDay(now, p.DUEDATE.Value) <= 29))
                                            && loc.location1 == loc.served_by_locn
                                 group p by new
                                 {
                                     t1 = (String.IsNullOrEmpty(filter.Country) ? loc.COUNTRy1.country_description :
                                            String.IsNullOrEmpty(filter.CarSegment) ? cg.CAR_CLASS.CAR_SEGMENT.car_segment1 :
                                            String.IsNullOrEmpty(filter.CarClass) ? cg.CAR_CLASS.car_class1 :
                                            cg.car_group1),
                                     t2 = (time == Enums.DayActualTime.THREE
                                                    ? SqlMethods.DateDiffHour(now, p.DUEDATE.Value.AddHours(p.DUETIME.Value.Hour)
                                                            .AddHours(loc.turnaround_hours.HasValue ? loc.turnaround_hours.Value : 0))
                                                    : SqlMethods.DateDiffDay(now, p.DUEDATE.Value.AddHours(loc.turnaround_hours.HasValue ? loc.turnaround_hours.Value : 0))
                                                    )
                                 }
                                     into g
                                     select new DayActualEntity
                                     {
                                         Tme = g.Key.t2,
                                         Label = g.Key.t1,
                                         Available = 0,
                                         Checkin = 0,
                                         OnewayCheckin = 0,
                                         LocalCheckIn = 0,
                                         Opentrips = 0,
                                         Offset = g.Sum(p => p.TOTAL_FLEET) ?? 0
                                     };
            }





            var combinedData = from cd in availableEntities.Union(checkInEntities).Union(openTripsEntities).Union(offsetEntities)
                               group cd by new { cd.Tme, cd.Label }
                                   into gd
                                   select new DayActualEntity
                                   {
                                       Tme = gd.Key.Tme,
                                       Label = gd.Key.Label,
                                       Available = gd.Sum(d => d.Available),
                                       Opentrips = gd.Sum(d => d.Opentrips),
                                       Checkin = gd.Sum(d => d.Checkin),
                                       OnewayCheckin = gd.Sum(d => d.OnewayCheckin),
                                       LocalCheckIn = gd.Sum(d => d.LocalCheckIn),
                                       Offset = gd.Sum(d => d.Offset)
                                   };


            return combinedData;
        }



    }
}