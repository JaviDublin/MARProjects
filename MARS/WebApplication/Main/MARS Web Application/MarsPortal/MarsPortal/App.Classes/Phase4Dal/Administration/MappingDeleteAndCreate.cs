using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;

namespace Mars.App.Classes.Phase4Dal.Administration
{
    public class MappingDeleteAndCreate : IDisposable
    {

        public MarsDBDataContext DataContext;

        private const string ForeignKeyError = "You can not delete this Record as it has other Records Foreign Keyed to it.";
        

        public MappingDeleteAndCreate()
        {
            DataContext = new MarsDBDataContext();
        }

        protected MappingDeleteAndCreate(MarsDBDataContext dbc)
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
            catch (SqlException e)
            {
                if (e.ErrorCode == -2146232060)
                {
                    return ForeignKeyError;
                }
                return e.Message;
            }
            return string.Empty;
        }

        public string DeleteCarClass(int carClassId)
        {
            var classToDelete = DataContext.CAR_CLASSes.Single(d => d.car_class_id == carClassId);

            DataContext.CAR_CLASSes.DeleteOnSubmit(classToDelete);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string CreateNewCarClass(CarClassEntity cce)
        {
            var classAlreadyExistsInRegion =
                CarClassEntityCheck.DoesClassExistForCountry(DataContext, cce.CarClassName,
                                                                           cce.CountryId);
            if (classAlreadyExistsInRegion)
            {
                return CarClassEntityCheck.ClassAlreadyExistsForCountry;
            }


            var newCarClassEnitiy = new CAR_CLASS
            {
                IsActive = true,
                car_class1 = cce.CarClassName,
                car_segment_id = cce.CarSegmentId
            };

            DataContext.CAR_CLASSes.InsertOnSubmit(newCarClassEnitiy);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string DeleteCarSegment(int carSegmentId)
        {
            var segmentToDelete = DataContext.CAR_SEGMENTs.Single(d => d.car_segment_id == carSegmentId);

            DataContext.CAR_SEGMENTs.DeleteOnSubmit(segmentToDelete);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string CreateNewCarSegment(CarSegmentEntity cse)
        {
            var segmentAlreadyExistsInCountry =
                CarSegmentEntityCheck.DoesSegmentExistForCountry(DataContext, cse.CarSegmentName,
                                                                           cse.CountryId);
            if (segmentAlreadyExistsInCountry)
            {
                return CarSegmentEntityCheck.SegmentAlreadyExistsForCountry;
            }

            var countryCode = DataContext.COUNTRies.Single(d => d.CountryId == cse.CountryId).country1;
            var newCarSegmentEnitiy = new CAR_SEGMENT
            {
                IsActive = true,
                car_segment1 = cse.CarSegmentName,
                country = countryCode,
            };

            DataContext.CAR_SEGMENTs.InsertOnSubmit(newCarSegmentEnitiy);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string CreateNewArea(AreaEntity ae)
        {
            var areaAlreadyExistsInRegion =
                AreaEntityCheck.DoesAreaNameExistForRegion(DataContext, ae.AreaName,
                                                                           ae.RegionId);
            if (areaAlreadyExistsInRegion)
            {
                return AreaEntityCheck.AreaAlreadyExistsForCountry;
            }

            var newAreaEntity = new OPS_AREA
            {
                isActive = true,
                ops_area1 = ae.AreaName,
                ops_region_id = ae.RegionId,
            };

            DataContext.OPS_AREAs.InsertOnSubmit(newAreaEntity);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string DeleteArea(int areaId)
        {
            var areaToDelete = DataContext.OPS_AREAs.Single(d => d.ops_area_id == areaId);

            DataContext.OPS_AREAs.DeleteOnSubmit(areaToDelete);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string DeleteLocationGroup(int locationGroupId)
        {
            var locationGroupToDelete = DataContext.CMS_LOCATION_GROUPs.Single(d => d.cms_location_group_id == locationGroupId);

            DataContext.CMS_LOCATION_GROUPs.DeleteOnSubmit(locationGroupToDelete);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string CreateNewLocationGroup(LocationGroupEntity lge)
        {
            var locationGroupAlreadyExistsInPool =
                LocationGroupEntityCheck.DoesLocationGroupNameExistForCountry(DataContext, lge.LocationGroupName,
                                                                           lge.PoolId);

            if (locationGroupAlreadyExistsInPool)
            {
                return LocationGroupEntityCheck.LocationGroupAlreadyExistsForCountry;
            }

            var newLocationGroupEntity = new CMS_LOCATION_GROUP
                                             {
                                                 IsActive = true,
                                                 cms_location_group1 = lge.LocationGroupName,
                                                 cms_location_group_code_dw = string.Empty,
                                                 cms_pool_id = lge.PoolId,
                                             };

            DataContext.CMS_LOCATION_GROUPs.InsertOnSubmit(newLocationGroupEntity);
            var returned = SubmitDbChanges();

            if (returned != string.Empty)
            {
                return returned;
            }

            //Insert new MARS_CMS_NECESSARY_FLEET

            var twoLetterCountryCode = DataContext.COUNTRies.Single(d => d.CountryId == lge.CountryId).country1;
            int locationGroupId = newLocationGroupEntity.cms_location_group_id;

            var allCarGroups = from cg in DataContext.CAR_GROUPs
                where cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.CountryId == lge.CountryId
                select cg.car_group_id;

            var fleetToInsert = new List<MARS_CMS_NECESSARY_FLEET>();
            
            foreach (var cg in allCarGroups)
            {
                    fleetToInsert.Add(new MARS_CMS_NECESSARY_FLEET
                                      {
                                           COUNTRY = twoLetterCountryCode,
                                           CMS_LOCATION_GROUP_ID = locationGroupId,
                                           CAR_CLASS_ID = cg,   //Not Car Class, NessFleet has bad column Naming
                                           UTILISATION = 100,
                                           NONREV_FLEET = 0,
                                      });
            }

            DataContext.MARS_CMS_NECESSARY_FLEETs.InsertAllOnSubmit(fleetToInsert);
            returned = SubmitDbChanges();
            return returned;
        }

        public string DeletePool(int poolId)
        {
            var poolToDelete = DataContext.CMS_POOLs.Single(d => d.cms_pool_id == poolId);

            DataContext.CMS_POOLs.DeleteOnSubmit(poolToDelete);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string CreateNewPool(PoolEntity pe)
        {
            var poolAlreadyExistsInCountry = PoolEntityCheck.DoesPoolNameExistForCountry(DataContext, pe.PoolName,
                                                                                         pe.CountryId);

            if(poolAlreadyExistsInCountry)
            {
                return PoolEntityCheck.PoolAlreadyExistsForCountry;
            }

            var countryCode = DataContext.COUNTRies.Single(d => d.CountryId == pe.CountryId).country1;
            var newPoolEnitiy = new CMS_POOL
                                    {
                                        IsActive = true,
                                        cms_pool1 = pe.PoolName,
                                        country = countryCode,
                                    };

            DataContext.CMS_POOLs.InsertOnSubmit(newPoolEnitiy);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string DeleteRegion(int regionId)
        {
            var regionToDelete = DataContext.OPS_REGIONs.Single(d => d.ops_region_id == regionId);

            DataContext.OPS_REGIONs.DeleteOnSubmit(regionToDelete);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string CreateNewRegion(RegionEntity re)
        {
            

            var regionAlreadyExistsInCountry = RegionEntityCheck.DoesRegionNameExistForCountry(DataContext, re.RegionName,
                                                                                         re.CountryId);

            if (regionAlreadyExistsInCountry)
            {
                return PoolEntityCheck.PoolAlreadyExistsForCountry;
            }

            var countryCode = DataContext.COUNTRies.Single(d => d.CountryId == re.CountryId).country1;
            var newRegionEnitiy = new OPS_REGION
            {
                isActive = true,
                ops_region1 = re.RegionName,
                country = countryCode,
            };

            DataContext.OPS_REGIONs.InsertOnSubmit(newRegionEnitiy);

            var returned = SubmitDbChanges();
            return returned;
        }

        public string CreateNewCountry(CountryEntity ce)
        {
            var countryCodeAlreadyExists = CountryEntityCheck.DoesCountryCodeAlreadyExist(DataContext, ce.CountryCode);

            if (countryCodeAlreadyExists)
            {
                return CountryEntityCheck.CountryCodeAlreadyExists;
            }

            var dwAlreadyExists = CountryEntityCheck.DoesCountryDwAlreadyExist(DataContext, ce.CountryDw, ce.Id);

            if (dwAlreadyExists)
            {
                return CountryEntityCheck.DwCodeAlreadyExists;
            }
            var newCountryEntity = new COUNTRy
                                       {
                                           active = true,
                                           country1 = ce.CountryCode,
                                           country_dw = ce.CountryDw,
                                           country_description = ce.CountryName
                                       };

            DataContext.COUNTRies.InsertOnSubmit(newCountryEntity);

            var returned = SubmitDbChanges();

            return returned;
        }
    
    }
}