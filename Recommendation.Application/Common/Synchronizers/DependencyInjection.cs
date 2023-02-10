using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.Synchronizers.Factory;
using Recommendation.Application.Common.Synchronizers.Interfaces;

namespace Recommendation.Application.Common.Synchronizers;

public static class DependencyInjection
{
    public static void AddSynchronizers(this IServiceCollection services)
    {
        services.AddScoped<ISynchronizer, AverageRatingSynchronizer>();
        services.AddScoped<ISynchronizer, LikeSynchronizer>();
        services.AddScoped<ISynchronizer, EfAlgoliaSynchronizer>();
        services.AddScoped<ISynchronizationFactory, SynchronizationFactory>();
    }
}