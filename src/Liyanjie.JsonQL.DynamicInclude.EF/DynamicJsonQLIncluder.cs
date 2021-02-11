﻿using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicJsonQLIncluder : IJsonQLIncluder
    {
        static readonly MethodInfo includeMethodInfo = typeof(QueryableExtensions)
            .GetTypeInfo()
            .GetDeclaredMethods("Include")
            .Single(mi => mi.GetParameters().Any(pi => typeof(LambdaExpression).IsAssignableFrom(pi.ParameterType)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="navigationPropertyPath"></param>
        /// <returns></returns>
        public IQueryable Include(IQueryable queryable, params string[] navigationPropertyPath)
        {
            if (navigationPropertyPath != null)
                foreach (var path in navigationPropertyPath)
                {
                    var parameter = Expression.Parameter(queryable.ElementType);
                    var propertyOrField = Expression.PropertyOrField(parameter, path);
                    var lambda = Expression.Lambda(propertyOrField, parameter);
                    var expression = Expression.Call(
                        instance: null,
                        method: includeMethodInfo.MakeGenericMethod(queryable.ElementType, propertyOrField.Type),
                        arguments: new[] { queryable.Expression, Expression.Quote(lambda) }
                    );
                    queryable = queryable.Provider.CreateQuery(expression);
                }
            return queryable;
        }
    }
}
