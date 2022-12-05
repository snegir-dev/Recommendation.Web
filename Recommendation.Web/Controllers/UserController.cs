using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.AuthenticationScheme.Queries.GetSpecifiedAuthenticationScheme;
using Recommendation.Application.CQs.User.Command.Registration;
using Recommendation.Application.CQs.User.Queries.ExternalLoginCallback;
using Recommendation.Application.CQs.User.Queries.Login;
using Recommendation.Domain;
using Recommendation.Web.Models.User;

namespace Recommendation.Web.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserController(IMapper mapper, IMediator mediator, SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _mapper = mapper;
        _mediator = mediator;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegistrationUserDto userDto)
    {
        var registrationUserCommand = _mapper.Map<RegistrationUserCommand>(userDto);
        var userId = await _mediator.Send(registrationUserCommand);

        return Created("api/users/register", userId);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserDto userDto)
    {
        var registrationUserCommand = _mapper.Map<LoginUserQuery>(userDto);
        await _mediator.Send(registrationUserCommand);

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("external-login")]
    public async Task<ActionResult> ExternalLogin(string provider)
    {
        var authenticationPropertiesQuery = new GetSpecifiedAuthenticationSchemeQuery(provider);
        var authenticationProperties = await _mediator.Send(authenticationPropertiesQuery);

        return new ChallengeResult(provider, authenticationProperties);
    }

    [AllowAnonymous]
    [HttpGet("external-login-callback")]
    public async Task<ActionResult> ExternalLoginCallback()
    {
        var externalLoginCallbackQuery = new ExternalLoginCallbackQuery();
        await _mediator.Send(externalLoginCallbackQuery);

        return Ok();
    }
}