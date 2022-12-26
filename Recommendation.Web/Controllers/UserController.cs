using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.CQs.AuthenticationScheme.Queries.GetSpecifiedAuthenticationScheme;
using Recommendation.Application.CQs.User.Command.Block;
using Recommendation.Application.CQs.User.Command.Delete;
using Recommendation.Application.CQs.User.Command.Logout;
using Recommendation.Application.CQs.User.Command.Registration;
using Recommendation.Application.CQs.User.Command.Unblock;
using Recommendation.Application.CQs.User.Queries.ExternalLoginCallback;
using Recommendation.Application.CQs.User.Queries.GetAllUser;
using Recommendation.Application.CQs.User.Queries.GetUserInfo;
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

    [Authorize]
    [HttpGet("get-user-info")]
    public async Task<ActionResult<UserInfoDto>> GetUserInfo()
    {
        var getUserInfoQuery = new GetUserInfoQuery(UserId);
        var userInfo = await Mediator.Send(getUserInfoQuery);

        return Ok(userInfo);
    }

    [Authorize(Roles = Role.Admin)]
    [HttpGet]
    public async Task<ActionResult<GetAllUserVm>> Get()
    {
        var getAllUserQuery = new GetAllUserQuery();
        return await Mediator.Send(getAllUserQuery);
    }

    [HttpGet("get-claims")]
    public ActionResult GetClaims()
    {
        var userClaims = User.Claims
            .Select(x => new { x.Type, x.Value }).ToList();
        return Ok(userClaims);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegistrationUserDto userDto)
    {
        var registrationUserCommand = Mapper.Map<RegistrationUserCommand>(userDto);
        var userId = await Mediator.Send(registrationUserCommand);

        return Created("api/users/register", userId);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserDto userDto)
    {
        var registrationUserCommand = Mapper.Map<LoginUserQuery>(userDto);
        await Mediator.Send(registrationUserCommand);

        return Ok();
    }

    [HttpGet("external-login")]
    public async Task<ActionResult> ExternalLogin(string provider)
    {
        var authenticationPropertiesQuery = new GetSpecifiedAuthenticationSchemeQuery(provider);
        var authenticationProperties = await Mediator.Send(authenticationPropertiesQuery);

        return new ChallengeResult(provider, authenticationProperties);
    }

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

    [Authorize(Roles = Role.Admin)]
    [HttpPost("block/{userId:guid}")]
    public async Task<ActionResult> Block(Guid userId)
    {
        var blockUserCommand = new BlockUserCommand(userId, UserId);
        await Mediator.Send(blockUserCommand);

        return Ok();
    }

    [Authorize(Roles = Role.Admin)]
    [HttpPost("unblock/{userId:guid}")]
    public async Task<ActionResult> Unblock(Guid userId)
    {
        var unblockUserCommand = new UnblockUserCommand(userId);
        await Mediator.Send(unblockUserCommand);

        return Ok();
    }

    [Authorize(Roles = Role.Admin)]
    [HttpDelete("delete/{userId:guid}")]
    public async Task<ActionResult> Delete(Guid userId)
    {
        var deleteUserCommand = new DeleteUserCommand(userId, UserId);
        await Mediator.Send(deleteUserCommand);

        return Ok();
    }
}