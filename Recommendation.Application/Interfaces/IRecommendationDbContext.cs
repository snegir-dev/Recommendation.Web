namespace Recommendation.Application.Interfaces;

public interface IRecommendationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}