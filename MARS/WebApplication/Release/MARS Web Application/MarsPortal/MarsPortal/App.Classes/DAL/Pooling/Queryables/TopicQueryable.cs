using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Entities.Pooling;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.DAL.Pooling.Queryables {
    public class TopicQueryable {
        public IQueryable<DayActualEntity> GetQueryable(MarsDBDataContext db,IQueryable<FLEET_EUROPE_ACTUAL> q) {
            return from p in q
                   join tc1 in db.COUNTRies on p.COUNTRY equals tc1.country1
                   where (tc1.active)
                   group p by tc1.country_description into g
                   select new DayActualEntity { Tme=0,Label=g.Key,Available=g.Count(p => p.RT==1) };
        }
    }
}