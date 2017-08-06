using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess
{
    public class RevenueDataAccess : BaseDataAccess
    {
        public RevenueDataAccess(Dictionary<DictionaryParameter, string> parameters)
        {
            Parameters = parameters;
        }

        public List<RevenueRow> GetRevenueData()
        {
            var revData = from rev in DataContext.RevenueByCommercialCarSegments
                select rev;

            revData = FilterByCar(revData);
            revData = FilterByLocation(revData);

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CommercialCarSegment))
            {
                var commercialCarSegmentId = int.Parse(Parameters[DictionaryParameter.CommercialCarSegment]);
                revData = revData.Where(d => d.CommercialCarSegmentId == commercialCarSegmentId);
            }

            var revenueRows = from fd in revData
                select new RevenueRow
                       {
                           Date = new DateTime(fd.Year, fd.Month, 1),
                           LocationCode = fd.LOCATION.location1,
                           OwningCountry = fd.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description,
                           CarSegment = fd.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1,
                           CarClass = fd.CAR_GROUP.CAR_CLASS.car_class1,
                           CarGroup = fd.CAR_GROUP.car_group1,
                           CommercialCarSegment = fd.CommercialCarSegment.Name,
                           FinanceDays = fd.FinanceDays,
                           Revenue = fd.GrossRevenue
                       };

            var returned = revenueRows.ToList();
            return returned;
        }

        private IQueryable<RevenueByCommercialCarSegment> FilterByCar(IQueryable<RevenueByCommercialCarSegment> revData)
        {
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupId = int.Parse(Parameters[DictionaryParameter.CarGroup]);
                revData = revData.Where(d => d.CarGroupId == carGroupId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassId = int.Parse(Parameters[DictionaryParameter.CarClass]);
                revData = revData.Where(d => d.CAR_GROUP.car_class_id == carClassId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(Parameters[DictionaryParameter.CarSegment]);
                revData = revData.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var country = Parameters[DictionaryParameter.OwningCountry];
                revData = revData.Where(d => d.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == country);
            }
            return revData;
        }

        private IQueryable<RevenueByCommercialCarSegment> FilterByLocation(IQueryable<RevenueByCommercialCarSegment> revData)
        {
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var locationId = int.Parse(Parameters[DictionaryParameter.Location]);
                revData = revData.Where(d => d.LocationId == locationId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
            {
                var locationGroupId = int.Parse(Parameters[DictionaryParameter.LocationGroup]);
                revData = revData.Where(d => d.LOCATION.cms_location_group_id == locationGroupId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {
                var poolId = int.Parse(Parameters[DictionaryParameter.Pool]);
                revData = revData.Where(d => d.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var country = Parameters[DictionaryParameter.LocationCountry];
                revData = revData.Where(d => d.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country == country);
            }
            return revData;
        }
    }
}