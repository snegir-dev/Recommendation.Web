using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Domain;
using static System.Guid;

namespace Recommendation.Web.Filters;

public class BaseFilter
{
    protected readonly IMediator Mediator;
    protected readonly SignInManager<UserApp> SignInManager;

    public BaseFilter(IMediator mediator, SignInManager<UserApp> signInManager)
    {
        Mediator = mediator;
        SignInManager = signInManager;
    }
    
    protected async Task<UserApp> GetUser(Guid userId)
    {
        var query = new GetUserDbQuery(userId);
        return await Mediator.Send(query);
    }

    protected Guid? GetUserId(ActionContext context)
    {
        var claimsPrincipal = context.HttpContext.User;
        var currentUserId = claimsPrincipal
            .FindFirstValue(ClaimTypes.NameIdentifier);
        var isParsed = TryParse(currentUserId, out var userId);
        if (!isParsed)
            return null;

        return userId;
    }
}