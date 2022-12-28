using Algolia.Search.Models.Search;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.AlgoliaSearch;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Common.Queries;
using Recommendation.Application.CQs.Review.Commands;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Queries.GetPageReviews;

public class GetPageReviewsQueryHandler
    : IRequestHandler<GetPageReviewsQuery, GetPageReviewsVm>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;
    private readonly AlgoliaSearchClient _searchClient;

    public GetPageReviewsQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper, AlgoliaSearchClient searchClient)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
        _searchClient = searchClient;
    }

    public async Task<GetPageReviewsVm> Handle(GetPageReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var countRecordSkip = request.NumberPage * request.PageSize - request.PageSize;
        var reviews = await GetReviews(request.SearchValue);
        if (reviews.Any())
        {
            reviews = await Filter(reviews, request.Filter, request.Tag);
            reviews = await GetPageReviewsDto(reviews, countRecordSkip, request.PageSize);
        }
        var reviewCount = reviews.LongCount();
        
        return new GetPageReviewsVm()
        {
            TotalCountReviews = reviewCount,
            ReviewDtos = reviews.ProjectTo<GetPageReviewsDto>(_mapper.ConfigurationProvider)
        };
    }

    private async Task<IQueryable<Domain.Review>> GetReviews(string? searchValue)
    {
        IQueryable<Domain.Review> reviews;
        if (!string.IsNullOrWhiteSpace(searchValue))
            reviews = await Search(searchValue);
        else
            reviews = _recommendationDbContext.Reviews
                .Include(r => r.ImageInfos)
                .AsQueryable();

        return reviews;
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

    private async Task<IQueryable<Domain.Review>> Search(string? searchValue)
    {
        var reviews = await _searchClient.Search<Domain.Review>(searchValue);
        return reviews.AsQueryable();
    }

    private Task<IOrderedQueryable<Domain.Review>> Filter(IQueryable<Domain.Review> reviews,
        string? filter, string? tag)
    {
        if (tag != null)
            reviews = reviews.Where(r => r.Tags.Any(t => t.Name == tag));
        var sortFunction = FiltrationQuery.Filtration
            .GetValueOrDefault(filter ?? FiltrationType.Default)!;
        var orderedReviews = sortFunction.Invoke(reviews);

        return Task.FromResult(orderedReviews);
    }
}