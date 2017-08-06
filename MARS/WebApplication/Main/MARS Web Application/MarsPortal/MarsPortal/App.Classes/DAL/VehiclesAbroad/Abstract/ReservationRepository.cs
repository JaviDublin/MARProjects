using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using Mars.App.Classes.DAL.MarsDBContext;


namespace DAL.VehiclesAbroad.Abstract {
    public abstract class ReservationRepository<T>:Queryable<T> {
        public IFilterEntity Filters { get; set; }
        public ICarFilterEntity CarFilters { get; set; }
        public string SortExpression { get; set; }
        public abstract IList<T> GetList(IFilterEntity Filters, ICarFilterEntity CarFilters, string SortExpression);
        public abstract IQueryable<T> GetQueryable(MarsDBDataContext db, IFilterEntity Filters, ICarFilterEntity CarFilters, string SortExpression);
    }
}