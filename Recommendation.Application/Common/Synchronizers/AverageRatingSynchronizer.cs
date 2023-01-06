using Recommendation.Application.Common.Synchronizers.Interfaces;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.Common.Synchronizers;

public class AverageRatingSynchronizer : ISynchronizer
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public AverageRatingSynchronizer(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task Sync()
    {
        _recommendationDbContext.ChangeTracker.DetectChanges();
        var entityEntries = _recommendationDbContext.ChangeTracker
            .Entries<Composition>()
            .ToList();

        foreach (var entityEntry in entityEntries)
        {
            await entityEntry.Collection(c => c.Ratings).LoadAsync();
            entityEntry.Entity.AverageRating = RecalculateAverageRate(entityEntry.Entity.Ratings);
        }
    }

    private double RecalculateAverageRate(IEnumerable<Rating> ratings)
    {
        var averageRate = ratings
            .Select(r => r.RatingValue)
            .DefaultIfEmpty()
            .Average();

        return averageRate;
    }
}