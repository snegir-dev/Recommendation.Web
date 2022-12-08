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
            ? "Host=dpg-ce9f8ecgqg4bcbg4rbt0-a;Port=5432;Database=recommendation_1tno;Username=snegir;Password=LXpoU1KBVtLwjac8hUKjObv48LRmIvTl"
            : "Host=localhost;Port=5432;Database=recommendation_db;Username=postgres;Password=postqwe";

        return connectionString
               ?? throw new NullReferenceException("The connection string must not be null");
    }
}