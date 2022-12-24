using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Application.Common.Clouds;
using Recommendation.Application.Common.OAuthConfigurations;

namespace Recommendation.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddOAuthConfiguration(configuration);
        services.AddClouds(configuration);
        services.AddAlgoliaSearchClient(configuration);

        return services;
    }

    private static void AddOAuthConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddGoogleOAuthConfiguration(configuration)
            .AddDiscordOAuthConfiguration(configuration);
    }
}