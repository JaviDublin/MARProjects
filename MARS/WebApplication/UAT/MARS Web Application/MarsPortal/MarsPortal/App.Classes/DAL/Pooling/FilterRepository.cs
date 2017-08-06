using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Pooling.Abstract;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;

namespace App.Classes.DAL.Pooling {
    public class FilterRepository : IFilterRepository
    {
        public IList<string> getList(params string[] dependants)
        {
            using (PoolingDataClassesDataContext db = new PoolingDataClassesDataContext())
            {
                var returned = (from p in db.ResTopics 
                                where p.Name.Contains("Reservations") 
                                select p.Name).ToList();

                return returned;
            }
        }
    }
}