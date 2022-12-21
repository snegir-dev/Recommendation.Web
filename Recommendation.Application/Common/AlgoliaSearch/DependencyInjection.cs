using Algolia.Search.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Recommendation.Application.Common.AlgoliaSearch;

public static class DependencyInjection
{
    public static void AddAlgoliaSearchClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAlgolia(configuration);
        services.AddScoped<AlgoliaSearchClient>();
        services.AddScoped<EfAlgoliaSync>();
    }

    private static void AddAlgolia(this IServiceCollection services,
        IConfiguration configuration)
    {
        var appId = configuration["AlgoliaConfiguration:ApplicationId"];
        var apiKey = configuration["AlgoliaConfiguration:ApiKey"];
        if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(apiKey))
            throw new NullReferenceException("Missing keys to connect to Algolia");

        services.AddScoped<ISearchClient, SearchClient>(_ => new SearchClient(appId, apiKey));
    }
}