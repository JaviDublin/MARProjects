using System;
using System.Web;

namespace App.BLL.Utilities
{
    static class MarsV2Cache
    {
        public static readonly string CMSFleetPlanList = "CMSFleetPlanList";
        public static readonly string CMSFrozenZoneFleetPlanList = "CMSFrozenZoneFleetPlanList";
        public static readonly string CMSForecastTypeWithAlreadyBooked = "CMSForecastTypeWithAlreadyBooked";
        public static readonly string CMSForecastTypeWithoutAlreadyBooked = "CMSForecastTypeWithoutAlreadyBooked";
        public static readonly string CMSReportingTimeZone = "CMSReportingTimeZone";
        public static readonly string MarsLocationGroupList = "MarsLocationGroupList";
        public static readonly string MarsBranchList = "MarsBranchList";
        public static readonly string CountryList = "CountryList";

        public static readonly string LicencePlate = "LicencePlate";
        public static readonly string Vin = "Vin";
        public static readonly string UnitNumber = "UnitNumber";
        public static readonly string DriverName = "DriverName";
        public static readonly string VehicleColour = "VehicleColour";
        public static readonly string ModelDescription = "ModelDescription";

        public static object GetCacheObject(string cacheKey)
        {
            return HttpRuntime.Cache[cacheKey.ToLower()];
        }

        public static void AddObjectToCache(string cacheKey, object cacheItem)
        {
            HttpRuntime.Cache.Insert(cacheKey.ToLower(), cacheItem, null, DateTime.MaxValue, TimeSpan.FromMinutes(
                            ConfigAccess.GetCacheMinutesFromConfig()));
        }

        public static void AddObjectToCacheWithNoSlidingExpiry(string cacheKey, object cacheItem)
        {
            HttpRuntime.Cache.Insert(cacheKey.ToLower(), cacheItem, null,
                            DateTime.Now.AddMinutes(ConfigAccess.GetCacheMinutesFromConfig()), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public static void RemoveObjectFromCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey.ToLower());
        }
    }
}