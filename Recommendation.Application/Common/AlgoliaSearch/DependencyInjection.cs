using Algolia.Search.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Common.Synchronizers;

namespace Recommendation.Application.Common.AlgoliaSearch;

public static class DependencyInjection
{
    public static void AddAlgoliaSearchClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAlgolia(configuration);
        services.AddScoped<IAlgoliaSearchClient, AlgoliaSearchClient>();
    }

    private static void AddAlgolia(this IServiceCollection services,
        IConfiguration configuration)
    {
        var dictionaryConfiguration = GetAlgoliaConfiguration(configuration);
        services.AddScoped<ISearchClient, SearchClient>(_ => 
            new SearchClient(dictionaryConfiguration["appId"], dictionaryConfiguration["apiKey"]));
    }

    private static Dictionary<string, string> GetAlgoliaConfiguration(IConfiguration configuration)
    {
        var section = Environment.GetEnvironmentVariable(EnvironmentConfiguration.AspNetEnvironment)
                      == EnvironmentConfiguration.ProductionType
            ? configuration.GetSection("AlgoliaConfiguration:Production")
            : configuration.GetSection("AlgoliaConfiguration:Develop");
        var appId = section["ApplicationId"];
        var apiKey = section["ApiKey"];
        if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(apiKey))
            throw new NullReferenceException("Missing keys to connect to Algolia");

        return new Dictionary<string, string> { { "appId", appId }, { "apiKey", apiKey } };
    }
}