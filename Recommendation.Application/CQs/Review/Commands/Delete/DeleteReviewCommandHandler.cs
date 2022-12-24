using Algolia.Search.Clients;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Application.Common.Clouds.Firebase;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Commands.Delete;

public class DeleteReviewCommandHandler
    : IRequestHandler<DeleteReviewCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly FirebaseCloud _firebaseCloud;

    public DeleteReviewCommandHandler(IRecommendationDbContext recommendationDbContext, 
        FirebaseCloud firebaseCloud)
    {
        _recommendationDbContext = recommendationDbContext;
        _firebaseCloud = firebaseCloud;
    }

    public async Task<Unit> Handle(DeleteReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _recommendationDbContext.Reviews
                         .Include(r => r.ImageInfo)
                         .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken)
                     ?? throw new NullReferenceException("Review is not found");
        _recommendationDbContext.Reviews.Remove(review);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);
        await _firebaseCloud.DeleteFolder(review.ImageInfo.FolderName);

        return Unit.Value;
    }
}