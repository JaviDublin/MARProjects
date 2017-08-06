using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using System.Data.Linq;
using Mars.App.Classes.DAL.MarsDBContext;

namespace App.DAL.VehiclesAbroad.Queryables {
    public class VehicleDetailsQueryable {

        public IQueryable<ICarSearchDataEntity> getQueryable(MarsDBDataContext db) {
            return from p in db.FLEET_EUROPE_ACTUALs
                   join clg in db.CMS_LOCATION_GROUPs on p.LOC_GROUP equals clg.cms_location_group_id
                   join tp in db.CMS_POOLs on clg.cms_pool_id equals tp.cms_pool_id
                   join tc1 in db.COUNTRies on p.COUNTRY equals tc1.country1
                   where ((p.FLEET_RAC_TTL ?? false) || (p.FLEET_CARSALES ?? false))
                     && (tc1.active) // Only for Active corporate countries
                   select new CarSearchDataEntity {
                       Lstwwd = p.LSTWWD ?? "",
                       Lstdate = p.LSTDATE,
                       Vc = p.VC ?? "",
                       Unit = p.UNIT ?? "",
                       License = p.LICENSE ?? "",
                       Model = p.MODEL ?? "",
                       Moddesc = p.MODDESC ?? "",
                       Duewwd = p.DUEWWD ?? "",
                       Duedate = p.DUEDATE,
                       Duetime = p.DUETIME == null ? "" : p.DUETIME.Value.ToShortTimeString(),
                       Op = p.OPERSTAT ?? "",
                       Mt = p.MOVETYPE ?? "",
                       Hold = p.CARHOLD1 ?? "",
                       Nr = p.DAYSREV == null ? "" : p.DAYSREV.ToString(),
                       Driver = p.DRVNAME ?? "",
                       Doc = p.LSTNO ?? "",
                       Lstmlg = (int?)p.LSTMLG ?? 0,
                       Remarks = "", // not used
                       Charged = p.RC ?? "",
                       Nonrev = p.DAYSREV ?? 0,
                       Regdate = p.IDATE == null ? "" : p.IDATE.Value.ToShortDateString(),
                       Ownarea = p.OWNAREA ?? "",
                       Remarkdate = "",
                       Bddays = p.BDDAYS == null ? "" : p.BDDAYS.ToString(),
                       Mmdays = p.MMDAYS == null ? "" : p.MMDAYS.ToString(),
                       Prevwwd = p.PREVWWD ?? "",
                       OwnCountry = p.COUNTRY ?? "",
                       Serial = p.SERIAL ?? "",
                       Lstoorc = p.LSTOORC ?? "",
                       Overdue = p.OVERDUE ?? 0,
                       OnRent = p.ON_RENT ?? 0,
                       Lsttype = p.LSTTYPE ?? "",
                       Pool = tp.cms_pool1 == null ? "" : tp.cms_pool1,
                       LocGroup = clg.cms_location_group1 == null ? "" : clg.cms_location_group1,
                       CarVan = p.CARVAN ?? ""
                   };


        }
    }
}