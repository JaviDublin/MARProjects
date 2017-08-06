using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Abstract;
using Mars.Pooling.Entities;

namespace Mars.DAL.Pooling.Filters {
    public class CarSegRepository:IFilterRepository3 {
        public IList<DropdownEntity> getList(params string[] dependants) {

            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {

                string country = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";

                return (from p in db.CAR_SEGMENTs
                        where p.COUNTRy1.country_description==country
                           && p.COUNTRy1.active
                        select new DropdownEntity { Name=p.car_segment1,Id=p.car_segment_id }).ToList();
            }
        }
    }
}