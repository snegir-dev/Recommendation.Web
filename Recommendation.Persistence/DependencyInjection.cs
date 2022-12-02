using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.ConfigurationModels;
using Recommendation.Application.Interfaces;
using Recommendation.Persistence.Contexts;

namespace Recommendation.Persistence;

public static class DependencyInjection
{
    private const string AspNetEnvironment = "ASPNETCORE_ENVIRONMENT";
    private const string TypeEnvironment = "Production";

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var connectionString = GetConnectionString(serviceProvider);

        services.AddDbContext<RecommendationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IRecommendationDbContext, RecommendationDbContext>();
        return services;
    }


    private static string GetConnectionString(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<ConnectionStringsConfiguration>();
        var connectionString = Environment.GetEnvironmentVariable(AspNetEnvironment)
                               == TypeEnvironment
            ? configuration.RecommendationDbConnectionStringProduction
            : configuration.RecommendationDbConnectionStringDevelop;

        return connectionString;
    }
}