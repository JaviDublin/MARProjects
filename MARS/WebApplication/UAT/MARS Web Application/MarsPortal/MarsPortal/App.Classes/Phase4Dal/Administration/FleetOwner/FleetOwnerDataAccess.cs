using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Administration.UserEntities;

namespace Mars.App.Classes.Phase4Dal.Administration.FleetOwner
{
    public class FleetOwnerDataAccess : IDisposable
    {
        protected MarsDBDataContext DataContext;

        public FleetOwnerDataAccess()
        {
            DataContext = new MarsDBDataContext();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        public void UpdateFleetOwner(FleetOwnerEntity foe)
        {
            var dbFleetOwner = DataContext.FleetOwners.Single(d => d.FleetOwnerId == foe.FleetOwnerId);
            if (foe.CompanyId == -1)
            {
                dbFleetOwner.Company = null;
            }
            else
            {
                dbFleetOwner.CompanyId = foe.CompanyId;
            }
            dbFleetOwner.OwnerName = foe.FleetOwnerName;    
            
            DataContext.SubmitChanges();
        }

        public List<FleetOwnerEntity> GetFleetOwnerEntities()
        {
            var fleetData = from fo in DataContext.FleetOwners
                select new FleetOwnerEntity
                       {
                           FleetOwnerId = fo.FleetOwnerId,
                           FleetOwnerCode = fo.OwningAreaCode,
                           FleetOwnerName = fo.OwnerName,
                           CountryId = fo.CountryId,
                           CountryName = fo.COUNTRy.country_description,
                           CompanyId = fo.CompanyId,

                           CompanyName = fo.CompanyId == null ? string.Empty : fo.Company.CompanyName
                       };
            var returned = fleetData.ToList();
            return returned;
        }
        

    }
}