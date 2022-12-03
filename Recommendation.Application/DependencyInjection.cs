using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.ConfigurationModels;
using Recommendation.Domain;

namespace Recommendation.Application;

public static class DependencyInjection
{
    private const string ConnectionStringsSection = "ConnectionStrings";

    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddStringsConnectionConfiguration(configuration);

        return services;
    }

    private static IServiceCollection AddStringsConnectionConfiguration(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringsConfiguration = new ConnectionStringsConfiguration();
        configuration.GetSection(ConnectionStringsSection)
            .Bind(connectionStringsConfiguration);
        services.AddSingleton(connectionStringsConfiguration);

        return services;
    }
}