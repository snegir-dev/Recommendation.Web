using MediatR;

namespace Recommendation.Application.CQs.Comment.Queries.GetAllComment;

public class GetAllCommentQuery : IRequest<IEnumerable<GetAllCommentDto>>
{
    public Guid ReviewId { get; set; }

    public GetAllCommentQuery(Guid reviewId)
    {
        ReviewId = reviewId;
    }
}