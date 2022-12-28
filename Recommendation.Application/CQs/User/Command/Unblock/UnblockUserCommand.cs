using MediatR;

namespace Recommendation.Application.CQs.User.Command.Unblock;

public class UnblockUserCommand : IRequest
{
    public Guid UserIdToUnblock { get; set; }

    public UnblockUserCommand(Guid userIdToUnblock)
    {
        UserIdToUnblock = userIdToUnblock;
    }
}