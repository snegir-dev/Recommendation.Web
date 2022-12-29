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
        var review = await GetReview(request.ReviewId);
        var user = await GetUser(request.UserId);
        var like = await GetLike(request.UserId, request.ReviewId);
        if (like == null)
        {
            await CreateLike(review, user, request.IsLike, cancellationToken);
            return Unit.Value;
        }

        like.IsLike = request.IsLike;
        review.User.CountLike = request.IsLike ? review.User.CountLike += 1 : review.User.CountLike -= 1;
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<Domain.Like?> GetLike(Guid userId, Guid reviewId)
    {
        var getLikeQuery = new GetLikeDbQuery(userId, reviewId);
        return await _mediator.Send(getLikeQuery);
    }

    private async Task CreateLike(Domain.Review review, UserApp user,
        bool isLike, CancellationToken cancellationToken)
    {
        review.User.CountLike = isLike ? review.User.CountLike+= 1 : review.User.CountLike -= 1;
        var grade = new Domain.Like()
        {
            IsLike = isLike,
            User = user
        };

        review.Likes.Add(grade);
        _recommendationDbContext.Reviews.Update(review);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);
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