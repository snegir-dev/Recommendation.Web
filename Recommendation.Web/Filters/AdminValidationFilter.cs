using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Common.Exceptions;
using Recommendation.Domain;

namespace Recommendation.Web.Filters;

public class AdminValidationFilter : BaseFilter, IAsyncActionFilter
{
    private readonly UserManager<UserApp> _userManager;

    public AdminValidationFilter(IMediator mediator,
        SignInManager<UserApp> signInManager, UserManager<UserApp> userManager)
        : base(mediator, signInManager)
    {
        _userManager = userManager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
        {
            await next();
            return;
        }
        
        var authorizeAttribute = controllerActionDescriptor.MethodInfo
            .GetCustomAttribute<AuthorizeAttribute>();
        if (authorizeAttribute is { Roles: { } } && authorizeAttribute.Roles.Contains(Role.Admin))
        {
            var role = await GetUserRole(context);
            if (role != Role.Admin)
            {
                await SignInManager.SignOutAsync();
                throw new AccessDeniedException("Not enough rights");
            }
        }

        await next();
    }

    private async Task<string> GetUserRole(ActionContext context)
    {
        var userId = GetUserId(context);
        if (userId == null)
            throw new NotFoundException(nameof(UserApp), userId);
        var user = await GetUser(userId.Value);
        var roles = await _userManager.GetRolesAsync(user);
        return roles.FirstOrDefault() ?? Role.User;
    }
}