using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Parameters;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles
{
    public class HistoricalTrendDataAccess : BaseDataAccess
    {
        public HistoricalTrendDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {
            
        }

        public List<HistoricalTrendRow> GetHistoricAgeRowData()
        {
            var vehicleHistories = BaseVehicleDataAccess.GetVehicleHistoryQueryable(Parameters, DataContext, true, true);
            if (vehicleHistories == null) return new List<HistoricalTrendRow>();

            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);
            var endDate = Parameters.GetDateFromDictionary(DictionaryParameter.EndDate);

            var daysInCountry = 0;
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.MinDaysInCountry))
            {
                daysInCountry = int.Parse(Parameters[DictionaryParameter.MinDaysInCountry]);
            }
            
            vehicleHistories = from vh in vehicleHistories
                               where vh.TimeStamp >= startDate
                               && vh.TimeStamp <= endDate
                                && vh.IsFleet
                                && vh.DaysInCountry >= daysInCountry
                               select vh;

            vehicleHistories = VehicleFieldRestrictions.RestrictVehicleHistoryByIdleInForeignCountry(vehicleHistories);


            var comparisonType = ComparisonLevelLookup.GetSiteComparisonTypeFromParameters(Parameters);

            IQueryable<HistoricalTrendRow> historicalTrendData = null;
            switch (comparisonType)
            {
                case DictionaryParameter.LocationCountry:
                    historicalTrendData = from v in vehicleHistories
                                        group v by new {v.TimeStamp.Date, v.LOCATION.COUNTRy1.country_description}
                                      into gd
                                      select new HistoricalTrendRow
                                       {
                                           ColumnCode = gd.Key.country_description,
                                           CodeCount = gd.Count(),
                                           Date = gd.Key.Date
                                       };
                    break;
                case DictionaryParameter.Pool:
                    historicalTrendData = from v in vehicleHistories
                                        group v by new {v.TimeStamp.Date, v.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1}
                                      into gd
                                      select new HistoricalTrendRow
                                       {
                                           ColumnCode = gd.Key.cms_pool1,
                                           CodeCount = gd.Count(),
                                           Date = gd.Key.Date
                                       };
                                      
                    break;
                case DictionaryParameter.LocationGroup:
                    historicalTrendData = from v in vehicleHistories
                                      group v by new {v.TimeStamp.Date, v.LOCATION.CMS_LOCATION_GROUP.cms_location_group1}
                                      into gd
                                      select new HistoricalTrendRow
                                       {
                                           ColumnCode = gd.Key.cms_location_group1,
                                           CodeCount = gd.Count(),
                                           Date = gd.Key.Date
                                       };
                    break;
                case DictionaryParameter.Area:
                    historicalTrendData = from v in vehicleHistories
                                    group v by new {v.TimeStamp.Date, v.LOCATION.OPS_AREA.ops_area1}
                                      into gd
                                      select new HistoricalTrendRow
                                       {
                                           ColumnCode = gd.Key.ops_area1,
                                           CodeCount = gd.Count(),
                                           Date = gd.Key.Date
                                       };
                    break;
                case DictionaryParameter.Region:
                    historicalTrendData = from v in vehicleHistories
                                        group v by new {v.TimeStamp.Date,  v.LOCATION.OPS_AREA.OPS_REGION.ops_region1}
                                        into gd
                                        select new HistoricalTrendRow
                                        {
                                            ColumnCode = gd.Key.ops_region1,
                                            CodeCount = gd.Count(),
                                            Date = gd.Key.Date
                                        };
                    break;
                case DictionaryParameter.Location:
                    historicalTrendData = from v in vehicleHistories
                                          group v by new { v.TimeStamp.Date, v.LOCATION.location1 }
                                          into gd
                                        select new HistoricalTrendRow
                                        {
                                            ColumnCode = gd.Key.location1,
                                            CodeCount = gd.Count(),
                                            Date = gd.Key.Date
                                        };
                    break;
            }

            
            if(historicalTrendData == null) return new List<HistoricalTrendRow>();

            var returned = historicalTrendData.ToList();
            return returned;

        }
    }
}