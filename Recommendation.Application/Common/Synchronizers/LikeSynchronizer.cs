using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.Common.Synchronizers.Interfaces;
using Recommendation.Application.CQs.User.Queries.GetUserDbByReviewId;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.Common.Synchronizers;

public class LikeSynchronizer : ISynchronizer
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;

    public LikeSynchronizer(IRecommendationDbContext recommendationDbContext, 
        IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _mediator = mediator;
    }

    public async Task Sync()
    {
        await MarkLikesForDeletion();
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

    private Task MarkLikesForDeletion()
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
        var getUserDbByReviewIdQuery = new GetUserDbByReviewIdQuery(reviewId);
        return await _mediator.Send(getUserDbByReviewIdQuery);
    }
}