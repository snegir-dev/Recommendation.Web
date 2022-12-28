using System.Security.Claims;
using Lucene.Net.QueryParsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Application.CQs.User.Queries.GetUserDb;
using Recommendation.Domain;
using static System.Guid;

namespace Recommendation.Web.Filters;

public class UserActionValidationAttribute : Attribute, IAsyncActionFilter
{
    private readonly IMediator _mediator;
    private readonly SignInManager<UserApp> _signInManager;

    public UserActionValidationAttribute(SignInManager<UserApp> signInManager,
        IMediator mediator)
    {
        _signInManager = signInManager;
        _mediator = mediator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var userId = GetUserId(context);
        if (userId == null)
        {
            await next();
            return;
        }

        try
        {
            var user = await GetUser(userId.Value);
            if (user.AccessStatus == UserAccessStatus.Block)
            {
                await _signInManager.SignOutAsync();
                throw new AccessDeniedException("User has been blocked");
            }
        }
        catch (NotFoundException e)
        {
            await next();
        }
        
        await next();
    }

    private async Task<UserApp> GetUser(Guid userId)
    {
        var query = new GetUserDbQuery(userId);
        return await _mediator.Send(query);
    }

    private Guid? GetUserId(ActionContext context)
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