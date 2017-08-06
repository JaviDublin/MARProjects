using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;

namespace Mars.App.Classes.Phase4Dal.Administration
{
    public class MappingSingleUpdate : IDisposable
    {
        public MarsDBDataContext DataContext;

        public MappingSingleUpdate()
        {
            DataContext = new MarsDBDataContext();
        }

        protected MappingSingleUpdate(MarsDBDataContext dbc)
        {
            DataContext = dbc ?? new MarsDBDataContext();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        private string SubmitDbChanges()
        {
            try
            {
                DataContext.SubmitChanges();
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return string.Empty;
        }
        
        public string UpdateCountry(CountryEntity ce)
        {
            var dwAlreadyExists = CountryEntityCheck.DoesCountryDwAlreadyExist(DataContext, ce.CountryDw, ce.Id);

            if(dwAlreadyExists)
            {
                return CountryEntityCheck.DwCodeAlreadyExists;
            }

            var countryDbEntry = DataContext.COUNTRies.Single(d => d.CountryId == ce.Id);

            countryDbEntry.country1 = ce.CountryCode;
            countryDbEntry.country_dw = ce.CountryDw;
            countryDbEntry.country_description = ce.CountryName;
            countryDbEntry.active = ce.Active;

            var returned = SubmitDbChanges();
            return returned;
        }

        public string UpdatePool(PoolEntity pe)
        {
            var countryId = DataContext.CMS_POOLs.Single(d=> d.cms_pool_id == pe.Id).COUNTRy1.CountryId;
            

            var poolAlreadyExistsInCountry = PoolEntityCheck.DoesPoolNameExistForCountry(DataContext, pe.PoolName,
                                                                                         countryId, pe.Id);

            if (poolAlreadyExistsInCountry)
            {
                return PoolEntityCheck.PoolAlreadyExistsForCountry;
            }

            var poolDbEntry = DataContext.CMS_POOLs.Single(d => d.cms_pool_id == pe.Id);
            poolDbEntry.cms_pool1 = pe.PoolName;
            poolDbEntry.IsActive = pe.Active;

            var returned = SubmitDbChanges();
            return returned;
        }

        public string UpdateLocationGroup(LocationGroupEntity lge)
        {
            var locationGroupAlreadyExistsInPool =
                LocationGroupEntityCheck.DoesLocationGroupNameExistForCountry(DataContext, lge.LocationGroupName,
                                                                           lge.PoolId, lge.Id);
            if (locationGroupAlreadyExistsInPool)
            {
                return LocationGroupEntityCheck.LocationGroupAlreadyExistsForCountry;
            }


            var locationGroupDbEntry =
                DataContext.CMS_LOCATION_GROUPs.Single(d => d.cms_location_group_id == lge.Id);

            locationGroupDbEntry.cms_pool_id = lge.PoolId;
            locationGroupDbEntry.cms_location_group1 = lge.LocationGroupName;
            locationGroupDbEntry.IsActive = lge.Active;

            var returned = SubmitDbChanges();
            return returned;
        }

        public string UpdateRegion(RegionEntity re)
        {
            var countryId = DataContext.OPS_REGIONs.Single(d => d.ops_region_id == re.Id).COUNTRy1.CountryId;
            var poolAlreadyExistsInCountry = RegionEntityCheck.DoesRegionNameExistForCountry(DataContext, re.RegionName,
                                                                                         countryId, re.Id);

            if (poolAlreadyExistsInCountry)
            {
                return RegionEntityCheck.RegionAlreadyExistsForCountry;
            }


            var regionDbEntry = DataContext.OPS_REGIONs.Single(d => d.ops_region_id == re.Id);
            regionDbEntry.ops_region1 = re.RegionName;
            regionDbEntry.isActive = re.Active;
            
            var returned = SubmitDbChanges();
            return returned;
        }

        public string UpdateArea(AreaEntity ae)
        {
            var areaAlreadyExistsInRegion =
                AreaEntityCheck.DoesAreaNameExistForRegion(DataContext, ae.AreaName,
                                                                           ae.RegionId, ae.Id);
            if (areaAlreadyExistsInRegion)
            {
                return AreaEntityCheck.AreaAlreadyExistsForCountry;
            }

            var areaDbEntry =
                DataContext.OPS_AREAs.Single(d => d.ops_area_id == ae.Id);

            areaDbEntry.ops_region_id = ae.RegionId;
            areaDbEntry.ops_area1 = ae.AreaName;
            areaDbEntry.isActive = ae.Active;

            var returned = SubmitDbChanges();
            return returned;
        }

        public string UpdateLocation(LocationEntity le)
        {
            var locationDbEntry = DataContext.LOCATIONs.Single(d => d.dim_Location_id == le.Id);

            var servedByLocationExist = LocationEntityCheck.DoesServedByLocationExist(DataContext, le.ServedBy);

            if (!servedByLocationExist)
            {
                return LocationEntityCheck.ServedByLocationDoesntExist;
            }

            var servedBySameCountry = LocationEntityCheck.IsServedByLocationInSameCountry(DataContext,
                                                                                               le.Id,
                                                                                               le.ServedBy);
            if (!servedBySameCountry)
            {
                return LocationEntityCheck.ServedByLocationNotInCountry;
            }
                


            locationDbEntry.cms_location_group_id = le.LocationGroupId;
            locationDbEntry.ops_area_id = le.AreaId;
            locationDbEntry.location1 = le.LocationCode;
            locationDbEntry.active = le.Active;
            locationDbEntry.cal = le.CorporateAgencyLicencee;
            locationDbEntry.ap_dt_rr = le.AirportDowntownRailroad;
            locationDbEntry.location_name = le.LocationFullName;
            locationDbEntry.served_by_locn = le.ServedBy;
            locationDbEntry.turnaround_hours = le.TurnaroundHours;
            locationDbEntry.CompanyId = le.CompanyId;

            var returned = SubmitDbChanges();
            return returned;
        }

        public string UpdateCarSegment(CarSegmentEntity cse)
        {
            var carSegmentDbEntry = DataContext.CAR_SEGMENTs.Single(d => d.car_segment_id == cse.Id);

            var segmentAlreadyExistsInCountry =
                CarSegmentEntityCheck.DoesSegmentExistForCountry(DataContext, cse.CarSegmentName,
                                                                           carSegmentDbEntry.COUNTRy1.CountryId, cse.Id);
            if (segmentAlreadyExistsInCountry)
            {
                return CarSegmentEntityCheck.SegmentAlreadyExistsForCountry;
            }

            carSegmentDbEntry.car_segment1 = cse.CarSegmentName;
            carSegmentDbEntry.IsActive = cse.Active;

            var returned = SubmitDbChanges();
            return returned;
        }

        public string UpdateCarClass(CarClassEntity cce)
        {
            var carClassDbEntry = DataContext.CAR_CLASSes.Single(d => d.car_class_id == cce.Id);

            var classAlreadyExistsInCountry =
                CarClassEntityCheck.DoesClassExistForCountry(DataContext, cce.CarClassName,
                                                                           carClassDbEntry.CAR_SEGMENT.COUNTRy1.CountryId
                                                                           , cce.Id);
            if (classAlreadyExistsInCountry)
            {
                return CarClassEntityCheck.ClassAlreadyExistsForCountry;
            }

            carClassDbEntry.car_class1 = cce.CarClassName;
            //carClassDbEntry.car_segment_id = cce.CarSegmentId;

            carClassDbEntry.IsActive = cce.Active;

            var returned = SubmitDbChanges();
            return returned;
        }

        public string UpdateCarGroup(CarGroupEntity cge)
        {
            var carGroupDbEntry = DataContext.CAR_GROUPs.Single(d => d.car_group_id == cge.Id);

            carGroupDbEntry.car_class_id = cge.CarClassId;
            carGroupDbEntry.car_group_gold = cge.CarGroupGold;
            carGroupDbEntry.car_group_presidentCircle = cge.CarGroupPresidentCircle;
            carGroupDbEntry.car_group_platinum = cge.CarGroupPlatinum;
            carGroupDbEntry.car_group_fivestar = cge.CarGroupFiveStar;

            //carGroupDbEntry.CAR_CLASS.car_segment_id = cge.CarSegmentId;

            carGroupDbEntry.IsActive = cge.Active;

            var returned = SubmitDbChanges();
            return returned;
        }
        
    }
}