using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.Clouds.Firebase;

namespace Recommendation.Application.Common.Clouds;

public static class CloudDependencyInjection
{
    public static void AddClouds(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFirebaseCloud(configuration);
    }

    private static void AddFirebaseCloud(this IServiceCollection services,
        IConfiguration configuration)
    {
        var credentialParameters = configuration
            .GetSection("FirebaseConfiguration").Get<JsonCredentialParameters>();
        var bucket = configuration["FirebaseConfiguration:Bucket"];
        if (credentialParameters is null || string.IsNullOrWhiteSpace(bucket))
            throw new NullReferenceException("Missing config for Firebase storage");

        services.AddScoped<FirebaseCloud>(_ =>
            new FirebaseCloud(credentialParameters, bucket));
    }
}