using MediatR;

namespace Recommendation.Application.CQs.Rating.Queries.GetOwnSetRating;

public class GetOwnSetRatingQuery : IRequest<int>
{
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }

    public GetOwnSetRatingQuery(Guid userId, Guid reviewId)
    {
        UserId = userId;
        ReviewId = reviewId;
    }
}