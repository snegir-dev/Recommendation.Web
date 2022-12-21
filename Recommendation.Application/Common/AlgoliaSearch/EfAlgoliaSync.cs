using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.Common.AlgoliaSearch;

public class EfAlgoliaSync
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly AlgoliaSearchClient _searchClient;

    public EfAlgoliaSync(IRecommendationDbContext recommendationDbContext, 
        AlgoliaSearchClient searchClient)
    {
        _recommendationDbContext = recommendationDbContext;
        _searchClient = searchClient;
    }

    public async Task Sync(CancellationToken cancellationToken)
    {
        _recommendationDbContext.ChangeTracker.DetectChanges();
        var entityEntries = _recommendationDbContext.ChangeTracker
            .Entries<Review>()
            .ToList();
        
        foreach (var entry in entityEntries)
        {
            await ExecuteAction(entry, cancellationToken);
        }
    }

    private async Task ExecuteAction(EntityEntry<Review> entry, 
        CancellationToken cancellationToken)
    {
        var entity = entry.Entity;
        await LoadAsync(entity, cancellationToken);
        switch (entry.State)
        {
            case EntityState.Added or EntityState.Modified or EntityState.Unchanged:
                await _searchClient.AddOrUpdateEntity(entity, entity.Id.ToString());
                break;
            case EntityState.Deleted:
                await _searchClient.DeleteEntity(entity);
                break;
        }
    }

    private async Task LoadAsync(Review review, CancellationToken cancellationToken)
    {
        await _recommendationDbContext.Entry(review)
            .Collection(e => e.Comments).LoadAsync(cancellationToken);
        await _recommendationDbContext.Entry(review)
            .Collection(e => e.Tags).LoadAsync(cancellationToken);;
        await _recommendationDbContext.Entry(review)
            .Collection(e => e.Likes).LoadAsync(cancellationToken);
        await _recommendationDbContext.Entry(review)
            .Reference(e => e.Category).LoadAsync(cancellationToken);
    }
}