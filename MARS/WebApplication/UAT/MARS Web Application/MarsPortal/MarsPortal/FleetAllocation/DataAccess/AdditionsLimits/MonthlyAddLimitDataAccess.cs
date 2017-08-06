using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.AdditionsLimits
{
    public class MonthlyAddLimitDataAccess : BaseDataAccess
    {

        public void UpdateWeeklyLimit(int weeklyLimitId, int additions)
        {
            var dbWeeklyDbEnitiy =
                DataContext.WeeklyLimitOnCarSegments.Single(d => d.WeeklyLimitOnCarSegmentId == weeklyLimitId);
            dbWeeklyDbEnitiy.Additions = additions;
            DataContext.SubmitChanges();
        }

        public List<WeeklyLimitRow> GetWeekLyLimitRows(int? carSegmentId, DateTime currentDate)
        {
            var weeklyLimits = from wd in DataContext.WeeklyLimitOnCarSegments
                               
                                select wd;

            if (carSegmentId.HasValue)
            {
                weeklyLimits = weeklyLimits.Where(d => d.CarSegmentId == carSegmentId);
            }

            var weeklyLimitData = from wd in weeklyLimits
                                  join wtm in DataContext.IsoWeekToMonths on
                                        new { wd.Year, wd.Week }
                                  equals new { wtm.Year, Week = wtm.IsoWeekNumber }
                                  where wtm.MonthDate >= currentDate
                                  select new WeeklyLimitRow
                                  {
                                      Year = wd.Year,
                                      Week = wd.Week,
                                      Month = wtm.Month,
                                      CarSegmentId = wd.CarSegmentId,
                                      CarSegmentName = wd.CAR_SEGMENT.car_segment1,
                                      AdditionsLimit = wd.Additions
                                  };
        
            var returned = weeklyLimitData.ToList();
            return returned;
        }

        public List<MonthlyLimitRow> GetMonthlyLimitRows(int? carSegementId, DateTime currentDate)
        {
            var monthlyLimits = from md in DataContext.MonthlyLimitOnCarGroups
                                select md;
            if (carSegementId.HasValue)
            {
                monthlyLimits = monthlyLimits.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegementId);
            }

            var monthlyLimitData = from md in monthlyLimits
                                   where md.MonthDate >= currentDate
                                   select new MonthlyLimitRow
                                   {
                                       Year = md.Year,
                                       Month = md.Month,
                                       CarGroupId = md.CarGroupId,
                                       CarSegmentName = md.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1,
                                       CarGroupName = md.CAR_GROUP.car_group1,
                                       AdditionsLimit = md.Additions,
                                   };

            var returned = monthlyLimitData.ToList();
            return returned;
        }

        public WeeklyLimitOnCarSegment GetWeeklyLimitEntiy(int weeklyLimitId)
        {
            var returned = DataContext.WeeklyLimitOnCarSegments.Single(d => d.WeeklyLimitOnCarSegmentId == weeklyLimitId);
            return returned;
        }
       

        /// <summary>
        /// Gets weekly Limits for a month, and inserts any missing entries
        /// </summary>
        public List<WeeklyEditRow> GetWeekLyLimits(DateTime targetMonth, int carSegmentId)
        {
            var expectedEntries = from wtm in DataContext.IsoWeekToMonths
                                         where wtm.Year == targetMonth.Year 
                                                && wtm.Month == targetMonth.Month
                                          select new
                                                 {
                                                     wtm.Year, 
                                                     wtm.IsoWeekNumber,
                                                     CarSegmentId = carSegmentId
                                                 };

            var actualEntries = from wd in DataContext.WeeklyLimitOnCarSegments
                                join wtm in DataContext.IsoWeekToMonths on
                                    new {wd.Year, wd.Week}
                                    equals new {wtm.Year, Week = wtm.IsoWeekNumber}
                                where wtm.Year == targetMonth.Year && wtm.Month == targetMonth.Month
                                        && wd.CarSegmentId == carSegmentId
                                select new
                                       {
                                           wtm.Year,
                                           wtm.IsoWeekNumber,
                                           wd.CarSegmentId
                                       };

            var missingEntries = expectedEntries.Except(actualEntries).ToList();


            var weeklyLimitsToInsert = from me in missingEntries
                select new WeeklyLimitOnCarSegment
                       {
                           Additions = 0,
                           CarSegmentId = me.CarSegmentId,
                           Year = me.Year,
                           Week = me.IsoWeekNumber,
                           FileUploadId = 1
                       };

            DataContext.WeeklyLimitOnCarSegments.InsertAllOnSubmit(weeklyLimitsToInsert);
            DataContext.SubmitChanges();
            

            var weeklyLimitData = from wd in DataContext.WeeklyLimitOnCarSegments
                                  join wtm in DataContext.IsoWeekToMonths on
                                        new { wd.Year, wd.Week }
                                  equals new { wtm.Year, Week = wtm.IsoWeekNumber }
                                  where wtm.Year == targetMonth.Year && wtm.Month == targetMonth.Month
                                    && wd.CarSegmentId == carSegmentId
                                  select new WeeklyEditRow
                                  {
                                      Year = wd.Year,
                                      Week = wd.Week,
                                      Month = wtm.Month,
                                      CarSegmentId = wd.CarSegmentId,
                                      CarSegmentName = wd.CAR_SEGMENT.car_segment1,
                                      AdditionsLimit = wd.Additions,
                                      WeeklyLimitOnCarSegmentId = wd.WeeklyLimitOnCarSegmentId
                                  };
            var returned = weeklyLimitData.ToList();
            return returned;
        }

        public List<MonthlyLimitRow> GetMonthlyLimits(DateTime targetMonth, int carSegmentId)
        {
            var monthlyLimitData = from md in DataContext.MonthlyLimitOnCarGroups
                                   where md.MonthDate == targetMonth
                                        && md.Additions > 0
                                        && md.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId
                                   select new MonthlyLimitRow
                                   {
                                       Year = md.Year,
                                       Month = md.Month,
                                       CarGroupId = md.CarGroupId,
                                       CarSegmentName = md.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1,
                                       CarGroupName = md.CAR_GROUP.car_group1,
                                       AdditionsLimit = md.Additions,
                                   };

            var returned = monthlyLimitData.ToList();
            return returned;
        }


        public List<MonthlyLimitOnCarGroup> MatchUploadToDatabaseEntities(List<MonthlyLimitUploadRow> uploadedData)
        {
            var returned = new List<MonthlyLimitOnCarGroup>();
            foreach (var ud in uploadedData)
            {
                var stringCarGroup = ud.CarGroup;
                var carGroup =
                    DataContext.CAR_GROUPs.FirstOrDefault(
                        d => d.car_group1 == stringCarGroup && d.CAR_CLASS.CAR_SEGMENT.country == "GE");
                if (carGroup == null) continue;
                var newDbEnity = new MonthlyLimitOnCarGroup
                                 {
                                     MonthDate = new DateTime(ud.Year, ud.Month, 1),
                                     Month = (byte)ud.Month,
                                     Year = (short)ud.Year,
                                     FileUploadId = 1,
                                     CarGroupId = carGroup.car_group_id,
                                     Additions = ud.Additions
                                 };
                returned.Add(newDbEnity);
            }


            return returned;
        }

        public void UploadDatabaseEntities(List<MonthlyLimitOnCarGroup> entitiesToUpload)
        {
            var minDate = entitiesToUpload.Min(d => d.MonthDate);
            var entitiesToDelete = DataContext.MonthlyLimitOnCarGroups.Where(d => d.MonthDate >= minDate);
            DataContext.MonthlyLimitOnCarGroups.DeleteAllOnSubmit(entitiesToDelete);
            DataContext.SubmitChanges();

            DataContext.MonthlyLimitOnCarGroups.InsertAllOnSubmit(entitiesToUpload);
            DataContext.SubmitChanges();
        }
    }
}