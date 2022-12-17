using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.CQs.Tag.Queries.GetAllTags;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.User.Queries.GetAllUser;

public class GetAllUserQueryHandler
    : IRequestHandler<GetAllUserQuery, GetAllUserVm>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetAllUserQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<GetAllUserVm> Handle(GetAllUserQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _recommendationDbContext.Users
            .ProjectTo<GetAllUserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetAllUserVm(users);
    }
}