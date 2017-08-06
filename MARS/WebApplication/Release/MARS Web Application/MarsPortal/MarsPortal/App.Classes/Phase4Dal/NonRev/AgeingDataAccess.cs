using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using App.BLL.ExtensionMethods;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.Classes.Phase4Dal.NonRev
{
    public class AgeingDataAccess : NonRevBaseDataAccess
    {
        public AgeingDataAccess(Dictionary<DictionaryParameter, string> parameters) : base (parameters)
        {
        }

        protected static AgeingRow BuildTotalAgeingRow(List<AgeingRow> ageData)
        {
            var totalRow = new AgeingRow
            {
                Key = TotalKeyName,
                FleetCount = ageData.Sum(d => d.FleetCount),
                NonRevCount = ageData.Sum(d => d.NonRevCount),
                Group1 = ageData.Sum(d => d.Group1),
                Group2 = ageData.Sum(d => d.Group2),
                Group3 = ageData.Sum(d => d.Group3),
                Group4 = ageData.Sum(d => d.Group4),
                Group5 = ageData.Sum(d => d.Group5),
                Group6 = ageData.Sum(d => d.Group6),
                Group7 = ageData.Sum(d => d.Group7),
                Group8 = ageData.Sum(d => d.Group8),
                Group9 = ageData.Sum(d => d.Group9),
                PercentNonRevOfTotalNonRev = 1,
                PercentOfTotalFleet = ageData.Sum(d => d.PercentOfTotalFleet)
            };
            return totalRow;
        }


        public List<AgeingRow> GetAgeingEntries()
        {
            DictionaryParameter groupingType;
            if (Parameters.ContainsKey(DictionaryParameter.OperationalStatusGrouping)
                && Parameters[DictionaryParameter.OperationalStatusGrouping] == "True")
            {
                groupingType = DictionaryParameter.OperationalStatusGrouping;
            }
            else if (Parameters.ContainsKey(DictionaryParameter.KciGrouping) &&
                Parameters[DictionaryParameter.KciGrouping] == "True")
            {
                groupingType = DictionaryParameter.KciGrouping;
            }
            else throw new Exception("Invalid Grouping Type passed to GetComparisonByStatusEntries()");



            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);
            var ageingRow = startDate == DateTime.Now.Date
                ? GetCurrentAgeRowData(groupingType)
                : GetHistoricAgeRowData(groupingType);


            
            ageingRow.ForEach(d => d.AssignGroups());

            var totalNonRev = ageingRow.Sum(d => d.NonRevCount);
            var totalFleet = ageingRow.Sum(d => d.FleetCount);

            ageingRow.ForEach(d => d.CalculatePercentOfTotalNonRev(totalNonRev));
            ageingRow.ForEach(d => d.CalculatePercentOfTotalFleet(totalFleet));


            ageingRow.Add(BuildTotalAgeingRow(ageingRow));
            return ageingRow;
        }

        private List<AgeingRow> GetCurrentAgeRowData(DictionaryParameter comparisonType)
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);

            if(vehicles == null) return new List<AgeingRow>();


 
            IQueryable<IGrouping<string, Vehicle>> groupedData;
            switch (comparisonType)
            {
                case DictionaryParameter.OperationalStatusGrouping:
                    groupedData = from v in vehicles
                                  group v by v.Operational_Status.OperationalStatusCode
                                      into gd
                                      select gd;

                    break;

                case DictionaryParameter.KciGrouping:
                    groupedData = from v in vehicles
                                  group v by v.Operational_Status.KCICode
                                      into gd
                                      select gd;

                    break;

                default:
                    return null;
            }

            IEnumerable<int> selectedOperStats = null;
            if (Parameters.ContainsKey(DictionaryParameter.OperationalStatuses))
            {
                if (Parameters[DictionaryParameter.OperationalStatuses] == string.Empty) return null;
                selectedOperStats = Parameters[DictionaryParameter.OperationalStatuses].Split(',').Select(int.Parse);
            }

            IEnumerable<int> selectedMovementTypes = null;
            if (Parameters.ContainsKey(DictionaryParameter.MovementTypes))
            {
                if (Parameters[DictionaryParameter.MovementTypes] == string.Empty) return null;
                selectedMovementTypes = Parameters[DictionaryParameter.MovementTypes].Split(',').Select(int.Parse);
            }

            var compData = from gd in groupedData
                           select new AgeingRow
                           {
                               Key = gd.Key,
                               FleetCount = gd.Count(),
                               NonRevCount = gd.Sum(d => d.IsNonRev
                                                && (selectedOperStats == null || selectedOperStats.Contains(d.LastOperationalStatusId))
                                                && (selectedMovementTypes == null || selectedMovementTypes.Contains(d.LastMovementTypeId))
                                                        ? 1 : 0),
                               Ages = gd.Where(d => 
                                                d.IsNonRev
                                                && (selectedOperStats == null || selectedOperStats.Contains(d.LastOperationalStatusId))
                                                && (selectedMovementTypes == null || selectedMovementTypes.Contains(d.LastMovementTypeId))
                                            ).Select(d => d.DaysSinceLastRevenueMovement).ToList()
                           };
            var returned = compData.ToList();
            return returned;
        }


        private List<AgeingRow> GetHistoricAgeRowData(DictionaryParameter comparisonType)
        {
            var vehicleHistories = BaseVehicleDataAccess.GetVehicleHistoryQueryable(Parameters, DataContext, true, true);
            if(vehicleHistories == null) return new List<AgeingRow>();

            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);

            vehicleHistories = from hd in vehicleHistories
                               where hd.TimeStamp == startDate
                               select hd;

            IQueryable<IGrouping<string, VehicleHistory>> groupedData;
            switch (comparisonType)
            {
                case DictionaryParameter.OperationalStatusGrouping:
                    groupedData = from v in vehicleHistories
                                  group v by v.Operational_Status.OperationalStatusCode
                                      into gd
                                      select gd;

                    break;

                case DictionaryParameter.KciGrouping:
                    groupedData = from v in vehicleHistories
                                  group v by v.Operational_Status.KCICode
                                      into gd
                                      select gd;

                    break;

                default:
                    return null;
            }

            IEnumerable<int> selectedOperStats = null;
            if (Parameters.ContainsKey(DictionaryParameter.OperationalStatuses))
            {
                if (Parameters[DictionaryParameter.OperationalStatuses] == string.Empty) return null;
                selectedOperStats = Parameters[DictionaryParameter.OperationalStatuses].Split(',').Select(int.Parse);
            }

            IEnumerable<int> selectedMovementTypes = null;
            if (Parameters.ContainsKey(DictionaryParameter.MovementTypes))
            {
                if (Parameters[DictionaryParameter.MovementTypes] == string.Empty) return null;
                selectedMovementTypes = Parameters[DictionaryParameter.MovementTypes].Split(',').Select(int.Parse);
            }

            var compData = from gd in groupedData
                           select new AgeingRow
                           {
                               Key = gd.Key,
                               FleetCount = gd.Count(),
                               NonRevCount = gd.Sum(d => d.IsNonRev
                                                && (selectedOperStats == null || selectedOperStats.Contains(d.OperationalStatusId))
                                                && (selectedMovementTypes == null || selectedMovementTypes.Contains(d.MovementTypeId))
                                                ? 1 : 0),
                               Ages = gd.Where(d => d.IsNonRev
                                                && (selectedOperStats == null || selectedOperStats.Contains(d.OperationalStatusId))
                                                && (selectedMovementTypes == null || selectedMovementTypes.Contains(d.MovementTypeId))
                                                ).Select(d=> d.DaysNonRev).ToList()
                           };
            var returned = compData.ToList();
            return returned;
        }
    }
}