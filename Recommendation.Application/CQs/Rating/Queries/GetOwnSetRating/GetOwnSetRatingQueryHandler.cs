using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Rating.Queries.GetOwnSetRating;

public class GetOwnSetRatingQueryHandler
    : IRequestHandler<GetOwnSetRatingQuery, int>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetOwnSetRatingQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<int> Handle(GetOwnSetRatingQuery request,
        CancellationToken cancellationToken)
    {
        var rating = await _recommendationDbContext.Ratings
                         .Include(r => r.User)
                         .Include(r => r.Composition.Reviews)
                         .Where(r => r.User.Id == request.UserId &&
                                     r.Composition.Reviews.Any(cr => cr.Id == request.ReviewId))
                         .FirstOrDefaultAsync(cancellationToken)
                     ?? new Domain.Rating();
        return rating.RatingValue;
    }
}