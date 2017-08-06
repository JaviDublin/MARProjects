using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.Reservations.Queryables.SortRepository.Abstract;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using System.Data.Linq;
using System.Linq.Expressions;

namespace Mars.DAL.Reservations.Queryables
{
    public class ResDetailsSortQueryable
    {
        ISortRepository _repository;
        public ResDetailsSortQueryable(ISortRepository r) { _repository = r; }
        public IQueryable<Reservation> getQueryable(string sortExpression, string sortDirection, IQueryable<Reservation> q)
        {
            if (string.IsNullOrEmpty(sortExpression)) return q;
            string orderby = sortDirection.Contains("desc") ? "OrderByDescending" : "OrderBy";
            // Compose the expression tree that represents the parameter to the predicate.
            var type = typeof(Reservation);
            var property = type.GetProperty(_repository.getValue(sortExpression));
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), orderby, new Type[] { type, property.PropertyType }, q.Expression, Expression.Quote(orderByExp));
            return q.Provider.CreateQuery<Reservation>(resultExp);
        }
    }
}