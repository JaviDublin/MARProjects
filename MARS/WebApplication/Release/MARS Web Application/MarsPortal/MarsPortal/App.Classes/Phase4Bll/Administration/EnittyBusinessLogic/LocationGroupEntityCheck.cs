using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic
{
    public static class LocationGroupEntityCheck
    {
        internal const string LocationGroupAlreadyExistsForCountry = "A Location Group with this name already exists in this Country";

        internal static bool DoesLocationGroupNameExistForCountry(MarsDBDataContext dataContext, string locationGroupName
                                                                                , int poolId, int locationGroupId = 0)
        {
            var countryId = dataContext.CMS_POOLs.Single(d => d.cms_pool_id == poolId).COUNTRy1.CountryId;
            var returned = dataContext.CMS_LOCATION_GROUPs.Any(d => d.CMS_POOL.COUNTRy1.CountryId == countryId
                                                                        && d.cms_location_group1 == locationGroupName
                                                                        && ((d.cms_location_group_id != locationGroupId) || locationGroupId == 0));
            return returned;
        }

    }
}