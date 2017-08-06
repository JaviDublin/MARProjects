using System;
using System.Collections.Generic;
using App.DAL.Management;
using App.Entities;
using System.Transactions;
using App.BLL.Utilities;

namespace App.BLL.Management
{
    public class BLLManagement
    {
        private DALManagement dal = new DALManagement();

        public List<Country> CountryGetAllByRole(string user)
        {

            string cacheKey = user + "Country";
            object cacheItem = MarsV2Cache.GetCacheObject(cacheKey);

            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                cacheItem = dal.CountryGetAllByRole(user.Substring(user.LastIndexOf("\\") + 1));
                MarsV2Cache.AddObjectToCacheWithNoSlidingExpiry(cacheKey, cacheItem);
            }
            return new List<Country>((List<Country>)cacheItem);

        }

        public FleetPlanDetailListContainer GetFleetPlanDetailByCountryID(string countryID)
        {
            FleetPlanDetailListContainer fleetPlanDetailListContainer = dal.GetFleetPlanDetailByCountryID(countryID);
            return fleetPlanDetailListContainer;
        }

        public FleetPlanDetailListContainer GetFleetPlanDetailBy(string country,
        int locationGroup,
        int carClassGroup,
        DateTime startDate,
        DateTime endDate)
        {
            FleetPlanDetailListContainer fleetPlanDetailListContainer =
                dal.GetFleetPlanDetailBy(country, locationGroup, carClassGroup, startDate, endDate);
                
            return fleetPlanDetailListContainer;
        }

        public void DeleteFleetPlanDetailByFleetPlanDetailID(int fleetPlanDetailID)
        {
            dal.DeleteFleetPlanDetailByFleetPlanDetailID(fleetPlanDetailID);
        }

        public FleetPlanDetail GetFleetPlanDetailByID(int fleetPlanDetailID)
        {
            FleetPlanDetail fleetplanDetail = dal.GetFleetPlanDetailByID(fleetPlanDetailID);
            return fleetplanDetail;
        }

        public void FleetPlanMovementUpdate(int fleetPlanEntryID, int fleetPlanDetailID, int locationGroupFrom, int locationGroupTo, 
             string carClass, int movementCount, DateTime dateOfMovement)
        {
            FleetPlanDetail fleetplanDetailAddition = new FleetPlanDetail();
            FleetPlanDetail fleetplanDetailDeletion = new FleetPlanDetail();

            fleetplanDetailAddition.FleetPlanEntryID = fleetPlanEntryID;
            fleetplanDetailAddition.FleetPlanDetailID = fleetPlanDetailID;
            fleetplanDetailAddition.LocationGroup.LocationGroupID = locationGroupTo;
            fleetplanDetailAddition.Addition = movementCount;
            fleetplanDetailAddition.DateOfMovement = dateOfMovement;
            fleetplanDetailAddition.CarGroup.CarGroupID = Convert.ToInt32(carClass);

            fleetplanDetailDeletion.FleetPlanEntryID = fleetPlanEntryID;
            fleetplanDetailDeletion.FleetPlanDetailID = fleetPlanDetailID;
            fleetplanDetailDeletion.LocationGroup.LocationGroupID = locationGroupFrom;
            fleetplanDetailDeletion.Deletion = movementCount;
            fleetplanDetailDeletion.DateOfMovement = dateOfMovement;
            fleetplanDetailDeletion.CarGroup.CarGroupID = Convert.ToInt32(carClass);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    dal.FleetPlanUpdate(fleetplanDetailAddition);
                    dal.FleetPlanUpdate(fleetplanDetailDeletion);

                    scope.Complete();
                }
            }
            catch 
            {
                // do nothing
            }
        }

        public void FleetPlanAddDelUpdate(int fleetPlanEntryID, int fleetPlanDetailID, int targetLocationGroup,
            string carClass, int addition, int deletion, DateTime dateOfMovement)
        {
            var fleetplanDetail = new FleetPlanDetail
                                      {
                                          FleetPlanEntryID = fleetPlanEntryID,
                                          FleetPlanDetailID = fleetPlanDetailID,
                                          LocationGroup = {LocationGroupID = targetLocationGroup},
                                          Addition = addition,
                                          Deletion = deletion,
                                          DateOfMovement = dateOfMovement,
                                          CarGroup = {CarGroupID = Convert.ToInt32(carClass)}
                                      };

            dal.FleetPlanUpdate(fleetplanDetail);
        }

        public void FleetPlanBulkInsert(string user, string originalFileName, string archiveFileName, int fleetPlanID, string country, bool isAddition)
        {
            dal.FleetPlanBulkInsert(user, originalFileName, archiveFileName, fleetPlanID, country, isAddition);
        }

        public void FleetPlanEntryUploadArchiveCreate(string user, string originalFileName, string archiveFileName, int fleetPlanID, string country, bool isAddition)
        {
            dal.FleetPlanEntryUploadArchiveCreate(user, originalFileName, archiveFileName, fleetPlanID, country, isAddition);
        }

        public List<FleetPlanEntryArchive> FleetPlanEntryUploadArchiveGetByCountry(string country)
        {
            List<FleetPlanEntryArchive> fleetPlanEntryArchive = dal.FleetPlanEntryUploadArchiveGetByCountry(country);
            return fleetPlanEntryArchive;
        }

        public List<FrozenZoneAcceptance> FrozenZoneAcceptanceGetBy(string country)
        {
            List<FrozenZoneAcceptance> frozenZoneAcceptance = dal.FrozenZoneAcceptanceGetBy(country);
            return frozenZoneAcceptance;
        }
        
        public void UpdateFrozenForecast(string country, DateTime fromDate, DateTime toDate, string acceptedBy, int weekNumber)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    string year = fromDate.Year.ToString();

                    dal.UpdateFrozenForecast(country, fromDate, toDate);
                    dal.UpdateFrozenForecastAcceptanceLog(country, year, acceptedBy, weekNumber);
                    ts.Complete();
                }
            }
            catch
            {
            }

        }

        public List<NecessaryFleet> GetNecessaryFleetByCountryID(string countryID)
        {
            List<NecessaryFleet> necessaryFleetList = dal.GetNecessaryFleetByCountryID(countryID);
            return necessaryFleetList;
        }

        public void NecessaryFleetMultipleUpdate(string country, int locationGroupId, int carGroupID, double utilisation, double nonRev)
        {
            dal.NecessaryFleetMultipleUpdate(country, locationGroupId, carGroupID, utilisation, nonRev);
        }

        public void NecessaryFleetUpdate(NecessaryFleet necessaryFleet)
        {
            dal.NecessaryFleetUpdate(necessaryFleet);
        }
        
        public void NecessaryFleetNonRevUpdate(string countryID, DateTime from, DateTime to)
        {
            dal.NecessaryFleetNonRevUpdate(countryID, from, to);
        }

        public void NecessaryFleetUtilisationUpdate(string countryID, DateTime from, DateTime to)
        {
            dal.NecessaryFleetUtilisationUpdate(countryID, from, to);
        }

        public List<ForecastAdjustmentEntity> ForecastAdjustmentGet(string countryID, int carSegmentID,
            int carClassGroupID, int carClassID, int cmsPoolID, int locationGroupID, DateTime date)
        {
            List<ForecastAdjustmentEntity> forecastAdjustmentList = dal.ForecastAdjustmentGet(countryID, carSegmentID,
            carClassGroupID, carClassID, cmsPoolID, locationGroupID, date);
            return forecastAdjustmentList;
        }

        public void AdjustmentUpdate(int carSegmentID,
        int carClassGroupID,
        int carClassID,
        int cmsPoolID,
        int locationGroupID,
        int adjustmentToUpdate,
        int adjustmentType,
        bool addition,
        decimal adjustmentValue,
        DateTime date)
        {
            dal.AdjustmentUpdate(carSegmentID, carClassGroupID, carClassID, cmsPoolID, locationGroupID,
                adjustmentToUpdate, adjustmentType, addition, adjustmentValue, date);
        }

        public void AdjustmentAdapt(int carSegmentID,
        int carClassGroupID,
        int carClassID,
        int cmsPoolID,
        int locationGroupID,
        AdjustmentForecast from,
        Adjustment to,
        DateTime date)
        {
            dal.AdjustmentAdapt(carSegmentID, carClassGroupID, carClassID, cmsPoolID, locationGroupID, from, to, date);
        }

        public void AdjustmentReconcile(int carSegmentID,
        int carClassGroupID,
        int carClassID,
        int cmsPoolID,
        int locationGroupID,
        Adjustment from,
        DateTime date)
        {
            dal.AdjustmentReconcile(carSegmentID, carClassGroupID, carClassID, cmsPoolID, locationGroupID, from, date);
        }

        public List<UserRole> UsersInRolesGet(string user)
        {
            string cacheKey = user + "Roles";
            object cacheItem = MarsV2Cache.GetCacheObject(cacheKey);

            if ((ConfigAccess.ByPassCache()) || (cacheItem == null))
            {
                cacheItem = dal.UsersInRolesGet(user.Substring(user.LastIndexOf("\\") + 1));
                MarsV2Cache.AddObjectToCache(cacheKey, cacheItem);
            }
            return new List<UserRole>((List<UserRole>)cacheItem);

        }

        public void ForecastOperationalFleetUpdate(DateTime date, int fleetplanID, string country)
        {
            date = DateTime.Parse(date.ToShortDateString());
            dal.ForecastOperationalFleetUpdate(date, fleetplanID, country);
        }
    }
}