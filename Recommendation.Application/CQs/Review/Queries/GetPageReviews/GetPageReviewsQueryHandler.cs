using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.CQs.Like.Queries.GetCountLike;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Queries.GetPageReviews;

public class GetPageReviewsQueryHandler
    : IRequestHandler<GetPageReviewsQuery, GetPageReviewsVm>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;
    private IMediator _mediator;

    public GetPageReviewsQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper, IMediator mediator)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<GetPageReviewsVm> Handle(GetPageReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var countRecordSkip = request.NumberPage * request.PageSize - request.PageSize;
        var reviewCount = await _recommendationDbContext.Reviews
            .LongCountAsync(cancellationToken);
        if (reviewCount <= 0)
            return new GetPageReviewsVm();

        var reviews = _recommendationDbContext.Reviews.AsQueryable();
        reviews = await Filter(reviews, request.Filter, request.Tag);
        reviews = await GetPageReviewsDto(reviews, countRecordSkip, request.PageSize);

        return new GetPageReviewsVm()
        {
            TotalCountReviews = reviewCount,
            ReviewDtos = reviews.ProjectTo<GetPageReviewsDto>(_mapper.ConfigurationProvider)
        };
    }

    private Task<IQueryable<Domain.Review>> GetPageReviewsDto(IQueryable<Domain.Review> reviews,
        int countRecordSkip, int pageSize)
    {
        reviews = reviews
            .Include(r => r.Composition.Ratings)
            .Include(r => r.Likes)
            .Skip(countRecordSkip)
            .Take(pageSize);

        return Task.FromResult(reviews);
    }

    private Task<IQueryable<Domain.Review>> Filter(IQueryable<Domain.Review> reviews,
        string? filter, string? tag)
    {
        if (tag != null)
            reviews = reviews.Where(r => r.Tags.Any(t => t.Name == tag));
        reviews = filter switch
        {
            Filtration.Date => reviews.OrderByDescending(r => r.DateCreation),
            Filtration.Rating => reviews.OrderByDescending(r =>
                r.Composition.Ratings.Select(rating => rating.RatingValue).DefaultIfEmpty().Average()),
            _ => reviews.OrderByDescending(r => r.DateCreation)
        };

        return Task.FromResult(reviews);
    }
}