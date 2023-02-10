using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.CQs.Tag.Queries.GetAllTags;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;

namespace Recommendation.Application.CQs.User.Queries.GetAllUser;

public class GetAllUserQueryHandler
    : IRequestHandler<GetAllUserQuery, GetAllUserVm>
{
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<UserApp> _userManager;

    public GetAllUserQueryHandler(IRecommendationDbContext recommendationDbContext,
        IMapper mapper, UserManager<UserApp> userManager)
    {
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<GetAllUserVm> Handle(GetAllUserQuery request,
        CancellationToken cancellationToken)
    {
        var userDtos = new List<GetAllUserDto>();
        var users = await _recommendationDbContext.Users
            .ToListAsync(cancellationToken);
        
        foreach (var user in users)
        {
            var userDto = _mapper.Map<UserApp, GetAllUserDto>(user);
            userDto.Role = await GetRole(user);
            userDtos.Add(userDto);
        }

        return new GetAllUserVm(userDtos);
    }
    
    private async Task<string> GetRole(UserApp user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.FirstOrDefault() ?? Role.User;
    }
}