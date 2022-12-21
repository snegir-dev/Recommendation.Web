using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Like.Queries.GetLikeDb;

public class GetLikeDbQueryHandler
    : IRequestHandler<GetLikeDbQuery, Domain.Like?>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetLikeDbQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Domain.Like?> Handle(GetLikeDbQuery request,
        CancellationToken cancellationToken)
    {
        var like = await _recommendationDbContext.Likes
            .Include(g => g.Review)
            .Include(g => g.User)
            .FirstOrDefaultAsync(g => g.User.Id == request.UserId &&
                                      g.Review.Id == request.ReviewId, cancellationToken);

        return like;
    }
}