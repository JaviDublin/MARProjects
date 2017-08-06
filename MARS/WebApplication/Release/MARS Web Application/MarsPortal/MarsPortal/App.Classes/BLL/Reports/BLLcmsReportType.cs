using System.Collections.Generic;
using App.BLL.Utilities;
using App.DAL.CMSReportType;
using App.Entities;

namespace App.BLL.CMSReportType
{
    public class BLLcmsReportType
    {
        readonly DALcmsReportType _dal = new DALcmsReportType();

        public List<CMSFleetPlan> CMSFleetPlanGetAll(bool hideScenarios)
        {
           string cacheKey = hideScenarios ? MarsV2Cache.CMSFrozenZoneFleetPlanList : MarsV2Cache.CMSFleetPlanList;
           object cacheItem = MarsV2Cache.GetCacheObject(cacheKey);
           
           if((ConfigAccess.ByPassCache()) || (cacheItem == null))
           {
               cacheItem = _dal.CMSFleetPlanGetAll(hideScenarios);
              MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
           }

           return (List<CMSFleetPlan>)cacheItem;
        }

        public List<CMSForecastType> CMSForecastTypeGetAll(bool frozenZoneSelected)
        {
            string cacheKey = frozenZoneSelected ? MarsV2Cache.CMSForecastTypeWithAlreadyBooked : MarsV2Cache.CMSForecastTypeWithoutAlreadyBooked;
            object cacheItem = MarsV2Cache.GetCacheObject(cacheKey);
            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                cacheItem = _dal.CMSForecastTypeGetAll(frozenZoneSelected);
                MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
            }

            return (List<CMSForecastType>)cacheItem;
        }

        public List<CMSReportingTimeZone> CMSReportingTimeZoneGetAll()
        {
            string cacheKey = MarsV2Cache.CMSReportingTimeZone;
            object cacheItem = MarsV2Cache.GetCacheObject(cacheKey);
            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                cacheItem = _dal.CMSReportingTimeZoneGetAll();
                MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
            }

            return (List<CMSReportingTimeZone>)cacheItem;
        }
    }
}