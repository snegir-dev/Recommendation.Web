using MediatR;

namespace Recommendation.Application.CQs.Review.Queries.GetUpdatedReview;

public class GetUpdatedReviewQuery 
    : IRequest<GetUpdatedReviewDto>
{
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }
    public string? Role { get; set; }

    public GetUpdatedReviewQuery(Guid reviewId, Guid userId, string? role)
    {
        ReviewId = reviewId;
        UserId = userId;
        Role = role;
    }
}