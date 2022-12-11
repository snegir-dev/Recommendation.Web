using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Rating.Commands.SetRating;

public class SetRatingCommandHandler
    : IRequestHandler<SetRatingCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public SetRatingCommandHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Unit> Handle(SetRatingCommand request,
        CancellationToken cancellationToken)
    {
        var rating = await GetRatingByUserId(request.UserId, cancellationToken);
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
        var review = await GetReview(reviewId, cancellationToken);
        var user = await GetUser(userId, cancellationToken);
        var rating = new Domain.Rating()
        {
            RatingValue = gradeValue,
            User = user
        };

        review.Composition.Ratings.Add(rating);
        _recommendationDbContext.Reviews.Update(review);
    }

    private async Task<UserApp> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _recommendationDbContext.Users
                       .FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
                   ?? throw new NullReferenceException("The user must not be null");

        return user;
    }

    private async Task<Domain.Rating?> GetRatingByUserId(Guid userId,
        CancellationToken cancellationToken)
    {
        var rating = await _recommendationDbContext.Ratings
            .Include(g => g.User)
            .FirstOrDefaultAsync(g => g.User.Id == userId, cancellationToken);

        return rating;
    }

    private async Task<Domain.Review> GetReview(Guid id,
        CancellationToken cancellationToken)
    {
        var review = await _recommendationDbContext.Reviews
                         .Include(r => r.Composition)
                         .FirstOrDefaultAsync(r => r.Id == id, cancellationToken)
                     ?? throw new NullReferenceException("The review must not be null");

        return review;
    }
}