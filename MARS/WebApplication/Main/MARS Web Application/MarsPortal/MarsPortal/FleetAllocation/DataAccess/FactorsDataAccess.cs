using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;
using Castle.Core.Internal;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BulkInserts;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.ParameterFiltering;
using Mars.FleetAllocation.DataAccess.StoredProcedureParameters;
using Mars.FleetAllocation.DataContext;
using Rad.Security;

namespace Mars.FleetAllocation.DataAccess
{
    public class FactorsDataAccess : BaseDataAccess
    {
        public FactorsDataAccess(Dictionary<DictionaryParameter, string> parameters)
        {
            Parameters = parameters;
        }

       

        public List<MaxFleetFactorRow> GetMaxFleetFactors(int scenarioId)
        {
            if (!Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.DayOfWeek))
            {
                return null;
            }
            
            var splitDaysOfWeek =
                Parameters[DictionaryParameter.DayOfWeek].Split(VehicleFieldRestrictions.Separator.ToCharArray())
                    .Select(int.Parse)
                    .ToList();

            var locations = LocationQueryable.GetLocations(DataContext, Parameters);
            var carGroups = CarGroupQueryable.GetCarGroups(DataContext, Parameters);
            var weekDays = from wd in DataContext.WeekDays
                where splitDaysOfWeek.Contains(wd.DayOfWeekId)
                select wd.DayOfWeekId;

            var locationsAndGroups = from l in locations
                from cg in carGroups
                from wd in weekDays
                select new
                       {
                           LocationId = l.dim_Location_id,
                           CarGroupId = cg.car_group_id,
                           DayOfWeekId = wd
                       };

            var rows = from lag in locationsAndGroups
                       join mff in DataContext.MaxFleetFactors.Where(d => d.MaxFleetFactorScenarioId == scenarioId) on
                    new {lag.CarGroupId, lag.LocationId, lag.DayOfWeekId} equals
                    new {mff.CarGroupId, mff.LocationId, mff.DayOfWeekId}
                into jmf 
                from joinedMaxFleet in jmf.DefaultIfEmpty()
                join cg in DataContext.CAR_GROUPs on lag.CarGroupId equals cg.car_group_id
                join loc in DataContext.LOCATIONs on lag.LocationId equals loc.dim_Location_id
                select new MaxFleetFactorRow(lag.DayOfWeekId)
                       {
                            Country = cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description,
                            Pool = loc.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1,
                            LocationGroup = loc.CMS_LOCATION_GROUP.cms_location_group1,
                            Location = loc.location1,
                            CarSegment = cg.CAR_CLASS.CAR_SEGMENT.car_segment1,
                            CarClass = cg.CAR_CLASS.car_class1,
                            CarGroup = cg.car_group1,
                            LastChangedOn = joinedMaxFleet == null ? (DateTime?) null : joinedMaxFleet.UpdatedOn,
                            LastChangedBy = joinedMaxFleet == null ? string.Empty : joinedMaxFleet.MarsUser.EmployeeId,
                            NonRevenue = joinedMaxFleet == null ?  (double?) null : joinedMaxFleet.NonRevPercentage,
                            Utilization = joinedMaxFleet == null ? (double?) null : joinedMaxFleet.UtilizationPercentage,
                       };
            var returned = rows.ToList();
            return returned;
        }
        public void UpdateMaxFleetFactors(double? nonRevPercent, double? utilizationPercent, int scenarioId)
        {
            if (!Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.DayOfWeek))
            {
                return;
            }

            var splitDaysOfWeek =
                Parameters[DictionaryParameter.DayOfWeek].Split(VehicleFieldRestrictions.Separator.ToCharArray())
                    .Select(int.Parse)
                    .ToList();

            var locations = LocationQueryable.GetLocations(DataContext, Parameters);
            var carGroups = CarGroupQueryable.GetCarGroups(DataContext, Parameters);
            var weekDays = from wd in DataContext.WeekDays
                           where splitDaysOfWeek.Contains(wd.DayOfWeekId)
                           select wd.DayOfWeekId;

            var locationsAndGroups = from l in locations
                                     from cg in carGroups
                                     from wd in weekDays
                                     select new
                                     {
                                         LocationId = l.dim_Location_id,
                                         CarGroupId = cg.car_group_id,
                                         DayOfWeekId = wd
                                     };

            var employeeId = ApplicationAuthentication.GetEmployeeId();
            var marsUserId = GetMarsUserId(employeeId);

            var updateParam = BuildSprocParameter();
            updateParam.ScenarioId = scenarioId;
            updateParam.DayOfWeekIds = Parameters[DictionaryParameter.DayOfWeek];
            updateParam.MarsUserId = marsUserId;
            updateParam.NonRevPercentage = (float?) nonRevPercent;
            updateParam.UtilizationPercentage = (float?) utilizationPercent;


            DataContext.UpdateMaxFleetFactors(updateParam.ScenarioId, updateParam.LocationId,
                updateParam.LocationGroupId
                , updateParam.PoolId, updateParam.LocationCountry, updateParam.CarGroupId,
                updateParam.CarClassId, updateParam.CarSegmentId, updateParam.OwningCountry,
                updateParam.DayOfWeekIds, updateParam.MarsUserId, updateParam.NonRevPercentage
                , updateParam.UtilizationPercentage);


            var entriesToCreate = from fullCs in locationsAndGroups
                                  join mff in DataContext.MaxFleetFactors.Where(d=> d.MaxFleetFactorScenarioId == scenarioId) on
                                      new { fullCs.LocationId, fullCs.CarGroupId, fullCs.DayOfWeekId }
                                      equals new { mff.LocationId, mff.CarGroupId, mff.DayOfWeekId }
                                  into joinedFleetFactors
                                  from fleetFactors in joinedFleetFactors.DefaultIfEmpty()
                                  where fleetFactors == null
                                  select new
                                  {
                                      LocationId = fullCs.LocationId,
                                      CarGroupId = fullCs.CarGroupId,
                                      DayOfWeekId = fullCs.DayOfWeekId,
                                      NonRev = nonRevPercent,
                                      Utilization = utilizationPercent,
                                      LastChangedOn = DateTime.Now,
                                      MarsUserId = marsUserId
                                  };

            
            DataContext.SubmitChanges();
            var localEntities = (from etc in entriesToCreate.ToList()
                                select new MaxFleetFactor
                                {
                                    MaxFleetFactorScenarioId = scenarioId,
                                    LocationId = etc.LocationId,
                                    CarGroupId = etc.CarGroupId,
                                    DayOfWeekId = etc.DayOfWeekId,
                                    NonRevPercentage = etc.NonRev,
                                    UtilizationPercentage = etc.Utilization,
                                    UpdatedOn = DateTime.Now,
                                    MarsUserId = etc.MarsUserId
                                }).ToList();

            localEntities.ToList().BulkCopyToDatabase("MaxFleetFactor", DataContext, "fao");

            //DataContext.MaxFleetFactors.InsertAllOnSubmit(localEntities);
            DataContext.SubmitChanges();
        }

        private UpdateMaxFleetFactorParameter BuildSprocParameter()
        {
            var returned = new UpdateMaxFleetFactorParameter();


            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                returned.CarGroupId = int.Parse(Parameters[DictionaryParameter.CarGroup]);

            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                returned.CarClassId = int.Parse(Parameters[DictionaryParameter.CarClass]);


            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                returned.CarSegmentId = int.Parse(Parameters[DictionaryParameter.CarSegment]);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
               returned.OwningCountry = Parameters[DictionaryParameter.OwningCountry];
            }


            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                returned.LocationId = int.Parse(Parameters[DictionaryParameter.Location]);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
            {
                returned.LocationGroupId = int.Parse(Parameters[DictionaryParameter.LocationGroup]);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {
                returned.PoolId = int.Parse(Parameters[DictionaryParameter.Pool]);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                returned.LocationCountry = Parameters[DictionaryParameter.LocationCountry];
            }
            return returned;
        }

        public void UpdateMinCommercialSegments(double newMinCommSegPercent, int scenarioId)
        {
            var comSegData = CommercialSegmentQueryable.GetCommercialCarSemgents(DataContext, Parameters);
            var locations = LocationQueryable.GetLocations(DataContext, Parameters);
            var carSegments = CarSegmentQueryable.GetCarSegments(DataContext, Parameters);

            var fullComSeg = from ccs in comSegData
                             from l in locations
                             from cs in carSegments
                             select new
                             {
                                 LocationId = l.dim_Location_id,
                                 CarSegmentId = cs.car_segment_id,
                                 ccs.CommercialCarSegmentId
                             };

            var minCommIdsToUpdate = from fullCs in fullComSeg
                                     join mcs in DataContext.MinCommercialSegments.Where(d => d.MinCommercialSegmentScenarioId == scenarioId) on
                                            new {fullCs.LocationId, fullCs.CarSegmentId, fullCs.CommercialCarSegmentId}
                                    equals new {mcs.LocationId, mcs.CarSegmentId, mcs.CommercialCarSegmentId}
                                    select mcs;

            var employeeId = ApplicationAuthentication.GetEmployeeId();
            var marsUserId = GetMarsUserId(employeeId);
            minCommIdsToUpdate.ForEach(d => d.Percentage = newMinCommSegPercent);
            minCommIdsToUpdate.ForEach(d => d.MarsUserId = marsUserId);
            minCommIdsToUpdate.ForEach(d => d.UpdatedOn = DateTime.Now);

           

            var entriesToCreate = from fullCs in fullComSeg
                                  join mcs in DataContext.MinCommercialSegments.Where(d => d.MinCommercialSegmentScenarioId == scenarioId) on
                                            new { fullCs.LocationId, fullCs.CarSegmentId, fullCs.CommercialCarSegmentId }
                                      equals new { mcs.LocationId, mcs.CarSegmentId, mcs.CommercialCarSegmentId }
                                  into joinedComSeg
                                  from comSegFigures in joinedComSeg.DefaultIfEmpty()
                                  where comSegFigures == null
                                  select new 
                                         {
                                             LocationId = fullCs.LocationId,
                                             CarSegmentId = fullCs.CarSegmentId,
                                             CommercialCarSegmentId = fullCs.CommercialCarSegmentId,
                                             Percentage = newMinCommSegPercent,
                                             LastChangedOn = DateTime.Now,
                                             MarsUserId = marsUserId
                                         };

            
            var localEntities = from etc in entriesToCreate.AsEnumerable()
                    select new MinCommercialSegment
                       {
                           LocationId = etc.LocationId,
                           MinCommercialSegmentScenarioId = scenarioId,
                           CarSegmentId = etc.CarSegmentId,
                           CommercialCarSegmentId = etc.CommercialCarSegmentId,
                           Percentage = newMinCommSegPercent,
                           UpdatedOn = DateTime.Now,
                           MarsUserId = marsUserId
                       };
            DataContext.MinCommercialSegments.InsertAllOnSubmit(localEntities);
            DataContext.SubmitChanges();
        }

        public List<MinCommercialSegmentRow> GetMinCommercialSegments(int scenarioId)
        {
            var comSegData = CommercialSegmentQueryable.GetCommercialCarSemgents(DataContext, Parameters);
            
            var locations = LocationQueryable.GetLocations(DataContext, Parameters);
            var carSegments = CarSegmentQueryable.GetCarSegments(DataContext, Parameters);


            var fullComSeg = from ccs in comSegData
                             from l in locations
                             from cs in carSegments
                             select new
                             {
                                 LocationId = l.dim_Location_id,
                                 CarSegmentId = cs.car_segment_id,
                                 ccs.CommercialCarSegmentId
                             };

            var minComSegments =
                DataContext.MinCommercialSegments.Where(d => d.MinCommercialSegmentScenarioId == scenarioId);

            var rows = from fullCs in fullComSeg
                       join mcs in minComSegments on new { fullCs.LocationId, fullCs.CarSegmentId, fullCs.CommercialCarSegmentId }
                                                equals new { mcs.LocationId, mcs.CarSegmentId, mcs.CommercialCarSegmentId }
                        into joinedComSeg
                        from comSegFigures in joinedComSeg.DefaultIfEmpty()
                        join l in DataContext.LOCATIONs on fullCs.LocationId equals l.dim_Location_id
                        join cs in DataContext.CAR_SEGMENTs on fullCs.CarSegmentId equals cs.car_segment_id
                        join ccs in DataContext.CommercialCarSegments on fullCs.CommercialCarSegmentId equals ccs.CommercialCarSegmentId
                        
                select new MinCommercialSegmentRow
                       {
                           CarSegment = cs.car_segment1,
                           CommercialSegment = ccs.Name,
                           Country = cs.country,
                           LastChangedBy = comSegFigures == null ? string.Empty : comSegFigures.MarsUser.EmployeeId,
                           LastChangedOn = comSegFigures == null ? (DateTime?) null: comSegFigures.UpdatedOn,
                           Location = l.location1,
                           LocationGroup = l.CMS_LOCATION_GROUP.cms_location_group1,
                           Percentage = comSegFigures == null ? (double?)null : comSegFigures.Percentage,
                           Pool = l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                       };
            var returned = rows.ToList();
            return returned;
        }
    }
}