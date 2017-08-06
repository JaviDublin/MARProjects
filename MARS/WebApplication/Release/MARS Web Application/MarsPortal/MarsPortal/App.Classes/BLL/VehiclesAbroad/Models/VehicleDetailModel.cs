using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad; // added
using Mars.App.Classes.DAL.MarsDBContext; // added

using System.Web.UI.WebControls;
using App.Classes.BLL.Workers;

namespace App.BLL.VehiclesAbroad {

    public class VehicleDetailModel : GeneralFilterModel {

        public string NonrevArgument { get; set; }

        public List<CarSearchDataEntity> getVehicleDetails(string dueCountry, int vehiclePredicament,
                                                            string ownCountry, string pool, string locationGroup,
                                                            string carSegment, string carClass, string carGroup,
                                                            string unit, string license, string model, string modelDescription,
                                                            string vin, string customerName, string colour, string mileage,
                                                            int nonRev, string operstat, string moveType,
                                                            string nonRevArg,
                                                            string sortExpression) {

            if (!string.IsNullOrEmpty(nonRevArg)) {
                NonrevArgument = nonRevArg ?? "";
                dueCountry = VehiclesAbroadWorker.NonRevDueCountry;
                ownCountry = VehiclesAbroadWorker.NonRevOwnCountry;
            }

            dueCountry = processString(dueCountry);
            ownCountry = processString(ownCountry);
            VehiclesAbroadWorker.NonRevDueCountry = "";
            VehiclesAbroadWorker.NonRevOwnCountry = "";


            //// Presentation logic
            pool = dueCountry == "" ? "" : processString(pool);
            locationGroup = pool == "" ? "" : processString(locationGroup);
            carSegment = ownCountry == "" ? "" : processString(carSegment);
            carClass = carSegment == "" ? "" : processString(carClass);
            carGroup = carClass == "" ? "" : processString(carGroup);
            operstat = string.IsNullOrEmpty(operstat) ? "" : processString(operstat);
            moveType = string.IsNullOrEmpty(moveType) ? "" : processString(moveType);

            FilterEntity fe = new FilterEntity {
                DueCountry = dueCountry,
                VehiclePredicament = vehiclePredicament,
                OwnCountry = ownCountry,
                Pool = pool,
                Location = locationGroup,
                CarSegment = carSegment,
                CarClass = carClass,
                CarGroup = carGroup,
                Operstat = operstat,
                MoveType = moveType
            };

            CarFilterEntity cfe = new CarFilterEntity {
                Unit = unit,
                License = license,
                Model = model,
                ModelDesc = modelDescription,
                Vin = vin,
                Name = customerName,
                Colour = colour,
                Mileage = mileage
            };

            switch (vehiclePredicament) {
                case 0: // all
                    return getVehicleDetailsAll(fe, cfe, nonRev, sortExpression);
                case 1: // On rent owning country to foreign country
                    return getVehicleDetails(fe, cfe, nonRev, sortExpression);
                case 2:
                    return getVehicleDetails(fe, cfe, nonRev, sortExpression);
                case 3:
                    return getVehicleDetailsIdle(fe, cfe, nonRev, sortExpression);
                case 4:
                    return getVehicleDetails(fe, cfe, nonRev, sortExpression);
                case 5:
                    return getVehicleDetails(fe, cfe, nonRev, sortExpression);
                case 6:
                    return getVehicleDetails(fe, cfe, nonRev, sortExpression);
                case 7: // non rev
                    return getVehicleDetailsNonrev(fe, cfe, nonRev, sortExpression);
                default:
                    return getVehicleDetailsIdle(fe, cfe, nonRev, sortExpression);
            }
        }

        private List<CarSearchDataEntity> getVehicleDetailsNonrev(FilterEntity filters, CarFilterEntity cf, int nonRev, string sortExpression) {
            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<CarSearchDataEntity> l = new List<CarSearchDataEntity>();

                // Convert mileage to an integer or -1 if not a string 
                int mileage = Int32.TryParse(cf.Mileage, out mileage) ? mileage : -1;

                // Exactly the same query as in FleetEntity but not grouped - Also CarFilters added
                var q = from p in db.FLEET_EUROPE_ACTUALs
                        //join loc in db.LOCATIONs on p.DUEAREA equals loc.ownarea
                        join clg in db.CMS_LOCATION_GROUPs on p.LOC_GROUP equals clg.cms_location_group_id
                        join tp in db.CMS_POOLs on clg.cms_pool_id equals tp.cms_pool_id
                        join tc1 in db.COUNTRies on p.COUNTRY equals tc1.country1
                        //join tc2 in db.COUNTRies on p.DUEWWD.Substring(0, 2) equals tc2.country1
                        where ((p.FLEET_RAC_TTL ?? false) || (p.FLEET_CARSALES ?? false))
                          && (tc1.active) // Only for Active corporate countries
                          && ((p.COUNTRY == p.LSTWWD.Substring(0, 2) && p.DUEWWD != null && p.COUNTRY != p.DUEWWD.Substring(0, 2))
                            || (p.COUNTRY != p.LSTWWD.Substring(0, 2)))
                          && (p.COUNTRY != p.DUEWWD.Substring(0, 2) && p.DUEWWD != null && p.COUNTRY != p.LSTWWD.Substring(0, 2))
                          && (p.ON_RENT != 1)
                            //&& (NonrevArgument == "RENT" ? p.RT == 1 : true)
                            //&& (NonrevArgument == "SHOP" ? (p.MM == 1 || p.BD == 1 || p.TW == 1) : true)
                            //&& (NonrevArgument == "OTHER" ? !(p.MM == 1 || p.BD == 1 || p.TW == 1 || p.RT == 1) : true)
                          && (filters.OwnCountry.Equals(p.COUNTRY) || String.IsNullOrEmpty(filters.OwnCountry))
                          && (filters.DueCountry.Equals(p.DUEWWD.Substring(0, 2)) || String.IsNullOrEmpty(filters.DueCountry))
                          && (tp.cms_pool1 == filters.Pool || filters.Pool == "" || filters.Pool == null)
                          && (clg.cms_location_group1 == filters.Location || filters.Location == "" || filters.Location == null)
                          && (p.VC == filters.CarGroup || filters.CarGroup == "" || filters.CarGroup == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               where cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.VC
                               select cc.car_class1).Contains(filters.CarClass) || filters.CarClass == "" || filters.CarClass == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               from cs in db.CAR_SEGMENTs
                               where cs.car_segment_id == cc.car_segment_id
                               && cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.VC
                               select cs.car_segment1).Contains(filters.CarSegment) || filters.CarSegment == "" || filters.CarSegment == null)
                          && (p.UNIT.Contains(cf.Unit) || cf.Unit == "" || cf.Unit == null)
                          && (p.LICENSE.Contains(cf.License) || cf.License == "" || cf.License == null)
                          && (p.MODEL.Contains(cf.Model) || cf.Model == "" || cf.Model == null)
                          && (p.MODDESC.Contains(cf.ModelDesc) || cf.ModelDesc == "" || cf.ModelDesc == null)
                          && (p.SERIAL.Contains(cf.Vin) || cf.Vin == "" || cf.Vin == null)
                          && (p.DRVNAME.Contains(cf.Name) || cf.Name == "" || cf.Name == null)
                          && (p.COLOR.Contains(cf.Colour) || cf.Colour == "" || cf.Colour == null)
                          && (p.LSTMLG > mileage || mileage == -1)
                          && (p.DAYSREV >= nonRev) // non Revenue selection
                          && (p.MOVETYPE.Contains(filters.MoveType) || string.IsNullOrEmpty(filters.MoveType))
                          && (p.OPERSTAT.Contains(filters.Operstat) || string.IsNullOrEmpty(filters.Operstat))
                        select p;

                // the sorting by column (there must be an easier way!)
                switch (sortExpression) {
                    case "Lstwwd": q = q.OrderBy(p => p.LSTWWD); break;
                    case "Lstwwd DESC": q = q.OrderByDescending(p => p.LSTWWD); break;
                    case "Lstdate": q = q.OrderBy(p => p.LSTDATE); break;
                    case "Lstdate DESC": q = q.OrderByDescending(p => p.LSTDATE); break;
                    case "Vc": q = q.OrderBy(p => p.VC); break;
                    case "Vc DESC": q = q.OrderByDescending(p => p.VC); break;
                    case "Unit": q = q.OrderBy(p => p.UNIT); break;
                    case "Unit DESC": q = q.OrderByDescending(p => p.UNIT); break;
                    case "License": q = q.OrderBy(p => p.LICENSE); break;
                    case "License DESC": q = q.OrderByDescending(p => p.LICENSE); break;
                    case "Model": q = q.OrderBy(p => p.MODEL); break;
                    case "Model DESC": q = q.OrderByDescending(p => p.MODEL); break;
                    case "Moddesc": q = q.OrderBy(p => p.MODDESC); break;
                    case "Moddesc DESC": q = q.OrderByDescending(p => p.MODDESC); break;
                    case "Duewwd": q = q.OrderBy(p => p.DUEWWD); break;
                    case "Duewwd DESC": q = q.OrderByDescending(p => p.DUEWWD); break;
                    case "Duedate": q = q.OrderBy(p => p.DUEDATE); break;
                    case "Duedate DESC": q = q.OrderByDescending(p => p.DUEDATE); break;
                    case "Duetime": q = q.OrderBy(p => p.DUETIME); break;
                    case "Duetime DESC": q = q.OrderByDescending(p => p.DUETIME); break;
                    case "Op": q = q.OrderBy(p => p.OPERSTAT); break;
                    case "Op DESC": q = q.OrderByDescending(p => p.OPERSTAT); break;
                    case "Mt": q = q.OrderBy(p => p.MOVETYPE); break;
                    case "Mt DESC": q = q.OrderByDescending(p => p.MOVETYPE); break;
                    case "Nr": q = q.OrderBy(p => p.DAYSREV); break;
                    case "Nr DESC": q = q.OrderByDescending(p => p.DAYSREV); break;
                    case "Driver": q = q.OrderBy(p => p.DRVNAME); break;
                    case "Driver DESC": q = q.OrderByDescending(p => p.DRVNAME); break;
                    case "Doc": q = q.OrderBy(p => p.LSTNO); break;
                    case "Doc DESC": q = q.OrderByDescending(p => p.LSTNO); break;
                    case "Lstmlg": q = q.OrderBy(p => p.LSTMLG); break;
                    case "Lstmlg DESC": q = q.OrderByDescending(p => p.LSTMLG); break;
                    case "NonRev": q = q.OrderBy(p => p.DAYSREV); break;
                    case "NonRev DESC": q = q.OrderByDescending(p => p.DAYSREV); break;
                }

                foreach (var item in q) {
                    l.Add(new CarSearchDataEntity {
                        Lstwwd = item.LSTWWD,
                        Lstdate = item.LSTDATE,
                        Vc = item.VC,
                        Unit = item.UNIT,
                        License = item.LICENSE,
                        Model = item.MODEL,
                        Moddesc = item.MODDESC,
                        Duewwd = item.DUEWWD ?? "",
                        Duedate = item.DUEDATE,
                        Duetime = item.DUETIME == null ? "" : item.DUETIME.Value.ToShortTimeString(),
                        Op = item.OPERSTAT,
                        Mt = item.MOVETYPE,
                        Hold = item.CARHOLD1,
                        Nr = item.DAYSREV.ToString(),
                        Driver = item.DRVNAME,
                        Doc = item.LSTNO,
                        Lstmlg = (int?)item.LSTMLG ?? 0,
                        Remarks = "", // not used
                        Charged = item.RC,
                        Nonrev = item.DAYSREV ?? 0,
                        Regdate = item.IDATE == null ? "" : item.IDATE.Value.ToShortDateString(),
                        Ownarea = item.OWNAREA ?? "",
                        Remarkdate = "",
                        Bddays = item.BDDAYS == null ? "" : item.BDDAYS.ToString(),
                        Mmdays = item.MMDAYS == null ? "" : item.MMDAYS.ToString(),
                        Prevwwd = item.PREVWWD ?? ""
                    });
                }
                cf.NoRecords = l.Count();
                return l;
            }
        }

        public List<CarSearchDataEntity> getVehicleDetailsAll(FilterEntity filters, CarFilterEntity cf, int nonRev, string sortExpression) {
            //
            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<CarSearchDataEntity> l = new List<CarSearchDataEntity>();

                // Convert mileage to an integer or -1 if not a string 
                int mileage = Int32.TryParse(cf.Mileage, out mileage) ? mileage : -1;

                // Exactly the same query as in FleetEntity but not grouped - Also CarFilters added
                var q = from p in db.FLEET_EUROPE_ACTUALs
                        //join loc in db.LOCATIONs on p.DUEAREA equals loc.ownarea
                        join clg in db.CMS_LOCATION_GROUPs on p.LOC_GROUP equals clg.cms_location_group_id
                        join tp in db.CMS_POOLs on clg.cms_pool_id equals tp.cms_pool_id
                        join tc1 in db.COUNTRies on p.COUNTRY equals tc1.country1
                        //join tc2 in db.COUNTRies on p.DUEWWD.Substring(0, 2) equals tc2.country1
                        where ((p.FLEET_RAC_TTL ?? false) || (p.FLEET_CARSALES ?? false))
                          && (
                               (p.COUNTRY == p.LSTWWD.Substring(0, 2) && p.DUEWWD != null && (p.COUNTRY != p.DUEWWD.Substring(0, 2)))
                               ||
                               (p.COUNTRY != p.LSTWWD.Substring(0, 2))
                             )
                          && (
                               (p.COUNTRY != p.DUEWWD.Substring(0, 2)) && p.DUEWWD != null && (p.COUNTRY != p.LSTWWD.Substring(0, 2))
                             )
                          && (tc1.active) // Only for Active corporate countries
                          && (filters.OwnCountry.Equals(p.COUNTRY) || String.IsNullOrEmpty(filters.OwnCountry))
                          && (filters.DueCountry.Equals(p.DUEWWD.Substring(0, 2)) || String.IsNullOrEmpty(filters.DueCountry))
                          && (tp.cms_pool1 == filters.Pool || filters.Pool == "" || filters.Pool == null)
                          && (clg.cms_location_group1 == filters.Location || filters.Location == "" || filters.Location == null)
                          && (p.VC == filters.CarGroup || filters.CarGroup == "" || filters.CarGroup == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               where cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.VC
                               select cc.car_class1).Contains(filters.CarClass) || filters.CarClass == "" || filters.CarClass == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               from cs in db.CAR_SEGMENTs
                               where cs.car_segment_id == cc.car_segment_id
                               && cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.VC
                               select cs.car_segment1).Contains(filters.CarSegment) || filters.CarSegment == "" || filters.CarSegment == null)
                          && (p.UNIT.Contains(cf.Unit) || cf.Unit == "" || cf.Unit == null)
                          && (p.LICENSE.Contains(cf.License) || cf.License == "" || cf.License == null)
                          && (p.MODEL.Contains(cf.Model) || cf.Model == "" || cf.Model == null)
                          && (p.MODDESC.Contains(cf.ModelDesc) || cf.ModelDesc == "" || cf.ModelDesc == null)
                          && (p.SERIAL.Contains(cf.Vin) || cf.Vin == "" || cf.Vin == null)
                          && (p.DRVNAME.Contains(cf.Name) || cf.Name == "" || cf.Name == null)
                          && (p.COLOR.Contains(cf.Colour) || cf.Colour == "" || cf.Colour == null)
                          && (p.LSTMLG > mileage || mileage == -1)
                          && (p.DAYSREV >= nonRev) // non Revenue selection
                          && (p.MOVETYPE.Contains(filters.MoveType) || string.IsNullOrEmpty(filters.MoveType))
                          && (p.OPERSTAT.Contains(filters.Operstat) || string.IsNullOrEmpty(filters.Operstat))
                        select p;

                // the sorting by column (there must be an easier way!)
                switch (sortExpression) {
                    case "Lstwwd": q = q.OrderBy(p => p.LSTWWD); break;
                    case "Lstwwd DESC": q = q.OrderByDescending(p => p.LSTWWD); break;
                    case "Lstdate": q = q.OrderBy(p => p.LSTDATE); break;
                    case "Lstdate DESC": q = q.OrderByDescending(p => p.LSTDATE); break;
                    case "Vc": q = q.OrderBy(p => p.VC); break;
                    case "Vc DESC": q = q.OrderByDescending(p => p.VC); break;
                    case "Unit": q = q.OrderBy(p => p.UNIT); break;
                    case "Unit DESC": q = q.OrderByDescending(p => p.UNIT); break;
                    case "License": q = q.OrderBy(p => p.LICENSE); break;
                    case "License DESC": q = q.OrderByDescending(p => p.LICENSE); break;
                    case "Model": q = q.OrderBy(p => p.MODEL); break;
                    case "Model DESC": q = q.OrderByDescending(p => p.MODEL); break;
                    case "Moddesc": q = q.OrderBy(p => p.MODDESC); break;
                    case "Moddesc DESC": q = q.OrderByDescending(p => p.MODDESC); break;
                    case "Duewwd": q = q.OrderBy(p => p.DUEWWD); break;
                    case "Duewwd DESC": q = q.OrderByDescending(p => p.DUEWWD); break;
                    case "Duedate": q = q.OrderBy(p => p.DUEDATE); break;
                    case "Duedate DESC": q = q.OrderByDescending(p => p.DUEDATE); break;
                    case "Duetime": q = q.OrderBy(p => p.DUETIME); break;
                    case "Duetime DESC": q = q.OrderByDescending(p => p.DUETIME); break;
                    case "Op": q = q.OrderBy(p => p.OPERSTAT); break;
                    case "Op DESC": q = q.OrderByDescending(p => p.OPERSTAT); break;
                    case "Mt": q = q.OrderBy(p => p.MOVETYPE); break;
                    case "Mt DESC": q = q.OrderByDescending(p => p.MOVETYPE); break;
                    case "Nr": q = q.OrderBy(p => p.DAYSREV); break;
                    case "Nr DESC": q = q.OrderByDescending(p => p.DAYSREV); break;
                    case "Driver": q = q.OrderBy(p => p.DRVNAME); break;
                    case "Driver DESC": q = q.OrderByDescending(p => p.DRVNAME); break;
                    case "Doc": q = q.OrderBy(p => p.LSTNO); break;
                    case "Doc DESC": q = q.OrderByDescending(p => p.LSTNO); break;
                    case "Lstmlg": q = q.OrderBy(p => p.LSTMLG); break;
                    case "Lstmlg DESC": q = q.OrderByDescending(p => p.LSTMLG); break;
                    case "NonRev": q = q.OrderBy(p => p.DAYSREV); break;
                    case "NonRev DESC": q = q.OrderByDescending(p => p.DAYSREV); break;
                }

                foreach (var item in q) {
                    l.Add(new CarSearchDataEntity {
                        Lstwwd = item.LSTWWD,
                        Lstdate = item.LSTDATE,
                        Vc = item.VC,
                        Unit = item.UNIT,
                        License = item.LICENSE,
                        Model = item.MODEL,
                        Moddesc = item.MODDESC,
                        Duewwd = item.DUEWWD ?? "",
                        Duedate = item.DUEDATE,
                        Duetime = item.DUETIME == null ? "" : item.DUETIME.Value.ToShortTimeString(),
                        Op = item.OPERSTAT,
                        Mt = item.MOVETYPE,
                        Hold = item.CARHOLD1,
                        Nr = item.DAYSREV.ToString(),
                        Driver = item.DRVNAME,
                        Doc = item.LSTNO,
                        Lstmlg = (int?)item.LSTMLG ?? 0,
                        Remarks = "", // not used
                        Charged = item.RC,
                        Nonrev = item.DAYSREV ?? 0,
                        Regdate = item.IDATE == null ? "" : item.IDATE.Value.ToShortDateString(),
                        Ownarea = item.OWNAREA ?? "",
                        Remarkdate = "",
                        Bddays = item.BDDAYS == null ? "" : item.BDDAYS.ToString(),
                        Mmdays = item.MMDAYS == null ? "" : item.MMDAYS.ToString(),
                        Prevwwd = item.PREVWWD ?? ""
                    });
                }
                cf.NoRecords = l.Count();
                return l;
            }
        }

        public List<CarSearchDataEntity> getVehicleDetails(FilterEntity filters, CarFilterEntity cf, int nonRev, string sortExpression) {
            //
            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<CarSearchDataEntity> l = new List<CarSearchDataEntity>();

                // Convert mileage to an integer or -1 if not a string 
                int mileage = Int32.TryParse(cf.Mileage, out mileage) ? mileage : -1;

                // Exactly the same query as in FleetEntity but not grouped - Also CarFilters added
                var q = from p in db.FLEET_EUROPE_ACTUALs
                        //join loc in db.LOCATIONs on p.DUEAREA equals loc.ownarea
                        join clg in db.CMS_LOCATION_GROUPs on p.LOC_GROUP equals clg.cms_location_group_id
                        join tp in db.CMS_POOLs on clg.cms_pool_id equals tp.cms_pool_id
                        join tc1 in db.COUNTRies on p.COUNTRY equals tc1.country1
                        //join tc2 in db.COUNTRies on p.DUEWWD.Substring(0, 2) equals tc2.country1
                        where ((p.FLEET_RAC_TTL ?? false) || (p.FLEET_CARSALES ?? false))
                          && (filters.VehiclePredicament == 1 ? (p.COUNTRY == p.LSTWWD.Substring(0, 2) && p.COUNTRY != p.DUEWWD.Substring(0, 2) && p.MOVETYPE == "R-O" && p.ON_RENT == 1) : true) // On rent: Owning country to foreign country
                          && (filters.VehiclePredicament == 2 ? (p.COUNTRY == p.LSTWWD.Substring(0, 2) && p.COUNTRY != p.DUEWWD.Substring(0, 2) && p.MOVETYPE != "R-O") : true) // Transfer: Owning country to foreign country
                          && (filters.VehiclePredicament == 4 ? (p.COUNTRY != p.LSTWWD.Substring(0, 2) && p.COUNTRY != p.DUEWWD.Substring(0, 2) && p.MOVETYPE == "R-O" && p.ON_RENT == 1) : true) // On rent in or between foreign county(ies)
                          && (filters.VehiclePredicament == 5 ? (p.COUNTRY != p.LSTWWD.Substring(0, 2) && p.COUNTRY == p.DUEWWD.Substring(0, 2) && p.MOVETYPE == "R-O" && (p.ON_RENT == 1 || p.OVERDUE == 1)) : true) // On rent: Returning to owning country
                          && (filters.VehiclePredicament == 6 ? (p.COUNTRY != p.LSTWWD.Substring(0, 2) && p.COUNTRY == p.DUEWWD.Substring(0, 2) && p.MOVETYPE != "R-O" && (p.OVERDUE != 1 && p.ON_RENT != 1) && p.LSTOORC == "O") : true) // Transfer: Returning to owning country
                          && (tc1.active) // Only for Active corporate countries
                          && (filters.OwnCountry.Equals(p.COUNTRY) || String.IsNullOrEmpty(filters.OwnCountry))
                          && (filters.DueCountry.Equals(p.DUEWWD.Substring(0, 2)) || String.IsNullOrEmpty(filters.DueCountry))
                          && (tp.cms_pool1 == filters.Pool || filters.Pool == "" || filters.Pool == null)
                          && (clg.cms_location_group1 == filters.Location || filters.Location == "" || filters.Location == null)
                          && (p.VC == filters.CarGroup || filters.CarGroup == "" || filters.CarGroup == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               where cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.VC
                               select cc.car_class1).Contains(filters.CarClass) || filters.CarClass == "" || filters.CarClass == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               from cs in db.CAR_SEGMENTs
                               where cs.car_segment_id == cc.car_segment_id
                               && cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.VC
                               select cs.car_segment1).Contains(filters.CarSegment) || filters.CarSegment == "" || filters.CarSegment == null)
                          && (p.UNIT.Contains(cf.Unit) || cf.Unit == "" || cf.Unit == null)
                          && (p.LICENSE.Contains(cf.License) || cf.License == "" || cf.License == null)
                          && (p.MODEL.Contains(cf.Model) || cf.Model == "" || cf.Model == null)
                          && (p.MODDESC.Contains(cf.ModelDesc) || cf.ModelDesc == "" || cf.ModelDesc == null)
                          && (p.SERIAL.Contains(cf.Vin) || cf.Vin == "" || cf.Vin == null)
                          && (p.DRVNAME.Contains(cf.Name) || cf.Name == "" || cf.Name == null)
                          && (p.COLOR.Contains(cf.Colour) || cf.Colour == "" || cf.Colour == null)
                          && (p.LSTMLG > mileage || mileage == -1)
                          && (p.DAYSREV >= nonRev) // non Revenue selection
                          && (p.MOVETYPE.Contains(filters.MoveType) || string.IsNullOrEmpty(filters.MoveType))
                          && (p.OPERSTAT.Contains(filters.Operstat) || string.IsNullOrEmpty(filters.Operstat))
                        select p;

                // the sorting by column (there must be an easier way!)
                switch (sortExpression) {
                    case "Lstwwd": q = q.OrderBy(p => p.LSTWWD); break;
                    case "Lstwwd DESC": q = q.OrderByDescending(p => p.LSTWWD); break;
                    case "Lstdate": q = q.OrderBy(p => p.LSTDATE); break;
                    case "Lstdate DESC": q = q.OrderByDescending(p => p.LSTDATE); break;
                    case "Vc": q = q.OrderBy(p => p.VC); break;
                    case "Vc DESC": q = q.OrderByDescending(p => p.VC); break;
                    case "Unit": q = q.OrderBy(p => p.UNIT); break;
                    case "Unit DESC": q = q.OrderByDescending(p => p.UNIT); break;
                    case "License": q = q.OrderBy(p => p.LICENSE); break;
                    case "License DESC": q = q.OrderByDescending(p => p.LICENSE); break;
                    case "Model": q = q.OrderBy(p => p.MODEL); break;
                    case "Model DESC": q = q.OrderByDescending(p => p.MODEL); break;
                    case "Moddesc": q = q.OrderBy(p => p.MODDESC); break;
                    case "Moddesc DESC": q = q.OrderByDescending(p => p.MODDESC); break;
                    case "Duewwd": q = q.OrderBy(p => p.DUEWWD); break;
                    case "Duewwd DESC": q = q.OrderByDescending(p => p.DUEWWD); break;
                    case "Duedate": q = q.OrderBy(p => p.DUEDATE); break;
                    case "Duedate DESC": q = q.OrderByDescending(p => p.DUEDATE); break;
                    case "Duetime": q = q.OrderBy(p => p.DUETIME); break;
                    case "Duetime DESC": q = q.OrderByDescending(p => p.DUETIME); break;
                    case "Op": q = q.OrderBy(p => p.OPERSTAT); break;
                    case "Op DESC": q = q.OrderByDescending(p => p.OPERSTAT); break;
                    case "Mt": q = q.OrderBy(p => p.MOVETYPE); break;
                    case "Mt DESC": q = q.OrderByDescending(p => p.MOVETYPE); break;
                    case "Nr": q = q.OrderBy(p => p.DAYSREV); break;
                    case "Nr DESC": q = q.OrderByDescending(p => p.DAYSREV); break;
                    case "Driver": q = q.OrderBy(p => p.DRVNAME); break;
                    case "Driver DESC": q = q.OrderByDescending(p => p.DRVNAME); break;
                    case "Doc": q = q.OrderBy(p => p.LSTNO); break;
                    case "Doc DESC": q = q.OrderByDescending(p => p.LSTNO); break;
                    case "Lstmlg": q = q.OrderBy(p => p.LSTMLG); break;
                    case "Lstmlg DESC": q = q.OrderByDescending(p => p.LSTMLG); break;
                    case "NonRev": q = q.OrderBy(p => p.DAYSREV); break;
                    case "NonRev DESC": q = q.OrderByDescending(p => p.DAYSREV); break;
                }

                foreach (var item in q) {
                    l.Add(new CarSearchDataEntity {
                        Lstwwd = item.LSTWWD,
                        Lstdate = item.LSTDATE,
                        Vc = item.VC,
                        Unit = item.UNIT,
                        License = item.LICENSE,
                        Model = item.MODEL,
                        Moddesc = item.MODDESC,
                        Duewwd = item.DUEWWD ?? "",
                        Duedate = item.DUEDATE,
                        Duetime = item.DUETIME == null ? "" : item.DUETIME.Value.ToShortTimeString(),
                        Op = item.OPERSTAT,
                        Mt = item.MOVETYPE,
                        Hold = item.CARHOLD1,
                        Nr = item.DAYSREV.ToString(),
                        Driver = item.DRVNAME,
                        Doc = item.LSTNO,
                        Lstmlg = (int?)item.LSTMLG ?? 0,
                        Remarks = "", // not used
                        Charged = item.RC,
                        Nonrev = item.DAYSREV ?? 0,
                        Regdate = item.IDATE == null ? "" : item.IDATE.Value.ToShortDateString(),
                        Ownarea = item.OWNAREA ?? "",
                        Remarkdate = "",
                        Bddays = item.BDDAYS == null ? "" : item.BDDAYS.ToString(),
                        Mmdays = item.MMDAYS == null ? "" : item.MMDAYS.ToString(),
                        Prevwwd = item.PREVWWD ?? ""
                    });
                }
                cf.NoRecords = l.Count();
                return l;
            }
        }

        public List<CarSearchDataEntity> getVehicleDetailsIdle(FilterEntity filters, CarFilterEntity cf, int nonRev, string sortExpression) {
            //
            using (MarsDBDataContext db = new MarsDBDataContext()) {

                List<CarSearchDataEntity> l = new List<CarSearchDataEntity>();

                // Convert mileage to an integer or -1 if not a string 
                int mileage = Int32.TryParse(cf.Mileage, out mileage) ? mileage : -1;

                // Exactly the same query as in FleetEntity but not grouped - Also CarFilters added
                var q = from p in db.FLEET_EUROPE_ACTUALs
                        //join loc in db.LOCATIONs on p.DUEAREA equals loc.ownarea
                        join clg in db.CMS_LOCATION_GROUPs on p.LOC_GROUP equals clg.cms_location_group_id
                        join tp in db.CMS_POOLs on clg.cms_pool_id equals tp.cms_pool_id
                        join tc1 in db.COUNTRies on p.COUNTRY equals tc1.country1
                        //join tc2 in db.COUNTRies on p.DUEWWD.Substring(0, 2) equals tc2.country1
                        where
                          ((p.FLEET_RAC_TTL ?? false) || (p.FLEET_CARSALES ?? false))
                          && (p.COUNTRY != p.LSTWWD.Substring(0, 2)
                                && ((p.OVERDUE == 1 && p.COUNTRY != p.DUEWWD.Substring(0, 2))
                                || (p.LSTOORC != "O")))
                          && (tc1.active) // Only for Active corporate countries
                          && (filters.OwnCountry.Equals(p.COUNTRY) || String.IsNullOrEmpty(filters.OwnCountry))
                          && (filters.DueCountry.Equals(p.LSTWWD.Substring(0, 2)) || String.IsNullOrEmpty(filters.DueCountry))
                          && (tp.cms_pool1 == filters.Pool || filters.Pool == "" || filters.Pool == null)
                          && (clg.cms_location_group1 == filters.Location || filters.Location == "" || filters.Location == null)
                          && (p.VC == filters.CarGroup || filters.CarGroup == "" || filters.CarGroup == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               where cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.VC
                               select cc.car_class1).Contains(filters.CarClass) || filters.CarClass == "" || filters.CarClass == null)
                          && ((from cg in db.CAR_GROUPs
                               from cc in db.CAR_CLASSes
                               from cs in db.CAR_SEGMENTs
                               where cs.car_segment_id == cc.car_segment_id
                               && cg.car_class_id == cc.car_class_id
                               && cg.car_group1 == p.VC
                               select cs.car_segment1).Contains(filters.CarSegment) || filters.CarSegment == "" || filters.CarSegment == null)
                          && (p.UNIT.Contains(cf.Unit) || cf.Unit == "" || cf.Unit == null)
                          && (p.LICENSE.Contains(cf.License) || cf.License == "" || cf.License == null)
                          && (p.MODEL.Contains(cf.Model) || cf.Model == "" || cf.Model == null)
                          && (p.MODDESC.Contains(cf.ModelDesc) || cf.ModelDesc == "" || cf.ModelDesc == null)
                          && (p.SERIAL.Contains(cf.Vin) || cf.Vin == "" || cf.Vin == null)
                          && (p.DRVNAME.Contains(cf.Name) || cf.Name == "" || cf.Name == null)
                          && (p.COLOR.Contains(cf.Colour) || cf.Colour == "" || cf.Colour == null)
                          && (p.LSTMLG > mileage || mileage == -1)
                          && (p.DAYSREV >= nonRev) // non Revenue selection
                          && (p.MOVETYPE.Contains(filters.MoveType) || string.IsNullOrEmpty(filters.MoveType))
                          && (p.OPERSTAT.Contains(filters.Operstat) || string.IsNullOrEmpty(filters.Operstat))
                        select p;

                // the sorting by column (there must be an easier way!)
                switch (sortExpression) {
                    case "Lstwwd": q = q.OrderBy(p => p.LSTWWD); break;
                    case "Lstwwd DESC": q = q.OrderByDescending(p => p.LSTWWD); break;
                    case "Lstdate": q = q.OrderBy(p => p.LSTDATE); break;
                    case "Lstdate DESC": q = q.OrderByDescending(p => p.LSTDATE); break;
                    case "Vc": q = q.OrderBy(p => p.VC); break;
                    case "Vc DESC": q = q.OrderByDescending(p => p.VC); break;
                    case "Unit": q = q.OrderBy(p => p.UNIT); break;
                    case "Unit DESC": q = q.OrderByDescending(p => p.UNIT); break;
                    case "License": q = q.OrderBy(p => p.LICENSE); break;
                    case "License DESC": q = q.OrderByDescending(p => p.LICENSE); break;
                    case "Model": q = q.OrderBy(p => p.MODEL); break;
                    case "Model DESC": q = q.OrderByDescending(p => p.MODEL); break;
                    case "Moddesc": q = q.OrderBy(p => p.MODDESC); break;
                    case "Moddesc DESC": q = q.OrderByDescending(p => p.MODDESC); break;
                    case "Duewwd": q = q.OrderBy(p => p.DUEWWD); break;
                    case "Duewwd DESC": q = q.OrderByDescending(p => p.DUEWWD); break;
                    case "Duedate": q = q.OrderBy(p => p.DUEDATE); break;
                    case "Duedate DESC": q = q.OrderByDescending(p => p.DUEDATE); break;
                    case "Duetime": q = q.OrderBy(p => p.DUETIME); break;
                    case "Duetime DESC": q = q.OrderByDescending(p => p.DUETIME); break;
                    case "Op": q = q.OrderBy(p => p.OPERSTAT); break;
                    case "Op DESC": q = q.OrderByDescending(p => p.OPERSTAT); break;
                    case "Mt": q = q.OrderBy(p => p.MOVETYPE); break;
                    case "Mt DESC": q = q.OrderByDescending(p => p.MOVETYPE); break;
                    case "Nr": q = q.OrderBy(p => p.DAYSREV); break;
                    case "Nr DESC": q = q.OrderByDescending(p => p.DAYSREV); break;
                    case "Driver": q = q.OrderBy(p => p.DRVNAME); break;
                    case "Driver DESC": q = q.OrderByDescending(p => p.DRVNAME); break;
                    case "Doc": q = q.OrderBy(p => p.LSTNO); break;
                    case "Doc DESC": q = q.OrderByDescending(p => p.LSTNO); break;
                    case "Lstmlg": q = q.OrderBy(p => p.LSTMLG); break;
                    case "Lstmlg DESC": q = q.OrderByDescending(p => p.LSTMLG); break;
                    case "NonRev": q = q.OrderBy(p => p.DAYSREV); break;
                    case "NonRev DESC": q = q.OrderByDescending(p => p.DAYSREV); break;
                }

                foreach (var item in q) {
                    l.Add(new CarSearchDataEntity {
                        Lstwwd = item.LSTWWD ?? "",
                        Lstdate = item.LSTDATE,
                        Vc = item.VC ?? "",
                        Unit = item.UNIT ?? "",
                        License = item.LICENSE ?? "",
                        Model = item.MODEL ?? "",
                        Moddesc = item.MODDESC ?? "",
                        Duewwd = item.DUEWWD ?? "",
                        Duedate = item.DUEDATE,
                        Duetime = item.DUETIME == null ? "" : item.DUETIME.Value.ToShortTimeString(),
                        Op = item.OPERSTAT ?? "",
                        Mt = item.MOVETYPE ?? "",
                        Hold = item.CARHOLD1 ?? "",
                        Nr = item.DAYSREV == null ? "" : item.DAYSREV.ToString(),
                        Driver = item.DRVNAME ?? "",
                        Doc = item.LSTNO ?? "",
                        Lstmlg = (int?)item.LSTMLG ?? 0,
                        Remarks = "", // not used
                        Charged = item.RC ?? "",
                        Nonrev = item.DAYSREV ?? 0,
                        Regdate = item.IDATE == null ? "" : item.IDATE.Value.ToShortDateString(),
                        Ownarea = item.OWNAREA ?? "",
                        Remarkdate = "",
                        Bddays = item.BDDAYS == null ? "" : item.BDDAYS.ToString(),
                        Mmdays = item.MMDAYS == null ? "" : item.MMDAYS.ToString(),
                        Prevwwd = item.PREVWWD ?? "",
                        OwnCountry = item.COUNTRY ?? ""
                    });
                }
                cf.NoRecords = l.Count();
                return l;
            }
        }

        public CarSearchDataEntity getVehicleDetailsFromLicense(string license) {
            // returns the vehicles details for a single license
            using (MarsDBDataContext db = new MarsDBDataContext()) {

                // get the vehicles details
                var q = (from fea in db.FLEET_EUROPE_ACTUALs
                         where fea.LICENSE.Equals(license)
                         select fea).First();

                // construct the return value
                CarSearchDataEntity carSearchDataEntity = new CarSearchDataEntity {
                    Lstwwd = q.LSTWWD ?? "",
                    Lstdate = q.LSTDATE,
                    Vc = q.VC ?? "",
                    Unit = q.UNIT ?? "",
                    License = q.LICENSE ?? "",
                    Model = q.MODEL ?? "",
                    Moddesc = q.MODDESC ?? "",
                    Duewwd = q.DUEWWD ?? "",
                    Duedate = q.DUEDATE,
                    Duetime = (q.DUETIME == null ? "" : q.DUETIME.Value.ToShortTimeString()),
                    Op = q.OPERSTAT ?? "",
                    Mt = q.MOVETYPE ?? "",
                    Hold = q.CARHOLD1 ?? "",
                    Nr = (q.DAYSREV == null ? "" : q.DAYSREV.ToString()),
                    Driver = q.DRVNAME ?? "",
                    Doc = q.LSTNO ?? "",
                    Lstmlg = (int?)q.LSTMLG ?? 0,
                    Charged = q.RC ?? "",
                    Nonrev = q.DAYSREV ?? 0,
                    Regdate = (q.IDATE == null ? "" : q.IDATE.Value.ToShortDateString()),
                    Ownarea = q.OWNAREA ?? "",
                    Blockdate = (q.CAPDATE == null ? "" : q.CAPDATE.Value.ToShortDateString()),
                    Bddays = (q.BDDAYS == null ? "" : q.BDDAYS.ToString()),
                    Mmdays = (q.MMDAYS == null ? "" : q.MMDAYS.ToString()),
                    Prevwwd = q.PREVWWD ?? "",
                    Remarkdate = "",// (comment == null ? "" : comment.UpdateDate.Value.ToShortDateString()),
                    Remarks = ""//(comment == null ? "" : comment.Comment)
                };

                return carSearchDataEntity;
            }
        }

        public void saveRemark(string license, string remark) {
            // adds a comment to the VehiclesAbroadComments table

            using (MarsDBDataContext db = new MarsDBDataContext()) {

                var q = (from p in db.VehiclesAbroadComments where p.License == license select p).FirstOrDefault();
                if (q == null) {
                    VehiclesAbroadComment vac = new VehiclesAbroadComment { License = license, Comment = remark, UpdateDate = DateTime.Now };
                    db.VehiclesAbroadComments.InsertOnSubmit(vac);
                }
                else {
                    q.Comment = remark;
                    q.UpdateDate = DateTime.Now;
                }
                db.SubmitChanges();
            }
        }
    }
}