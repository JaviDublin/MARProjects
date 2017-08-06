using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Expressions;
using App.Properties;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal
{
    internal static class ParameterDataAccess
    {
        internal static ListItem EmptyItem = new ListItem("***All***", string.Empty);
        public static string Separator = Properties.Settings.Default.Seperator;

        

        internal static List<ListItem> GetOwningCountryListItems(MarsDBDataContext dataContext)
        {
            var countries = from v in dataContext.Vehicles
                            where v.IsFleet
                join c in dataContext.COUNTRies on v.OwningCountry equals c.country1
                
                select new ListItem(c.country_description, v.OwningCountry);

            var returned = countries.Distinct().ToList().OrderBy(d => d.Text).ToList();
            
            return returned;
   
            //var returned = (from c in dataContext.COUNTRies
            //            where c.active
            //            orderby c.country1
                        
            //            select new ListItem(c.country_description, c.country1)).ToList();
            
            //return returned;   
        }

        internal static List<ListItem> GetReservationOutCountryListItems(MarsDBDataContext dataContext)
        {

            var returned = (from r in dataContext.Reservations1.Select(d=> d.Country).Distinct()
                            join cntry in dataContext.COUNTRies on r equals cntry.country1
                            orderby cntry.country_description

                            select new ListItem(cntry.country_description, r)).ToList();

            return returned;
        }

        internal static List<ListItem> GetLocationCountryListItems(MarsDBDataContext dataContext)
        {

            var returned = (from v in dataContext.Vehicles
                            group v by v.LastLocationCode.Substring(0, 2)
                            into groupedData
                            join c in  dataContext.COUNTRies on groupedData.Key equals c.country1
                            orderby c.country1
                            select new ListItem(c.country_description, c.country1)).ToList();

            return returned;
        }


        internal static List<ListItem> GetRegionListItems(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dataContext
                            , bool checkOut = false)
        {
            var locationCountry = parameters[checkOut ? DictionaryParameter.OwningCountry : DictionaryParameter.LocationCountry].ToLower();

            if (string.IsNullOrEmpty(locationCountry))
            {
                return new List<ListItem>();
            }
            List<ListItem> returned;
            if (locationCountry.Contains(Separator))
            {
                var splitCountries = locationCountry.Split(Separator.ToCharArray());
                returned = (from a in dataContext.OPS_REGIONs

                            where splitCountries.Contains(a.country.ToLower())
                            orderby a.country, a.ops_region1
                            select new ListItem(a.country + "-" + a.ops_region1, a.ops_region_id.ToString())).ToList();
            }
            else
            {
                returned = (from r in dataContext.OPS_REGIONs
                        where r.country.ToLower() == locationCountry
                        orderby r.ops_region1
                        select new ListItem(r.ops_region1, r.ops_region_id.ToString())).ToList();
            }
            
            
            return returned;
        }

        internal static List<ListItem> GetAreaListItems(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dataContext
            , bool checkOut = false)
        {
            var opsRegion = parameters[checkOut ? DictionaryParameter.CheckOutRegion : DictionaryParameter.Region];

            if (string.IsNullOrEmpty(opsRegion))
            {
                return new List<ListItem>();
            }

            List<ListItem> returned;
            if (opsRegion.Contains(Separator))
            {
                var splitRegions = opsRegion.Split(Separator.ToCharArray()).Select(int.Parse).ToList();
                returned = (from a in dataContext.OPS_AREAs
                            where splitRegions.Contains(a.ops_region_id)
                            orderby a.OPS_REGION.country, a.ops_area1
                            select new ListItem(a.OPS_REGION.country + "-" + a.OPS_REGION.ops_region1 + "-" + a.ops_area1, a.ops_area_id.ToString())).ToList();
            }
            else
            {
                returned = (from a in dataContext.OPS_AREAs
                            where a.OPS_REGION.ops_region_id == int.Parse(opsRegion)
                            orderby a.ops_area1
                            select new ListItem(a.ops_area1, a.ops_area_id.ToString())).ToList();
            }


            return returned;
            
        }

        

        internal static List<ListItem> GetPoolListItems(Dictionary<DictionaryParameter, string> parameters
                        , MarsDBDataContext dataContext
                        , bool checkOut = false)
        {
            List<ListItem> returned;
            var locationCountry = parameters[checkOut ? DictionaryParameter.CheckOutCountry : DictionaryParameter.LocationCountry].ToLower();
            if (string.IsNullOrEmpty(locationCountry))
            {
                return new List<ListItem>();
            }

            if (locationCountry.Contains(Separator))
            {
                var splitCountries = locationCountry.Split(Separator.ToCharArray());
                returned = (from p in dataContext.CMS_POOLs
                            
                            where splitCountries.Contains(p.country.ToLower())
                            orderby p.country, p.cms_pool1
                            select new ListItem(p.country + "-" + p.cms_pool1, p.cms_pool_id.ToString())).ToList();
            }
            else
            {
                returned = (from p in dataContext.CMS_POOLs
                            where p.country.ToLower() == locationCountry
                            orderby p.cms_pool1
                            select new ListItem(p.cms_pool1, p.cms_pool_id.ToString())).ToList();
            }


            return returned;
        }

        internal static List<ListItem> GetLocationGroupListItems(Dictionary<DictionaryParameter, string> parameters
                    , MarsDBDataContext dataContext, bool checkOut = false)
        {
            List<ListItem> returned;
            var cmsPoolId = parameters[checkOut ? DictionaryParameter.CheckOutPool : DictionaryParameter.Pool];
            if (string.IsNullOrEmpty(cmsPoolId))
            {
                return new List<ListItem>();
            }
            if (cmsPoolId.Contains(Separator))
            {
                var splitPools = cmsPoolId.Split(Separator.ToCharArray()).Select(int.Parse).ToList();

                returned = (from lg in dataContext.CMS_LOCATION_GROUPs
                            where splitPools.Contains(lg.CMS_POOL.cms_pool_id)
                            orderby lg.CMS_POOL.cms_pool1, lg.cms_location_group1
                            select new ListItem(lg.CMS_POOL.country  + "-" + lg.CMS_POOL.cms_pool1 
                                                + "-" + lg.cms_location_group1
                                , lg.cms_location_group_id.ToString())).ToList();
            }
            else
            {
                returned = (from lg in dataContext.CMS_LOCATION_GROUPs
                        where lg.CMS_POOL.cms_pool_id == int.Parse(cmsPoolId)
                        orderby lg.cms_location_group1
                        select new ListItem(lg.cms_location_group1, lg.cms_location_group_id.ToString())).ToList();
            }

            return returned;
            
        }


        internal static List<ListItem> GetLocationListItems(Dictionary<DictionaryParameter, string> parameters
            , MarsDBDataContext dataContext, bool checkOut = false)
        {
            var returned = new List<ListItem>();

            var dpLg = checkOut ? DictionaryParameter.CheckOutLocationGroup : DictionaryParameter.LocationGroup;
            var dpArea = checkOut ? DictionaryParameter.CheckOutArea : DictionaryParameter.Area;

            var locationGroup = parameters.ContainsValueAndIsntEmpty(dpLg)
                        ? parameters[dpLg] : null;
            var opsArea = parameters.ContainsValueAndIsntEmpty(dpArea)
                        ? parameters[dpArea] : null;



            if (locationGroup != null)
            {
                if (locationGroup.Contains(Separator))
                {
                    var splitLocationGroups = locationGroup.Split(Separator.ToCharArray()).Select(int.Parse).ToList();
                    returned = (from l in dataContext.LOCATIONs
                                where splitLocationGroups.Contains(l.cms_location_group_id.Value)
                                         && l.active
                                orderby l.location_name
                                select new ListItem(l.location1, l.dim_Location_id.ToString())).ToList();

                }
                else
                {
                    var locationGroupId = int.Parse(locationGroup);
                    returned = (from l in dataContext.LOCATIONs
                                where l.cms_location_group_id == locationGroupId
                                         && l.active
                                orderby l.location_name
                                select new ListItem(l.location1, l.dim_Location_id.ToString())).ToList();
                }
                return returned;
            }

            if (opsArea != null)
            {
                if (opsArea.Contains(Separator))
                {
                    var splitAreas = opsArea.Split(Separator.ToCharArray()).Select(int.Parse).ToList();
                    returned = (from l in dataContext.LOCATIONs
                        where splitAreas.Contains(l.ops_area_id)
                              && l.active
                        orderby l.location_name
                        select new ListItem(l.location1 + " " + l.location_name, l.dim_Location_id.ToString())).ToList();
                }
                else
                {
                    var opsAreaId = int.Parse(opsArea);
                    returned = (from l in dataContext.LOCATIONs
                        where l.ops_area_id == opsAreaId
                              && l.active
                        orderby l.location_name
                        select new ListItem(l.location1 + " " + l.location_name, l.dim_Location_id.ToString())).ToList();
                    
                }
            }
            return returned;
            
        }


        internal static List<ListItem> GetCarSegmentListItems(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dataContext)
        {
            var owningCountry = parameters[DictionaryParameter.OwningCountry].ToLower();
            if (string.IsNullOrEmpty(owningCountry))
            {
                return new List<ListItem>();
            }
            List<ListItem> returned;

            if (owningCountry.Contains(Separator))
            {
                var splitCountries = owningCountry.Split(Separator.ToCharArray());
                returned = (from cs in dataContext.CAR_SEGMENTs
                            where splitCountries.Contains(cs.country)
                            orderby cs.country, cs.car_segment1
                            select new ListItem(cs.country + "-" + cs.car_segment1
                                , cs.car_segment_id.ToString())).ToList();
            }
            else
            {
                returned = (from cs in dataContext.CAR_SEGMENTs
                            where cs.country.ToLower() == owningCountry
                            orderby cs.sort_car_segment
                            select new ListItem(cs.car_segment1, cs.car_segment_id.ToString())).ToList();
            }

            return returned;           
        }

        internal static List<ListItem> GetCarClassListItems(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dataContext)
        {
            var carSegmentId = parameters[DictionaryParameter.CarSegment];

            if (string.IsNullOrEmpty(carSegmentId))
            {
                return new List<ListItem>();
            }
            List<ListItem> returned;


            if (carSegmentId.Contains(Separator))
            {
                var splitSegments = carSegmentId.Split(Separator.ToCharArray()).Select(int.Parse).ToList();
                returned = (from cc in dataContext.CAR_CLASSes
                            where splitSegments.Contains(cc.car_segment_id)
                            orderby cc.CAR_SEGMENT.car_segment1, cc.car_class1
                            select new ListItem(cc.CAR_SEGMENT.country + "-" + cc.CAR_SEGMENT.car_segment1 + "-" + cc.car_class1
                                , cc.car_class_id.ToString())).ToList();
            }
            else
            {
                returned = (from cc in dataContext.CAR_CLASSes
                            where cc.CAR_SEGMENT.car_segment_id == int.Parse(carSegmentId)
                            orderby cc.sort_car_class
                            select new ListItem(cc.car_class1, cc.car_class_id.ToString())).ToList();
            }

            return returned;

        }

        internal static List<ListItem> GetCarCarGroupListItems(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dataContext)
        {
            var carClassId = parameters[DictionaryParameter.CarClass].ToLower();

            if (string.IsNullOrEmpty(carClassId))
            {
                return new List<ListItem>();
            }
            List<ListItem> returned;

            if (carClassId.Contains(Separator))
            {
                var splitClasses = carClassId.Split(Separator.ToCharArray()).Select(int.Parse).ToList();
                returned = (from cg in dataContext.CAR_GROUPs
                            where splitClasses.Contains(cg.car_class_id)
                           orderby cg.CAR_CLASS.CAR_SEGMENT.country, cg.car_group1
                           select new ListItem(cg.CAR_CLASS.CAR_SEGMENT.country + "-" + cg.CAR_CLASS.car_class1 + "-" + cg.car_group1, cg.car_group_id.ToString())).ToList();
            }
            else
            {
                returned = (from cg in dataContext.CAR_GROUPs
                        where cg.CAR_CLASS.car_class_id == int.Parse(carClassId)
                        orderby cg.sort_car_group
                        select new ListItem(cg.car_group1, cg.car_group_id.ToString())).ToList();    
            }

            return returned;
            
        }
    }
}