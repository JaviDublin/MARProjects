using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.ComponentModel;
using System.EnterpriseServices;
using System.Globalization;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.LicenceeRestriction;
using Mars.App.Classes.Phase4Dal.ManagerReports.Entities;

namespace Mars.App.Classes.Phase4Dal.ManagerReports
{
    public class LocationSummaryDataAccess : IDisposable
    {
        protected MarsDBDataContext DataContext;

        public LocationSummaryDataAccess()
        {
            DataContext = new MarsDBDataContext();
        }

        public int GetLocationIdFromCode(string locationCode)
        {
            var locationEntity = DataContext.LOCATIONs.SingleOrDefault(d => d.location1 == locationCode);
            if (locationEntity == null) return 0;
            return locationEntity.dim_Location_id;
        }

        public string GetLocationName(int locationId)
        {
            return DataContext.LOCATIONs.Single(d => d.dim_Location_id == locationId).location_name;
        }

        public List<LocationSummaryRow> GetFleetStatusData(Dictionary<DictionaryParameter, string> parameters)
        {
            var locationId = int.Parse(parameters[DictionaryParameter.Location]);
            var countryId = DataContext.LOCATIONs.Single(d => d.dim_Location_id == locationId).COUNTRy1.CountryId;


            var availabilityRows = GetAvailabilitySummaryData(parameters);
            var nonRevRows = GetNonRevSummaryData(parameters);
            var resCheckIn = GetReservationCheckInData(parameters);
            var resCheckOut = GetReservationCheckOutData(parameters);

            var returned = CombineSummaryRows(countryId, availabilityRows, nonRevRows, resCheckIn, resCheckOut);

            return returned;
        }



        public List<LocationSummaryForeignRow> GetForeignFleetStatusData(Dictionary<DictionaryParameter, string> parameters)
        {
            var vehicleDetails = GetForeignVehicleSummaryData(parameters).ToList();
            var reservationDetails = GetForeignReservationCheckOutData(parameters);

            var combinedCarSegments =
                vehicleDetails.Select(d => d.CarSegmentName).Union(reservationDetails.Select(d => d.CarSegmentName)).Distinct();

            var returned = new List<LocationSummaryForeignRow>();
            foreach (var cs in combinedCarSegments)
            {
                var segmentName = cs;
                var vd = vehicleDetails.SingleOrDefault(d => d.CarSegmentName == segmentName);
                var rd = reservationDetails.SingleOrDefault(d => d.CarSegmentName == segmentName);
                var fr = new LocationSummaryForeignRow
                         {
                             CarSegmentName = segmentName,
                             VehicleCount = vd == null ? 0 : vd.VehicleCount,
                             ReservationCount = rd == null ? 0 : rd.ReservationCount
                         };
                returned.Add(fr);
            }
            
            return returned;
        }

        private List<LocationSummaryRow> CombineSummaryRows(int countryId
                                    , IEnumerable<LocationSummaryRow> availabilityData
                                    , IEnumerable<LocationSummaryRow> nonRevData
                                    , IEnumerable<LocationSummaryRow> reservationCheckIn
                                    , IEnumerable<LocationSummaryRow> reservationCheckOut)
        {
            var emptyData = from cc in DataContext.CAR_CLASSes
                where cc.CAR_SEGMENT.COUNTRy1.CountryId == countryId
                orderby cc.car_segment_id, cc.car_class_id
                select new LocationSummaryRow
                       {
                           CarClassName = cc.car_class1,
                           CarSegmentName = cc.CAR_SEGMENT.car_segment1,
                           RowId = cc.car_class_id
                       };

            var localEmptyData = emptyData.ToList();


            var joinedData = from ed in localEmptyData
                join ad in availabilityData on ed.RowId equals ad.RowId into adJoined 
                from ad in adJoined.DefaultIfEmpty()
                join nr in nonRevData on ed.RowId equals nr.RowId into nrJoined
                from nr in nrJoined.DefaultIfEmpty()
                join ci in reservationCheckIn on ed.RowId equals ci.RowId into ciJoined
                from ci in ciJoined.DefaultIfEmpty()
                join co in reservationCheckOut on ed.RowId equals co.RowId into coJoined
                from co in coJoined.DefaultIfEmpty()
                select new LocationSummaryRow
                       {
                           RowId = ed.RowId,
                           CarClassName = ed.CarClassName,
                           CarSegmentName = ed.CarSegmentName,
                           AvailabilityOp = ad == null ? 0 : ad.AvailabilityOp,
                           AvailabilityShop = ad == null ? 0 : ad.AvailabilityShop,
                           AvailabilityAvailable = ad == null ? 0 : ad.AvailabilityAvailable,
                           AvailabilityOnRent = ad == null ? 0 : ad.AvailabilityOnRent,
                           AvailabilityIdle = ad == null ? 0 : ad.AvailabilityIdle,
                           AvailabilityOverdue = ad == null ? 0 : ad.AvailabilityOverdue,
                           AvailabilityUtilization = ad == null ? 0 : ad.AvailabilityUtilization,
                           NonRevGreaterThanThree = nr == null ? 0 : nr.NonRevGreaterThanThree,
                           NonRevGreaterThanSeven = nr == null ? 0 : nr.NonRevGreaterThanSeven,
                           ReservationCheckInToday = ci == null ? 0 : ci.ReservationCheckInToday,
                           ReservationCheckInRemaining = ci == null ? 0 : ci.ReservationCheckInRemaining,
                           ReservationCheckOutToday = co == null ? 0 : co.ReservationCheckOutToday,
                           ReservationCheckOutRemaining = co == null ? 0 : co.ReservationCheckOutRemaining,
                       };
            var returned = joinedData.ToList();
            return returned;
        }

        private IEnumerable<LocationSummaryRow> GetReservationCheckInData(
            Dictionary<DictionaryParameter, string> parameters)
        {
            var locationId = int.Parse(parameters[DictionaryParameter.Location]);
            var nowDate = DateTime.Now.Date.AddHours(DateTime.Now.Hour);

            var resData = DataContext.Reservations1.Select(d => d);
            resData = ReservationRestriction.RestrictVehicleQueryable(DataContext, resData);

            var checkInData = from res in resData
                                where res.ReturnDate.Date == DateTime.Now.Date
                                     && res.ReturnLocationId == locationId
                               group res by res.ReservedCarGroup.car_class_id
                                   into groupedData
                                   select new LocationSummaryRow
                                   {
                                       RowId = groupedData.Key,
                                       ReservationCheckInToday = groupedData.Count(),
                                       ReservationCheckInRemaining = groupedData.Count(d => d.ReturnDate > nowDate)
                                   };

            var returned = checkInData.ToList();
            return returned;
        }

        private IEnumerable<LocationSummaryRow> GetReservationCheckOutData(
            Dictionary<DictionaryParameter, string> parameters)
        {
            var locationId = int.Parse(parameters[DictionaryParameter.Location]);
            var nowDate = DateTime.Now.Date.AddHours(DateTime.Now.Hour);

            var resData = DataContext.Reservations1.Select(d => d);
            resData = ReservationRestriction.RestrictVehicleQueryable(DataContext, resData);

            var checkOutData = from res in resData
                               where res.PickupDate.Date == DateTime.Now.Date
                                     && res.PickupLocationId == locationId
                               group res by res.ReservedCarGroup.car_class_id
                                   into groupedData
                                   select new LocationSummaryRow
                                   {
                                       RowId = groupedData.Key,
                                       ReservationCheckOutToday = groupedData.Count(),
                                       ReservationCheckOutRemaining = groupedData.Count(d => d.PickupDate > nowDate)
                                   };


            var returned = checkOutData.ToList();

            return returned;
        }

        private List<LocationSummaryForeignRow> GetForeignReservationCheckOutData(
            Dictionary<DictionaryParameter, string> parameters)
        {
            var locationId = int.Parse(parameters[DictionaryParameter.Location]);
            var nowDate = DateTime.Now.Date.AddHours(DateTime.Now.Hour);

            var resData = DataContext.Reservations1.Select(d => d);
            resData = ReservationRestriction.RestrictVehicleQueryable(DataContext, resData);


            var checkOutData = from res in resData
                               where res.PickupDate.Date == DateTime.Now.Date
                                     && res.PickupLocationId == locationId
                                     && res.ReturnLocation.COUNTRy1.CountryId != res.PickupLocation.COUNTRy1.CountryId
                               group res by res.ReservedCarGroup.CAR_CLASS.CAR_SEGMENT.car_segment1
                                   into groupedData
                                   select new LocationSummaryForeignRow
                                   {
                                       CarSegmentName = groupedData.Key,
                                       ReservationCount = groupedData.Count(d => d.PickupDate > nowDate)
                                   };


            var returned = checkOutData.ToList();

            return returned;
        }

        private IEnumerable<LocationSummaryRow> GetNonRevSummaryData(
            Dictionary<DictionaryParameter, string> parameters)
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(parameters, DataContext, false, true);
            var carClassGroupedData = GroupByCarClass(vehicles);

            var lsrData = BuildLocationSummaryNonRevRows(carClassGroupedData);

            var returned = lsrData.ToList();
            return returned;
        }

        private IEnumerable<LocationSummaryRow> BuildLocationSummaryNonRevRows(IQueryable<IGrouping<string, Vehicle>> groupedQueryable)
        {
            var lsrData = from gq in groupedQueryable
                          select new LocationSummaryRow
                          {
                              RowId = int.Parse(gq.Key),
                              NonRevGreaterThanThree = gq.Sum(d => d.DaysSinceLastRevenueMovement > 3 ? 1 : 0),
                              NonRevGreaterThanSeven = gq.Sum(d => d.DaysSinceLastRevenueMovement > 7 ? 1 : 0),
                          };
            return lsrData;
        }

        private IEnumerable<LocationSummaryRow> GetAvailabilitySummaryData(Dictionary<DictionaryParameter, string> parameters)
        {
            var fleetStatusDataAccess = new ComparisonDataAccess(parameters, DataContext);

            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(parameters, DataContext, true, true);


            var carClassGroupedData = GroupByCarClass(vehicles);

            var extractedData = fleetStatusDataAccess.ExtractVehicleColumns(carClassGroupedData);
            var availabilityRows = BuildLocationSummaryAvailabilityRows(extractedData);
            return availabilityRows;
        }

        private IEnumerable<LocationSummaryForeignRow> GetForeignVehicleSummaryData(Dictionary<DictionaryParameter, string> parameters)
        {
            var fleetStatusDataAccess = new ComparisonDataAccess(parameters, DataContext);

            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(parameters, DataContext, true, true);


            vehicles = from v in vehicles
                       where v.LastLocationCode.Substring(0, 2) != v.OwningCountry
                       select v;

            var carClassGroupedData = GroupByCarSegmentName(vehicles);

            var extractedData = fleetStatusDataAccess.ExtractVehicleColumns(carClassGroupedData);
            var availabilityRows = BuildLocationSummaryForeignRows(extractedData);
            return availabilityRows;
        }

        private IQueryable<IGrouping<string, Vehicle>> GroupByCarClass(IQueryable<Vehicle> vehicles )
        {
            var carClassGroupedData = from v in vehicles
                                      join cg in DataContext.CAR_GROUPs on new { v.CarGroup, v.OwningCountry } equals
                                                    new { CarGroup = cg.car_group1, OwningCountry = cg.CAR_CLASS.CAR_SEGMENT.country }
                                      group v by cg.car_class_id.ToString()
                                          into gd
                                          select gd;
            return carClassGroupedData;
        }

        private IQueryable<IGrouping<string, Vehicle>> GroupByCarSegmentName(IQueryable<Vehicle> vehicles)
        {
            var carClassGroupedData = from v in vehicles
                                      join cg in DataContext.CAR_GROUPs on new { v.CarGroup, v.OwningCountry } equals
                                                    new { CarGroup = cg.car_group1, OwningCountry = cg.CAR_CLASS.CAR_SEGMENT.country }
                                      group v by cg.CAR_CLASS.CAR_SEGMENT.car_segment1
                                          into gd
                                          select gd;
            return carClassGroupedData;
        }

        private IEnumerable<LocationSummaryRow> BuildLocationSummaryAvailabilityRows(IQueryable<FleetStatusRow> fe)
        {
            var data = fe.Select(fleetRow => new LocationSummaryRow
                                         {
                                             RowId = int.Parse(fleetRow.Key),
                                             AvailabilityOp = fleetRow.OperationalFleet,
                                             AvailabilityShop = (fleetRow.Tw + fleetRow.Bd + fleetRow.Mm),
                                             AvailabilityAvailable = fleetRow.AvailableFleet,
                                             AvailabilityOnRent = fleetRow.OnRent,
                                             AvailabilityIdle = fleetRow.Idle,
                                             AvailabilityOverdue = fleetRow.Overdue,
                                             AvailabilityUtilization = fleetRow.UtilizationPercent
                                         });

            var returned = data.ToList();
            return returned;
        }

        private IEnumerable<LocationSummaryForeignRow> BuildLocationSummaryForeignRows(IQueryable<FleetStatusRow> fe)
        {
            var data = fe.Select(fleetRow => new LocationSummaryForeignRow
            {
                CarSegmentName = fleetRow.Key,
                VehicleCount = fleetRow.Idle
            });

            var returned = data.ToList();
            return returned;
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        public Dictionary<DictionaryParameter, string> GetDictionaryForLocation(int locationId)
        {
            var locationEntity = DataContext.LOCATIONs.Single(d => d.dim_Location_id == locationId);
            var countryCode = locationEntity.country;
            var poolId = locationEntity.CMS_LOCATION_GROUP.cms_pool_id;
            var locationGroupId = locationEntity.cms_location_group_id;

            var today = DateTime.Now.Date.ToShortDateString();

            var returned = new Dictionary<DictionaryParameter, string>
                           {
                               {DictionaryParameter.Location, locationId.ToString(CultureInfo.InvariantCulture)}
                               ,{DictionaryParameter.LocationCountry, countryCode}
                               ,{DictionaryParameter.OwningCountry, countryCode}
                               ,{DictionaryParameter.Pool, poolId.ToString(CultureInfo.InvariantCulture)}
                               ,{DictionaryParameter.LocationGroup, locationGroupId.ToString()}
                               , {DictionaryParameter.FleetTypes, string.Empty}
                               , {DictionaryParameter.StartDate, today}
                               , {DictionaryParameter.EndDate, today }
                           };

            return returned;
        }
    }
}