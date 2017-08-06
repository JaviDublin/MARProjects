using System;
using System.Collections.Generic;
using App.BLL.Utilities;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.ParameterAccess.DataHolders;
using Mars.App.Classes.DAL.Data;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Parameters;

namespace App.BLL.Cache
{
    internal static class ParameterCache
    {
        /// <summary>
        /// Retrieves the List of Location Groups either from the Cache or Database
        /// </summary>
        /// <returns>Location Pool Ids and Names</returns>
        internal static List<LocationGroupHolder> GetAllLocationGroups()
        {
            var cacheKey = MarsV2Cache.MarsLocationGroupList;
            var cacheItem = MarsV2Cache.GetCacheObject(cacheKey);

            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                cacheItem = ParameterDataAccess.GetAllLocationPools();
                MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
            }

            return (List<LocationGroupHolder>)cacheItem;
        }

        internal static List<BranchHolder> GetAllBranches()
        {
            var cacheKey = MarsV2Cache.MarsBranchList;
            var cacheItem = MarsV2Cache.GetCacheObject(cacheKey);

            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                cacheItem = ParameterDataAccess.GetAllBranches();
                MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
            }

            return (List<BranchHolder>)cacheItem;
        }

        internal static List<string> GetAllLicencePlates()
        {
            var cacheKey = MarsV2Cache.LicencePlate;
            var cacheItem = MarsV2Cache.GetCacheObject(cacheKey);

            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                using (var dataAccess = new MarsDBDataContext())
                {
                    cacheItem = NonRevParameterDataAccess.GetLicecePlates(dataAccess);
                }
                MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
            }

            return (List<string>)cacheItem;
        }

        internal static List<string> GetVehicleAutoComplete(AutoCompleteTypes typeOfAutoComplete, string cacheKey)
        {
            var cacheItem = MarsV2Cache.GetCacheObject(cacheKey);
            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                using (var dataAccess = new MarsDBDataContext())
                {
                    switch (typeOfAutoComplete)
                    {
                        case AutoCompleteTypes.Vin:
                            cacheItem = NonRevParameterDataAccess.GetVins(dataAccess);
                            break;
                        case AutoCompleteTypes.LicencePlate:
                            cacheItem = NonRevParameterDataAccess.GetLicecePlates(dataAccess);
                            break;
                        case AutoCompleteTypes.UnitNumber:
                            cacheItem = NonRevParameterDataAccess.GetUnitNumbers(dataAccess);
                            break;
                        case AutoCompleteTypes.DriverName:
                            cacheItem = NonRevParameterDataAccess.GetDriverNames(dataAccess);
                            break;
                        case AutoCompleteTypes.Colour:
                            cacheItem = NonRevParameterDataAccess.GetVehicleColours(dataAccess);
                            break;
                        case AutoCompleteTypes.ModelDescription:
                            cacheItem = NonRevParameterDataAccess.GetModelDescription(dataAccess);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("typeOfAutoComplete");
                    }
                }
            }
            return (List<string>)cacheItem;
        }
    }
}