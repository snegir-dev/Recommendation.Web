using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.AuthenticationScheme.Queries.GetSpecifiedAuthenticationScheme;
using Recommendation.Application.CQs.User.Command.Logout;
using Recommendation.Application.CQs.User.Command.Registration;
using Recommendation.Application.CQs.User.Queries.ExternalLoginCallback;
using Recommendation.Application.CQs.User.Queries.Login;
using Recommendation.Domain;
using Recommendation.Web.Models.User;

namespace Recommendation.Web.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : BaseController
{
    public UserController(IMapper mapper, IMediator mediator)
        : base(mapper, mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegistrationUserDto userDto)
    {
        var registrationUserCommand = Mapper.Map<RegistrationUserCommand>(userDto);
        var userId = await Mediator.Send(registrationUserCommand);

        return Created("api/users/register", userId);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserDto userDto)
    {
        var registrationUserCommand = Mapper.Map<LoginUserQuery>(userDto);
        await Mediator.Send(registrationUserCommand);

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("external-login")]
    public async Task<ActionResult> ExternalLogin(string provider)
    {
        var authenticationPropertiesQuery = new GetSpecifiedAuthenticationSchemeQuery(provider);
        var authenticationProperties = await Mediator.Send(authenticationPropertiesQuery);

        return new ChallengeResult(provider, authenticationProperties);
    }

    [AllowAnonymous]
    [HttpGet("external-login-callback")]
    public async Task<ActionResult> ExternalLoginCallback()
    {
        var externalLoginCallbackQuery = new ExternalLoginCallbackQuery();
        await Mediator.Send(externalLoginCallbackQuery);

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var logoutCommand = new LogoutCommand();
        await Mediator.Send(logoutCommand);

        return Ok();
    }
}