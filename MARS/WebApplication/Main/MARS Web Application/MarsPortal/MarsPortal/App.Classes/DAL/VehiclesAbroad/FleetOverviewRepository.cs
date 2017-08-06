using System.Collections.Generic;
using System.Linq;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using App.DAL.VehiclesAbroad.Queryables;
using App.BLL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;
using App.Classes.Entities.VehiclesAbroad.Abstract;
using App.Classes.Entities.VehiclesAbroad;


namespace App.DAL.VehiclesAbroad {
    public class FleetOverviewRepository : IFleetOverviewRepository {

        public IList<IDataTableEntity> getList(IFilterEntity filters) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                IList<IDataTableEntity> l = new List<IDataTableEntity>();
                var qDetails = new VehicleDetailsQueryable().getQueryable(db);
                var qFilter = new VehiclesFilterQueryable().getQueryable(filters, db, qDetails);
                IQueryable<ICarSearchDataEntity> qVDFilter;
                switch (filters.VehiclePredicament) {
                    case 0: qVDFilter = new VehicleDetailsAllQueryable().getQueryable(filters, db, qFilter); break;
                    case 1: qVDFilter = new VehicleDetailsOROwnToForeign().getQueryable(filters, db, qFilter); break;
                    case 2: qVDFilter = new VehicleDetailsTXToForeignQueryable().getQueryable(filters, db, qFilter); break;
                    case 3: qVDFilter = new VehicleDetailsIdleQueryable().getQueryable(filters, db, qFilter); break;
                    case 4: qVDFilter = new VehicleDetailsORInBetweenQueryable().getQueryable(filters, db, qFilter); break;
                    case 5: qVDFilter = new VehicleDetailsORReturningQueryable().getQueryable(filters, db, qFilter); break;
                    case 6: qVDFilter = new VehicleDetailsTXReturningQueryable().getQueryable(filters, db, qFilter); break;
                    case 7: qVDFilter = new VehicleDetailsNonRevQueryable().getQueryable(filters, db, qFilter); break;
                    default: qVDFilter = qFilter; break;
                }
                IQueryable<IVehiclePredicamentEntity> q1 = from p in qVDFilter
                                                           join own in db.COUNTRies on p.OwnCountry equals own.country1
                                                           join due in db.COUNTRies on p.Duewwd.Substring(0, 2) equals due.country1 into dj
                                                           from what in dj.DefaultIfEmpty()
                                                           join lst in db.COUNTRies on p.Lstwwd.Substring(0, 2) equals lst.country1
                                                           select new VehiclePredicamentEntity { ownwwd = own.country_description, duewwd = what.country_description, lstwwd = lst.country_description };
                IQueryable<IVehiclePredicamentEntity> q2;
                switch (filters.VehiclePredicament) {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        q2 = from p in q1
                             select new VehiclePredicamentEntity { ownwwd = p.ownwwd, duewwd = (p.duewwd == null || p.duewwd == "") ? p.lstwwd : p.duewwd };
                        break;
                    default:
                        q2 = from p in q1
                             select new VehiclePredicamentEntity { ownwwd = p.ownwwd, duewwd = p.lstwwd }; break;
                }
                var grp = (from p in q2
                           group p by new { o = p.ownwwd, d = p.duewwd } into g
                           select new { ownwwd = g.Key.o, duewwd = g.Key.d, sum = g.Key.o.Count() })
                          .OrderBy(p => p.duewwd).ThenBy(p => p.ownwwd);

                foreach (var item in grp) {
                    l.Add(new DataTableEntity { header = item.ownwwd, rowDefinition = item.duewwd, theValue = item.sum.ToString() });
                }
                return l;
            }
        }
    }
}