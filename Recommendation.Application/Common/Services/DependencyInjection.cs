using Microsoft.Extensions.DependencyInjection;

namespace Recommendation.Application.Common.Services;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<RecalculationAverageRatingService>();
    }
}