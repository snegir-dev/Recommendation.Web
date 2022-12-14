using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Like.Queries.GetCountLike;

public class GetCountLikeQueryHandler
    : IRequestHandler<GetCountLikeQuery, int>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetCountLikeQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<int> Handle(GetCountLikeQuery request,
        CancellationToken cancellationToken)
    {
        var countLike = await _recommendationDbContext.Likes
            .Include(l => l.Review)
            .Where(l => l.IsLike == true &&
                         l.Review.Id == request.ReviewId)
            .CountAsync(cancellationToken);

        return countLike;
    }
}