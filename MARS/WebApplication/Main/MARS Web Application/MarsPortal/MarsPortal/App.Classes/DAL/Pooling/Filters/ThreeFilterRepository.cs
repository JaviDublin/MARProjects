using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Pooling.Abstract;
using App.DAL.VehiclesAbroad.Abstract;

namespace Mars.DAL.Pooling.Filters {
    public class ThreeFilterRepository:IThreeFilterRepository {
        public IFilterRepository3 TopRepository { get; set; }
        public IFilterRepository3 MiddleRepository { get; set; }
        public IFilterRepository3 BottomRepository { get; set; }
        public ThreeFilterRepository(IFilterRepository3 t,IFilterRepository3 m,IFilterRepository3 b) {
            TopRepository=t;
            MiddleRepository=m;
            BottomRepository=b;
        }
    }
}