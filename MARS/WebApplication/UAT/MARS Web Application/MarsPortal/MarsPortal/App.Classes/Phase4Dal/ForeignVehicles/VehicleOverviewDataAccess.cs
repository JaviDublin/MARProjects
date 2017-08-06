using System;
using System.Collections.Generic;
using System.Linq;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;

namespace Mars.App.Classes.Phase4Dal.ForeignVehicles
{
    public class VehicleOverviewDataAccess : BaseDataAccess
    {
        public const string JoinLocationParts = "-";
        public const string TotalString = "Total";

        public VehicleOverviewDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc = null)
            : base(parameters, dbc)
        {
            
        }

        public Dictionary<string, string> GetDistinctOwningLocationIds()
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);

            vehicles = from v in vehicles
                       //where v.LastLocationCode.Substring(0, 2) != v.OwningCountry
                       select v;

            IQueryable<LocationIdHolder> dataHolder = null;
            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)
                || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
            {
                dataHolder = (from v in vehicles
                                join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                select new LocationIdHolder { LocationId = l.dim_Location_id.ToString(), LocationName = l.location1 });
            }
            else
            {
                if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CmsSelected))
                {
                    if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
                    {
                        var multiSelectionMade = Parameters[DictionaryParameter.Pool].Contains(VehicleFieldRestrictions.Separator);
                        
                        dataHolder = (from v in vehicles
                                    join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                      select new LocationIdHolder 
                                      { 
                                          LocationId = l.cms_location_group_id.ToString(),
                                          LocationName = (multiSelectionMade ? (l.country + JoinLocationParts 
                                                    + l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 + JoinLocationParts)
                                                : string.Empty) 
                                                + l.CMS_LOCATION_GROUP.cms_location_group1 
                                      });

                        
                    } else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
                    {
                        var multiSelectionMade = Parameters[DictionaryParameter.LocationCountry].Contains(VehicleFieldRestrictions.Separator);

                        dataHolder = (from v in vehicles
                                      join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                      select new LocationIdHolder
                                      {
                                          LocationId = l.CMS_LOCATION_GROUP.cms_pool_id.ToString(),
                                          LocationName = (multiSelectionMade ? (l.country + JoinLocationParts) 
                                                    : string.Empty) 
                                                    + l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1
                                      });
                    }
                }
                else
                {
                    if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
                    {
                        var multiSelectionMade = Parameters[DictionaryParameter.Region].Contains(VehicleFieldRestrictions.Separator);

                        dataHolder = (from v in vehicles
                                      join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                      select new LocationIdHolder
                                      {
                                          LocationId = l.ops_area_id.ToString(),
                                          LocationName = (multiSelectionMade ? (l.country + JoinLocationParts
                                                    + l.OPS_AREA.OPS_REGION.ops_region1 + JoinLocationParts) 
                                                    : string.Empty) 
                                                    + l.OPS_AREA.ops_area1
                                      });
                    } else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
                    {
                        var multiSelectionMade = Parameters[DictionaryParameter.LocationCountry].Contains(VehicleFieldRestrictions.Separator);

                        dataHolder = (from v in vehicles
                                      join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                      select new LocationIdHolder
                                      {
                                          LocationId = l.OPS_AREA.ops_region_id.ToString(),
                                          LocationName = (multiSelectionMade ? (l.country + JoinLocationParts)
                                                    : string.Empty)
                                                    + l.OPS_AREA.OPS_REGION.ops_region1
                                      });
                    }
                }
            }

            if (dataHolder == null)
            {
                dataHolder = (from v in vehicles
                              join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                              select new LocationIdHolder
                              {
                                  LocationId = l.country,
                                  LocationName = l.COUNTRy1.country_description
                              });
                
            }
            var returned = dataHolder.Distinct().OrderBy(d=> d.LocationName).ToDictionary(d => d.LocationId, d => d.LocationName);
            
            return returned;
        }

        public List<string> GetActiveOwningCountries()
        {
            var activeOwningCountries = DataContext.Vehicles.Where(d=> d.IsFleet).Select(d => d.OwningCountry).Distinct().ToList();
            activeOwningCountries.Sort();
            return activeOwningCountries;
        }

        public List<CountryHolder> GetActiveOwningCountryHolders()
        {
            var activeOwningCountries = DataContext.Vehicles.Where(d => d.IsFleet).Select(d => d.OwningCountry).Distinct();

            var countryHolders = from ac in activeOwningCountries
                join c in DataContext.COUNTRies on ac equals c.country1
                select new CountryHolder
                       {
                           CountryId = ac,
                           CountryDescription = c.country_description
                       };

            var returned = countryHolders.OrderBy(d=> d.CountryDescription).ToList();
            return returned;
        }

        private DictionaryParameter GetGroupingLevel()
        {
            var returned = DictionaryParameter.LocationCountry;

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)
                || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
            {
                returned = DictionaryParameter.Location;
            }
            else
            {
                if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CmsSelected))
                {
                    if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
                    {
                        returned = DictionaryParameter.LocationGroup;
                    } else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
                    {
                        returned = DictionaryParameter.Pool;                        
                    }
                }
                else
                {
                    if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
                    {
                        returned = DictionaryParameter.Area;
                    } else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
                    {
                        returned = DictionaryParameter.Region;
                    }
                }
            }
            
            return returned;
        }

        public List<OverviewGridItemHolder> GetForeignVehicleOverviewGrid()
        {
            var vehicles = BaseVehicleDataAccess.GetVehicleQueryable(Parameters, DataContext, true, true);

            vehicles = VehicleFieldRestrictions.RestrictByPredicament(vehicles, Parameters);


            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.MinDaysNonRev))
            {
                var daysSinceLastRevenue = int.Parse(Parameters[DictionaryParameter.MinDaysNonRev]);
                vehicles = vehicles.Where(d => d.DaysSinceLastRevenueMovement >= daysSinceLastRevenue);
            }

            var activeOwningCountries = GetActiveOwningCountries();

            var returned = new List<OverviewGridItemHolder>();
            var dict = GetCountryDescriptionDictionary();
            var groupingLevel = GetGroupingLevel();

            foreach (var oc in activeOwningCountries)
            {
                var owningCountry = oc;

                IQueryable<LocationCount> filteredOwner = null;
                switch (groupingLevel)
                {
                    case DictionaryParameter.LocationCountry:
                        filteredOwner = from v in vehicles
                                        where v.OwningCountry == owningCountry
                                        group v by v.LastLocationCode.Substring(0, 2)
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key, Count = g.Count() };
                        break;
                    case DictionaryParameter.Pool:
                        filteredOwner = from v in vehicles
                                        join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                        where v.OwningCountry == owningCountry
                                        group l by l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                    case DictionaryParameter.Region:
                        filteredOwner = from v in vehicles
                                        join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                        where v.OwningCountry == owningCountry
                                        group l by l.OPS_AREA.ops_region_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                    case DictionaryParameter.LocationGroup:
                        filteredOwner = from v in vehicles
                                        join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                        where v.OwningCountry == owningCountry
                                        group l by l.cms_location_group_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                    case DictionaryParameter.Area:
                        filteredOwner = from v in vehicles
                                        join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                        where v.OwningCountry == owningCountry
                                        group l by l.ops_area_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                    case DictionaryParameter.Location:
                        filteredOwner = from v in vehicles
                                        join l in DataContext.LOCATIONs on v.LastLocationCode equals l.location1
                                        where v.OwningCountry == owningCountry
                                        group l by l.dim_Location_id
                                            into g
                                            select new LocationCount { LocationGroupingId = g.Key.ToString(), Count = g.Count() };
                        break;
                }



                var gvItem = new OverviewGridItemHolder
                {
                    CountryId = oc,
                    CountryName = dict[oc],
                    ForeignVehiclesHolder = new List<LocationIdHolder>()
                };

                
                if (filteredOwner != null)
                {
                    var filteredLocalList = filteredOwner.ToList();
                    foreach (var fo in filteredLocalList)
                    {
                        gvItem.ForeignVehiclesHolder.Add( new LocationIdHolder
                                                          {
                                                              LocationId = fo.LocationGroupingId,
                                                              OwningCountry = owningCountry,
                                                              VehicleCount = fo.Count
                                                          });
                    }
                    
                }

                returned.Add(gvItem);
            }


            


            foreach (var gv in returned)
            {
                gv.ForeignVehiclesHolder.Add(new LocationIdHolder{LocationId = TotalString
                                    , LocationName = TotalString
                                    , VehicleCount = gv.ForeignVehiclesHolder.Sum(d=> d.VehicleCount)
                                    , OwningCountry = null});
            }

            return returned;
        }
    }
}