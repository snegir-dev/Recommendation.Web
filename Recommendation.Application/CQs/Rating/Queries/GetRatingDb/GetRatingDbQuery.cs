using MediatR;

namespace Recommendation.Application.CQs.Rating.Queries.GetRatingDb;

public class GetRatingDbQuery : IRequest<Domain.Rating?>
{
    public Guid UserId { get; set; } 
    public Guid ReviewId { get; set; }

    public GetRatingDbQuery(Guid userId, Guid reviewId)
    {
        UserId = userId;
        ReviewId = reviewId;
    }
}