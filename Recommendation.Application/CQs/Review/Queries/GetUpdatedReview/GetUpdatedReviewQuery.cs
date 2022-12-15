using MediatR;

namespace Recommendation.Application.CQs.Review.Queries.GetUpdatedReview;

public class GetUpdatedReviewQuery 
    : IRequest<GetUpdatedReviewDto>
{
    public Guid ReviewId { get; set; }

    public GetUpdatedReviewQuery(Guid reviewId)
    {
        ReviewId = reviewId;
    }
}