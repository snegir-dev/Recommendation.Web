using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Recommendation.Application.Common.OAuthConfigurations;

public static class DiscordOAuthConfiguration
{
    public static AuthenticationBuilder AddDiscordOAuthConfiguration(
        this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
    {
        authenticationBuilder.AddDiscord(options =>
        {
            options.ClientId = configuration["DiscordConfiguration:ClientId"] ?? string.Empty;
            options.ClientSecret = configuration["DiscordConfiguration:ClientSecret"] ?? string.Empty;
            options.SignInScheme = IdentityConstants.ExternalScheme;
            options.AccessDeniedPath = "/login";
            options.Scope.Add("email");
            options.Scope.Add("identify");
        });

        return authenticationBuilder;
    }
}