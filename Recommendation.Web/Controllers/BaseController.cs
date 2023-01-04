using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Web.Filters;

namespace Recommendation.Web.Controllers;

[ApiController]
[ServiceFilter(typeof(UserActionValidationAttribute))]
public class BaseController : ControllerBase
{
    protected readonly IMapper Mapper;
    protected readonly IMediator Mediator;

    public BaseController(IMapper mapper, IMediator mediator)
    {
        Mapper = mapper;
        Mediator = mediator;
    }

    protected string? UserRole => User.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

    protected Guid UserId => User.Identity!.IsAuthenticated
        ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!)
        : Guid.Empty;
}