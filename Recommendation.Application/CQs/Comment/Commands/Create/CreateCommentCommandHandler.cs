using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Hubs;
using Recommendation.Application.CQs.Comment.Queries.GetAllComment;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Comment.Commands.Create;

public class CreateCommentCommandHandler
    : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateCommentCommandHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper, IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateCommentCommand request,
        CancellationToken cancellationToken)
    {
        var comment = _mapper.Map<Domain.Comment>(request);
        comment.User = await GetUser(request.UserId);
        comment.Review = await GetReview(request.ReviewId);
        await _recommendationDbContext.Comments.AddAsync(comment, cancellationToken);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }

    private async Task<UserApp> GetUser(Guid userId)
    {
        var getUserDbQuery = new GetUserDbQuery(userId);
        return await _mediator.Send(getUserDbQuery);
    }

    private async Task<Domain.Review> GetReview(Guid reviewId)
    {
        var getReviewDbQuery = new GetReviewDbQuery(reviewId);
        return await _mediator.Send(getReviewDbQuery);
    }
}