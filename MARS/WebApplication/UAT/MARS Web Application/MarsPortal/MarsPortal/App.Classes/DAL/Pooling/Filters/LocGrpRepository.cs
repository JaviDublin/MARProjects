using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Abstract;
using Mars.Pooling.Entities;

namespace Mars.DAL.Pooling.Filters {
    public class LocGrpRepository:IFilterRepository3 {
        public IList<DropdownEntity> getList(params string[] dependants) {
            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {
                string poolId = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                string countryName = dependants[1];
                return (from lg in db.CMS_LOCATION_GROUPs
                        where lg.CMS_POOL.cms_pool1==poolId
                            && lg.CMS_POOL.COUNTRy1.country_description == countryName
                        select new DropdownEntity { Name=lg.cms_location_group1,Id=lg.cms_location_group_id }).ToList();

            }
        }
    }
}