using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Abstract;
using Mars.Pooling.Entities;

namespace Mars.DAL.Pooling.Filters {
    public class BranchRepository:IFilterRepository3 {
        public IList<DropdownEntity> getList(params string[] dependants) {
            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {

                string LocationGroup = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                
                string pool = dependants[1];
                string country = dependants[2];

                var returned =  (from p in db.LOCATIONs
                        where p.CMS_LOCATION_GROUP.cms_location_group1==LocationGroup
                            && p.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == pool
                            && p.COUNTRy1.country_description == country
                        select new DropdownEntity { Name=p.location1,Id=p.cms_location_group_id??-1 }).ToList();
                return returned;
            }
                

        }

    }
}