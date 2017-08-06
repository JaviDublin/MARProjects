using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Abstract;
using Mars.Pooling.Entities;

namespace Mars.DAL.Pooling.Filters {
    public class AreasRepository : IFilterRepository3 {
        public IList<DropdownEntity> getList(params string[] dependants) {
            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {

                string opsRegion = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                string country = dependants[1] == "***All***" ? string.Empty : dependants[1] ?? "";
                return (from p in db.OPS_AREAs
                        where p.OPS_REGION.COUNTRy1.country_description==country
                        && p.OPS_REGION.ops_region1==opsRegion
                        select new DropdownEntity { Name=p.ops_area1,Id=p.ops_area_id }).ToList();
            }
        }
    }
}