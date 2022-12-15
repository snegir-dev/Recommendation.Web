using MediatR;

namespace Recommendation.Application.CQs.Like.Queries.GetCountLike;

public class GetCountLikeQuery : IRequest<int>
{
    public Guid ReviewId { get; set; }

    public GetCountLikeQuery(Guid reviewId)
    {
        ReviewId = reviewId;
    }
}