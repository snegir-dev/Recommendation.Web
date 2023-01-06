using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Command.SetUserRole;

public class SetUserRoleCommandHandler
    : IRequestHandler<SetUserRoleCommand, Unit>
{
    private readonly UserManager<UserApp> _userManager;

    public SetUserRoleCommandHandler(UserManager<UserApp> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(SetUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await GetUser(_userManager.Users, request.UserId);
        var roles = await GetCurrentUserRoles(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        var identityResult = await _userManager.AddToRoleAsync(user, request.RoleName);
        if (!identityResult.Succeeded)
            throw new InternalServerException(identityResult.Errors);

        return Unit.Value;
    }

    private async Task<IEnumerable<string>> GetCurrentUserRoles(UserApp user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    private async Task<UserApp> GetUser(IQueryable<UserApp> users, Guid userId)
    {
        var user = await users
                       .FirstOrDefaultAsync(u => u.Id == userId)
                   ?? throw new NotFoundException(nameof(UserApp), userId);

        return user;
    }
}