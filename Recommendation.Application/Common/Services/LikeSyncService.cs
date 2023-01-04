using Microsoft.EntityFrameworkCore;
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
        var entityEntries = _recommendationDbContext.ChangeTracker
            .Entries<Review>()
            .ToList();

        foreach (var entry in entityEntries)
        {
            await entry.IncludesAsync(e => e.User, e => e.Likes);
            await _recommendationDbContext.Entry(entry.Entity.User).IncludesAsync(u => u.Reviews);
            var likes = entry.Entity.User.Reviews.SelectMany(r => r.Likes);
            switch (entry.State)
            {
                case EntityState.Detached or EntityState.Modified or EntityState.Unchanged:
                    entry.Entity.User.CountLike = likes.Count(l => l.IsLike);
                    break;
                case EntityState.Deleted:
                    entry.Entity.User.CountLike -= likes.Count(l => l.IsLike);
                    break;
            }
        }
    }
}