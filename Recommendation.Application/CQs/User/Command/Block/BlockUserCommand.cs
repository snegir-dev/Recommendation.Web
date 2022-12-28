using System.Security.Claims;
using MediatR;

namespace Recommendation.Application.CQs.User.Command.Block;

public class BlockUserCommand : IRequest
{
    public Guid UserIdToBlock { get; set; }
    public Guid CurrentUserId { get; set; }

    public BlockUserCommand(Guid userIdToBlock, Guid currentUserId)
    {
        UserIdToBlock = userIdToBlock;
        CurrentUserId = currentUserId;
    }
}