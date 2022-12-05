using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Recommendation.Application.Common.OAuthConfigurations;

public static class GoogleOAuthConfiguration
{
    public static AuthenticationBuilder AddGoogleOAuthConfiguration(
        this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
    {
        authenticationBuilder.AddGoogle(options =>
        {
            options.ClientId = configuration["GoogleConfiguration:ClientId"] ?? string.Empty;
            options.ClientSecret = configuration["GoogleConfiguration:ClientSecret"] ?? string.Empty;
            options.SignInScheme = IdentityConstants.ExternalScheme;
        });

        return authenticationBuilder;
    }
}