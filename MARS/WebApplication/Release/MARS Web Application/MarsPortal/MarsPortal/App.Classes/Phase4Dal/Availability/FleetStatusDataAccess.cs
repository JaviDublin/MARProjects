using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using App.BLL.ExtensionMethods;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Availability.Entities;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;

namespace Mars.App.Classes.Phase4Dal.Availability
{
    public class FleetStatusDataAccess : AvailabilityDataAccess
    {
        public FleetStatusDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {
        }

        public FleetStatusRow GetFleetStatus()
        {
             var startDate = Parameters.GetDateFromDictionary(DictionaryParameter.StartDate);

            var returned = startDate == DateTime.Now.Date 
                            ? GetFleetCurrentStatusEntry() 
                            : GetFleetStatusHistoryEntry();
            return returned;

        }

        private FleetStatusRow GetFleetCurrentStatusEntry()
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);
            var dt = DateTime.Now.Date;
            if (!vehicles.Any())
            {
                return new FleetStatusRow(PercentageDivisorType.Values, false);
            }
            var returned = new FleetStatusRow(PercentageDivisorType.Values, false)
                           {
                               TotalFleet = vehicles.Count(),
                               Cu = vehicles.Sum(d => d.LastOperationalStatusId == 2 ? 1 : 0),
                               Ha = vehicles.Sum(d => d.LastOperationalStatusId == 4 ? 1 : 0),
                               Hl = vehicles.Sum(d => d.LastOperationalStatusId == 5 ? 1 : 0),
                               Ll = vehicles.Sum(d => d.LastOperationalStatusId == 6 ? 1 : 0),
                               Nc = vehicles.Sum(d => d.LastOperationalStatusId == 8 ? 1 : 0),
                               Pl = vehicles.Sum(d => d.LastOperationalStatusId == 9 ? 1 : 0),
                               Tc = vehicles.Sum(d => d.LastOperationalStatusId == 16 ? 1 : 0),
                               Sv = vehicles.Sum(d => d.LastOperationalStatusId == 14 ? 1 : 0),
                               Ws = vehicles.Sum(d => d.LastOperationalStatusId == 19 ? 1 : 0),
                               Bd = vehicles.Sum(d => d.LastOperationalStatusId == 1 ? 1 : 0),
                               Mm = vehicles.Sum(d => d.LastOperationalStatusId == 7 ? 1 : 0),
                               Tw = vehicles.Sum(d => d.LastOperationalStatusId == 18 ? 1 : 0),
                               Tb = vehicles.Sum(d => d.LastOperationalStatusId == 15 ? 1 : 0),
                               Fs = vehicles.Sum(d => d.LastOperationalStatusId == 3 ? 1 : 0),
                               Rl = vehicles.Sum(d => d.LastOperationalStatusId == 10 ? 1 : 0),
                               Rp = vehicles.Sum(d => d.LastOperationalStatusId == 11 ? 1 : 0),
                               Tn = vehicles.Sum(d => d.LastOperationalStatusId == 17 ? 1 : 0),
                               Idle = vehicles.Sum(d => (d.LastOperationalStatusId == 12 && d.LastMovementTypeId != 10) ? 1 : 0),
                               Su = vehicles.Sum(d => d.LastOperationalStatusId == 13 ? 1 : 0),
                               Overdue = vehicles.Sum(d => (d.LastOperationalStatusId == 12 && d.LastMovementTypeId == 10
                                                                && d.ExpectedDateTime < dt) ? 1 : 0)
                           };

            return returned;
        }
        
        public FleetStatusRow GetFleetStatusHistoryEntry()
        {
            var groupedQueryable = GetAvailabilityHistoryGroupedByDate();

            var availabilityKeyGrouping = AvailabilityGrouping.Average;
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.AvailabilityKeyGrouping))
            {
                availabilityKeyGrouping = (AvailabilityGrouping)Enum.Parse(typeof(AvailabilityGrouping), Parameters[DictionaryParameter.AvailabilityKeyGrouping]);
            }


            var fleetDayKeyGrouping = ExtractFleetHistoryColumns(availabilityKeyGrouping, groupedQueryable);
            var returned = GroupToSingle(fleetDayKeyGrouping);

            
            return returned;
        }
    }
}