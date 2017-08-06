using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using System.Data.Linq;
using System.Linq.Expressions;
using App.Classes.DAL.Reservations.Queryables.SortRepository.Abstract;

namespace App.Classes.DAL.Reservations.Queryables {
    public class ReservationQueryableSort {
        ISortRepository _repository;
        public ReservationQueryableSort(ISortRepository r) { _repository = r; }
       
        public IQueryable<Mars.App.Classes.DAL.MarsDBContext.Reservations> getQueryable(string sortExpression,
            string sortDirection, DataContext db, IQueryable<Mars.App.Classes.DAL.MarsDBContext.Reservations> q)
        {
            if (string.IsNullOrEmpty(sortExpression)) return q;
            string orderby = sortDirection.Contains("desc") ? "OrderByDescending" : "OrderBy";
            // Compose the expression tree that represents the parameter to the predicate.
            var type = typeof (Mars.App.Classes.DAL.MarsDBContext.Reservations);
            var property = type.GetProperty(_repository.getValue(sortExpression));
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof (Queryable), orderby,
                new Type[] {type, property.PropertyType}, q.Expression, Expression.Quote(orderByExp));
            return q.Provider.CreateQuery<Mars.App.Classes.DAL.MarsDBContext.Reservations>(resultExp);
        }
    }
}