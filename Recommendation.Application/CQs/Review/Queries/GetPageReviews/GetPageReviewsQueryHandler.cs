using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        var reviewsDtos = await GetPageReviewsDto(countRecordSkip, 
            request.PageSize, cancellationToken);

        return new GetPageReviewsVm()
        {
            TotalCountReviews = reviewCount,
            ReviewDtos = reviewsDtos
        };
    }

    private async Task<IEnumerable<GetPageReviewsDto>> GetPageReviewsDto(int countRecordSkip,
        int pageSize, CancellationToken cancellationToken)
    {
        return await _recommendationDbContext.Reviews
            .Include(r => r.Composition.Ratings)
            .Include(r => r.Likes)
            .OrderBy(r => r.DateCreation)
            .Skip(countRecordSkip)
            .Take(pageSize)
            .ProjectTo<GetPageReviewsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}