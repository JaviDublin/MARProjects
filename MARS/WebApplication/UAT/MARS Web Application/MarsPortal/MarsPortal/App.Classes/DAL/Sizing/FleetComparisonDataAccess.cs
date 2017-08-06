using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using App.DAL.MarsDataAccess.CsvExportStaticData;
using Mars.App.Classes.DAL.MarsDBContext;
using App.DAL.MarsDataAccess.Sizing.SiteAndFleetComparisonDataHolders;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using Mars.App.Classes.DAL.Sizing.GenericSizingDataAccess;

namespace App.DAL.MarsDataAccess.Sizing
{
    internal static class FleetComparisonDataAccess
    {
        internal static List<GraphSeries> GetFleetComarisonData(Dictionary<string, string> parameters, int topicId, int scenarioId)
        {
            var country = parameters.ContainsKey(ParameterNames.Country) ? parameters[ParameterNames.Country] : null;
            var carSegmentId = parameters.ContainsKey(ParameterNames.CarSegment) ? parameters[ParameterNames.CarSegment] : null;
            var carClassGroupId = parameters.ContainsKey(ParameterNames.CarClassGroup) ? parameters[ParameterNames.CarClassGroup] : null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                
                var joinedData = GenericSizingDataAccess.GetRawSiteAndFleetComparisonData(dataContext, parameters, scenarioId);

                
                var groupedByLocationGroupAndCarClass = from jd in joinedData
                                                            group jd by new { jd.CarClassId, jd.LocationGroupId, jd.Country, jd.ReportDate }
                                                            into g
                                                                select new
                                                                {
                                                                    g.Key.ReportDate,
                                                                    g.Key.Country,
                                                                    Constrained = g.Sum(d => d.Constrained),
                                                                    Unconstrained = g.Sum(d => d.Unconstrained),
                                                                    Booked = g.Sum(d => d.Booked),
                                                                    OperationalFleet = g.Sum(d => d.ExpectedFleet),
                                                                    CarClass = g.Key.CarClassId,
                                                                    LocationGroup = g.Key.LocationGroupId,
                                                                    ExpectedFleet = g.Sum(d => d.ExpectedFleet),
                                                                };

                groupedByLocationGroupAndCarClass = from jd in groupedByLocationGroupAndCarClass
                                                        group jd by new { jd.CarClass, jd.LocationGroup, jd.Country }
                                                            into g
                                                            select new
                                                            {
                                                                ReportDate = DateTime.Now,
                                                                Country = g.Key.Country,
                                                                Constrained = g.Average(d => d.Constrained),
                                                                Unconstrained = g.Average(d => d.Unconstrained),
                                                                Booked = g.Average(d => d.Booked),
                                                                OperationalFleet = g.Average(d => d.OperationalFleet),
                                                                CarClass = g.Key.CarClass,
                                                                LocationGroup = g.Key.LocationGroup,
                                                                ExpectedFleet = g.Average(d => d.ExpectedFleet),
                                                            };



                IQueryable<SiteAndFleetComparisonDataHolder> returnedData;

                if (!string.IsNullOrEmpty(carClassGroupId))
                {
                    var groupedByCarClass = from gd in groupedByLocationGroupAndCarClass
                                           group gd by gd.CarClass
                                               into g
                                               select new
                                               {
                                                   CarClass = g.Key,
                                                   ExpectedFleet = g.Sum(d => d.ExpectedFleet),
                                                   Constrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Constrained) / g.Sum(d => d.ExpectedFleet),
                                                   Unconstrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Unconstrained) / g.Sum(d => d.ExpectedFleet),
                                                   Booked = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Booked) / g.Sum(d => d.ExpectedFleet)
                                               };

                    returnedData = from rd in groupedByCarClass
                                   join cg in dataContext.CAR_GROUPs on rd.CarClass equals cg.car_group_id
                                   select new SiteAndFleetComparisonDataHolder
                                   {
                                       ColumnName = cg.car_group1,
                                       Value = (
                                              topicId == 1 ? rd.ExpectedFleet :
                                              topicId == 3 ? rd.Constrained :
                                              topicId == 4 ? rd.Unconstrained :
                                              topicId == 5 ? rd.Booked : 0)
                                   };

                } else if (!string.IsNullOrEmpty(carSegmentId))
                {
                    
                    var groupedByCarClassGroup = from gd in groupedByLocationGroupAndCarClass
                                                 join cg in dataContext.CAR_GROUPs on gd.CarClass equals cg.car_group_id
                                                   group gd by cg.car_class_id
                                                   into g
                                                   select new
                                                   {
                                                       CarClassGroup = g.Key,
                                                       ExpectedFleet = g.Sum(d => d.ExpectedFleet),
                                                       Constrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Constrained) / g.Sum(d => d.ExpectedFleet),
                                                       Unconstrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Unconstrained) / g.Sum(d => d.ExpectedFleet),
                                                       Booked = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Booked) / g.Sum(d => d.ExpectedFleet)
                                                   };
   
                    returnedData = from rd in groupedByCarClassGroup
                                   join ccg in dataContext.CAR_CLASSes on rd.CarClassGroup equals ccg.car_class_id
                                   select new SiteAndFleetComparisonDataHolder
                                   {
                                       ColumnName = ccg.car_class1,
                                       Value = (
                                              topicId == 1 ? rd.ExpectedFleet :
                                              topicId == 3 ? rd.Constrained :
                                              topicId == 4 ? rd.Unconstrained :
                                              topicId == 5 ? rd.Booked : 0)
                                   };
 

                } else if (!string.IsNullOrEmpty(country))
                {
                    var groupedByCarSegment = from gd in groupedByLocationGroupAndCarClass
                                              join lg in dataContext.CAR_GROUPs on gd.CarClass equals lg.car_group_id
                                              group gd by lg.CAR_CLASS.car_segment_id
                                              into g
                                              select new
                                              {
                                                   CarSegment = g.Key,
                                                   ExpectedFleet = g.Sum(d => d.ExpectedFleet),
                                                   Constrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Constrained) / g.Sum(d => d.ExpectedFleet),
                                                   Unconstrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Unconstrained) / g.Sum(d => d.ExpectedFleet),
                                                   Booked = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Booked) / g.Sum(d => d.ExpectedFleet)
                                              };

                    returnedData = from rd in groupedByCarSegment
                                   join cs in dataContext.CAR_SEGMENTs on rd.CarSegment equals cs.car_segment_id
                                   select new SiteAndFleetComparisonDataHolder
                                   {
                                       ColumnName = cs.car_segment1,
                                       Value = (
                                              topicId == 1 ? rd.ExpectedFleet :
                                              topicId == 3 ? rd.Constrained :
                                              topicId == 4 ? rd.Unconstrained :
                                              topicId == 5 ? rd.Booked : 0)
                                   };
                }
                else
                {
                    var groupedByCountry = from gd in groupedByLocationGroupAndCarClass
                                           group gd by gd.Country
                                               into g
                                               select new
                                               {
                                                   Country = g.Key,
                                                   ExpectedFleet = g.Sum(d => d.ExpectedFleet),
                                                   Constrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Constrained) / g.Sum(d => d.ExpectedFleet),
                                                   Unconstrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Unconstrained) / g.Sum(d => d.ExpectedFleet),
                                                   Booked = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Booked) / g.Sum(d => d.ExpectedFleet)
                                               };

                    returnedData = from rd in groupedByCountry
                                   join lg in dataContext.COUNTRies on rd.Country equals lg.country1
                                   select new SiteAndFleetComparisonDataHolder
                                   {
                                       ColumnName = lg.country_description,
                                       Value = (
                                              topicId == 1 ? rd.ExpectedFleet :
                                              topicId == 3 ? rd.Constrained :
                                              topicId == 4 ? rd.Unconstrained :
                                              topicId == 5 ? rd.Booked : 0)
                                   };
                }

                var columnNames = new List<object>();
                var columnValues = new List<double>();

                foreach (var fcd in returnedData.OrderByDescending(d => d.Value))
                {
                    columnNames.Add(fcd.ColumnName);
                    columnValues.Add((double)fcd.Value);
                }

                var seriesInformation = new List<GraphSeries>
                                            {
                                                new GraphSeries("Fleet")
                                                    {
                                                        GraphColour = Color.PowderBlue,
                                                        Xvalue = columnNames,
                                                        Yvalue = columnValues
                                                    },
                                            };

                return seriesInformation;
            }
        }

        internal static string GetFleetComparisonExcelData(Dictionary<string, string> parameters, int fleetGroup, int topicId, int scenarioId)
        {
            var csvData = new StringBuilder();
            var csvHeader = new StringBuilder();

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var joinedData = GenericSizingDataAccess.GetRawSiteAndFleetComparisonData(dataContext, parameters, scenarioId);

                var locationDetails = new StringBuilder();
                csvHeader.Append(CsvExportMethods.GetExportHeaders(0, fleetGroup));

                if (!string.IsNullOrEmpty(parameters[ParameterNames.Pool]))
                {
                    locationDetails.Append((from cc in dataContext.CMS_POOLs
                                       where cc.cms_pool_id == int.Parse(parameters[ParameterNames.Pool])
                                       select cc.cms_pool1).First() + ",");
                }

                if (!string.IsNullOrEmpty(parameters[ParameterNames.LocationGroup]))
                {
                    locationDetails.Append((from cc in dataContext.CMS_LOCATION_GROUPs
                                       where cc.cms_location_group_id == int.Parse(parameters[ParameterNames.LocationGroup])
                                       select cc.cms_location_group1).First() + ",");
                }

                var fullDataSet = from gd in joinedData
                                  join cc in dataContext.CAR_GROUPs on gd.CarClassId equals cc.car_group_id
                                  select new SiteAndFleetComparisonExcelDataHolder
                                  {
                                      ReportDate = gd.ReportDate,
                                      CountryId = gd.Country,
                                      CountryName = cc.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description,
                                      CarSegment = cc.CAR_CLASS.CAR_SEGMENT.car_segment1,
                                      CarSegmentId = cc.CAR_CLASS.car_segment_id,
                                      CarClassGroup = cc.CAR_CLASS.car_class1,
                                      CarClassGroupId = cc.car_class_id,
                                      CarClass = cc.car_group1,
                                      CarClassId = cc.car_group_id,
                                      ExpectedFleet = gd.ExpectedFleet,                                      
                                      Constrained = gd.Constrained,
                                      Unconstrained = gd.Unconstrained,
                                      Booked = gd.Booked,
                                      AdditionalColumns = locationDetails.ToString()
                                  };

                IQueryable<string> data;
                switch (fleetGroup)
                {
                    case 0:
                        data = from av in fullDataSet
                               group av by new { Country = av.CountryId, av.CountryName, av.ReportDate } into gd
                               orderby gd.Key.ReportDate, gd.Key.Country
                               select string.Format("{0},{1},{2}{3}\n", gd.Key.ReportDate.ToShortDateString(),
                                                    gd.Key.CountryName, gd.First().AdditionalColumns,
                                                    topicId == 1 ? Math.Round(gd.Sum(d => d.ExpectedFleet), 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 3 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Constrained) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 4 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Unconstrained) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 5 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Booked) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() : "0");

                        break;
                    case 1:
                        data = from av in fullDataSet
                               group av by new { Country = av.CountryId, av.CountryName, av.ReportDate, av.CarSegmentId, av.CarSegment } into gd
                               orderby gd.Key.ReportDate, gd.Key.Country, gd.Key.CarSegmentId
                               select string.Format("{0},{1},{2},{3}{4}\n", gd.Key.ReportDate.ToShortDateString(),
                                                    gd.Key.CountryName, gd.Key.CarSegment, gd.First().AdditionalColumns,
                                                    topicId == 1 ? Math.Round(gd.Sum(d => d.ExpectedFleet), 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 3 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Constrained) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 4 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Unconstrained) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 5 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Booked) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() : "0");
                        break;
                    case 2:
                        data = from av in fullDataSet
                               group av by new { Country = av.CountryId, av.CountryName, av.ReportDate, av.CarSegmentId, av.CarSegment, av.CarClassGroupId, av.CarClassGroup } into gd
                               orderby gd.Key.ReportDate, gd.Key.Country, gd.Key.CarSegmentId, gd.Key.CarClassGroupId
                               select string.Format("{0},{1},{2},{3},{4}{5}\n", gd.Key.ReportDate.ToShortDateString(),
                                                    gd.Key.CountryName, gd.Key.CarSegment, gd.Key.CarClassGroup, gd.First().AdditionalColumns,
                                                    topicId == 1 ? Math.Round(gd.Sum(d => d.ExpectedFleet), 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 3 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Constrained) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 4 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Unconstrained) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 5 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Booked) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() : "0");

                        break;
                    case 3:
                        data = from av in fullDataSet
                               group av by new { Country = av.CountryId, av.CountryName, av.ReportDate, av.CarSegmentId, av.CarSegment, av.CarClassGroupId, av.CarClassGroup, av.CarClass, av.CarClassId } into gd
                               orderby gd.Key.ReportDate, gd.Key.Country, gd.Key.CarSegmentId, gd.Key.CarClassGroupId, gd.Key.CarClassId
                               select string.Format("{0},{1},{2},{3},{4},{5}{6}\n", gd.Key.ReportDate.ToShortDateString(),
                                                    gd.Key.CountryName, gd.Key.CarSegment, gd.Key.CarClassGroup, gd.Key.CarClass, gd.First().AdditionalColumns,
                                                    topicId == 1 ? Math.Round(gd.Sum(d => d.ExpectedFleet), 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 3 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Constrained) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 4 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Unconstrained) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() :
                                                    topicId == 5 ? gd.Sum(d => d.ExpectedFleet) == 0 ? "0" :
                                                                        Math.Round(gd.Sum(d => d.Booked) / gd.Sum(d => d.ExpectedFleet) * 100, 0, MidpointRounding.AwayFromZero).ToString() : "0");

                        break;
                    default:
                        throw new NotImplementedException("Invalid Site Group passed to GetSiteComparisonExcelData()");
                }

                if (!string.IsNullOrEmpty(parameters[ParameterNames.Pool])) csvHeader.Append(CsvExportHeaders.Pool + ",");
                if (!string.IsNullOrEmpty(parameters[ParameterNames.LocationGroup])) csvHeader.Append(CsvExportHeaders.LocationGroup + ",");
                
                switch (topicId)
                {
                    case 1:
                        csvHeader.Append(CsvExportHeaders.OperationalFleet);
                        break;
                    case 2:
                        csvHeader.Append(CsvExportHeaders.AvailableFleet);
                        break;
                    case 3:
                        csvHeader.Append(CsvExportHeaders.UltilizationConstrained);
                        break;
                    case 4:
                        csvHeader.Append(CsvExportHeaders.UltilizationUnonstrained);
                        break;
                    case 5:
                        csvHeader.Append(CsvExportHeaders.UltilizationAlreadyBooked);
                        break;
                    default:
                        throw new NotImplementedException("Invalid topicId passed to GetSiteComparisonDataAccess");
                }
                csvData.AppendLine(csvHeader.ToString());
                data.ToList().ForEach(d => csvData.Append(d.ToString()));
                return csvData.ToString();

            }
        }


    }
}