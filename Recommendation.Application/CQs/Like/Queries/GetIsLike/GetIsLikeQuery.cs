using MediatR;

namespace Recommendation.Application.CQs.Like.Queries.GetIsLike;

public class GetIsLikeQuery : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }

    public GetIsLikeQuery(Guid userId, Guid reviewId)
    {
        UserId = userId;
        ReviewId = reviewId;
    }
}