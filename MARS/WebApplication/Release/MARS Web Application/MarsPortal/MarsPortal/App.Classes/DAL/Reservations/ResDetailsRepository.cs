using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.Reservations.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.Entities.Reservations.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using Mars.Entities.Reservations;
using Mars.DAL.Reservations.Queryables;
using App.Classes.DAL.Reservations.Queryables.SortRepository;
using App.Classes.DAL.Pooling;

namespace Mars.DAL.Reservations
{
    public class ResDetailsRepository : IReservationRepository
    {

        const string CHECKIN = "Check In";
        ReservationsFilterCar _resCarFilterQ;
        ReservationsSiteFilter _resFilterQ;
        ResDetailsQueryable _resDetQ;
        ResDetailCOQueryable _resCOQueryable;
        ResDetailCIQueryable _resCIQueryable;
        ResDetailsBottomFilterQueryable _resBFQ;
        ResDetailsSortQueryable _resSortQ;
        IList<IReservationDetailsEntity> _list;

        public ResDetailsRepository()
        {
            _resCarFilterQ = new ReservationsFilterCar();
            _resFilterQ = new ReservationsSiteFilter();
            _resDetQ = new ResDetailsQueryable();
            _resCOQueryable = new ResDetailCOQueryable();
            _resCIQueryable = new ResDetailCIQueryable();
            _resBFQ = new ResDetailsBottomFilterQueryable(new FilterRepository());
            _resSortQ = new ResDetailsSortQueryable(new DetailsSortRepository());
        }
        public IList<IReservationDetailsEntity> getList(IMainFilterEntity filter, IReservationDetailsFilterEntity rdfe, string sortExpression, string sortDirection)
        {
            using (var db = new PoolingDataClassesDataContext())
            {
                //db.Log = new DebugTextWriter();
                IQueryable<Mars.App.Classes.DAL.Pooling.PoolingDataContext.Reservation> q = _resCarFilterQ.FilterByCarParameters(db, filter);
                
                if (rdfe.CheckInOut == CHECKIN)
                {
                    
                    q = _resFilterQ.FilterByReturnLocation(q, filter);
                    
                    q = _resCIQueryable.GetQueryable(q, rdfe, filter);
                    
                }
                else
                {
                    q = _resFilterQ.FilterByRentalLocation(q, filter);

                    q = _resCOQueryable.GetQueryable(q, rdfe, filter);

                }
                q = _resBFQ.GetQueryable(q, rdfe);
                
                q = _resSortQ.getQueryable(sortExpression, sortDirection, q);


                _list = _resDetQ.getQueryable(db, q).ToList();
                var ss = _list.Count();
                return _list;
            }
        }
        public IReservationDetailsEntity getItem(string resId)
        {
            var returned =  _list.FirstOrDefault(p => p.RES_ID_NBR == resId);
            return returned;

        }
    }
}