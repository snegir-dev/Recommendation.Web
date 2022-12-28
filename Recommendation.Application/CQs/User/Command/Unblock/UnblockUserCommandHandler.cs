using MediatR;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Command.Unblock;

public class UnblockUserCommandHandler : IRequestHandler<UnblockUserCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;

    public UnblockUserCommandHandler(IRecommendationDbContext recommendationDbContext,
        IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UnblockUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await GetUser(request.UserIdToUnblock);
        user.AccessStatus = UserAccessStatus.Unblock;
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<UserApp> GetUser(Guid userId)
    {
        var getUserDbQuery = new GetUserDbQuery(userId);
        return await _mediator.Send(getUserDbQuery);
    }
}