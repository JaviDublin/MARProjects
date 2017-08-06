using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Parameters;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;


namespace Mars.App.Classes.Phase4Dal.ForeignVehicles
{
    public class AgeingDataAccess : BaseDataAccess
    {
        public AgeingDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {
            
        }

        public List<AgeingRow> GetAgeingEntries()
        {
            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);
            
            var ageingRow = startDate == DateTime.Now.Date ? GetTodayAgeRowData() : GetHistoricAgeRowData();
            
            ageingRow.ForEach(d => d.AssignGroups());

            if(ageingRow.Count > 0)
            {
                var totalRow = new AgeingRow { Key = "Total" };
                foreach (var ar in ageingRow)
                {
                    totalRow.FleetCount += ar.FleetCount;
                    totalRow.Group1 += ar.Group1;
                    totalRow.Group2 += ar.Group2;
                    totalRow.Group3 += ar.Group3;
                    totalRow.Group4 += ar.Group4;
                    totalRow.Group5 += ar.Group5;
                    totalRow.Group6 += ar.Group6;
                    totalRow.Group7 += ar.Group7;
                    totalRow.Group8 += ar.Group8;

                }
                ageingRow.Add(totalRow);
            }
            

            return ageingRow;
        }

        private List<AgeingRow> GetTodayAgeRowData()
        {
            
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);

            vehicles = VehicleFieldRestrictions.RestrictByIdleInForeignCountry(vehicles);
            


            var comparisonType = ComparisonLevelLookup.GetSiteComparisonTypeFromParameters(Parameters);

            IQueryable<IGrouping<string, Vehicle>> groupedData = null;
            switch (comparisonType)
            {
                case DictionaryParameter.LocationCountry:
                    groupedData = from v in vehicles
                                  group v by v.LOCATION.COUNTRy1.country_description
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Pool:
                    groupedData = from v in vehicles
                                  group v by v.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.LocationGroup:
                    groupedData = from v in vehicles
                                  group v by v.LOCATION.CMS_LOCATION_GROUP.cms_location_group1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Area:
                    groupedData = from v in vehicles
                                  group v by v.LOCATION.OPS_AREA.ops_area1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Region:
                    groupedData = from v in vehicles
                                  group v by v.LOCATION.OPS_AREA.OPS_REGION.ops_region1
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Location:
                    groupedData = from v in vehicles
                                  group v by v.LOCATION.location1
                                      into gd
                                      select gd;
                    break;
            }

            if (groupedData == null) return null;


            var compData = from gd in groupedData
                           select new AgeingRow
                           {
                               Key = gd.Key,
                               FleetCount = gd.Count(),
                               Ages = gd.Select(d => d.DaysInCountry).ToList()
                           };
            var returned = compData.ToList();
            return returned;
        }

        private List<AgeingRow> GetHistoricAgeRowData()
        {
            var vehicleHistories = BaseVehicleDataAccess.GetVehicleHistoryQueryable(Parameters, DataContext, true, true);
            if (vehicleHistories == null) return new List<AgeingRow>();

            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);

            vehicleHistories = VehicleFieldRestrictions.RestrictVehicleHistoryByIdleInForeignCountry(vehicleHistories);
            vehicleHistories = from vh in vehicleHistories
                               where vh.TimeStamp == startDate
                                && vh.IsFleet 
                               select vh;

            
            var comparisonType = ComparisonLevelLookup.GetSiteComparisonTypeFromParameters(Parameters);

            IQueryable<IGrouping<string, VehicleHistory>> groupedData = null;
            switch (comparisonType)
            {
                case DictionaryParameter.LocationCountry:
                    groupedData = from v in vehicleHistories
                                group v by v.LOCATION.COUNTRy1.country_description
                                into gd
                                select gd;
                    break;
                case DictionaryParameter.Pool:
                    groupedData = from v in vehicleHistories
                                group v by v.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                into gd
                                select gd;
                    break;
                case DictionaryParameter.LocationGroup:
                    groupedData = from v in vehicleHistories
                                  group v by v.LOCATION.CMS_LOCATION_GROUP.cms_location_group1
                                  into gd
                                  select gd;
                    break;
                case DictionaryParameter.Area:
                    groupedData = from v in vehicleHistories
                        group v by v.LOCATION.OPS_AREA.ops_area1
                        into gd
                        select gd;
                    break;
                case DictionaryParameter.Region:
                    groupedData = from v in vehicleHistories
                        group v by v.LOCATION.OPS_AREA.OPS_REGION.ops_region1
                        into gd
                        select gd;
                    break;
                case DictionaryParameter.Location:
                    groupedData = from v in vehicleHistories
                        group v by v.LOCATION.location1
                        into gd
                        select gd;
                    break;
            }

            if (groupedData == null) return null;


            var compData = from gd in groupedData
                           select new AgeingRow
                           {
                               Key = gd.Key,
                               FleetCount = gd.Count(),
                               Ages = gd.Select(d => d.DaysInCountry).ToList()
                           };
            var returned = compData.ToList();
            return returned;

        }



    }
}