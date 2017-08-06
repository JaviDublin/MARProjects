using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.Reservations;

namespace Mars.App.Classes.Phase4Dal.Pooling
{
    public class OverdueDataAccess : BaseDataAccess
    {
        public OverdueDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dataContext)
            : base(parameters, dataContext)
        {
            
        }

        public int GetCollections()
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);
            // 13 == Transfer Open     4 == Transfer Lorry Open

            var nowHour = DateTime.Now.Date.AddHours(DateTime.Now.Hour);
            var collections = vehicles.Where(d => (d.LastMovementTypeId == 13 
                                                    || d.LastMovementTypeId == 4)
                                                    && d.ExpectedDateTime < nowHour
                                                    );
            var returned = collections.Count();
            return returned;
        }

        public int GetOpenTripsDue()
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);
            // 12 == Rental Agreement Open

            var nowHour = DateTime.Now.Date.AddHours(DateTime.Now.Hour);
            var collections = vehicles.Where(d => (d.LastMovementTypeId == 12)
                                                    && d.ExpectedDateTime < nowHour
                                                    );
            var returned = collections.Count();
            return returned;
        }
    }
}