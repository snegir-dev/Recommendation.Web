using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Queries.GetUserInfo;

public class GetUserInfoQueryHandler
    : IRequestHandler<GetUserInfoQuery, UserInfoDto>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public GetUserInfoQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<UserInfoDto> Handle(GetUserInfoQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _recommendationDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        if (user == null)
            throw new NotFoundException(nameof(UserApp), request.UserId);
        
        return _mapper.Map<UserApp, UserInfoDto>(user);
    }
}