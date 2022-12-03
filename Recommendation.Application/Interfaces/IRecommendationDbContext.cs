using Microsoft.EntityFrameworkCore;
using Recommendation.Domain;

namespace Recommendation.Application.Interfaces;

public interface IRecommendationDbContext
{
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}