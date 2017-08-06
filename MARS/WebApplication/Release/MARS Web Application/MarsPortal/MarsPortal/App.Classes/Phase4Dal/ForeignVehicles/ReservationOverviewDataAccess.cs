using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;
using Mars.App.Classes.Phase4Dal.Reservations;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles
{
    public class ReservationOverviewDataAccess : ReservationDataAccess
    {
        public ReservationOverviewDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {

        }

        public Dictionary<string, string> GetDistinctPickupLocationIds()
        {
            var reservations = RestrictReservation();

            reservations = from r in reservations
                           where r.PickupLocation.location1.Substring(0, 2) != r.ReturnLocation.location1.Substring(0, 2)
                           select r;

            IQueryable<LocationIdHolder> dataHolder = null;
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutLocationGroup)
                || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutArea))
            {
                dataHolder = (from r in reservations
                              select new LocationIdHolder
                              {
                                  LocationId = r.PickupLocation.dim_Location_id.ToString(),
                                  LocationName = r.PickupLocation.location1
                              });
            }
            else
            {
                if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CmsSelected))
                {
                    if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutPool))
                    {
                        var multiSelectionMade = Parameters[DictionaryParameter.CheckOutPool].Contains(VehicleFieldRestrictions.Separator);

                        dataHolder = (from r in reservations
                                      select new LocationIdHolder
                                      {
                                          LocationId = r.PickupLocation.cms_location_group_id.ToString(),
                                          LocationName = (multiSelectionMade ? (r.PickupLocation.country + VehicleOverviewDataAccess.JoinLocationParts
                                                    + r.PickupLocation.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 + VehicleOverviewDataAccess.JoinLocationParts)
                                                : string.Empty)
                                                + r.PickupLocation.CMS_LOCATION_GROUP.cms_location_group1
                                      });


                    }
                    else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutCountry))
                    {
                        var multiSelectionMade = Parameters[DictionaryParameter.CheckOutCountry].Contains(VehicleFieldRestrictions.Separator);

                        dataHolder = (from r in reservations
                                      select new LocationIdHolder
                                      {
                                          LocationId = r.PickupLocation.CMS_LOCATION_GROUP.cms_pool_id.ToString(),
                                          LocationName = (multiSelectionMade ? (r.PickupLocation.country + VehicleOverviewDataAccess.JoinLocationParts)
                                                    : string.Empty)
                                                    + r.PickupLocation.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                      });
                    }
                }
                else
                {
                    if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutRegion))
                    {
                        var multiSelectionMade = Parameters[DictionaryParameter.CheckOutRegion].Contains(VehicleFieldRestrictions.Separator);

                        dataHolder = (from r in reservations
                                      select new LocationIdHolder
                                      {
                                          LocationId = r.PickupLocation.ops_area_id.ToString(),
                                          LocationName = (multiSelectionMade ? (r.PickupLocation.country + VehicleOverviewDataAccess.JoinLocationParts
                                                    + r.PickupLocation.OPS_AREA.OPS_REGION.ops_region1 + VehicleOverviewDataAccess.JoinLocationParts)
                                                    : string.Empty)
                                                    + r.PickupLocation.OPS_AREA.ops_area1
                                      });
                    }
                    else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutCountry))
                    {
                        var multiSelectionMade = Parameters[DictionaryParameter.CheckOutCountry].Contains(VehicleFieldRestrictions.Separator);

                        dataHolder = (from r in reservations
                                      select new LocationIdHolder
                                      {
                                          LocationId = r.PickupLocation.OPS_AREA.ops_region_id.ToString(),
                                          LocationName = (multiSelectionMade ? (r.PickupLocation.country + VehicleOverviewDataAccess.JoinLocationParts)
                                                    : string.Empty)
                                                    + r.PickupLocation.OPS_AREA.OPS_REGION.ops_region1
                                      });
                    }
                }
            }

            if (dataHolder == null)
            {
                dataHolder = (from r in reservations
                              select new LocationIdHolder
                              {
                                  LocationId = r.PickupLocation.country,
                                  LocationName = r.PickupLocation.COUNTRy1.country_description
                              });

            }
            var returned = dataHolder.Distinct().ToDictionary(d => d.LocationId, d => d.LocationName);

            return returned;
        }

        private DictionaryParameter GetGroupingLevel()
        {
            var returned = DictionaryParameter.LocationCountry;

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutLocationGroup)
                || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutArea))
            {
                returned = DictionaryParameter.Location;
            }
            else
            {
                if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CmsSelected))
                {
                    if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutPool))
                    {
                        returned = DictionaryParameter.LocationGroup;
                    }
                    else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutCountry))
                    {
                        returned = DictionaryParameter.Pool;
                    }
                }
                else
                {
                    if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutRegion))
                    {
                        returned = DictionaryParameter.Area;
                    }
                    else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutCountry))
                    {
                        returned = DictionaryParameter.Region;
                    }
                }
            }

            return returned;
        }



        public List<OverviewGridItemHolder> GetReservationOverviewGrid()
        {
            var reservations = RestrictReservation();

            reservations = from r in reservations
                           where r.PickupLocation.location1.Substring(0, 2) != r.ReturnLocation.location1.Substring(0, 2)
                           select r;

            var returnCountries = reservations.Select(d => d.ReturnLocation.location1.Substring(0, 2)).Distinct().ToList();
            


            var returned = new List<OverviewGridItemHolder>();
            var dict = GetCountryDescriptionDictionary();
            var groupingLevel = GetGroupingLevel();

            foreach (var ac in returnCountries)
            {
                var returnCountry = ac;

                IQueryable<LocationCount> filteredOwner = null;
                switch (groupingLevel)
                {
                    case DictionaryParameter.LocationCountry:
                        filteredOwner = from r in reservations
                                        where r.ReturnLocation.location1.Substring(0, 2) == returnCountry
                                        group r by r.PickupLocation.location1.Substring(0, 2)
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key, Count = g.Count() };
                        break;
                    case DictionaryParameter.Pool:
                        filteredOwner = from r in reservations
                                        where r.ReturnLocation.location1.Substring(0, 2) == returnCountry
                                        group r by r.PickupLocation.CMS_LOCATION_GROUP.cms_pool_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                    case DictionaryParameter.Region:
                        filteredOwner = from r in reservations
                                        where r.ReturnLocation.location1.Substring(0, 2) == returnCountry
                                        group r by r.PickupLocation.OPS_AREA.ops_region_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                    case DictionaryParameter.LocationGroup:
                        filteredOwner = from r in reservations
                                        where r.ReturnLocation.location1.Substring(0, 2) == returnCountry
                                        group r by r.PickupLocation.cms_location_group_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                    case DictionaryParameter.Area:
                        filteredOwner = from r in reservations
                                        where r.ReturnLocation.location1.Substring(0, 2) == returnCountry
                                        group r by r.PickupLocation.ops_area_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                    case DictionaryParameter.Location:
                        filteredOwner = from r in reservations
                                        where r.ReturnLocation.location1.Substring(0, 2) == returnCountry
                                        group r by r.PickupLocation.dim_Location_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                }



                var gvItem = new OverviewGridItemHolder
                {
                    CountryId = ac,
                    CountryName = dict[ac],
                    ForeignVehiclesHolder = new List<LocationIdHolder>()
                };


                if (filteredOwner != null)
                {
                    var filteredLocalList = filteredOwner.ToList();
                    foreach (var fo in filteredLocalList)
                    {
                        gvItem.ForeignVehiclesHolder.Add(new LocationIdHolder
                        {
                            LocationId = fo.LocationGroupingId,
                            OwningCountry = returnCountry,
                            VehicleCount = fo.Count
                        });
                    }
                }

                returned.Add(gvItem);
            }

            foreach (var gv in returned)
            {
                gv.ForeignVehiclesHolder.Add(new LocationIdHolder
                {
                    LocationId = VehicleOverviewDataAccess.TotalString,
                    LocationName = VehicleOverviewDataAccess.TotalString,
                    VehicleCount = gv.ForeignVehiclesHolder.Sum(d => d.VehicleCount),
                    OwningCountry = null
                });
            }

            return returned;
        }


    }
}