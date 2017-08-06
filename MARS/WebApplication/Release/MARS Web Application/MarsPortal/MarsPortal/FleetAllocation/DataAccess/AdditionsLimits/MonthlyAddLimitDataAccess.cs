using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;

namespace Mars.FleetAllocation.DataAccess.AdditionsLimits
{
    public class MonthlyAddLimitDataAccess : BaseDataAccess
    {
        public List<MonthlyFileLimitRow> GetMonthlyAdditionsFileRows()
        {
            var fileData = from fu in DataContext.FileUploads
                           where fu.FileUploadTypeId == 1
                select new MonthlyFileLimitRow
                       {
                           Country = fu.COUNTRy.country_description,
                           DateUploaded = fu.CreatedDate,
                           UploadedBy = fu.FileUploadedBy,
                           FileName = fu.FileName,
                           ViewParameterId = fu.FileUploadId
                       };

            var returned = fileData.ToList();
            return returned;
        }

        public List<WeeklyLimitRow> GetWeekLyLimitRows(int fileUploadId)
        {
            var weeklyLimitData = from wd in DataContext.WeeklyLimitOnCarSegments
                                  join wtm in DataContext.IsoWeekToMonths on
                                        new { wd.Year, wd.Week }
                                  equals new { wtm.Year, Week = wtm.IsoWeekNumber }
                                  where wd.FileUploadId == fileUploadId
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

        public List<MonthlyLimitRow> GetMonthlyLimitRows(int fileUploadId)
        {
            var monthlyLimitData = from md in DataContext.MonthlyLimitOnCarGroups
                                   where md.FileUploadId == fileUploadId
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
    }
}