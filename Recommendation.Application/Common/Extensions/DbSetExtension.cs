using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace Recommendation.Application.Common.Extensions;

public static class DbSetExtension
{
    public static DbSet<TEntity> Includes<TEntity>(
        this DbSet<TEntity> entities, params Expression<Func<TEntity, object>>[] includes)
        where TEntity : class
    {
        foreach (var include in includes)
        {
            entities.Include(include);
        }

        return entities;
    }
}