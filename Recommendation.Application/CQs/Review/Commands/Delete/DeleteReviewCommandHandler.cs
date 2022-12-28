using Algolia.Search.Clients;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Application.Common.Clouds.Firebase;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.CQs.Like.Commands.RecalculationUserLike;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Commands.Delete;

public class DeleteReviewCommandHandler
    : IRequestHandler<DeleteReviewCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly FirebaseCloud _firebaseCloud;
    private readonly IMediator _mediator;

    public DeleteReviewCommandHandler(IRecommendationDbContext recommendationDbContext,
        FirebaseCloud firebaseCloud, IMediator mediator)
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
        await RecalculationUserLike(review.User.Id);
        if (review.ImageInfos != null && review.ImageInfos.Count > 0)
            await _firebaseCloud.DeleteFolder(review.ImageInfos[0].FolderName);

        return Unit.Value;
    }

    private async Task<Domain.Review> GetReview(Guid reviewId)
    {
        var getReviewDbQuery = new GetReviewDbQuery(reviewId);
        var review = await _mediator.Send(getReviewDbQuery);
        await _recommendationDbContext.Entry(review).IncludesAsync(r => r.ImageInfos!);

        return review;
    }

    private async Task RecalculationUserLike(Guid userId)
    {
        var recalculationUserLikeCommand = new RecalculationUserLikeCommand(userId);
        await _mediator.Send(recalculationUserLikeCommand);
    }
}