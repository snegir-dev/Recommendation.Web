using MediatR;

namespace Recommendation.Application.CQs.Review.Queries.GetRelatedReviewById;

public class GetRelatedReviewByIdQuery : IRequest<IEnumerable<GetRelatedReviewByIdDto>>
{
    public Guid ReviewId { get; set; }

    public GetRelatedReviewByIdQuery(Guid reviewId)
    {
        ReviewId = reviewId;
    }
}