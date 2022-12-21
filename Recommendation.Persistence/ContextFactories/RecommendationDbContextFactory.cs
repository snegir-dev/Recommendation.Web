using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Persistence.Contexts;

namespace Recommendation.Persistence.ContextFactories;

public class RecommendationDbContextFactory
    : IDesignTimeDbContextFactory<RecommendationDbContext>
{
    private const string CurrentAssemblyName = "Recommendation.Persistence";
    private const string MainAssemblyName = "Recommendation.Web";

    public RecommendationDbContext CreateDbContext(string[] args)
    {
        var serviceProvider = CreateServiceProvider();
        var recommendationDbContext = serviceProvider.GetRequiredService<RecommendationDbContext>();

        return new RecommendationDbContext(recommendationDbContext.Options, serviceProvider);
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var configuration = CreateConfiguration();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddPersistence(configuration);
        serviceCollection.AddAlgoliaSearchClient(configuration);

        return serviceCollection.BuildServiceProvider();
    }

    private static IConfiguration CreateConfiguration()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!
            .Replace(CurrentAssemblyName, MainAssemblyName);
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile(Path.Combine(path, "appsettings.json"))
            .AddEnvironmentVariables()
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .AddJsonFile("/etc/secrets/secrets.json", true);

        return builder.Build();
    }
}