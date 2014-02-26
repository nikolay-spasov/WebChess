using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace MultiplayerWebChess.Domain.DomainContext
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query,
            params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                    (current, include) => current.Include(include));
            }

            return query;
        }
    }
}
