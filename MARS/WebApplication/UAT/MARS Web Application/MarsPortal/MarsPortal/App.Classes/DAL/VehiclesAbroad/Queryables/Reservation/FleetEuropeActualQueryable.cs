using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.VehiclesAbroad;
using DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;

namespace DAL.VehiclesAbroad.Queryables.Reservation {
    public class FleetEuropeActualQueryable:Queryable<FleetActualEntity> {
        public override IQueryable<FleetActualEntity> GetQueryable(MarsDBDataContext db, params string[] s) {
            return from fea in db.FLEET_EUROPE_ACTUALs
                   join clg in db.CMS_LOCATION_GROUPs on fea.LOC_GROUP equals clg.cms_location_group_id
                   where fea.COUNTRY != fea.LSTWWD.Substring(0, 2)
                              && fea.COUNTRY != ((fea.DUEWWD == null || fea.DUEWWD == "") ? fea.LSTWWD.Substring(0, 2) : fea.DUEWWD.Substring(0, 2))
                              && (fea.ON_RENT != 1)
                   group fea by new { fea.COUNTRY, clg.cms_location_group1 } into g
                   select new FleetActualEntity { OwningCountry = g.Key.COUNTRY, CurrentLocation = g.Key.cms_location_group1, Count = g.Count() };
        }
        public override IQueryable<FleetActualEntity> GetQueryable(MarsDBDataContext db, IQueryable<FleetActualEntity> q, params string[] s) {
            throw new NotImplementedException();
        }
    }
}