using System;
using System.Collections.Generic;
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Entities.Graphing.Parameters;

namespace App.DAL.MarsDataAccess.ParameterAccess
{
    internal static class ForecastParameterRestriction
    {
        internal static IQueryable<MARS_CMS_NECESSARY_FLEET> RestrictMarsCmsNessesaryFleetByParameters(IDictionary<string, string> parameters
                , IQueryable<MARS_CMS_NECESSARY_FLEET> resultSet, MarsDBDataContext mdc)
        {
            var country = parameters.ContainsKey(ParameterNames.Country) ? parameters[ParameterNames.Country] : null;
            var carClassId = parameters.ContainsKey(ParameterNames.CarClass) ? parameters[ParameterNames.CarClass] : null;
            var locationGroupId = parameters.ContainsKey(ParameterNames.LocationGroup) ? parameters[ParameterNames.LocationGroup] : null;


            if (!string.IsNullOrEmpty(carClassId))
            {
                resultSet = from r in resultSet
                            join cg in mdc.CAR_GROUPs on r.CAR_CLASS_ID equals cg.car_group_id
                            where r.CAR_CLASS_ID == int.Parse(carClassId)
                            select r;
            } else if (!string.IsNullOrEmpty(locationGroupId))
            {
                resultSet = from r in resultSet
                            where r.CMS_LOCATION_GROUP_ID == int.Parse(locationGroupId)
                            select r;
            } else if (!String.IsNullOrEmpty(country))
            {
                resultSet = from p in resultSet
                            where p.COUNTRY == country
                            select p;
            }


            return resultSet;

        }

        internal static IQueryable<MARS_CMS_FORECAST> RestrictMarsCmsForecastByParameters(IDictionary<string, string> parameters
                , IQueryable<MARS_CMS_FORECAST> resultSet, MarsDBDataContext mdc)
        {
            var fromDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.FromDate) ? parameters[ParameterNames.FromDate] : null);
            var toDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.ToDate) ? parameters[ParameterNames.ToDate] : null);

            var country = parameters.ContainsKey(ParameterNames.Country) ? parameters[ParameterNames.Country] : null;
            var carSegmentId = parameters.ContainsKey(ParameterNames.CarSegment) ? parameters[ParameterNames.CarSegment] : null;
            var carClassGroupId = parameters.ContainsKey(ParameterNames.CarClassGroup) ? parameters[ParameterNames.CarClassGroup] : null;
            var carClassId = parameters.ContainsKey(ParameterNames.CarClass) ? parameters[ParameterNames.CarClass] : null;
            var cmsPoolId = parameters.ContainsKey(ParameterNames.Pool) ? parameters[ParameterNames.Pool] : null;
            var locationGroupId = parameters.ContainsKey(ParameterNames.LocationGroup) ? parameters[ParameterNames.LocationGroup] : null;

            if(string.IsNullOrEmpty(carSegmentId) && string.IsNullOrEmpty(cmsPoolId) )
            {
                if (!String.IsNullOrEmpty(country))
                {
                    resultSet = from p in resultSet
                                where p.COUNTRY == country
                                select p;
                }
            }

            if (!string.IsNullOrEmpty(locationGroupId))
            {
                resultSet = from r in resultSet
                            where r.CMS_LOCATION_GROUP_ID == int.Parse(locationGroupId)
                            select r;
            }
            else if (!string.IsNullOrEmpty(cmsPoolId))
            {
                resultSet = from r in resultSet
                            join lg in mdc.CMS_LOCATION_GROUPs on r.CMS_LOCATION_GROUP_ID equals lg.cms_location_group_id
                            where lg.cms_pool_id == int.Parse(cmsPoolId)
                            select r;
            }

            if (!string.IsNullOrEmpty(carClassId))
            {
                resultSet = from r in resultSet
                            join cg in mdc.CAR_GROUPs on r.CAR_CLASS_ID equals cg.car_group_id
                            where r.CAR_CLASS_ID == int.Parse(carClassId)
                            select r;
            } else if (!string.IsNullOrEmpty(carClassGroupId))
            {
                resultSet = from r in resultSet
                            join cg in mdc.CAR_GROUPs on r.CAR_CLASS_ID equals cg.car_group_id
                            where cg.car_class_id == int.Parse(carClassGroupId)
                            select r;
            } else if (!string.IsNullOrEmpty(carSegmentId))
            {
                resultSet = from r in resultSet
                            join ccg in mdc.CAR_GROUPs on r.CAR_CLASS_ID equals ccg.car_group_id
                            where ccg.CAR_CLASS.car_segment_id == int.Parse(carSegmentId)
                            select r;
            }

            if (fromDate != null)
            {
                resultSet = resultSet.Where(d => d.REP_DATE >= fromDate && d.REP_DATE <= toDate);
            }

            return resultSet;
        }


        internal static IQueryable<FleetSizeFutureTrend> RestrictFutureTrendDataByParameters(IDictionary<string, string> parameters
            , IQueryable<FleetSizeFutureTrend> resultSet, MarsDBDataContext mdc,
                    int fleetPlanId = 1)
        {
            var fromDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.FromDate) ? parameters[ParameterNames.FromDate] : null);
            var toDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.ToDate) ? parameters[ParameterNames.ToDate] : null);

            var country = parameters.ContainsKey(ParameterNames.Country) ? parameters[ParameterNames.Country] : null;
            var carSegmentId = parameters.ContainsKey(ParameterNames.CarSegment) ? parameters[ParameterNames.CarSegment] : null;
            var carClassGroupId = parameters.ContainsKey(ParameterNames.CarClassGroup) ? parameters[ParameterNames.CarClassGroup] : null;
            var carClassId = parameters.ContainsKey(ParameterNames.CarClass) ? parameters[ParameterNames.CarClass] : null;
            var cmsPoolId = parameters.ContainsKey(ParameterNames.Pool) ? parameters[ParameterNames.Pool] : null;
            var locationGroupId = parameters.ContainsKey(ParameterNames.LocationGroup) ? parameters[ParameterNames.LocationGroup] : null;

            if (!String.IsNullOrEmpty(country))
            {
                resultSet = from p in resultSet
                            where p.Country == country
                            select p;
            }

            if (!string.IsNullOrEmpty(locationGroupId))
            {
                resultSet = from r in resultSet
                            where r.LocGrpId == int.Parse(locationGroupId)
                            select r;
            }

            if (!string.IsNullOrEmpty(cmsPoolId))
            {
                resultSet = from r in resultSet
                            join lg in mdc.CMS_LOCATION_GROUPs on r.LocGrpId equals lg.cms_location_group_id
                            where lg.cms_pool_id == int.Parse(cmsPoolId)
                            select r;
            }

            if (!string.IsNullOrEmpty(carClassId))
            {
                resultSet = from r in resultSet
                            join cg in mdc.CAR_GROUPs on r.CarGrpId equals cg.car_group_id
                            where r.CarGrpId == int.Parse(carClassId)
                            select r;
            }

            if (!string.IsNullOrEmpty(carClassGroupId))
            {
                resultSet = from r in resultSet
                            join cg in mdc.CAR_GROUPs on r.CarGrpId equals cg.car_group_id
                            where cg.car_class_id == int.Parse(carClassGroupId)
                            select r;
            }

            if (!string.IsNullOrEmpty(carSegmentId))
            {
                resultSet = from r in resultSet
                            join ccg in mdc.CAR_GROUPs on r.CarGrpId equals ccg.car_group_id
                            where ccg.CAR_CLASS.car_segment_id == int.Parse(carSegmentId)
                            select r;
            }

            if (fromDate != null)
            {
                resultSet = resultSet.Where(d => d.TargetDate >= fromDate && d.TargetDate <= toDate && d.FleetPlanId == fleetPlanId);
            }

            return resultSet;
        }


        internal static IQueryable<CmsForecastView> RestrictForecastByParametersForCmsView(IDictionary<string, string> parameters
            , IQueryable<CmsForecastView> resultSet, MarsDBDataContext mdc)
        {
            var fromDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.FromDate) ? parameters[ParameterNames.FromDate] : null);
            var toDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.ToDate) ? parameters[ParameterNames.ToDate] : null);

            var country = parameters.ContainsKey(ParameterNames.Country) ? parameters[ParameterNames.Country] : null;
            var carSegmentId = parameters.ContainsKey(ParameterNames.CarSegment) ? parameters[ParameterNames.CarSegment] : null;
            var carClassGroupId = parameters.ContainsKey(ParameterNames.CarClassGroup) ? parameters[ParameterNames.CarClassGroup] : null;
            var carClassId = parameters.ContainsKey(ParameterNames.CarClass) ? parameters[ParameterNames.CarClass] : null;
            var cmsPoolId = parameters.ContainsKey(ParameterNames.Pool) ? parameters[ParameterNames.Pool] : null;
            var locationGroupId = parameters.ContainsKey(ParameterNames.LocationGroup) ? parameters[ParameterNames.LocationGroup] : null;

            if (!String.IsNullOrEmpty(country))
            {
                resultSet = resultSet.Where(p => p.COUNTRY == country);
            }

            if (!string.IsNullOrEmpty(locationGroupId))
            {
                resultSet = resultSet.Where(r => r.CMS_LOCATION_GROUP_ID == int.Parse(locationGroupId));
            }

            if (!string.IsNullOrEmpty(cmsPoolId))
            {
                resultSet = resultSet.Where(r => r.cms_pool_id == int.Parse(cmsPoolId));
            }

            if (!string.IsNullOrEmpty(carClassId))
            {
                resultSet = resultSet.Where(r => r.car_class_id == int.Parse(carClassId));
            }

            if (!string.IsNullOrEmpty(carClassGroupId))
            {
                resultSet = resultSet.Where(r => r.car_class_id == int.Parse(carClassGroupId));
            }

            if (!string.IsNullOrEmpty(carSegmentId))
            {
                resultSet = resultSet.Where(r => r.car_segment_id == int.Parse(carSegmentId));
            }

            if (fromDate != null)
            {
                resultSet = resultSet.Where(d => d.REP_DATE >= fromDate && d.REP_DATE <= toDate);
            }

            return resultSet;
        }

        internal static IQueryable<MARS_CMS_FORECAST_HISTORY> RestrictHistoricalForecastByParameters(IDictionary<string, string> parameters, IQueryable<MARS_CMS_FORECAST_HISTORY> resultSet, MarsDBDataContext mdc)
        {
            var fromDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.FromDate) ? parameters[ParameterNames.FromDate] : null);
            var toDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.ToDate) ? parameters[ParameterNames.ToDate] : null);

            var country = parameters.ContainsKey(ParameterNames.Country) ? parameters[ParameterNames.Country] : null;
            var carSegmentId = parameters.ContainsKey(ParameterNames.CarSegment) ? parameters[ParameterNames.CarSegment] : null;
            var carClassGroupId = parameters.ContainsKey(ParameterNames.CarClassGroup) ? parameters[ParameterNames.CarClassGroup] : null;
            var carClassId = parameters.ContainsKey(ParameterNames.CarClass) ? parameters[ParameterNames.CarClass] : null;
            var cmsPoolId = parameters.ContainsKey(ParameterNames.Pool) ? parameters[ParameterNames.Pool] : null;
            var locationGroupId = parameters.ContainsKey(ParameterNames.LocationGroup) ? parameters[ParameterNames.LocationGroup] : null;

            if(string.IsNullOrEmpty(locationGroupId) && string.IsNullOrEmpty(carSegmentId))
            {
                if(!string.IsNullOrEmpty(country))
                {
                    resultSet = from r in resultSet
                                join lg in mdc.CMS_LOCATION_GROUPs on r.CMS_LOCATION_GROUP_ID equals lg.cms_location_group_id
                                where lg.CMS_POOL.COUNTRy1.country1 == country
                                select r;
                }
            }

            if (!string.IsNullOrEmpty(locationGroupId))
            {
                resultSet = from r in resultSet
                            where r.CMS_LOCATION_GROUP_ID == int.Parse(locationGroupId)
                            select r;
            } else if (!string.IsNullOrEmpty(cmsPoolId))
            {
                resultSet = from r in resultSet
                            join lg in mdc.CMS_LOCATION_GROUPs on r.CMS_LOCATION_GROUP_ID equals lg.cms_location_group_id
                            where lg.cms_pool_id == int.Parse(cmsPoolId)
                            select r;
            }

            if (!string.IsNullOrEmpty(carClassId))
            {
                resultSet = from r in resultSet
                            where r.CAR_CLASS_ID == int.Parse(carClassId)
                            select r;
            }
            else if (!string.IsNullOrEmpty(carClassGroupId))
            {
                resultSet = from r in resultSet
                            join cg in mdc.CAR_GROUPs on r.CAR_CLASS_ID equals cg.car_group_id  //Aweful database naming! The class switches to group
                            where cg.car_class_id == int.Parse(carClassGroupId)
                            select r;
            } else if (!string.IsNullOrEmpty(carSegmentId))
            {
                resultSet = from r in resultSet
                            join ccg in mdc.CAR_GROUPs on r.CAR_CLASS_ID equals ccg.car_group_id
                            where ccg.CAR_CLASS.car_segment_id == int.Parse(carSegmentId)
                            select r;
            }

            resultSet = resultSet.Where(d => d.REP_DATE >= fromDate && d.REP_DATE <= toDate);

            return resultSet;
        }
    }
}