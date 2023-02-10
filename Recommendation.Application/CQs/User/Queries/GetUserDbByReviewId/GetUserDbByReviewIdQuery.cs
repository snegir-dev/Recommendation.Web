using MediatR;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Queries.GetUserDbByReviewId;

public class GetUserDbByReviewIdQuery : IRequest<UserApp>
{
    public Guid ReviewId { get; set; }

    public GetUserDbByReviewIdQuery(Guid reviewId)
    {
        ReviewId = reviewId;
    }
}