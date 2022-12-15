using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.CQs.Rating.Queries.GetRatingDb;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Rating.Commands.SetRating;

public class SetRatingCommandHandler
    : IRequestHandler<SetRatingCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;

    public SetRatingCommandHandler(IRecommendationDbContext recommendationDbContext,
        IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(SetRatingCommand request,
        CancellationToken cancellationToken)
    {
        var rating = await GetRating(request.UserId, request.ReviewId);
        if (rating == null)
        {
            await CreateGrade(request.ReviewId, request.UserId,
                request.GradeValue, cancellationToken);
            return Unit.Value;
        }

        rating.RatingValue = request.GradeValue;
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task CreateGrade(Guid reviewId, Guid userId,
        int gradeValue, CancellationToken cancellationToken)
    {
        var review = await GetReview(reviewId);
        var user = await GetUser(userId);
        var rating = new Domain.Rating()
        {
            RatingValue = gradeValue,
            User = user
        };

        review.Composition.Ratings.Add(rating);
        _recommendationDbContext.Reviews.Update(review);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<UserApp> GetUser(Guid id)
    {
        var userDbQuery = new GetUserDbQuery(id);
        return await _mediator.Send(userDbQuery);
    }

    private async Task<Domain.Rating?> GetRating(Guid userId, Guid reviewId)
    {
        var getRatingDbQuery = new GetRatingDbQuery(userId, reviewId);
        return await _mediator.Send(getRatingDbQuery);
    }

    private async Task<Domain.Review> GetReview(Guid id)
    {
        var getReviewDbQuery = new GetReviewDbQuery(id);
        return await _mediator.Send(getReviewDbQuery);
    }
}