using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;
using Mars.FleetAllocation.DataAccess.Entities.Output;

namespace Mars.FleetAllocation.DataAccess.VehicleDistribution
{
    public class VehicleDistributionDataAccess : BaseDataAccess
    {
        

        public void FillNames(List<WeeklyAddition> calculatedAdditions)
        {
            foreach (var ca in calculatedAdditions)
            {
                int carGroupId = ca.CarGroupId;
                int locationId = ca.LocationId;
                ca.CarGroup = DataContext.CAR_GROUPs.First(d => d.car_group_id == carGroupId).car_group1;
                ca.Location = DataContext.LOCATIONs.First(d => d.dim_Location_id == locationId).location1;
            }
        }


    }
}