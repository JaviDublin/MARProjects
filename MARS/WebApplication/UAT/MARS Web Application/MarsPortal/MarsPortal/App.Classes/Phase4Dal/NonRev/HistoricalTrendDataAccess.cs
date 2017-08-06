using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.ExtensionMethods;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.Classes.Phase4Dal.NonRev
{
    public class HistoricalTrendDataAccess : NonRevBaseDataAccess
    {
        public HistoricalTrendDataAccess(Dictionary<DictionaryParameter, string> parameters)
            : base(parameters)
        {
        }

        private IQueryable<VehicleHistory> GetHistories(bool includeRevVehicles)
        {
            var vehicleHistories = BaseVehicleDataAccess.GetVehicleHistoryQueryable(Parameters, DataContext, includeRevVehicles, true);
            if (vehicleHistories == null) return null;

            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);
            var endDate = Parameters.GetDateFromDictionary(DictionaryParameter.EndDate);


            vehicleHistories = from hd in vehicleHistories
                               where hd.TimeStamp >= startDate
                                && hd.TimeStamp <= endDate
                               select hd;
            return vehicleHistories;
        }

        public List<ComparisonRow> GetHistoryEntries()
        {
            var vehicleHistories = GetHistories(true);
            if (vehicleHistories == null) return new List<ComparisonRow>();

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

            int minDays = 0;
            if (Parameters.ContainsKey(DictionaryParameter.MinDaysNonRev)
                && !string.IsNullOrEmpty(Parameters[DictionaryParameter.MinDaysNonRev]))
            {
                minDays = int.Parse(Parameters[DictionaryParameter.MinDaysNonRev]);
            }

            var totalHistory = from vh in vehicleHistories
                               group vh by vh.TimeStamp
                                   into gd
                                   orderby gd.Key descending 
                                   select new ComparisonRow
                                          {
                                              Key = gd.Key.ToShortDateString(), 
                                              NonRevCount = gd.Count(d => 
                                                            d.IsNonRev
                                                            && (selectedOperStats == null || selectedOperStats.Contains(d.OperationalStatusId))
                                                            && (selectedMovementTypes == null || selectedMovementTypes.Contains(d.MovementTypeId))
                                                            && d.DaysNonRev >= minDays
                                                            ), 
                                              FleetCount = gd.Count(), 
                                              ReasonsEntered = gd.Count(d => 
                                                   (selectedOperStats == null || selectedOperStats.Contains(d.OperationalStatusId))
                                                  && (selectedMovementTypes == null || selectedMovementTypes.Contains(d.MovementTypeId))
                                                  && d.DaysNonRev >= minDays
                                                  && d.RemarkId != null)
                                          };
            var returned = totalHistory.ToList();
            return returned;
        }


        public List<HistoricalTrendRow> GetHistoricalTrendEntries()
        {
            DataContext.Log = new DebugTextWriter();
            var vehicleHistories = GetHistories(false);
            if(vehicleHistories == null) return new List<HistoricalTrendRow>();

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

            int minDays = 0;
            if (Parameters.ContainsKey(DictionaryParameter.MinDaysNonRev)
                && !string.IsNullOrEmpty(Parameters[DictionaryParameter.MinDaysNonRev]))
            {
                minDays = int.Parse(Parameters[DictionaryParameter.MinDaysNonRev]);
            }

            IQueryable<HistoricalTrendRow> groupedHistory;
            if (groupingType == DictionaryParameter.KciGrouping)
            {
                groupedHistory = from vh in vehicleHistories
                                 group vh by new { vh.Operational_Status.KCICode, vh.TimeStamp }
                                     into gd
                                     select new HistoricalTrendRow
                                            {
                                                Date = gd.Key.TimeStamp,
                                                ColumnCode = gd.Key.KCICode,
                                                CodeCount = gd.Count(d => d.DaysNonRev >= minDays)
                                            };
            }

            else
            {
                groupedHistory = from vh in vehicleHistories
                                 group vh by new { vh.Operational_Status.OperationalStatusCode, vh.TimeStamp }
                                     into gd
                                     select new HistoricalTrendRow
                                            {
                                                Date = gd.Key.TimeStamp,
                                                ColumnCode = gd.Key.OperationalStatusCode,
                                                CodeCount = gd.Count(d => d.DaysNonRev >= minDays)
                                            };
                
            }

            var localHistory = groupedHistory.ToList();

            var totalHistory = from lh in localHistory
                group lh by lh.Date
                into gd
                select new HistoricalTrendRow
                       {
                           CodeCount = gd.Sum(d => d.CodeCount)
                           , ColumnCode = "Total Non Rev"
                           , Date = gd.Key
                       };

            //var totalHistory = from vh in vehicleHistories
            //    group vh by vh.TimeStamp.Date
            //    into gd
            //    select new {Date = gd.Key
            //                , NonRevCount = gd.Count(d => d.IsNonRev && d.DaysNonRev >= minDays)
            //                };


            var returned = new List<HistoricalTrendRow>();
            //todo Change this to stop two accesses to the database

            
            //returned.AddRange(totalHistory.Select(t => new HistoricalTrendRow
            //                                           {
            //                                               CodeCount = t.NonRevCount
            //                                               , ColumnCode = "Total Non Rev"
            //                                               , Date = t.Date
                                                       
            //                                           }));

            returned.AddRange(totalHistory);


            returned.AddRange(localHistory);

            return returned;

        }

    }
}