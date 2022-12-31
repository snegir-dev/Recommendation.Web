using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Review.Queries.GetRelatedReviewById;

public class GetRelatedReviewByIdQueryHandler
    : IRequestHandler<GetRelatedReviewByIdQuery, IEnumerable<GetRelatedReviewByIdDto>>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetRelatedReviewByIdQueryHandler(IRecommendationDbContext recommendationDbContext, 
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetRelatedReviewByIdDto>> Handle(GetRelatedReviewByIdQuery request,
        CancellationToken cancellationToken)
    {
        var reviewNames = await _recommendationDbContext.Reviews
            .Where(r => r.Id == request.ReviewId)
            .Select(r => r.Composition)
            .SelectMany(c => c.Reviews)
            .Where(c => c.Id != request.ReviewId)
            .Distinct()
            .ProjectTo<GetRelatedReviewByIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return reviewNames;
    }
}