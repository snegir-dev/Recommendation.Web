using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Recommendation.Domain;

namespace Recommendation.Application.Interfaces;

public interface IRecommendationDbContext
{
    DbSet<UserApp> Users { get; set; }
    DbSet<Review> Reviews { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Tag> Tags { get; set; }
    DbSet<Rating> Ratings { get; set; }
    DbSet<Like> Likes { get; set; }
    DbSet<Comment> Comments { get; set; }
    DbSet<Composition> Compositions { get; set; }

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    ChangeTracker ChangeTracker { get; }
    EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}