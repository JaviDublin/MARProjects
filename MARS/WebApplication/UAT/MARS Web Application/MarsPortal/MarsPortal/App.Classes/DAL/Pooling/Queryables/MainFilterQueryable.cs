using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.DAL.Pooling.Queryables {
    public class MainFilterQueryable {
        public IQueryable<PoolingDataContext.vw_Pooling_FEA> FilterData(IQueryable<PoolingDataContext.vw_Pooling_FEA> q, IMainFilterEntity filter, int numberOfDays) {
            return from p in q
                   where (filter.Country == p.country_description || String.IsNullOrEmpty(filter.Country))
                          && ((filter.Logic == true
                                    && (filter.PoolRegion == p.cms_pool || String.IsNullOrEmpty(filter.PoolRegion))
                                    && (filter.LocationGrpArea == p.cms_location_group || String.IsNullOrEmpty(filter.LocationGrpArea))
                                    && (filter.Branch == p.location || String.IsNullOrEmpty(filter.Branch)))
                          || (filter.Logic == false
                                    && (filter.PoolRegion == p.ops_region || String.IsNullOrEmpty(filter.PoolRegion))
                                    && (filter.LocationGrpArea == p.ops_area || String.IsNullOrEmpty(filter.LocationGrpArea))
                                    && (filter.Branch == p.location || String.IsNullOrEmpty(filter.Branch))))
                         && (filter.CarSegment == p.car_segment || String.IsNullOrEmpty(filter.CarSegment))
                         && (filter.CarClass == p.car_class || String.IsNullOrEmpty(filter.CarClass))
                         && (filter.CarGroup == p.car_group || String.IsNullOrEmpty(filter.CarGroup))
                   select p;
        }
    }
}