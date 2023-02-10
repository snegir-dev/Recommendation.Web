using MediatR;

namespace Recommendation.Application.CQs.Review.Queries.GetAllReviewByUserId;

public class GetAllReviewByUserIdQuery : IRequest<IEnumerable<GetAllReviewByUserIdDto>>
{
    public Guid UserId { get; set; }

    public GetAllReviewByUserIdQuery(Guid userId)
    {
        UserId = userId;
    }
}