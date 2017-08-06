using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Abstract;

using Mars.Pooling.Entities;

namespace Mars.DAL.Pooling.Filters {
    public class PoolRepository:IFilterRepository3 {
        public IList<DropdownEntity> getList(params string[] dependants) {
            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {
                string country = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                return (from p in db.CMS_POOLs
                       where p.COUNTRy1.country_description==country
                       orderby p.cms_pool1
                       select new DropdownEntity { Name=p.cms_pool1,Id=p.cms_pool_id }).ToList();
            }

        }
        
    }
}