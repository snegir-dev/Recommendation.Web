using MediatR;

namespace Recommendation.Application.CQs.Review.Queries.GetReviewDb;

public class GetReviewDbQuery : IRequest<Domain.Review>
{
    public Guid ReviewId { get; set; }

    public GetReviewDbQuery(Guid reviewId)
    {
        ReviewId = reviewId;
    }
}