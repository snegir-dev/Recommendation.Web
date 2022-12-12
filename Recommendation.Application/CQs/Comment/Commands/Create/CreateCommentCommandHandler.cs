using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Hubs;
using Recommendation.Application.CQs.Comment.Queries.GetAllComment;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Comment.Commands.Create;

public class CreateCommentCommandHandler
    : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;
    private readonly IHubContext<CommentHub> _hubCommentContext;

    public CreateCommentCommandHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper, IHubContext<CommentHub> hubCommentContext)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
        _hubCommentContext = hubCommentContext;
    }

    public async Task<Guid> Handle(CreateCommentCommand request,
        CancellationToken cancellationToken)
    {
        var comment = _mapper.Map<Domain.Comment>(request);
        comment.User = await GetUser(request.UserId, cancellationToken);
        comment.Review = await GetReview(request.ReviewId, cancellationToken);
        await _recommendationDbContext.Comments.AddAsync(comment, cancellationToken);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }

    private async Task<UserApp> GetUser(Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await _recommendationDbContext.Users
                       .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
                   ?? throw new NullReferenceException("The user must not be null");

        return user;
    }

    private async Task<Domain.Review> GetReview(Guid reviewId,
        CancellationToken cancellationToken)
    {
        var review = await _recommendationDbContext.Reviews
                         .FirstOrDefaultAsync(u => u.Id == reviewId, cancellationToken)
                     ?? throw new NullReferenceException("The review must not be null");

        return review;
    }
}