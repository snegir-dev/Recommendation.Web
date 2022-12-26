using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.Constants;
using Recommendation.Domain;

namespace Recommendation.Persistence.Initializers;

public static class UserDefaultInitializer
{
    public static async Task InitializerAsync(IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<UserApp>>();

        var userModel = GetUserFromConfig(configuration);
        var user = await userManager.FindByEmailAsync(userModel.Email!);
        if (user == null)
        {
            user = userModel;
            await userManager.CreateAsync(user, userModel.PasswordHash!);
        }

        await userManager.AddToRoleAsync(user, Role.Admin);
    }

    private static UserApp GetUserFromConfig(IConfiguration configuration)
    {
        return new UserApp()
        {
            UserName = configuration["DefaultUserAdmin:UserName"],
            Email = configuration["DefaultUserAdmin:Email"],
            PasswordHash = configuration["DefaultUserAdmin:Password"],
            AccessStatus = UserAccessStatus.Unblock
        };
    }
}