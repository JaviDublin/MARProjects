using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using App.BLL.Utilities;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;
using Mars.App.Classes.Phase4Dal.LicenceeRestriction;
using Mars.App.Classes.Phase4Dal.Reservations;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles
{
    public class MatchDataAccess : ReservationDataAccess
    {
        public const int DaysForReservationMatchFuture = 3;

        public MatchDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {

        }

        public List<VehicleMatchGridRow> GetVehicleMatches(int? reservationId = null)
        {
            var reservations = DataContext.Reservations1.Select(d => d);

            
            reservations = ReservationRestriction.RestrictVehicleQueryable(DataContext, reservations);

            


            IQueryable<Vehicle> vehicles;
            if (reservationId == null)
            {
                reservations = reservations.Where(d =>
                                                  d.PickupLocation.country != d.ReturnLocation.country
                                                  && d.PickupDate.Date < DateTime.Now.Date.AddDays(DaysForReservationMatchFuture));
                vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);
                vehicles = BaseVehicleDataAccess.RestrictByAdditionalParameters(Parameters, DataContext, vehicles);   
            }
            else
            {
                vehicles = DataContext.Vehicles.Select(d => d).Where(d => d.IsFleet);
                
                vehicles = VehicleRestriction.RestrictVehicleQueryable(DataContext, vehicles);
                
                reservations = reservations.Where(d => d.ReservationId == reservationId);
            }

            vehicles = VehicleFieldRestrictions.RestrictByMatchPredicament(vehicles);

            var vehicleResCountHolder = from v in vehicles
                                        join r in reservations on new
                                        {
                                            Country = v.OwningCountry,
                                            LocationGroupId = v.LOCATION.cms_location_group_id
                                        }
                                        equals new
                                        {
                                            Country = r.ReturnLocation.country,
                                            LocationGroupId = r.PickupLocation.cms_location_group_id
                                        }
                                        where r.PickupDate > DateTime.Now
                                        group v by v.VehicleId
                                            into groupedReservations
                                            select new { VehicleId = groupedReservations.Key, MatchCount = groupedReservations.Count() };

            IQueryable<VehicleMatchGridRow> matchRowEntities;

            if (reservationId == null)
            {
                matchRowEntities = from v in vehicles
                                   join rch in vehicleResCountHolder on v.VehicleId equals rch.VehicleId into rvch
                                   from rch in rvch.DefaultIfEmpty()
                                   select new VehicleMatchGridRow
                                          {
                                              VehicleId = v.VehicleId,
                                              CarGroup = v.CarGroup,
                                              LastLocation = v.LastLocationCode,
                                              LicensePlate = v.LicensePlate,
                                              ModelDescription = v.ModelDescription,
                                              NonRevDays =
                                                  v.DaysSinceLastRevenueMovement.HasValue ? v.DaysSinceLastRevenueMovement.Value : 0,
                                              OperationalStatusCode = v.Operational_Status.OperationalStatusCode,
                                              OwningCountry = v.OwningCountry,
                                              UnitNumber = v.UnitNumber.HasValue ? v.UnitNumber.Value : 0,
                                              ReservationsMatched = rch == null ? 0 : rch.MatchCount
                                          };
            }
            else
            {
                matchRowEntities = from v in vehicles
                                   join rch in vehicleResCountHolder on v.VehicleId equals rch.VehicleId
                                   select new VehicleMatchGridRow
                                   {
                                       VehicleId = v.VehicleId,
                                       CarGroup = v.CarGroup,
                                       LastLocation = v.LastLocationCode,
                                       LicensePlate = v.LicensePlate,
                                       ModelDescription = v.ModelDescription,
                                       NonRevDays =
                                           v.DaysSinceLastRevenueMovement.HasValue ? v.DaysSinceLastRevenueMovement.Value : 0,
                                       OperationalStatusCode = v.Operational_Status.OperationalStatusCode,
                                       OwningCountry = v.OwningCountry,
                                       UnitNumber = v.UnitNumber.HasValue ? v.UnitNumber.Value : 0,
                                       ReservationsMatched = rch.MatchCount
                                   };
            }


            var returned = ApplyDefaultOrderToVehicles(matchRowEntities).ToList();
            return returned;
        }

        public List<ReservationMatchGridRow> GetReservationMatches(int? vehicleId = null)
        {
            var vehicles = DataContext.Vehicles.Select(d => d).Where(d => d.IsFleet);

            
            vehicles = VehicleRestriction.RestrictVehicleQueryable(DataContext, vehicles);
            

            IQueryable<Reservation> reservations;
            if (vehicleId == null)
            {
                reservations = RestrictReservation();
                vehicles = VehicleFieldRestrictions.RestrictByMatchPredicament(vehicles);
                if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutCountry))
                {
                    var checkOutCountry = Parameters[DictionaryParameter.CheckOutCountry];
                    vehicles = from v in vehicles
                               where v.LOCATION.country == checkOutCountry
                               select v;
                }

                var localVehicles = vehicles.ToList();
                var localReservations = reservations.ToList();

                var reservationVehicleCountHolder = from v in localVehicles
                                                    join r in localReservations on new
                                                    {
                                                        Country = v.OwningCountry,
                                                        LocationGroupId = v.LOCATION.cms_location_group_id
                                                    }
                                        equals new
                                        {
                                            Country = r.ReturnLocation.country,
                                            LocationGroupId = r.PickupLocation.cms_location_group_id
                                        }
                                                    where r.PickupDate > DateTime.Now
                                                    group r by r.ReservationId
                                                        into groupedVehicles
                                                        select new { ReservationId = groupedVehicles.Key, MatchCount = groupedVehicles.Count() };


                var reservationGridData = from r in localReservations
                                          join vch in reservationVehicleCountHolder on r.ReservationId equals vch.ReservationId into rvch
                                          from vch in rvch.DefaultIfEmpty()
                                          select new ReservationMatchGridRow
                                          {
                                              ReservationId = r.ReservationId,
                                              ExternalId = r.ExternalId,
                                              PickupDate = r.PickupDate,
                                              PickupLocation = r.PickupLocation.location1,
                                              CarGroup = r.CAR_GROUP.car_group1,
                                              CustomerName = r.CustomerName,
                                              //ReservationDuration = SqlMethods.DateDiffDay(r.PickupDate.Date, r.ReturnDate.Date),
                                              //DaysToPickup = SqlMethods.DateDiffDay(DateTime.Now, r.PickupDate.Date),
                                              ReservationDuration = (int)(r.ReturnDate.Date - r.PickupDate.Date).TotalDays,
                                              DaysToPickup = (int)(r.PickupDate.Date - DateTime.Now.Date).TotalDays,
                                              VehiclesMatched = vch == null ? 0 : vch.MatchCount
                                          };
                var returned = from r in reservationGridData
                               orderby r.DaysToPickup, r.PickupLocation, r.CarGroup
                               select r;


                return returned.ToList();
            }
            else
            {
                reservations = DataContext.Reservations1.Select(d => d).Where(d => d.PickupDate > DateTime.Now
                                    && d.PickupDate < DateTime.Now.Date.AddDays(DaysForReservationMatchFuture)
                                    );
                vehicles = vehicles.Where(d => d.VehicleId == vehicleId);

                var reservationVehicleCountHolder = from v in vehicles
                                                    join r in reservations on new
                                                    {
                                                        Country = v.OwningCountry,
                                                        LocationGroupId = v.LOCATION.cms_location_group_id
                                                    }
                                                    equals new
                                                    {
                                                        Country = r.ReturnLocation.country,
                                                        LocationGroupId = r.PickupLocation.cms_location_group_id
                                                    }
                                        group r by r.ReservationId
                                            into groupedVehicles
                                            select new { ReservationId = groupedVehicles.Key, MatchCount = groupedVehicles.Count() };


                var reservationGridData = from r in reservations
                                          join vch in reservationVehicleCountHolder on r.ReservationId equals vch.ReservationId
                                          select new ReservationMatchGridRow
                                          {
                                              ReservationId = r.ReservationId,
                                              ExternalId = r.ExternalId,
                                              PickupDate = r.PickupDate,
                                              PickupLocation = r.PickupLocation.location1,
                                              CarGroup = r.CAR_GROUP.car_group1,
                                              CustomerName = r.CustomerName,
                                              ReservationDuration = SqlMethods.DateDiffDay(r.PickupDate.Date, r.ReturnDate.Date),
                                              DaysToPickup = SqlMethods.DateDiffDay(DateTime.Now, r.PickupDate.Date),
                                              VehiclesMatched = vch == null ? 0 : vch.MatchCount
                                          };

                var returned = ApplyDefaultOrderToReservations(reservationGridData);


                return returned.ToList();
            }


        }

        public IQueryable<VehicleMatchGridRow> ApplyDefaultOrderToVehicles(IQueryable<VehicleMatchGridRow> vehicleData)
        {
            var orderedVehicleData = from v in vehicleData
                                     orderby v.OwningCountry, v.LastLocation, v.NonRevDays descending, v.CarGroup, v.LicensePlate
                                     select v;
            return orderedVehicleData;
        }

        public IQueryable<ReservationMatchGridRow> ApplyDefaultOrderToReservations(IQueryable<ReservationMatchGridRow> reservationData)
        {
            var orderedReservationData = from r in reservationData
                                         orderby r.DaysToPickup, r.PickupLocation, r.CarGroup
                                         select r;
            return orderedReservationData;
        }

        public string GetReservationMatchExcelExport()
        {
            var vehicles = DataContext.Vehicles.Select(d => d).Where(d => d.IsFleet);
            IQueryable<Reservation> reservations = RestrictReservation();


            vehicles = VehicleFieldRestrictions.RestrictByMatchPredicament(vehicles);

            var reservationVehicleCountHolder = from r in reservations
                                                join v in vehicles on new
                                                {
                                                    Country = r.ReturnLocation.country,
                                                    LocationGroupId = r.PickupLocation.cms_location_group_id
                                                }
                                                equals new
                                                {
                                                    Country = v.OwningCountry,
                                                    LocationGroupId = v.LOCATION.cms_location_group_id
                                                }

                                                where r.PickupDate > DateTime.Now
                                                orderby r.ReservationId
                                                select new
                                                {
                                                    v.OwningCountry
                                                 , r.ReservationId
                                                 , CurrentLocation = v.LOCATION.location1
                                                 , v.CarGroup
                                                 , v.LicensePlate
                                                 , v.UnitNumber
                                                 , v.ModelDescription
                                                 , v.DaysSinceLastRevenueMovement
                                                 , DaysUntilCheckout = SqlMethods.DateDiffDay(r.PickupDate.Date, r.ReturnDate.Date)
                                                 , CheckoutDate = r.PickupDate.Date
                                                 , CarGroupReserved = v.CarGroup
                                                 , r.ExternalId
                                                 , r.CustomerName
                                                 , Length = SqlMethods.DateDiffDay(DateTime.Now, r.PickupDate.Date)
                                                };




            var sb = new StringBuilder();

            sb.AppendLine(string.Format("Reservation:, {0},{1},{2},{3},{4},{5},{6}, Vehicle: ,{7},{8},{9},{10},{11},{12},{13}"
                    , "Days Until Checkout", "Check Out Date", "Check Out Location", "Group Reserved", "Reservation ID", "Customer"
                    , "Length", "Owning Country", "Current Location", "Group", "License", "Unit", "Description"
                    , "Days Non Rev"));

            //const string emptyReservationData = " , , , , , , ";
           // int previousReservationId = 0;
            foreach (var r in reservationVehicleCountHolder)
            {
                var vehicleData = string.Format("{0},{1},{2},{3},{4},{5},{6}", r.OwningCountry, r.CurrentLocation,
                                                r.CarGroup, r.LicensePlate
                                                , r.UnitNumber, r.ModelDescription, r.DaysSinceLastRevenueMovement);
                var reservationData = string.Format("{0},{1},{2},{3},{4},{5},{6}", r.DaysUntilCheckout,
                                                    r.CheckoutDate.ToShortDateString(), r.CurrentLocation, r.CarGroupReserved, r.ExternalId,
                                                    r.CustomerName, r.Length);
                sb.AppendLine(string.Format(" ,{0}, , {1}", reservationData,
                                            vehicleData));

                //previousReservationId = r.ReservationId;

            }


            return sb.ToString();
        }

        public string GetFleetMatchExcelExport()
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);
            vehicles = BaseVehicleDataAccess.RestrictByAdditionalParameters(Parameters, DataContext, vehicles);

            IQueryable<Reservation> reservations =
                DataContext.Reservations1.Select(d => d).Where(d => d.PickupDate > DateTime.Now
                        && d.PickupDate <= DateTime.Now.Date.AddDays(DaysForReservationMatchFuture));


            vehicles = VehicleFieldRestrictions.RestrictByMatchPredicament(vehicles);

            var reservationVehicleCountHolder = from v in vehicles
                                                join r in reservations on new
                                                {
                                                    Country = v.OwningCountry,
                                                    LocationGroupId = v.LOCATION.cms_location_group_id
                                                }
                                                    equals new
                                                    {
                                                        Country = r.ReturnLocation.country,
                                                        LocationGroupId = r.PickupLocation.cms_location_group_id
                                                    }
                                                orderby v.VehicleId
                                                select new
                                                { 
                                                   v.OwningCountry
                                                 , v.VehicleId
                                                 , CurrentLocation = v.LOCATION.location1
                                                 , v.CarGroup
                                                 , v.LicensePlate
                                                 , v.UnitNumber
                                                 , v.ModelDescription
                                                 , v.DaysSinceLastRevenueMovement
                                                 , DaysUntilCheckout = SqlMethods.DateDiffDay(r.PickupDate.Date, r.ReturnDate.Date)
                                                 , CheckoutDate = r.PickupDate.Date
                                                 , CarGroupReserved = v.CarGroup
                                                 , r.ExternalId
                                                 , r.CustomerName
                                                 , r.PickupLocation.location1
                                                 , Length = SqlMethods.DateDiffDay(DateTime.Now, r.PickupDate.Date)
                                                };




            var sb = new StringBuilder();

            sb.AppendLine(string.Format("Vehicle:, {0},{1},{2},{3},{4},{5},{6}, Reservation: ,{7},{8},{9},{10},{11},{12},{13}"
                    , "Owning Country", "Current Location", "Group", "License", "Unit", "Description"
                    , "Days Non Rev", "Days Until Checkout", "Check Out Date", "Check Out Location", "Group Reserved", "Reservation ID", "Customer"
                    , "Length"));

            //const string emptyVehicleData = " , , , , , ,";
            //int previousVehicleId = 0;
            foreach (var r in reservationVehicleCountHolder)
            {
                var vehicleData = string.Format("{0},{1},{2},{3},{4},{5},{6}", r.OwningCountry, r.CurrentLocation,
                                                r.CarGroup, r.LicensePlate
                                                , r.UnitNumber, r.ModelDescription, r.DaysSinceLastRevenueMovement);
                var reservationData = string.Format("{0},{1},{2},{3},{4},{5},{6}", r.DaysUntilCheckout,
                                                    r.CheckoutDate.ToShortDateString(), r.location1, r.CarGroupReserved, r.ExternalId,
                                                    r.CustomerName, r.Length);
                sb.AppendLine(string.Format(" ,{0}, ,{1}", vehicleData, reservationData));

                //previousVehicleId = r.VehicleId;

            }


            return sb.ToString();
        }

        
    }
}