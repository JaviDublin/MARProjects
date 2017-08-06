using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;


namespace DAL.VehiclesAbroad.Abstract {
    public abstract class Queryable<T> {
        //internal ILog _logger = log4net.LogManager.GetLogger("VehiclesAbroad");
        public abstract IQueryable<T> GetQueryable(MarsDBDataContext db, params string[] s);
        public abstract IQueryable<T> GetQueryable(MarsDBDataContext db, IQueryable<T> q, params string[] s);
    }
}