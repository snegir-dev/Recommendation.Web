using MediatR;
using Microsoft.AspNetCore.Identity;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Command.Logout;

public class LogoutCommandHandler
    : IRequestHandler<LogoutCommand, Unit>
{
    private readonly SignInManager<Domain.UserApp> _signInManager;

    public LogoutCommandHandler(SignInManager<UserApp> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<Unit> Handle(LogoutCommand request, 
        CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
        return Unit.Value;
    }
}