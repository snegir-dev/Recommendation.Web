using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.Interfaces;

namespace Recommendation.Application.CQs.User.Command.Registration;

public class RegistrationUserCommandHandler
    : IRequestHandler<RegistrationUserCommand, Guid>
{
    private readonly UserManager<Domain.User> _userManager;
    private readonly SignInManager<Domain.User> _signInManager;
    private readonly IRecommendationDbContext _recommendationDbContext;
    private readonly IMapper _mapper;

    public RegistrationUserCommandHandler(UserManager<Domain.User> userManager,
        SignInManager<Domain.User> signInManager, 
        IRecommendationDbContext recommendationDbContext, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _recommendationDbContext = recommendationDbContext;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(RegistrationUserCommand request,
        CancellationToken cancellationToken)
    {
        var isUserExist = await CheckUserExists(request, cancellationToken);
        if (isUserExist)
            throw new RecordExistsException(typeof(Domain.User));

        var user = _mapper.Map<Domain.User>(request);
        await _userManager.CreateAsync(user, request.Password);
        await _signInManager.SignInAsync(user, request.IsRemember);
        
        return Guid.Parse(user.Id);
    }

    private async Task<bool> CheckUserExists(RegistrationUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _recommendationDbContext.Users
            .AnyAsync(u => u.UserName == request.Login
                           || u.Email == request.Email, cancellationToken);
    }
}