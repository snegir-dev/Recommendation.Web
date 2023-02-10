using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.Tag.Queries.GetAllTags;

public class GetAllTagsQueryHandler
    : IRequestHandler<GetAllTagsQuery, GetAllTagsVm>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetAllTagsQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<GetAllTagsVm> Handle(GetAllTagsQuery request,
        CancellationToken cancellationToken)
    {
        var tags = await _recommendationDbContext.Tags
            .ProjectTo<GetAllTagsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetAllTagsVm() { Tags = tags };
    }
}