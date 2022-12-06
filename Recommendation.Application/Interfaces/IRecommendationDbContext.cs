using Microsoft.EntityFrameworkCore;
using Recommendation.Domain;

namespace Recommendation.Application.Interfaces;

public interface IRecommendationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Review> Reviews { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Hashtag> Hashtags { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}