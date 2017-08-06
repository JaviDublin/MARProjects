using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal.AdditionDeletions
{
    public static class AdditionFilter
    {
        public static IQueryable<ResAddition> GetAdditions(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dataContext)
        {
            var additions = from add in dataContext.ResAdditions
                          select add;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.StartDate) &&
                parameters.ContainsValueAndIsntEmpty(DictionaryParameter.EndDate))
            {
                var startDate = DateTime.Parse(parameters[DictionaryParameter.StartDate]);
                var endDate = DateTime.Parse(parameters[DictionaryParameter.EndDate]);

                additions = additions.Where(d => d.RepDate >= startDate && d.RepDate <= endDate);
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                additions = RestrictByCarGroup(additions,
                    parameters[DictionaryParameter.CarGroup]);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                additions = RestrictByCarClass(additions,
                    parameters[DictionaryParameter.CarClass]);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                additions = RestrictByCarSegment(additions,
                    parameters[DictionaryParameter.CarSegment]);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                additions = RestrictByOwningCountry(additions,
                    parameters[DictionaryParameter.OwningCountry]);
            }
            //
            bool restrictByLocationCountry = true;
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                additions = RestrictByLocation(additions,
                    parameters[DictionaryParameter.Location]);
                restrictByLocationCountry = false;
            }
            else
            {
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
                {
                    additions = RestrictByLocationGroup(additions,
                        parameters[DictionaryParameter.LocationGroup]);
                    restrictByLocationCountry = false;
                }
                else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
                {
                    additions = RestrictByPool(additions,
                        parameters[DictionaryParameter.Pool]);
                    restrictByLocationCountry = false;
                }

                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
                {
                    additions = RestrictByArea(additions,
                        parameters[DictionaryParameter.Area]);
                    restrictByLocationCountry = false;
                }
                else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
                {
                    additions = RestrictByRegion(additions,
                        parameters[DictionaryParameter.Region]);
                    restrictByLocationCountry = false;
                }
            }

            if (restrictByLocationCountry && parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                additions = RestrictByLocationCountry(additions,
                        parameters[DictionaryParameter.LocationCountry]);
            }
            return additions;
        }

        private static IQueryable<ResAddition> RestrictByOwningCountry(IQueryable<ResAddition> vehicles, string owningCountry)
        {
            if (!owningCountry.Contains(VehicleFieldRestrictions.Separator)) return vehicles.Where(d =>
                d.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country == owningCountry);
            var splitOwningCountry = owningCountry.Split(VehicleFieldRestrictions.Separator.ToCharArray());
            return vehicles.Where(d => splitOwningCountry.Contains(d.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country));
        }

        private static IQueryable<ResAddition> RestrictByCarSegment(IQueryable<ResAddition> vehicles, string carSegment)
        {
            if (!carSegment.Contains(VehicleFieldRestrictions.Separator))
            {
                var carSegmentId = int.Parse(carSegment);
                return from v in vehicles
                       where v.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment_id == carSegmentId
                       select v;
            }
            var splitCarSegments = carSegment.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);

            return from v in vehicles
                   where splitCarSegments.Contains(v.CAR_GROUP.CAR_CLASS.car_segment_id)
                   select v;
        }

        private static IQueryable<ResAddition> RestrictByCarClass(IQueryable<ResAddition> vehicles, string carClass)
        {
            if (!carClass.Contains(VehicleFieldRestrictions.Separator))
            {
                var carClassId = int.Parse(carClass);
                return from v in vehicles
                       where v.CAR_GROUP.CAR_CLASS.car_class_id == carClassId
                       select v;
            }
            var splitCarClasses = carClass.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);

            return from v in vehicles
                   where splitCarClasses.Contains(v.CAR_GROUP.car_class_id)
                   select v;
        }

        private static IQueryable<ResAddition> RestrictByCarGroup(IQueryable<ResAddition> vehicles, string carGroup)
        {
            if (!carGroup.Contains(VehicleFieldRestrictions.Separator))
            {
                var carGroupId = int.Parse(carGroup);
                return from v in vehicles
                       where v.CarGrpId == carGroupId
                       select v;
            }
            var splitCarGroups = carGroup.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);

            return from v in vehicles
                   where splitCarGroups.Contains(v.CarGrpId)
                   select v;
        }

        private static IQueryable<ResAddition> RestrictByLocationCountry(IQueryable<ResAddition> vehicles, string locationCountry)
        {

            if (!locationCountry.Contains(VehicleFieldRestrictions.Separator))
            {
                var locationId = int.Parse(locationCountry);
                return vehicles.Where(d => d.LocId == locationId);
            }
            var splitLocationCountry = locationCountry.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);
            return vehicles.Where(d => splitLocationCountry.Contains(d.LocId));

        }

        private static IQueryable<ResAddition> RestrictByPool(IQueryable<ResAddition> vehicles, string pool)
        {
            if (!pool.Contains(VehicleFieldRestrictions.Separator))
            {
                var poolId = int.Parse(pool);
                return from v in vehicles
                       where v.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId
                       select v;
            }
            var splitPool = pool.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);
            return from v in vehicles
                   where splitPool.Contains(v.LOCATION.CMS_LOCATION_GROUP.cms_pool_id)
                   select v;

        }

        private static IQueryable<ResAddition> RestrictByLocationGroup(IQueryable<ResAddition> vehicles, string locationGroup)
        {
            if (!locationGroup.Contains(VehicleFieldRestrictions.Separator))
            {
                var locationGroupId = int.Parse(locationGroup);
                return from v in vehicles
                       where v.LOCATION.cms_location_group_id == locationGroupId
                       select v;
            }
            var splitLocationGroup = locationGroup.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);
            return from v in vehicles
                   where splitLocationGroup.Contains(v.LOCATION.cms_location_group_id.Value)
                   select v;
        }

        private static IQueryable<ResAddition> RestrictByRegion(IQueryable<ResAddition> vehicles, string region)
        {
            if (!region.Contains(VehicleFieldRestrictions.Separator))
            {
                var regionId = int.Parse(region);
                return from v in vehicles
                       where v.LOCATION.OPS_AREA.ops_region_id == regionId
                       select v;
            }
            var splitRegion = region.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);
            return from v in vehicles
                   where splitRegion.Contains(v.LOCATION.OPS_AREA.ops_region_id)
                   select v;

        }

        private static IQueryable<ResAddition> RestrictByArea(IQueryable<ResAddition> vehicles, string area)
        {

            if (!area.Contains(VehicleFieldRestrictions.Separator))
            {
                var areaId = int.Parse(area);
                return from v in vehicles
                       where v.LOCATION.OPS_AREA.ops_area_id == areaId
                       select v;
            }
            var splitArea = area.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);
            return from v in vehicles
                   where splitArea.Contains(v.LOCATION.OPS_AREA.ops_area_id)
                   select v;

        }

        private static IQueryable<ResAddition> RestrictByLocation(IQueryable<ResAddition> vehicles, string location)
        {

            if (!location.Contains(VehicleFieldRestrictions.Separator))
            {
                var locationId = int.Parse(location);
                return from v in vehicles
                       where v.LocId == locationId
                       select v;
            }
            var splitlocation = location.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);
            return from v in vehicles
                   where splitlocation.Contains(v.LocId)
                   select v;


        }

    }
}