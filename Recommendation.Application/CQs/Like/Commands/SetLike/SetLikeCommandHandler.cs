using MediatR;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.CQs.Like.Queries.GetLikeDb;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Like.Commands.SetLike;

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
            await CreateLike(request.ReviewId, request.UserId, request.IsLike, cancellationToken);
            return Unit.Value;
        }
        like.IsLike = request.IsLike;
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task CreateLike(Guid reviewId, Guid userId,
        bool isLike, CancellationToken cancellationToken)
    {
        var review = await GetReview(reviewId);
        var user = await GetUser(userId);
        var grade = new Domain.Like() { IsLike = isLike, User = user };

        review.Likes.Add(grade);
        _recommendationDbContext.Reviews.Update(review);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task<Domain.Like?> GetLike(Guid userId, Guid reviewId)
    {
        var getLikeQuery = new GetLikeDbQuery(userId, reviewId);
        return await _mediator.Send(getLikeQuery);
    }

    private async Task<UserApp> GetUser(Guid id)
    {
        var getUserDbQuery = new GetUserDbQuery(id);
        return await _mediator.Send(getUserDbQuery);
    }

    private async Task<Domain.Review> GetReview(Guid id)
    {
        var getReviewDbQuery = new GetReviewDbQuery(id);
        var review = await _mediator.Send(getReviewDbQuery);
        await _recommendationDbContext.Entry(review).IncludesAsync(r => r.User);
        
        return review;
    }
}