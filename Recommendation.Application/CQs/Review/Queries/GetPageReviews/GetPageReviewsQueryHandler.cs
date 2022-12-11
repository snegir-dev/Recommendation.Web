using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Queries.GetPageReviews;

public class GetPageReviewsQueryHandler
    : IRequestHandler<GetPageReviewsQuery, GetPageReviewsVm>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetPageReviewsQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<GetPageReviewsVm> Handle(GetPageReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var countRecordSkip = request.NumberPage * request.PageSize - request.PageSize;
        var reviewCount = await _recommendationDbContext.Reviews
            .LongCountAsync(cancellationToken);
        if (reviewCount <= 0)
            return new GetPageReviewsVm();

        var reviewsDtos = await _recommendationDbContext.Reviews
            .OrderBy(r => r.DateCreation)
            .Skip(countRecordSkip)
            .Take(request.PageSize)
            .ProjectTo<GetPageReviewsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetPageReviewsVm()
        {
            TotalCountReviews = reviewCount,
            ReviewDtos = reviewsDtos
        };
    }
}