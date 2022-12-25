using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Extensions;
using Recommendation.Application.CQs.Review.Queries.GetReviewDb;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Queries.GetAllReviewByUserId;

public class GetAllReviewByUserIdQueryHandler
    : IRequestHandler<GetAllReviewByUserIdQuery, IEnumerable<GetAllReviewByUserIdDto>>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetAllReviewByUserIdQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAllReviewByUserIdDto>> Handle(GetAllReviewByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = await _recommendationDbContext.Reviews
            .Includes(r => r.User, r => r.ImageInfo, r => r.Composition.Ratings, r => r.Likes)
            .Where(r => r.User.Id == request.UserId)
            .ProjectTo<GetAllReviewByUserIdDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

        return reviews;
    }
}