using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Rating.Queries.GetRatingDb;

public class GetRatingDbQueryHandler
    : IRequestHandler<GetRatingDbQuery, Domain.Rating?>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetRatingDbQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Domain.Rating?> Handle(GetRatingDbQuery request,
        CancellationToken cancellationToken)
    {
        return await _recommendationDbContext.Ratings
            .Include(g => g.User)
            .Include(g => g.Composition.Reviews)
            .FirstOrDefaultAsync(g => g.User.Id == request.UserId
                                      && g.Composition.Reviews
                                          .Any(r => r.Id == request.ReviewId), cancellationToken);
    }
}