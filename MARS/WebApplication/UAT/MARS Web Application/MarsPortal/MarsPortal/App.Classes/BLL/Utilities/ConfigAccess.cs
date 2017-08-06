using System;
using System.Configuration;

namespace App.BLL.Utilities
{
    public static class ConfigAccess
    {
        public static double GetCacheMinutesFromConfig()
        {
            return Convert.ToDouble(ConfigurationManager.AppSettings["CacheTimeout"]);
        }

        public static bool RestrictedUserAccess()
        {
            bool attemptedParse;
            var succeeded = bool.TryParse(ConfigurationManager.AppSettings["FullUserAccess"], out attemptedParse);
            var restrictedAccess = succeeded && !attemptedParse;
            return restrictedAccess;
        }

        public static bool ByPassCache()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["BypassCache"]);
        }

        public static string GetSSISPackagePath()
        {
            return ConfigurationManager.AppSettings["SSISpackagePath"];
        }

        public static string GetSSISPackageVariable()
        {
            return ConfigurationManager.AppSettings["SSISpackageVariable"];
        }

        public static string GetFleetPlanUploadLocation()
        {
            return ConfigurationManager.AppSettings["FleetPlanUploadLocation"];
        }
    }
}