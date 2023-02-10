using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Domain;

namespace Recommendation.Web.Filters;

public class UserActionValidationFilter : BaseFilter, IAsyncActionFilter
{
    public UserActionValidationFilter(IMediator mediator, 
        SignInManager<UserApp> signInManager) : base(mediator, signInManager)
    {
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var userId = GetUserId(context.HttpContext.User);
        if (userId == null)
        {
            await next();
            return;
        }

        await CheckUserStatus(userId.Value);
        
        await next();
    }

    private async Task CheckUserStatus(Guid userId)
    {
        try
        {
            var user = await GetUser(userId);
            if (user.AccessStatus == UserAccessStatus.Block)
            {
                await SignInManager.SignOutAsync();
                throw new AccessDeniedException("User has been blocked");
            }
        }
        catch (NotFoundException e)
        {
            await SignInManager.SignOutAsync();
            throw new AccessDeniedException("User not available");
        }
    }
}