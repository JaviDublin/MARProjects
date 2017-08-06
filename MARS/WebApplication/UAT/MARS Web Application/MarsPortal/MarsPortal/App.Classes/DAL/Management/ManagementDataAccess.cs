using System;
using System.Collections.Generic;
using System.Linq;
using App.BLL.DynamicLinq;
using App.BLL.ExtensionMethods;
using System.Text;
using App.BLL.Utilities;
using App.DAL.MarsDataAccess.Sizing.FutureTrendDataHolders;
using Mars.App.Classes.DAL.MarsDBContext;
using App.DAL.MarsDataAccess.CsvExportStaticData;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.Entities.Graphing.Parameters;

namespace App.DAL.MarsDataAccess.Management
{
    public class ManagementDataAccess
    {
        internal static string GetAdditionDeletionPlanExcelData(string country, int scenarioId, bool isAddition, int siteGroup, int fleetGroup)
        {
            var newParameters = new Dictionary<string, string>
                                        {
                                            {ParameterNames.Country, country}
                                        };

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var joinedData = GetRawAdditionDeletionData(dataContext, newParameters, (byte)scenarioId, isAddition);
                return GenerateExcelDataFromRaw(dataContext, joinedData, siteGroup, fleetGroup, isAddition);
            }
        }

        internal static IQueryable<AdditionDeletionRawDataHolder> GetRawAdditionDeletionData(MarsDBDataContext dataContext, Dictionary<string, string> parameters, byte scenarioId, bool isAddition)
        {
            var country = parameters.ContainsKey(ParameterNames.Country) ? parameters[ParameterNames.Country] : null;

            var fullDataSet = from fc in dataContext.MARS_CMS_FleetPlanDetails
                              select fc;

            var restrictedData = AdditionDeletionParameterRestriction.RestrictForecastByParameters(country, scenarioId, fullDataSet, dataContext);

            var joinedData = from rd in restrictedData
                             select 
                             new AdditionDeletionRawDataHolder
                             {
                                 ReportDate = rd.targetDate,
                                 CalendarWeek = new Helper().GetWeekNumber(rd.targetDate),
                                 LocationGroupId = rd.cms_Location_Group_ID,
                                 CarClassId = rd.car_class_id,
                                 Country = rd.MARS_CMS_FleetPlanEntry.Country,
                                 Amount = isAddition ? rd.addition : rd.deletion,
                                 ScenarioID = scenarioId
                             };

            return joinedData;
        }

        private static string GenerateExcelDataFromRaw(MarsDBDataContext dataContext, IQueryable<AdditionDeletionRawDataHolder> joinedRawData,
                                                    int siteGroup, int fleetGroup, bool isAddition)
        {
            var csvData = new StringBuilder();

            var fullDataSet = GetJoinedExcelRawData(dataContext, joinedRawData);

            csvData.Append(CsvExportMethods.GetExportHeaders(siteGroup, fleetGroup, false, true));
            csvData.Append(string.Format("{0}\n", isAddition ? CsvExportHeaders.Addition : CsvExportHeaders.Deletion));

            var orderedData = fullDataSet.GroupByMany(CsvExportMethods.GetGroupingColumns(siteGroup, fleetGroup, false, true)).OrderBy(d => d.Key);

            var keyList = new List<string>();
            foreach (var gr in orderedData)
            {
                CheckSubGroup(gr, keyList, csvData);
            }
            return csvData.ToString();
        }

        private static void CheckSubGroup(GroupResult gr, ICollection<string> keyList, StringBuilder csvData)
        {
            keyList.Add(gr.Key.ToString());
            if (gr.SubGroups == null)
            {
                var ftData = gr.Items.Cast<AdditionDeletionExcelDataHolder>();

                var sumAmount = ftData.Sum(d => d.Amount);

                if (sumAmount > 0)
                {
                    var str = string.Format("{0}\n", Math.Round(sumAmount, 0, MidpointRounding.AwayFromZero));

                    var listOfKeys = new StringBuilder();
                    foreach (var key in keyList)
                    {
                        listOfKeys.Append(key + ",");
                    }
                    csvData.Append(listOfKeys + str);
                }
            }
            else
            {
                gr.SubGroups.ToList().ForEach(d => CheckSubGroup(d, keyList, csvData));
            }

            keyList.Remove(gr.Key.ToString());
        }
     
        private static IEnumerable<AdditionDeletionExcelDataHolder> GetJoinedExcelRawData(MarsDBDataContext dataContext, IQueryable<AdditionDeletionRawDataHolder> joinedData)
        {
            return from jd in joinedData
                   join lg in dataContext.CMS_LOCATION_GROUPs on jd.LocationGroupId equals lg.cms_location_group_id
                   join cs in dataContext.CAR_GROUPs on jd.CarClassId equals cs.car_group_id
                   select new AdditionDeletionExcelDataHolder
                   {
                       ReportDate = jd.ReportDate,
                       CalendarWeek = jd.CalendarWeek,
                       CountryId = lg.CMS_POOL.country,
                       CountryName = lg.CMS_POOL.COUNTRy1.country_description,
                       Pool = lg.CMS_POOL.cms_pool1,
                       PoolId = lg.cms_pool_id,
                       LocationGroupId = jd.LocationGroupId,
                       LocationGroup = lg.cms_location_group1,
                       CarSegment = cs.CAR_CLASS.CAR_SEGMENT.car_segment1,
                       CarSegmentId = cs.CAR_CLASS.car_segment_id,
                       CarClassGroup = cs.CAR_CLASS.car_class1,
                       CarClassGroupId = cs.car_class_id,
                       CarClass = cs.car_group1,
                       CarClassId = jd.CarClassId,
                       Amount = jd.Amount  
                   };
        }
    }
}