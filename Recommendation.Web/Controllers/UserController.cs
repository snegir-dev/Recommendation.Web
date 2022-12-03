using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.User.Command.Registration;
using Recommendation.Application.CQs.User.Queries.Login;
using Recommendation.Web.Models.User;

namespace Recommendation.Web.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UserController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegistrationUserDto userDto)
    {
        var registrationUserCommand = _mapper.Map<RegistrationUserCommand>(userDto);
        await _mediator.Send(registrationUserCommand);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginUserDto userDto)
    {
        var registrationUserCommand = _mapper.Map<LoginUserQuery>(userDto);
        await _mediator.Send(registrationUserCommand);

        return Ok();
    }
}