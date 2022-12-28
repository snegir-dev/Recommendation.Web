using MediatR;

namespace Recommendation.Application.CQs.User.Command.SetUserRole;

public class SetUserRoleCommand : IRequest
{
    public Guid UserId { get; set; }
    public string RoleName { get; set; }

    public SetUserRoleCommand(Guid userId, string roleName)
    {
        UserId = userId;
        RoleName = roleName;
    }
}