using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.DAL.Reservations.Queryables
{
    public class ReservationsSiteFilter
    {
        public IQueryable<Reservation> FilterByReturnLocation(IQueryable<Reservation> q, IMainFilterEntity filter)
        {
            return from p in q
                   where (p.ReturnLocation.COUNTRy1.active)
                   
                   && (filter.Country == p.ReturnLocation.COUNTRy1.country_description || string.IsNullOrEmpty(filter.Country))
                   && ((filter.CmsLogic ? filter.PoolRegion == p.ReturnLocation.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 : filter.PoolRegion == p.ReturnLocation.OPS_AREA.OPS_REGION.ops_region1) || string.IsNullOrEmpty(filter.PoolRegion))
                   && ((filter.CmsLogic ? filter.LocationGrpArea == p.ReturnLocation.CMS_LOCATION_GROUP.cms_location_group1 : filter.LocationGrpArea == p.ReturnLocation.OPS_AREA.ops_area1) || string.IsNullOrEmpty(filter.LocationGrpArea))
                   && (filter.Branch == p.ReturnLocation.served_by_locn || string.IsNullOrEmpty(filter.Branch))

                   select p;
        }

        public IQueryable<Reservation> FilterByRentalLocation(IQueryable<Reservation> q, IMainFilterEntity filter)
        {
            return from p in q
                   where (p.RentalLocation.COUNTRy1.active)
                   && (filter.Country == p.RentalLocation.COUNTRy1.country_description || string.IsNullOrEmpty(filter.Country))
                   && ((filter.CmsLogic ? filter.PoolRegion == p.RentalLocation.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 : filter.PoolRegion == p.RentalLocation.OPS_AREA.OPS_REGION.ops_region1) || string.IsNullOrEmpty(filter.PoolRegion))
                   && ((filter.CmsLogic ? filter.LocationGrpArea == p.RentalLocation.CMS_LOCATION_GROUP.cms_location_group1 : filter.LocationGrpArea == p.RentalLocation.OPS_AREA.ops_area1) || string.IsNullOrEmpty(filter.LocationGrpArea))
                   && (filter.Branch == p.RentalLocation.served_by_locn || string.IsNullOrEmpty(filter.Branch))

                   select p;
        }
    }
}