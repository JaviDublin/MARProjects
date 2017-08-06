using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal
{
    public static class VehicleFieldRestrictions
    {
        public static string Separator = Properties.Settings.Default.Seperator;

        public static IQueryable<Vehicle> RestrictByPredicament(IQueryable<Vehicle> vehicles, Dictionary<DictionaryParameter, string> parameters)
        {
            ForeignVehiclePredicament predicament;
            if (!parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ForeignVehiclePredicament))
            {
                predicament = ForeignVehiclePredicament.IdleInForeignCountry;
            }
            else
            {
                predicament = (ForeignVehiclePredicament)Enum.Parse(typeof(ForeignVehiclePredicament), parameters[DictionaryParameter.ForeignVehiclePredicament]);
            }


            switch (predicament)
            {
                case ForeignVehiclePredicament.All:
                    vehicles = from v in vehicles
                               where v.LOCATION.country != v.OwningCountry
                                   || v.ExpectedLocationCode.Substring(0, 2)  != v.OwningCountry
                                   
                               select v;
                    break;
                case ForeignVehiclePredicament.OnRentOwningCountryToForeign:
                    vehicles = from v in vehicles
                               where v.LOCATION.country == v.OwningCountry
                                    && v.ExpectedLocationCode.Substring(0, 2) != v.OwningCountry
                                    && v.Operational_Status.OperationalStatusId == 12 // RT
                                    && v.Movement_Type.MovementTypeId == 10         // R-O
                               select v;
                    break;
                case ForeignVehiclePredicament.TransferOwningCountryToForeign:
                    vehicles = from v in vehicles
                               where v.LOCATION.country == v.OwningCountry
                                    && v.ExpectedLocationCode.Substring(0, 2) != v.OwningCountry
                                    && (v.Movement_Type.MovementTypeId == 13            // T-O
                                        || v.Movement_Type.MovementTypeId == 4          // L-O
                                        || v.Movement_Type.MovementTypeId == 7          // P-O
                                        || v.Movement_Type.MovementTypeId == 16)        // W-O
                               select v;
                    break;
                case ForeignVehiclePredicament.IdleInForeignCountry:
                    vehicles = RestrictByIdleInForeignCountry(vehicles);
                    break;
                case ForeignVehiclePredicament.OnRentInOrBetweenForeignCountries:
                    vehicles = from v in vehicles
                               where v.LOCATION.country != v.OwningCountry
                                    && v.ExpectedLocationCode.Substring(0, 2) != v.OwningCountry
                                    && v.Operational_Status.OperationalStatusId == 12 // RT
                                    && v.Movement_Type.MovementTypeId == 10         // R-O
                               select v;
                    break;
                case ForeignVehiclePredicament.TransferInOrBetweenForeignCountries:
                    vehicles = from v in vehicles
                               where v.LOCATION.country != v.OwningCountry
                                    && (
                                        v.Movement_Type.MovementTypeId == 13         // T-O
                                    || v.Movement_Type.MovementTypeId == 4          // L-O
                                    || v.Movement_Type.MovementTypeId == 7          // P-O
                                    || v.Movement_Type.MovementTypeId == 16)         // W-O
                               select v;
                    break;
                case ForeignVehiclePredicament.OnRentReturningToOwningCountry:
                    vehicles = from v in vehicles
                               where v.LOCATION.country != v.OwningCountry
                                    && v.ExpectedLocationCode.Substring(0, 2) == v.OwningCountry
                                    && v.Operational_Status.OperationalStatusId == 12 // RT
                                    && v.Movement_Type.MovementTypeId == 10         // R-O
                               select v;

                    break;
                case ForeignVehiclePredicament.TransferReturningToOwningCountry:
                    vehicles = from v in vehicles
                               where v.LOCATION.country != v.OwningCountry
                                       && v.ExpectedLocationCode.Substring(0, 2) == v.OwningCountry
                                   && (
                                        v.Movement_Type.MovementTypeId == 13         // T-O
                                    || v.Movement_Type.MovementTypeId == 4          // L-O
                                    || v.Movement_Type.MovementTypeId == 7          // P-O
                                    || v.Movement_Type.MovementTypeId == 16)         // W-O
                               select v;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return vehicles;
        }

        public static IQueryable<Vehicle> RestrictByIdleInForeignCountry(IQueryable<Vehicle> vehicles)
        {
            vehicles = from v in vehicles
                       where v.LOCATION.country != v.OwningCountry
                            && (v.ExpectedLocationCode == null || v.ExpectedLocationCode.Substring(0, 2) != v.OwningCountry)
                            && v.Movement_Type.MovementTypeId != 13         // T-O
                            && v.Movement_Type.MovementTypeId != 10         // R-O
                            && v.Movement_Type.MovementTypeId != 4          // L-O
                            && v.Movement_Type.MovementTypeId != 7          // P-O
                            && v.Movement_Type.MovementTypeId != 16         // W-O
                       select v;

            return vehicles;
        }

        public static IQueryable<VehicleHistory> RestrictVehicleHistoryByIdleInForeignCountry(IQueryable<VehicleHistory> vehicleHistory)
        {
            vehicleHistory = from vh in vehicleHistory
                       where vh.LOCATION.country != vh.Vehicle.OwningCountry
                            && vh.Movement_Type.MovementTypeId != 13         // T-O
                            && vh.Movement_Type.MovementTypeId != 10         // R-O
                            && vh.Movement_Type.MovementTypeId != 4          // L-O
                            && vh.Movement_Type.MovementTypeId != 7          // P-O
                            && vh.Movement_Type.MovementTypeId != 16         // W-O
                       select vh;

            return vehicleHistory;
        }

        public static IQueryable<Vehicle> RestrictByMatchPredicament(IQueryable<Vehicle> vehicles)
        {
            vehicles = from v in vehicles
                       where v.LOCATION.country != v.OwningCountry
                            && (v.ExpectedLocationCode == null || v.ExpectedLocationCode.Substring(0, 2) != v.OwningCountry)
                            && v.Movement_Type.MovementTypeId != 13         // T-O
                            && v.Movement_Type.MovementTypeId != 10         // R-O
                            && v.Movement_Type.MovementTypeId != 4          // L-O
                            && v.Movement_Type.MovementTypeId != 7          // P-O
                            && v.Movement_Type.MovementTypeId != 16         // W-O
                            && v.LastOperationalStatusId == 12
                       select v;

            return vehicles;
        }

        public static IQueryable<Vehicle> RestrictByAgeingPredicament(IQueryable<Vehicle> vehicles)
        {
            vehicles = from v in vehicles
                       where v.LOCATION.country != v.OwningCountry
                           || v.ExpectedLocationCode.Substring(0, 2) != v.OwningCountry

                       select v;

            return vehicles;
        }

        public static IQueryable<Vehicle> RestrictByOwningCountry(IQueryable<Vehicle> vehicles, string owningCountry)
        {
            if (!owningCountry.Contains(Separator)) return vehicles.Where(d => d.OwningCountry == owningCountry);
            var splitOwningCountry = owningCountry.Split(Separator.ToCharArray());
            return vehicles.Where(d => splitOwningCountry.Contains(d.OwningCountry));
        }

        public static IQueryable<Vehicle> RestrictByCarSegment(IQueryable<Vehicle> vehicles, string carSegment, MarsDBDataContext dataContext)
        {
            if (!carSegment.Contains(Separator)) 
            {
                var carSegmentId = int.Parse(carSegment);
                return from v in vehicles
                           join cg in dataContext.CAR_GROUPs on
                                new { cg = v.CarGroup, oc = v.OwningCountry } equals new { cg = cg.car_group1, oc = cg.CAR_CLASS.CAR_SEGMENT.country }
                           where cg.CAR_CLASS.CAR_SEGMENT.car_segment_id == carSegmentId
                           select v;
            }
            var splitCarSegments = carSegment.Split(Separator.ToCharArray()).Select(int.Parse);

            return from v in vehicles
                   join cg in dataContext.CAR_GROUPs on
                        new { cg = v.CarGroup, oc = v.OwningCountry } equals new { cg = cg.car_group1, oc = cg.CAR_CLASS.CAR_SEGMENT.country }
                   where splitCarSegments.Contains(cg.CAR_CLASS.car_segment_id)
                   select v;
        }

        public static IQueryable<Vehicle> RestrictByCarClass(IQueryable<Vehicle> vehicles, string carClass, MarsDBDataContext dataContext)
        {
            if (!carClass.Contains(Separator))
            {
                var carClassId = int.Parse(carClass);
                return from v in vehicles
                       join cg in dataContext.CAR_GROUPs on
                              new { cg = v.CarGroup, oc = v.OwningCountry } equals new { cg = cg.car_group1, oc = cg.CAR_CLASS.CAR_SEGMENT.country }
                       where cg.CAR_CLASS.car_class_id == carClassId
                       select v;
            }
            var splitCarClasses = carClass.Split(Separator.ToCharArray()).Select(int.Parse);

            return from v in vehicles
                   join cg in dataContext.CAR_GROUPs on
                              new { cg = v.CarGroup, oc = v.OwningCountry } equals new { cg = cg.car_group1, oc = cg.CAR_CLASS.CAR_SEGMENT.country }
                   where splitCarClasses.Contains(cg.car_class_id)
                   select v;
        }

        public static IQueryable<Vehicle> RestrictByCarGroup(IQueryable<Vehicle> vehicles, string carGroup, MarsDBDataContext dataContext)
        {
            if (!carGroup.Contains(Separator))
            {
                var carGroupId = int.Parse(carGroup);
                return from v in vehicles
                       join cg in dataContext.CAR_GROUPs on
                              new { cg = v.CarGroup, oc = v.OwningCountry } equals new { cg = cg.car_group1, oc = cg.CAR_CLASS.CAR_SEGMENT.country }
                       where cg.car_group_id == carGroupId
                       select v;
            }
            var splitCarGroups = carGroup.Split(Separator.ToCharArray()).Select(int.Parse);

            return from v in vehicles
                   join cg in dataContext.CAR_GROUPs on
                              new { cg = v.CarGroup, oc = v.OwningCountry } equals new { cg = cg.car_group1, oc = cg.CAR_CLASS.CAR_SEGMENT.country }
                   where splitCarGroups.Contains(cg.car_group_id)
                   select v;
        }

        public static IQueryable<Vehicle> RestrictByLocationCountry(IQueryable<Vehicle> vehicles, string locationCountry, bool expectedLocationLogic = false)
        {
            if (expectedLocationLogic)
            {
                if (!locationCountry.Contains(Separator)) return vehicles.Where(d => d.ExpectedLocationCode.Substring(0, 2) == locationCountry);
                var splitLocationCountry = locationCountry.Split(Separator.ToCharArray());
                return vehicles.Where(d => splitLocationCountry.Contains(d.ExpectedLocationCode.Substring(0, 2)));
            }
            else
            {
                if (!locationCountry.Contains(Separator)) return vehicles.Where(d => d.LastLocationCode.Substring(0, 2) == locationCountry);
                var splitLocationCountry = locationCountry.Split(Separator.ToCharArray());
                return vehicles.Where(d => splitLocationCountry.Contains(d.LastLocationCode.Substring(0,2)));
            }
        }

        public static IQueryable<Vehicle> RestrictByPool(IQueryable<Vehicle> vehicles, string pool, MarsDBDataContext dataContext, bool expectedLocationLogic = false)
        {
            if (expectedLocationLogic)
            {
                if (!pool.Contains(Separator))
                {
                    var poolId = int.Parse(pool);
                    return from v in vehicles
                               join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                               where l.CMS_LOCATION_GROUP.cms_pool_id == poolId
                               select v;
                }
                var splitPool = pool.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                       where splitPool.Contains(l.CMS_LOCATION_GROUP.cms_pool_id)
                       select v;
            }
            else
            {
                if (!pool.Contains(Separator))
                {
                    var poolId = int.Parse(pool);
                    return from v in vehicles
                           join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                           where l.CMS_LOCATION_GROUP.cms_pool_id == poolId
                           select v;
                }
                var splitPool = pool.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                       where splitPool.Contains(l.CMS_LOCATION_GROUP.cms_pool_id)
                       select v;
            }
        }

        public static IQueryable<Vehicle> RestrictByLocationGroup(IQueryable<Vehicle> vehicles, string locationGroup, MarsDBDataContext dataContext, bool expectedLocationLogic = false)
        {
            if (expectedLocationLogic)
            {
                if (!locationGroup.Contains(Separator))
                {
                    var locationGroupId = int.Parse(locationGroup);
                    return from v in vehicles
                           join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                           where l.cms_location_group_id == locationGroupId
                           select v;
                }
                var splitLocationGroup = locationGroup.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                       where splitLocationGroup.Contains(l.cms_location_group_id.Value)
                       select v;
            }
            else
            {
                if (!locationGroup.Contains(Separator))
                {
                    var locationGroupId = int.Parse(locationGroup);
                    return from v in vehicles
                           join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                           where l.cms_location_group_id == locationGroupId
                           select v;
                }
                var splitLocationGroup = locationGroup.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                       where splitLocationGroup.Contains(l.cms_location_group_id.Value)
                       select v;
            }
        }

        public static IQueryable<Vehicle> RestrictByRegion(IQueryable<Vehicle> vehicles, string region, MarsDBDataContext dataContext, bool expectedLocationLogic = false)
        {
            if (expectedLocationLogic)
            {
                if (!region.Contains(Separator))
                {
                    var regionId = int.Parse(region);
                    return from v in vehicles
                           join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                           where l.OPS_AREA.ops_region_id == regionId
                           select v;
                }
                var splitRegion = region.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                       where splitRegion.Contains(l.OPS_AREA.ops_region_id)
                       select v;
            }
            else
            {
                if (!region.Contains(Separator))
                {
                    var regionId = int.Parse(region);
                    return from v in vehicles
                           join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                           where l.OPS_AREA.ops_region_id == regionId
                           select v;
                }
                var splitRegion = region.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                       where splitRegion.Contains(l.OPS_AREA.ops_region_id)
                       select v;
            }
        }

        public static IQueryable<Vehicle> RestrictByArea(IQueryable<Vehicle> vehicles, string area, MarsDBDataContext dataContext, bool expectedLocationLogic = false)
        {
            if (expectedLocationLogic)
            {
                if (!area.Contains(Separator))
                {
                    var areaId = int.Parse(area);
                    return from v in vehicles
                           join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                           where l.OPS_AREA.ops_area_id == areaId
                           select v;
                }
                var splitArea = area.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                       where splitArea.Contains(l.OPS_AREA.ops_area_id)
                       select v;
            }
            else
            {
                if (!area.Contains(Separator))
                {
                    var areaId = int.Parse(area);
                    return from v in vehicles
                           join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                           where l.OPS_AREA.ops_area_id == areaId
                           select v;
                }
                var splitArea = area.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                       where splitArea.Contains(l.OPS_AREA.ops_area_id)
                       select v;
            }
        }

        public static IQueryable<Vehicle> RestrictByLocation(IQueryable<Vehicle> vehicles, string location, MarsDBDataContext dataContext, bool expectedLocationLogic = false)
        {
            if (expectedLocationLogic)
            {
                if (!location.Contains(Separator))
                {
                    var locationId = int.Parse(location);
                    return from v in vehicles
                           join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                           where l.dim_Location_id == locationId
                           select v;
                }
                var splitlocation = location.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.ExpectedLocationCode equals l.location1
                       where splitlocation.Contains(l.dim_Location_id)
                       select v;
            }
            else
            {
                if (!location.Contains(Separator))
                {
                    var locationId = int.Parse(location);
                    return from v in vehicles
                           join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                           where l.dim_Location_id == locationId
                           select v;
                }
                var splitlocation = location.Split(Separator.ToCharArray()).Select(int.Parse);
                return from v in vehicles
                       join l in dataContext.LOCATIONs on v.LastLocationCode equals l.location1
                       where splitlocation.Contains(l.dim_Location_id)
                       select v;
            }
        }


    }
}