using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Administration.Sublease.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal.Administration.Sublease
{
    public class SubleaseDataAccess : BaseDataAccess
    {
        public SubleaseDataAccess(Dictionary<DictionaryParameter, string> parameters)
            : base(parameters, null)
        {
            
        }

        public List<ListItem> GetSubleasedModels()
        {

            var models = from slv in DataContext.VehicleSubleases
                select slv.Vehicle.ModelDescription;

            var distinctModels = models.Distinct().Select(d=> new ListItem(d)).ToList();
            return distinctModels;
        }

        public void TruncateSubleasesTable()
        {
            DataContext.TruncateVehicleSublease();
        }

        public string AddOrRemoveSubleasedVehicles(List<int> vehicleIds, bool addingSubleases
                                , string rentingCountry, DateTime startDate)
        {
            try
            {
                if (addingSubleases)
                {
                    var rentingCountryId = DataContext.COUNTRies.Single(d => d.country1 == rentingCountry).CountryId;

                    var subleaseEntities = from v in DataContext.Vehicles
                        join cntry in DataContext.COUNTRies on v.OwningCountry equals cntry.country1
                        where vehicleIds.Contains(v.VehicleId)
                                select new 
                               {
                                   VehicleId = v.VehicleId,
                                   StartDate = startDate,
                                   RentingCountryId = rentingCountryId,
                                   OwningCountryId = cntry.CountryId
                               };

                    var entitiesToInsert = from se in subleaseEntities.AsEnumerable()
                                           select new VehicleSublease
                                                  {
                                                      VehicleId = se.VehicleId,
                                                      StartDate = se.StartDate,
                                                      RentingCountryId = se.RentingCountryId,
                                                      OwningCountryId = se.OwningCountryId
                                                  };
                    DataContext.VehicleSubleases.InsertAllOnSubmit(entitiesToInsert);
                    
                }
                else
                {
                    var entitiesToDelete = DataContext.VehicleSubleases.Where(d => vehicleIds.Contains(d.VehicleId));
                    DataContext.VehicleSubleases.DeleteAllOnSubmit(entitiesToDelete);
                }
                DataContext.SubmitChanges();
                
            }
            catch (Exception e)
            
            {
                return e.Message;
            }
            return string.Empty;
        }

        public List<int> GetVehiclesFromVins(List<string> vins, bool addSubleases)
        {
            IQueryable<int> subleases;
            if (addSubleases)
            {
                subleases = from v in DataContext.Vehicles
                    where v.IsFleet
                          && vins.Contains(v.Vin)
                    select v.VehicleId;
            }
            else
            {
                subleases = from v in DataContext.VehicleSubleases
                            where vins.Contains(v.Vehicle.Vin)
                            select v.VehicleId;
            }

            var returned = subleases.ToList();
            return returned;
        }

        public List<SubleaseDataRow> GetSubleasedVehicles(string rentingCountry, List<string> models)
        {
            var vehicleData = from slv in DataContext.VehicleSubleases
                select slv;

            if (rentingCountry != string.Empty)
            {
                vehicleData = vehicleData.Where(d => d.COUNTRy1.country1 == rentingCountry);
            }

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountry = Parameters[DictionaryParameter.OwningCountry];
                vehicleData = vehicleData.Where(d => d.COUNTRy.country1 == owningCountry);
            }

            if (models.Any())
            {
                vehicleData = vehicleData.Where(d => models.Contains(d.Vehicle.ModelDescription));
            }
             
            var leasedVehicles = from slv in vehicleData
                select new SubleaseDataRow
                       {
                           Model = slv.Vehicle.ModelDescription,
                           OwningCountry = slv.COUNTRy.country_description,
                           RentingCountry = slv.COUNTRy1.country_description,
                           Vin = slv.Vehicle.Vin,
                           StartDate = slv.StartDate,
                           UnitNumber = slv.Vehicle.UnitNumber.HasValue ? slv.Vehicle.UnitNumber.Value : 0,
                       };

            var returned = leasedVehicles.ToList();
            return returned;
        }
    }
}