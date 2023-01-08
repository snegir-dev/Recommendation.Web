using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.Common.Synchronizers.Interfaces;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.Common.Synchronizers;

public class LikeSynchronizer : ISynchronizer
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public LikeSynchronizer(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task Sync()
    {
        await DetectRemoveLikes();
        _recommendationDbContext.ChangeTracker.DetectChanges();
        var likeEntityEntries = _recommendationDbContext.ChangeTracker
            .Entries<Like>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        await RecalculateUserLikes(likeEntityEntries);
    }

    private async Task RecalculateUserLikes(List<EntityEntry<Like>> entityEntries)
    {
        foreach (var entry in entityEntries)
        {
            var user = await GetUserByReviewId(entry.Entity.Review.Id);
            switch (entry.State)
            {
                case EntityState.Added or EntityState.Modified:
                    user.CountLike += entry.Entity.IsLike ? 1 : -1;
                    break;
                case EntityState.Deleted:
                    user.CountLike -= 1;
                    break;
            }
        }
    }

    private Task DetectRemoveLikes()
    {
        var reviewEntityEntries = _recommendationDbContext.ChangeTracker
            .Entries<Review>()
            .Where(e => e.State is EntityState.Deleted)
            .Select(e => e.Includes(r => r.Likes))
            .SelectMany(e => e.Entity.Likes)
            .ToList();
        _recommendationDbContext.Likes.RemoveRange(reviewEntityEntries);

        return Task.CompletedTask;
    }

    private async Task<UserApp> GetUserByReviewId(Guid reviewId)
    {
        var user = await _recommendationDbContext.Users
                       .Include(u => u.Reviews)
                       .FirstOrDefaultAsync(u => u.Reviews.Any(r => r.Id == reviewId))
                   ?? throw new NotFoundException("User not found");

        return user;
    }
}