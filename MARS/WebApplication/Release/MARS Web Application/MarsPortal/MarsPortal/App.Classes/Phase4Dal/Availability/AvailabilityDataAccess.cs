using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using AjaxControlToolkit;
using App.BLL.Utilities;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.LicenceeRestriction;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal.Availability
{
    public class AvailabilityDataAccess : BaseDataAccess
    {
        public AvailabilityDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {
            
        }

        protected IQueryable<IGrouping<DateTime, FleetHistory>> GetAvailabilityHistoryGroupedByDate()
        {
            var avHistory = GetAvailabilityHistory();


            avHistory = FleetHistoryRestriction.RestrictVehicleQueryable(DataContext, avHistory);
           

            var groupedData = from v in avHistory
                                group v by v.Timestamp
                                    into gd
                                    select gd;

            var returned = groupedData;
            return returned;
        }

        protected IQueryable<FleetStatusRow> ExtractFleetHistoryColumns(AvailabilityGrouping columnRequested
                 , IQueryable<IGrouping<DateTime, FleetHistory>> groupedQueryable)
        {
            var divisorType = BaseVehicleDataAccess.GetPercentageDivisorTypeFromParameters(Parameters);

            IQueryable<FleetStatusRow> fleetDayKeyGrouping;
            switch (columnRequested)
            {
                case AvailabilityGrouping.Min:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Day = gq.Key,
                                              TotalFleet = gq.Sum(d => d.MinTotal),
                                              Bd = gq.Sum(d => d.MinBd),
                                              Cu = gq.Sum(d => d.MinCu),
                                              Fs = gq.Sum(d => d.MinFs),
                                              Ha = gq.Sum(d => d.MinHa),
                                              Hl = gq.Sum(d => d.MinHl),
                                              Ll = gq.Sum(d => d.MinLl),
                                              Mm = gq.Sum(d => d.MinMm),
                                              Nc = gq.Sum(d => d.MinNc),
                                              Pl = gq.Sum(d => d.MinPl),
                                              Rl = gq.Sum(d => d.MinRl),
                                              Rp = gq.Sum(d => d.MinRp),
                                              Idle = gq.Sum(d => d.MinIdle),
                                              Su = gq.Sum(d => d.MinSu),
                                              Sv = gq.Sum(d => d.MinSv),
                                              Tb = gq.Sum(d => d.MinTb),
                                              Tc = gq.Sum(d => d.MinTc),
                                              Tn = gq.Sum(d => d.MinTn),
                                              Tw = gq.Sum(d => d.MinTw),
                                              Ws = gq.Sum(d => d.MinWs),
                                              Overdue = gq.Sum(d => d.MinOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.MinOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.MinAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d=> d.MinOnRent)
                                          };
                    break;
                case AvailabilityGrouping.Max:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Day = gq.Key.Date,
                                              TotalFleet = gq.Sum(d => d.MaxTotal),
                                              Bd = gq.Sum(d => d.MaxBd),
                                              Cu = gq.Sum(d => d.MaxCu),
                                              Fs = gq.Sum(d => d.MaxFs),
                                              Ha = gq.Sum(d => d.MaxHa),
                                              Hl = gq.Sum(d => d.MaxHl),
                                              Ll = gq.Sum(d => d.MaxLl),
                                              Mm = gq.Sum(d => d.MaxMm),
                                              Nc = gq.Sum(d => d.MaxNc),
                                              Pl = gq.Sum(d => d.MaxPl),
                                              Rl = gq.Sum(d => d.MaxRl),
                                              Rp = gq.Sum(d => d.MaxRp),
                                              Idle = gq.Sum(d => d.MaxIdle),
                                              Su = gq.Sum(d => d.MaxSu),
                                              Sv = gq.Sum(d => d.MaxSv),
                                              Tb = gq.Sum(d => d.MaxTb),
                                              Tc = gq.Sum(d => d.MaxTc),
                                              Tn = gq.Sum(d => d.MaxTn),
                                              Tw = gq.Sum(d => d.MaxTw),
                                              Ws = gq.Sum(d => d.MaxWs),
                                              Overdue = gq.Sum(d => d.MaxOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.MaxOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.MaxAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d => d.MaxOnRent)
                                          };
                    break;
                case AvailabilityGrouping.Average:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Day = gq.Key,
                                              TotalFleet = gq.Sum(d => d.AvgTotal),
                                              Bd = gq.Sum(d => d.AvgBd),
                                              Cu = gq.Sum(d => d.AvgCu),
                                              Fs = gq.Sum(d => d.AvgFs),
                                              Ha = gq.Sum(d => d.AvgHa),
                                              Hl = gq.Sum(d => d.AvgHl),
                                              Ll = gq.Sum(d => d.AvgLl),
                                              Mm = gq.Sum(d => d.AvgMm),
                                              Nc = gq.Sum(d => d.AvgNc),
                                              Pl = gq.Sum(d => d.AvgPl),
                                              Rl = gq.Sum(d => d.AvgRl),
                                              Rp = gq.Sum(d => d.AvgRp),
                                              Idle = gq.Sum(d => d.AvgIdle),
                                              Su = gq.Sum(d => d.AvgSu),
                                              Sv = gq.Sum(d => d.AvgSv),
                                              Tb = gq.Sum(d => d.AvgTb),
                                              Tc = gq.Sum(d => d.AvgTc),
                                              Tn = gq.Sum(d => d.AvgTn),
                                              Tw = gq.Sum(d => d.AvgTw),
                                              Ws = gq.Sum(d => d.AvgWs),
                                              Overdue = gq.Sum(d => d.AvgOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.AvgOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.AvgAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d => d.AvgOnRent)
                                          };
                    break;
                case AvailabilityGrouping.Peak:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Day = gq.Key,
                                              TotalFleet = gq.Sum(d => d.PeakTotal),
                                              Bd = gq.Sum(d => d.PeakBd),
                                              Cu = gq.Sum(d => d.PeakCu),
                                              Fs = gq.Sum(d => d.PeakFs),
                                              Ha = gq.Sum(d => d.PeakHa),
                                              Hl = gq.Sum(d => d.PeakHl),
                                              Ll = gq.Sum(d => d.PeakLl),
                                              Mm = gq.Sum(d => d.PeakMm),
                                              Nc = gq.Sum(d => d.PeakNc),
                                              Pl = gq.Sum(d => d.PeakPl),
                                              Rl = gq.Sum(d => d.PeakRl),
                                              Rp = gq.Sum(d => d.PeakRp),
                                              Idle = gq.Sum(d => d.PeakIdle),
                                              Su = gq.Sum(d => d.PeakSu),
                                              Sv = gq.Sum(d => d.PeakSv),
                                              Tb = gq.Sum(d => d.PeakTb),
                                              Tc = gq.Sum(d => d.PeakTc),
                                              Tn = gq.Sum(d => d.PeakTn),
                                              Tw = gq.Sum(d => d.PeakTw),
                                              Ws = gq.Sum(d => d.PeakWs),
                                              Overdue = gq.Sum(d => d.PeakOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.PeakOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.PeakAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d => d.PeakOnRent)
                                          };
                    break;
                case AvailabilityGrouping.Trough:
                    fleetDayKeyGrouping = from gq in groupedQueryable
                                          select new FleetStatusRow(divisorType, true)
                                          {
                                              Day = gq.Key,
                                              TotalFleet = gq.Sum(d => d.TroughTotal),
                                              Bd = gq.Sum(d => d.TroughBd),
                                              Cu = gq.Sum(d => d.TroughCu),
                                              Fs = gq.Sum(d => d.TroughFs),
                                              Ha = gq.Sum(d => d.TroughHa),
                                              Hl = gq.Sum(d => d.TroughHl),
                                              Ll = gq.Sum(d => d.TroughLl),
                                              Mm = gq.Sum(d => d.TroughMm),
                                              Nc = gq.Sum(d => d.TroughNc),
                                              Pl = gq.Sum(d => d.TroughPl),
                                              Rl = gq.Sum(d => d.TroughRl),
                                              Rp = gq.Sum(d => d.TroughRp),
                                              Idle = gq.Sum(d => d.TroughIdle),
                                              Su = gq.Sum(d => d.TroughSu),
                                              Sv = gq.Sum(d => d.TroughSv),
                                              Tb = gq.Sum(d => d.TroughTb),
                                              Tc = gq.Sum(d => d.TroughTc),
                                              Tn = gq.Sum(d => d.TroughTn),
                                              Tw = gq.Sum(d => d.TroughTw),
                                              Ws = gq.Sum(d => d.TroughWs),
                                              Overdue = gq.Sum(d => d.TroughOverdue),
                                              OperationalFleetDatabaseValue = gq.Sum(d => d.TroughOperationalFleet),
                                              AvailableFleetDatabaseValue = gq.Sum(d => d.TroughAvailableFleet),
                                              OnRentDatabaseValue = gq.Sum(d => d.TroughOnRent)
                                          };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return fleetDayKeyGrouping;
        }

        protected FleetStatusRow GroupToSingle(IQueryable<FleetStatusRow> fleetDayKeyGrouping)
        {
            var divisorType = BaseVehicleDataAccess.GetPercentageDivisorTypeFromParameters(Parameters);

            var availabilityDayGrouping =
                BaseVehicleDataAccess.GetAvailabilityDayGroupingFromParameters(Parameters);

            var ld = fleetDayKeyGrouping.ToList();

            if (ld.Count == 0)
            {
                return new FleetStatusRow(divisorType, true);
            }

            var fsr = new FleetStatusRow(divisorType, true)
                          {
                              TotalFleet = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.TotalFleet)
                                                     : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.TotalFleet)
                                                     : ld.Min(d => d.TotalFleet),
                              Bd = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Bd)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Bd)
                                  : ld.Min(d => d.Bd),
                              Cu = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Cu)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Cu)
                                  : ld.Min(d => d.Cu),
                              Fs = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Fs)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Fs)
                                  : ld.Min(d => d.Fs),
                              Ha = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Ha)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Ha)
                                  : ld.Min(d => d.Ha),
                              Hl = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Hl)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Hl)
                                  : ld.Min(d => d.Hl),
                              Ll = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Ll)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Ll)
                                  : ld.Min(d => d.Ll),
                              Mm = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Mm)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Mm)
                                  : ld.Min(d => d.Mm),
                              Nc = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Nc)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Nc)
                                  : ld.Min(d => d.Nc),
                              Pl = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Pl)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Pl)
                                  : ld.Min(d => d.Pl),
                              Rl = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Rl)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Rl)
                                  : ld.Min(d => d.Rl),
                              Rp = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Rp)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Rp)
                                  : ld.Min(d => d.Rp),
                              Idle = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Idle)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Idle)
                                  : ld.Min(d => d.Idle),
                              Su = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Su)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Su)
                                  : ld.Min(d => d.Su),
                              Sv = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Sv)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Sv)
                                  : ld.Min(d => d.Sv),
                              Tb = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Tb)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Tb)
                                  : ld.Min(d => d.Tb),
                              Tc = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Tc)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Tc)
                                  : ld.Min(d => d.Tc),
                              Tn = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Tn)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Tn)
                                  : ld.Min(d => d.Tn),
                              Tw = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Tw)
                                 : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Tw)
                                 : ld.Min(d => d.Tw),
                              Ws = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Ws)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Ws)
                                  : ld.Min(d => d.Ws),
                              Overdue = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.Overdue)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.Overdue)
                                  : ld.Min(d => d.Overdue),
                              OperationalFleetDatabaseValue = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.OperationalFleetDatabaseValue)
                                  : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.OperationalFleetDatabaseValue)
                                  : ld.Min(d => d.OperationalFleetDatabaseValue),
                              AvailableFleetDatabaseValue = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.AvailableFleetDatabaseValue)
                                : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.AvailableFleetDatabaseValue)
                                : ld.Min(d => d.AvailableFleetDatabaseValue),
                              OnRentDatabaseValue = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)ld.Average(d => d.OnRentDatabaseValue)
                                    : availabilityDayGrouping == AvailabilityGrouping.Max ? ld.Max(d => d.OnRentDatabaseValue)
                                    : ld.Min(d => d.OnRentDatabaseValue),
                          };



            var returned = fsr;
            return returned;
        }

        protected IEnumerable<FleetStatusRow> GroupByKey(IQueryable<FleetStatusRow> fleetDayKeyGrouping)
        {
            var divisorType = BaseVehicleDataAccess.GetPercentageDivisorTypeFromParameters(Parameters);

            var availabilityDayGrouping =
                BaseVehicleDataAccess.GetAvailabilityDayGroupingFromParameters(Parameters);

            var fleetKeyGrouping = from fd in fleetDayKeyGrouping
                                   group fd by fd.Key
                                       into g
                                       select new FleetStatusRow(divisorType, false)
                                       {
                                           Key = g.Key,
                                           TotalFleet = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.TotalFleet)
                                                     : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.TotalFleet)
                                                     : g.Min(d => d.TotalFleet),
                                           Bd = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Bd)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Bd)
                                               : g.Min(d => d.Bd),
                                           Cu = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Cu)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Cu)
                                               : g.Min(d => d.Cu),
                                           Fs = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Fs)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Fs)
                                               : g.Min(d => d.Fs),
                                           Ha = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Ha)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Ha)
                                               : g.Min(d => d.Ha),
                                           Hl = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Hl)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Hl)
                                               : g.Min(d => d.Hl),
                                           Ll = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Ll)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Ll)
                                               : g.Min(d => d.Ll),
                                           Mm = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Mm)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Mm)
                                               : g.Min(d => d.Mm),
                                           Nc = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Nc)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Nc)
                                               : g.Min(d => d.Nc),
                                           Pl = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Pl)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Pl)
                                               : g.Min(d => d.Pl),
                                           Rl = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Rl)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Rl)
                                               : g.Min(d => d.Rl),
                                           Rp = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Rp)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Rp)
                                               : g.Min(d => d.Rp),
                                           Idle = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Idle)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Idle)
                                               : g.Min(d => d.Idle),
                                           Su = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Su)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Su)
                                               : g.Min(d => d.Su),
                                           Sv = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Sv)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Sv)
                                               : g.Min(d => d.Sv),
                                           Tb = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Tb)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Tb)
                                               : g.Min(d => d.Tb),
                                           Tc = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Tc)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Tc)
                                               : g.Min(d => d.Tc),
                                           Tn = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Tn)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Tn)
                                               : g.Min(d => d.Tn),
                                           Tw = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Tw)
                                              : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Tw)
                                              : g.Min(d => d.Tw),
                                           Ws = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Ws)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Ws)
                                               : g.Min(d => d.Ws),
                                           Overdue = availabilityDayGrouping == AvailabilityGrouping.Average ? (int)g.Average(d => d.Overdue)
                                               : availabilityDayGrouping == AvailabilityGrouping.Max ? g.Max(d => d.Overdue)
                                               : g.Min(d => d.Overdue),
                                       };

            var returned = fleetKeyGrouping;
            return returned;
        }

        protected IQueryable<FleetNow> GetAvailabilityToday()
        {
            var availability = DataContext.FleetNows.Select(d => d);

            availability = FleetNowRestriction.RestrictVehicleQueryable(DataContext, availability);
           
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.FleetTypes))
            {
                var selectedFleetTypes = Parameters[DictionaryParameter.FleetTypes].Split(',').Select(int.Parse);
                availability = availability.Where(d => selectedFleetTypes.Contains(d.FleetTypeId));
            }

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupId = int.Parse(Parameters[DictionaryParameter.CarGroup]);
                availability = from av in availability
                               where av.CAR_GROUP.car_group_id == carGroupId
                               select av;
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassId = int.Parse(Parameters[DictionaryParameter.CarClass]);

                availability = from av in availability
                               where av.CAR_GROUP.car_class_id == carClassId
                               select av;
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(Parameters[DictionaryParameter.CarSegment]);

                availability = from av in availability
                               where av.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId
                               select av;
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountry = Parameters[DictionaryParameter.OwningCountry];

                availability = from av in availability
                               where av.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == owningCountry
                               select av;
            }


            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var location = int.Parse(Parameters[DictionaryParameter.Location]);

                availability = from av in availability
                               where av.LOCATION.dim_Location_id == location
                               select av;
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)
                    || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
            {
                if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
                {
                    var locationGroupId = int.Parse(Parameters[DictionaryParameter.LocationGroup]);

                    availability = from av in availability
                                   where av.LOCATION.cms_location_group_id == locationGroupId
                                   select av;
                }
                else
                {
                    var areaId = int.Parse(Parameters[DictionaryParameter.Area]);

                    availability = from av in availability
                                   where av.LOCATION.ops_area_id == areaId
                                   select av;

                }
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool)
                || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
            {
                if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
                {
                    var poolId = int.Parse(Parameters[DictionaryParameter.Pool]);
  
                    availability = from av in availability
                                   where av.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId
                                   select av;
                }
                else
                {
                    var regionId = int.Parse(Parameters[DictionaryParameter.Region]);

                    availability = from av in availability
                                   where av.LOCATION.OPS_AREA.ops_region_id == regionId
                                   select av;
                }
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountry = Parameters[DictionaryParameter.LocationCountry];

                availability = from av in availability
                               where av.LOCATION.country == locationCountry
                               select av;
            }

            return availability;
        }

        public IQueryable<FleetHistory> GetAvailabilityHistory()
        {
            var availability = DataContext.FleetHistories.Select(d=> d);


            availability = FleetHistoryRestriction.RestrictVehicleQueryable(DataContext, availability);    
            
            
            

            var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);
            var endDate = Parameters.GetDateFromDictionary(DictionaryParameter.EndDate);
            availability = availability.Where(d => d.Timestamp.Date >= startDate);
            if(endDate != DateTime.MinValue)
            {
                availability = availability.Where(d => d.Timestamp.Date <= endDate);
            }

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.FleetTypes))
            {
                var selectedFleetTypes = Parameters[DictionaryParameter.FleetTypes].Split(',').Select(byte.Parse);
                availability = availability.Where(d => selectedFleetTypes.Contains(d.FleetTypeId));
            }

            if (Parameters.ContainsKey(DictionaryParameter.DayOfWeek) &&
                    Parameters[DictionaryParameter.DayOfWeek] != string.Empty)
            {
                DayOfWeek dowEntered;
                var success = Enum.TryParse(Parameters[DictionaryParameter.DayOfWeek], out dowEntered);
                if (!success) throw new InvalidCastException("Unable to case Day of Week");
                availability = availability.Where(d => d.Timestamp.DayOfWeek == dowEntered);
            }

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupId = int.Parse(Parameters[DictionaryParameter.CarGroup]);
                availability = availability.Where(d => d.CarGroupId == carGroupId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassId = int.Parse(Parameters[DictionaryParameter.CarClass]);
                availability = availability.Where(d => d.CAR_GROUP.car_class_id == carClassId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(Parameters[DictionaryParameter.CarSegment]);
                availability = availability.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountry = Parameters[DictionaryParameter.OwningCountry];
                availability = availability.Where(d => d.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == owningCountry);
            }


            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var location = int.Parse(Parameters[DictionaryParameter.Location]);
                availability = availability.Where(d => d.LOCATION.dim_Location_id == location);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)
                    || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
            {
                if(Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
                {
                    var locationGroupId = int.Parse(Parameters[DictionaryParameter.LocationGroup]);
                    availability = from av in availability
                        where av.LOCATION.cms_location_group_id == locationGroupId
                        select av;
                }
                else
                {
                    var areaId = int.Parse(Parameters[DictionaryParameter.Area]);
                    availability = from av in availability
                                    where av.LOCATION.ops_area_id == areaId
                                    select av;
                }
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool)
                || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
            {
                if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
                {
                    var poolId = int.Parse(Parameters[DictionaryParameter.Pool]);
                    availability = from av in availability
                                    where av.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId
                                    select av;
                }
                else
                {
                    var regionId = int.Parse(Parameters[DictionaryParameter.Region]);
                    availability = from av in availability
                                    where av.LOCATION.OPS_AREA.ops_region_id == regionId
                                    select av;
                }
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountry = Parameters[DictionaryParameter.LocationCountry];
                availability = from av in availability
                    where av.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountry
                    select av;
            }

            return availability;
        }

        protected IQueryable<IGrouping<StringDateGrouping, FleetHistory>> GetTwoKeyGroupedAvailabilityHistory(DictionaryParameter comparisonType)
        {
            var avHistory = GetAvailabilityHistory();

            IQueryable<IGrouping<StringDateGrouping, FleetHistory>> groupedData;

   
            switch (comparisonType)
            {
                case DictionaryParameter.LocationCountry:
                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.LOCATION.COUNTRy1.country_description, Date = v.Timestamp }
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Pool:

                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1, Date = v.Timestamp }
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.LocationGroup:

                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.LOCATION.CMS_LOCATION_GROUP.cms_location_group1, Date = v.Timestamp }
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Area:

                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.LOCATION.OPS_AREA.ops_area1, Date = v.Timestamp }
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Region:

                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.LOCATION.OPS_AREA.OPS_REGION.ops_region1, Date = v.Timestamp }
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.Location:

                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.LOCATION.location1, Date = v.Timestamp }
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.OwningCountry:

                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description, Date = v.Timestamp }
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarSegment:


                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1, Date = v.Timestamp }                                  
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarClass:
                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.CAR_GROUP.CAR_CLASS.car_class1, Date = v.Timestamp }                                  
                                      into gd
                                      select gd;
                    break;
                case DictionaryParameter.CarGroup:

                    groupedData = from v in avHistory
                                  group v by new StringDateGrouping { Key = v.CAR_GROUP.car_group1, Date = v.Timestamp }                                  
                                      into gd
                                      select gd;

                    break;

                default:
                    return null;
            }


            return groupedData;
            
        }

        //protected IQueryable<IGrouping<string, FleetHistory>> GetSingleKeyGroupedAvailabilityHistory(DictionaryParameter comparisonType)
        //{
        //    var avHistory = GetAvailabilityHistory();

        //    IQueryable<IGrouping<string, FleetHistory>> groupedData;


        //    switch (comparisonType)
        //    {
        //        case DictionaryParameter.LocationCountry:
        //            groupedData = from v in avHistory
        //                          group v by v.LOCATION.COUNTRy1.country_description
        //                              into gd
        //                              select gd;
        //            break;
        //        case DictionaryParameter.Pool:

        //            groupedData = from v in avHistory
        //                          group v by v.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
        //                              into gd
        //                              select gd;
        //            break;
        //        case DictionaryParameter.LocationGroup:

        //            groupedData = from v in avHistory
        //                          group v by v.LOCATION.CMS_LOCATION_GROUP.cms_location_group1
        //                              into gd
        //                              select gd;
        //            break;
        //        case DictionaryParameter.Area:

        //            groupedData = from v in avHistory
        //                          group v by v.LOCATION.OPS_AREA.ops_area1
        //                              into gd
        //                              select gd;
        //            break;
        //        case DictionaryParameter.Region:

        //            groupedData = from v in avHistory
        //                          group v by v.LOCATION.OPS_AREA.OPS_REGION.ops_region1
        //                              into gd
        //                              select gd;
        //            break;
        //        case DictionaryParameter.Location:

        //            groupedData = from v in avHistory
        //                          group v by v.LOCATION.location1
        //                              into gd
        //                              select gd;
        //            break;
        //        case DictionaryParameter.OwningCountry:

        //            groupedData = from v in avHistory
        //                          group v by v.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description
        //                              into gd
        //                              select gd;
        //            break;
        //        case DictionaryParameter.CarSegment:


        //            groupedData = from v in avHistory
        //                          group v by v.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1
        //                              into gd
        //                              select gd;
        //            break;
        //        case DictionaryParameter.CarClass:
        //            groupedData = from v in avHistory
        //                          group v by v.CAR_GROUP.CAR_CLASS.car_class1
        //                              into gd
        //                              select gd;
        //            break;
        //        case DictionaryParameter.CarGroup:

        //            groupedData = from v in avHistory
        //                          group v by v.CAR_GROUP.car_group1
        //                              into gd
        //                              select gd;

        //            break;

        //        default:
        //            return null;
        //    }


        //    return groupedData;

        //}
    }
}