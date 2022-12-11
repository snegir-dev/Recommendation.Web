using MediatR;

namespace Recommendation.Application.CQs.Review.Queries.GetReview;

public class GetReviewQuery : IRequest<GetReviewDto>
{
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }

    public GetReviewQuery(Guid userId, Guid reviewId)
    {
        UserId = userId;
        ReviewId = reviewId;
    }
}