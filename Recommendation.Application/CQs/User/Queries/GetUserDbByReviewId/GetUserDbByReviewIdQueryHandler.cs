using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Queries.GetUserDbByReviewId;

public class GetUserDbByReviewIdQueryHandler 
    : IRequestHandler<GetUserDbByReviewIdQuery, UserApp>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetUserDbByReviewIdQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<UserApp> Handle(GetUserDbByReviewIdQuery request, 
        CancellationToken cancellationToken)
    {
        var user = await _recommendationDbContext.Users
                       .Include(u => u.Reviews)
                       .FirstOrDefaultAsync(u => u.Reviews
                           .Any(r => r.Id == request.ReviewId), cancellationToken)
                   ?? throw new NotFoundException($"User by review id ({request.ReviewId}) not found");

        return user;
    }
}