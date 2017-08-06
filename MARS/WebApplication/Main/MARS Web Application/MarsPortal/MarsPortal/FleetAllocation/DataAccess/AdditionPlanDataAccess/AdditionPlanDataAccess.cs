using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI.WebControls;
using Castle.Components.DictionaryAdapter.Xml;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BulkInserts;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess.Entities;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.Entities.Output;
using Mars.FleetAllocation.DataContext;
using Rad.Security;

namespace Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess
{
    public class AdditionPlanDataAccess : BaseDataAccess
    {
        public AdditionPlanDataAccess(Dictionary<DictionaryParameter, string> parameters = null) : base (parameters) 
        {
            
        }

        public List<ListItem> GetAdditionPlanListItems(int countryId)
        {
            var additionPlans = from ap in DataContext.AdditionPlans
                                where ap.CountryId == countryId
                                orderby ap.AdditionPlanId descending 
                select new ListItem(ap.Name, ap.AdditionPlanId.ToString());
            var returned = additionPlans.Take(30).ToList();
            return returned;
        }

        public List<WeeklyAddition> GetAdditionPlanEntryEntries(int additionPlanId, LocationLevelGroupings locationGrouping
                    , VehicleLevelGrouping vehicleGrouping)
        {
            var dbEntities = AdditionPlanEntryFilter.GetAdditionPlanEntries(DataContext, Parameters);

            dbEntities = from apmmv in dbEntities
                         where apmmv.AdditionPlanId == additionPlanId
                         select apmmv;

            
            var weeklyAdditions = ApplyLocationGroupingToWeeklyAdditions(dbEntities, locationGrouping);

            weeklyAdditions = ApplyVehicleGroupingOnWeeklyAdditions(weeklyAdditions, vehicleGrouping);


            var returned = weeklyAdditions.ToList();
            return returned;
        }

        private IQueryable<WeeklyAddition> ApplyVehicleGroupingOnWeeklyAdditions(IQueryable<WeeklyAddition> values, VehicleLevelGrouping vehicleGrouping)
        {
            IQueryable<WeeklyAddition> returned;
            switch (vehicleGrouping)
            {
                case VehicleLevelGrouping.CarGroup:
                    returned = from ape in values
                               join cg in DataContext.CAR_GROUPs on ape.CarGroupId equals cg.car_group_id
                               select new WeeklyAddition
                               {
                                   IsoWeek = ape.IsoWeek,
                                   Year = ape.Year,
                                   Amount = ape.Amount,
                                   CpU = ape.CpU,
                                   CarGroup = cg.car_group1,
                                   Location = ape.Location
                               };
                    break;
                case VehicleLevelGrouping.CarClass:
                    returned = from ape in values
                               join cg in DataContext.CAR_GROUPs on ape.CarGroupId equals cg.car_group_id
                               group ape by new {ape.Year, ape.IsoWeek, ape.Location, cg.CAR_CLASS.car_class1}
                               into groupedData
                               select new WeeklyAddition
                               {
                                   IsoWeek = groupedData.Key.IsoWeek,
                                   Year = groupedData.Key.Year,
                                   Amount = groupedData.Sum(d=> d.Amount),
                                   CpU = groupedData.Sum(d=> d.CpU),
                                   CarGroup = groupedData.Key.car_class1,
                                   Location = groupedData.Key.Location
                               };
                    break;
                case VehicleLevelGrouping.CarSegement:
                    returned = from ape in values
                               join cg in DataContext.CAR_GROUPs on ape.CarGroupId equals cg.car_group_id
                               group ape by new { ape.Year, ape.IsoWeek, ape.Location, cg.CAR_CLASS.CAR_SEGMENT.car_segment1 }
                                   into groupedData
                                   select new WeeklyAddition
                                   {
                                       IsoWeek = groupedData.Key.IsoWeek,
                                       Year = groupedData.Key.Year,
                                       Amount = groupedData.Sum(d => d.Amount),
                                       CpU = groupedData.Sum(d => d.CpU),
                                       CarGroup = groupedData.Key.car_segment1,
                                       Location = groupedData.Key.Location
                                   };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("vehicleGrouping");
            }
            return returned;
        }

        

        private IQueryable<WeeklyAddition> ApplyLocationGroupingToWeeklyAdditions(IQueryable<AdditionPlanEntry> values, LocationLevelGroupings locationLevel)
        {
            IQueryable<WeeklyAddition> returned;
            switch (locationLevel)
            {
                case LocationLevelGroupings.Location:
                    returned = from ape in values
                                      select new WeeklyAddition
                                      {
                                          IsoWeek = ape.Week,
                                          Year = ape.Year,
                                          Amount = ape.Additions,
                                          CpU = ape.ContributionPerUnit,
                                          CarGroupId = ape.CarGroupId,
                                          Location = ape.LOCATION.location1
                                      };
                    break;
                case LocationLevelGroupings.LocationGroup:
                    returned = from v in values
                               group v by new { v.Year, v.Week, v.CarGroupId, v.LOCATION.CMS_LOCATION_GROUP.cms_location_group1 }
                                   into groupedData
                                   select new WeeklyAddition
                                   {
                                       Year = groupedData.Key.Year,
                                       IsoWeek = groupedData.Key.Week,
                                       CarGroupId = groupedData.Key.CarGroupId,
                                       Location = groupedData.Key.cms_location_group1,
                                       CpU = groupedData.Sum(d => d.ContributionPerUnit),
                                       Amount = groupedData.Sum(d => d.Additions)
                                   };
                    break;
                case LocationLevelGroupings.Pool:
                    returned = from v in values
                               group v by new { v.Year, v.Week, v.CarGroupId, v.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 }
                                   into groupedData
                                   select new WeeklyAddition
                                   {
                                       Year = groupedData.Key.Year,
                                       IsoWeek = groupedData.Key.Week,
                                       CarGroupId = groupedData.Key.CarGroupId,
                                       Location = groupedData.Key.cms_pool1,
                                       CpU = groupedData.Sum(d => d.ContributionPerUnit),
                                       Amount = groupedData.Sum(d => d.Additions)
                                   };
                    break;
                case LocationLevelGroupings.Country:
                    returned = from v in values
                               group v by new { v.Year, v.Week, v.CarGroupId, v.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.COUNTRy1.country_description }
                                   into groupedData
                                   select new WeeklyAddition
                                   {
                                       Year = groupedData.Key.Year,
                                       IsoWeek = groupedData.Key.Week,
                                       CarGroupId = groupedData.Key.CarGroupId,
                                       Location = groupedData.Key.country_description,
                                       CpU = groupedData.Sum(d => d.ContributionPerUnit),
                                       Amount = groupedData.Sum(d => d.Additions)
                                   };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("locationLevel");
            }
            return returned;
        }

        public List<AdditionPlanMinMaxRow> GetAdditionPlanMinMaxRows(int additionPlanId)
        {
            var dbEntities = AdditionPlanMinMaxValueFilter.GetAdditionPlanMinMaxValues(DataContext, Parameters);

            dbEntities = from apmmv in dbEntities
                where apmmv.AdditionPlanId == additionPlanId
                select apmmv;

            var minMaxValues = from apmmv in dbEntities
                            select new AdditionPlanMinMaxRow
                            {
                                Year = apmmv.Year,
                                Week = apmmv.Week,
                                CarGroup = apmmv.CAR_GROUP.car_group1,
                                Location = apmmv.LOCATION.location1,
                                Rank = apmmv.Rank,
                                TotalFleet = apmmv.TotalFleet,
                                AdditionsAndDeletions = apmmv.AdditionsAndDeletions,
                                MinFleet = apmmv.MinFleet,
                                MaxFleet = apmmv.MaxFleet,
                                Contribution = apmmv.Contribution,
                            };
   

            var returned = minMaxValues.ToList();
            return returned;
        }

        public AdditionPlan GetAdditionPlan(int additionPlanId)
        {
            var returned = DataContext.AdditionPlans.Single(d => d.AdditionPlanId == additionPlanId);
            return returned;
        }

        public void UpdateLessonsLearnt(int additionPlanId, string lessonLearnt)
        {
            var additionPlan = DataContext.AdditionPlans.Single(d => d.AdditionPlanId == additionPlanId);
            additionPlan.LessonLearnt = lessonLearnt;
            DataContext.SubmitChanges();
        }

        public void ApplyAdditionPlan(int additionPlanId, bool applied)
        {
            var additionPlan = DataContext.AdditionPlans.Single(d => d.AdditionPlanId == additionPlanId);
            additionPlan.Applied = applied;
            DataContext.SubmitChanges();
        }

        public List<AdditionPlanEntity> GetAdditionPlanHistory(bool appliedOnly = false)
        {
            var plans = DataContext.AdditionPlans.Select(d=> d);
            if (appliedOnly)
            {
                plans = plans.Where(d => d.Applied);
            }


            var additionPlans = from ap in plans
                                orderby ap.AdditionPlanId descending 
                                select new AdditionPlanEntity
                                       {
                                           Name = ap.Name,
                                           Applied = ap.Applied ? "Yes" : string.Empty,
                                           MaxFleetScenarioName = ap.MaxFleetScenarioName,
                                           MaxFleetScenarioDescription = ap.MaxFleetScenarioDescription,
                                           MinComSegScenarioName = ap.MinComSegScenarioName,
                                           MinComSegScenarioDescription = ap.MinComSegSccenarioDescription,
                                           CurrentDate = ap.CurrentDate,
                                           CreatedDate = ap.DateCreated,
                                           EndRevenueDate = ap.EndRevenueDate,
                                           StartRevenueDate = ap.StartRevenueDate,
                                           WeeksCalculated = ap.WeeksCalculated,
                                           ViewParameterId = ap.AdditionPlanId
                                       };



            var returned = additionPlans.ToList();
            return returned;
        }
        
        public void CreateAdditionPlan(AdditionPlanEntity ape
            , List<AdditionEntity> weeklyAdditions
            , List<WeeklyMaxMinValues> minMaxValues
            , int countryId)
        {

            var loggedOnEmployee = ApplicationAuthentication.GetEmployeeId();
            var marsUserId = GetMarsUserId(loggedOnEmployee);
            var entityToCreate = new AdditionPlan
                                 {
                                     Name = ape.Name,
                                     MinComSegScenarioName = ape.MinComSegScenarioName,
                                     MinComSegSccenarioDescription = ape.GetMinComSegScenarioDescription(),
                                     MaxFleetScenarioName = ape.MaxFleetScenarioName,
                                     MaxFleetScenarioDescription = ape.GetMaxFleetScenarioDescription(),
                                     StartRevenueDate = ape.GetStartRevenue(),
                                     EndRevenueDate = ape.GetEndRevenue(),
                                     CurrentDate = ape.GetCurrentDate(),
                                     DateCreated = DateTime.Now,
                                     WeeksCalculated = ape.GetWeeksCalculated(),
                                     CountryId = countryId,
                                     //AdditionPlanEntries = additionPlanEntries,
                                     //AdditionPlanMinMaxValues = minMaxValueEntities,
                                     //CreatedBy = createdBy,
                                     MarsUserId = marsUserId
                                 };

            DataContext.AdditionPlans.InsertOnSubmit(entityToCreate);

            DataContext.SubmitChanges();

            var additionPlanId = entityToCreate.AdditionPlanId;
            
            var maxMinValues = from mmv in minMaxValues
                               select new AdditionPlanMinMaxValue
                               {
                                   AdditionPlanId = additionPlanId,
                                   Year = (short)mmv.Year,
                                   Week = (byte)mmv.WeekNumber,
                                   CarGroupId = mmv.GetCarGroupId(),
                                   LocationId = mmv.GetLocationId(),
                                   Rank = mmv.RankFromRevenue,
                                   TotalFleet = mmv.TotalFleet,
                                   AdditionsAndDeletions = mmv.AdditionDeletionSum,                                   
                                   MinFleet = mmv.MinFleet,
                                   MaxFleet = mmv.MaxFleet,
                                   Contribution = mmv.Contribution,
                               };

            var minMaxEntriesToBeInserted = maxMinValues.ToList();
            minMaxEntriesToBeInserted.BulkCopyToDatabase("AdditionPlanMinMaxValue", DataContext, "fao");


            var additionPlanEntryEntities = from wa in weeklyAdditions
                                            select new AdditionPlanEntry
                                            {
                                                Week = (byte)wa.IsoWeek,
                                                Year = (short)wa.Year,
                                                CarGroupId = wa.GetCarGroupId(),
                                                LocationId = wa.GetLocationId(),
                                                Additions = wa.Amount,
                                                AdditionPlanId = additionPlanId,
                                                ContributionPerUnit = (decimal) wa.Contribution,
                                            };


            var additionPlanEntriesToBeInserted = additionPlanEntryEntities.ToList();
            additionPlanEntriesToBeInserted.BulkCopyToDatabase("AdditionPlanEntry", DataContext, "fao");
        }

    }
}