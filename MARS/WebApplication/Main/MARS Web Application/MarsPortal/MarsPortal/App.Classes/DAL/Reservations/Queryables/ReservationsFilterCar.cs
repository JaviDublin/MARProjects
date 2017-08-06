using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.Entities.Reservations.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using Mars.Entities.Reservations;

namespace Mars.DAL.Reservations.Queryables
{
    public class ReservationsFilterCar
    {
        public IQueryable<Mars.App.Classes.DAL.Pooling.PoolingDataContext.Reservation> FilterByCarParameters(PoolingDataClassesDataContext db, IMainFilterEntity filter, bool isReturnLocation = false)
        {

            var returned = from p in db.Reservations
                           join c in db.COUNTRies on p.COUNTRY equals c.country1
                           where (p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 == filter.CarSegment || String.IsNullOrEmpty(filter.CarSegment))
                           && (p.RentalLocation.COUNTRy1.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                           && (c.country_description == filter.Country || String.IsNullOrEmpty(filter.Country))
                           && (p.CAR_GROUP.CAR_CLASS.car_class1 == filter.CarClass || String.IsNullOrEmpty(filter.CarClass))
                           && (p.CAR_GROUP.car_group1 == filter.CarGroup || String.IsNullOrEmpty(filter.CarGroup))
                           select p;

            
            if (filter.ExcludeLongterm)
            {
                if (isReturnLocation)
                {
                    //returned = returned.Where(d => !(d.ReturnLocation.location1.Substring(5, 1) != "5" ||
                    //                           SqlMethods.DateDiffDay(d.RS_ARRIVAL_DATE, d.RTRN_DATE) > 27));

                    returned = returned.Except(returned.Where(d => d.ReturnLocation.served_by_locn.Substring(5, 1) != "5" &&
                                               SqlMethods.DateDiffDay(d.RS_ARRIVAL_DATE, d.RTRN_DATE) > 27));
                }
                else
                {
                    //returned = returned.Where(d => !(d.RentalLocation.location1.Substring(5, 1) != "5" ||
                    //                               SqlMethods.DateDiffDay(d.RS_ARRIVAL_DATE, d.RTRN_DATE) > 27));

                    returned = returned.Except(returned.Where(d => d.RentalLocation.served_by_locn.Substring(5, 1) != "5" &&
                                               SqlMethods.DateDiffDay(d.RS_ARRIVAL_DATE, d.RTRN_DATE) > 27));
                }
                
            }
            

            return returned;
        }
    }
}