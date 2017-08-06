using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.Pooling.Entities;
using Mars.DAL.Pooling.Abstract;


namespace Mars.DAL.Pooling.Filters {
    public class CarGrpRepository:IFilterRepository3 {
        public IList<DropdownEntity> getList(params string[] dependants) {

            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {
                List<string> l = new List<string>();
                string country = dependants[0] == "***All***" ? string.Empty : dependants[0] ?? "";
                string carSegment = dependants[1] == "***All***" ? string.Empty : dependants[1] ?? "";
                string carClass = dependants[2] == "***All***" ? string.Empty : dependants[2] ?? "";
                return (from p in db.CAR_GROUPs
                        where p.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description==country
                        && p.CAR_CLASS.CAR_SEGMENT.car_segment1==carSegment
                        && p.CAR_CLASS.car_class1==carClass
                        select new DropdownEntity { Name=p.car_group1,Id=p.car_group_id }).ToList();
            }
        }
    }
}