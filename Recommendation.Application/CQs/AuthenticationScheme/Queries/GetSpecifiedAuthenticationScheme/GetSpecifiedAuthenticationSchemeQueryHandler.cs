using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Recommendation.Application.CQs.AuthenticationScheme.Queries.GetSpecifiedAuthenticationScheme;

public class GetSpecifiedAuthenticationSchemeQueryHandler
    : IRequestHandler<GetSpecifiedAuthenticationSchemeQuery, AuthenticationProperties>
{
    private readonly SignInManager<Domain.UserApp> _signInManager;

    private const string RedirectUrl = "/login-callback";

    public GetSpecifiedAuthenticationSchemeQueryHandler(SignInManager<Domain.UserApp> signInManager)
    {
        _signInManager = signInManager;
    }

    public Task<AuthenticationProperties> Handle(GetSpecifiedAuthenticationSchemeQuery request,
        CancellationToken cancellationToken)
    {
        var authenticationProperties = _signInManager
            .ConfigureExternalAuthenticationProperties(request.Provider, RedirectUrl);

        return Task.FromResult(authenticationProperties);
    }
}