using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.CQs.Like.Queries.GetLikeDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Like.Commands;

public class SetLikeCommandHandler
    : IRequestHandler<SetLikeCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;

    public SetLikeCommandHandler(IRecommendationDbContext recommendationDbContext,
        IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(SetLikeCommand request,
        CancellationToken cancellationToken)
    {
        var like = await GetLike(request.UserId, request.ReviewId);
        if (like == null)
        {
            await CreateLike(request.ReviewId, request.UserId,
                request.IsLike, cancellationToken);
            return Unit.Value;
        }

        like.IsLike = request.IsLike;
        like.User.CountLike = request.IsLike ? like.User.CountLike += 1 : like.User.CountLike -= 1;
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<Domain.Like?> GetLike(Guid userId, Guid reviewId)
    {
        var getLikeQuery = new GetLikeDbQuery(userId, reviewId);
        return await _mediator.Send(getLikeQuery);
    }

    private async Task CreateLike(Guid reviewId, Guid userId,
        bool isLike, CancellationToken cancellationToken)
    {
        var review = await GetReview(reviewId, cancellationToken);
        var user = await GetUser(userId, cancellationToken);
        user.CountLike = isLike ? user.CountLike += 1 : user.CountLike -= 1;
        var grade = new Domain.Like()
        {
            IsLike = isLike,
            User = user
        };

        review.Likes.Add(grade);
        _recommendationDbContext.Reviews.Update(review);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<UserApp> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _recommendationDbContext.Users
                       .FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
                   ?? throw new NullReferenceException("The user must not be null");

        return user;
    }

    private async Task<Domain.Review> GetReview(Guid id,
        CancellationToken cancellationToken)
    {
        var review = await _recommendationDbContext.Reviews
                         .FirstOrDefaultAsync(r => r.Id == id, cancellationToken)
                     ?? throw new NullReferenceException("The review must not be null");

        return review;
    }
}