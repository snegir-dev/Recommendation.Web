using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Recommendation.Application.Common.Exceptions;

namespace Recommendation.Application.CQs.User.Queries.ExternalLoginCallback;

public class ExternalLoginCallbackQueryHandler
    : IRequestHandler<ExternalLoginCallbackQuery, Unit>
{
    private readonly SignInManager<Domain.UserApp> _signInManager;
    private readonly UserManager<Domain.UserApp> _userManager;

    private const bool SaveCookiesAfterExitingBrowser = false;

    public ExternalLoginCallbackQueryHandler(SignInManager<Domain.UserApp> signInManager,
        UserManager<Domain.UserApp> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ExternalLoginCallbackQuery request,
        CancellationToken cancellationToken)
    {
        var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
        var signInResult = await _signInManager.ExternalLoginSignInAsync(loginInfo!.LoginProvider,
            loginInfo.ProviderKey, SaveCookiesAfterExitingBrowser, bypassTwoFactor: true);
        if (signInResult.Succeeded)
            return Unit.Value;

        var email = GetEmailWithExternalLogin(loginInfo);
        var user = await _userManager.FindByEmailAsync(email)
                   ?? await CreateUser(loginInfo);

        await _userManager.AddLoginAsync(user, loginInfo);
        await _signInManager.SignInAsync(user, SaveCookiesAfterExitingBrowser);

        return Unit.Value;
    }

    private async Task<Domain.UserApp> CreateUser(ExternalLoginInfo loginInfo)
    {
        var user = new Domain.UserApp()
        {
            UserName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name),
            Email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email)
        };

        var identityResult = await _userManager.CreateAsync(user);
        if (!identityResult.Succeeded)
            throw new AuthenticationException(identityResult.Errors.ElementAt(0).Description);

        return user;
    }

    private string GetEmailWithExternalLogin(ExternalLoginInfo loginInfo)
    {
        var email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        if (email == null)
            throw new AuthenticationException("Email is null");

        return email;
    }
}