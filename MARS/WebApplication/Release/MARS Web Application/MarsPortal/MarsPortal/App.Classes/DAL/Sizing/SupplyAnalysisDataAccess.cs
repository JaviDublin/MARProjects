using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using App.BLL.DynamicLinq;
using App.BLL.ExtensionMethods;
using App.BLL.ReportEnums.FleetSize;
using App.DAL.MarsDataAccess.CsvExportStaticData;
using Mars.App.Classes.DAL.MarsDBContext;
using App.DAL.MarsDataAccess.Sizing.FutureTrendDataHolders;
using App.DAL.MarsDataAccess.Sizing.SupplyAnalysisDataHolders;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using Mars.App.Classes.DAL.Sizing.GenericSizingDataAccess;
using Mars.DAL.Sizing;
using Rad.App.Classes.Common.Dates;


namespace App.DAL.MarsDataAccess.Sizing
{
    //Note the Raw data access used in this class is identical to the Future Trend Raw Data access, so they were merged

    internal static class SupplyAnalysisDataAccess
    {
        internal static List<GraphSeries> GetSupplyAnalysisData(Dictionary<string, string> parameters, FutureTrendDataType dataType, int scenarioId, DataType timeZone)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                if (timeZone == DataType.FrozenZone)
                    parameters[ParameterNames.ToDate] = FutureTrendDataAccess.SetFrozenZoneToDateLimit(DateTime.Parse(parameters[ParameterNames.ToDate]));

                IQueryable<FutureTrendOrSupplyAnalysisRawDataHolder> groupedData;
                if(timeZone == DataType.DailyChanging)
                {
                    //groupedData = new FutureTrendRepository().GetData(parameters, scenarioId);
                    groupedData = GenericSizingDataAccess.GetFutureTrendGroupyByDayData(dataContext, parameters, scenarioId);
                }
                else
                {
                    var joinedData = FutureTrendDataAccess.GetRawFrozenZoneFutureTrendData (dataContext, parameters, (byte)scenarioId);
                    groupedData = from fc in joinedData
                                  group fc by new { fc.ReportDate }
                                      into g
                                      orderby g.Key.ReportDate ascending
                                      select new FutureTrendOrSupplyAnalysisRawDataHolder {
                                          ReportDate=g.Key.ReportDate,
                                          Constrained = g.Sum(c => c.Constrained),
                                          Unconstrained = g.Sum(c => c.Unconstrained),
                                          Booked = g.Sum(c => c.Booked),
                                          NessesaryConstrained = g.Sum(c => c.NessesaryConstrained),
                                          NessesaryUnconstrained = g.Sum(c => c.NessesaryUnconstrained),
                                          NessesaryBooked = g.Sum(c => c.NessesaryBooked),
                                          Expected = g.Sum(c => c.Expected)
                                      };
                }

                

                IQueryable<SupplyAnalysisGraphDataHolder> data;
                switch (dataType)
                {
                    case FutureTrendDataType.Constrained:
                        data = from gd in groupedData
                               select new SupplyAnalysisGraphDataHolder
                                          {
                                              ReportDate = gd.ReportDate,
                                              Expected = gd.Expected - gd.NessesaryConstrained,
                                          };
                        break;
                    case FutureTrendDataType.Unconstrained:
                        data = from gd in groupedData
                               select new SupplyAnalysisGraphDataHolder
                                          {
                                              ReportDate = gd.ReportDate,
                                              Expected = gd.Expected - gd.NessesaryUnconstrained,
                                          };
                        break;
                    case FutureTrendDataType.AlreadyBooked:
                        data = from gd in groupedData
                               select new SupplyAnalysisGraphDataHolder
                                          {
                                              ReportDate = gd.ReportDate,
                                              Expected = gd.Expected - gd.NessesaryBooked,
                                          };
                        break;
                    default:
                        throw new ArgumentException("Invalid Future Trend Datatype selected");
                }
                var sw = new Stopwatch();
                sw.Start();
                var finalGraphingData = data.ToList();
                sw.Stop();
                var ell = sw.Elapsed;
                var graphData = GetSupplyAnalysisSeriesData(finalGraphingData);



                graphData.Add(GetSupplyAnalysisWeeklyData(finalGraphingData));

                return graphData;
            }
        }

        private static GraphSeries GetSupplyAnalysisWeeklyData(IEnumerable<SupplyAnalysisGraphDataHolder> supplyAnalysisDataFromDatabase)
        {
            var res = from sa in supplyAnalysisDataFromDatabase
                       group sa by sa.ReportDate.GetIso8061WeekOfYear()
                       into groupedData
                          select new SupplyAnalysisWeeklyGrouping
                                  {
                                      Expected = groupedData.Min(d => d.Expected),
                                      Week = groupedData.Key,
                                      Year = groupedData.First().ReportDate.Year
                                  };

            var saData = GetWeeklySupplyAnalysisSeriesData(res);
            return saData.First();
        }

        private static IEnumerable<GraphSeries> GetWeeklySupplyAnalysisSeriesData(IEnumerable<SupplyAnalysisWeeklyGrouping> weeklySupplyAnalysis)
        {
            var weeks = new List<object>();
            var expected = new List<double>();

            foreach (var ft in weeklySupplyAnalysis)
            {
                weeks.Add(ft.Week);
                expected.Add((int)ft.Expected);
            }

            var seriesInformation = new List<GraphSeries>()
                                            {
                                                new GraphSeries("Weekly Minimum Supply")
                                                    {
                                                        GraphColour = Color.Gold,
                                                        Xvalue = weeks,
                                                        Yvalue = expected
                                                    },
                                            };

            return seriesInformation;
        }

        private static List<GraphSeries> GetSupplyAnalysisSeriesData(IEnumerable<SupplyAnalysisGraphDataHolder> supplyAnalysisDataFromDatabase)
        {
            var dateTimes = new List<object>();
            var expected = new List<double>();

            foreach (var ft in supplyAnalysisDataFromDatabase)
            {
                dateTimes.Add(ft.ReportDate);
                expected.Add((int)ft.Expected);
            }

            var seriesInformation = new List<GraphSeries>()
                                            {
                                                new GraphSeries("Supply")
                                                    {
                                                        GraphColour = Color.Gold,
                                                        Xvalue = dateTimes,
                                                        Yvalue = expected
                                                    },
                                            };

            return seriesInformation;
        }

        internal static string GetSupplyAnalysisExcelData(Dictionary<string, string> parameters, FutureTrendDataType dataType,
                                                   int scenarioId, int siteGroup, int fleetGroup, DataType timeZone, bool weeklyGrouping)
        {
            var toDate = timeZone == DataType.FrozenZone
                             ? FutureTrendDataAccess.SetFrozenZoneToDateLimit(DateTime.Parse(parameters[ParameterNames.ToDate]))
                             : parameters[ParameterNames.ToDate];


            var newParameters = new Dictionary<string, string>
                                    {
                                        {ParameterNames.Country, parameters[ParameterNames.Country]},
                                        {ParameterNames.ToDate, toDate},
                                        {ParameterNames.FromDate, parameters[ParameterNames.FromDate]}
                                    };

            
                if(timeZone == DataType.DailyChanging)
                {
                    //return new FutureTrendRepository().GetSupplyAnalysisCsvData(newParameters, scenarioId, siteGroup, fleetGroup
                     //       , dataType, weeklyGrouping);
                    using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
                    {
                        var joinedData = timeZone == DataType.DailyChanging
                                             ? GenericSizingDataAccess.GetFutureTrendRawData(dataContext, parameters,
                                                                                             scenarioId)
                                             : FutureTrendDataAccess.GetRawFrozenZoneFutureTrendData(dataContext, newParameters,
                                                                               (byte) scenarioId);



                        return GenerateExcelDataFromRaw(dataContext, joinedData, siteGroup, fleetGroup, dataType, weeklyGrouping);
                    }
                }
                else
                {
                    using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
                    {
                        var joinedData = FutureTrendDataAccess.GetRawFrozenZoneFutureTrendData(dataContext,
                                                                                               newParameters,
                                                                                               (byte) scenarioId);
                        return GenerateExcelDataFromRaw(dataContext, joinedData, siteGroup, fleetGroup, dataType,
                                                        weeklyGrouping);
                     
                    }
                }

            return null;
        }

        private static IEnumerable<SupplyAnalysisExcelDataHolder> GetJoinedExcelRawData(MarsDBDataContext dataContext, IQueryable<FutureTrendOrSupplyAnalysisRawDataHolder> joinedData, FutureTrendDataType dataType)
        {

            var data = from jd in joinedData
                   join lg in dataContext.CMS_LOCATION_GROUPs on jd.LocationGroupId equals lg.cms_location_group_id
                   join cs in dataContext.CAR_GROUPs on jd.CarClassId equals cs.car_group_id
                   select new SupplyAnalysisExcelDataHolder
                   {
                       ReportDate = jd.ReportDate,
                       Week = jd.ReportDate.GetIso8061WeekOfYear(),
                       Year = jd.ReportDate.Year,
                       Country = lg.CMS_POOL.country,
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
                       ExpectedFleet = jd.Expected,
                       Difference = 0,
                       NessesaryFleet = dataType == FutureTrendDataType.Constrained 
                                    ? jd.NessesaryConstrained : 
                                        dataType == FutureTrendDataType.Unconstrained 
                                    ? jd.NessesaryUnconstrained 
                                    : jd.NessesaryBooked
                   };

            return data;
        }

        private static string GenerateExcelDataFromRaw(MarsDBDataContext dataContext, IQueryable<FutureTrendOrSupplyAnalysisRawDataHolder> joinedRawData,
                                                    int siteGroup, int fleetGroup, FutureTrendDataType dataType, bool weeklyGrouping)
        {
            var csvData = new StringBuilder();
            

            var fullDataSet = GetJoinedExcelRawData(dataContext, joinedRawData, dataType);


            csvData.Append(CsvExportMethods.GetExportHeaders(siteGroup, fleetGroup, weeklyGrouping));
            csvData.Append(weeklyGrouping
                               ? string.Format("{0}\n", CsvExportHeaders.MinWeeklyDifference)
                               : string.Format("{0},{1},{2}\n", CsvExportHeaders.NessesaryFleet,
                                               CsvExportHeaders.ExpectedFleet, CsvExportHeaders.Difference));


            if (weeklyGrouping)
            {
                fullDataSet = fullDataSet.OrderBy(d => d.Week);    
            }
            
            var groupedData = fullDataSet.GroupByMany(CsvExportMethods.GetGroupingColumns(siteGroup, fleetGroup, weeklyGrouping));
            var orderedData = groupedData.OrderBy(d => d.Key);

            var keyList = new List<string>();
            foreach (var gr in orderedData)
            {
                CheckSubGroup(gr, keyList, csvData, weeklyGrouping);
            }
            return csvData.ToString();
        }



        private static void CheckSubGroup(GroupResult gr, ICollection<string> keyList, StringBuilder csvData, bool weeklyGrouping)
        {
            keyList.Add(gr.Key.ToString());
            if (gr.SubGroups == null)
            {
                var ftData = gr.Items.Cast<SupplyAnalysisExcelDataHolder>();
                string lineOfData;

                if(weeklyGrouping)
                {
                    var data = from gd in ftData.GroupBy(d => d.ReportDate)
                            select new
                                       {
                                           Date = gd.Key,
                                           //Value = gd.Sum(d => d.ExpectedFleet) - gd.Sum(d => d.NessesaryFleet)
                                           Value = Math.Round(gd.Sum(d => d.ExpectedFleet) - gd.Sum(d => d.NessesaryFleet), 0, MidpointRounding.AwayFromZero)
                                       };
                    
                    lineOfData = data.Min(d => d.Value) + "\n";
                }
                else
                {
                    decimal sumExpected = 0;
                    decimal sumNessFleet = 0;
                    decimal sumDifference = 0;
                    foreach (var d in ftData)
                    {
                        sumNessFleet += d.NessesaryFleet;
                        sumExpected += d.ExpectedFleet;
                        //sumDifference += d.ExpectedFleet - d.NessesaryFleet;
                    }

                    sumDifference = sumExpected - sumNessFleet;

                    lineOfData = string.Format("{0},{1},{2}\n",
                                            Math.Round(sumExpected, 0, MidpointRounding.AwayFromZero),
                                            Math.Round(sumNessFleet, 0, MidpointRounding.AwayFromZero),
                                            Math.Round(sumDifference, 0, MidpointRounding.AwayFromZero));
                }


                var listOfKeys = new StringBuilder();
                foreach (var key in keyList)
                {
                    listOfKeys.Append(key + ",");
                }
                csvData.Append(listOfKeys + lineOfData);
            }
            else
            {
                gr.SubGroups.ToList().ForEach(d => CheckSubGroup(d, keyList, csvData, weeklyGrouping));
            }

            keyList.Remove(gr.Key.ToString());
        }

       

    }
}