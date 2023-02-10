using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualBasic;

namespace Recommendation.Application.Common.Extensions;

public static class EntityEntryExtension
{
    public static async Task<EntityEntry<TEntity>> IncludesAsync<TEntity>(this EntityEntry<TEntity> entry,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : class
    {
        foreach (var include in includes)
        {
            var type = include.Body.Type;
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var modifiedExpression = Expression
                    .Lambda<Func<TEntity, IEnumerable<object>>>(include.Body, include.Parameters);
                await entry.Collection(modifiedExpression).LoadAsync();
            }
            else
            {
                await entry.Reference(include!).LoadAsync();
            }
        }

        return entry;
    }
    
    public static EntityEntry<TEntity> Includes<TEntity>(this EntityEntry<TEntity> entry,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : class
    {
        foreach (var include in includes)
        {
            var type = include.Body.Type;
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var modifiedExpression = Expression
                    .Lambda<Func<TEntity, IEnumerable<object>>>(include.Body, include.Parameters);
                entry.Collection(modifiedExpression).Load();
            }
            else
            {
                entry.Reference(include!).Load();
            }
        }

        return entry;
    }
}