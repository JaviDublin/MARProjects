using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.AdditionDeletions;
using Mars.App.Classes.Phase4Dal.Buffers;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.LicenceeRestriction;
using Mars.App.Classes.Phase4Dal.Pooling.Entities;
using Mars.App.Classes.Phase4Dal.Pooling.Enums;
using Mars.App.Classes.Phase4Dal.Reservations;

namespace Mars.App.Classes.Phase4Dal.Pooling
{
    public class DayActualsDataAccess : BaseDataAccess
    {
        public const int HoursForActuals = 72;
        public DayActualsDataAccess(Dictionary<DictionaryParameter, string> parameters)
            : base(parameters, null)
        {
            
        }

        public MarsDBDataContext ShareDataContext()
        {
            return DataContext;
        }

        public List<DayActualsRow> GetActualsRows()
        {
            var actualsRows = new List<DayActualsRow>();
            var addDel = new DayActualsRow { RowName = DayActualTypeTranslator.GetTypeName(DayActualRowType.AdditionDeletion) };
            var av = new DayActualsRow { RowName = DayActualTypeTranslator.GetTypeName(DayActualRowType.Available) };
            var rentOut = new DayActualsRow { RowName = DayActualTypeTranslator.GetTypeName(DayActualRowType.Reservations) };
            var returnsIn = new DayActualsRow { RowName = DayActualTypeTranslator.GetTypeName(DayActualRowType.CheckIns) };
            var buffer = new DayActualsRow { RowName = DayActualTypeTranslator.GetTypeName(DayActualRowType.Buffers) };
            var balance = new DayActualsRow { RowName = DayActualTypeTranslator.GetTypeName(DayActualRowType.Balance) };
            
            var nowAvailable = GetCurrentAvailable();
            var singleBuffer = GetBuffer();

            var ro = GetReservationsCheckOuts();
            var ri = GetReservationCheckIns();
            var ci = GetRentalCheckIns();
            var adds = GetAdditions();
            var dels = GetDeletions();

            var nowTime = DateTime.Now.Date.AddHours(DateTime.Now.Hour);
            int currentAvailable = nowAvailable;
            for (int i = 0; i < HoursForActuals; i++)
            {
                var currentHourSlot = nowTime.AddHours(i);

                var currentAdds = adds.Where(d => d.Date == currentHourSlot).Select(d => d.Number).FirstOrDefault();
                var currentDels = dels.Where(d => d.Date == currentHourSlot).Select(d => d.Number).FirstOrDefault();
                var currentRentalOut = ro.Where(d => d.Date == currentHourSlot).Select(d=> d.Number).FirstOrDefault();
                var currentRentalIn = ri.Where(d => d.Date == currentHourSlot).Select(d => d.Number).FirstOrDefault();
                var currentCheckIn = ci.Where(d => d.Date == currentHourSlot).Select(d => d.Number).FirstOrDefault();

                var currentBalance = currentAvailable - currentRentalOut + currentRentalIn + currentCheckIn;

                addDel.CellValues.Add(new DayActualCell { CellValue = currentAdds + currentDels });
                av.CellValues.Add(new DayActualCell { CellValue = currentAvailable });
                rentOut.CellValues.Add(new DayActualCell { CellValue =  currentRentalOut, LinkButton = true});
                returnsIn.CellValues.Add(new DayActualCell { CellValue = currentRentalIn + currentCheckIn });
                balance.CellValues.Add(new DayActualCell { CellValue = currentBalance - singleBuffer });
                buffer.CellValues.Add(new DayActualCell { CellValue = singleBuffer });
                currentAvailable = currentBalance;
            }
            actualsRows.Add(addDel);
            actualsRows.Add(av);
            actualsRows.Add(rentOut);
            actualsRows.Add(returnsIn);
            actualsRows.Add(buffer);
            actualsRows.Add(balance);
            

            return actualsRows;
        }
        public int GetCurrentAvailable()
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);


            var totalAvailable =
                vehicles.Count(d => d.LastOperationalStatusId == 12 && d.LastMovementTypeId != 10);

            return totalAvailable;
        }

        public int GetBuffer()
        {
            var buffers = BufferFilter.GetBuffers(Parameters, DataContext);
            if (!buffers.Any())
            {
                return 0;
            }
            var returned = buffers.Sum(d => d.Value);
            return returned;
        }

        public List<ActualDataEntity> GetAdditions()
        {
            var additions = AdditionFilter.GetAdditions(Parameters, DataContext);

            var groupedAdditions = from res in additions
                                 group res by new { res.RepDate.Date, res.RepDate.Hour }
                                     into groupedRes
                                     orderby groupedRes.Key.Date, groupedRes.Key.Hour
                                     select new ActualDataEntity
                                     {
                                         Day = groupedRes.Key.Date,
                                         Hour = groupedRes.Key.Hour,
                                         Number = groupedRes.Sum(d => d.Value)
                                     };

            var returned = groupedAdditions.ToList();
            return returned;
        }

        public List<ActualDataEntity> GetDeletions()
        {
            var deletions = DeletionFilter.GetDeletions(Parameters, DataContext);

            var groupedDeletions = from res in deletions
                                   group res by new { res.RepDate.Date, res.RepDate.Hour }
                                       into groupedRes
                                       orderby groupedRes.Key.Date, groupedRes.Key.Hour
                                       select new ActualDataEntity
                                       {
                                           Day = groupedRes.Key.Date,
                                           Hour = groupedRes.Key.Hour,
                                           Number = groupedRes.Sum(d=> d.Value)
                                       };

            var returned = groupedDeletions.ToList();
            return returned;
        }

        public List<ActualDataEntity> GetReservationsCheckOuts()
        {
            var resDataAccess = new ReservationDataAccess(Parameters, DataContext);
            var reservations = resDataAccess.RestrictReservation();

            reservations = RestrictOnLongtermOffAirports(reservations);

            var groupedResData = from res in reservations
                                 group res by new { res.PickupDate.Date, res.PickupDate.Hour}
                                 into groupedRes
                                     orderby groupedRes.Key.Date, groupedRes.Key.Hour
                                     select new ActualDataEntity
                                    {
                                            Day =  groupedRes.Key.Date,
                                            Hour = groupedRes.Key.Hour,
                                            Number = groupedRes.Count()
                                    };

            var returned = groupedResData.ToList();
            return returned;
        }

        private IQueryable<Reservation> RestrictOnLongtermOffAirports(IQueryable<Reservation> reservations)
        {
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ExcludeAirportForLongtermRentals)
                    && Parameters[DictionaryParameter.ExcludeAirportForLongtermRentals] == true.ToString())
            {
                reservations = reservations.Except(reservations.Where(d => d.PickupLocation.location1.Substring(5, 1) != "5"
                                               && SqlMethods.DateDiffDay(d.PickupDate, d.ReturnDate) > 27));
            }
            return reservations;
        }

        public List<ActualDataEntity> GetReservationCheckIns()
        {
            var resDataAccess = new ReservationDataAccess(Parameters, DataContext);

            Parameters[DictionaryParameter.ReservationCheckOutInDateLogic] = false.ToString();
            var reservations = resDataAccess.RestrictReservation();
            Parameters[DictionaryParameter.ReservationCheckOutInDateLogic] = true.ToString();

            reservations = RestrictOnLongtermOffAirports(reservations);

            var groupedResData = from res in reservations
                                 
                                 group res by new { res.ReturnDate.Date, TOtalHours = res.ReturnDate.Hour + res.ReturnLocation.turnaround_hours ?? 0 }
                                     into groupedRes
                                     orderby groupedRes.Key.Date, groupedRes.Key.TOtalHours
                                     select new ActualDataEntity
                                     {
                                         Day = groupedRes.Key.Date,
                                         Hour = groupedRes.Key.TOtalHours,
                                         Number = groupedRes.Count()
                                     };

            var returned = groupedResData.ToList();
            return returned;
        }



        public List<ActualDataEntity> GetRentalCheckIns()
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ExcludeAirportForLongtermRentals)
                && Parameters[DictionaryParameter.ExcludeAirportForLongtermRentals] == true.ToString())
            {
                vehicles = vehicles.Except(vehicles.Where(d => d.LastLocationCode.Substring(5, 1) != "5"
                                               && SqlMethods.DateDiffDay(d.LastChangeDateTime, d.ExpectedDateTime) > 27));
            }

            var startDate = DateTime.Parse(Parameters[DictionaryParameter.StartDate]);
            var endDate = DateTime.Parse(Parameters[DictionaryParameter.EndDate]);

            var totalCheckInVehicles = vehicles.Where(d => d.LastMovementTypeId == 10 
                                                    && d.ExpectedDateTime.HasValue
                                                    && d.ExpectedDateTime >= startDate 
                                                    && d.ExpectedDateTime <= endDate);

            var groupedVehData = from ci in totalCheckInVehicles
                                 join loc in DataContext.LOCATIONs on ci.ExpectedLocationCode equals loc.location1
                                 group ci by new { ci.ExpectedDateTime.Value.Date, TotalHours = ci.ExpectedDateTime.Value.Hour + loc.turnaround_hours ?? 0  }
                                     into groupedRes
                                     orderby groupedRes.Key.Date, groupedRes.Key.TotalHours
                                     select new ActualDataEntity
                                     {
                                         Day = groupedRes.Key.Date,
                                         Hour = groupedRes.Key.TotalHours,
                                         Number = groupedRes.Count()
                                     };
            var returned = groupedVehData.ToList();
            return returned;
        }
    }
}