using MediatR;
using Microsoft.AspNetCore.Identity;
using Recommendation.Application.Common.Exceptions;

namespace Recommendation.Application.CQs.User.Queries.Login;

public class LoginUserQueryHandler
    : IRequestHandler<LoginUserQuery, Unit>
{
    private readonly UserManager<Domain.User> _userManager;
    private readonly SignInManager<Domain.User> _signInManager;

    public LoginUserQueryHandler(UserManager<Domain.User> userManager,
        SignInManager<Domain.User> signInManager)
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

    private async Task<Domain.User> CheckUserValidationAsync(LoginUserQuery request)
    {
        var user = await GetUserByEmailAsync(request.Email);
        await CheckUserPasswordAsync(user, request.Password);

        return user;
    }

    private async Task<Domain.User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new NotFoundException(nameof(User), email);

        return user;
    }

    private async Task CheckUserPasswordAsync(Domain.User user, string password)
    {
        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!isCorrectPassword)
            throw new AuthenticationException($"Password for user({user.Email}) is not correct");
    }
}