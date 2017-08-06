using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;

namespace Mars.DAL.Pooling.Filters
{
    public class TopicRepository : IFilterRepository
    {
        public IList<string> getList(params string[] dependants)
        {
            using (PoolingDataClassesDataContext db = new PoolingDataClassesDataContext())
            {
                var returned =  (from p in db.ResTopics
                                 where !p.Name.Equals("Predelivery")
                                 select p.Name).ToList();

                return returned;
            }
        }

        public static IList<ResTopic> GetResTopicList()
        {
            using (PoolingDataClassesDataContext db = new PoolingDataClassesDataContext())
            {
                var returned = db.ResTopics.ToList();
                return returned;
            }
        }
    }
}