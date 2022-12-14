using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Composition.Queries.GetAverageRate;

public class GetAverageRateQueryHandler
    : IRequestHandler<GetAverageRateQuery, double>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetAverageRateQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<double> Handle(GetAverageRateQuery request,
        CancellationToken cancellationToken)
    {
        var averageRate = await _recommendationDbContext.Compositions
            .Where(c => c.Review.Id == request.ReviewId)
            .SelectMany(c => c.Ratings)
            .Select(c => c.RatingValue)
            .DefaultIfEmpty()
            .AverageAsync(cancellationToken);

        return averageRate;
    }
}