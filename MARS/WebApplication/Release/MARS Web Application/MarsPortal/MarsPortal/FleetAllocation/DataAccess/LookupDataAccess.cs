using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess
{
    public class LookupDataAccess : BaseDataAccess
    {
        public List<int> FindCarGroupsInSameCarClass(int carGroupId)
        {
            var carClassId = DataContext.CAR_GROUPs.Single(d => d.car_group_id == carGroupId).car_class_id;


            var carGroupIds = from cg in DataContext.CAR_GROUPs
                where cg.car_group_id != carGroupId
                      && cg.car_class_id == carClassId
                select cg.car_group_id;
            var returned = carGroupIds.ToList();
            return returned;
        }

        public List<int> FindCarGroupsInCarClass(int carClassId)
        {
            var carGroupIds = from cg in DataContext.CAR_GROUPs
                              where cg.car_class_id == carClassId
                              select cg.car_group_id;
            var returned = carGroupIds.ToList();
            return returned;
        }
    }
}