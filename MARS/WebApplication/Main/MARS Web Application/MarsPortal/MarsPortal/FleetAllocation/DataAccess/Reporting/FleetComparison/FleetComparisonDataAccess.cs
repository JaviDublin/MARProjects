using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter.Xml;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess;
using Mars.FleetAllocation.DataAccess.Reporting.FleetComparison.Entities;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.Reporting.FleetComparison
{
    public class FleetComparisonDataAccess : BaseDataAccess
    {
        public FleetComparisonDataAccess(Dictionary<DictionaryParameter, string> parameters) : base(parameters)
        {
            
        }

        public List<FleetComparisonEntity> GetComparisonEntities(int additionPlanId, int weeksInFuture)
        {
            var additions = AdditionPlanEntryFilter.GetAdditionPlanEntries(DataContext, Parameters);

            var currentDate = DataContext.FleetHistories.Max(d => d.Timestamp);
            var futureTarget = currentDate.AddDays(weeksInFuture * 7);
            var targetWeek = DataContext.IsoWeekOfYears.First(d => d.Day == futureTarget).WeekOfYear;
            var targetYear = futureTarget.Year;


            additions = additions.Where(d => d.AdditionPlanId == additionPlanId
                            && ((d.Week <= targetWeek && d.Year == targetYear)
                              || d.Year < targetYear ));
            IQueryable<FleetComparisonEntity> comparisonEntites;



            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup)
                || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                comparisonEntites = from mmd in additions
                    group mmd by new {FleetGroup = mmd.CAR_GROUP.car_group1, mmd.Week, mmd.Year}
                    into groupedData
                             select new FleetComparisonEntity
                           {
                               FleetGroupName = groupedData.Key.FleetGroup,
                               Year = groupedData.Key.Year,
                               Week = groupedData.Key.Week,
                               Additions = groupedData.Sum(d=> d.Additions),
                           };
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                comparisonEntites = from mmd in additions
                                    group mmd by new { FleetGroup = mmd.CAR_GROUP.CAR_CLASS.car_class1, mmd.Week, mmd.Year }
                                        into groupedData
                                        select new FleetComparisonEntity
                                        {
                                            FleetGroupName = groupedData.Key.FleetGroup,
                                            Year = groupedData.Key.Year,
                                            Week = groupedData.Key.Week,
                                            Additions = groupedData.Sum(d => d.Additions),
                                        };
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                comparisonEntites = from mmd in additions
                                    group mmd by new { FleetGroup = mmd.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1, mmd.Week, mmd.Year }
                                        into groupedData
                                        select new FleetComparisonEntity
                                        {
                                            FleetGroupName = groupedData.Key.FleetGroup,
                                            Year = groupedData.Key.Year,
                                            Week = groupedData.Key.Week,
                                            Additions = groupedData.Sum(d => d.Additions),
                                        };
            }
            else
            {
                comparisonEntites = from mmd in additions
                                    group mmd by new { FleetGroup = mmd.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description, mmd.Week, mmd.Year }
                                        into groupedData
                                        select new FleetComparisonEntity
                                        {
                                            FleetGroupName = groupedData.Key.FleetGroup,
                                            Year = groupedData.Key.Year,
                                            Week = groupedData.Key.Week,
                                            Additions = groupedData.Sum(d => d.Additions),
                                        };

            }

            var returned = comparisonEntites.ToList();
            return returned;
        }
    }
}