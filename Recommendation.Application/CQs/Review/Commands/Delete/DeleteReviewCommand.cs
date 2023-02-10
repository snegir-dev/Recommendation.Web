using MediatR;

namespace Recommendation.Application.CQs.Review.Commands.Delete;

public class DeleteReviewCommand : IRequest
{
    public Guid ReviewId { get; set; }

    public DeleteReviewCommand(Guid reviewId)
    {
        ReviewId = reviewId;
    }
}