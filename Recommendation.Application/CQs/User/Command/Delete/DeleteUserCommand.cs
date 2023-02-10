using MediatR;

namespace Recommendation.Application.CQs.User.Command.Delete;

public class DeleteUserCommand : IRequest
{
    public Guid UserIdToDelete { get; set; }
    public Guid CurrentUserId { get; set; }

    public DeleteUserCommand(Guid userIdToDelete, Guid currentUserId)
    {
        UserIdToDelete = userIdToDelete;
        CurrentUserId = currentUserId;
    }
}