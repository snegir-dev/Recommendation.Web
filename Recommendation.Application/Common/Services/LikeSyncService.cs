using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.Common.Services;

public class LikeSyncService
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public LikeSyncService(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task Sync()
    {
        _recommendationDbContext.ChangeTracker.DetectChanges();
        _recommendationDbContext.ChangeTracker.CascadeChanges();
        var entityEntries = _recommendationDbContext.ChangeTracker
            .Entries<Like>()
            .ToList();

        await RecalculateUserLikes(entityEntries);
    }

    private async Task RecalculateUserLikes(List<EntityEntry<Like>> entityEntries)
    {
        foreach (var entry in entityEntries)
        {
            await _recommendationDbContext.Entry(entry.Entity.Review)
                .IncludesAsync(r => r.User);

            switch (entry.State)
            {
                case EntityState.Added or EntityState.Modified:
                    entry.Entity.Review.User.CountLike += entry.Entity.IsLike ? 1 : -1;
                    break;
                case EntityState.Deleted:
                    entry.Entity.Review.User.CountLike -= 1;
                    break;
            }
        }
    }
}