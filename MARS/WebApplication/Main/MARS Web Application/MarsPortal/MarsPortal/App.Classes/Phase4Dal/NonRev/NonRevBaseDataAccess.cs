using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using App.BLL.ExtensionMethods;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;
using Mars.App.Classes.Phase4Dal.NonRev.Parameters;
using Mars.Pooling.Models;

namespace Mars.App.Classes.Phase4Dal.NonRev
{
    public class NonRevBaseDataAccess : BaseDataAccess
    {
        
        public NonRevBaseDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null) : base(parameters, dbc)
        {
            
        }

        protected static ComparisonRow BuildTotalRow(List<ComparisonRow> comparisonData)
        {
            var totalRow = new ComparisonRow
            {
                Key = TotalKeyName,
                FleetCount = comparisonData.Where(d=> d != null).Sum(d => d.FleetCount),
                DaysNonRev = comparisonData.Where(d => d != null).Sum(d => d.DaysNonRev),
                NonRevCount = comparisonData.Where(d => d != null).Sum(d => d.NonRevCount),
                ReasonsEntered = comparisonData.Where(d => d != null).Sum(d => d.ReasonsEntered)
            };
            return totalRow;
        }

        protected ComparisonRow BuildUnmappedVehicleRow(bool siteComparison)
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);
            if (vehicles == null) return new ComparisonRow();

            IQueryable<int> matchedSet;
            if (siteComparison)
            {
                var locationCountry = Parameters[DictionaryParameter.LocationCountry];
                if (!string.IsNullOrEmpty(locationCountry))
                {
                    return null;
                }

                matchedSet = from v in vehicles
                             join loc in DataContext.LOCATIONs.Select(d => d.location1).Distinct()
                                    on v.LastLocationCode equals loc
                             select v.VehicleId;
            }
            else
            {
                var owningCountry = Parameters[DictionaryParameter.OwningCountry];
                if (!string.IsNullOrEmpty(owningCountry))
                {
                    return null;
                }

                var carGroups = (from cg in DataContext.CAR_GROUPs
                                 select new { cg = cg.car_group1, cg.CAR_CLASS.CAR_SEGMENT.country }).Distinct();

                matchedSet = from v in vehicles
                             join cg in carGroups on new { cg = v.CarGroup, country = v.OwningCountry }
                                 equals new { cg.cg, cg.country }
                             select v.VehicleId;
            }
            


            var unmappedCars = from vh in vehicles.Where(d => !matchedSet.Any(m => m == d.VehicleId))
                               select vh;

            if (!unmappedCars.Any())
            {
                return null;
            }

            var remData = from rem in DataContext.VehicleNonRevPeriodEntryRemarks
                join l in DataContext.LOCATIONs on
                    rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Vehicle.LastLocationCode equals l.location1
                          where unmappedCars.Any(d => d.VehicleId == rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Vehicle.VehicleId)
                where rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Active
                group rem by rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.VehicleId
                into g
                join rem2 in DataContext.VehicleNonRevPeriodEntryRemarks
                    on g.Max(d => d.VehicleNonRevPeriodEntryRemarkId) equals rem2.VehicleNonRevPeriodEntryRemarkId
                select g.Key;


            var totalRow = new ComparisonRow
            {
                Key = NotFoundKeyName,
                FleetCount = unmappedCars.Count(),
                NonRevCount = unmappedCars.Sum(d => d.IsNonRev ? 1 : 0),
                ReasonsEntered = remData.Count(),
            };
            return totalRow;
        }

        protected ComparisonRow BuildUnmappedVehicleHistoryRow(bool siteComparison)
        {
            var vehicleHistories = BaseVehicleDataAccess.GetVehicleHistoryQueryable(Parameters, DataContext, true, true);
            if (vehicleHistories == null) return new ComparisonRow();

            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);

            vehicleHistories = from hd in vehicleHistories
                               where hd.TimeStamp == startDate
                               select hd;


            var historyGroups = from v in vehicleHistories
                                select new { v.VehicleHistoryId, cg = v.Vehicle.CarGroup, country = v.Vehicle.OwningCountry, lastLocation = v.LocationCode };

            IQueryable<int> matchedSet;

            if (siteComparison)
            {
                matchedSet = from hg in historyGroups
                             join loc in DataContext.LOCATIONs.Select(d=> d.location1).Distinct()
                                    on hg.lastLocation equals loc
                             select hg.VehicleHistoryId;
            }
            else
            {
                var carGroups = (from cg in DataContext.CAR_GROUPs
                                 select new { cg = cg.car_group1, cg.CAR_CLASS.CAR_SEGMENT.country }).Distinct();

                matchedSet = from hg in historyGroups
                                             join cg in carGroups on new { hg.cg, hg.country }
                                                 equals new { cg.cg, cg.country }
                                             select hg.VehicleHistoryId;
            }


            var unmappedCars = from vh in vehicleHistories.Where(d => !matchedSet.Any(m => m == d.VehicleHistoryId))
                select vh;
            

            if (!unmappedCars.Any())
            {
                return null;
            }

            var totalRow = new ComparisonRow
            {
                Key = NotFoundKeyName,
                FleetCount = unmappedCars.Count(),
                NonRevCount = unmappedCars.Sum(d=> d.IsNonRev ? 1 : 0),
                ReasonsEntered = unmappedCars.Sum(d => d.RemarkId == null ? 0 : 1),
            };
            return totalRow;
        }

        protected List<ComparisonRow> GetHistoryComparisonData(DictionaryParameter comparisonType)
        {
            var groupedData = GetGroupedVehicleHistoryQuery(comparisonType);
            if (groupedData == null)
            {
                return new List<ComparisonRow>();
            }
            var compData = from gd in groupedData
                           select new ComparisonRow
                           {
                               Key = gd.Key,
                               FleetCount = gd.Count(),
                               NonRevCount = gd.Sum(d => d.IsNonRev ? 1 : 0),
                               ReasonsEntered = gd.Sum(d => d.RemarkId == null ? 0 : 1),
                           };
            var returned = compData.ToList();
            return returned;
        }

        public IQueryable<IGrouping<string, VehicleHistory>> GetGroupedVehicleHistoryQuery(DictionaryParameter comparisonType)
        {
            var vehicleHistories = BaseVehicleDataAccess.GetVehicleHistoryQueryable(Parameters, DataContext, true, true);
            if (vehicleHistories == null) return null;


            if (Parameters.ContainsKey(DictionaryParameter.OperationalStatuses))
            {
                if (Parameters[DictionaryParameter.OperationalStatuses] == string.Empty) return null;
                var selectedOperStats = Parameters[DictionaryParameter.OperationalStatuses].Split(',').Select(int.Parse);

                vehicleHistories = vehicleHistories.Where(d => selectedOperStats.Contains(d.OperationalStatusId));
            }

            if (Parameters.ContainsKey(DictionaryParameter.MovementTypes))
            {
                if (Parameters[DictionaryParameter.MovementTypes] == string.Empty) return null;
                var selectedMovementTypes = Parameters[DictionaryParameter.MovementTypes].Split(',').Select(int.Parse);
                vehicleHistories = vehicleHistories.Where(d => selectedMovementTypes.Contains(d.MovementTypeId));
            }

            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);

            vehicleHistories = from hd in vehicleHistories
                               where hd.TimeStamp == startDate
                               select hd;

            IQueryable<IGrouping<string, VehicleHistory>> groupedData;
            switch (comparisonType)
            {
                case DictionaryParameter.LocationCountry:
                    groupedData = from v in vehicleHistories
                                  join l in DataContext.LOCATIONs on v.LocationCode equals l.location1
                                  group v by l.COUNTRy1.country_description
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Pool:
                    groupedData = from v in vehicleHistories
                                  join l in DataContext.LOCATIONs on v.LocationCode equals l.location1
                                  group v by l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.LocationGroup:
                    groupedData = from v in vehicleHistories
                                  join l in DataContext.LOCATIONs on v.LocationCode equals l.location1
                                  group v by l.CMS_LOCATION_GROUP.cms_location_group1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Area:
                    groupedData = from v in vehicleHistories
                                  join l in DataContext.LOCATIONs on v.LocationCode equals l.location1
                                  group v by l.OPS_AREA.ops_area1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Region:
                    groupedData = from v in vehicleHistories
                                  join l in DataContext.LOCATIONs on v.LocationCode equals l.location1
                                  group v by l.OPS_AREA.OPS_REGION.ops_region1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Location:
                    groupedData = from v in vehicleHistories
                                  join l in DataContext.LOCATIONs on v.LocationCode equals l.location1
                                  group v by l.location1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.OwningCountry:
                    groupedData = from v in vehicleHistories
                                  join cg in DataContext.CAR_GROUPs on
                                      new { carG = v.Vehicle.CarGroup, country = v.Vehicle.OwningCountry }
                                      equals new { carG = cg.car_group1, country = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  group v by cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarSegment:
                    groupedData = from v in vehicleHistories
                                  join cg in DataContext.CAR_GROUPs on
                                      new { carG = v.Vehicle.CarGroup, country = v.Vehicle.OwningCountry }
                                      equals new { carG = cg.car_group1, country = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  group v by cg.CAR_CLASS.CAR_SEGMENT.car_segment1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarClass:
                    groupedData = from v in vehicleHistories
                                  join cg in DataContext.CAR_GROUPs on
                                      new { carG = v.Vehicle.CarGroup, country = v.Vehicle.OwningCountry }
                                      equals new { carG = cg.car_group1, country = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  group v by cg.CAR_CLASS.car_class1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarGroup:
                    groupedData = from v in vehicleHistories
                                  group v by v.Vehicle.CarGroup
                                      into gd
                                      select gd;

                    break;

                case DictionaryParameter.OperationalStatusGrouping:
                    groupedData = from v in vehicleHistories
                                  group v by v.Operational_Status.OperationalStatusCode
                                      into gd
                                      select gd;

                    break;

                case DictionaryParameter.KciGrouping:
                    groupedData = from v in vehicleHistories
                                  group v by v.Operational_Status.KCICode
                                      into gd
                                      select gd;

                    break;

                default:
                    return null;
            }
            return groupedData;
        }


        protected List<ComparisonRow> GetCurrentComparisonData(DictionaryParameter comparisonType)
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);
            if (vehicles == null) return new List<ComparisonRow>();

            
            IQueryable<IGrouping<string, VehicleRemarkIdHolder>> groupedRemData = null;

            int minDays = 0;
            if (Parameters.ContainsKey(DictionaryParameter.MinDaysNonRev) &&
                Parameters[DictionaryParameter.MinDaysNonRev] != string.Empty)
            {
                minDays = int.Parse(Parameters[DictionaryParameter.MinDaysNonRev]);
            }

            var allRemarks = from r in DataContext.VehicleNonRevPeriodEntryRemarks
                select r;

            IEnumerable<int> selectedOperStats = null;
            
            
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OperationalStatuses))
            {
                selectedOperStats = Parameters[DictionaryParameter.OperationalStatuses].Split(',').Select(int.Parse);
                allRemarks = allRemarks.Where(d => selectedOperStats.Contains(d.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Vehicle.LastOperationalStatusId));
            }

            IEnumerable<int> selectedMovementTypes = null;
            
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.MovementTypes))
            {
                selectedMovementTypes = Parameters[DictionaryParameter.MovementTypes].Split(',').Select(int.Parse);
                allRemarks = allRemarks.Where(d => selectedMovementTypes.Contains(d.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Vehicle.LastMovementTypeId));
            }


            var remData = from rem in allRemarks
                          //join l in DataContext.LOCATIONs on rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Vehicle.LastLocationCode equals l.location1
                          where rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Active
                            && (rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Vehicle.DaysSinceLastRevenueMovement >= minDays)
                            && rem.ExpectedResolutionDate.Date >= DateTime.Now.Date
                          group rem by rem.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.VehicleId
                              into g
                              join rem2 in DataContext.VehicleNonRevPeriodEntryRemarks
                                      on g.Max(d => d.VehicleNonRevPeriodEntryRemarkId) equals rem2.VehicleNonRevPeriodEntryRemarkId
                              select new VehicleRemarkIdHolder
                              {
                                  VehicleId = g.Key,
                                  ReasonId = rem2.RemarkId
                              };

            

            switch (comparisonType)
            {
                case DictionaryParameter.LocationCountry:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                     group rd by l.COUNTRy1.country_description
                                         into gd
                                         select gd;
                    break;
                case DictionaryParameter.Pool:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                     group rd by l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                         into gd
                                         select gd;

                    break;
                case DictionaryParameter.LocationGroup:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                     group rd by l.CMS_LOCATION_GROUP.cms_location_group1
                                         into gd
                                         select gd;
                    break;
                case DictionaryParameter.Area:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                     group rd by l.OPS_AREA.ops_area1
                                         into gd
                                         select gd;
                    break;
                case DictionaryParameter.Region:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                     group rd by l.OPS_AREA.OPS_REGION.ops_region1
                                         into gd
                                         select gd;

                    break;
                case DictionaryParameter.Location:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     group rd by v.LastLocationCode
                                         into gd
                                         select gd;
                    break;
                case DictionaryParameter.OwningCountry:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     join cg in DataContext.CAR_GROUPs on
                                      new { carG = v.CarGroup, country = v.OwningCountry }
                                      equals new { carG = cg.car_group1, country = cg.CAR_CLASS.CAR_SEGMENT.country }
                                     group rd by cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description
                                         into gd
                                         select gd;

                    break;
                case DictionaryParameter.CarSegment:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     join cg in DataContext.CAR_GROUPs on
                                      new { carG = v.CarGroup, country = v.OwningCountry }
                                      equals new { carG = cg.car_group1, country = cg.CAR_CLASS.CAR_SEGMENT.country }
                                     group rd by cg.CAR_CLASS.CAR_SEGMENT.car_segment1
                                         into gd
                                         select gd;
                    break;
                case DictionaryParameter.CarClass:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     join cg in DataContext.CAR_GROUPs on
                                      new { carG = v.CarGroup, country = v.OwningCountry }
                                      equals new { carG = cg.car_group1, country = cg.CAR_CLASS.CAR_SEGMENT.country }
                                     group rd by cg.CAR_CLASS.car_class1
                                         into gd
                                         select gd;
                    break;
                case DictionaryParameter.CarGroup:
                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     group rd by v.CarGroup
                                         into gd
                                         select gd;
                    break;
                case DictionaryParameter.OperationalStatusGrouping:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     group rd by v.Operational_Status.OperationalStatusCode
                                         into gd
                                         select gd;
                    break;

                case DictionaryParameter.KciGrouping:

                    groupedRemData = from rd in remData
                                     join v in vehicles on rd.VehicleId equals v.VehicleId
                                     group rd by v.Operational_Status.KCICode
                                         into gd
                                         select gd;

                    break;
                default:
                    return null;
            }

            var groupedData = GetGroupedVehicleQueryable(comparisonType);

            var compData = from gd in groupedData
                           select new ComparisonRow
                           {
                               Key = gd.Key,
                               FleetCount = gd.Count(),
                               NonRevCount = gd.Sum(d => d.IsNonRev
                                    && d.DaysSinceLastRevenueMovement >= minDays
                                    && (selectedMovementTypes == null || selectedMovementTypes.Contains(d.LastMovementTypeId))
                                    && (selectedOperStats == null || selectedOperStats.Contains(d.LastOperationalStatusId))
                                    ? 1 : 0),
                           };


            var reasonRows = from rd in groupedRemData
                             select new ComparisonRow
                             {
                                 Key = rd.Key,
                                 ReasonsEntered = rd.Sum(d => d.ReasonId > 0 ? 1 : 0)
                             };

            var returned = compData.ToList();
            foreach (var cr in returned)
            {
                var keyToMatch = cr.Key;
                var reasonRow = reasonRows.FirstOrDefault(d => d.Key == keyToMatch);
                if (reasonRow == null) continue;

                cr.ReasonsEntered = reasonRow.ReasonsEntered;
            }

            return returned;
        }

        public IQueryable<IGrouping<string, Vehicle>> GetGroupedVehicleQueryable(DictionaryParameter comparisonType)
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);
            if (vehicles == null) return null;

            IQueryable<IGrouping<string, Vehicle>> groupedData;
            

            switch (comparisonType)
            {
                case DictionaryParameter.LocationCountry:


                    groupedData = from v in vehicles
                                  join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                  group v by l.COUNTRy1.country_description
                                      into gd
                                      select gd;


                    break;
                case DictionaryParameter.Pool:

                    groupedData = from v in vehicles
                                  join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                  group v by l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.LocationGroup:

                    groupedData = from v in vehicles
                                  join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                  group v by l.CMS_LOCATION_GROUP.cms_location_group1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Area:

                    groupedData = from v in vehicles
                                  join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                  group v by l.OPS_AREA.ops_area1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Region:

                    groupedData = from v in vehicles
                                  join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                  group v by l.OPS_AREA.OPS_REGION.ops_region1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Location:

                    groupedData = from v in vehicles
                                  group v by v.LastLocationCode
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.OwningCountry:

                    groupedData = from v in vehicles
                                  join cg in DataContext.CAR_GROUPs on
                                      new { carG = v.CarGroup, country = v.OwningCountry }
                                      equals new { carG = cg.car_group1, country = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  group v by cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarSegment:


                    groupedData = from v in vehicles
                                  join cg in DataContext.CAR_GROUPs on
                                      new { carG = v.CarGroup, country = v.OwningCountry }
                                      equals new { carG = cg.car_group1, country = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  group v by cg.CAR_CLASS.CAR_SEGMENT.car_segment1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarClass:


                    groupedData = from v in vehicles
                                  join cg in DataContext.CAR_GROUPs on
                                      new { carG = v.CarGroup, country = v.OwningCountry }
                                      equals new { carG = cg.car_group1, country = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  group v by cg.CAR_CLASS.car_class1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarGroup:

                    groupedData = from v in vehicles
                                  group v by v.CarGroup
                                      into gd
                                      select gd;

                    break;


                case DictionaryParameter.OperationalStatusGrouping:


                    groupedData = from v in vehicles
                                  group v by v.Operational_Status.OperationalStatusCode
                                      into gd
                                      select gd;

                    break;

                case DictionaryParameter.KciGrouping:


                    groupedData = from v in vehicles
                                  group v by v.Operational_Status.KCICode
                                      into gd
                                      select gd;

                    break;
                default:
                    return null;
            }


            return groupedData;
        }






    }
}