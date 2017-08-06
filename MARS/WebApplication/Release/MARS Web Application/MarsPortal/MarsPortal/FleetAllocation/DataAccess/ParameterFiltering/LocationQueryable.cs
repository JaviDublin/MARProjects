using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.ParameterFiltering
{
    public static class LocationQueryable
    {
        public static string Separator = Properties.Settings.Default.Seperator;

        public static IQueryable<LOCATION> GetLocations(FaoDataContext dataContext
                        , Dictionary<DictionaryParameter, string> parameters)
        {
            var locations = from l in dataContext.LOCATIONs
                            select l;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var locationString = parameters[DictionaryParameter.Location];

                if (!locationString.Contains(Separator))
                {
                    var locationId = int.Parse(locationString);
                    locations = locations.Where(d => d.dim_Location_id == locationId);
                }
                else
                {
                    var splitLocationIds = locationString.Split(Separator.ToCharArray()).Select(int.Parse);
                    locations = from l in locations
                                where splitLocationIds.Contains(l.dim_Location_id)
                                select l;
                }

            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
            {
                var locationGroupString = parameters[DictionaryParameter.LocationGroup];
                if (!locationGroupString.Contains(Separator))
                {
                    var locationGroupId = int.Parse(locationGroupString);
                    locations = locations.Where(d => d.cms_location_group_id == locationGroupId);
                }
                else
                {
                    var splitLocationGroupIds = locationGroupString.Split(Separator.ToCharArray()).Select(int.Parse);
                    locations = from l in locations
                        where l.cms_location_group_id != null && splitLocationGroupIds.Contains(l.cms_location_group_id.Value)
                                select l;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {

                var poolsString = parameters[DictionaryParameter.Pool];
                if (!poolsString.Contains(Separator))
                {
                    var poolId = int.Parse(poolsString);
                    locations = locations.Where(d => d.CMS_LOCATION_GROUP.cms_pool_id == poolId);
                }
                else
                {
                    var splitPoolIds = poolsString.Split(Separator.ToCharArray()).Select(int.Parse);
                    locations = from l in locations
                                where l.cms_location_group_id != null && splitPoolIds.Contains(l.CMS_LOCATION_GROUP.cms_pool_id)
                                select l;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountryString = parameters[DictionaryParameter.LocationCountry];
                if (!locationCountryString.Contains(Separator))
                {
                    var locationCountryId = locationCountryString;
                    locations = locations.Where(d => d.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountryId);
                }
                else
                {
                    var splitLocationCountryIds = locationCountryString.Split(Separator.ToCharArray());
                    locations = from l in locations
                                where l.cms_location_group_id != null && splitLocationCountryIds.Contains(l.CMS_LOCATION_GROUP.CMS_POOL.country)
                                select l;
                }
            }
            return locations;
        }

    }
}