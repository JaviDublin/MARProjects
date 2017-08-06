using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Dal.Reservations
{
    public static class ReservationFieldRestrictions
    {
        public static string Separator = Properties.Settings.Default.Seperator;

        public static IQueryable<Reservation> RestrictByOwningCountry(IQueryable<Reservation> res, string owningCountry)
        {
            if (!owningCountry.Contains(Separator))
            {
                return res.Where(d => d.Country == owningCountry);    
            }
            var splitOwningCountry = owningCountry.Split(Separator.ToCharArray());
            return res.Where(d => splitOwningCountry.Contains(d.Country));
            
        }

        public static IQueryable<Reservation> RestrictByLocationCountry(IQueryable<Reservation> res, string locationCountry
                , bool checkoutLogic )
        {
            if (!locationCountry.Contains(Separator))
            {
                return checkoutLogic
                    ? res.Where(d => d.PickupLocation.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountry)
                    : res.Where(d => d.ReturnLocation.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountry);                
            }


            var splitLocationCountry = locationCountry.Split(Separator.ToCharArray());
            return res.Where(d => splitLocationCountry.Contains(checkoutLogic
                            ? d.PickupLocation.CMS_LOCATION_GROUP.CMS_POOL.country
                            : d.ReturnLocation.CMS_LOCATION_GROUP.CMS_POOL.country));
        }

        public static IQueryable<Reservation> RestrictByPool(IQueryable<Reservation> res, string pool, bool checkoutLogic)
        {
            if (!pool.Contains(Separator))
            {
                var poolId = int.Parse(pool);
                return checkoutLogic
                    ? res.Where(d => d.PickupLocation.CMS_LOCATION_GROUP.cms_pool_id == poolId)
                    : res.Where(d => d.ReturnLocation.CMS_LOCATION_GROUP.cms_pool_id == poolId);
            }

            var splitPools = pool.Split(Separator.ToCharArray()).Select(int.Parse);
            return res.Where(d => splitPools.Contains(checkoutLogic
                            ? d.PickupLocation.CMS_LOCATION_GROUP.cms_pool_id
                            : d.ReturnLocation.CMS_LOCATION_GROUP.cms_pool_id));
        }

        public static IQueryable<Reservation> RestrictByRegion(IQueryable<Reservation> res, string region, bool checkoutLogic)
        {
            if (!region.Contains(Separator))
            {
                var regionId = int.Parse(region);
                return checkoutLogic
                    ? res.Where(d => d.PickupLocation.OPS_AREA.ops_region_id == regionId)
                    : res.Where(d => d.ReturnLocation.OPS_AREA.ops_region_id == regionId);
            }

            var splitRegions = region.Split(Separator.ToCharArray()).Select(int.Parse);
            return res.Where(d => splitRegions.Contains(checkoutLogic
                            ? d.PickupLocation.OPS_AREA.ops_region_id
                            : d.ReturnLocation.OPS_AREA.ops_region_id));
        }

        public static IQueryable<Reservation> RestrictByLocationGroup(IQueryable<Reservation> res, string locationGroup, bool checkoutLogic)
        {
            if (!locationGroup.Contains(Separator))
            {
                var locationGroupId = int.Parse(locationGroup);
                return checkoutLogic
                    ? res.Where(d => d.PickupLocation.cms_location_group_id == locationGroupId)
                    : res.Where(d => d.ReturnLocation.cms_location_group_id == locationGroupId);
            }

            var splitLocationGroups = locationGroup.Split(Separator.ToCharArray()).Select(int.Parse);
            return res.Where(d => splitLocationGroups.Contains(checkoutLogic
                            ? d.PickupLocation.CMS_LOCATION_GROUP.cms_location_group_id
                            : d.ReturnLocation.CMS_LOCATION_GROUP.cms_location_group_id));
        }

        public static IQueryable<Reservation> RestrictByArea(IQueryable<Reservation> res, string area, bool checkoutLogic)
        {
            if (!area.Contains(Separator))
            {
                var areaId = int.Parse(area);
                return checkoutLogic
                    ? res.Where(d => d.PickupLocation.ops_area_id == areaId)
                    : res.Where(d => d.ReturnLocation.ops_area_id == areaId);
            }

            var splitAreas = area.Split(Separator.ToCharArray()).Select(int.Parse);
            return res.Where(d => splitAreas.Contains(checkoutLogic
                            ? d.PickupLocation.ops_area_id
                            : d.ReturnLocation.ops_area_id));
        }

        public static IQueryable<Reservation> RestrictByLocation(IQueryable<Reservation> res, string location, bool checkoutLogic)
        {
            if (!location.Contains(Separator))
            {
                var locationId = int.Parse(location);
                return checkoutLogic
                    ? res.Where(d => d.PickupLocation.dim_Location_id == locationId)
                    : res.Where(d => d.ReturnLocation.dim_Location_id == locationId);
            }

            var splitLocations = location.Split(Separator.ToCharArray()).Select(int.Parse);
            return res.Where(d => splitLocations.Contains(checkoutLogic
                            ? d.PickupLocation.dim_Location_id
                            : d.ReturnLocation.dim_Location_id));
        }

        public static IQueryable<Reservation> RestrictByCarSegment(IQueryable<Reservation> res, string carSegment, bool reservedLogic)
        {
            if (!carSegment.Contains(Separator))
            {
                var carSegmentId = int.Parse(carSegment);
                return res.Where(d => reservedLogic 
                                        ? d.ReservedCarGroup.CAR_CLASS.car_segment_id == carSegmentId
                                        : d.UpgradedCarGroup.CAR_CLASS.car_segment_id == carSegmentId);
            }

            var splitCarSegments = carSegment.Split(Separator.ToCharArray()).Select(int.Parse);
            return res.Where(d => splitCarSegments.Contains(reservedLogic
                                ? d.ReservedCarGroup.CAR_CLASS.car_segment_id
                                : d.UpgradedCarGroup.CAR_CLASS.car_segment_id));
        }

        public static IQueryable<Reservation> RestrictByCarClass(IQueryable<Reservation> res, string carClass, bool reservedLogic)
        {
            if (!carClass.Contains(Separator))
            {
                var carClassId = int.Parse(carClass);
                return res.Where(d => reservedLogic
                                        ? d.ReservedCarGroup.car_class_id == carClassId
                                        : d.UpgradedCarGroup.car_class_id == carClassId);
            }

            var splitCarClass = carClass.Split(Separator.ToCharArray()).Select(int.Parse);
            return res.Where(d => splitCarClass.Contains(reservedLogic
                                ? d.ReservedCarGroup.car_class_id
                                : d.UpgradedCarGroup.car_class_id));
        }

        public static IQueryable<Reservation> RestrictByCarGroup(IQueryable<Reservation> res, string carGroup, bool reservedLogic)
        {
            if (!carGroup.Contains(Separator))
            {
                var carGroupId = int.Parse(carGroup);
                return res.Where(d => reservedLogic
                                        ? d.ReservedCarGroup.car_group_id == carGroupId
                                        : d.UpgradedCarGroup.car_group_id == carGroupId);
            }

            var splitCarGroup = carGroup.Split(Separator.ToCharArray()).Select(int.Parse);
            return res.Where(d => splitCarGroup.Contains(reservedLogic
                                ? d.ReservedCarGroup.car_group_id
                                : d.UpgradedCarGroup.car_group_id));
        }
    }
}