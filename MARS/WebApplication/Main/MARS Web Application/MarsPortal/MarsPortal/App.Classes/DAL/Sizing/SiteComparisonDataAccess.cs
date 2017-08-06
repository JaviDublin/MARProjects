using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using App.DAL.MarsDataAccess.CsvExportStaticData;
using App.DAL.MarsDataAccess.ParameterAccess;
using Mars.App.Classes.DAL.MarsDBContext;
using App.DAL.MarsDataAccess.Sizing.SiteAndFleetComparisonDataHolders;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using Mars.App.Classes.DAL.Sizing.GenericSizingDataAccess;

namespace App.DAL.MarsDataAccess.Sizing
{
    internal static class SiteComparisonDataAccess
    {
        internal static List<GraphSeries> GetSiteComparisonData(Dictionary<string, string> parameters, int topicId, int scenarioId)
        {
            var country = parameters.ContainsKey(ParameterNames.Country) ? parameters[ParameterNames.Country] : null;
            var cmsPoolId = parameters.ContainsKey(ParameterNames.Pool) ? parameters[ParameterNames.Pool] : null;

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var joinedData = GenericSizingDataAccess.GetRawSiteAndFleetComparisonData(dataContext, parameters, scenarioId);
                //var joinedData = GetRawSiteAndFleetComparisonData(dataContext, parameters, scenarioId);

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

                if (!string.IsNullOrEmpty(cmsPoolId))
                {
                    var groupedByLocationGroup = from gd in groupedByLocationGroupAndCarClass
                                                group gd by gd.LocationGroup
                                                into g
                                                select new
                                                {
                                                    LocationGroup = g.Key,
                                                    ExpectedFleet = g.Sum(d => d.ExpectedFleet),
                                                    Constrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Constrained) / g.Sum(d => d.ExpectedFleet),
                                                    Unconstrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Unconstrained) / g.Sum(d => d.ExpectedFleet),
                                                    Booked = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Booked) / g.Sum(d => d.ExpectedFleet)
                                                };

                    returnedData = from rd in groupedByLocationGroup
                                   join lg in dataContext.CMS_LOCATION_GROUPs on rd.LocationGroup equals lg.cms_location_group_id
                                   select new SiteAndFleetComparisonDataHolder()
                                   {
                                       ColumnName = lg.cms_location_group1,
                                       Value =
                                              topicId == 1 ? rd.ExpectedFleet :
                                              topicId == 3 ? rd.Constrained :
                                              topicId == 4 ? rd.Unconstrained :
                                              topicId == 5 ? rd.Booked : 0
                                   };
                }
                else if (!string.IsNullOrEmpty(country))
                {

                    var groupedByPool = from gd in groupedByLocationGroupAndCarClass
                                        join lg in dataContext.CMS_LOCATION_GROUPs on gd.LocationGroup equals lg.cms_location_group_id
                                           group gd by lg.cms_pool_id
                                               into g
                                               select new
                                               {
                                                   Pool = g.Key,
                                                   ExpectedFleet = g.Sum(d => d.ExpectedFleet),
                                                   Constrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Constrained) / g.Sum(d => d.ExpectedFleet),
                                                   Unconstrained = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Unconstrained) / g.Sum(d => d.ExpectedFleet),
                                                   Booked = g.Sum(d => d.ExpectedFleet) == 0 ? 0 : g.Sum(d => d.Booked) / g.Sum(d => d.ExpectedFleet)
                                               };

                    returnedData = from rd in groupedByPool
                                   join p in dataContext.CMS_POOLs on rd.Pool equals p.cms_pool_id
                                   select new SiteAndFleetComparisonDataHolder()
                                   {
                                       ColumnName = p.cms_pool1,
                                       Value =
                                              topicId == 1 ? rd.ExpectedFleet :
                                              topicId == 3 ? rd.Constrained :
                                              topicId == 4 ? rd.Unconstrained :
                                              topicId == 5 ? rd.Booked : 0
                                   };
                }
                else
                {
                    var groupedByCountry = from gd in groupedByLocationGroupAndCarClass
                                           join lg in dataContext.CMS_LOCATION_GROUPs on gd.LocationGroup equals lg.cms_location_group_id
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
                                   select new SiteAndFleetComparisonDataHolder()
                                   {
                                       ColumnName = lg.country_description,
                                       Value =
                                              topicId == 1 ? rd.ExpectedFleet :
                                              topicId == 3 ? rd.Constrained  :
                                              topicId == 4 ? rd.Unconstrained :
                                              topicId == 5 ? rd.Booked : 0                                            
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
                                                        GraphColour = Color.LawnGreen,
                                                        Xvalue = columnNames,
                                                        Yvalue = columnValues
                                                    },
                                            };
                return seriesInformation;
            }
        }

        internal static string GetSiteComparisonExcelData(Dictionary<string, string> parameters, int siteGroup, int topicId, int scenarioId)
        {
            var csvData = new StringBuilder();
            var csvHeader = new StringBuilder();

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var joinedData = GenericSizingDataAccess.GetRawSiteAndFleetComparisonData(dataContext, parameters, scenarioId);

                var carDetails = new StringBuilder();
                csvHeader.Append(CsvExportMethods.GetExportHeaders(siteGroup, 0));
                
                if (!string.IsNullOrEmpty(parameters[ParameterNames.CarSegment]))
                {
                    carDetails.Append((from cc in dataContext.CAR_SEGMENTs
                                       where cc.car_segment_id == int.Parse(parameters[ParameterNames.CarSegment])
                                       select cc.car_segment1).First() + ",");
                }

                if (!string.IsNullOrEmpty(parameters[ParameterNames.CarClassGroup]))
                {
                    carDetails.Append((from cc in dataContext.CAR_CLASSes
                                       where cc.car_class_id == int.Parse(parameters[ParameterNames.CarClassGroup])
                                       select cc.car_class1).First() + ",");
                }

                if(!string.IsNullOrEmpty(parameters[ParameterNames.CarClass]))
                {
                    carDetails.Append((from cc in dataContext.CAR_GROUPs
                                    where cc.car_group_id == int.Parse(parameters[ParameterNames.CarClass])
                                      select cc.car_group1).First() + ",");
                }


                var fullDataSet = from gd in joinedData
                            join lg in dataContext.CMS_LOCATION_GROUPs on gd.LocationGroupId equals lg.cms_location_group_id
                            select new SiteAndFleetComparisonExcelDataHolder
                            {
                                ReportDate = gd.ReportDate,
                                CountryId = gd.Country,
                                CountryName = lg.CMS_POOL.COUNTRy1.country_description,
                                Pool = lg.CMS_POOL.cms_pool1,
                                PoolId = lg.cms_pool_id,
                                LocationGroupId = gd.LocationGroupId,
                                LocationGroup = lg.cms_location_group1,
                                ExpectedFleet = gd.ExpectedFleet,
                                Constrained = gd.Constrained,
                                Unconstrained = gd.Unconstrained,
                                Booked = gd.Booked,
                                AdditionalColumns = carDetails.ToString()
                            };

                IQueryable<string> data;
                switch (siteGroup)
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
                               group av by new { Country = av.CountryId, av.CountryName, av.ReportDate, av.PoolId, av.Pool } into gd
                               orderby gd.Key.ReportDate, gd.Key.Country, gd.Key.PoolId
                               select string.Format("{0},{1},{2},{3}{4}\n", gd.Key.ReportDate.ToShortDateString(),
                                                    gd.Key.CountryName, gd.Key.Pool, gd.First().AdditionalColumns,
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
                               group av by new { Country = av.CountryId, av.CountryName, av.ReportDate, av.PoolId, av.Pool, av.LocationGroupId, av.LocationGroup } into gd
                               orderby gd.Key.ReportDate, gd.Key.Country, gd.Key.PoolId, gd.Key.LocationGroupId
                               select string.Format("{0},{1},{2},{3},{4}{5}\n", gd.Key.ReportDate.ToShortDateString(),
                                                    gd.Key.CountryName, gd.Key.Pool, gd.Key.LocationGroup, gd.First().AdditionalColumns,
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

                if (!string.IsNullOrEmpty(parameters[ParameterNames.CarSegment])) csvHeader.Append(CsvExportHeaders.CarSegment + ",");
                if (!string.IsNullOrEmpty(parameters[ParameterNames.CarClassGroup])) csvHeader.Append(CsvExportHeaders.CarClassGroup + ",");
                if (!string.IsNullOrEmpty(parameters[ParameterNames.CarClass])) csvHeader.Append(CsvExportHeaders.CarClass + ",");


                switch(topicId)
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