using System.Collections.Generic;
using App.DAL.ReportParameters;
using App.Entities;
using App.BLL.Utilities;

namespace App.BLL.ReportParameters
{
    public class BLLReportParameters
    {
        private DALReportParameters DAL = new DALReportParameters();

        public List<Country> CountryGetAll()
        {

            string cacheKey = MarsV2Cache.CountryList;
            object cacheItem = MarsV2Cache.GetCacheObject(cacheKey);

            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                cacheItem = DAL.CountryGetAll();
                MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
            }
            return new List<Country>((List<Country>)cacheItem);
           
        }

        public List<LocationGroup> LocationGroupGetByCountryID(string countryID)
        {
            string cacheKey = "LocationGroupAll_" + countryID;
            object cacheItem = MarsV2Cache.GetCacheObject(cacheKey);

            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                cacheItem = DAL.LocationGroupGetByCountryID(countryID);
                MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
            }
            return new List<LocationGroup>((List<LocationGroup>)cacheItem);
        }

        public List<CarGroup> CarGroupListGetByCountryID(string countryID)
        {
            string cacheKey = "CarGroupAll_" + countryID;
            object cacheItem = MarsV2Cache.GetCacheObject(cacheKey);

            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                cacheItem = DAL.CarGroupListGetByCountryID(countryID);
                MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
            }
            return new List<CarGroup>((List<CarGroup>)cacheItem);
        }
        
        public List<CarClass> CarClassGetByCountryID(string countryID)
        {
            List<CarClass> carClassList = DAL.CarClassGetByCountryID(countryID);
            return carClassList;
        }
    }
}