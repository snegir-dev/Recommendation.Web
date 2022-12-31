using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.Common.Services;

public class RecalculationAverageRatingService
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public RecalculationAverageRatingService(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task Recalculate()
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