using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Queries.GetReviewDb;

public class GetReviewDbQueryHandler 
    : IRequestHandler<GetReviewDbQuery, Domain.Review>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetReviewDbQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<Domain.Review> Handle(GetReviewDbQuery request,
        CancellationToken cancellationToken)
    {
        return await _recommendationDbContext.Reviews
                   .Include(r => r.Composition)
                   .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken)
               ?? throw new NullReferenceException($"The review: {request.ReviewId} must not be null");
    }
}