using Algolia.Search.Clients;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Application.Common.Services;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Persistence.Contexts;

public sealed class RecommendationDbContext
    : IdentityDbContext<UserApp, IdentityRole<Guid>, Guid>, IRecommendationDbContext
{
    public DbSet<UserApp> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Composition> Compositions { get; set; }

    private readonly IServiceProvider _serviceProvider;
    public readonly DbContextOptions<RecommendationDbContext> Options;

    public RecommendationDbContext(DbContextOptions<RecommendationDbContext> options,
        IServiceProvider serviceProvider) : base(options)
    {
        _serviceProvider = serviceProvider;
        Options = options;
        Database.Migrate();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken
        = new())
    {
        await _serviceProvider.GetRequiredService<EfAlgoliaSync>().Sync(cancellationToken);
        await _serviceProvider.GetRequiredService<RecalculationAverageRatingService>().Recalculate();
        return await base.SaveChangesAsync(cancellationToken);
    }
}