using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.CQs.Category.Queries.GetCategory;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.CQs.Tag.Queries.GetListTagContainsNames;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Commands.Update;

public class UpdateReviewQueryHandler
    : IRequestHandler<UpdateReviewQuery, Unit>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UpdateReviewQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMediator mediator, IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateReviewQuery request,
        CancellationToken cancellationToken)
    {
        var review = await GetReview(request.ReviewId, request.UserId);
        var updatedReview = _mapper.Map(request, review);
        updatedReview.Category = await GetCategory(request.Category);
        updatedReview.Tags = await GetTags(request.Tags);
        updatedReview.Composition.Name = request.NameDescription;

        _recommendationDbContext.Reviews.Update(review);
        await _recommendationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<List<Domain.Tag>> GetTags(string[] tagNames)
    {
        var getListTagDbContainsNamesQuery = new GetListTagDbContainsNamesQuery(tagNames);
        var tags = await _mediator.Send(getListTagDbContainsNamesQuery);

        return tags.ToList();
    }

    private async Task<Domain.Category> GetCategory(string categoryName)
    {
        var getCategoryDbQuery = new GetCategoryDbQuery(categoryName);
        return await _mediator.Send(getCategoryDbQuery);
    }

    private async Task<Domain.Review> GetReview(Guid reviewId, Guid userId)
    {
        var getReviewDbQuery = new GetReviewDbQuery(reviewId);
        var review = await _mediator.Send(getReviewDbQuery);
        await _recommendationDbContext.Entry(review).Reference(r => r.User).LoadAsync();
        await _recommendationDbContext.Entry(review).Reference(r => r.Composition).LoadAsync();
        if (review.User.Id != userId)
            throw new AccessDeniedException("Access denied");

        return review;
    }
}