using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal.Administration
{
    public static class AdminParameterDataAccess
    {
        private static List<ListItem> GetEmptyParameterListItem()
        {
            var emptyList = new List<ListItem>();
            emptyList.Insert(0, ParameterDataAccess.EmptyItem);
            return emptyList;
        }

        internal static List<ListItem> GetAllCountryListItems(MarsDBDataContext dataContext, string activeOnly = null, bool includeAllListItem = true)
        {
            var countryEntities = dataContext.COUNTRies.Select(d => d);
            if (activeOnly == true.ToString())
            {
                countryEntities = countryEntities.Where(d => d.active);
            }

            var countries = from c in countryEntities
                            orderby c.country_description
                            select new ListItem
                            {
                                Text = c.country_description,
                                Value = c.CountryId.ToString()
                            };

            var returned = countries.ToList();

            if (includeAllListItem)
            {
                returned.Insert(0, ParameterDataAccess.EmptyItem);
            }

            return returned;
        }

        internal static List<ListItem> GetAdminPoolListItems(Dictionary<DictionaryParameter, string> parameters
                        , MarsDBDataContext dataContext, string activeOnly, bool includeAllListItem = true)
        {
            var poolData = from p in dataContext.CMS_POOLs

                           select p;
            
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                int countryId = int.Parse(parameters[DictionaryParameter.LocationCountry]);
                poolData = poolData.Where(d => d.COUNTRy1.CountryId == countryId);
            }
            else
            {
                return GetEmptyParameterListItem();
            }

            var returnedData = from p in poolData
                               orderby p.cms_pool1
                               select new ListItem(p.cms_pool1, p.cms_pool_id.ToString());

            var returned = returnedData.ToList();
            if (includeAllListItem)
            {
                returned.Insert(0, ParameterDataAccess.EmptyItem);
            }
            return returned;
        }


        internal static List<ListItem> GetAdminLocationGroupListItems(Dictionary<DictionaryParameter, string> parameters
                        , MarsDBDataContext dataContext, string activeOnly, bool includeAllListItem = true)
        {
            var locationGroupData = from lg in dataContext.CMS_LOCATION_GROUPs
                           select lg;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {
                int poolId = int.Parse(parameters[DictionaryParameter.Pool]);
                locationGroupData = locationGroupData.Where(d => d.cms_pool_id == poolId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                int countryId = int.Parse(parameters[DictionaryParameter.LocationCountry]);
                locationGroupData = locationGroupData.Where(d => d.CMS_POOL.COUNTRy1.CountryId == countryId);
            }
            else
            {
                return GetEmptyParameterListItem();
            }

            var returnedData = from p in locationGroupData
                               orderby p.cms_location_group1
                               select new ListItem(p.cms_location_group1, p.cms_location_group_id.ToString());

            var returned = returnedData.ToList();
            if (includeAllListItem)
            {
                returned.Insert(0, ParameterDataAccess.EmptyItem);
            }
            return returned;
        }

        internal static List<ListItem> GetAdminRegionListItems(Dictionary<DictionaryParameter, string> parameters
                , MarsDBDataContext dataContext, string activeOnly, bool includeAllListItem = true)
        {
            var regionData = from r in dataContext.OPS_REGIONs
                             select r;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                int countryId = int.Parse(parameters[DictionaryParameter.LocationCountry]);
                regionData = regionData.Where(d => d.COUNTRy1.CountryId == countryId);
            }
            else
            {
                return GetEmptyParameterListItem();
            }

            var returnedData = from p in regionData
                               orderby p.ops_region1
                               select new ListItem(p.ops_region1, p.ops_region_id.ToString());

            var returned = returnedData.ToList();
            if (includeAllListItem)
            {
                returned.Insert(0, ParameterDataAccess.EmptyItem);
            }
            return returned;
        }

        internal static List<ListItem> GetAdminAreaListItems(Dictionary<DictionaryParameter, string> parameters
                , MarsDBDataContext dataContext, string activeOnly, bool includeAllListItem = true)
        {
            var areaData = from r in dataContext.OPS_AREAs
                             select r;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
            {
                int regionId = int.Parse(parameters[DictionaryParameter.Region]);
                areaData = areaData.Where(d => d.ops_region_id == regionId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                int countryId = int.Parse(parameters[DictionaryParameter.LocationCountry]);
                areaData = areaData.Where(d => d.OPS_REGION.COUNTRy1.CountryId == countryId);
            }
            else
            {
                return GetEmptyParameterListItem();
            }

            var returnedData = from p in areaData
                               orderby p.ops_area1
                               select new ListItem(p.ops_area1, p.ops_area_id.ToString());

            var returned = returnedData.ToList();
            if (includeAllListItem)
            {
                returned.Insert(0, ParameterDataAccess.EmptyItem);
            }
            return returned;
        }

        internal static List<ListItem> GetAdminCarSegmentListItems(Dictionary<DictionaryParameter, string> parameters
                        , MarsDBDataContext dataContext, string activeOnly, bool includeAllListItem = true)
        {

            var segmentData = from cs in dataContext.CAR_SEGMENTs
                              select cs;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                int countryId = int.Parse(parameters[DictionaryParameter.LocationCountry]);
                segmentData = segmentData.Where(d => d.COUNTRy1.CountryId == countryId);
            }
            else
            {
                return GetEmptyParameterListItem();
            }

            var returnedData = from cs in segmentData
                               orderby cs.car_segment1
                               select new ListItem(cs.car_segment1, cs.car_segment_id.ToString());

            var returned = returnedData.ToList();
            if (includeAllListItem)
            {
                returned.Insert(0, ParameterDataAccess.EmptyItem);    
            }
            
            return returned;
        }

        internal static List<ListItem> GetAdminCarClassListItems(Dictionary<DictionaryParameter, string> parameters
                        , MarsDBDataContext dataContext, string activeOnly, bool includeAllListItem = true)
        {

            var classData = from cc in dataContext.CAR_CLASSes
                              select cc;

            
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                int segmentId = int.Parse(parameters[DictionaryParameter.CarSegment]);
                classData = classData.Where(d => d.car_segment_id == segmentId);
            }
            else
            {
                return GetEmptyParameterListItem();
            }


            var returnedData = from cs in classData
                               orderby cs.car_class1
                               select new ListItem(cs.car_class1, cs.car_class_id.ToString());

            var returned = returnedData.ToList();
            if (includeAllListItem)
            {
                returned.Insert(0, ParameterDataAccess.EmptyItem);
            }
            return returned;
        }

        internal static List<ListItem> GetAdminCarGroupsWithinSegmentListItems(Dictionary<DictionaryParameter, string> parameters
                        , MarsDBDataContext dataContext, string activeOnly, bool includeAllListItem = true)
        {
            var groupData = from cc in dataContext.CAR_GROUPs
                            select cc;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                int segmentId = int.Parse(parameters[DictionaryParameter.CarSegment]);
                groupData = groupData.Where(d => d.CAR_CLASS.car_segment_id == segmentId);
            }
            else
            {
                return GetEmptyParameterListItem();
            }

            var returnedData = from cg in groupData
                               orderby cg.car_group1
                               select new ListItem(cg.car_group1, cg.car_group_id.ToString());

            var returned = returnedData.ToList();
            if (includeAllListItem)
            {
                returned.Insert(0, ParameterDataAccess.EmptyItem);
            }
            return returned;
        }



    }
}