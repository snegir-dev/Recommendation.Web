using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Rating.Queries.GetAverageRating;

public class GetAverageRatingQueryHandler
    : IRequestHandler<GetAverageRatingQuery, double>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetAverageRatingQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<double> Handle(GetAverageRatingQuery request,
        CancellationToken cancellationToken)
    {
        var ratings = await _recommendationDbContext.Compositions
            .Where(c => c.Id == request.CompositionId)
            .SelectMany(c => c.Ratings)
            .Select(c => c.RatingValue)
            .ToListAsync(cancellationToken);
        if (request.AdditionalRating != null)
            ratings.Add(request.AdditionalRating.Value);
        var averageRating = ratings
            .DefaultIfEmpty()
            .Average();
        
        return averageRating;
    }
}