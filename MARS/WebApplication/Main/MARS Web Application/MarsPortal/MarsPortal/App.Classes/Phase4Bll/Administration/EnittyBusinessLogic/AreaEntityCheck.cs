using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic
{
    public static class AreaEntityCheck
    {
        internal const string AreaAlreadyExistsForCountry = "An Area with this name already exists in this Country";

        internal static bool DoesAreaNameExistForRegion(MarsDBDataContext dataContext, string areaName, int regionId, int areaId = 0)
        {
            var countryId = dataContext.OPS_REGIONs.Single(d => d.ops_region_id == regionId).COUNTRy1.CountryId;
            var returned = dataContext.OPS_AREAs.Any(d => d.OPS_REGION.COUNTRy1.CountryId == countryId
                                                                        && d.ops_area1 == areaName
                                                                        && ((d.ops_area_id != areaId) || areaId == 0));
            return returned;
        }

    }
}