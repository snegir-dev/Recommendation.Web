using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.OAuthConfigurations;
using Recommendation.Application.ConfigurationModels;
using Recommendation.Domain;

namespace Recommendation.Application;

public static class DependencyInjection
{
    private const string ConnectionStringsSection = "ConnectionStrings";
    private const string GoogleConfigurationSection = "GoogleConfiguration";

    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddStringsConnectionConfiguration(configuration);
        services.AddOAuthConfiguration(configuration);

        return services;
    }

    private static void AddStringsConnectionConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionStringsConfiguration = new ConnectionStringsConfiguration();
        configuration.GetSection(ConnectionStringsSection)
            .Bind(connectionStringsConfiguration);
        services.AddSingleton(connectionStringsConfiguration);
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