using System;
using App.BLL;
using App.Classes.DAL.Reservations.Abstract;
using App.Classes.Entities.Reservations.Abstract;

using App.Classes.Entities.Reservations;
using System.Data.SqlClient;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using System.Linq;

namespace App.Classes.DAL.Reservations
{
    public class ReservationLogRepository : IReservationLogRepository
    {
        //ILog _logger = log4net.LogManager.GetLogger("Pooling");
        public IReservationDBUpdateEntity getItem()
        {
            using (var db = new PoolingDataClassesDataContext())
            {
                try
                {
                    var lastReservation =
                        db.ReservationTeradataControlLogs.Where(d => d.Processed).Max(d => d.TeradataTimeStamp);

                    if (!lastReservation.HasValue)
                    {
                        lastReservation = DateTime.MinValue;
                    }

                    
                    string centralZoneId = "Central Standard Time";
                    string gmtZoneId = "GMT Standard Time";
                    TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById(centralZoneId);
                    TimeZoneInfo gmtZone = TimeZoneInfo.FindSystemTimeZoneById(gmtZoneId);

                    var convertedDate = TimeZoneInfo.ConvertTime(lastReservation.Value, centralZone, gmtZone);
                    convertedDate = convertedDate.AddSeconds(-convertedDate.Second);
                    var lastFleet = ImportDetails.GetLastDataImportTime((int)ImportDetails.ImportType.Availability);
                    lastFleet = lastFleet.AddSeconds(-lastFleet.Second);

                    var returned = new ReservationDBUpdateEntity
                                   {
                                       TeraDataMessage = string.Format("Reservations up to: {0} GMT", convertedDate)
                                       , FleetMessage = string.Format("Fleet Last Updated: {0} GMT", lastFleet)
                                   };
                    return returned;
                    //return (from p in db.ResControls
                    //        select new ReservationDBUpdateEntity
                    //               {
                    //                   LastUpdate = p.TimeStamp
                    //                   , Id = p.Id
                    //               }).OrderByDescending(p => p.Id).First();
                }
                catch (SqlException ex)
                {
                    //if (_logger != null) _logger.Error("Exception thrown in ReservationLogRepository, message : " + ex.Message);
                }
                return new ReservationDBUpdateEntity();
            }
        }
    }
}