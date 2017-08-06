using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using App.BLL.DynamicLinq;
using App.BLL.Utilities;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.LicenceeRestriction;
using Mars.App.Classes.Phase4Dal.Reservations.Entities;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal.Reservations
{
    public class ReservationDataAccess : BaseDataAccess
    {

        public ReservationDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {
            
        }

        public IQueryable<Reservation> RestrictReservation()
        {
            var resData = DataContext.Reservations1.Select(d => d);

            resData = ReservationRestriction.RestrictVehicleQueryable(DataContext, resData);
            
            bool checkOutLogic = Parameters[DictionaryParameter.ReservationCheckOutInDateLogic] == true.ToString();
            
            var reservedLogic = Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.UpgradedLogic)
                                && Parameters[DictionaryParameter.UpgradedLogic] == false.ToString();

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.StartDate))
            {
                var startDate = DateTime.Parse(Parameters[DictionaryParameter.StartDate]);
                var endDate = DateTime.Parse(Parameters[DictionaryParameter.EndDate]);
                resData = checkOutLogic
                        ? resData.Where(d => d.PickupDate >= startDate && d.PickupDate <= endDate)
                        : resData.Where(d => d.ReturnDate >= startDate && d.ReturnDate <= endDate);
            }

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ReservationCustomerName))
            {
                var custName = Parameters[DictionaryParameter.ReservationCustomerName];
                resData = resData.Where(d => d.CustomerName == custName);
            }

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ReservationExternalId))
            {
                var externalId = Parameters[DictionaryParameter.ReservationExternalId];
                resData = resData.Where(d => d.ExternalId == externalId);
            }

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ReservationFlightNumber))
            {
                var flightNumber = Parameters[DictionaryParameter.ReservationFlightNumber];
                resData = resData.Where(d => d.FlightNumber == flightNumber);
            }

            //Car Fields

            resData = RestrictByCarFields(resData, reservedLogic);

            //Location Fields

            resData = RestrictByLocation(resData, true);
            resData = RestrictByLocation(resData, false);


            return resData;
        }

        private IQueryable<Reservation> RestrictByCarFields(IQueryable<Reservation> resData, bool reservedLogic)
        {
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                resData = ReservationFieldRestrictions.RestrictByCarGroup(resData,
                    Parameters[DictionaryParameter.CarGroup], reservedLogic);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                resData = ReservationFieldRestrictions.RestrictByCarClass(resData,
                    Parameters[DictionaryParameter.CarClass], reservedLogic);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                resData = ReservationFieldRestrictions.RestrictByCarSegment(resData,
                    Parameters[DictionaryParameter.CarSegment], reservedLogic);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                resData = ReservationFieldRestrictions.RestrictByOwningCountry(resData,
                    Parameters[DictionaryParameter.OwningCountry]);
            }
            return resData;
        }

        private IQueryable<Reservation> RestrictByLocation(IQueryable<Reservation> resData, bool checkOutLogic)
        {
            if (Parameters.ContainsValueAndIsntEmpty(checkOutLogic ? DictionaryParameter.CheckOutLocation : DictionaryParameter.Location))
            {
                resData = ReservationFieldRestrictions.RestrictByLocation(resData,
                    Parameters[checkOutLogic ? DictionaryParameter.CheckOutLocation : DictionaryParameter.Location], checkOutLogic);
            }
            else
            {
                bool foundParameter = false;
                if (Parameters.ContainsValueAndIsntEmpty(checkOutLogic ? DictionaryParameter.CheckOutLocationGroup : DictionaryParameter.LocationGroup))
                {
                    resData = ReservationFieldRestrictions.RestrictByLocationGroup(resData,
                        Parameters[checkOutLogic ? DictionaryParameter.CheckOutLocationGroup : DictionaryParameter.LocationGroup], checkOutLogic);
                    foundParameter = true;
                }
                else if (Parameters.ContainsValueAndIsntEmpty(checkOutLogic ? DictionaryParameter.CheckOutPool : DictionaryParameter.Pool))
                {
                    resData = ReservationFieldRestrictions.RestrictByPool(resData,
                        Parameters[checkOutLogic ? DictionaryParameter.CheckOutPool : DictionaryParameter.Pool], checkOutLogic);
                    foundParameter = true;
                }

                if (!foundParameter)
                {
                    if (Parameters.ContainsValueAndIsntEmpty(checkOutLogic ? DictionaryParameter.CheckOutArea : DictionaryParameter.Area))
                    {
                        resData = ReservationFieldRestrictions.RestrictByArea(resData,
                            Parameters[checkOutLogic ? DictionaryParameter.CheckOutArea : DictionaryParameter.Area], checkOutLogic);
                        foundParameter = true;
                    }
                    else if (Parameters.ContainsValueAndIsntEmpty(checkOutLogic ? DictionaryParameter.CheckOutRegion : DictionaryParameter.Region))
                    {
                        resData = ReservationFieldRestrictions.RestrictByRegion(resData,
                            Parameters[checkOutLogic ? DictionaryParameter.CheckOutRegion : DictionaryParameter.Region], checkOutLogic);
                        foundParameter = true;
                    }
                }

                if (!foundParameter)
                {
                    if (Parameters.ContainsValueAndIsntEmpty(checkOutLogic ? DictionaryParameter.CheckOutCountry : DictionaryParameter.LocationCountry))
                    {
                        resData = ReservationFieldRestrictions.RestrictByLocationCountry(resData,
                                Parameters[checkOutLogic ? DictionaryParameter.CheckOutCountry : DictionaryParameter.LocationCountry], checkOutLogic);
                    }
                }
            }
            return resData;
        }

        public List<ReservationData> GetReservations()
        {
            var resData = RestrictReservation();

  
            var resEntities = from r in resData
                              join lp in DataContext.ResLoyaltyPrograms on r.N1Type equals lp.N1Type
                              into lpj
                              from lp in lpj.DefaultIfEmpty()
                select new ReservationData
                       {
                           ExternalId = r.ExternalId,
                           ReservationId = r.ReservationId,
                           Country = r.Country,
                           PickupLocation = r.PickupLocation.location1,
                           ReturnLocation = r.ReturnLocation.location1,
                           PickupDate = r.PickupDate,
                           ReturnDate = r.ReturnDate,
                           BookedDate = r.BookedDate,
                           CarGroupReserved = r.ReservedCarGroup.car_group1,
                           CarGroupUpgraded = r.UpgradedCarGroup.car_group1,
                           NeverLost  = r.NeverLost,
                           CustomerName = r.CustomerName,
                           FlightNumber = r.FlightNumber,
                           GoldService = lp == null ? "" : lp.ResGoldLevel.GoldLevelName,
                           Tariff = r.Tariff,
                           N1Type = r.N1Type,
                           Remark = r.Remark,
                           Comment = r.Comment
                       };

            var returned = resEntities.ToList();
            return returned;
        }

        public void UpdateReservationComment(int reservationId, string comment)
        {
            var reservation = DataContext.Reservations1.FirstOrDefault(d => d.ReservationId == reservationId);
            if(reservation == null) throw new InvalidDataException("Invalid ReservationId passed to Update Reservation Comment");
            reservation.Comment = comment;
            DataContext.SubmitChanges();
        }

        public string GetLastGwdRequest()
        {
            var lastTimeStamp = DataContext.ReservationTeradataControlLogs.Where(d=> d.Processed).Max(d => d.HertzTimeStamp);
            var returned =  !lastTimeStamp.HasValue ? DateTime.MinValue.ToString("dd/MM/yy HH:mm:ss") : lastTimeStamp.Value.ToString("dd/MM/yy HH:mm:ss");
            return returned + " GMT";
        }
    }
}