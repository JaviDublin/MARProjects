using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using App.BLL.ExtensionMethods;
using App.Entities;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Parameters;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;

namespace Mars.App.Classes.Phase4Dal.Availability
{
    public class ComparisonDataAccess : AvailabilityDataAccess
    {
        public ComparisonDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {

        }

        public List<FleetStatusRow> GetComparisonData(bool siteComparison)
        {
            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);

            var returned = startDate == DateTime.Now.Date
                            ? GetCurrentFleetComparison(siteComparison)
                            : GetHistoricalComparison(siteComparison);

            //var returned = GetHistoricalComparison(siteComparison);
            return returned;
        }

        private List<FleetStatusRow> GetCurrentFleetComparison(bool siteComparison)
        {
            var comparisonType = siteComparison ? ComparisonLevelLookup.GetSiteComparisonTypeFromParameters(Parameters)
                : ComparisonLevelLookup.GetFleetComparisonTypeFromParameters(Parameters);


            var groupedData = GetSingleKeyGroupedVehicle(comparisonType);

            var extractedData = ExtractVehicleColumns(groupedData);


            var returned = extractedData.ToList();
            return returned;
        }



        private List<FleetStatusRow> GetHistoricalComparison(bool siteComparison)
        {
            var comparisonType = siteComparison ? ComparisonLevelLookup.GetSiteComparisonTypeFromParameters(Parameters)
                : ComparisonLevelLookup.GetFleetComparisonTypeFromParameters(Parameters);

            var groupedQueryable = GetTwoKeyGroupedAvailabilityHistory(comparisonType);


            var availabilityKeyGrouping = BaseVehicleDataAccess.GetAvailabilityGroupingFromParameters(Parameters);

            var fleetDayKeyGrouping = ExtractFleetHistoryColumns(availabilityKeyGrouping, groupedQueryable);
            var fleetKeyGrouping = GroupByKey(fleetDayKeyGrouping);


            var returned = fleetKeyGrouping.ToList();
            return returned;
        }

        public IQueryable<IGrouping<string, Vehicle>> GetSingleKeyGroupedVehicle(DictionaryParameter comparisonType)
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);

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
                                  join cg in DataContext.CAR_GROUPs on new { v.CarGroup, v.OwningCountry } equals
                                                new { CarGroup = cg.car_group1, OwningCountry = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  group v by cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarSegment:


                    groupedData = from v in vehicles
                                  join cg in DataContext.CAR_GROUPs on new { v.CarGroup, v.OwningCountry } equals
                                                new { CarGroup = cg.car_group1, OwningCountry = cg.CAR_CLASS.CAR_SEGMENT.country }
                                  group v by cg.CAR_CLASS.CAR_SEGMENT.car_segment1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarClass:
                    groupedData = from v in vehicles
                                  join cg in DataContext.CAR_GROUPs on new {v.CarGroup, v.OwningCountry } equals 
                                                new {CarGroup = cg.car_group1 , OwningCountry = cg.CAR_CLASS.CAR_SEGMENT.country}
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

                default:
                    return null;
            }


            return groupedData;

        }


        public IQueryable<FleetStatusRow> ExtractVehicleColumns(IQueryable<IGrouping<string, Vehicle>> groupedQueryable)
        {
            var divisorType = BaseVehicleDataAccess.GetPercentageDivisorTypeFromParameters(Parameters);
            var dt = DateTime.Now.Date;
            var fsrData = from gq in groupedQueryable
                                      select new FleetStatusRow(divisorType, false)
                                      {
                                          Key = gq.Key,
                                          TotalFleet = gq.Count(),
                                          Cu = gq.Sum(d => d.LastOperationalStatusId == 2 ? 1 : 0),
                                          Ha = gq.Sum(d => d.LastOperationalStatusId == 4 ? 1 : 0),
                                          Hl = gq.Sum(d => d.LastOperationalStatusId == 5 ? 1 : 0),
                                          Ll = gq.Sum(d => d.LastOperationalStatusId == 6 ? 1 : 0),
                                          Nc = gq.Sum(d => d.LastOperationalStatusId == 8 ? 1 : 0),
                                          Pl = gq.Sum(d => d.LastOperationalStatusId == 9 ? 1 : 0),
                                          Tc = gq.Sum(d => d.LastOperationalStatusId == 16 ? 1 : 0),
                                          Sv = gq.Sum(d => d.LastOperationalStatusId == 14 ? 1 : 0),
                                          Ws = gq.Sum(d => d.LastOperationalStatusId == 19 ? 1 : 0),
                                          Bd = gq.Sum(d => d.LastOperationalStatusId == 1 ? 1 : 0),
                                          Mm = gq.Sum(d => d.LastOperationalStatusId == 7 ? 1 : 0),
                                          Tw = gq.Sum(d => d.LastOperationalStatusId == 18 ? 1 : 0),
                                          Tb = gq.Sum(d => d.LastOperationalStatusId == 15 ? 1 : 0),
                                          Fs = gq.Sum(d => d.LastOperationalStatusId == 3 ? 1 : 0),
                                          Rl = gq.Sum(d => d.LastOperationalStatusId == 10 ? 1 : 0),
                                          Rp = gq.Sum(d => d.LastOperationalStatusId == 11 ? 1 : 0),
                                          Tn = gq.Sum(d => d.LastOperationalStatusId == 17 ? 1 : 0),
                                          Idle = gq.Sum(d => (d.LastOperationalStatusId == 12 && d.LastMovementTypeId != 10) ? 1 : 0),
                                          Su = gq.Sum(d => d.LastOperationalStatusId == 13 ? 1 : 0),
                                          Overdue = gq.Sum(d => (d.LastOperationalStatusId == 12 && d.LastMovementTypeId == 10
                                                                           && d.ExpectedDateTime < dt) ? 1 : 0)
                                      };
            var returned = fsrData;
            return returned;
        }

        /// <summary>
        /// Special version of the Extract Columns method in the base class. Groups by StringDateGrouping rather than just date
        /// </summary>
        private IQueryable<FleetStatusRow> ExtractFleetHistoryColumns(AvailabilityGrouping columnRequested, IQueryable<IGrouping<StringDateGrouping, FleetHistory>> groupedQueryable)
        {
            var divisorType = BaseVehicleDataAccess.GetPercentageDivisorTypeFromParameters(Parameters);

            IQueryable<FleetStatusRow> fleetDayKeyGrouping;
            switch (columnRequested)
            {
                case AvailabilityGrouping.Min:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Key = gq.Key.Key,
                                              Day = gq.Key.Date,
                                              TotalFleet = gq.Sum(d => d.MinTotal),
                                              Bd = gq.Sum(d => d.MinBd),
                                              Cu = gq.Sum(d => d.MinCu),
                                              Fs = gq.Sum(d => d.MinFs),
                                              Ha = gq.Sum(d => d.MinHa),
                                              Hl = gq.Sum(d => d.MinHl),
                                              Ll = gq.Sum(d => d.MinLl),
                                              Mm = gq.Sum(d => d.MinMm),
                                              Nc = gq.Sum(d => d.MinNc),
                                              Pl = gq.Sum(d => d.MinPl),
                                              Rl = gq.Sum(d => d.MinRl),
                                              Rp = gq.Sum(d => d.MinRp),
                                              Idle = gq.Sum(d => d.MinIdle),
                                              Su = gq.Sum(d => d.MinSu),
                                              Sv = gq.Sum(d => d.MinSv),
                                              Tb = gq.Sum(d => d.MinTb),
                                              Tc = gq.Sum(d => d.MinTc),
                                              Tn = gq.Sum(d => d.MinTn),
                                              Tw = gq.Sum(d => d.MinTw),
                                              Ws = gq.Sum(d => d.MinWs),
                                              Overdue = gq.Sum(d => d.MinOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.MinOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.MinAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d => d.MinOnRent)
                                          };
                    break;
                case AvailabilityGrouping.Max:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Key = gq.Key.Key,
                                              Day = gq.Key.Date,
                                              TotalFleet = gq.Sum(d => d.MaxTotal),
                                              Bd = gq.Sum(d => d.MaxBd),
                                              Cu = gq.Sum(d => d.MaxCu),
                                              Fs = gq.Sum(d => d.MaxFs),
                                              Ha = gq.Sum(d => d.MaxHa),
                                              Hl = gq.Sum(d => d.MaxHl),
                                              Ll = gq.Sum(d => d.MaxLl),
                                              Mm = gq.Sum(d => d.MaxMm),
                                              Nc = gq.Sum(d => d.MaxNc),
                                              Pl = gq.Sum(d => d.MaxPl),
                                              Rl = gq.Sum(d => d.MaxRl),
                                              Rp = gq.Sum(d => d.MaxRp),
                                              Idle = gq.Sum(d => d.MaxIdle),
                                              Su = gq.Sum(d => d.MaxSu),
                                              Sv = gq.Sum(d => d.MaxSv),
                                              Tb = gq.Sum(d => d.MaxTb),
                                              Tc = gq.Sum(d => d.MaxTc),
                                              Tn = gq.Sum(d => d.MaxTn),
                                              Tw = gq.Sum(d => d.MaxTw),
                                              Ws = gq.Sum(d => d.MaxWs),
                                              Overdue = gq.Sum(d => d.MaxOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.MaxOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.MaxAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d => d.MaxOnRent)
                                          };
                    break;
                case AvailabilityGrouping.Average:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Key = gq.Key.Key,
                                              Day = gq.Key.Date,
                                              TotalFleet = gq.Sum(d => d.AvgTotal),
                                              Bd = gq.Sum(d => d.AvgBd),
                                              Cu = gq.Sum(d => d.AvgCu),
                                              Fs = gq.Sum(d => d.AvgFs),
                                              Ha = gq.Sum(d => d.AvgHa),
                                              Hl = gq.Sum(d => d.AvgHl),
                                              Ll = gq.Sum(d => d.AvgLl),
                                              Mm = gq.Sum(d => d.AvgMm),
                                              Nc = gq.Sum(d => d.AvgNc),
                                              Pl = gq.Sum(d => d.AvgPl),
                                              Rl = gq.Sum(d => d.AvgRl),
                                              Rp = gq.Sum(d => d.AvgRp),
                                              Idle = gq.Sum(d => d.AvgIdle),
                                              Su = gq.Sum(d => d.AvgSu),
                                              Sv = gq.Sum(d => d.AvgSv),
                                              Tb = gq.Sum(d => d.AvgTb),
                                              Tc = gq.Sum(d => d.AvgTc),
                                              Tn = gq.Sum(d => d.AvgTn),
                                              Tw = gq.Sum(d => d.AvgTw),
                                              Ws = gq.Sum(d => d.AvgWs),
                                              Overdue = gq.Sum(d => d.AvgOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.AvgOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.AvgAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d => d.AvgOnRent)
                                          };
                    break;
                case AvailabilityGrouping.Peak:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Key = gq.Key.Key,
                                              Day = gq.Key.Date,
                                              TotalFleet = gq.Sum(d => d.PeakTotal),
                                              Bd = gq.Sum(d => d.PeakBd),
                                              Cu = gq.Sum(d => d.PeakCu),
                                              Fs = gq.Sum(d => d.PeakFs),
                                              Ha = gq.Sum(d => d.PeakHa),
                                              Hl = gq.Sum(d => d.PeakHl),
                                              Ll = gq.Sum(d => d.PeakLl),
                                              Mm = gq.Sum(d => d.PeakMm),
                                              Nc = gq.Sum(d => d.PeakNc),
                                              Pl = gq.Sum(d => d.PeakPl),
                                              Rl = gq.Sum(d => d.PeakRl),
                                              Rp = gq.Sum(d => d.PeakRp),
                                              Idle = gq.Sum(d => d.PeakIdle),
                                              Su = gq.Sum(d => d.PeakSu),
                                              Sv = gq.Sum(d => d.PeakSv),
                                              Tb = gq.Sum(d => d.PeakTb),
                                              Tc = gq.Sum(d => d.PeakTc),
                                              Tn = gq.Sum(d => d.PeakTn),
                                              Tw = gq.Sum(d => d.PeakTw),
                                              Ws = gq.Sum(d => d.PeakWs),
                                              Overdue = gq.Sum(d => d.PeakOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.PeakOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.PeakAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d => d.PeakOnRent)
                                          };
                    break;
                case AvailabilityGrouping.Trough:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Key = gq.Key.Key,
                                              Day = gq.Key.Date,
                                              TotalFleet = gq.Sum(d => d.TroughTotal),
                                              Bd = gq.Sum(d => d.TroughBd),
                                              Cu = gq.Sum(d => d.TroughCu),
                                              Fs = gq.Sum(d => d.TroughFs),
                                              Ha = gq.Sum(d => d.TroughHa),
                                              Hl = gq.Sum(d => d.TroughHl),
                                              Ll = gq.Sum(d => d.TroughLl),
                                              Mm = gq.Sum(d => d.TroughMm),
                                              Nc = gq.Sum(d => d.TroughNc),
                                              Pl = gq.Sum(d => d.TroughPl),
                                              Rl = gq.Sum(d => d.TroughRl),
                                              Rp = gq.Sum(d => d.TroughRp),
                                              Idle = gq.Sum(d => d.TroughIdle),
                                              Su = gq.Sum(d => d.TroughSu),
                                              Sv = gq.Sum(d => d.TroughSv),
                                              Tb = gq.Sum(d => d.TroughTb),
                                              Tc = gq.Sum(d => d.TroughTc),
                                              Tn = gq.Sum(d => d.TroughTn),
                                              Tw = gq.Sum(d => d.TroughTw),
                                              Ws = gq.Sum(d => d.TroughWs),
                                              Overdue = gq.Sum(d => d.TroughOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.TroughOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.TroughAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d => d.TroughOnRent)
                                          };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return fleetDayKeyGrouping;
        }


        
    }
}