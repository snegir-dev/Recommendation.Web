using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Recommendation.Web.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IMapper Mapper;
    protected readonly IMediator Mediator;

    public BaseController(IMapper mapper, IMediator mediator)
    {
        Mapper = mapper;
        Mediator = mediator;
    }

    internal Guid UserId => User.Identity!.IsAuthenticated
        ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!)
        : Guid.NewGuid();
}