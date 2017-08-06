using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic
{
    public class RegionEntityCheck
    {
        internal const string RegionAlreadyExistsForCountry = "A Region with this name already exists in this Country";

        internal static bool DoesRegionNameExistForCountry(MarsDBDataContext dataContext, string regionName
                                , int countryId, int regionId = 0)
        {
            var returned = dataContext.OPS_REGIONs.Any(d => d.COUNTRy1.CountryId == countryId
                                            && d.ops_region1 == regionName
                                            && ((d.ops_region_id != regionId) || regionId == 0 ));
            
            return returned;
        }
    }
}