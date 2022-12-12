using MediatR;

namespace Recommendation.Application.CQs.Comment.Queries.GetComment;

public class GetCommentQuery : IRequest<CommentDto>
{
    public Guid CommentId { get; set; }

    public GetCommentQuery(Guid commentId)
    {
        CommentId = commentId;
    }
}