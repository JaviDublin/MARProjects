using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic
{
    public static class PoolEntityCheck
    {
        internal const string PoolAlreadyExistsForCountry = "A Pool with this name already exists in this Country";

        internal static bool DoesPoolNameExistForCountry(MarsDBDataContext dataContext, string poolName
                                                        , int countryId, int poolId = 0)
        {
            var returned = dataContext.CMS_POOLs.Any(d => d.COUNTRy1.CountryId == countryId 
                                                    && d.cms_pool1 == poolName
                                                    && ((d.cms_pool_id != poolId) || poolId == 0));

            return returned;
        }
    }
}