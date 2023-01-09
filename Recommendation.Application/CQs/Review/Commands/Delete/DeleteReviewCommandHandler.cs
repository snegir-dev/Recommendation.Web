using MediatR;
using Recommendation.Application.Common.Clouds.Firebase;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Commands.Delete;

public class DeleteReviewCommandHandler
    : IRequestHandler<DeleteReviewCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IFirebaseCloud _firebaseCloud;
    private readonly IMediator _mediator;

    public DeleteReviewCommandHandler(IRecommendationDbContext recommendationDbContext,
        IFirebaseCloud firebaseCloud, IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _firebaseCloud = firebaseCloud;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(DeleteReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await GetReview(request.ReviewId);
        _recommendationDbContext.Reviews.Remove(review);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);
        if (review.ImageInfos != null && review.ImageInfos.Count > 0)
            await _firebaseCloud.DeleteFolderAsync(review.ImageInfos[0].FolderName);

        return Unit.Value;
    }

    private async Task<Domain.Review> GetReview(Guid reviewId)
    {
        var getReviewDbQuery = new GetReviewDbQuery(reviewId);
        var review = await _mediator.Send(getReviewDbQuery);
        await _recommendationDbContext.Entry(review).IncludesAsync(r => r.ImageInfos!);

        return review;
    }
}