using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Pooling.Abstract;
using Mars.Entities.Pooling;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.Pooling.Models
{
    public class AlertsBreakdownModel
    {
        public IMainFilterEntity GetBreakdown(string x, bool b)
        {
            IMainFilterEntity mf = new MainFilterEntity();
            char[] c = { ' ', '|' };
            string[] s = x.Split(c);
            using (MarsDBDataContext db = new MarsDBDataContext())
            {
                var countryMatch = db.COUNTRies.Where(p => p.country1 == s[0].Substring(0, 2));
                var firstCountryMatch = countryMatch.FirstOrDefault();
                if (firstCountryMatch == null)
                {
                    throw new NullReferenceException();
                }
                mf.Country = firstCountryMatch.country_description;
                mf.Branch = s[0];
                if (b)
                {
                    mf.PoolRegion = db.LOCATIONs.Where(p => p.location1 == s[0]).FirstOrDefault().CMS_LOCATION_GROUP.CMS_POOL.cms_pool1;
                    mf.LocationGrpArea = db.LOCATIONs.Where(p => p.location1 == s[0]).FirstOrDefault().CMS_LOCATION_GROUP.cms_location_group1;
                }
                else
                {
                    mf.PoolRegion = db.LOCATIONs.Where(p => p.location1 == s[0]).FirstOrDefault().OPS_AREA.OPS_REGION.ops_region1;
                    mf.LocationGrpArea = db.LOCATIONs.Where(p => p.location1 == s[0]).FirstOrDefault().OPS_AREA.ops_area1;
                }
                mf.CarSegment = db.CAR_GROUPs.Where(p => p.car_group1 == s[1] && p.CAR_CLASS.CAR_SEGMENT.country == s[0].Substring(0, 2)).FirstOrDefault().CAR_CLASS.CAR_SEGMENT.car_segment1;
                mf.CarClass = db.CAR_GROUPs.Where(p => p.car_group1 == s[1] && p.CAR_CLASS.CAR_SEGMENT.country == s[0].Substring(0, 2)).FirstOrDefault().CAR_CLASS.car_class1;
                mf.CarGroup = s[1];
            }
            return mf;
        }
    }
}