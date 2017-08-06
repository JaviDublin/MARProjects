using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Abstract;
using Mars.Pooling.Entities;

namespace Mars.DAL.Pooling.Filters {
    public class CountryRepository:IFilterRepository3 {

        public IList<DropdownEntity> getList(params string[] dependants) {

            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {

                return (from p in db.COUNTRies
                        where p.active
                        orderby p.country_description
                        select new DropdownEntity { Name=p.country_description,Code=p.country1 }).ToList();
            }
        }
    }
}