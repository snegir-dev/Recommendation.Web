using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Commands.Delete;

public class DeleteReviewCommandHandler
    : IRequestHandler<DeleteReviewCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public DeleteReviewCommandHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Unit> Handle(DeleteReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _recommendationDbContext.Reviews
                         .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken)
                     ?? throw new NullReferenceException("Review is not found");
        _recommendationDbContext.Reviews.Remove(review);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}