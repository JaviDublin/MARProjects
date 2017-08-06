using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Pooling.PoolingDataContext;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.DAL.Pooling.Queryables {
    public class FilterQueryable {
        public IQueryable<Reservation> GetQueryable(PoolingDataClassesDataContext db,IMainFilterEntity filter) {
            return  from p in db.Reservations
                    where (p.LOCATION1.COUNTRy1.country_description==filter.Country || String.IsNullOrEmpty(filter.Country))
                    && ((filter.Logic == true
                            && (filter.PoolRegion == p.LOCATION1.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 || String.IsNullOrEmpty(filter.PoolRegion))
                            && (filter.LocationGrpArea == p.LOCATION1.CMS_LOCATION_GROUP.cms_location_group1 || String.IsNullOrEmpty(filter.LocationGrpArea))
                            && (filter.Branch == p.LOCATION1.location1 || String.IsNullOrEmpty(filter.Branch))) 
                    || (filter.Logic == false
                            && (filter.PoolRegion == p.LOCATION1.OPS_AREA.OPS_REGION.ops_region1 || String.IsNullOrEmpty(filter.PoolRegion))
                            && (filter.LocationGrpArea == p.LOCATION1.OPS_AREA.ops_area1 || String.IsNullOrEmpty(filter.LocationGrpArea))
                            && (filter.Branch == p.LOCATION1.location1 || String.IsNullOrEmpty(filter.Branch)))) 
                    && (filter.CarSegment == p.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1 || String.IsNullOrEmpty(filter.CarSegment))
                    && (filter.CarClass == p.CAR_GROUP.CAR_CLASS.car_class1 || String.IsNullOrEmpty(filter.CarClass))
                    && (filter.CarGroup == p.CAR_GROUP.car_group1 || String.IsNullOrEmpty(filter.CarGroup))
                    select p;
        }
    }
}