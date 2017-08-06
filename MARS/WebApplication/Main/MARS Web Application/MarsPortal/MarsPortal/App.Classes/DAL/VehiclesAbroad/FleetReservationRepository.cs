using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;
using Mars.App.Classes.DAL.MarsDBContext;
using App.DAL.VehiclesAbroad.Queryables;
using App.Classes.Entities.VehiclesAbroad.Abstract;

namespace App.Classes.DAL.VehiclesAbroad {
    public class FleetReservationRepository : IFleetReservationRepository {

        public List<FleetMatchEntity> getList(IFilterEntity f, string license, string sortExpression) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {
                var qDetails = new VehicleDetailsQueryable().getQueryable(db);
                var qFilter = new VehiclesFilterQueryable().getQueryable(f, db, qDetails);
                var q1 = new VehicleDetailsIdleQueryable().getQueryable(f, db, qFilter);

                var tmp = from rea in db.Reservations
                    join rentLoc in db.LOCATIONs on rea.RENT_LOC equals rentLoc.dim_Location_id
                    join rentClg in db.CMS_LOCATION_GROUPs on rentLoc.cms_location_group_id equals
                        rentClg.cms_location_group_id
                    //Return location
                    join returnLoc in db.LOCATIONs on rea.RTRN_LOC equals returnLoc.dim_Location_id
                    where rea.RS_ARRIVAL_DATE >= f.ReservationStartDate && rea.RS_ARRIVAL_DATE <= f.ReservationEndDate
                    group rea by
                        new
                        {
                            rtrnLoc = returnLoc.served_by_locn.Substring(0, 2),
                            resLocGrp = rentClg.cms_location_group1
                        }
                    into grp
                    select new {grp.Key.rtrnLoc, grp.Key.resLocGrp, c = grp.Count()};
                
                var q = from p in q1
                        join loc in db.LOCATIONs on p.Lstwwd equals loc.location1
                        join clg in db.CMS_LOCATION_GROUPs on loc.cms_location_group_id equals clg.cms_location_group_id
                        join t1 in tmp on new { x1 = p.OwnCountry, x2 = clg.cms_location_group1 } equals new { x1 = t1.rtrnLoc, x2 = t1.resLocGrp } into tj
                        from t in tj.DefaultIfEmpty()
                        where p.License == license || string.IsNullOrEmpty(license)
                        && p.Overdue != 1
                        select new { p, count = (t.c == null ? 0 : t.c) };

                switch (sortExpression) {
                    case "OwnCountry": q = q.OrderBy(p => p.p.OwnCountry); break;
                    case "OwnCountry DESC": q = q.OrderByDescending(p => p.p.OwnCountry); break;
                    case "Vc": q = q.OrderBy(p => p.p.Vc); break;
                    case "Vc DESC": q = q.OrderByDescending(p => p.p.Vc); break;
                    case "Unit": q = q.OrderBy(p => p.p.Unit); break;
                    case "Unit DESC": q = q.OrderByDescending(p => p.p.Unit); break;
                    case "License": q = q.OrderBy(p => p.p.License); break;
                    case "License DESC": q = q.OrderByDescending(p => p.p.License); break;
                    case "ModelDesc": q = q.OrderBy(p => p.p.Moddesc); break;
                    case "ModelDesc DESC": q = q.OrderByDescending(p => p.p.Moddesc); break;
                    case "Location": q = q.OrderBy(p => p.p.Lstwwd); break;
                    case "Location DESC": q = q.OrderByDescending(p => p.p.Lstwwd); break;
                    case "Operstat": q = q.OrderBy(p => p.p.Op); break;
                    case "Operstat DESC": q = q.OrderByDescending(p => p.p.Op); break;
                    case "Daysrev": q = q.OrderBy(p => p.p.Nonrev); break;
                    case "Daysrev DESC": q = q.OrderByDescending(p => p.p.Nonrev); break;
                    case "Matches": q = q.OrderBy(p => p.count); break;
                    case "Matches DESC": q = q.OrderByDescending(p => p.count); break;
                    default: q = q.OrderBy(p => p.p.Nonrev).ThenBy(p => p.p.Duewwd); break;
                }
                return (from item in q
                        select new FleetMatchEntity {
                            OwnCountry = item.p.OwnCountry,
                            Unit = item.p.Unit,
                            License = item.p.License,
                            ModelDesc = item.p.Moddesc,
                            Vc = item.p.Vc,
                            Operstat = item.p.Op,
                            Location = item.p.Lstwwd,
                            Daysrev = item.p.Nonrev.ToString(),
                            CarVan = item.p.CarVan,
                            Matches = item.count.ToString()
                        }).ToList();
            }
        }
    }
}