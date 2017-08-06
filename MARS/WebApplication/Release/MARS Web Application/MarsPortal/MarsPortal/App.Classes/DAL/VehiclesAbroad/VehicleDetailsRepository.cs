using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Entities.VehiclesAbroad;
using App.DAL.VehiclesAbroad.Queryables;

namespace App.DAL.VehiclesAbroad {

    public class VehicleDetailsRepository : IVehicleDetailsRepository {

        public IList<ICarSearchDataEntity> getQueryable(IFilterEntity filters, ICarFilterEntity cf, string sortExpression, string sortDirection) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                var qDetails = new VehicleDetailsQueryable().getQueryable(db);
                var qFilter = new VehiclesFilterQueryable().getQueryable(filters, db, qDetails);
                var qCarFilter = new CarFilterQueryable().getQueryable(cf, db, qFilter);
                IQueryable<ICarSearchDataEntity> qVDFilter;
                switch (filters.VehiclePredicament) {
                    case 0: qVDFilter = new VehicleDetailsAllQueryable().getQueryable(filters, db, qCarFilter); break;
                    case 1: qVDFilter = new VehicleDetailsOROwnToForeign().getQueryable(filters, db, qCarFilter); break;
                    case 2: qVDFilter = new VehicleDetailsTXToForeignQueryable().getQueryable(filters, db, qCarFilter); break;
                    case 3: qVDFilter = new VehicleDetailsIdleQueryable().getQueryable(filters, db, qCarFilter); break;
                    case 4: qVDFilter = new VehicleDetailsORInBetweenQueryable().getQueryable(filters, db, qCarFilter); break;
                    case 5: qVDFilter = new VehicleDetailsORReturningQueryable().getQueryable(filters, db, qCarFilter); break;
                    case 6: qVDFilter = new VehicleDetailsTXReturningQueryable().getQueryable(filters, db, qCarFilter); break;
                    case 7: qVDFilter = new VehicleDetailsNonRevQueryable().getQueryable(filters, db, qCarFilter); break;
                    default: qVDFilter = qCarFilter; break;
                }
                var l1 = qVDFilter.ToList();
                var l2 = new VehicleDetailsSortQueryable().getQueryable(sortExpression, sortDirection, db, l1.AsQueryable()).ToList();
                return l2;
            }
        }
        public ICarSearchDataEntity getVehicleDetail(string License) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                var qDetails = new VehicleDetailsQueryable().getQueryable(db);
                return (from p in qDetails where p.License == License select p).First();
            }
        }
    }
}