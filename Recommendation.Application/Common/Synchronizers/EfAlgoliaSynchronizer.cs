using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.Common.Synchronizers.Interfaces;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.Common.Synchronizers;

public class EfAlgoliaSynchronizer : ISynchronizer
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly AlgoliaSearchClient _searchClient;

    public EfAlgoliaSynchronizer(IRecommendationDbContext recommendationDbContext, 
        AlgoliaSearchClient searchClient)
    {
        _recommendationDbContext = recommendationDbContext;
        _searchClient = searchClient;
    }

    public async Task Sync()
    {
        _recommendationDbContext.ChangeTracker.DetectChanges();
        var entityEntries = _recommendationDbContext.ChangeTracker
            .Entries<Review>()
            .ToList();
        
        foreach (var entry in entityEntries)
        {
            await ExecuteAction(entry);
        }
    }

    private async Task ExecuteAction(EntityEntry<Review> entry)
    {
        var entity = entry.Entity;
        await _recommendationDbContext.Entry(entity)
            .IncludesAsync(r => r.Comments, r => r.Tags, r => r.Likes, r => r.Category);
        switch (entry.State)
        {
            case EntityState.Added or EntityState.Modified or EntityState.Unchanged:
                await _searchClient.AddOrUpdateEntity(entity, AlgoliaIndexes.Review);
                break;
            case EntityState.Deleted:
                await _searchClient.DeleteEntity(entity.Id.ToString(), AlgoliaIndexes.Review);
                break;
        }
    }
}