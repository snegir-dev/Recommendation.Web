using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Application.Common.Clouds.Mega;
using Recommendation.Application.Common.OAuthConfigurations;

namespace Recommendation.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddOAuthConfiguration(configuration);
        services.AddCloudsConfiguration(configuration);
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

    private static void AddCloudsConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var email = configuration["MegaCloud:Email"];
        var password = configuration["MegaCloud:Password"];
        if (email is null || password is null)
            throw new NullReferenceException("Missing mega cloud connection data");

        services.AddScoped<IMegaCloud, MegaCloud>(_ =>
            new MegaCloud(email, password));
    }
}