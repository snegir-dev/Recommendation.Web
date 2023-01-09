using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Recommendation.Application.Common.Clouds.Firebase;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Command.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly SignInManager<UserApp> _signInManager;
    private readonly IMediator _mediator;
    private readonly IFirebaseCloud _firebaseCloud;

    public DeleteUserCommandHandler(IRecommendationDbContext recommendationDbContext,
        SignInManager<UserApp> signInManager, IMediator mediator, IFirebaseCloud firebaseCloud)
    {
        _recommendationDbContext = recommendationDbContext;
        _signInManager = signInManager;
        _mediator = mediator;
        _firebaseCloud = firebaseCloud;
    }

    public async Task<Unit> Handle(DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await GetUserAsync(request.UserIdToDelete);
        _recommendationDbContext.Users.Remove(user);
        await RemoveReviewsAsync(user.Reviews.AsQueryable());
        if (user.Id == request.CurrentUserId)
            await _signInManager.SignOutAsync();

        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<UserApp> GetUserAsync(Guid userId)
    {
        var getUserDbQuery = new GetUserDbQuery(userId);
        var user = await _mediator.Send(getUserDbQuery);
        await _recommendationDbContext.Entry(user).IncludesAsync(u => u.Reviews);
        
        return user;
    }

    private async Task RemoveReviewsAsync(IQueryable<Domain.Review> reviews)
    {
        var imageInfos = reviews
            .Where(r => r.ImageInfos != null)
            .SelectMany(r => r.ImageInfos!);
        foreach (var imageInfo in imageInfos)
            await _firebaseCloud.DeleteFolderAsync(imageInfo.FolderName);

        _recommendationDbContext.Reviews.RemoveRange(reviews);
    }
}