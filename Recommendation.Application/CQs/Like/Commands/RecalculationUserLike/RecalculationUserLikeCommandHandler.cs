using MediatR;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Like.Commands.RecalculationUserLike;

public class RecalculationUserLikeCommandHandler
    : IRequestHandler<RecalculationUserLikeCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;

    public RecalculationUserLikeCommandHandler(IRecommendationDbContext recommendationDbContext,
        IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(RecalculationUserLikeCommand request,
        CancellationToken cancellationToken)
    {
        var user = await GetUser(request.UserId);
        var countLike = user.Reviews
            .SelectMany(r => r.Likes)
            .Count(l => l.IsLike);
        user.CountLike = countLike;
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<UserApp> GetUser(Guid id)
    {
        var getUserDbQuery = new GetUserDbQuery(id);
        var user = await _mediator.Send(getUserDbQuery);
        _recommendationDbContext.Entry(user)
            .Includes(u => u.Reviews).Includes(t => t.Likes);

        return user;
    }
}