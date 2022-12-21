using MediatR;

namespace Recommendation.Application.CQs.Like.Queries.GetLikeDb;

public class GetLikeDbQuery : IRequest<Domain.Like?>
{
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }

    public GetLikeDbQuery(Guid userId, Guid reviewId)
    {
        UserId = userId;
        ReviewId = reviewId;
    }
}