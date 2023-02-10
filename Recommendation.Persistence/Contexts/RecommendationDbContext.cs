﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.Enams;
using Recommendation.Application.Common.Synchronizers;
using Recommendation.Application.Common.Synchronizers.Interfaces;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;
using Recommendation.Persistence.EntityTypeConfigurations;

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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new CommentConfiguration());
        builder.ApplyConfiguration(new CompositionConfiguration());
        builder.ApplyConfiguration(new ImageInfoConfiguration());
        builder.ApplyConfiguration(new LikeConfiguration());
        builder.ApplyConfiguration(new RatingConfiguration());
        builder.ApplyConfiguration(new ReviewConfiguration());
        builder.ApplyConfiguration(new TagConfiguration());
        builder.ApplyConfiguration(new UserAppConfiguration());

        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken
        = new())
    {
        var synchronizationFactory = _serviceProvider.GetRequiredService<ISynchronizationFactory>();
        await synchronizationFactory.GetInstance(TypeSync.LikeSynchronizer).Sync();
        await synchronizationFactory.GetInstance(TypeSync.AverageRatingSynchronizer).Sync();
        await synchronizationFactory.GetInstance(TypeSync.EfAlgoliaSynchronizer).Sync();
        return await base.SaveChangesAsync(cancellationToken);
    }
}