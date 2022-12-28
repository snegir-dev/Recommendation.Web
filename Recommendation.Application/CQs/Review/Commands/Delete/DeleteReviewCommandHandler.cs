using Algolia.Search.Clients;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Application.Common.Clouds.Firebase;
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
        await _firebaseCloud.DeleteFolder(review.ImageInfos?[0].FolderName);

        return Unit.Value;
    }

    public async Task<Domain.Review> GetReview(Guid reviewId)
    {
        var getReviewDbQuery = new GetReviewDbQuery(reviewId);
        return await _mediator.Send(getReviewDbQuery);
    }
}