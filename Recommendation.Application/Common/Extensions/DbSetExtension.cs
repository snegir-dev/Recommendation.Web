using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace Recommendation.Application.Common.Extensions;

public static class DbSetExtension
{
    public static IQueryable<TEntity> Includes<TEntity>(
        this DbSet<TEntity> entities, params Expression<Func<TEntity, object>>[] includes)
        where TEntity : class
    {
        var query = entities.AsQueryable();
        return includes.Aggregate(query, (current, include) => current.Include(include));
    }
}