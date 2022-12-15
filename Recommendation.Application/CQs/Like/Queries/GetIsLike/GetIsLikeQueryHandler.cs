using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Like.Queries.GetIsLike;

public class GetIsLikeQueryHandler
    : IRequestHandler<GetIsLikeQuery, bool>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetIsLikeQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<bool> Handle(GetIsLikeQuery request,
        CancellationToken cancellationToken)
    {
        var like = await _recommendationDbContext.Likes
                       .Include(g => g.User)
                       .Where(g => g.User.Id == request.UserId &&
                                   g.Review.Id == request.ReviewId)
                       .FirstOrDefaultAsync(cancellationToken)
                   ?? new Domain.Like();
        return like.IsLike;
    }
}