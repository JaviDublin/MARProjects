using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using App.DAL.Data;
using App.DAL.MarsDataAccess.ParameterAccess.DataHolders;
using Mars.App.Classes.DAL.Data;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Entities.Graphing.Parameters;

namespace App.DAL.MarsDataAccess.ParameterAccess
{
    internal static class ParameterDataAccess
    {
        internal static List<ListItem> GetCountryParameterListItems(Dictionary<string, string> parameters)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from c in dataContext.COUNTRies
                           where c.active
                           orderby c.country1
                           select new ListItem(c.country_description, c.country1)).ToList();
                return res;
            }
        }

        internal static DateTime GetLastDateFromCmsForecast()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var returnedDate = from fc in dataContext.MARS_CMS_FORECASTs
                                   orderby fc.REP_DATE
                                   select fc.REP_DATE;

                return returnedDate.First();
            }
        }

        internal static DateTime GetLastDateFromViewNonRevLogStats()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var returnedDate = from fc in dataContext.Fact_NonRevLog_Stats
                                   orderby fc.Rep_Date
                                   select fc.Rep_Date;

                return returnedDate.First();
            }
        }

        internal static List<ListItem> GetOpsRegionListItems(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();
            if (string.IsNullOrEmpty(country)) return null;


            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from r in dataContext.OPS_REGIONs
                           where r.country.ToLower() == country
                           orderby r.ops_region1
                           select new ListItem(r.ops_region1, r.ops_region_id.ToString())).ToList();
                return res;
            }
        }

        internal static List<ListItem> GetOpsAreaListItems(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();
            var opsRegionId = int.Parse(parameters[ParameterNames.Region]);
            if (string.IsNullOrEmpty(country)) return null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from a in dataContext.OPS_AREAs
                           join r in dataContext.OPS_REGIONs on a.ops_region_id equals r.ops_region_id
                           where r.country.ToLower() == country && r.ops_region_id == opsRegionId
                           orderby a.ops_area1
                           select new ListItem(a.ops_area1, a.ops_area_id.ToString())).ToList();
                return res;
            }
        }

        internal static List<ListItem> GetPoolParameterListItems(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();
            if (string.IsNullOrEmpty(country)) return null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from p in dataContext.CMS_POOLs
                           where p.country.ToLower() == country
                           orderby p.cms_pool1
                           select new ListItem(p.cms_pool1, p.cms_pool_id.ToString())).ToList();
                return res;
            }
        }

        internal static List<ListItem> GetLocationGroupParameterListItems(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();
            var cmsPoolId = parameters[ParameterNames.Pool];
            if (string.IsNullOrEmpty(cmsPoolId)) return null;
            if (string.IsNullOrEmpty(country)) return null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from lg in dataContext.CMS_LOCATION_GROUPs
                           where lg.CMS_POOL.country.ToLower() == country
                                && lg.CMS_POOL.cms_pool_id == int.Parse(cmsPoolId)
                           orderby lg.cms_location_group1
                           select new ListItem(lg.cms_location_group1, lg.cms_location_group_id.ToString())).ToList();
                return res;
            }
        }


        internal static List<ListItem> GetAllBranches(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();

            var locationGroupId = parameters.ContainsKey(ParameterNames.LocationGroup) &&
                    parameters[ParameterNames.LocationGroup] != string.Empty
                        ? int.Parse(parameters[ParameterNames.LocationGroup]) : (int?)null;
            var opsAreadId = parameters.ContainsKey(ParameterNames.Area)
                        && parameters[ParameterNames.Area] != string.Empty
                        ? int.Parse(parameters[ParameterNames.Area]) : (int?)null;


            if (string.IsNullOrEmpty(country)) return null;
            if (locationGroupId == null && opsAreadId == null) return null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {

                var res = (from l in dataContext.LOCATIONs
                           where l.country.ToLower() == country
                                 && (locationGroupId != null
                                    ? l.cms_location_group_id == locationGroupId //CMS
                                    : l.ops_area_id == opsAreadId)               //Ops
                                 && l.active
                           orderby l.location_name
                           select new ListItem(l.location1 + " " + l.location_name, l.dim_Location_id.ToString())).ToList();



                return res;
            }
        }

        internal static List<ListItem> GetLocationGroupParameterListItemsAllCountry(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();
            if (string.IsNullOrEmpty(country)) return null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from lg in dataContext.CMS_LOCATION_GROUPs
                           where lg.CMS_POOL.country.ToLower() == country
                           orderby lg.cms_location_group1
                           select new ListItem(lg.cms_location_group1, lg.cms_location_group_id.ToString())).ToList();
                return res;
            }
        }

        internal static List<ListItem> GetCarSegmentParameterListItems(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();
            if (string.IsNullOrEmpty(country)) return null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from cs in dataContext.CAR_SEGMENTs
                           where cs.country.ToLower() == country
                           orderby cs.sort_car_segment
                           select new ListItem(cs.car_segment1, cs.car_segment_id.ToString())).ToList();
                return res;
            }
        }

        public static List<ListItem> GetCarClassParameterListItemsAllCountry(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();

            if (string.IsNullOrEmpty(country)) return null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from cg in dataContext.CAR_GROUPs
                           where cg.CAR_CLASS.CAR_SEGMENT.country.ToLower() == country
                                && cg.CAR_CLASS.CAR_SEGMENT.car_segment_id == cg.CAR_CLASS.car_segment_id
                           orderby cg.car_group1
                           select new ListItem(cg.car_group1, cg.car_group_id.ToString())).ToList();
                return res;
            }
        }
        internal static List<ListItem> GetCarClassGroupParameterListItems(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();
            var carSegmentId = parameters[ParameterNames.CarSegment];

            if (string.IsNullOrEmpty(country)) return null;
            if (string.IsNullOrEmpty(carSegmentId)) return null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from cc in dataContext.CAR_CLASSes
                           where cc.CAR_SEGMENT.car_segment_id == int.Parse(carSegmentId)
                                && cc.CAR_SEGMENT.country.ToLower() == country.ToLower()
                           orderby cc.sort_car_class
                           select new ListItem(cc.car_class1, cc.car_class_id.ToString())).ToList();
                return res;
            }
        }

        internal static List<ListItem> GetCarClassParameterListItems(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();
            var carSegmentId = parameters[ParameterNames.CarSegment].ToLower();
            var carClassGroupId = parameters[ParameterNames.CarClassGroup].ToLower();

            if (string.IsNullOrEmpty(country)) return null;
            if (string.IsNullOrEmpty(carSegmentId)) return null;
            if (string.IsNullOrEmpty(carClassGroupId)) return null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from cg in dataContext.CAR_GROUPs
                           where cg.CAR_CLASS.CAR_SEGMENT.country.ToLower() == country
                                && cg.CAR_CLASS.CAR_SEGMENT.car_segment_id == int.Parse(carSegmentId)
                                && cg.CAR_CLASS.car_class_id == int.Parse(carClassGroupId)
                           orderby cg.sort_car_group
                           select new ListItem(cg.car_group1, cg.car_group_id.ToString())).ToList();
                return res;
            }
        }

        internal static List<ListItem> GetComparisonTopics()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from ct in dataContext.ComparisonTopics
                           orderby ct.ComparisonTopicId
                           select new ListItem(ct.ComparisonName, ct.ComparisonTopicId.ToString())).ToList();
                return res;
            }
        }

        internal static List<ListItem> GetKpiCalculationTypes()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from kct in dataContext.KpiCalculationTypes
                           orderby kct.KpiCalculationTypeId
                           select new ListItem(kct.CalculationName, kct.KpiCalculationTypeId.ToString())).ToList();
                return res;
            }
        }

        internal static List<ListItem> GetCarClassParameterListItemsViaSproc(Dictionary<string, string> parameters)
        {
            var country = parameters[ParameterNames.Country].ToLower();
            var carSegmentId = parameters[ParameterNames.CarSegment].ToLower();
            var carClassGroupId = parameters[ParameterNames.CarClassGroup].ToLower();

            var con = DBManager.CreateConnection();
            var cmd = DBManager.CreateProcedure(StoredProcedures.CarGroupsSelect, con);

            cmd.Parameters.Add(Parameters.Country, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.Country].Value = country;
            cmd.Parameters[Parameters.Country].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.CarSegmentId2, SqlDbType.Int, 1);
            cmd.Parameters[Parameters.CarSegmentId2].Value = carSegmentId;
            cmd.Parameters[Parameters.CarSegmentId2].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.CarClass2, SqlDbType.Int, 1);
            cmd.Parameters[Parameters.CarClass2].Value = carClassGroupId;
            cmd.Parameters[Parameters.CarClass2].Direction = ParameterDirection.Input;

            var retList = new List<ListItem>();
            using (con)
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var li = new ListItem();


                    if (reader["car_group_id"] != DBNull.Value)
                        li.Value = reader["car_group_id"].ToString();
                    if (reader["car_group"] != DBNull.Value)
                        li.Text = reader["car_group"].ToString();

                    retList.Add(li);
                }
            }
            return retList;
        }

        internal static List<LocationGroupHolder> GetAllLocationPools()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from lg in dataContext.CMS_LOCATION_GROUPs
                           select
                               new LocationGroupHolder
                                   {
                                       LocationGroupId = lg.cms_location_group_id,
                                       LocationGroupName = lg.cms_location_group1,
                                       PoolId = lg.cms_pool_id,
                                       PoolName = lg.CMS_POOL.cms_pool1,
                                       Country = lg.CMS_POOL.COUNTRy1.country1
                                   }).ToList();
                return res;
            }
        }

        internal static List<BranchHolder> GetAllBranches()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var res = (from l in dataContext.LOCATIONs
                           join lg in dataContext.CMS_LOCATION_GROUPs on l.cms_location_group_id equals lg.cms_location_group_id
                           join a in dataContext.OPS_AREAs on l.ops_area_id equals a.ops_area_id
                           where l.active && l.COUNTRy1.active
                           select
                               new BranchHolder
                               {
                                   LocationGroupId = l.cms_location_group_id ?? 0,
                                   LocationGroupName = lg.cms_location_group1,
                                   PoolId = lg.cms_pool_id,
                                   PoolName = lg.CMS_POOL.cms_pool1,
                                   Country = lg.CMS_POOL.COUNTRy1.country1,
                                   AreaId = l.ops_area_id,
                                   AreaName = a.ops_area1,
                                   RegionId = a.OPS_REGION.ops_region_id,
                                   RegionName = a.OPS_REGION.ops_region1,
                                   BranchCode = l.location1,
                                   BranchId = l.dim_Location_id
                               }).ToList();
                return res;
            }
        }

        internal static List<CarGroupHolder> GetCarGroups()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var results = from cg in dataContext.CAR_GROUPs
                              join cc in dataContext.CAR_CLASSes on cg.car_class_id equals cc.car_class_id
                              join cs in dataContext.CAR_SEGMENTs on cc.car_segment_id equals cs.car_segment_id
                              select new CarGroupHolder
                                     {
                                         CarGroup = cg.car_group1,
                                         Country = cs.country
                                     };
                return results.ToList();
            }
        }


        internal static List<CmsPoolHolder> GetAllCmsPools()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var cmsPools = (from c in dataContext.CMS_POOLs
                                select new CmsPoolHolder
                                       {
                                           CmsPoolId = c.cms_pool_id,
                                           CmsPool = c.cms_pool1,
                                           Country = c.country
                                       }).ToList();
                return cmsPools;
            }
        }


        internal static List<CmsLocationGroupHolder> GetAllCmsLocationGroups()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var cmsPools = (from c in dataContext.CMS_LOCATION_GROUPs
                                select new CmsLocationGroupHolder
                                {
                                    CmsLocGroupId = c.cms_location_group_id,
                                    CmsLocGroup = c.cms_location_group1
                                }).ToList();
                return cmsPools;
            }
        }

        internal static List<CmsLocationGroupHolder> GetAllOpsRegions()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var opsRegions = (from c in dataContext.OPS_REGIONs
                    select new CmsLocationGroupHolder
                           {
                               CmsLocGroupId = c.ops_region_id,
                               CmsLocGroup = c.ops_region1
                           }).ToList();
                return opsRegions;
            }
        }


        internal static List<CmsLocationGroupHolder> GetAllCarSegments()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var carSegments = (from c in dataContext.CAR_SEGMENTs
                                select new CmsLocationGroupHolder
                                {
                                    CmsLocGroupId = c.car_segment_id,
                                    CmsLocGroup = c.car_segment1
                                }).ToList();
                return carSegments;
            }
        }


        internal static List<CmsLocationGroupHolder> GetAllCarGroup()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var carSegments = (from c in dataContext.CAR_GROUPs
                                   select new CmsLocationGroupHolder
                                   {
                                       CmsLocGroupId = c.car_group_id,
                                       CmsLocGroup = c.car_group1
                                   }).ToList();
                return carSegments;
            }
        }

        internal static List<CountryHolder> GetAllCountries()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var countries = (from c in dataContext.COUNTRies
                                   select new CountryHolder()
                                   {
                                       CountryCode = c.country1,
                                       CountryDesc = c.country_description
                                   }).ToList();
                return countries;
            }
        }
    }
}