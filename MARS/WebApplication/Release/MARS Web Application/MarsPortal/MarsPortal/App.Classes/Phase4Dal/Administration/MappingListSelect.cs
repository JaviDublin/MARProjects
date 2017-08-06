using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.BLL.ExtensionMethods;

namespace Mars.App.Classes.Phase4Dal.Administration
{
    public class MappingListSelect : IDisposable
    {

        public MarsDBDataContext DataContext;
        

        public MappingListSelect()
        {
            DataContext = new MarsDBDataContext();
        }

        protected MappingListSelect(MarsDBDataContext dbc)
        {
            DataContext = dbc ?? new MarsDBDataContext();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        public List<CountryEntity> GetAllCountries(Dictionary<DictionaryParameter, string> parameter)
        {
            var countryData = from c in DataContext.COUNTRies

                              select new CountryEntity
                                         {
                                             CountryCode = c.country1,
                                             CountryName = c.country_description,
                                             CountryDw = c.country_dw,
                                             Id = c.CountryId,
                                             Active = c.active
                                         };

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ActiveOnly))
            {
                var activeOnly = parameter[DictionaryParameter.ActiveOnly];
                var activeOnlyBool = bool.Parse(activeOnly);
                countryData = countryData.Where(d => d.Active == activeOnlyBool);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ContainsString))
            {
                var quickSelect = parameter[DictionaryParameter.ContainsString];
                countryData = countryData.Where(d => d.CountryName.Contains(quickSelect));
            }
            var returned = countryData.ToList();
            return returned;
        }

        public List<CarSegmentEntity> GetCarSegments(Dictionary<DictionaryParameter, string> parameter)
        {
            var segmentData = from cs in DataContext.CAR_SEGMENTs
                              orderby cs.COUNTRy1.country_description, cs.car_segment1
                              select new CarSegmentEntity
                                         {
                                             Id = cs.car_segment_id,
                                             CarSegmentName = cs.car_segment1,
                                             CountryName = cs.COUNTRy1.country_description,
                                             CountryId = cs.COUNTRy1.CountryId,
                                             Active = cs.IsActive.HasValue && cs.IsActive.Value
                                         };

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ActiveOnly))
            {
                var activeOnly = parameter[DictionaryParameter.ActiveOnly];
                var activeOnlyBool = bool.Parse(activeOnly);
                segmentData = segmentData.Where(d => d.Active == activeOnlyBool);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var countryId = int.Parse(parameter[DictionaryParameter.LocationCountry]);
                segmentData = segmentData.Where(d => d.CountryId == countryId);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ContainsString))
            {
                var quickSelect = parameter[DictionaryParameter.ContainsString];
                segmentData = segmentData.Where(d => d.CarSegmentName.Contains(quickSelect));
            }

            var returned = segmentData.ToList();
            return returned;
        }

        public List<CarGroupEntity> GetCarGroups(Dictionary<DictionaryParameter, string> parameter)
        {
            var groupData = from cg in DataContext.CAR_GROUPs
                            orderby cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description, cg.car_group1
                            select new CarGroupEntity
                            {
                                Id = cg.car_group_id,
                                CarSegmentName = cg.CAR_CLASS.CAR_SEGMENT.car_segment1,
                                CountryName = cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description,
                                CountryId = cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.CountryId,
                                CarSegmentId = cg.CAR_CLASS.CAR_SEGMENT.car_segment_id,
                                CarClassName = cg.CAR_CLASS.car_class1,
                                CarClassId = cg.car_class_id,
                                CarGroupName = cg.car_group1,
                                CarGroupGold = cg.car_group_gold,
                                CarGroupPlatinum = cg.car_group_platinum,
                                CarGroupFiveStar = cg.car_group_fivestar,
                                CarGroupPresidentCircle = cg.car_group_presidentCircle,
                                Active = cg.IsActive.HasValue && cg.IsActive.Value
                            };

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ActiveOnly))
            {
                var activeOnly = parameter[DictionaryParameter.ActiveOnly];
                var activeOnlyBool = bool.Parse(activeOnly);
                groupData = groupData.Where(d => d.Active == activeOnlyBool);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassId = int.Parse(parameter[DictionaryParameter.CarClass]);
                groupData = groupData.Where(d => d.CarClassId == carClassId);
            }
            else if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(parameter[DictionaryParameter.CarSegment]);
                groupData = groupData.Where(d => d.CarSegmentId == carSegmentId);

            }
            else if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var countryId = int.Parse(parameter[DictionaryParameter.LocationCountry]);
                groupData = groupData.Where(d => d.CountryId == countryId);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ContainsString))
            {
                var quickSelect = parameter[DictionaryParameter.ContainsString];
                groupData = groupData.Where(d => d.CarGroupName.Contains(quickSelect));
            }

            var returned = groupData.ToList();
            return returned;
        }

        public List<CarClassEntity> GetCarClasses(Dictionary<DictionaryParameter, string> parameter)
        {
            var classData = from cc in DataContext.CAR_CLASSes
                            orderby cc.CAR_SEGMENT.COUNTRy1.country1, cc.CAR_SEGMENT.car_segment1, cc.car_class1
                              select new CarClassEntity
                              {
                                  Id = cc.car_class_id,
                                  CarSegmentName = cc.CAR_SEGMENT.car_segment1,
                                  CountryName = cc.CAR_SEGMENT.COUNTRy1.country_description,
                                  CountryId = cc.CAR_SEGMENT.COUNTRy1.CountryId,
                                  CarSegmentId = cc.CAR_SEGMENT.car_segment_id,
                                  CarClassName = cc.car_class1,
                                  Active = cc.IsActive.HasValue && cc.IsActive.Value
                              };

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ActiveOnly))
            {
                var activeOnly = parameter[DictionaryParameter.ActiveOnly];
                var activeOnlyBool = bool.Parse(activeOnly);
                classData = classData.Where(d => d.Active == activeOnlyBool);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(parameter[DictionaryParameter.CarSegment]);
                classData = classData.Where(d => d.CarSegmentId == carSegmentId);

            } else if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var countryId = int.Parse(parameter[DictionaryParameter.LocationCountry]);
                classData = classData.Where(d => d.CountryId == countryId);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ContainsString))
            {
                var quickSelect = parameter[DictionaryParameter.ContainsString];
                classData = classData.Where(d => d.CarClassName.Contains(quickSelect));
            }

            var returned = classData.ToList();
            return returned;
        }

        public List<PoolEntity> GetPools(Dictionary<DictionaryParameter, string> parameter)
        {
            var poolData = from p in DataContext.CMS_POOLs
                           orderby p.COUNTRy1.country1, p.cms_pool1
                              select new PoolEntity
                              {
                                  PoolName = p.cms_pool1,
                                  CountryName = p.COUNTRy1.country_description,
                                  Id = p.cms_pool_id,
                                  CountryId = p.COUNTRy1.CountryId,
                                  Active = p.IsActive.HasValue && p.IsActive.Value
                              };

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ActiveOnly))
            {
                var activeOnly = parameter[DictionaryParameter.ActiveOnly];
                var activeOnlyBool = bool.Parse(activeOnly);
                poolData = poolData.Where(d => d.Active == activeOnlyBool);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var countryId = int.Parse(parameter[DictionaryParameter.LocationCountry]);
                poolData = poolData.Where(d => d.CountryId == countryId);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ContainsString))
            {
                var quickSelect = parameter[DictionaryParameter.ContainsString];
                poolData = poolData.Where(d => d.PoolName.Contains(quickSelect));
            }
            var returned = poolData.ToList();
            return returned;
        }

        public List<RegionEntity> GetRegions(Dictionary<DictionaryParameter, string> parameter)
        {
            var regionData = from r in DataContext.OPS_REGIONs
                             orderby r.COUNTRy1.country_description, r.ops_region1
                           select new RegionEntity
                           {
                               RegionName = r.ops_region1,
                               CountryName = r.COUNTRy1.country_description,
                               Id = r.ops_region_id,
                               CountryId = r.COUNTRy1.CountryId,
                               Active = r.isActive.HasValue && r.isActive.Value
                           };

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ActiveOnly))
            {
                var activeOnly = parameter[DictionaryParameter.ActiveOnly];
                var activeOnlyBool = bool.Parse(activeOnly);
                regionData = regionData.Where(d => d.Active == activeOnlyBool);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var countryId = int.Parse(parameter[DictionaryParameter.LocationCountry]);
                regionData = regionData.Where(d => d.CountryId == countryId);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ContainsString))
            {
                var quickSelect = parameter[DictionaryParameter.ContainsString];
                regionData = regionData.Where(d => d.RegionName.Contains(quickSelect));
            }
            var returned = regionData.ToList();
            return returned;
        }

        public List<AreaEntity> GetAreas(Dictionary<DictionaryParameter, string> parameter)
        {
            var areas = from a in DataContext.OPS_AREAs
                        orderby a.OPS_REGION.COUNTRy1.country_description, a.OPS_REGION.ops_region1, a.ops_area1
                                    select new AreaEntity
                                    {
                                        AreaName = a.ops_area1,
                                        RegionName = a.OPS_REGION.ops_region1,
                                        RegionId = a.ops_region_id,
                                        CountryName = a.OPS_REGION.COUNTRy1.country_description,
                                        Id = a.ops_area_id,
                                        CountryId = a.OPS_REGION.COUNTRy1.CountryId,
                                        Active = a.isActive.HasValue && a.isActive.Value
                                    };

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ActiveOnly))
            {
                var activeOnly = parameter[DictionaryParameter.ActiveOnly];
                var activeOnlyBool = bool.Parse(activeOnly);
                areas = areas.Where(d => d.Active == activeOnlyBool);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
            {
                var regionId = int.Parse(parameter[DictionaryParameter.Region]);
                areas = areas.Where(d => d.RegionId == regionId);
            }
            else if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var countryId = int.Parse(parameter[DictionaryParameter.LocationCountry]);
                areas = areas.Where(d => d.CountryId == countryId);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ContainsString))
            {
                var quickSelect = parameter[DictionaryParameter.ContainsString];
                areas = areas.Where(d => d.AreaName.Contains(quickSelect));
            }
            var returned = areas.ToList();
            return returned;
        }

        public List<LocationGroupEntity> GetLocationGroups(Dictionary<DictionaryParameter, string> parameter)
        {
            var locationGroupData = from lg in DataContext.CMS_LOCATION_GROUPs
                                    orderby lg.CMS_POOL.COUNTRy1.country_description, lg.CMS_POOL.cms_pool1, lg.cms_location_group1
                           select new LocationGroupEntity
                           {
                               LocationGroupName = lg.cms_location_group1,
                               PoolName = lg.CMS_POOL.cms_pool1,
                               PoolId = lg.cms_pool_id,
                               CountryName = lg.CMS_POOL.COUNTRy1.country_description,
                               Id = lg.cms_location_group_id,
                               CountryId = lg.CMS_POOL.COUNTRy1.CountryId,
                               Active = lg.IsActive.HasValue && lg.IsActive.Value
                           };

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ActiveOnly))
            {
                var activeOnly = parameter[DictionaryParameter.ActiveOnly];
                var activeOnlyBool = bool.Parse(activeOnly);
                locationGroupData = locationGroupData.Where(d => d.Active == activeOnlyBool);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {
                var poolId = int.Parse(parameter[DictionaryParameter.Pool]);
                locationGroupData = locationGroupData.Where(d => d.PoolId == poolId);
            }
            else if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var countryId = int.Parse(parameter[DictionaryParameter.LocationCountry]);
                locationGroupData = locationGroupData.Where(d => d.CountryId == countryId);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ContainsString))
            {
                var quickSelect = parameter[DictionaryParameter.ContainsString];
                locationGroupData = locationGroupData.Where(d => d.LocationGroupName.Contains(quickSelect));
            }
            var returned = locationGroupData.ToList();
            return returned;
        }

        public List<LocationEntity> GetLocations(Dictionary<DictionaryParameter, string> parameter)
        {
            var locations = from l in DataContext.LOCATIONs
                            orderby l.COUNTRy1.country_description, l.location1
                                    select new LocationEntity
                                    {
                                        LocationGroupName = l.CMS_LOCATION_GROUP == null ? string.Empty : l.CMS_LOCATION_GROUP.cms_location_group1,
                                        LocationGroupId = l.cms_location_group_id.HasValue ? l.cms_location_group_id.Value : 0,
                                        PoolName = l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1,
                                        PoolId = l.CMS_LOCATION_GROUP.cms_pool_id,
                                        RegionName = l.OPS_AREA.OPS_REGION.ops_region1,
                                        RegionId = l.OPS_AREA.ops_region_id,
                                        AreaName = l.OPS_AREA.ops_area1,
                                        AreaId = l.ops_area_id,
                                        LocationFullName = l.location_name,
                                        CountryName = l.CMS_LOCATION_GROUP.CMS_POOL.COUNTRy1.country_description,
                                        Id = l.dim_Location_id,
                                        LocationCode = l.location1,
                                        CountryId = l.CMS_LOCATION_GROUP.CMS_POOL.COUNTRy1.CountryId,
                                        CorporateAgencyLicencee = l.cal,
                                        AirportDowntownRailroad = l.ap_dt_rr,
                                        ServedBy = l.served_by_locn,
                                        CompanyName = l.CompanyId == null ? string.Empty : l.Company.CompanyName,
                                        CompanyId = l.CompanyId,
                                        Active = l.active
                                    };

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ActiveOnly))
            {
                var activeOnly = parameter[DictionaryParameter.ActiveOnly];
                var activeOnlyBool = bool.Parse(activeOnly);
                locations = locations.Where(d => d.Active == activeOnlyBool);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)
                || parameter.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
            {
                if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
                {
                    var locationGroupId = int.Parse(parameter[DictionaryParameter.LocationGroup]);
                    locations = locations.Where(d => d.LocationGroupId == locationGroupId);
                }
                else
                {
                    var areaId = int.Parse(parameter[DictionaryParameter.Area]);
                    locations = locations.Where(d => d.RegionId == areaId);
                }
            }
            else if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.Pool)
                || parameter.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
            {
                if(parameter.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
                {
                    var poolId = int.Parse(parameter[DictionaryParameter.Pool]);
                    locations = locations.Where(d => d.PoolId == poolId);    
                }
                else
                {
                    var regionId = int.Parse(parameter[DictionaryParameter.Region]);
                    locations = locations.Where(d => d.RegionId == regionId);    
                }
            }
            else if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var countryId = int.Parse(parameter[DictionaryParameter.LocationCountry]);
                locations = locations.Where(d => d.CountryId == countryId);
            }

            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.CompanyId))
            {
                var companyId = int.Parse(parameter[DictionaryParameter.CompanyId]);
                locations = companyId == -1 ? 
                    locations.Where(d => !d.CompanyId.HasValue) 
                    : locations.Where(d => d.CompanyId.HasValue && d.CompanyId.Value == companyId);
            }


            if (parameter.ContainsValueAndIsntEmpty(DictionaryParameter.ContainsString))
            {
                var quickSelect = parameter[DictionaryParameter.ContainsString];
                locations = locations.Where(d => d.LocationCode.Contains(quickSelect));
            }
            var returned = locations.ToList();
            return returned;
        }
    }
}