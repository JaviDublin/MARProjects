using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Entities.Pooling;

namespace Mars.DAL.Pooling.Queryables
{
    public class AlertsReturnQueryable
    { //not really a queryable
        const String DELIMITER = "|";
        
        public IList<AlertEntity> GetQueryable(IList<AlertTempEntity> l)
        {
            var returned = (from p in l
                    group p by new { p.rsTime, p.crGrp, p.rentLoc } into pg
                    select new AlertEntity
                    {
                        NextHour = pg.Key.rsTime == 1 ? pg.Key.rentLoc + " " 
                                + pg.Key.crGrp + DELIMITER + pg.Sum(o => o.rsTime == 1 ? o.Amnt : 0) : string.Empty,
                        Follow4Hours = pg.Key.rsTime == 2 ? pg.Key.rentLoc + " " 
                                + pg.Key.crGrp + DELIMITER + pg.Sum(o => o.rsTime == 2 ? o.Amnt : 0) : string.Empty,
                        RestOfDay = pg.Key.rsTime == 3 ? pg.Key.rentLoc + " " 
                                + pg.Key.crGrp + DELIMITER + pg.Sum(o => o.rsTime == 3 ? o.Amnt : 0) : string.Empty,
                        Custom = pg.Key.rsTime == 4 ? pg.Key.rentLoc + " " 
                                + pg.Key.crGrp + DELIMITER + pg.Sum(o => o.rsTime == 4 ? o.Amnt : 0) : string.Empty,
                    }).ToList();
            return returned;
        }
    }
}