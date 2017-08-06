using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Abstract;
using Mars.Entities.Pooling;

namespace Mars.DAL.Pooling {
    public class AlertsPopupRepository:HtmlTableRepository<AlertsPopupEntity> {
        enum _columns { divNextHour, divFollow4Hours, divRestOfDay };
        public override IList<AlertsPopupEntity> GetTable(params String[] s) {
            Int32 hr=DateTime.Now.Hour+1;
            Int32 frhr=hr+4;
            DateTime endOfDay=DateTime.Now.Date.AddDays(1);
            using(PoolingDataClassesDataContext db = new PoolingDataClassesDataContext()) {
                IQueryable<AlertsPopupEntity> q = from p in db.Reservations
                                                  where p.CAR_GROUP.car_group1==s[1] 
                                                  && p.LOCATION1.location1==s[0] 
                                                  && (p.RS_ARRIVAL_DATE==DateTime.Now.Date)
                                                  select new AlertsPopupEntity { ResNbr=p.RES_ID_NBR,RtrnLoc=p.LOCATION.location1,RtrnDate=p.RS_ARRIVAL_TIME };
                if(s[2]==_columns.divNextHour.ToString()) return (from p in q where p.RtrnDate.Value.Hour<hr select p).ToList();
                if(s[2]==_columns.divFollow4Hours.ToString()) return (from p in q where p.RtrnDate.Value.Hour>=hr && p.RtrnDate.Value.Hour<frhr select p).ToList();
                return (from p in q where p.RtrnDate.Value.Hour>=frhr select p).ToList();
            }
        }
    }
}