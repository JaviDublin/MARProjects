using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Abstract;
using Mars.Pooling.Entities;

namespace Mars.DAL.Pooling.Filters {
    public class BranchOPSRepository : IFilterRepository3 {
        public IList<DropdownEntity> getList(params string[] dependants) {
            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {

                
                string opsArea = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                string opsRegionDescription = dependants[1];
                string countryDescription = dependants[2];

                var returned = (from p in db.LOCATIONs
                        where p.OPS_AREA.ops_area1 == opsArea
                            && p.OPS_AREA.OPS_REGION.ops_region1 == opsRegionDescription
                            && p.COUNTRy1.country_description == countryDescription
                        select new DropdownEntity { Name=p.location1,Id=p.ops_area_id }).ToList();

                return returned;
            }
              
        }
    }
}