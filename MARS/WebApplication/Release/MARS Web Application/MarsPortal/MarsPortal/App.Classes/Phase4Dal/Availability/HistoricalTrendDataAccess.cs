using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.ExtensionMethods;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Availability.Entities;

using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal.Availability
{
    public class HistoricalTrendDataAccess : AvailabilityDataAccess
    {
        public HistoricalTrendDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {
        }


        public List<FleetStatusRow> GetHistoricalTrend()
        {
            var availabilityKeyGrouping = BaseVehicleDataAccess.GetAvailabilityGroupingFromParameters(Parameters);

            var groupedQueryable = GetAvailabilityHistoryGroupedByDate();
            var fleetDayKeyGrouping = ExtractFleetHistoryColumns(availabilityKeyGrouping, groupedQueryable);
            var returned = fleetDayKeyGrouping.OrderBy(d => d.Day).ToList();
            return returned;
        }

        public List<FleetStatusRow> GetCurrentTrend()
        {
            var divisorType = BaseVehicleDataAccess.GetPercentageDivisorTypeFromParameters(Parameters);
            var todayAvailability = GetAvailabilityToday();

            var groupedData = from v in todayAvailability
                              group v by v.Timestamp
                                  into gq
                                  select new FleetStatusRow(divisorType, false)
                                          {
                                              Day = gq.Key,
                                              TotalFleet = gq.Sum(d => (int)d.SumTotal),
                                              Bd = gq.Sum(d => (int)d.SumBd),
                                              Cu = gq.Sum(d => (int)d.SumCu),
                                              Fs = gq.Sum(d => (int)d.SumFs),
                                              Ha = gq.Sum(d => (int)d.SumHa),
                                              Hl = gq.Sum(d => (int)d.SumHl),
                                              Ll = gq.Sum(d => (int)d.SumLl),
                                              Mm = gq.Sum(d => (int)d.SumMm),
                                              Nc = gq.Sum(d => (int)d.SumNc),
                                              Pl = gq.Sum(d => (int)d.SumPl),
                                              Rl = gq.Sum(d => (int)d.SumRl),
                                              Rp = gq.Sum(d => (int)d.SumRp),
                                              Idle = gq.Sum(d => (int)d.SumIdle),
                                              Su = gq.Sum(d => (int)d.SumSu),
                                              Sv = gq.Sum(d => (int)d.SumSv),
                                              Tb = gq.Sum(d => (int)d.SumTb),
                                              Tc = gq.Sum(d => (int)d.SumTc),
                                              Tn = gq.Sum(d => (int)d.SumTn),
                                              Tw = gq.Sum(d => (int)d.SumTw),
                                              Ws = gq.Sum(d => (int)d.SumWs),
                                              Overdue = gq.Sum(d => (int)d.SumOverdue),
                                          };;

            var returned = groupedData.OrderBy(d => d.Day).ToList();
            return returned;
        }
    }
}