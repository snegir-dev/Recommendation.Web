using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Clouds.Mega;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.CQs.Tag.Command.Create;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.Review.Commands.Create;

public class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IMegaCloud _megaCloud;

    public CreateReviewCommandHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper, IMediator mediator, IMegaCloud megaCloud)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
        _mediator = mediator;
        _megaCloud = megaCloud;
    }

    public async Task<Guid> Handle(CreateReviewCommand request,
        CancellationToken cancellationToken)
    {
        await CreateMissingHashtags(request.Tags);

        var review = _mapper.Map<Domain.Review>(request);
        review.User = await GetUser(request.UserId, cancellationToken);
        review.Tags = await GetHashtags(request.Tags, cancellationToken);
        review.Category = await GetCategory(request.Category, cancellationToken);
        review.UrlImage = await _megaCloud.UploadFile(request.Image);
        review.Composition = new Composition() { Name = request.NameReview };

        await _recommendationDbContext.Reviews.AddAsync(review, cancellationToken);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return review.Id;
    }

    private async Task<UserApp> GetUser(Guid userId,
        CancellationToken cancellationToken)
    {
        return await _recommendationDbContext.Users
                   .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
               ?? throw new AccessDeniedException("User is not found");
    }

    private async Task<List<Domain.Tag>> GetHashtags(string[] tags,
        CancellationToken cancellationToken)
    {
        var hashtagsList = await _recommendationDbContext.Tags
            .Where(h => tags.Contains(h.Name))
            .ToListAsync(cancellationToken);

        return hashtagsList;
    }

    private async Task<Domain.Category> GetCategory(string category,
        CancellationToken cancellationToken)
    {
        return await _recommendationDbContext.Categories
                   .FirstOrDefaultAsync(c => c.Name == category, cancellationToken)
               ?? throw new NullReferenceException("The category must not be null");
    }

    private async Task CreateMissingHashtags(string[] tags)
    {
        var createHashtagsCommand = new CreateHashtagsCommand()
        {
            Tags = tags
        };
        await _mediator.Send(createHashtagsCommand);
    }
}