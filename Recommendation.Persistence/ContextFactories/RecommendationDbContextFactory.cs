using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Recommendation.Application.Common.Constants;
using Recommendation.Persistence.Contexts;

namespace Recommendation.Persistence.ContextFactories;

public class RecommendationDbContextFactory
    : IDesignTimeDbContextFactory<RecommendationDbContext>
{
    public RecommendationDbContext CreateDbContext(string[] args)
    {
        var connectionString = GetConnectionString();
        var optionsBuilder = new DbContextOptionsBuilder<RecommendationDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new RecommendationDbContext(optionsBuilder.Options);
    }

    private static string GetConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentConfiguration.AspNetEnvironment)
                               == EnvironmentConfiguration.ProductionType
            ? "Host=dpg-ce6hbco2i3mk2v4ik720-a;Port=5432;Database=recommendation;Username=snegir;Password=UfhdCivsBYD3c3taE7BMTiDm8KQCw8LG"
            : "Host=localhost;Port=5432;Database=recommendation_db;Username=postgres;Password=postqwe";

        return connectionString
               ?? throw new NullReferenceException("The connection string must not be null");
    }
}