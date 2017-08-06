using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.DAL.FleetDemand
{
    public class DataAccess : IDisposable
    {
        private readonly FleetDemandDataContext _fleetDemandDataContext;

        public DataAccess()
        {
            _fleetDemandDataContext = new FleetDemandDataContext();
        }

        public void Dispose()
        {
            _fleetDemandDataContext.Dispose();
        }

        public List<CountriesSelectResult> GetCountries()
        {
            var ret = _fleetDemandDataContext.CountriesSelect().ToList();
            return ret;
        }

        public List<DimDaysSelectResult> GetDimDays()
        {
            var ret = _fleetDemandDataContext.DimDaysSelect().ToList();
            return ret;
        }

        public List<PoolsForCountrySelectResult> GetPoolsForCountry(int countryKey)
        {
            var ret = _fleetDemandDataContext.PoolsForCountrySelect(countryKey).ToList();
            return ret;
        }

        public List<LocationGroupsForPoolSelectResult> GetLocationGroupForPool(int poolKey)
        {
            var ret = _fleetDemandDataContext.LocationGroupsForPoolSelect(poolKey).ToList();
            return ret;
        }

        public List<LocationsForLocationGroupSelectResult> GetLocationsForLocationGroup(int locationGroupKey)
        {
            var ret = _fleetDemandDataContext.LocationsForLocationGroupSelect(locationGroupKey).ToList();
            return ret;
        }

        public List<OpFleetRatioLogSelectResult> GetOpFleetRatioLog(int? countryKey, int? cmsPoolKey, int? locationGroupKey, int? locationKey, int?dayKey)
        {
            var ret = _fleetDemandDataContext.OpFleetRatioLogSelect(countryKey, cmsPoolKey, locationGroupKey, locationKey, dayKey).ToList();
            return ret;
        }

        public List<CarClassForCountrySelectResult> GetCarClassForCountry(int countryKey)
        {
            var ret = _fleetDemandDataContext.CarClassForCountrySelect(countryKey).ToList();
            return ret;
        }

        public List<PriceablePercentLogSelectResult> GetPriceablePercentLog(int? countryKey, int? cmsPoolKey, int? locationGroupKey, int? locationKey, int? carClassKey, int? reportDateKey)
        {
            var ret = _fleetDemandDataContext.PriceablePercentLogSelect(countryKey, cmsPoolKey, locationGroupKey, locationKey, carClassKey, reportDateKey).ToList();
            return ret;
        }

        public List<AvgRpdLogSelectResult> GetAvgRpdLog(int? countryKey, int? cmsPoolKey, int? locationGroupKey, int? locationKey, int? carClassKey, int? reportDateKey)
        {
            var ret = _fleetDemandDataContext.AvgRpdLogSelect(countryKey, cmsPoolKey, locationGroupKey, locationKey, carClassKey, reportDateKey).ToList();
            return ret;
        }

        public List<OneWayLogSelectResult> GetOneWayLog(int? countryKey, int? cmsPoolKey, int? locationGroupKey, int? locationKey, int? dayKey)
        {
            var ret = _fleetDemandDataContext.OneWayLogSelect(countryKey, cmsPoolKey, locationGroupKey, locationKey, dayKey).ToList();
            return ret;
        }

    }



}