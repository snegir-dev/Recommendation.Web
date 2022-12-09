using MediatR;
using Microsoft.AspNetCore.Identity;
using Recommendation.Application.Common.Exceptions;

namespace Recommendation.Application.CQs.User.Queries.Login;

public class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, Unit>
{
    private readonly UserManager<Domain.UserApp> _userManager;
    private readonly SignInManager<Domain.UserApp> _signInManager;

    public LoginUserQueryHandler(UserManager<Domain.UserApp> userManager,
        SignInManager<Domain.UserApp> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Unit> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await CheckUserValidationAsync(request);
        await _signInManager.SignInAsync(user, request.IsRemember);

        return Unit.Value;
    }

    private async Task<Domain.UserApp> CheckUserValidationAsync(LoginUserQuery request)
    {
        var user = await GetUserByEmailAsync(request.Email);
        await CheckUserPasswordAsync(user, request.Password);

        return user;
    }

    private async Task<Domain.UserApp> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new NotFoundException(nameof(User), email);

        return user;
    }

    private async Task CheckUserPasswordAsync(Domain.UserApp userApp, string password)
    {
        var isCorrectPassword = await _userManager.CheckPasswordAsync(userApp, password);
        if (!isCorrectPassword)
            throw new AuthenticationException($"Password for user({userApp.Email}) is not correct");
    }
}