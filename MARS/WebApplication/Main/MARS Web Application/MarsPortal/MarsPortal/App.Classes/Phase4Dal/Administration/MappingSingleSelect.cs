using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;

namespace Mars.App.Classes.Phase4Dal.Administration
{
    public class MappingSingleSelect : IDisposable
    {
        public MarsDBDataContext DataContext;


        public MappingSingleSelect()
        {
            DataContext = new MarsDBDataContext();
        }

        protected MappingSingleSelect(MarsDBDataContext dbc)
        {
            DataContext = dbc ?? new MarsDBDataContext();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        public CountryEntity GetCountryEnitity(int id)
        {
            var country = from c in DataContext.COUNTRies
                           where c.CountryId == id
                           select new CountryEntity
                                      {
                                          Id = c.CountryId,
                                          CountryName = c.country_description,
                                          CountryDw = c.country_dw,
                                          CountryCode = c.country1,
                                          Active = c.active
                                      };
            var returned = country.FirstOrDefault();
            return returned;
        }

        public PoolEntity GetPoolEnitity(int id)
        {
            var pool = from p in DataContext.CMS_POOLs
                                 where p.cms_pool_id == id
                                 select new PoolEntity
                                 {
                                     Id = p.cms_pool_id,
                                     CountryName = p.COUNTRy1.country_description,
                                     PoolName = p.cms_pool1,
                                     Active = p.IsActive.HasValue && p.IsActive.Value
                                 };
            var returned = pool.FirstOrDefault();
            return returned;
        }

        public LocationGroupEntity GetLocationGroupEnitity(int id)
        {
            var locationGroup = from lg in DataContext.CMS_LOCATION_GROUPs
                                 where lg.cms_location_group_id == id
                                 select new LocationGroupEntity
                                 {
                                     Id = lg.cms_location_group_id,
                                     CountryName = lg.CMS_POOL.COUNTRy1.country_description,
                                     PoolName = lg.CMS_POOL.cms_pool1,
                                     PoolId = lg.cms_pool_id,
                                     CountryId = lg.CMS_POOL.COUNTRy1.CountryId,
                                     LocationGroupName = lg.cms_location_group1,
                                     Active = lg.IsActive.HasValue && lg.IsActive.Value
                                 };

            var returned = locationGroup.FirstOrDefault();
            return returned;
        }

        public RegionEntity GetRegionEntity(int id)
        {
            var region = from r in DataContext.OPS_REGIONs
                         where r.ops_region_id == id
                         select new RegionEntity
                                    {
                                        Id = r.ops_region_id,
                                        CountryName = r.COUNTRy1.country_description,
                                        RegionName = r.ops_region1,
                                        Active = r.isActive.HasValue && r.isActive.Value
                                    };
            var returned = region.FirstOrDefault();
            return returned;
        }

        public AreaEntity GetAreaEntity(int id)
        {
            var area = from a in DataContext.OPS_AREAs
                         where a.ops_area_id == id
                       select new AreaEntity
                         {
                             Id = a.ops_area_id,
                             CountryName = a.OPS_REGION.COUNTRy1.country_description,
                             CountryId = a.OPS_REGION.COUNTRy1.CountryId,
                             RegionName = a.OPS_REGION.ops_region1,
                             RegionId = a.ops_region_id,
                             AreaName = a.ops_area1,
                             Active = a.isActive.HasValue && a.isActive.Value
                         };
            var returned = area.FirstOrDefault();
            return returned;
        }

        public List<string> GetAutoCompletedLocations(string prefix, int count, string countryCode)
        {
            var locations = from l in DataContext.LOCATIONs
                            where l.country == countryCode
                                && SqlMethods.Like(l.location1, prefix + "%")
                            orderby l.location1
                            select l.location1;
            var returned = locations.Take(count).ToList();
            return returned;
        }

        public LocationEntity GetLocationEntity(int id)
        {
            var locations = from l in DataContext.LOCATIONs
                       where l.dim_Location_id == id
                       select new LocationEntity
                       {
                           Id = l.dim_Location_id,
                           CountryName = l.CMS_LOCATION_GROUP.CMS_POOL.COUNTRy1.country_description,
                           CountryId = l.CMS_LOCATION_GROUP.CMS_POOL.COUNTRy1.CountryId,
                           RegionName = l.OPS_AREA.OPS_REGION.ops_region1,
                           CountryCode = l.country,
                           RegionId = l.OPS_AREA.ops_region_id,
                           AreaId = l.ops_area_id,
                           PoolName = l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1,
                           PoolId = l.CMS_LOCATION_GROUP.cms_pool_id,
                           LocationGroupId = l.cms_location_group_id.HasValue ? l.cms_location_group_id.Value : 0,
                           LocationCode = l.location1,
                           LocationFullName = l.location_name,
                           Active = l.active,
                           AirportDowntownRailroad = l.ap_dt_rr,
                           CorporateAgencyLicencee = l.cal,
                           TurnaroundHours = l.turnaround_hours.HasValue ? l.turnaround_hours.Value :0,
                           ServedBy = l.served_by_locn,
                           CompanyId = l.CompanyId
                           
                       };
            var returned = locations.FirstOrDefault();
            return returned;
        }

        public CarSegmentEntity GetCarSegmentEntity(int id)
        {
            var segments = from cs in DataContext.CAR_SEGMENTs
                            where cs.car_segment_id == id
                            select new CarSegmentEntity
                            {
                                Id = cs.car_segment_id,
                                CountryName = cs.COUNTRy1.country_description,
                                CarSegmentName = cs.car_segment1,
                                Active = cs.IsActive.HasValue && cs.IsActive.Value
                            };
            var returned = segments.FirstOrDefault();
            return returned;
        }

        public CarClassEntity GetCarClassEntity(int id)
        {
            var locations = from cc in DataContext.CAR_CLASSes
                            where cc.car_class_id == id
                            select new CarClassEntity
                            {
                                Id = cc.car_segment_id,
                                CountryName = cc.CAR_SEGMENT.COUNTRy1.country_description,
                                CountryId = cc.CAR_SEGMENT.COUNTRy1.CountryId,
                                CarSegmentName = cc.CAR_SEGMENT.car_segment1,
                                CarClassName = cc.car_class1,
                                Active = cc.IsActive.HasValue && cc.IsActive.Value
                            };
            var returned = locations.FirstOrDefault();
            return returned;
        }

        public CarGroupEntity GetCarGroupEntity(int id)
        {
            var locations = from cg in DataContext.CAR_GROUPs
                            where cg.car_group_id == id
                            select new CarGroupEntity
                            {
                                Id = cg.car_group_id,
                                CountryName = cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description,
                                CountryId = cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.CountryId,
                                CarSegmentName = cg.CAR_CLASS.CAR_SEGMENT.car_segment1,
                                CarClassName = cg.CAR_CLASS.car_class1,
                                CarClassId = cg.car_class_id,
                                CarSegmentId = cg.CAR_CLASS.car_segment_id,
                                CarGroupName = cg.car_group1,
                                CarGroupGold = cg.car_group_gold,
                                CarGroupFiveStar = cg.car_group_fivestar,
                                CarGroupPlatinum = cg.car_group_platinum,
                                CarGroupPresidentCircle = cg.car_group_presidentCircle,
                                Active = cg.IsActive.HasValue && cg.IsActive.Value
                            };
            var returned = locations.FirstOrDefault();
            return returned;
        }
    }
}