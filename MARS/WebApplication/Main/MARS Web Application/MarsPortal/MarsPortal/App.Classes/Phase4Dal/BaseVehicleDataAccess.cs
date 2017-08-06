using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using App.BLL.ExtensionMethods;
using App.BLL.Utilities;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.LicenceeRestriction;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal
{
    public static class BaseVehicleDataAccess
    {

        public static PercentageDivisorType GetPercentageDivisorTypeFromParameters(Dictionary<DictionaryParameter, string> parameters)
        {
            var type = PercentageDivisorType.Values;
            if (parameters.ContainsKey(DictionaryParameter.PercentageCalculation)
                && parameters[DictionaryParameter.PercentageCalculation] != string.Empty)
            {
                type = (PercentageDivisorType)Enum.Parse(typeof(PercentageDivisorType), parameters[DictionaryParameter.PercentageCalculation]);

            }
            return type;
        }

        public static AvailabilityGrouping GetAvailabilityGroupingFromParameters(Dictionary<DictionaryParameter, string> parameters)
        {
            var availabilityKeyGrouping = AvailabilityGrouping.Average;
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.AvailabilityKeyGrouping))
            {
                availabilityKeyGrouping = (AvailabilityGrouping)Enum.Parse(typeof(AvailabilityGrouping), parameters[DictionaryParameter.AvailabilityKeyGrouping]);
            }

            return availabilityKeyGrouping;
        }
        public static AvailabilityGrouping GetAvailabilityDayGroupingFromParameters(Dictionary<DictionaryParameter, string> parameters)
        {
            var availabilityDayGrouping = AvailabilityGrouping.Average;
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.AvailabilityDayGrouping))
            {
                availabilityDayGrouping = (AvailabilityGrouping)Enum.Parse(typeof(AvailabilityGrouping), parameters[DictionaryParameter.AvailabilityDayGrouping]);
            }

            return availabilityDayGrouping;
        }

        public static IQueryable<VehicleHistory> GetVehicleHistoryQueryable(Dictionary<DictionaryParameter, string> parameters
                                                                            , MarsDBDataContext dataContext
                                                                            , bool includeRev, bool includeNonRev)
        {
            var vehicleHistory = dataContext.VehicleHistories.Select(d => d);


            vehicleHistory = VehicleHistoryRestriction.RestrictVehicleHistoryQueryable(dataContext, vehicleHistory);


            if (!(includeRev && includeNonRev))
            {
                if (!includeRev)
                {
                    vehicleHistory = vehicleHistory.Where(d => d.IsNonRev);
                }
                if (!includeNonRev)
                {
                    vehicleHistory = vehicleHistory.Where(d => !d.IsNonRev);
                }
            }

            if (parameters.ContainsKey(DictionaryParameter.DayOfWeek) &&
                    parameters[DictionaryParameter.DayOfWeek] != string.Empty)
            {
                DayOfWeek dowEntered;
                var success = Enum.TryParse(parameters[DictionaryParameter.DayOfWeek], out dowEntered);
                if (!success) throw new InvalidCastException("Unable to case Day of Week");
                vehicleHistory = vehicleHistory.Where(d => d.TimeStamp.DayOfWeek == dowEntered);
            }

            var owningCountry = parameters.ContainsKey(DictionaryParameter.OwningCountry) ? parameters[DictionaryParameter.OwningCountry] : string.Empty;
            var carSegmentId = parameters.GetIdFromDictionary(DictionaryParameter.CarSegment);
            var carClassId = parameters.GetIdFromDictionary(DictionaryParameter.CarClass);
            var carGroupId = parameters.GetIdFromDictionary(DictionaryParameter.CarGroup);


            var locationCountry = parameters.ContainsKey(DictionaryParameter.LocationCountry) ? parameters[DictionaryParameter.LocationCountry] : string.Empty;

            var poolId = parameters.GetIdFromDictionary(DictionaryParameter.Pool);
            var locationGroupId = parameters.GetIdFromDictionary(DictionaryParameter.LocationGroup);

            var areaId = parameters.GetIdFromDictionary(DictionaryParameter.Area);
            var regionId = parameters.GetIdFromDictionary(DictionaryParameter.Region);

            var locationId = parameters.GetIdFromDictionary(DictionaryParameter.Location);


            var defleetedVehicle = parameters.ContainsKey(DictionaryParameter.DefleetedVehicles) 
                        ? parameters[DictionaryParameter.DefleetedVehicles] : string.Empty;

            var noReason = parameters.ContainsKey(DictionaryParameter.NoReason)
                        ? parameters[DictionaryParameter.NoReason] : string.Empty;

            if (defleetedVehicle == "1")
            {
                vehicleHistory = vehicleHistory.Where(d => d.IsFleet);
            }

            if (carGroupId != 0)
            {
                vehicleHistory = from v in vehicleHistory
                                 join cg in dataContext.CAR_GROUPs on
                                  new { cg = v.Vehicle.CarGroup, oc = v.Vehicle.OwningCountry } equals new { cg = cg.car_group1, oc = cg.CAR_CLASS.CAR_SEGMENT.country }
                                 where cg.car_group_id == carGroupId
                                 select v;
            }
            else if (carClassId != 0)
            {
                vehicleHistory = from v in vehicleHistory
                                 join cg in dataContext.CAR_GROUPs on
                                    new { cg = v.Vehicle.CarGroup, oc = v.Vehicle.OwningCountry } equals new { cg = cg.car_group1, oc = cg.CAR_CLASS.CAR_SEGMENT.country }
                                 where cg.CAR_CLASS.car_class_id == carClassId
                                 select v;
            }
            else if (carSegmentId != 0)
            {
                vehicleHistory = from v in vehicleHistory
                                 join cg in dataContext.CAR_GROUPs on
                                      new { cg = v.Vehicle.CarGroup, oc = v.Vehicle.OwningCountry } equals new { cg = cg.car_group1, oc = cg.CAR_CLASS.CAR_SEGMENT.country }
                                 where cg.CAR_CLASS.CAR_SEGMENT.car_segment_id == carSegmentId
                                 select v;
            }
            else if (!string.IsNullOrEmpty(owningCountry))
            {
                vehicleHistory = vehicleHistory.Where(d => d.Vehicle.OwningCountry == owningCountry);
            }

            if (locationId != 0)
            {
                vehicleHistory = from v in vehicleHistory
                                 join l in dataContext.LOCATIONs on v.LocationCode equals l.location1
                                 where l.dim_Location_id == locationId
                                 select v;
            }
            else
            {
                if (locationGroupId != 0)
                {
                    vehicleHistory = from v in vehicleHistory
                                     join l in dataContext.LOCATIONs on v.LocationCode equals l.location1
                                     where l.cms_location_group_id == locationGroupId
                                     select v;
                }
                else if (poolId != 0)
                {
                    vehicleHistory = from v in vehicleHistory
                                     join l in dataContext.LOCATIONs on v.LocationCode equals l.location1
                                     where l.CMS_LOCATION_GROUP.cms_pool_id == poolId
                                     select v;
                }


                if (regionId != 0)
                {
                    vehicleHistory = from v in vehicleHistory
                                     join l in dataContext.LOCATIONs on v.LocationCode equals l.location1
                                     where l.OPS_AREA.ops_region_id == regionId
                                     select v;
                }
                else if (areaId != 0)
                {
                    vehicleHistory = from v in vehicleHistory
                                     join l in dataContext.LOCATIONs on v.LocationCode equals l.location1
                                     where l.ops_area_id == areaId
                                     select v;
                }

                if (!string.IsNullOrEmpty(locationCountry))
                {
                    vehicleHistory = from v in vehicleHistory
                        join l in dataContext.LOCATIONs on v.LocationCode equals l.location1
                        where l.location1.Substring(0, 2).ToLower() == locationCountry.ToLower()
                        select v;
                }
            }



            //if (parameters.ContainsKey(DictionaryParameter.MinDaysNonRev) 
            //    && !string.IsNullOrEmpty(parameters[DictionaryParameter.MinDaysNonRev]))
            //{
            //    var minDays = int.Parse(parameters[DictionaryParameter.MinDaysNonRev]);

            //    vehicleHistory = from vd in vehicleHistory
            //                  where vd.DaysNonRev >= minDays
            //                  select vd;
            //}

            if (noReason == string.Empty)
            {
                vehicleHistory = vehicleHistory.Where(d => d.RemarkId == null);
            }

            if (parameters.ContainsKey(DictionaryParameter.FleetTypes))
            {
                if (parameters[DictionaryParameter.FleetTypes] == string.Empty) return null;
                var selectedFleetTypes = parameters[DictionaryParameter.FleetTypes].Split(',').Select(int.Parse);
                vehicleHistory = vehicleHistory.Where(d => selectedFleetTypes.Contains(d.VehicleFleetTypeId));
            }



            if (parameters.ContainsKey(DictionaryParameter.OwningArea))
            {
                if (parameters[DictionaryParameter.OwningArea] != string.Empty)
                {
                    //var selectedOwningAreas = parameters[DictionaryParameter.OwningArea].Split(',').Select(d => d);
                    //vehicleHistory = vehicleHistory.Where(d => selectedOwningAreas.Contains(d.Vehicle.OwningArea));
                    vehicleHistory = vehicleHistory.Where(d => d.Vehicle.OwningArea == parameters[DictionaryParameter.OwningArea]); 
                }
                
            }
            return vehicleHistory;
        }


        public static IQueryable<Vehicle> GetVehicleQueryable(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dataContext
                        , bool includeRev, bool includeNonRev)
        {
            var vehicles = dataContext.Vehicles.Select(d => d);

            vehicles = VehicleRestriction.RestrictVehicleQueryable(dataContext, vehicles);

            if (!(includeRev && includeNonRev))
            {
                if (!includeRev)
                {
                    vehicles = vehicles.Where(d => d.IsNonRev);
                }
                if (!includeNonRev)
                {
                    vehicles = vehicles.Where(d => !d.IsNonRev);
                }
            }

            var defleetedVehicle = parameters.ContainsKey(DictionaryParameter.DefleetedVehicles)
            ? parameters[DictionaryParameter.DefleetedVehicles] : string.Empty;

            var noReason = parameters.ContainsKey(DictionaryParameter.NoReason)
                  ? parameters[DictionaryParameter.NoReason] : string.Empty;

            if (defleetedVehicle == string.Empty)
            {
                vehicles = vehicles.Where(d => d.IsFleet);
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                vehicles = VehicleFieldRestrictions.RestrictByCarGroup(vehicles,
                    parameters[DictionaryParameter.CarGroup], dataContext);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                vehicles = VehicleFieldRestrictions.RestrictByCarClass(vehicles,
                    parameters[DictionaryParameter.CarClass], dataContext);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                vehicles = VehicleFieldRestrictions.RestrictByCarSegment(vehicles,
                    parameters[DictionaryParameter.CarSegment], dataContext);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                vehicles = VehicleFieldRestrictions.RestrictByOwningCountry(vehicles,
                    parameters[DictionaryParameter.OwningCountry]);
            }

            var expectedLocationLogic = parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ExpectedLocationLogic);

            bool restrictByLocationCountry = true;
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                vehicles = VehicleFieldRestrictions.RestrictByLocation(vehicles,
                    parameters[DictionaryParameter.Location], dataContext, expectedLocationLogic);
                restrictByLocationCountry = false;
            }
            else
            {
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
                {
                    vehicles = VehicleFieldRestrictions.RestrictByLocationGroup(vehicles,
                        parameters[DictionaryParameter.LocationGroup], dataContext, expectedLocationLogic);
                    restrictByLocationCountry = false;
                }
                else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
                {
                    vehicles = VehicleFieldRestrictions.RestrictByPool(vehicles,
                        parameters[DictionaryParameter.Pool], dataContext, expectedLocationLogic);
                    restrictByLocationCountry = false;
                }

                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
                {
                    vehicles = VehicleFieldRestrictions.RestrictByArea(vehicles,
                        parameters[DictionaryParameter.Area], dataContext, expectedLocationLogic);
                    restrictByLocationCountry = false;
                }
                else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
                {
                    vehicles = VehicleFieldRestrictions.RestrictByRegion(vehicles,
                        parameters[DictionaryParameter.Region], dataContext, expectedLocationLogic);
                    restrictByLocationCountry = false;
                }
            }

            if (restrictByLocationCountry && parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                vehicles = VehicleFieldRestrictions.RestrictByLocationCountry(vehicles,
                        parameters[DictionaryParameter.LocationCountry], expectedLocationLogic);
            }

            if (noReason == "0")
            {
                var reasonsEntered = from per in dataContext.VehicleNonRevPeriodEntryRemarks
                    where per.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Active
                    && per.ExpectedResolutionDate >= DateTime.Now.Date           //
                    select per.VehicleNonRevPeriodEntry.VehicleNonRevPeriod.Vehicle;

                vehicles = vehicles.Except(reasonsEntered.Distinct());

            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.FleetTypes))
            {
                
                var selectedFleetTypes = parameters[DictionaryParameter.FleetTypes].Split(',').Select(byte.Parse);
                vehicles = vehicles.Where(d => selectedFleetTypes.Contains(d.VehicleFleetTypeId));
            }


            if (parameters.ContainsKey(DictionaryParameter.OwningArea))
            {
                if (parameters[DictionaryParameter.OwningArea] != string.Empty)
                {
                    //var selectedOwningAreas = parameters[DictionaryParameter.OwningArea].Split(',').Select(d => d);
                    //vehicles = vehicles.Where(d => selectedOwningAreas.Contains(d.OwningArea));
                    vehicles = vehicles.Where(d => d.OwningArea == parameters[DictionaryParameter.OwningArea]); 
                }   
            }
            return vehicles;
        }

        public static IQueryable<Vehicle> RestrictByAdditionalParameters(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dataContext
                                                , IQueryable<Vehicle> vehicleData)
        {
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ForeignVehiclePredicament))
            {
                vehicleData = VehicleFieldRestrictions.RestrictByPredicament(vehicleData, parameters);

            }

            if (parameters.ContainsKey(DictionaryParameter.LicencePlate) &&
                !string.IsNullOrEmpty(parameters[DictionaryParameter.LicencePlate]))
            {
                vehicleData = vehicleData.Where(d => d.LicensePlate.Contains(parameters[DictionaryParameter.LicencePlate]));
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ExcludeOverdue))
            {
                vehicleData = parameters[DictionaryParameter.ExcludeOverdue] == "1"
                    ? vehicleData.Where(d => d.ExpectedDateTime.HasValue && d.ExpectedDateTime.Value.Date >= DateTime.Now.Date)
                    : vehicleData.Where(d => d.ExpectedDateTime.HasValue && d.ExpectedDateTime.Value.Date < DateTime.Now.Date);

            }

            if (parameters.ContainsKey(DictionaryParameter.Vin) && !string.IsNullOrEmpty(parameters[DictionaryParameter.Vin]))
            {
                vehicleData = vehicleData.Where(d => d.Vin.Contains(parameters[DictionaryParameter.Vin]));
            }

            if (parameters.ContainsKey(DictionaryParameter.UnitNumber)
                    && !string.IsNullOrEmpty(parameters[DictionaryParameter.UnitNumber]))
            {
                vehicleData = vehicleData.Where(d => d.UnitNumber == int.Parse(parameters[DictionaryParameter.UnitNumber]));
            }

            if (parameters.ContainsKey(DictionaryParameter.DriverName) && !string.IsNullOrEmpty(parameters[DictionaryParameter.DriverName]))
            {
                vehicleData = vehicleData.Where(d => d.LastDriverName.Contains(parameters[DictionaryParameter.DriverName]));
            }

            if (parameters.ContainsKey(DictionaryParameter.Colour) && !string.IsNullOrEmpty(parameters[DictionaryParameter.Colour]))
            {
                vehicleData = vehicleData.Where(d => d.Colour == parameters[DictionaryParameter.Colour]);
            }

            if (parameters.ContainsKey(DictionaryParameter.ModelDescription)
                && !string.IsNullOrEmpty(parameters[DictionaryParameter.ModelDescription]))
            {
                vehicleData = vehicleData.Where(d => d.ModelDescription.Contains(parameters[DictionaryParameter.ModelDescription]));
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OperationalStatuses))
            {

                var selectedOperStats = parameters[DictionaryParameter.OperationalStatuses].Split(',').Select(int.Parse);

                vehicleData = vehicleData.Where(d => selectedOperStats.Contains(d.LastOperationalStatusId));
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.MovementTypes))
            {

                var selectedMovementTypes = parameters[DictionaryParameter.MovementTypes].Split(',').Select(int.Parse);
                vehicleData = vehicleData.Where(d => selectedMovementTypes.Contains(d.LastMovementTypeId));
            }

            

            if (parameters.ContainsKey(DictionaryParameter.MinDaysNonRev) && parameters[DictionaryParameter.MinDaysNonRev] != string.Empty)
            {
                var minDays = int.Parse(parameters[DictionaryParameter.MinDaysNonRev]);

                vehicleData = from vd in vehicleData
                              where vd.DaysSinceLastRevenueMovement >= minDays
                              select vd;
            }

            return vehicleData;
        }
    }
}