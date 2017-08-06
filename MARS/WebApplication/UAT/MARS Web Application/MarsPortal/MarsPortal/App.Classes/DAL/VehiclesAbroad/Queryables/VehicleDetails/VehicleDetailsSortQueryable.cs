using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using System.Data.Linq;
using System.Linq.Expressions;

namespace App.DAL.VehiclesAbroad.Queryables {
    public class VehicleDetailsSortQueryable {

        public IQueryable<ICarSearchDataEntity> getQueryable(string sortExpression, string sortDirection, DataContext db, IQueryable<ICarSearchDataEntity> q) {
            if (string.IsNullOrEmpty(sortExpression)) return q;
            string orderby = "OrderBy";
            if (sortDirection.Contains("desc")) orderby = "OrderByDescending";
            // Compose the expression tree that represents the parameter to the predicate.
            var type = typeof(ICarSearchDataEntity);
            System.Reflection.PropertyInfo property = type.GetProperty(sortExpression);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), orderby, new Type[] { type, property.PropertyType }, q.Expression, Expression.Quote(orderByExp));
            return q.Provider.CreateQuery<ICarSearchDataEntity>(resultExp);
        }
    }
}