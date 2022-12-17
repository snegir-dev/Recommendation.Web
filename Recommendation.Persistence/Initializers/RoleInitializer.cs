using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.Constants;

namespace Recommendation.Persistence.Initializers;

public static class RoleInitializer
{
    private static readonly List<string> Roles = new() { Role.User, Role.Admin };

    public static async Task InitializerAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider
            .GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        foreach (var roleName in Roles)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
}