using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Command.Block;

public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly SignInManager<UserApp> _signInManager;
    private readonly IMediator _mediator;

    public BlockUserCommandHandler(IRecommendationDbContext recommendationDbContext,
        SignInManager<UserApp> signInManager, IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _signInManager = signInManager;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(BlockUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await GetUser(request.UserIdToBlock);
        user.AccessStatus = UserAccessStatus.Block;

        if (request.CurrentUserId == user.Id)
            await _signInManager.SignOutAsync();
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<UserApp> GetUser(Guid userId)
    {
        var getUserDbQuery = new GetUserDbQuery(userId);
        return await _mediator.Send(getUserDbQuery);
    }
}