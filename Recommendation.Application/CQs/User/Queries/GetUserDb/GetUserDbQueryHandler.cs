using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Queries.GetUserDb;

public class GetUserDbQueryHandler
    : IRequestHandler<GetUserDbQuery, UserApp>
{
    private readonly IRecommendationDbContext _recommendationDbContext;

    public GetUserDbQueryHandler(IRecommendationDbContext recommendationDbContext)
    {
        _recommendationDbContext = recommendationDbContext;
    }

    public async Task<UserApp> Handle(GetUserDbQuery request, 
        CancellationToken cancellationToken)
    {
        var user = await _recommendationDbContext.Users
                   .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
               ?? throw new NotFoundException(nameof(UserApp), request.UserId);

        return user;
    }
}