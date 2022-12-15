using MediatR;
using Microsoft.AspNetCore.SignalR;
using Recommendation.Application.CQs.Comment.Queries.GetComment;

namespace Recommendation.Application.Hubs;

public class CommentHub : Hub
{
    private readonly IMediator _mediator;

    public CommentHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ConnectGroup(Guid reviewId)
    {
        var connectionId = Context.ConnectionId;
        await Groups.AddToGroupAsync(connectionId, reviewId.ToString());
    }

    public async Task SendComment(Guid reviewId, Guid commentId)
    {
        var connectionId = Context.ConnectionId;
        var getCommentQuery = new GetCommentQuery(commentId);
        var comment = await _mediator.Send(getCommentQuery);

        await Clients.GroupExcept(reviewId.ToString(), connectionId)
            .SendAsync("GetComment", comment);
    }
}