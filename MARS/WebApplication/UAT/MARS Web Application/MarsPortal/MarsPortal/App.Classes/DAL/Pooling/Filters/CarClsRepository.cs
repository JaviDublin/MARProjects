using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.Pooling.Entities;
using Mars.DAL.Pooling.Abstract;

namespace Mars.DAL.Pooling.Filters {
    public class CarClsRepository:IFilterRepository3 {
        public IList<DropdownEntity> getList(params string[] dependants) {
            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {

                List<string> l = new List<string>();
                string country = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                string carSegment = dependants[1] == "***All***" ? string.Empty : dependants[1] ?? "";
                return (from p in db.CAR_CLASSes
                        where p.CAR_SEGMENT.car_segment1==carSegment
                       && p.CAR_SEGMENT.COUNTRy1.country_description==country
                        select new DropdownEntity { Name=p.car_class1,Id=p.car_class_id }).ToList();
            }
        }
    }
}