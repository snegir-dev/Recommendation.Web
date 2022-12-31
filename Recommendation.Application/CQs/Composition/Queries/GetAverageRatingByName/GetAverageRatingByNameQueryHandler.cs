using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Composition.Queries.GetAverageRatingByName;

public class GetAverageRatingByNameQueryHandler
    : IRequestHandler<GetAverageRatingByNameQuery, double>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetAverageRatingByNameQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<double> Handle(GetAverageRatingByNameQuery request,
        CancellationToken cancellationToken)
    {
        var averageRate = await _recommendationDbContext.Compositions
            .Where(c => c.Name == request.CompositionName)
            .SelectMany(c => c.Ratings)
            .Select(c => c.RatingValue)
            .DefaultIfEmpty()
            .AverageAsync(cancellationToken);

        return averageRate;
    }
}